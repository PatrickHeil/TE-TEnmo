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
        private IAccountDao accountDao;

        public TransfersController(ITransferDao _transferDao, IAccountDao _accountDao)
        {
            this.transferDao = _transferDao;
            this.accountDao = _accountDao;
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
        public void UpdateAccount(Transfer transfer, Account account)
        {
            if(transfer.TransferStatusId == 2)
            {
                if(transfer.AccountFrom == account.AccountId)
                {
                    transferDao.UpdateBalanceSender(account);
                }            
            }
            else if(transfer.TransferStatusId == 1)
            {
                if(transfer.AccountTo == account.AccountId)
                {
                    transferDao.UpdateBalanceRecipient(account);
                }
            }
        }
    }
}
