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
        public Transfer GetTransferByTransferId(int transferId);
        public List<Transfer> GetTransfersOfUser(int userId);
        public void UpdateBalanceSender(Account account);
        public void UpdateBalanceRecipient(Account account);
        public List<Transfer> GetTransfers();
    }
}
