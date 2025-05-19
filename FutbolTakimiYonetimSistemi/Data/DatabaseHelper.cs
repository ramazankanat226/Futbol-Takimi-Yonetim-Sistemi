using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;

namespace FutbolTakimiYonetimSistemi.Data
{
    public class DatabaseHelper
    {
        private static readonly string DatabaseName = "FutbolTakimi.accdb";
        private static string _connectionString = "";

        public static string ConnectionString 
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    string dbPath = Path.Combine(Application.StartupPath, DatabaseName);
                    _connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";
                }
                return _connectionString;
            }
        }

        public static OleDbConnection GetConnection()
        {
            return new OleDbConnection(ConnectionString);
        }

        public static DataTable ExecuteQuery(string query, params OleDbParameter[] parameters)
        {
            using (OleDbConnection connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        DataTable dataTable = new DataTable();
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                        return dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Veritabanı hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        public static int ExecuteNonQuery(string query, params OleDbParameter[] parameters)
        {
            using (OleDbConnection connection = GetConnection())
            {
                OleDbTransaction transaction = null;
                try
                {
                    connection.Open();
                    // Begin a transaction to ensure database consistency
                    transaction = connection.BeginTransaction();
                    
                    using (OleDbCommand command = new OleDbCommand(query, connection, transaction))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        int result = command.ExecuteNonQuery();
                        
                        // If we got here without errors, commit the transaction
                        transaction.Commit();
                        return result;
                    }
                }
                catch (OleDbException dbEx)
                {
                    // Try to rollback the transaction if it exists
                    try { transaction?.Rollback(); } catch { /* Ignore rollback error */ }
                    
                    string errorDetails = $"SQL Hata Kodu: {dbEx.ErrorCode}, Hata: {dbEx.Message}";
                    MessageBox.Show($"Veritabanı işlemi sırasında hata oluştu: {errorDetails}", "Veritabanı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                catch (Exception ex)
                {
                    // Try to rollback the transaction if it exists
                    try { transaction?.Rollback(); } catch { /* Ignore rollback error */ }
                    
                    MessageBox.Show($"Veritabanı işlemi sırasında beklenmeyen hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
            }
        }

        public static object ExecuteScalar(string query, params OleDbParameter[] parameters)
        {
            using (OleDbConnection connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        return command.ExecuteScalar();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Veritabanı hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        public static bool VerifyDatabaseConnection()
        {
            try
            {
                using (OleDbConnection connection = GetConnection())
                {
                    connection.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
} 