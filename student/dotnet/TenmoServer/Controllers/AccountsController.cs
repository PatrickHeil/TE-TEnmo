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

        public AccountsController()
        {

        }

        [HttpGet("{userId}")]
        public decimal GetAccountBalance(int userId)
        {
            //get account balance by input userId
            decimal accountBalance = accountDao.GetBalance(userId);
            return accountBalance;
        }

    }

}
