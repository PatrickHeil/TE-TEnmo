﻿using System;
using System.Collections.Generic;
using TenmoClient.Models;

namespace TenmoClient
{
    class Program
    {
        private static readonly ConsoleService consoleService = new ConsoleService();
        private static readonly AuthService authService = new AuthService();
        private static readonly ApiService apiService = new ApiService();
        //private static readonly ApiService apiService = new ApiService("https://localhost:44315/"); //added this line of code

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
                Console.WriteLine("3: View your pending requests"); //this is optional use case #8
                Console.WriteLine("4: Send TE bucks");
                Console.WriteLine("5: Request TE bucks"); //this is optional use case #7
                Console.WriteLine("6: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else if (menuSelection == 1)
                {
                    
                    //int userId = UserService.GetUserId();
                    decimal currentAccountBalance = apiService.GetAccountBalance(UserService.GetUserId());
                    Console.WriteLine($"Your current account balance is: {currentAccountBalance}");
                }
                else if (menuSelection == 2)
                {

                }
                else if (menuSelection == 3)
                {

                }
                else if (menuSelection == 4) //***OPTION 4 TO SEND TE BUCKS TO A USER ID***
                {
                    //ApiService api = new ApiService(); //api variable not used anywhere else in code. commented out code
                    List<ApiUser> allUsers = apiService.GetAllUsers();
                    for (int i = 0; i < allUsers.Count; i++)
                    {
                        Console.WriteLine($"{allUsers[i].UserId}, {allUsers[i].Username}");
                    }
                    Console.WriteLine("Please select user by userId: ");
                    string userId = Console.ReadLine();
                    int userIdAsInt = Convert.ToInt32(userId);

                    for (int i = 0; i < allUsers.Count; i++)
                    {
                        if (userIdAsInt == allUsers[i].UserId)
                        {
                            Console.WriteLine("Please enter the amount you would like to send: ");
                            string amountResponse = Console.ReadLine();
                            decimal amountToTransfer = Convert.ToDecimal(amountResponse);
                        }
                    }
                }
                else if (menuSelection == 5)
                {

                }
                else if (menuSelection == 6)
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
