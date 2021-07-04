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
    public class UserController : Controller
    {
        private IUserDao userDao;
        private IAccountDao accountDao;

        public UserController(IUserDao _userDao, IAccountDao accountDao)
        {
            this.userDao = _userDao;
            this.accountDao = accountDao;
        }

        [HttpGet()]
        public List<User> GetUsersForMoneyTransfer()
        {
            List<User> listOfAllUsers = userDao.GetUsersForMoneyTransfer();
            return listOfAllUsers;
        }


    }
}
