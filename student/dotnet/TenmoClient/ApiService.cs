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

        public ApiAccount GetAccount(int userId) // gets all account information based on userId
        {
            RestRequest request = new RestRequest(API_URL + "accounts/" + userId.ToString());
            IRestResponse<ApiAccount> response = restClient.Get<ApiAccount>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception();
            }
            else
            {
                return response.Data;
            }
        }

        public decimal GetAccountBalance(int userId) // gets balance of a user's account based on userId
        {
            RestRequest request = new RestRequest(API_URL + "accounts/" + userId.ToString());
            IRestResponse<ApiAccount> response = restClient.Get<ApiAccount>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception();
            }
            else
            {
                return response.Data.Balance;
            }
        }

        public int GetAccountIdByUserId(int userId) // input user id to obtain user's TEnmo account id
        {
            RestRequest request = new RestRequest(API_URL + "accounts/" + userId.ToString());
            IRestResponse<ApiAccount> response = restClient.Get<ApiAccount>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception();
            }
            return response.Data.AccountId;
        }

        public List<ApiTransfer> GetTransfersOfUser(int userId) // gets all transfers of a particular user -- STEP 5 -- Program line 98
        {
            RestRequest request = new RestRequest(API_URL + "transfers/" + "user" + userId.ToString());
            IRestResponse<List<ApiTransfer>> response = restClient.Get<List<ApiTransfer>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception();
            }

            return response.Data;
        }

        public ApiTransfer GetTransferByTransferId(int transferId) // get all transfer information after inputing transfer id -- STEP 6 -- Program line 110
        {
            RestRequest request = new RestRequest(API_URL + "transfers/" + "transfer" + transferId.ToString());
            IRestResponse<ApiTransfer> response = restClient.Get<ApiTransfer>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception();
            }

            return response.Data;
        }

        public void UpdateAccounts(ApiTransfer transfer) // updates accounts by sending full transfer over to accounts controller to be broken into transfer.AccountFrom and transfer.AccountTo
        {                                                                                                   // line 168 in Program
            RestRequest request = new RestRequest(API_URL + "accounts/" + transfer.TransferId);
            request.AddJsonBody(transfer);
            IRestResponse response = restClient.Put(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception();
            }
        }

        public void PostNewTransferToDatabase(ApiTransfer apiTransfer) // posts new transfer created in console to be added to database
        {
            RestRequest request = new RestRequest(API_URL + "transfers/" + apiTransfer.TransferId.ToString());
            request.AddJsonBody(apiTransfer);
            IRestResponse<ApiTransfer> response = restClient.Post<ApiTransfer>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception();
            }        
        }

    }


}

