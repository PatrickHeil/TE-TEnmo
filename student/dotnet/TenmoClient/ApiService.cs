using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Models;


namespace TenmoClient
{
    public class ApiService
    {
        private readonly static string API_URL = "https://localhost:44315/";
        private readonly IRestClient restClient;
        private static ApiUser user = new ApiUser();
        private static ApiAccount account = new ApiAccount();
        public ApiService()
        {
            restClient = new RestClient();
        }
        public ApiService(IRestClient restClient)
        {
            restClient = this.restClient;
        }

        public List<User> GetAllUsers()
        {
            RestRequest request = new RestRequest(API_URL + "user");
            IRestResponse<List<User>> response = restClient.Get<List<User>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception();
            }

            return response.Data;
        }

        public decimal GetAccountBalance(int userId)
        {
            RestRequest request = new RestRequest(API_URL + "accounts/" + userId.ToString());
            IRestResponse<decimal> response = restClient.Get<decimal>(request);
            //request.AddJsonBody(userId);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception();
            }
            return response.Data;
        }

        public decimal UpdateSenderAccount(ApiAccount senderAccount, decimal cash)
        {
            senderAccount.Balance -= cash;
            RestRequest request = new RestRequest(API_URL + "accounts/" + senderAccount.UserId.ToString());
            request.AddJsonBody(senderAccount);
            IRestResponse<ApiAccount> response = restClient.Put<ApiAccount>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception();
            }

            return senderAccount.Balance;
        }

        public decimal UpdateRecipientAccount(ApiAccount recipientBalance, decimal cash)
        {
            recipientBalance.Balance += cash;
            RestRequest request = new RestRequest(API_URL + "accounts/" + recipientBalance.UserId.ToString());
            request.AddJsonBody(recipientBalance.Balance + cash);
            IRestResponse<ApiAccount> response = restClient.Put<ApiAccount>(request);
           
            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception();
            }

            return recipientBalance.Balance;
        }

        public static int GetAccountId()
        {
            return account.AccountId;
        }

    }
}

