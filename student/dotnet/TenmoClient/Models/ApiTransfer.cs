using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    public class ApiTransfer
    {
        public ApiTransfer()
        {

        }

        public ApiTransfer(int transferTypeId, int transferStatusId, int accountFrom, int accountTo, decimal amount)
        {
            this.TransferTypeId = transferTypeId;
            this.TransferStatusId = transferStatusId;
            this.AccountFrom = accountFrom;
            this.AccountTo = accountTo;
            this.Amount = amount;
        }

        public int TransferId { get; set; }
        public int TransferTypeId { get; set; } = 2;
        public int TransferStatusId { get; set; } = 2;
        public int AccountFrom { get; set; }
        public int AccountTo { get; set; }
        public decimal Amount { get; set; }
    }
}
