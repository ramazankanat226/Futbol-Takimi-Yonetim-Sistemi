using System;
using System.Drawing;
using System.Windows.Forms;
using FutbolTakimiYonetimSistemi.Models;
using FutbolTakimiYonetimSistemi.Services;

namespace FutbolTakimiYonetimSistemi.Forms
{
    public partial class AnaForm : Form
    {
        private Yonetici _girisYapanYonetici;
        private Panel panelHeader;
        private Panel panelKartlar;
        private Label lblToplamFutbolcu;
        private Label lblAktifFutbolcu;

        public AnaForm(Yonetici yonetici)
        {
            InitializeComponent();
            _girisYapanYonetici = yonetici;
            ModernDashboard();
        }

        private void ModernDashboard()
        {
            // Form arkaplan
            this.BackColor = ColorTranslator.FromHtml("#ECF0F1");

            // Header paneli
            panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = ColorTranslator.FromHtml("#2C3E50")
            };
            this.Controls.Add(panelHeader);

            // Hoşgeldiniz label'ını güncelle
            lblHosgeldiniz.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblHosgeldiniz.ForeColor = Color.White;
            lblHosgeldiniz.Location = new Point(30, 35);
            panelHeader.Controls.Add(lblHosgeldiniz);

            // Kartlar paneli
            panelKartlar = new Panel
            {
                Location = new Point(50, 150),
                Size = new Size(900, 400),
                BackColor = Color.Transparent
            };
            this.Controls.Add(panelKartlar);
            panelKartlar.BringToFront();

            // İstatistik kartları oluştur
            OlusturIstatistikKartlari();

            // Menü kartlarını modernleştir
            ModernKartOlustur(btnFutbolcular, "⚽ FUTBOLCU LİSTESİ", "#3498DB", 0);
            ModernKartOlustur(btnYeniFutbolcu, "+ YENİ FUTBOLCU", "#27AE60", 1);
            ModernKartOlustur(btnAntrenmanlar, "📋 ANTRENMANLAR", "#E67E22", 2);

            // Çıkış butonu
            btnCikis.BackColor = ColorTranslator.FromHtml("#E74C3C");
            btnCikis.ForeColor = Color.White;
            btnCikis.FlatStyle = FlatStyle.Flat;
            btnCikis.FlatAppearance.BorderSize = 0;
            btnCikis.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnCikis.Text = "⎋ ÇIKIŞ";
            btnCikis.Size = new Size(120, 45);
            btnCikis.Cursor = Cursors.Hand;
            btnCikis.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCikis.MouseEnter += (s, e) => btnCikis.BackColor = ColorTranslator.FromHtml("#C0392B");
            btnCikis.MouseLeave += (s, e) => btnCikis.BackColor = ColorTranslator.FromHtml("#E74C3C");

            // Kartları panele ekle
            panelKartlar.Controls.Add(btnFutbolcular);
            panelKartlar.Controls.Add(btnYeniFutbolcu);
            panelKartlar.Controls.Add(btnAntrenmanlar);
        }

