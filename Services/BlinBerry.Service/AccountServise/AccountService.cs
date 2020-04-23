using BlinBerry.Data.Models.IdentityModels;
using BlinBerry.Services.Common.AccountService;
using BlinBerry.Services.Common.AccountService.Models;
using GlobalContants;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlinBerry.Service.AccountServise
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<OperationResult> Login(string userName, string password)
        {
            var result = new OperationResult()
            {
                Succeeded = false,
                Message = "Не удалось войти"
            };
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                result.Message = "Такого пользователя не существует";
            }
            else
            {
                var signInResult = await signInManager.PasswordSignInAsync(user, password, false, false);
                if (signInResult.Succeeded)
                {
                    result.Succeeded = true;
                }
            }

            return result;
        }

        public async Task Logout()
        {
            await this.signInManager.SignOutAsync();
        }

        public async Task<OperationResult> Register(string userName, string password, string confirm)
        {
            var result = new OperationResult()
            {
                Succeeded = false,
                Message = "Не удалось войти"
            };

            var chekcUser = await userManager.FindByNameAsync(userName);
            if(chekcUser == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = userName
                };
                await userManager.CreateAsync(newUser, password);             
                // установка куки
                await signInManager.SignInAsync(newUser, false);
                result.Succeeded = true;
                result.Message = null;
                return result;               
               
            }
            else
            {
                return result;
            }

        }
    }
}
