﻿using GlobalContants;
using System.Threading.Tasks;

namespace BlinBerry.Services.Common.AccountService
{
    public interface IAccountService
    {
        Task<OperationResult> Login(string userName, string password);

        Task Logout();
    }
}