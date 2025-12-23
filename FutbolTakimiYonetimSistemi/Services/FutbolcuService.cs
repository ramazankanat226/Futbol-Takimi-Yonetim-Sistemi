using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using FutbolTakimiYonetimSistemi.Data;
using FutbolTakimiYonetimSistemi.Models;

namespace FutbolTakimiYonetimSistemi.Services
{
    public class FutbolcuService
    {
        public static List<Futbolcu> GetAllFutbolcular()
        {
            // PostgreSQL'de function SELECT ile çağrılır
            string query = "SELECT * FROM sp_futbolcu_listele()";
            DataTable result = DatabaseHelper.ExecuteQuery(query);
            List<Futbolcu> futbolcular = new List<Futbolcu>();

            if (result != null)
            {
                foreach (DataRow row in result.Rows)
                {
                    // Basit liste için
                    futbolcular.Add(new Futbolcu
                    {
                        FutbolcuID = Convert.ToInt32(row["futbolcuid"]),
                        Ad = row["ad"].ToString(),
                        Soyad = row["soyad"].ToString(),
                        Pozisyon = row["pozisyon"].ToString(),
                        FormaNo = Convert.ToInt32(row["formano"]),
                        Durumu = row["durumu"].ToString()
                    });
                }
            }

            return futbolcular;
        }

        public static Futbolcu GetById(int futbolcuId)
        {
            string query = "SELECT * FROM Futbolcular WHERE FutbolcuID = @FutbolcuID";
            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@FutbolcuID", futbolcuId)
            };

            DataTable result = DatabaseHelper.ExecuteQuery(query, parameters);

            if (result != null && result.Rows.Count > 0)
            {
                return CreateFutbolcuFromDataRow(result.Rows[0]);
            }

            return null;
        }

