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

        public TransferSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public void Transfer(int accountFrom, int accountTo, decimal transferredAmount)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO dbo.transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) "
                       + "VALUES(2, 1, @account_from, @account_to, @amount)", conn);
                    cmd.Parameters.AddWithValue("@account_from", accountFrom);
                    cmd.Parameters.AddWithValue("@account_to", accountTo);
                    cmd.Parameters.AddWithValue("@amount", transferredAmount);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }
        }

        public void UpdateBalanceSender(int accountId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE dbo.accounts SET balance -= (SELECT amount FROM transfers WHERE account_from = @@IDENTITY) " +
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

        public void UpdateBalanceRecipient(int accountId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE dbo.accounts SET balance += (SELECT amount FROM transfers WHERE account_from = @@IDENTITY) " +
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
                TransferStatus = Convert.ToInt32(reader["transfer_status_id"]),
                AccountFrom = Convert.ToInt32(reader["account_from"]),
                AccountTo = Convert.ToInt32(reader["account_to"]),
                Amount = Convert.ToDecimal(reader["amount"]),
            };

            return transfer;

        }

    }
}
    