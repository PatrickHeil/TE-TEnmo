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
            else
            {
                return response.Data;
            }
        }

        public decimal GetAccountBalance(int userId)
        {
            RestRequest request = new RestRequest(API_URL + "accounts/" + userId.ToString());
            IRestResponse<ApiAccount> response = restClient.Get<ApiAccount>(request);
            //request.AddJsonBody(userId);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception();
            }
            else
            {
                return response.Data.Balance;
            }
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

        public List<ApiTransfer> GetTransfersOfUser(int userId)
        {
            RestRequest request = new RestRequest(API_URL + "transfers/" + "user" + userId.ToString());
            IRestResponse<List<ApiTransfer>> response = restClient.Get<List<ApiTransfer>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception();
            }

            return response.Data;
        }

        public ApiTransfer GetTransferByTransferId(int transferId)
        {
            RestRequest request = new RestRequest(API_URL + "transfers/" + "transfer" + transferId.ToString());
            IRestResponse<ApiTransfer> response = restClient.Get<ApiTransfer>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception();
            }

            return response.Data;
        }

        public ApiTransfer GetLatestTransfer(int userId)
        {
            RestRequest request = new RestRequest(API_URL + "transfers/" + "transfer" + userId.ToString());
            IRestResponse<ApiTransfer> response = restClient.Get<ApiTransfer>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception();
            }
            return response.Data;
        }

        public void UpdateAccounts(ApiTransfer transfer)
        {
            RestRequest request = new RestRequest(API_URL + "accounts/" + transfer.TransferId);
            request.AddJsonBody(transfer);
            IRestResponse response = restClient.Put(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception();
            }
        }

        //public void UpdateRecipientAccount(ApiTransfer transfer)
        //{
        //    RestRequest request = new RestRequest(API_URL + "accounts/" + transfer.AccountTo);
        //    request.AddJsonBody(transfer);
        //    IRestResponse<ApiAccount> response = restClient.Put<ApiAccount>(request);

        //    if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
        //    {
        //        throw new Exception();
        //    }
        //}

        public void PostNewTransferToDatabase(ApiTransfer apiTransfer)
        {
            RestRequest request = new RestRequest(API_URL + "transfers/" + apiTransfer.TransferId.ToString());
            request.AddJsonBody(apiTransfer);
            IRestResponse<ApiTransfer> response = restClient.Post<ApiTransfer>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception();
            }        
        }



        public static int GetAccountId()
        {
            return apiService.GetAccount(user.UserId).AccountId;
        }
    }
}

