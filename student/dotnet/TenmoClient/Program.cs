using System;
using System.Collections.Generic;
using TenmoClient.Models;

namespace TenmoClient
{
    class Program
    {
        private static readonly ConsoleService consoleService = new ConsoleService();
        private static readonly AuthService authService = new AuthService();
        private static readonly ApiService apiService = new ApiService();

        static void Main(string[] args)
        {
            Run();
        }

        private static void Run()
        {
            int loginRegister = -1;
            while (loginRegister != 1 && loginRegister != 2)
            {
                Console.WriteLine("Welcome to TEnmo!");
                Console.WriteLine("1: Login");
                Console.WriteLine("2: Register");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out loginRegister))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else if (loginRegister == 1)
                {
                    while (!UserService.IsLoggedIn()) //will keep looping until user is logged in
                    {
                        LoginUser loginUser = consoleService.PromptForLogin();
                        ApiUser user = authService.Login(loginUser);
                        if (user != null)
                        {
                            UserService.SetLogin(user);
                        }
                    }
                }
                else if (loginRegister == 2)
                {
                    bool isRegistered = false;
                    while (!isRegistered) //will keep looping until user is registered
                    {
                        LoginUser registerUser = consoleService.PromptForLogin();
                        isRegistered = authService.Register(registerUser);
                        if (isRegistered)
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Registration successful. You can now log in.");
                            loginRegister = -1; //reset outer loop to allow choice for login
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid selection.");
                }
            }

            MenuSelection();
        }

        private static void MenuSelection()
        {
            int menuSelection = -1;
            while (menuSelection != 0)
            {
                Console.WriteLine("");
                Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                Console.WriteLine("1: View your current balance");
                Console.WriteLine("2: View your past transfers");
                Console.WriteLine("3: View specified transfer");
                Console.WriteLine("4: View your pending requests"); //this is optional use case #8
                Console.WriteLine("5: Send TE bucks");
                Console.WriteLine("6: Request TE bucks"); //this is optional use case #7
                Console.WriteLine("7: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");
                

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else if (menuSelection == 1)
                {
                    decimal currentAccountBalance = apiService.GetAccount(UserService.GetUserId()).Balance;
                    Console.WriteLine();
                    Console.WriteLine($"Your current account balance is: ${currentAccountBalance}"); // writes current user's account balance
                    Console.WriteLine();
                }
                else if (menuSelection == 2)
                {
                    List<ApiTransfer> pastTransfers = apiService.GetTransfersOfUser(UserService.GetUserId());
                    Console.WriteLine();
                    Console.WriteLine("List of All Transfers: ");
                    Console.WriteLine();
                    for (int i = 0; i < pastTransfers.Count; i++)
                    {
                        Console.WriteLine($"Transfer Id: {pastTransfers[i].TransferId}, Transfer Type Id: {pastTransfers[i].TransferTypeId}, Transfer Status Id: {pastTransfers[i].TransferStatusId}, " + 
                            $"Sender: {pastTransfers[i].AccountFrom}, Recipient: {pastTransfers[i].AccountTo}, Amount: {pastTransfers[i].Amount}"); // displays details of all past transfers of current user
                    }
                }
                else if (menuSelection == 3)
                {
                    Console.WriteLine("Please enter the desired transfer ID: ");
                    Console.WriteLine();
                    int desiredTransferId = Convert.ToInt32(Console.ReadLine());
                    ApiTransfer desiredTransfer = apiService.GetTransferByTransferId(desiredTransferId);
                    Console.WriteLine();                                                                 // all of this displays the details of one particular transfer, based on transfer id
                    Console.WriteLine($"Transfer Id: {desiredTransfer.TransferId}");
                    Console.WriteLine($"Transfer Type Id: {desiredTransfer.TransferTypeId}");
                    Console.WriteLine($"Transfer Status Id: {desiredTransfer.TransferStatusId}");
                    Console.WriteLine($"Sender: {desiredTransfer.AccountFrom}");
                    Console.WriteLine($"Recipient: {desiredTransfer.AccountTo}");
                    Console.WriteLine($"Amount: {desiredTransfer.Amount}"); 
                }
                else if (menuSelection == 4) 
                {
                    
                }
                else if (menuSelection == 5) //***OPTION 4 TO SEND TE BUCKS TO A USER ID***
                {
                    List<User> allUsers = apiService.GetAllUsers();
                    for (int i = 0; i < allUsers.Count; i++)
                    {
                        Console.WriteLine($"{allUsers[i].UserId}, {allUsers[i].Username}"); // lists all users
                    }
                    Console.WriteLine();
                    Console.WriteLine("Please select recipient by userId: ");
                    string userId = Console.ReadLine();
                    int userIdAsInt = Convert.ToInt32(userId); // recipient userId entered by current user


                    for (int i = 0; i < allUsers.Count; i++)
                    {
                        if (userIdAsInt == UserService.GetUserId()) // in case current user enters their own id
                        {
                            Console.WriteLine();
                            Console.WriteLine("Recipient cannot be the same account as sender.");
                            Console.WriteLine();
                            break;
                        }
                        else if (userIdAsInt == allUsers[i].UserId)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Please enter the amount you would like to send: ");
                            string amountResponse = Console.ReadLine();
                            decimal amountToTransfer = Convert.ToDecimal(amountResponse);
                            if (amountToTransfer <= 0) // prevents user from transfering a negative number into another user's account (which would result in an exception, anyway)
                            {
                                Console.WriteLine();
                                Console.WriteLine("Transfer must be $0.01 or more.");
                                Console.WriteLine();
                                break;
                            }
                            if (apiService.GetAccountBalance(UserService.GetUserId()) - amountToTransfer < 0) // prevents current user from overdrafting account
                            {
                                Console.WriteLine();
                                Console.WriteLine("Not enough funds to transfer.");
                                Console.WriteLine();
                                break;
                            }

                            ApiTransfer newTransfer = new ApiTransfer(2, 2, apiService.GetAccountIdByUserId(UserService.GetUserId()), 
                                apiService.GetAccountIdByUserId(userIdAsInt), amountToTransfer); // instantiates new transfer based off input thus far, with default values for status/type id
                            apiService.PostNewTransferToDatabase(newTransfer); // posts transfer to transfer table in database

                            apiService.UpdateAccounts(newTransfer); // updates sender and recipient accounts to new balances based off transfer information

                            Console.WriteLine();
                            Console.WriteLine($"Current balance: ${apiService.GetAccountBalance(UserService.GetUserId())}"); // display balance of current user after transfer

                        }
                    }
                }
                else if (menuSelection == 6)
                {
                    
                }
                else if (menuSelection == 7)
                {
                    Console.WriteLine("");
                    UserService.SetLogin(new ApiUser()); //wipe out previous login info
                    Run(); //return to entry point
                }
                else
                {
                    Console.WriteLine("Goodbye!");
                    Environment.Exit(0);
                }
            }
        }
    }
}
