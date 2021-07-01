using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IAccountDao
    {
        decimal GetBalance(int userId);
        public void UpdateBalanceSender(int userId, decimal transferredCash);
        public void UpdateBalanceRecipient(int userId, decimal transferredCash);

    }
}