        private void OlusturIstatistikKartlari()
        {
            try
            {
                // Toplam futbolcu sayısı
                var tumFutbolcular = FutbolcuService.GetAllFutbolcular();
                int toplamFutbolcu = tumFutbolcular.Count;
                int aktifFutbolcu = tumFutbolcular.FindAll(f => f.Durumu == "Aktif").Count;

                // İstatistik kartı 1
                Panel kartToplam = new Panel
                {
                    Size = new Size(200, 80),
                    Location = new Point(500, 10),
                    BackColor = ColorTranslator.FromHtml("#9B59B6")
                };
                panelHeader.Controls.Add(kartToplam);

                lblToplamFutbolcu = new Label
                {
                    Text = toplamFutbolcu.ToString(),
                    Font = new Font("Segoe UI", 32, FontStyle.Bold),
                    ForeColor = Color.White,
                    Size = new Size(200, 45),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(0, 5)
                };
                kartToplam.Controls.Add(lblToplamFutbolcu);

                Label lblToplamText = new Label
                {
                    Text = "Toplam Futbolcu",
                    Font = new Font("Segoe UI", 10, FontStyle.Regular),
                    ForeColor = Color.White,
                    Size = new Size(200, 25),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(0, 50)
                };
                kartToplam.Controls.Add(lblToplamText);

                // İstatistik kartı 2
                Panel kartAktif = new Panel
                {
                    Size = new Size(200, 80),
                    Location = new Point(720, 10),
                    BackColor = ColorTranslator.FromHtml("#1ABC9C")
                };
                panelHeader.Controls.Add(kartAktif);

                lblAktifFutbolcu = new Label
                {
                    Text = aktifFutbolcu.ToString(),
                    Font = new Font("Segoe UI", 32, FontStyle.Bold),
                    ForeColor = Color.White,
                    Size = new Size(200, 45),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(0, 5)
                };
                kartAktif.Controls.Add(lblAktifFutbolcu);

                Label lblAktifText = new Label
                {
                    Text = "Aktif Futbolcu",
                    Font = new Font("Segoe UI", 10, FontStyle.Regular),
                    ForeColor = Color.White,
                    Size = new Size(200, 25),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(0, 50)
                };
                kartAktif.Controls.Add(lblAktifText);
            }
            catch
            {
                // Hata durumunda sessizce geç
            }
        }

        private void ModernKartOlustur(Button btn, string text, string renkKodu, int pozisyon)
        {
            btn.Text = text;
            btn.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            btn.Size = new Size(270, 150);
            btn.Location = new Point(pozisyon * 300, 20);
            btn.BackColor = ColorTranslator.FromHtml(renkKodu);
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.TextAlign = ContentAlignment.MiddleCenter;

            // Hover efekti
            Color hoverColor = ControlPaint.Dark(ColorTranslator.FromHtml(renkKodu), 0.1f);
            btn.MouseEnter += (s, e) => 
            {
                btn.BackColor = hoverColor;
                btn.Font = new Font("Segoe UI", 15, FontStyle.Bold);
            };
            btn.MouseLeave += (s, e) => 
            {
                btn.BackColor = ColorTranslator.FromHtml(renkKodu);
                btn.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            };
        }

        private void AnaForm_Load(object sender, EventArgs e)
        {
            lblHosgeldiniz.Text = $"⚽ Hoş Geldiniz, {_girisYapanYonetici.TamAd}";
            this.Text = $"Futbol Takımı Yönetim Sistemi - {_girisYapanYonetici.KullaniciAdi}";

            // Antrenör ise bazı butonları gizle
            if (_girisYapanYonetici.KullaniciTipi == "Antrenor")
            {
                btnYeniFutbolcu.Enabled = false;
                btnYeniFutbolcu.BackColor = ColorTranslator.FromHtml("#95A5A6");
                btnYeniFutbolcu.Text = "🔒 YETKİ YOK";
                btnYeniFutbolcu.Cursor = Cursors.No;
            }
        }

