using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TenmoServer.Models;
using TenmoServer.Security;
using TenmoServer.Security.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDao : ITransferDao
    {
        private readonly string connectionString;
        //const decimal startingBalance = 1000;
        //private static Transfer transfer;
        private readonly Account account;
        

        public TransferSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public void Transfer(Transfer transfer)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO dbo.transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) "
                       + "VALUES(@transfer_type_id, @transfer_status_id, @account_from, @account_to, @amount)", conn);
                    cmd.Parameters.AddWithValue("@transfer_type_id", transfer.TransferTypeId);
                    cmd.Parameters.AddWithValue("@transfer_status_id", transfer.TransferStatusId);
                    cmd.Parameters.AddWithValue("@account_from", transfer.AccountFrom);
                    cmd.Parameters.AddWithValue("@account_to", transfer.AccountTo);
                    cmd.Parameters.AddWithValue("@amount", transfer.Amount);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }
        }

        public void UpdateBalanceSender(Account account)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE dbo.accounts SET balance -= (SELECT amount FROM transfers WHERE transfer_id = @@IDENTITY) " +
                        "WHERE account_id = (SELECT account_id FROM accounts WHERE account_id = @account_id)", conn);
                    cmd.Parameters.AddWithValue("@account_id", account.AccountId);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }
        }

        public void UpdateBalanceRecipient(int accountId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE dbo.accounts SET balance += (SELECT amount FROM transfers WHERE transfer_id = @@IDENTITY) " +
                        "WHERE account_id = (SELECT account_id FROM accounts WHERE account_id = @account_id)", conn);
                    cmd.Parameters.AddWithValue("@account_id", accountId);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }
        }

        public List<Transfer> GetTransfers()
        {
            List<Transfer> transferList = new List<Transfer>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT transfer_id, transfer_type_id, transfer_status_id, account_from, account_to, amount FROM dbo.transfers", conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        transferList.Add(GetTransferFromReader(reader));
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return transferList;
        }

        private Transfer GetTransferFromReader(SqlDataReader reader)
        {
            Transfer transfer = new Transfer()
            {
                TransferId = Convert.ToInt32(reader["transfer_id"]),
                TransferTypeId = Convert.ToInt32(reader["transfer_type_id"]),
                TransferStatusId = Convert.ToInt32(reader["transfer_status_id"]),
                AccountFrom = Convert.ToInt32(reader["account_from"]),
                AccountTo = Convert.ToInt32(reader["account_to"]),
                Amount = Convert.ToDecimal(reader["amount"]),
            };

            return transfer;

        }

    }
}
    