using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("/")]
    [ApiController]
    public class AccountsController : Controller
    {
        private IAccountDao accountDao;
        private readonly ITransferDao transferDao;
        
        public AccountsController(IAccountDao _accountDao, ITransferDao _transferDao)
        {
            this.accountDao = _accountDao;
            this.transferDao = _transferDao;
        }

        [HttpGet]
        public List<Account> GetAccounts() // display all accounts
        {
            List<Account> accounts = accountDao.GetAccounts();
            return accounts;
        }

        [HttpGet("accounts/{userId}")]
        public Account GetAccountByUserId(int userId) // display one account based on userId
        {
            Account account = accountDao.GetAccount(userId);
            return account;
        }

        [HttpGet("accounts/account{accountId}")]
        public Account GetAccountByAccountId(int accountId) // display one account based on accountId
        {
            Account account = accountDao.GetAccount(accountId);
            return account;
        }


        [HttpPut("accounts/{accountId}")]
        public void UpdateAccount(Transfer transfer) // updates sender and recipient accounts based on the transfer (line 119 in Api Services, client side)
        {
            transferDao.UpdateBalances(transfer);
        }
    }
}
