using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TenmoServer.Models;
using TenmoServer.Security;
using TenmoServer.Security.Models;

namespace TenmoServer.DAO
{
    public class AccountSqlDao : IAccountDao
    {

        private readonly string connectionString;
        const decimal startingBalance = 1000;

        public AccountSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public decimal GetBalance(int userId)
        {
            decimal returnBalance = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT account_id, user_id, balance FROM dbo.accounts "
                        + "WHERE user_id = @user_id", conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Account balance = GetAccountFromReader(reader);
                        returnBalance = balance.Balance;
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return returnBalance;
        }

        public decimal UpdateBalanceSender(int userId, decimal transferredCash)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT users.user_id, accounts.balance FROM dbo.accounts "
                        + "JOIN users ON accounts.user_id = users.user_id WHERE users.user_id = @user_id", conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return accountBalance;
        }

        private Account GetAccountFromReader(SqlDataReader reader)
        {
            Account balance = new Account()
            {
                AccountId = Convert.ToInt32(reader["account_id"]),
                UserId = Convert.ToInt32(reader["user_id"]),
                Balance = Convert.ToDecimal(reader["balance"]),
            };

            return balance;
        }
    }
}
