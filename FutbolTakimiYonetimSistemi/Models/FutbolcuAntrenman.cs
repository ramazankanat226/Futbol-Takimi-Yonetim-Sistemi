using System;

namespace FutbolTakimiYonetimSistemi.Models
{
    public class FutbolcuAntrenman
    {
        public int FutbolcuAntrenmanID { get; set; }
        public int FutbolcuID { get; set; }
        public int AntrenmanID { get; set; }
        public bool Katilim { get; set; }
        public int Performans { get; set; }
        public string Notlar { get; set; }

        // Navigation properties
        public Futbolcu Futbolcu { get; set; }
        public Antrenman Antrenman { get; set; }

        public string PerformansText
        {
            get
            {
                return Performans > 0 ? Performans.ToString() : "Değerlendirilmedi";
            }
        }

        public string KatilimText
        {
            get
            {
                return Katilim ? "Katıldı" : "Katılmadı";
            }
        }
    }
} 