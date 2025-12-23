using System;
using System.Drawing;
using System.Windows.Forms;

namespace FutbolTakimiYonetimSistemi.Utils
{
    /// <summary>
    /// Form ve kontrollerin modern görünümünü sağlayan yardımcı sınıf
    /// </summary>
    public static class FormStilleri
    {
        /// <summary>
        /// Kullanılan renkler
        /// </summary>
        public static class Renkler
        {
            public static readonly Color AnaArkaplan = Color.FromArgb(240, 244, 248);
            public static readonly Color AcikArkaplan = Color.FromArgb(248, 250, 252);
            public static readonly Color KartArkaplan = Color.White;
            
            public static readonly Color Baslik = Color.FromArgb(30, 41, 59);
            public static readonly Color AltBaslik = Color.FromArgb(71, 85, 105);
            public static readonly Color Yazi = Color.FromArgb(51, 65, 85);
            
            public static readonly Color Mavi = Color.FromArgb(59, 130, 246);
            public static readonly Color MaviHover = Color.FromArgb(37, 99, 235);
            
            public static readonly Color Yesil = Color.FromArgb(34, 197, 94);
            public static readonly Color YesilHover = Color.FromArgb(22, 163, 74);
            
            public static readonly Color Turuncu = Color.FromArgb(251, 146, 60);
            public static readonly Color TuruncuHover = Color.FromArgb(249, 115, 22);
            
            public static readonly Color Kirmizi = Color.FromArgb(239, 68, 68);
            public static readonly Color KirmiziHover = Color.FromArgb(220, 38, 38);
            
            public static readonly Color Gri = Color.FromArgb(148, 163, 184);
            public static readonly Color GriHover = Color.FromArgb(100, 116, 139);
            
            public static readonly Color Kenarlık = Color.FromArgb(226, 232, 240);
        }

        /// <summary>
        /// Form için modern stil uygula
        /// </summary>
        public static void ModernForm(Form form)
        {
            form.BackColor = Renkler.AnaArkaplan;
            form.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
        }

        /// <summary>
        /// DataGridView için modern stil
        /// </summary>
        public static void ModernDataGridView(DataGridView dgv)
        {
            // Genel ayarlar
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.ReadOnly = true;
            dgv.EnableHeadersVisualStyles = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Satır ayarları
            dgv.RowTemplate.Height = 40;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dgv.DefaultCellStyle.SelectionForeColor = Renkler.Baslik;
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Renkler.Yazi;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgv.DefaultCellStyle.Padding = new Padding(10, 5, 10, 5);

            // Alternatif satır rengi
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);

            // Başlık ayarları
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Renkler.Baslik;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 8, 10, 8);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersHeight = 45;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // Kenarlık efekti
            dgv.GridColor = Renkler.Kenarlık;
        }

        /// <summary>
        /// Modern buton stili
        /// </summary>
        public static void ModernButon(Button btn, string renk = "mavi")
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.Height = 40;
            btn.ForeColor = Color.White;

            Color normalRenk, hoverRenk;

            switch (renk.ToLower())
            {
                case "yesil":
                    normalRenk = Renkler.Yesil;
                    hoverRenk = Renkler.YesilHover;
                    break;
                case "kirmizi":
                    normalRenk = Renkler.Kirmizi;
                    hoverRenk = Renkler.KirmiziHover;
                    break;
                case "turuncu":
                    normalRenk = Renkler.Turuncu;
                    hoverRenk = Renkler.TuruncuHover;
                    break;
                case "gri":
                    normalRenk = Renkler.Gri;
                    hoverRenk = Renkler.GriHover;
                    break;
                default: // mavi
                    normalRenk = Renkler.Mavi;
                    hoverRenk = Renkler.MaviHover;
                    break;
            }

            btn.BackColor = normalRenk;

            // Hover efekti
            btn.MouseEnter += (s, e) => btn.BackColor = hoverRenk;
            btn.MouseLeave += (s, e) => btn.BackColor = normalRenk;
        }

        /// <summary>
        /// Modern TextBox stili
        /// </summary>
        public static void ModernTextBox(TextBox txt)
        {
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.Font = new Font("Segoe UI", 10F);
            txt.Height = 35;
            txt.BackColor = Color.White;
            txt.ForeColor = Renkler.Yazi;
        }

        /// <summary>
        /// Modern Label stili
        /// </summary>
        public static void ModernLabel(Label lbl, bool baslik = false)
        {
            if (baslik)
            {
                lbl.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                lbl.ForeColor = Renkler.Baslik;
            }
            else
            {
                lbl.Font = new Font("Segoe UI", 9.5F);
                lbl.ForeColor = Renkler.AltBaslik;
            }
        }

        /// <summary>
        /// Satır durumuna göre renklendirme
        /// </summary>
        public static void RenklendirilmisSatir(DataGridViewRow row, string durum)
        {
            if (row == null) return;

            switch (durum?.ToUpper())
            {
                case "AKTİF":
                case "AKTIF":
                    row.DefaultCellStyle.ForeColor = Renkler.Yesil;
                    break;
                case "SAKAT":
                    row.DefaultCellStyle.ForeColor = Renkler.Turuncu;
                    break;
                case "CEZALI":
                case "PASİF":
                case "PASIF":
                    row.DefaultCellStyle.ForeColor = Renkler.Kirmizi;
                    break;
                default:
                    row.DefaultCellStyle.ForeColor = Renkler.Yazi;
                    break;
            }
        }

        /// <summary>
        /// Modern Panel stili
        /// </summary>
        public static void ModernPanel(Panel pnl, bool kartStili = true)
        {
            if (kartStili)
            {
                pnl.BackColor = Renkler.KartArkaplan;
                pnl.BorderStyle = BorderStyle.None;
                pnl.Padding = new Padding(15);
            }
            else
            {
                pnl.BackColor = Renkler.AcikArkaplan;
                pnl.BorderStyle = BorderStyle.None;
            }
        }

        /// <summary>
        /// Modern ComboBox stili
        /// </summary>
        public static void ModernComboBox(ComboBox cmb)
        {
            cmb.FlatStyle = FlatStyle.Flat;
            cmb.Font = new Font("Segoe UI", 9.5F);
            cmb.BackColor = Color.White;
            cmb.ForeColor = Renkler.Yazi;
            cmb.Height = 35;
        }

        /// <summary>
        /// Modern DateTimePicker stili
        /// </summary>
        public static void ModernDateTimePicker(DateTimePicker dtp)
        {
            dtp.Font = new Font("Segoe UI", 9.5F);
            dtp.CalendarForeColor = Renkler.Yazi;
            dtp.CalendarTitleBackColor = Renkler.Mavi;
            // Eğer özel format ayarlı değilse kısa tarih kullan
            if (dtp.Format != DateTimePickerFormat.Custom)
            {
                dtp.Format = DateTimePickerFormat.Short;
            }
        }

        /// <summary>
        /// Modern NumericUpDown stili
        /// </summary>
        public static void ModernNumericUpDown(NumericUpDown nud)
        {
            nud.BorderStyle = BorderStyle.FixedSingle;
            nud.Font = new Font("Segoe UI", 9.5F);
            nud.BackColor = Color.White;
            nud.ForeColor = Renkler.Yazi;
        }
    }
}


