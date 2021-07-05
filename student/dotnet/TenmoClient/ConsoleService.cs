using System;
using System.Collections.Generic;
using TenmoClient.Models;

namespace TenmoClient
{
    public class ConsoleService
    {
        /// <summary>
        /// Prompts for transfer ID to view, approve, or reject
        /// </summary>
        /// <param name="action">String to print in prompt. Expected values are "Approve" or "Reject" or "View"</param>
        /// <returns>ID of transfers to view, approve, or reject</returns>
        public int PromptForTransferID(string action)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("");
            Console.Write("Please enter transfer ID to " + action + " (0 to cancel): ");
            Console.ForegroundColor = ConsoleColor.White;
            if (!int.TryParse(Console.ReadLine(), out int auctionId))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine();
                Console.WriteLine("ERROR:");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Invalid input. Only input a number.");
                Console.ForegroundColor = ConsoleColor.White;
                return 0;
            }
            else
            {
                return auctionId;
            }
        }

        public LoginUser PromptForLogin()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Username: ");
            Console.ForegroundColor = ConsoleColor.White;
            string username = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            string password = GetPasswordFromConsole("Password: ");
            Console.ForegroundColor = ConsoleColor.White;

            LoginUser loginUser = new LoginUser
            {
                Username = username,
                Password = password
            };
            return loginUser;
        }

        private string GetPasswordFromConsole(string displayMessage)
        {
            string pass = "";
            Console.Write(displayMessage);
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                // Backspace Should Not Work
                if (!char.IsControl(key.KeyChar))
                {
                    pass += key.KeyChar;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Remove(pass.Length - 1);
                        Console.Write("\b \b");
                    }
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);
            Console.WriteLine("");
            return pass;
        }
    }
}
