using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public int TransferId { get; set; }

        public int TransferTypeId { get; set; } = 2;

        public int TransferStatusId { get; set; } = 1;

        public int AccountFrom { get; set; }

        public int AccountTo { get; set; }

        [Range(0.01, 9999999999999)]
        public decimal Amount { get; set; }
    }
}
