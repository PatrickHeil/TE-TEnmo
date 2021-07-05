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
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Welcome to TEnmo!");
                Console.WriteLine();
                Console.WriteLine("_/_/_/_/_/  _/_/_/_/  _/      _/  _/      _/    _/_/    ");
                Console.WriteLine("   _/      _/        _/_/    _/  _/_/  _/_/  _/    _/   ");
                Console.WriteLine("  _/      _/_/_/    _/  _/  _/  _/  _/  _/  _/    _/    ");
                Console.WriteLine(" _/      _/        _/    _/_/  _/      _/  _/    _/     ");
                Console.WriteLine("_/      _/_/_/_/  _/      _/  _/      _/    _/_/        ");
                Console.WriteLine();
                Console.WriteLine("1: Login");
                Console.WriteLine("2: Register");
                Console.WriteLine();
                Console.Write("Please choose an option: ");
                Console.ForegroundColor = ConsoleColor.White;

                if (!int.TryParse(Console.ReadLine(), out loginRegister))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Invalid input. Please enter only a number.");
                    Console.ForegroundColor = ConsoleColor.White;
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
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("");
                            Console.WriteLine("Registration successful. You can now log in.");
                            loginRegister = -1; //reset outer loop to allow choice for login
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Invalid selection.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            MenuSelection();
        }

        private static void MenuSelection()
        {
            int menuSelection = -1;
            while (menuSelection != 0)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("");
                Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                Console.WriteLine();
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
                Console.ForegroundColor = ConsoleColor.White;


                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine();
                    Console.WriteLine("ERROR:");

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else if (menuSelection == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    decimal currentAccountBalance = apiService.GetAccount(UserService.GetUserId()).Balance;
                    Console.WriteLine();
                    Console.Write("Your current account balance is: ");

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"${ currentAccountBalance}"); // writes current user's account balance
                    Console.WriteLine();
                    Console.ReadLine();
                }
                else if (menuSelection == 2)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    List<ApiTransfer> pastTransfers = apiService.GetTransfersOfUser(UserService.GetUserId());
                    Console.WriteLine();
                    Console.WriteLine("List of All Transfers for current user:");
                    Console.WriteLine();
                    for (int i = 0; i < pastTransfers.Count; i++)
                    {
                        if (i % 2 == 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"Transfer Id: {pastTransfers[i].TransferId}, Transfer Type Id: {pastTransfers[i].TransferTypeId}, Transfer Status Id: {pastTransfers[i].TransferStatusId}, " +
                                $"Sender: {pastTransfers[i].AccountFrom}, Recipient: {pastTransfers[i].AccountTo}, Amount: {pastTransfers[i].Amount}"); // displays details of all past transfers of current user
                        }
                        else if (i % 2 == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"Transfer Id: {pastTransfers[i].TransferId}, Transfer Type Id: {pastTransfers[i].TransferTypeId}, Transfer Status Id: {pastTransfers[i].TransferStatusId}, " +
                                $"Sender: {pastTransfers[i].AccountFrom}, Recipient: {pastTransfers[i].AccountTo}, Amount: {pastTransfers[i].Amount}"); // displays details of all past transfers of current user
                        }

                    }
                    Console.ReadLine();
                }
                else if (menuSelection == 3)
                {
                    List<ApiTransfer> pastTransfers = apiService.GetTransfersOfUser(UserService.GetUserId());

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("Please enter the desired transfer ID: ");

                    Console.ForegroundColor = ConsoleColor.White;
                    int desiredTransferId = Convert.ToInt32(Console.ReadLine());
                    ApiTransfer desiredTransfer = apiService.GetTransferByTransferId(desiredTransferId); Console.ForegroundColor = ConsoleColor.Cyan;

                    for(int i = 0; i <= pastTransfers.Count; i++)
                    {
                        try
                        {
                            if (pastTransfers[i].TransferId == desiredTransfer.TransferId)
                            {
                                Console.WriteLine();    // all of this displays the details of one particular transfer, based on transfer id
                                Console.Write("Transfer Id: "); Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine($"{desiredTransfer.TransferId}"); Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.Write("Transfer Type Id: "); Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine($"{desiredTransfer.TransferTypeId}"); Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.Write("Transfer Status Id: "); Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine($"{desiredTransfer.TransferStatusId}"); Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.Write("Sender: "); Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine($"{desiredTransfer.AccountFrom}"); Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.Write("Recipient: "); Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine($"{desiredTransfer.AccountTo}"); Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.Write("Amount: "); Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine($"{desiredTransfer.Amount}");
                                Console.ReadLine();
                                break;
                            }
                        }
                        catch            // handles user input error if transferId does not match list of past transfers
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine();
                            Console.WriteLine("ERROR:");

                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("Please select a valid transfer id for current user.");
                            Console.ReadLine();
                        }
                    }
                }
                else if (menuSelection == 4)
                {

                }
                else if (menuSelection == 5) //***OPTION 4 TO SEND TE BUCKS TO A USER ID***
                {
                    List<User> allUsers = apiService.GetAllUsers();
                    for (int i = 0; i < allUsers.Count; i++)
                    {
                        if (i % 2 == 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"{allUsers[i].UserId}, {allUsers[i].Username}"); // lists all users
                        }
                        else if (i % 2 == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"{allUsers[i].UserId}, {allUsers[i].Username}"); // lists all users
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine();
                    Console.Write("Please select recipient by userId: ");

                    Console.ForegroundColor = ConsoleColor.White;
                    string userId = Console.ReadLine();
                    int userIdAsInt = Convert.ToInt32(userId); // recipient userId entered by current user


                    for (int i = 0; i < allUsers.Count; i++)
                    {
                        if (userIdAsInt == UserService.GetUserId()) // in case current user enters their own id
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine();
                            Console.WriteLine("ERROR:");

                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("Recipient cannot be the same account as sender.");
                            Console.WriteLine();

                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        }
                        else if (userIdAsInt == allUsers[i].UserId)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine();
                            Console.Write("Please enter the amount you would like to send: ");

                            Console.ForegroundColor = ConsoleColor.White;
                            string amountResponse = Console.ReadLine();
                            decimal amountToTransfer = Convert.ToDecimal(amountResponse);
                            if (amountToTransfer <= 0) // prevents user from transfering a negative number into another user's account (which would result in an exception, anyway)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine();
                                Console.WriteLine("ERROR:");

                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine("Transfer must be $0.01 or more.");
                                Console.WriteLine();

                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                            if (apiService.GetAccountBalance(UserService.GetUserId()) - amountToTransfer < 0) // prevents current user from overdrafting account
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine();
                                Console.WriteLine("ERROR:");

                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine("Not enough funds to transfer.");
                                Console.WriteLine();

                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }

                            ApiTransfer newTransfer = new ApiTransfer(2, 2, apiService.GetAccountIdByUserId(UserService.GetUserId()),
                                apiService.GetAccountIdByUserId(userIdAsInt), amountToTransfer); // instantiates new transfer based off input thus far, with default values for status/type id
                            apiService.PostNewTransferToDatabase(newTransfer); // posts transfer to transfer table in database

                            apiService.UpdateAccounts(newTransfer); // updates sender and recipient accounts to new balances based off transfer information

                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine();
                            Console.Write($"Current balance: ");

                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"{apiService.GetAccountBalance(UserService.GetUserId())}"); // display balance of current user after transfer
                            Console.ReadLine();
                        }
                    }
                }
                else if (menuSelection == 6)
                {

                }
                else if (menuSelection == 7)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("");
                    UserService.SetLogin(new ApiUser()); //wipe out previous login info
                    Run(); //return to entry point

                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Goodbye!");
                    Environment.Exit(0);

                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
    }
}
