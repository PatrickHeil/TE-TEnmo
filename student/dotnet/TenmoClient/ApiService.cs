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
        private static ApiService apiService = new ApiService();
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

        public ApiAccount GetAccount(int userId)
        {
            RestRequest request = new RestRequest(API_URL + "accounts/" + userId.ToString());
            IRestResponse<ApiAccount> response = restClient.Get<ApiAccount>(request);
            //request.AddJsonBody(userId);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception();
            }
            return response.Data;
        }

        public int GetAccountIdByUserId(int userId)
        {
            RestRequest request = new RestRequest(API_URL + "accounts/" + userId.ToString());
            IRestResponse<ApiAccount> response = restClient.Get<ApiAccount>(request);
            //request.AddJsonBody(userId);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception();
            }
            return response.Data.AccountId;
        }

        public void UpdateSenderAccount(ApiAccount senderAccount, decimal cash)
        {
            senderAccount.Balance -= cash;
            RestRequest request = new RestRequest(API_URL + "accounts/" + senderAccount.UserId.ToString());
            request.AddJsonBody(senderAccount);
            IRestResponse response = restClient.Put(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception();
            }
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
            return apiService.GetAccount(user.UserId).AccountId;
        }

    }
}

