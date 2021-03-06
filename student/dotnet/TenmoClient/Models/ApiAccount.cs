using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    
    public class ApiAccount
    {
        private static ApiService apiService = new ApiService();
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public decimal Balance { get; set; }

        public ApiAccount()
        {
            
        }

        public ApiAccount(decimal balance)
        {
            this.Balance = balance;
            this.UserId = UserService.GetUserId();
            this.AccountId = apiService.GetAccountIdByUserId(this.UserId);
        }
    }
}
