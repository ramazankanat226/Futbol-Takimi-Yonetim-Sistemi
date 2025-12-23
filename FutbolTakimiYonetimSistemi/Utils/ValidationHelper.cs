using System;
using System.Text.RegularExpressions;

namespace FutbolTakimiYonetimSistemi.Utils
{
    /// <summary>
    /// Veri doğrulama yardımcı sınıfı
    /// Client-side validation için
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// E-posta adresi geçerli mi?
        /// </summary>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            try
            {
                // Basit regex kontrolü
                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                return Regex.IsMatch(email, pattern);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Telefon numarası geçerli mi? (Türkiye formatı)
        /// </summary>
        public static bool IsValidPhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return false;
            }

            // Sadece rakam, boşluk, tire, parantez içerebilir
            string pattern = @"^[\d\s\-\(\)]{10,20}$";
            return Regex.IsMatch(phone, pattern);
        }

        /// <summary>
        /// Şifre güçlü mü?
        /// En az 8 karakter, 1 büyük, 1 küçük, 1 rakam
        /// </summary>
        public static bool IsStrongPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            // Minimum 8 karakter
            if (password.Length < 8)
            {
                return false;
            }

            // En az 1 büyük harf
            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                return false;
            }

            // En az 1 küçük harf
            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                return false;
            }

            // En az 1 rakam
            if (!Regex.IsMatch(password, @"[0-9]"))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Şifre kuvvet mesajı
        /// </summary>
        public static string GetPasswordStrengthMessage(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return "Şifre boş olamaz!";
            }

            if (password.Length < 8)
            {
                return "Şifre en az 8 karakter olmalıdır!";
            }

            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                return "Şifre en az 1 büyük harf içermelidir!";
            }

            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                return "Şifre en az 1 küçük harf içermelidir!";
            }

            if (!Regex.IsMatch(password, @"[0-9]"))
            {
                return "Şifre en az 1 rakam içermelidir!";
            }

            return "Şifre güçlü!";
        }

        /// <summary>
        /// Kullanıcı adı geçerli mi?
        /// </summary>
        public static bool IsValidUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return false;
            }

            // 3-50 karakter arası, sadece harf, rakam ve alt çizgi
            if (username.Length < 3 || username.Length > 50)
            {
                return false;
            }

            string pattern = @"^[a-zA-Z0-9_]+$";
            return Regex.IsMatch(username, pattern);
        }

        /// <summary>
        /// Ad veya soyad geçerli mi?
        /// </summary>
        public static bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            // 2-50 karakter arası
            if (name.Trim().Length < 2 || name.Length > 50)
            {
                return false;
            }

            // Sadece harf ve boşluk
            string pattern = @"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$";
            return Regex.IsMatch(name, pattern);
        }

        /// <summary>
        /// Sayı aralığında mı?
        /// </summary>
        public static bool IsInRange(int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Tarih geçerli mi?
        /// </summary>
        public static bool IsValidDateRange(DateTime start, DateTime end)
        {
            return start < end;
        }

        /// <summary>
        /// Gelecek tarih mi?
        /// </summary>
        public static bool IsFutureDate(DateTime date)
        {
            return date > DateTime.Now;
        }

        /// <summary>
        /// Geçmiş tarih mi?
        /// </summary>
        public static bool IsPastDate(DateTime date)
        {
            return date < DateTime.Now;
        }

        /// <summary>
        /// Yaş kontrolü
        /// </summary>
        public static bool IsValidAge(DateTime birthDate, int minAge, int maxAge)
        {
            int age = DateTime.Now.Year - birthDate.Year;
            if (birthDate.Date > DateTime.Now.AddYears(-age)) age--;

            return age >= minAge && age <= maxAge;
        }
    }
}

