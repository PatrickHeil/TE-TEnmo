using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDao
    {

        public void Transfer(int accountFrom, int accountTo, decimal transferredAmount);
        public void UpdateBalanceSender(int accountId);
        public void UpdateBalanceRecipient(int accountId);
        public List<Transfer> GetTransfers();

    }
}
