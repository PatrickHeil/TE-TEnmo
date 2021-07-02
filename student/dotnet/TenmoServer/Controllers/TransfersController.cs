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
    public class TransfersController : Controller
    {
        private ITransferDao transferDao;

        public TransfersController(ITransferDao _transferDao)
        {
            this.transferDao = _transferDao;
        }

        [HttpGet]
        public List<Transfer> GetTransfers()
        {
            List<Transfer> listOfTransfers = transferDao.GetTransfers();
            return listOfTransfers;
        }

        [HttpPost("{accountFrom}")]
        public void CreateTransfer(Transfer transfer)
        {
            transferDao.Transfer(transfer);
        }

        [HttpPut("{userId}")]
        public void UpdateAccount(Account account)
        {

            transferDao.UpdateBalanceSender(account);
        }

    }
}