        private void btnFutbolcular_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullanıcı tipine göre salt okunur modda aç
                bool saltOkunur = (_girisYapanYonetici.KullaniciTipi == "Antrenor");
                FutbolcuListesiForm futbolcuListesiForm = new FutbolcuListesiForm(saltOkunur);
                futbolcuListesiForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Futbolcu listesi açılırken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnYeniFutbolcu_Click(object sender, EventArgs e)
        {
            try
            {
                FutbolcuEditForm futbolcuEditForm = new FutbolcuEditForm();
                futbolcuEditForm.ShowDialog();
                
                // Eğer futbolcu listesi açıksa yenile
                foreach (Form form in Application.OpenForms)
                {
                    if (form is FutbolcuListesiForm)
                    {
                        ((FutbolcuListesiForm)form).YenileFutbolcuListesi();
                        break;
                    }
                }

                // İstatistikleri güncelle
                OlusturIstatistikKartlari();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Yeni futbolcu formu açılırken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAntrenmanlar_Click(object sender, EventArgs e)
        {
            try
            {
                AntrenmanListesiForm antrenmanListesiForm = new AntrenmanListesiForm();
                antrenmanListesiForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Antrenman listesi açılırken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Çıkmak istediğinize emin misiniz?", "Çıkış", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void InitializeComponent()
        {
            this.lblHosgeldiniz = new System.Windows.Forms.Label();
            this.btnFutbolcular = new System.Windows.Forms.Button();
            this.btnYeniFutbolcu = new System.Windows.Forms.Button();
            this.btnAntrenmanlar = new System.Windows.Forms.Button();
            this.btnCikis = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblHosgeldiniz
            // 
            this.lblHosgeldiniz.AutoSize = true;
            this.lblHosgeldiniz.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblHosgeldiniz.Location = new System.Drawing.Point(24, 23);
            this.lblHosgeldiniz.Name = "lblHosgeldiniz";
            this.lblHosgeldiniz.Size = new System.Drawing.Size(126, 25);
            this.lblHosgeldiniz.TabIndex = 0;
            this.lblHosgeldiniz.Text = "Hoş Geldiniz";
            // 
            // btnFutbolcular
            // 
            this.btnFutbolcular.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnFutbolcular.Location = new System.Drawing.Point(150, 120);
            this.btnFutbolcular.Name = "btnFutbolcular";
            this.btnFutbolcular.Size = new System.Drawing.Size(250, 100);
            this.btnFutbolcular.TabIndex = 1;
            this.btnFutbolcular.Text = "Futbolcu Listesi";
            this.btnFutbolcular.UseVisualStyleBackColor = true;
            this.btnFutbolcular.Click += new System.EventHandler(this.btnFutbolcular_Click);
            // 
            // btnYeniFutbolcu
            // 
            this.btnYeniFutbolcu.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnYeniFutbolcu.Location = new System.Drawing.Point(450, 120);
            this.btnYeniFutbolcu.Name = "btnYeniFutbolcu";
            this.btnYeniFutbolcu.Size = new System.Drawing.Size(250, 100);
            this.btnYeniFutbolcu.TabIndex = 2;
            this.btnYeniFutbolcu.Text = "Yeni Futbolcu Ekle";
            this.btnYeniFutbolcu.UseVisualStyleBackColor = true;
            this.btnYeniFutbolcu.Click += new System.EventHandler(this.btnYeniFutbolcu_Click);
            // 
            // btnAntrenmanlar
            // 
            this.btnAntrenmanlar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnAntrenmanlar.Location = new System.Drawing.Point(300, 250);
            this.btnAntrenmanlar.Name = "btnAntrenmanlar";
            this.btnAntrenmanlar.Size = new System.Drawing.Size(250, 100);
            this.btnAntrenmanlar.TabIndex = 3;
            this.btnAntrenmanlar.Text = "Antrenman Programı";
            this.btnAntrenmanlar.UseVisualStyleBackColor = true;
            this.btnAntrenmanlar.Click += new System.EventHandler(this.btnAntrenmanlar_Click);
            // 
            // btnCikis
            // 
            this.btnCikis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCikis.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnCikis.Location = new System.Drawing.Point(1050, 650);
            this.btnCikis.Name = "btnCikis";
            this.btnCikis.Size = new System.Drawing.Size(120, 40);
            this.btnCikis.TabIndex = 4;
            this.btnCikis.Text = "Çıkış";
            this.btnCikis.UseVisualStyleBackColor = true;
            this.btnCikis.Click += new System.EventHandler(this.btnCikis_Click);
            // 
            // AnaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 720);
            this.Controls.Add(this.btnCikis);
            this.Controls.Add(this.btnAntrenmanlar);
            this.Controls.Add(this.btnYeniFutbolcu);
            this.Controls.Add(this.btnFutbolcular);
            this.Controls.Add(this.lblHosgeldiniz);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.MaximizeBox = true;
            this.Name = "AnaForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Futbol Takımı Yönetim Sistemi";
            this.Load += new System.EventHandler(this.AnaForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblHosgeldiniz;
        private System.Windows.Forms.Button btnFutbolcular;
        private System.Windows.Forms.Button btnYeniFutbolcu;
        private System.Windows.Forms.Button btnAntrenmanlar;
        private System.Windows.Forms.Button btnCikis;
    }
} 
