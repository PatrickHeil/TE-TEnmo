using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Models;

namespace TenmoClient
{
    public class ApiService
    {
        private readonly string API_URL = ""; //removed keyword static
        private readonly IRestClient restClient;
        private static ApiUser user = new ApiUser();

        public ApiService(string api_url) //added this method
        {
            API_URL = api_url;
        }
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

        public decimal GetAccountBalance(int userId) //COME BACK TO THIS PROBLEM
        {
            RestRequest request = new RestRequest(API_URL + "accounts" + "/" + userId.ToString());
            IRestResponse<Account> response = restClient.Get<Account>(request);
            request.AddJsonBody(userId);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception();
            }

            return response.Data.Balance;
        }




    }
}

