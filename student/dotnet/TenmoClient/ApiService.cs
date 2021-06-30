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

    }
}

