using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LungHypertensionApp.Data;
using LungHypertensionApp.Services;
using LungHypertensionApp.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using LungHypertensionApp.Data.Entities;
using Microsoft.Extensions.Logging;
using System.Threading;
using Microsoft.AspNetCore.Identity;
namespace LungHypertensionApp.Controllers
{
    public class PatientController : Controller
    {
        private readonly ILungHypertensionRepository repository;
        private readonly ILogger<AppController> logger;
        private readonly UserManager<StoreUser> userManager;
        private readonly object lockObject = new object();
        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public PatientController(ILungHypertensionRepository repository, ILogger<AppController> logger, UserManager<StoreUser> userManager)
        {
            this.repository = repository;
            this.logger = logger;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpGet("patient")]
        public IActionResult Patient()
        {
            var model = new PatientViewModel();
            model = PopulateEnums(model);

            return View(model);
        }

        [Authorize]
        [HttpGet("/Patient/Params")]
        public IActionResult Params()
        {
            Request.Query.ContainsKey("parameter1");
          //   Request.QueryString.HasValue["parameter1"];
            return View();
        }

        [Authorize]
        public IActionResult UpdateControll(int idPatient, int idControll)
        {
            // provera timestamp-a ako neki user drugi promeni 
            PatientControllViewModel model = new PatientControllViewModel()
            {
                PatientId = idPatient,
                Id = idControll
            };
            //   Request.QueryString.HasValue["parameter1"];
            return RedirectToAction("PatientControll", model);
        }
        

        [Authorize]
        [HttpGet("patientControll")]
        public IActionResult PatientControll(PatientControllViewModel model)
        {
            if (model.PatientId > 0)
            {
                model.PatientControlls = repository.GetAllControllsForPatient(model.PatientId);
            }
            if (model.PatientControlls == null)
            {
                model.PatientControlls = new List<PatientControll>();
            }
            return View(model);
        }


        [Authorize]
        [HttpPost("patientControll")]
        public IActionResult PatientControll(PatientControllViewModel model, string submit)
        {
            lock (lockObject)
            {
                logger.LogInformation($"Usao u post patient controll thread {Thread.CurrentThread.ManagedThreadId}");
                try
                {
                    PatientControll patientControll = null;

                    switch (submit)
                    {
                        case "Save":
                            patientControll = new PatientControll()
                            {
                                //         Id = model.Id, automatski popunjava, jer smo rekli da je ovo kljuc. Inace puca SQL exception
                                
                                ControllDate = model.ControllDate,
                                Patient = repository.GetPatientById(model.PatientId),
                                TimeStamp = DateTime.UtcNow.Ticks,
                                WeekHearth = model.WeekHearth
                            };

                            repository.SavePatientControll(patientControll);
                            bool result = repository.SaveAll();
                            if (result)
                            {
                                ViewBag.UserMessage = $"Kontrola sa ID-jem: {patientControll.Id} je uspesno dodata u bazu.";
                                model = new PatientControllViewModel()
                                {
                                    PatientId = model.PatientId
                                };
                            }
                            else
                            {
                                ViewBag.UserMessage = $"Kontrola sa ID-jem: {patientControll.Id} nije uspesno dodata u bazu.";
                            }

                            break;

                        case "Update": // Prvo moramo proveriti da li je ono sto je inicijalno ucitano preko serch-a
                            //patient = repository.GetPatientById(model.Id);
                            //if (patient != null && patient.TimeStamp == model.TimeStamp)
                            //{
                            //    if (patient.Id != model.Id)
                            //    {
                            //        ViewBag.UserMessage = $"Ne moze se promeniti ID: {patient.Id} na {model.Id}, jer je to jedinstveni identifikator pacijenta.";
                            //        PopulateEnums(model);
                            //        break;
                            //    }
                            //    patient.Id = model.Id;
                            //    patient.FirstName = model.FirstName;
                            //    patient.LastName = model.LastName;
                            //    patient.Institution = repository.GetInstitutionById(model.InstitutionName);
                            //    patient.Address = model.Address;
                            //    patient.Telephone = model.Telephone;
                            //    patient.Mobile = model.Mobile;
                            //    patient.Email = model.Email;
                            //    patient.EKG = model.EKG;
                            //    patient.Risk = model.Risk;
                            //    patient.WHO = model.WHO;
                            //    patient.NtProBnp = model.NtProBNP;
                            //    patient.Hgb = model.Hgb;
                            //    patient.Hct = model.Hct;
                            //    patient.TimeStamp = DateTime.UtcNow.Ticks;
                            //    repository.UpdatePatient(patient);
                            //    bool resultSave = repository.SaveAll();
                            //    if (resultSave)
                            //    {
                            //        ViewBag.UserMessage = $"Pacijent sa ID-jem: {patient.Id} uspesno izmenjen u bazi.";
                            //    }
                            //    else
                            //    {
                            //        ViewBag.UserMessage = $"Pacijent sa ID-jem: {patient.Id} nije uspesno izmenjen u bazi.";
                            //    }
                            //}
                            //else
                            //{
                            //    ViewBag.UserMessage = $"Pacijent sa ID-jem: {model.Id} koji se zeli promeniti je u medjuvremeu promenjen ili obrisan od strane drugog korisnika. Molim vas ucitajte ponovo pacijenta.";
                            //}
                            //PopulateEnums(model);
                            break;
                    }

                    ModelState.Clear(); // ljubim te u dupe!!! Potrebno da se trenutno onemoguci ugradjena validacija nad poljima koja ima bug
                }
                catch (Exception ex)
                {
                    ModelState.Clear();
                }
                logger.LogInformation($"Zavrsava u post patient controll thread {Thread.CurrentThread.ManagedThreadId}");
                return View(model);
            }
        }

