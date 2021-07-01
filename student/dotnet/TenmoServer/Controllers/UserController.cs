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
        private IAccountDao accountDao; //change made

        public UserController(IUserDao _userDao, IAccountDao accountDao) //change made
        {
            this.userDao = _userDao;
            this.accountDao = accountDao; //change made
        }

        [HttpGet()]
        public List<User> GetUsersForMoneyTransfer()
        {
            //Get list of all users in the database
            List<User> listOfAllUsers = userDao.GetUsersForMoneyTransfer();
            return listOfAllUsers;
        }


    }
}
