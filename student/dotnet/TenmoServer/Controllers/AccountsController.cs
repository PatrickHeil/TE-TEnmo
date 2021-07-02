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
    [Route("[controller]")]
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
        public List<Account> GetAccounts()
        {
            List<Account> accounts = accountDao.GetAccounts();
            return accounts;
        }

        [HttpGet("{userId}")]
        public Account GetAccountByUserId(int userId)
        {
            Account account = accountDao.GetAccount(userId);
            return account;
        }

        [HttpGet("{accountId}")]
        public Account GetAccountByAccountId(int accountId)
        {
            Account account = accountDao.GetAccount(accountId);
            return account;
        }


        [HttpPut("{accountId}")]
        public void UpdateAccount(Account account)
        {
            transferDao.UpdateBalanceSender(account);
        }

        //public AccountsController()
        //{

        //}

        //[HttpGet("{userId}")]
        //public decimal GetAccountBalance(int userId)
        //{
        //    //get account balance by input userId
        //    decimal accountBalance = accountDao.GetBalance(userId);
        //    return accountBalance;
        //}
    }

}
