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
        private readonly Account account;

        public TransferSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public TransferSqlDao(Account account)
        {
            this.account = account;
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

        public void UpdateBalances(Transfer transfer)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand send = new SqlCommand("UPDATE dbo.accounts SET balance -= (SELECT amount FROM transfers WHERE transfer_id = " +
                        "(SELECT TOP 1 transfer_id FROM transfers ORDER BY transfer_id DESC)) WHERE account_id = @account_id", conn);
                    send.Parameters.AddWithValue("@account_id", transfer.AccountFrom);

                    send.ExecuteNonQuery();

                    SqlCommand receive = new SqlCommand("UPDATE dbo.accounts SET balance += (SELECT amount FROM transfers WHERE transfer_id = " +
                        "(SELECT TOP 1 transfer_id FROM transfers ORDER BY transfer_id DESC)) WHERE account_id = @account_id", conn);
                    receive.Parameters.AddWithValue("@account_id", transfer.AccountTo);

                    receive.ExecuteNonQuery();
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


        public List<Transfer> GetTransfersOfUser(int userId)
        {
            List<Transfer> transferList = new List<Transfer>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT transfer_id, transfer_type_id, transfer_status_id, account_from, account_to, amount " +
                        "FROM dbo.transfers JOIN accounts ON accounts.account_id = transfers.account_from " +
                                "WHERE user_id = @userId", conn);
                    cmd.Parameters.AddWithValue("@userId", userId);

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

        public Transfer GetLastTransferOfUser(int userId)
        {
            List<Transfer> transferList = new List<Transfer>();
            Transfer transfer = new Transfer();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT transfer_id, transfer_type_id, transfer_status_id, account_from, account_to, amount " +
                        "FROM dbo.transfers JOIN accounts ON accounts.account_id = transfers.account_from " +
                                "WHERE user_id = @userId", conn);
                    cmd.Parameters.AddWithValue("@userId", userId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        transferList.Add(GetTransferFromReader(reader));
                    }
                    for (int i = 0; i < transferList.Count; i++)
                    {
                        if (i == transferList.Count - 1)
                        {
                            transfer = transferList[i];
                        }
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return transfer;
        }

        public Transfer GetTransferByTransferId(int transferId)
        {
            Transfer desiredTransfer = new Transfer();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT transfer_id, transfer_type_id, transfer_status_id, account_from, account_to, amount " +
                        "FROM dbo.transfers WHERE transfer_id = @transferId", conn);
                    cmd.Parameters.AddWithValue("@transferId", transferId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        desiredTransfer = GetTransferFromReader(reader);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return desiredTransfer;
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
