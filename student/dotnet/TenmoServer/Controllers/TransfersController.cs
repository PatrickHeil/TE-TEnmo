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
    [Route("transfers/")]
    [ApiController]
    public class TransfersController : Controller
    {
        private ITransferDao transferDao;

        public TransfersController(ITransferDao _transferDao)
        {
            this.transferDao = _transferDao;
        }

        [HttpGet]
        public List<Transfer> GetTransfers() // list all transfers
        {
            List<Transfer> listOfTransfers = transferDao.GetTransfers();
            return listOfTransfers;
        }

        [HttpGet("user{userId}")]
        public List<Transfer> GetTransfersOfUser(int userId) // list one user's transfers
        {
            List<Transfer> listOfTransfers = transferDao.GetTransfersOfUser(userId);
            return listOfTransfers;
        }

        [HttpGet("transfer{transferId}")]
        public Transfer GetTransferByTransferId(int transferId) // get details of one transfer based on transferId
        {
            Transfer desiredTransfer = transferDao.GetTransferByTransferId(transferId);
            return desiredTransfer;
        }

        [HttpPost("{accountFrom}")]
        public void CreateTransfer(Transfer transfer) // create a new transfer
        {
            transferDao.Transfer(transfer);
        }
    }
}
