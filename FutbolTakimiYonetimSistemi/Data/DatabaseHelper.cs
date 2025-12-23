using System;
using System.Configuration;
using System.Data;
using Npgsql;
using System.Windows.Forms;

namespace FutbolTakimiYonetimSistemi.Data
{
    public class DatabaseHelper
    {
        private static string _connectionString = "";

        /// <summary>
        /// App.config'ten bağlantı string'ini alır
        /// </summary>
        public static string ConnectionString 
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    try
                    {
                        // App.config'ten al
                        _connectionString = ConfigurationManager.ConnectionStrings["FutbolTakimiDB"]?.ConnectionString;

                        if (string.IsNullOrEmpty(_connectionString))
                        {
                            throw new InvalidOperationException("App.config'te 'FutbolTakimiDB' bağlantı string'i bulunamadı!");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Bağlantı ayarları yüklenemedi: {ex.Message}\n\nApp.config dosyasını kontrol edin!", 
                            "Yapılandırma Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw;
                    }
                }
                return _connectionString;
            }
        }

        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        public static DataTable ExecuteQuery(string query, params NpgsqlParameter[] parameters)
        {
            using (NpgsqlConnection connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        DataTable dataTable = new DataTable();
                        using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
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

        public static int ExecuteNonQuery(string query, params NpgsqlParameter[] parameters)
        {
            using (NpgsqlConnection connection = GetConnection())
            {
                NpgsqlTransaction transaction = null;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection, transaction))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        int result = command.ExecuteNonQuery();
                        transaction.Commit();
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    try { transaction?.Rollback(); } catch { }
                    MessageBox.Show($"Veritabanı işlemi sırasında hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
            }
        }

        public static object ExecuteScalar(string query, params NpgsqlParameter[] parameters)
        {
            using (NpgsqlConnection connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
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

        // Stored Procedure çağırma metodu (YENİ)
        public static DataTable ExecuteStoredProcedure(string procedureName, params NpgsqlParameter[] parameters)
        {
            using (NpgsqlConnection connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        DataTable dataTable = new DataTable();
                        using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                        return dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Stored Procedure hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        // Stored Procedure scalar değer döndürme (YENİ)
        public static object ExecuteStoredProcedureScalar(string procedureName, params NpgsqlParameter[] parameters)
        {
            using (NpgsqlConnection connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        return command.ExecuteScalar();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Stored Procedure hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        public static bool VerifyDatabaseConnection()
        {
            try
            {
                using (NpgsqlConnection connection = GetConnection())
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
