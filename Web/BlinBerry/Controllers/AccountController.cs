﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlinBerry.Services.Common.AccountService;
using BlinBerry.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlinBerry.Controllers
{
    public class AccountController : Controller
    {

        private readonly IAccountService accountService;
        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            this.ViewData["ReturnUrl"] = returnUrl;

            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            this.ViewData["ReturnUrl"] = returnUrl;
            if (this.ModelState.IsValid)
            {
                var result = await this.accountService.Login(model.UserName, model.Password);
                if (result.Succeeded)
                {
                    return this.RedirectToLocal(returnUrl);
                }
                else
                {
                    this.ModelState.AddModelError("LogInFailed", result.Message);
                    return this.View(model);
                }
            }

            return this.View(model);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (this.Url.IsLocalUrl(returnUrl))
            {
                return this.Redirect(returnUrl);
            }
            else
            {
                return this.RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await this.accountService.Logout();
            return this.RedirectToAction(nameof(AccountController.Login), "Account");
        }


        [HttpGet]
        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            this.ViewData["ReturnUrl"] = returnUrl;
            if (this.ModelState.IsValid)
            {
                var result = await this.accountService.Register(model.Name, model.Password, model.PasswordConfirm);
                if (result.Succeeded)
                {
                    return this.RedirectToLocal(returnUrl);
                }
                else
                {
                    this.ModelState.AddModelError("LogInFailed", result.Message);
                    return this.View(model);
                }
            }

            return this.View(model);
        }

    }
}