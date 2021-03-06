﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LungHypertensionApp.Data.Entities;
using LungHypertensionApp.ViewModels;

namespace LungHypertensionApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> logger;
        private readonly SignInManager<StoreUser> signInManager;

        public AccountController(ILogger<AccountController> logger, SignInManager<StoreUser> signInManager)
        {
            this.logger = logger;
            this.signInManager = signInManager;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "App");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Username, model.Password,
                    model.RememberMe, false); // ovde izmeniti ako hocemo da lockujemo accout

                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }
                    else
                    {
                        return RedirectToAction("Index", "App"); // setovati nesto smisleno, recimo pacijente da vidi
                    }
                }

            }

            ModelState.AddModelError("", "Failed to login");
           

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "App");
        }
    }
}
