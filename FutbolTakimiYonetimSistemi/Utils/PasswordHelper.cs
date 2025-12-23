using System;
using BCrypt.Net;

namespace FutbolTakimiYonetimSistemi.Utils
{
    /// <summary>
    /// Şifre hashleme ve doğrulama işlemleri için yardımcı sınıf
    /// BCrypt algoritması kullanır (güvenli ve yavaş - brute force saldırılarına karşı dirençli)
    /// </summary>
    public static class PasswordHelper
    {
        /// <summary>
        /// Düz metin şifreyi BCrypt ile hashler
        /// </summary>
        /// <param name="password">Hashlenecek şifre</param>
        /// <returns>Hashlenmiş şifre (60 karakter)</returns>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Şifre boş olamaz!", nameof(password));
            }

            // BCrypt.Net ile hash (WorkFactor: 11 = 2^11 iterasyon)
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 11);
        }

        /// <summary>
        /// Düz metin şifreyi hash ile karşılaştırır
        /// </summary>
        /// <param name="password">Kontrol edilecek düz metin şifre</param>
        /// <param name="hash">Veritabanındaki hash</param>
        /// <returns>Eşleşiyorsa true</returns>
        public static bool VerifyPassword(string password, string hash)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(hash))
            {
                return false;
            }

            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch
            {
                // Hash formatı hatalıysa (eski düz metin şifreler)
                return false;
            }
        }

        /// <summary>
        /// Düz metin şifreyi kontrol eder ve hash'lenmiş versiyonunu döndürür
        /// İlk giriş sırasında otomatik hash'leme için kullanılır
        /// </summary>
        public static (bool isValid, string newHash) MigrateOldPassword(string password, string storedValue)
        {
            // Önce BCrypt hash olup olmadığını kontrol et
            if (storedValue.StartsWith("$2a$") || storedValue.StartsWith("$2b$"))
            {
                // Zaten hash'lenmiş
                return (VerifyPassword(password, storedValue), storedValue);
            }

            // Düz metin karşılaştırma (GEÇİCİ - üretimde olmamalı!)
            if (password == storedValue)
            {
                // Şifreyi hash'le ve yeni hash'i döndür
                string newHash = HashPassword(password);
                return (true, newHash);
            }

            return (false, null);
        }
    }
}

