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
        [HttpPost("patient")]
        public IActionResult Patient(PatientViewModel model, string submit)
        {
            lock (lockObject)
            {
                logger.LogInformation($"Usao u post patient thread {Thread.CurrentThread.ManagedThreadId}");
                try
                {
                    PatientBaseData patientBaseData = null;
                    switch (submit)
                    {
                        case "Save":
                            //patientBaseData = repository.GetInstitutionById(model.Id);
                            //if (institution != null) // institucija vec postoji
                            //{
                            //    ViewBag.UserMessage = $"Trazena institucija sa imenom: {institution.Id} vec postoji u bazi!";
                            //}
                            //else
                            //{
                            //    institution = new Institution()
                            //    {
                            //        Id = model.Id,
                            //        InstitutionAddress = model.InstitutionAddress,
                            //        InstitutionHolder = model.InstitutionHolder,
                            //        TimeStamp = DateTime.UtcNow.Ticks
                            //    };
                            //    model.SearchName = "";
                            //    repository.SaveInstitution(institution);
                            //    bool result = repository.SaveAll();
                            //    if (result)
                            //    {
                            //        ViewBag.UserMessage = $"Institucija {institution.Id} uspesno dodata u bazu.";
                            //        model = new InstitutionViewModel();
                            //    }
                            //    else
                            //    {
                            //        ViewBag.UserMessage = $"Institucija {institution.Id} nije uspesno dodata u bazu.";
                            //    }
                            //}
                            break;

                        case "Update": // Prvo moramo proveriti da li je ono sto je inicijalno ucitano preko serch-a
                            //institution = repository.GetInstitutionById(model.Id);
                            //if (institution != null && institution.TimeStamp == model.TimeStamp)
                            //{
                            //    if (institution.Id != model.Id)
                            //    {
                            //        ViewBag.UserMessage = $"Ne moze se promeniti ime {institution.Id}, posto se pokusava promenit ime na {model.Id} ";
                            //        break;
                            //    }
                            //    institution.InstitutionAddress = model.InstitutionAddress;
                            //    institution.InstitutionHolder = model.InstitutionHolder;
                            //    institution.TimeStamp = DateTime.UtcNow.Ticks;
                            //    repository.UpdateInstitution(institution);
                            //    bool result = repository.SaveAll();
                            //    if (result)
                            //    {
                            //        ViewBag.UserMessage = $"Institucija {institution.Id} uspesno izmenjena u bazi.";
                            //    }
                            //    else
                            //    {
                            //        ViewBag.UserMessage = $"Institucija {institution.Id} nije uspesno izmenjena u bazi.";
                            //    }
                            //}
                            //else
                            //{
                            //    ViewBag.UserMessage = $"Institucija {model.Id} koja se zeli promeniti je u medjuvremeu promenjena ili obrisana od strane drugog korisnika. Molim vas ucitajte ponovo instituciju.";
                            //}
                            break;

                        case "Delete":
                            //institution = repository.GetInstitutionById(model.Id);
                            //if (institution != null && institution.TimeStamp == model.TimeStamp)
                            //{
                            //    if (institution.Id != model.Id)
                            //    {
                            //        ViewBag.UserMessage = $"Ne moze se promeniti ime {institution.Id}, posto se pokusava promenit ime na {model.Id} ";
                            //        break;
                            //    }
                            //    repository.DeleteInstitution(institution);
                            //    bool result = repository.SaveAll();
                            //    if (result)
                            //    {
                            //        ViewBag.UserMessage = $"Institucija {institution.Id} uspesno obrisana iz bazi.";
                            //        model = new InstitutionViewModel();
                            //    }
                            //    else
                            //    {
                            //        ViewBag.UserMessage = $"Institucija {institution.Id} nije uspesno obrisana iz bazi.";
                            //    }
                            //}
                            //else
                            //{
                            //    ViewBag.UserMessage = $"Institucija {model.Id} je u medjuvremenu obrisana od strane drugog korisnika.";
                            //}
                            break;

                        case "searchForm":
                            //institution = repository.GetInstitutionById(model.SearchName);
                            //if (institution != null) // pronadjena
                            //{
                            //    model.SearchName = "";
                            //    model.Id = institution.Id;
                            //    model.InstitutionAddress = institution.InstitutionAddress;
                            //    model.InstitutionHolder = institution.InstitutionHolder;
                            //    model.TimeStamp = institution.TimeStamp;
                            //    ViewBag.Enable = "Enabled"; // enabled samo ako je pronadjen iz baze
                            //}
                            //else
                            //{
                            //    ViewBag.CantFind = "Trazena institucija ne postoji u bazi";
                            //}

                            break;
                    }

                    ModelState.Clear(); // ljubim te u dupe!!! Potrebno da se trenutno onemoguci ugradjena validacija nad poljima koja ima bug
                }
                catch (Exception ex)
                {
                    ModelState.Clear();
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

            return model;
        }
    }
}
