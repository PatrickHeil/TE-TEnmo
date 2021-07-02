using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    
    public class ApiAccount
    {
        

        public int AccountId { get; set; }
        public int UserId { get; set; }
        public decimal Balance { get; set; }

        public ApiAccount()
        {
            
        }

        public ApiAccount(decimal balance)
        {
            this.Balance = balance;
            this.AccountId = ApiService.GetAccountId();
            this.UserId = UserService.GetUserId();
        }

    //public Account(int userId, decimal balance)
    //{
    //    this.UserId = userId;
    //    this.Balance = balance;
    //}


    }
}
