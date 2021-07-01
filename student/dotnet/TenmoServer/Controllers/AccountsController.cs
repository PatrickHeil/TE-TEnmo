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

        public AccountsController(IAccountDao _accountDao)
        {
            this.accountDao = _accountDao;
        }
        //public AccountsController()
        //{

        //}

        [HttpGet("{userId}")]
        public decimal GetAccountBalance(int userId)
        {
            //get account balance by input userId
            decimal accountBalance = accountDao.GetBalance(userId);
            return accountBalance;
        }

        [HttpPut("{userId}")]
        public void UpdateBalanceSender(int userId, decimal transferredCash)
        {
            accountDao.UpdateBalanceSender(userId, transferredCash);
        }

        [HttpPut("{userId}")]
        public void UpdateBalanceRecipient(int userId, decimal transferredCash)
        {
            accountDao.UpdateBalanceRecipient(userId, transferredCash);
        }
    }

}
