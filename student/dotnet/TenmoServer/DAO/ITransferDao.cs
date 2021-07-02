using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDao
    {

        public void Transfer(Transfer transfer);
        public void UpdateBalanceSender(Account account);
        public void UpdateBalanceRecipient(int accountId);
        public List<Transfer> GetTransfers();

    }
}