        [Authorize]
        [HttpPost("patient")]
        public IActionResult Patient(PatientViewModel model, string submit)
        {
            lock (lockObject)
            {
                logger.LogInformation($"Usao u post patient thread {Thread.CurrentThread.ManagedThreadId}");
                try
                {
                    Patient patient = null;

                    switch (submit)
                    {
                        case "Save":
                            model.Id = repository.GetPatientMaxId() + 1;
                            if (model.Id == 0) // ne bi trebalo nikada da se desio, osim kada je baza nedostupna
                            {
                                ViewBag.UserMessage = $"Problem sa pristupom bazi. Molim vas kontaktirajte korisnicku podrsku.";
                                break;
                            }
                            patient = new Patient()
                            {
                       //         Id = model.Id, automatski popunjava, jer smo rekli da je ovo kljuc. Inace puca SQL exception
                                FirstName = model.FirstName,
                                LastName = model.LastName,
                                Institution = repository.GetInstitutionById(model.InstitutionName),
                                TimeStamp = DateTime.UtcNow.Ticks,
                                Address = model.Address,
                                Telephone = model.Telephone,
                                Mobile = model.Mobile,
                                Email = model.Email,
                                EKG = model.EKG,
                                Risk = model.Risk,
                                WHO = model.WHO,
                                NtProBnp = model.NtProBNP,
                                Hgb = model.Hgb,
                                Hct = model.Hct
                            };

                            model.SearchIdPatient = 0;
                            repository.SavePatient(patient);
                            bool result = repository.SaveAll();
                            if (result)
                            {
                                ViewBag.UserMessage = $"Pacijent sa ID-jem: {patient.Id} uspesno dodat u bazu.";
                                model = new PatientViewModel();
                            }
                            else
                            {
                                ViewBag.UserMessage = $"Pacijent sa ID-jem: {patient.Id} nije uspesno dodat u bazu.";
                            }
                            
                            PopulateEnums(model);

                            break;

                        case "Update": // Prvo moramo proveriti da li je ono sto je inicijalno ucitano preko serch-a
                            patient = repository.GetPatientById(model.Id);
                            if (patient != null && patient.TimeStamp == model.TimeStamp)
                            {
                                if (patient.Id != model.Id)
                                {
                                    ViewBag.UserMessage = $"Ne moze se promeniti ID: {patient.Id} na {model.Id}, jer je to jedinstveni identifikator pacijenta.";
                                    PopulateEnums(model);
                                    break;
                                }
                                patient.Id = model.Id;
                                patient.FirstName = model.FirstName;
                                patient.LastName = model.LastName;
                                patient.Institution = repository.GetInstitutionById(model.InstitutionName);
                                patient.Address = model.Address;
                                patient.Telephone = model.Telephone;
                                patient.Mobile = model.Mobile;
                                patient.Email = model.Email;
                                patient.EKG = model.EKG;
                                patient.Risk = model.Risk;
                                patient.WHO = model.WHO;
                                patient.NtProBnp = model.NtProBNP;
                                patient.Hgb = model.Hgb;
                                patient.Hct = model.Hct;
                                patient.TimeStamp = DateTime.UtcNow.Ticks;
                                repository.UpdatePatient(patient);
                                bool resultSave = repository.SaveAll();
                                if (resultSave)
                                {
                                    ViewBag.UserMessage = $"Pacijent sa ID-jem: {patient.Id} uspesno izmenjen u bazi.";
                                }
                                else
                                {
                                    ViewBag.UserMessage = $"Pacijent sa ID-jem: {patient.Id} nije uspesno izmenjen u bazi.";
                                }
                            }
                            else
                            {
                                ViewBag.UserMessage = $"Pacijent sa ID-jem: {model.Id} koji se zeli promeniti je u medjuvremeu promenjen ili obrisan od strane drugog korisnika. Molim vas ucitajte ponovo pacijenta.";
                            }
                            PopulateEnums(model);
                            break;

                        case "Delete":
                            patient = repository.GetPatientById(model.Id);
                            if (patient != null && patient.TimeStamp == model.TimeStamp)
                            {
                                if (patient.Id != model.Id)
                                {
                                    ViewBag.UserMessage = $"Ne moze se promeniti ID: {patient.Id} na {model.Id}, jer je to jedinstveni identifikator pacijenta.";
                                    PopulateEnums(model);
                                    break;
                                }
                                repository.DeletePatient(patient);
                                bool resultDelete = repository.SaveAll();
                                if (resultDelete)
                                {
                                    ViewBag.UserMessage = $"Pacijenta sa ID-jem: {patient.Id} uspesno obrisan iz bazi.";
                                    model = new PatientViewModel();
                                }
                                else
                                {
                                    ViewBag.UserMessage = $"Pacijent sa ID-jem: {patient.Id} nije uspesno obrisan iz bazi.";
                                }
                            }
                            else
                            {
                                ViewBag.UserMessage = $"Pacijent sa ID-jem: {model.Id} je u medjuvremenu obrisan od strane drugog korisnika.";
                            }
                            PopulateEnums(model);
                            break;

                        case "searchForm":
                            patient = repository.GetPatientById(model.SearchIdPatient);
                            if (patient != null) // pronadjen
                            {
                                model.Id = patient.Id;
                                model.TimeStamp = patient.TimeStamp;
                                model.FirstName = patient.FirstName;
                                model.LastName = patient.LastName;
                                model.InstitutionName = patient.Institution.Id;
                                model.Address = patient.Address;
                                model.Telephone = patient.Telephone;
                                model.Mobile = patient.Mobile;
                                model.Email = patient.Email;
                                model.EKG = patient.EKG;
                                model.Risk = patient.Risk;
                                model.WHO = patient.WHO;
                                model.NtProBNP = patient.NtProBnp;
                                model.Hgb = patient.Hgb;
                                model.Hct = patient.Hct;
                                ViewBag.Enable = "Enabled"; // enabled samo ako je pronadjen iz baze
                            }
                            else
                            {
                                ViewBag.CantFind = "Trazeni pacijent ne postoji u bazi";
                            }
                            PopulateEnums(model);

                            break;
                        case "Controls":
                            return RedirectToAction("PatientControll", new PatientControllViewModel() { PatientId = model.Id});
                    }

                    ModelState.Clear(); // ljubim te u dupe!!! Potrebno da se trenutno onemoguci ugradjena validacija nad poljima koja ima bug
                }
                catch (Exception ex)
                {
                    ModelState.Clear();
                    PopulateEnums(model);
                    //     ViewBag.CantFind(ex.Message); bude null
                }
                logger.LogInformation($"Zavrsava u post institution thread {Thread.CurrentThread.ManagedThreadId}");
                return View(model);
            }
        }

        private PatientViewModel PopulateEnums(PatientViewModel model)
        {
            model.EnumWHO = new List<string>(4) { "I", "II", "III", "IV" };
            model.EnumEKG = new List<string>(4) { "sinusni ritam", "BDG", "atrijalna fibrilacija/flater", "pacemaker" };
            model.EnumRisk = new List<string>(3) { "nizak", "umeren", "visok" };
            model.AllInstitutions = repository.GetAllInstitutions().Select(i => i.Id);

            return model;
        }
    }
}
