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
        private static IUserDao userDao;

        public UserController(IUserDao _userDao)
        {
            userDao = _userDao;
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
