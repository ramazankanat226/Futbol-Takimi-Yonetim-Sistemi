using System;
using FutbolTakimiYonetimSistemi.Models;

namespace FutbolTakimiYonetimSistemi.Utils
{
    /// <summary>
    /// Oturum yönetimi - Giriş yapan kullanıcının bilgilerini saklar
    /// Singleton pattern kullanır
    /// </summary>
    public sealed class SessionManager
    {
        private static SessionManager _instance;
        private static readonly object _lock = new object();

        // Oturum bilgileri
        public int KullaniciID { get; private set; }
        public string KullaniciAdi { get; private set; }
        public string AdSoyad { get; private set; }
        public string KullaniciTipi { get; private set; } // Yonetici, Antrenor, Admin
        public DateTime GirisSaati { get; private set; }
        public bool IsAuthenticated { get; private set; }

        private SessionManager()
        {
            IsAuthenticated = false;
        }

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static SessionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new SessionManager();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Yönetici girişi
        /// </summary>
        public void LoginYonetici(Yonetici yonetici)
        {
            if (yonetici == null)
            {
                throw new ArgumentNullException(nameof(yonetici));
            }

            KullaniciID = yonetici.YoneticiID;
            KullaniciAdi = yonetici.KullaniciAdi;
            AdSoyad = yonetici.TamAd;
            KullaniciTipi = yonetici.KullaniciTipi ?? "Yonetici";
            GirisSaati = DateTime.Now;
            IsAuthenticated = true;
        }

        /// <summary>
        /// Antrenör girişi
        /// </summary>
        public void LoginAntrenor(Antrenor antrenor)
        {
            if (antrenor == null)
            {
                throw new ArgumentNullException(nameof(antrenor));
            }

            KullaniciID = antrenor.AntrenorID;
            KullaniciAdi = antrenor.KullaniciAdi;
            AdSoyad = antrenor.TamAd;
            KullaniciTipi = antrenor.KullaniciTipi ?? "Antrenor";
            GirisSaati = DateTime.Now;
            IsAuthenticated = true;
        }

        /// <summary>
        /// Çıkış yap
        /// </summary>
        public void Logout()
        {
            KullaniciID = 0;
            KullaniciAdi = null;
            AdSoyad = null;
            KullaniciTipi = null;
            GirisSaati = DateTime.MinValue;
            IsAuthenticated = false;
        }

        /// <summary>
        /// Yetki kontrolü
        /// </summary>
        public bool HasPermission(string requiredRole)
        {
            if (!IsAuthenticated)
            {
                return false;
            }

            // Admin her şeyi yapabilir
            if (KullaniciTipi == "Admin")
            {
                return true;
            }

            // Belirtilen rol ile eşleşiyor mu?
            return KullaniciTipi == requiredRole;
        }

        /// <summary>
        /// Birden fazla rolden birine sahip mi?
        /// </summary>
        public bool HasAnyPermission(params string[] roles)
        {
            if (!IsAuthenticated)
            {
                return false;
            }

            if (KullaniciTipi == "Admin")
            {
                return true;
            }

            foreach (var role in roles)
            {
                if (KullaniciTipi == role)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Oturum bilgilerini string olarak döndürür
        /// </summary>
        public override string ToString()
        {
            if (!IsAuthenticated)
            {
                return "Oturum açık değil";
            }

            return $"{AdSoyad} ({KullaniciTipi}) - Giriş: {GirisSaati:dd.MM.yyyy HH:mm}";
        }
    }
}