        public static bool EkleFutbolcu(Futbolcu futbolcu)
        {
            try
            {
                // Forma numarası kontrolü
                if (FormaNoKullaniliyor(futbolcu.FormaNo, 0))
                {
                    System.Windows.Forms.MessageBox.Show($"Forma numarası {futbolcu.FormaNo} zaten kullanımda!\nLütfen farklı bir numara seçin.", 
                        "Forma Numarası Hatası", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return false;
                }

                // PostgreSQL'de function SELECT ile çağrılır
                string query = @"SELECT sp_futbolcu_ekle(@p_ad, @p_soyad, @p_dogum, @p_boy, @p_kilo, 
                                @p_pozisyon, @p_forma_no, @p_maas, @p_sozlesme_bas, @p_sozlesme_bit, 
                                @p_uyruk, @p_durumu, @p_notlar)";
                
                var parameters = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@p_ad", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = futbolcu.Ad },
                    new NpgsqlParameter("@p_soyad", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = futbolcu.Soyad },
                    new NpgsqlParameter("@p_dogum", NpgsqlTypes.NpgsqlDbType.Date) { Value = futbolcu.DogumTarihi.Date },
                    new NpgsqlParameter("@p_boy", NpgsqlTypes.NpgsqlDbType.Integer) { Value = futbolcu.Boy },
                    new NpgsqlParameter("@p_kilo", NpgsqlTypes.NpgsqlDbType.Integer) { Value = futbolcu.Kilo },
                    new NpgsqlParameter("@p_pozisyon", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = futbolcu.Pozisyon },
                    new NpgsqlParameter("@p_forma_no", NpgsqlTypes.NpgsqlDbType.Integer) { Value = futbolcu.FormaNo },
                    new NpgsqlParameter("@p_maas", NpgsqlTypes.NpgsqlDbType.Numeric) { Value = futbolcu.Maas },
                    new NpgsqlParameter("@p_sozlesme_bas", NpgsqlTypes.NpgsqlDbType.Date) { Value = futbolcu.SozlesmeBaslangic.Date },
                    new NpgsqlParameter("@p_sozlesme_bit", NpgsqlTypes.NpgsqlDbType.Date) { Value = futbolcu.SozlesmeBitis.Date },
                    new NpgsqlParameter("@p_uyruk", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = futbolcu.Uyruk },
                    new NpgsqlParameter("@p_durumu", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = futbolcu.Durumu },
                    new NpgsqlParameter("@p_notlar", NpgsqlTypes.NpgsqlDbType.Text) { Value = (object)futbolcu.Notlar ?? DBNull.Value }
                };

                object result = DatabaseHelper.ExecuteScalar(query, parameters);
                return result != null && Convert.ToInt32(result) > 0;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Futbolcu eklenirken hata: " + ex.Message, 
                    "Hata", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool GuncelleFutbolcu(Futbolcu futbolcu)
        {
            try
            {
                // Forma numarası kontrolü (kendi ID'si hariç)
                if (FormaNoKullaniliyor(futbolcu.FormaNo, futbolcu.FutbolcuID))
                {
                    System.Windows.Forms.MessageBox.Show($"Forma numarası {futbolcu.FormaNo} başka bir futbolcuda kullanılıyor!\nLütfen farklı bir numara seçin.", 
                        "Forma Numarası Hatası", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return false;
                }

                // PostgreSQL'de function SELECT ile çağrılır
                string query = @"SELECT sp_futbolcu_guncelle(@p_id, @p_ad, @p_soyad, @p_dogum, @p_boy, @p_kilo, 
                                @p_pozisyon, @p_forma_no, @p_maas, @p_sozlesme_bas, @p_sozlesme_bit, 
                                @p_uyruk, @p_durumu, @p_notlar)";
                
                var parameters = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@p_id", NpgsqlTypes.NpgsqlDbType.Integer) { Value = futbolcu.FutbolcuID },
                    new NpgsqlParameter("@p_ad", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = futbolcu.Ad },
                    new NpgsqlParameter("@p_soyad", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = futbolcu.Soyad },
                    new NpgsqlParameter("@p_dogum", NpgsqlTypes.NpgsqlDbType.Date) { Value = futbolcu.DogumTarihi.Date },
                    new NpgsqlParameter("@p_boy", NpgsqlTypes.NpgsqlDbType.Integer) { Value = futbolcu.Boy },
                    new NpgsqlParameter("@p_kilo", NpgsqlTypes.NpgsqlDbType.Integer) { Value = futbolcu.Kilo },
                    new NpgsqlParameter("@p_pozisyon", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = futbolcu.Pozisyon },
                    new NpgsqlParameter("@p_forma_no", NpgsqlTypes.NpgsqlDbType.Integer) { Value = futbolcu.FormaNo },
                    new NpgsqlParameter("@p_maas", NpgsqlTypes.NpgsqlDbType.Numeric) { Value = futbolcu.Maas },
                    new NpgsqlParameter("@p_sozlesme_bas", NpgsqlTypes.NpgsqlDbType.Date) { Value = futbolcu.SozlesmeBaslangic.Date },
                    new NpgsqlParameter("@p_sozlesme_bit", NpgsqlTypes.NpgsqlDbType.Date) { Value = futbolcu.SozlesmeBitis.Date },
                    new NpgsqlParameter("@p_uyruk", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = futbolcu.Uyruk },
                    new NpgsqlParameter("@p_durumu", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = futbolcu.Durumu },
                    new NpgsqlParameter("@p_notlar", NpgsqlTypes.NpgsqlDbType.Text) { Value = (object)futbolcu.Notlar ?? DBNull.Value }
                };

                object result = DatabaseHelper.ExecuteScalar(query, parameters);
                return result != null && Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Futbolcu güncellenirken hata: " + ex.Message, 
                    "Hata", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool SilFutbolcu(int futbolcuId)
        {
            try
            {
                // PostgreSQL'de function SELECT ile çağrılır
                string query = "SELECT sp_futbolcu_sil(@p_id)";
                
                var parameters = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@p_id", futbolcuId)
                };

                object result = DatabaseHelper.ExecuteScalar(query, parameters);
                return result != null && Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Futbolcu silinirken hata: " + ex.Message, 
                    "Hata", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }

        public static List<Futbolcu> FutbolcuAra(string aramaMetni)
        {
            string query = "SELECT * FROM Futbolcular WHERE Ad ILIKE @Arama OR Soyad ILIKE @Arama OR Pozisyon ILIKE @Arama OR Uyruk ILIKE @Arama ORDER BY Soyad, Ad";
            string aramaParametresi = "%" + aramaMetni + "%";
            
            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@Arama", aramaParametresi)
            };

            DataTable result = DatabaseHelper.ExecuteQuery(query, parameters);
            List<Futbolcu> futbolcular = new List<Futbolcu>();

            if (result != null)
            {
                foreach (DataRow row in result.Rows)
                {
                    futbolcular.Add(CreateFutbolcuFromDataRow(row));
                }
            }

            return futbolcular;
        }

        private static Futbolcu CreateFutbolcuFromDataRow(DataRow row)
        {
            Futbolcu futbolcu = new Futbolcu
            {
                FutbolcuID = Convert.ToInt32(row["futbolcuid"]),
                Ad = row["ad"].ToString(),
                Soyad = row["soyad"].ToString(),
                DogumTarihi = Convert.ToDateTime(row["dogumtarihi"]),
                Boy = Convert.ToInt32(row["boy"]),
                Kilo = Convert.ToInt32(row["kilo"]),
                Pozisyon = row["pozisyon"].ToString(),
                FormaNo = Convert.ToInt32(row["formano"]),
                Maas = Convert.ToDecimal(row["maas"]),
                SozlesmeBaslangic = Convert.ToDateTime(row["sozlesmebaslangic"]),
                SozlesmeBitis = Convert.ToDateTime(row["sozlesmebitis"]),
                Uyruk = row["uyruk"].ToString(),
                Durumu = row["durumu"].ToString()
            };

            // Notlar null olabilir
            if (row["notlar"] != DBNull.Value)
            {
                futbolcu.Notlar = row["notlar"].ToString();
            }

            return futbolcu;
        }

        /// <summary>
        /// Forma numarasının başka bir futbolcu tarafından kullanılıp kullanılmadığını kontrol eder
        /// </summary>
        /// <param name="formaNo">Kontrol edilecek forma numarası</param>
        /// <param name="futbolcuId">Güncelleme işleminde kendi ID'sini hariç tutmak için (0 ise ekleme)</param>
        /// <returns>Kullanılıyorsa true, değilse false</returns>
        private static bool FormaNoKullaniliyor(int formaNo, int futbolcuId)
        {
            try
            {
                string query = futbolcuId > 0 
                    ? "SELECT COUNT(*) FROM Futbolcular WHERE FormaNo = @FormaNo AND FutbolcuID != @FutbolcuID"
                    : "SELECT COUNT(*) FROM Futbolcular WHERE FormaNo = @FormaNo";

                var parameters = futbolcuId > 0
                    ? new NpgsqlParameter[]
                    {
                        new NpgsqlParameter("@FormaNo", NpgsqlTypes.NpgsqlDbType.Integer) { Value = formaNo },
                        new NpgsqlParameter("@FutbolcuID", NpgsqlTypes.NpgsqlDbType.Integer) { Value = futbolcuId }
                    }
                    : new NpgsqlParameter[]
                    {
                        new NpgsqlParameter("@FormaNo", NpgsqlTypes.NpgsqlDbType.Integer) { Value = formaNo }
                    };

                object result = DatabaseHelper.ExecuteScalar(query, parameters);
                return result != null && Convert.ToInt32(result) > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
