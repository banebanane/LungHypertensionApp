using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using LungHypertensionApp.Data;
using LungHypertensionApp.Services;
using LungHypertensionApp.ViewModels;
using LungHypertensionApp.Data.Entities;
using Microsoft.Extensions.Logging;
using System.Threading;
using Microsoft.AspNetCore.Identity;

namespace LungHypertensionApp.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService mailService;
        private readonly ILungHypertensionRepository repository;
        private readonly ILogger<AppController> logger;
        private readonly UserManager<StoreUser> userManager;
        private readonly object lockObject = new object();
        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public AppController(IMailService mailService, ILungHypertensionRepository repository, ILogger<AppController> logger, UserManager<StoreUser> userManager)
        {
            this.mailService = mailService;
            this.repository = repository;
            this.logger = logger;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("contact")]
        [Authorize]
        public IActionResult Contact()
        {
            ViewBag.Tittle = "Contact";
            return View();
        }

        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                mailService.SendMessage(model.Name, model.Email, model.Message);
                ViewBag.UserMessage = "Mail sent";
                ModelState.Clear();
            }
            
            return View();
        }

        [HttpGet("about")]
        public IActionResult About()
        {
            return View();
        }

        //[Authorize]
        //[HttpGet("Patient")]
        //public IActionResult Patient()
        //{
        //    var model = new PatientViewModel()
        //    {

        //    };
        //    model.EnumWHO = new List<string>(4) { "I", "II", "III", "IV" };
        //    model.EnumEKG = new List<string>(4) { "sinusni ritam", "BDG", "atrijalna fibrilacija/flater", "pacemaker" };

        //    return View(model);
        //}

        //[Authorize]
        //[HttpGet("institution")]
        //public IActionResult Institution()
        //{
        //        var model = new InstitutionViewModel()
        //        {
        //            Id = "init ID",
        //            InstitutionHolder = "INIT holder",
        //            InstitutionAddress = "INIT adresa",
        //            SearchName = "Init search"

        //        };

        //    return View(model);
        //}

        [Authorize]
        [HttpGet("userApp")]
        public IActionResult UserApp()
        {
            var model = new UserViewModel();
            model.AllInstitutions = repository.GetAllInstitutions().Select(i => i.Id);

            return View(model);
        }

        [Authorize]
        [HttpPost("userApp")]
        public async Task<IActionResult> UserApp(UserViewModel model, string submit)
        {
            logger.LogInformation($"Usao u post user thread {Thread.CurrentThread.ManagedThreadId}");
            try
            {
                await semaphoreSlim.WaitAsync();
                StoreUser user = null;
                switch (submit)
                {
                    case "Save":                     
                        user = await userManager.FindByEmailAsync(model.Email);
                        if (user != null) // user sa ovim mejlom vec postoji
                        {
                            ViewBag.UserMessage = $"Trazeni korisnik sa email adresom: {user.Email} vec postoji u bazi!";
                        }
                        else
                        {
                            user = new StoreUser()
                            {
                                FirstName = model.FirstName,
                                LastName = model.LastName,
                                Email = model.Email,
                                UserName = model.Username,
                                Titule = model.Titule,
                                InstitutionName = repository.GetInstitutionById(model.InstitutionName),
                            };

                            var result = await userManager.CreateAsync(user, model.Password);

                            model.SearchEmail = "";

                            if (result != IdentityResult.Success)
                            {
                                ViewBag.UserMessage = $"Korisnik sa email adresom: {user.Email} nije uspesno dodat u bazu.";
                                throw new InvalidOperationException("Could not create new user in database");
                            }
                            else // add user role
                            {
                                var result1 = await userManager.AddToRoleAsync(user, model.Role);

                                if (result1 != IdentityResult.Success)
                                {
                                    ViewBag.UserMessage = $"Korisniku sa email adresom: {user.Email} nije uspesno dodata rola u bazu.";
                                    throw new InvalidOperationException($"Could not add {model.Role} role to the user {user.UserName}");
                                }
                            }

                            model = new UserViewModel();
                            ViewBag.UserMessage = $"Korisnik sa email adresom: {user.Email} uspesno dodat u bazu.";                    
                            
                        }
                        model.AllInstitutions = repository.GetAllInstitutions().Select(i => i.Id);
                        break;

                    case "Update":

                        user = await userManager.FindByEmailAsync(model.Email);
                        if (user != null)
                        {
                            user.FirstName = model.FirstName;
                            user.LastName = model.LastName;
                            user.Titule = model.Titule;
                            user.UserName = model.Username; // mozda ne dozvoliti ni promenu userName-a
                            user.InstitutionName = repository.GetInstitutionById(model.InstitutionName);
                            
                            await userManager.UpdateAsync(user);
                            repository.UpdateUserRole(user, model.Role);

                            //posebno za obradu pasworda ako moze,mada to ne bih ni dozvolio
                            var resultSaveAll = repository.SaveAll();              

                            if (resultSaveAll)
                            {
                                model = new UserViewModel();
                                ViewBag.UserMessage = $"Korisnik sa email adresom: {user.Email} uspesno izmenjen u bazi.";
                            }
                            else
                            {
                                ViewBag.UserMessage = $"Korisnik sa email adresom: {user.Email} nije uspesno izmenjen u bazi.";
                            }
                        }
                        else
                        {
                            ViewBag.UserMessage = $"Korisinik {model.Email} koji se zeli promeniti je u medjuvremenu obrisan od strane drugog korisnika. Molim vas ucitajte ponovo korisnika.";
                        }
                        model.AllInstitutions = repository.GetAllInstitutions().Select(i => i.Id);
                        break;

                    case "Delete":
                        user = repository.GetUserByEmail(model.Email);
                        if (user != null)
                        {
                            if (user.Email != model.Email)
                            {
                                ViewBag.UserMessage = $"Ne moze se promeniti email {user.Email}, posto se pokusava promenit email na {model.Email}. ";
                                break;
                            }
                            repository.DeleteUser(user);
                            bool result = repository.SaveAll();
                            if (result)
                            {
                                model = new UserViewModel();
                                ViewBag.UserMessage = $"Korisnik sa Email adresom {user.Email} uspesno obrisan iz baze.";                             
                            }
                            else
                            {
                                ViewBag.UserMessage = $"Korisnik sa Email adresom {user.Email} nije uspesno obrisan iz baze.";
                            }
                        }
                        else
                        {
                            ViewBag.UserMessage = $"Korisnik sa Email adresom {model.Email} je u medjuvremenu obrisan od strane drugog korisnika.";
                        }
                        model.AllInstitutions = repository.GetAllInstitutions().Select(i => i.Id);
                        break;

                    case "searchForm":
                        model.AllInstitutions = repository.GetAllInstitutions().Select(i => i.Id); // moralo je ovako jer imamo fakticki 2 forme
                        user = repository.GetUserByEmail(model.SearchEmail);
                        if (user != null) // pronadjen user
                        {
                            model.FirstName = user.FirstName;
                            model.LastName = user.LastName;
                            model.Email = user.Email;
                            model.Username = user.UserName;
                            model.Titule = user.Titule;
                            model.InstitutionName = user.InstitutionName.Id;
                            ViewBag.Enable = "Enabled"; // enabled samo ako je pronadjen iz baze
                        }
                        else
                        {
                            ViewBag.CantFind = "Trazeni korisnik ne postoji u bazi";
                        }

                        break;
                }

                ModelState.Clear(); // ljubim te u dupe!!! Potrebno da se trenutno onemoguci ugradjena validacija nad poljima koja ima bug
            }
            catch (Exception ex)
            {
                ModelState.Clear();
               // ViewBag.CantFind(ex.Message); bude null
            }
            finally
            {
                model.AllInstitutions = repository.GetAllInstitutions().Select(i => i.Id);
                semaphoreSlim.Release();
            }
            logger.LogInformation($"Zavrsava u post user thread {Thread.CurrentThread.ManagedThreadId}");
            return View(model);
        }

        [Authorize]
        [HttpGet("institution")]
        public IActionResult Institution()
        {
             var model = new InstitutionViewModel()
                {
                    Id = "",
                    InstitutionHolder = "",
                    InstitutionAddress = "",
                    SearchName = ""
                };

            return View(model);
        }

        [Authorize]
        [HttpPost("institution")]
        public IActionResult Institution(InstitutionViewModel model, string submit)
        {
            lock (lockObject)
            {
                logger.LogInformation($"Usao u post institution thread {Thread.CurrentThread.ManagedThreadId}");
                try
                {
                    Institution institution = null;
                    switch (submit)
                    {
                        case "Save":
                            institution = repository.GetInstitutionById(model.Id);
                            if (institution != null) // institucija vec postoji
                            {
                                ViewBag.UserMessage = $"Trazena institucija sa imenom: {institution.Id} vec postoji u bazi!";
                            }
                            else
                            {
                                institution = new Institution()
                                {
                                    Id = model.Id,
                                    InstitutionAddress = model.InstitutionAddress,
                                    InstitutionHolder = model.InstitutionHolder,
                                    TimeStamp = DateTime.UtcNow.Ticks
                                };
                                model.SearchName = "";
                                repository.SaveInstitution(institution);
                                bool result = repository.SaveAll();
                                if (result)
                                {
                                    ViewBag.UserMessage = $"Institucija {institution.Id} uspesno dodata u bazu.";
                                    model = new InstitutionViewModel();
                                }
                                else
                                {
                                    ViewBag.UserMessage = $"Institucija {institution.Id} nije uspesno dodata u bazu.";
                                }
                            }
                            break;

                        case "Update": // Prvo moramo proveriti da li je ono sto je inicijalno ucitano preko serch-a
                            institution = repository.GetInstitutionById(model.Id);
                            if (institution != null && institution.TimeStamp == model.TimeStamp)
                            {
                                if (institution.Id != model.Id)
                                {
                                    ViewBag.UserMessage = $"Ne moze se promeniti ime {institution.Id}, posto se pokusava promenit ime na {model.Id} ";
                                    break;
                                }
                                institution.InstitutionAddress = model.InstitutionAddress;
                                institution.InstitutionHolder = model.InstitutionHolder;
                                institution.TimeStamp = DateTime.UtcNow.Ticks;
                                repository.UpdateInstitution(institution);
                                bool result = repository.SaveAll();
                                if (result)
                                {
                                    ViewBag.UserMessage = $"Institucija {institution.Id} uspesno izmenjena u bazi.";
                                }
                                else
                                {
                                    ViewBag.UserMessage = $"Institucija {institution.Id} nije uspesno izmenjena u bazi.";
                                }
                            }
                            else
                            {
                                ViewBag.UserMessage = $"Institucija {model.Id} koja se zeli promeniti je u medjuvremeu promenjena ili obrisana od strane drugog korisnika. Molim vas ucitajte ponovo instituciju.";
                            }
                            break;

                        case "Delete":
                            institution = repository.GetInstitutionById(model.Id);
                            if (institution != null && institution.TimeStamp == model.TimeStamp)
                            {
                                if (institution.Id != model.Id)
                                {
                                    ViewBag.UserMessage = $"Ne moze se promeniti ime {institution.Id}, posto se pokusava promenit ime na {model.Id} ";
                                    break;
                                }                
                                repository.DeleteInstitution(institution);
                                bool result = repository.SaveAll();
                                if (result)
                                {
                                    ViewBag.UserMessage = $"Institucija {institution.Id} uspesno obrisana iz bazi.";
                                    model = new InstitutionViewModel();
                                }
                                else
                                {
                                    ViewBag.UserMessage = $"Institucija {institution.Id} nije uspesno obrisana iz bazi.";
                                }
                            }
                            else
                            {
                                ViewBag.UserMessage = $"Institucija {model.Id} je u medjuvremenu obrisana od strane drugog korisnika.";
                            }
                            break;

                        case "searchForm":
                            institution = repository.GetInstitutionById(model.SearchName);
                            if (institution != null) // pronadjena
                            {
                                model.SearchName = "";
                                model.Id = institution.Id;
                                model.InstitutionAddress = institution.InstitutionAddress;
                                model.InstitutionHolder = institution.InstitutionHolder;
                                model.TimeStamp = institution.TimeStamp;
                                ViewBag.Enable = "Enabled"; // enabled samo ako je pronadjen iz baze
                            }
                            else
                            {
                                ViewBag.CantFind = "Trazena institucija ne postoji u bazi";
                            }

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

        //[Authorize]
        //[HttpPost("searchForm")]
        //public IActionResult SearchForm(InstitutionViewModel model)
        //{
        //    var vm = new ReportViewModel();
        //    vm.Name = "SuperManReport";
        //    return View(vm);
        //    model.InstitutionHolder = "dsds";
        //    model.InstitutionAddress = "fdfdfdfd";
        //    model.Id = "id";
        //    model.SearchName = "";
        //    return View("Institution", model);
        //    return View(model);
        //}

        //[Authorize]
        //[HttpPost("patient")]
        //public IActionResult Patient(PatientViewModel model)
        //{
        //    //if (ModelState.IsValid)
        //    //{
        //    //    mailService.SendMessage(model.Name, model.Email, model.Message);
        //    //    ViewBag.UserMessage = "Mail sent";
        //    //    ModelState.Clear();
        //    //}

        //    return View();
        //}
    }
}
