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
        public ApiService()
        {
            restClient = new RestClient();
        }
        public ApiService(IRestClient restClient)
        {
            restClient = this.restClient;
        }

        public List<ApiUser> GetAllUsers()
        {
            RestRequest request = new RestRequest(API_URL + "user");
            IRestResponse<List<ApiUser>> response = restClient.Get<List<ApiUser>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                return response.Data;
            }
            
            return null;
        }

        public decimal GetAccountBalance(int userId) //COME BACK TO THIS PROBLEM
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

        public decimal UpdateSenderAccount(ApiAccount senderBalance, decimal cash)
        {
            RestRequest request = new RestRequest(API_URL + "accounts/" + senderBalance.UserId.ToString());
            IRestResponse<ApiAccount> response = restClient.Put<ApiAccount>(request);
            request.AddJsonBody(senderBalance.Balance - cash);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception();
            }

            return senderBalance.Balance;
        }

        public decimal UpdateRecipientAccount(ApiAccount recipientBalance, decimal cash)
        {
            RestRequest request = new RestRequest(API_URL + "accounts/" + recipientBalance.UserId.ToString());
            IRestResponse<ApiAccount> response = restClient.Put<ApiAccount>(request);
            request.AddJsonBody(recipientBalance.Balance + cash);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception();
            }

            return recipientBalance.Balance;
        }

    }
}

