using System;
using System.Drawing;
using System.Windows.Forms;
using FutbolTakimiYonetimSistemi.Models;
using FutbolTakimiYonetimSistemi.Services;

namespace FutbolTakimiYonetimSistemi.Forms
{
    public partial class GirisForm : Form
    {
        private Panel panelLogin;
        private Label lblBaslik;
        private Label lblAltBaslik;
        private RadioButton rbYonetici;
        private RadioButton rbAntrenor;
        private Label lblKullaniciTipi;

        public GirisForm()
        {
            InitializeComponent();
            ModernTasarim();
        }

        private void ModernTasarim()
        {
            // Form arkaplan rengi - koyu tema
            this.BackColor = ColorTranslator.FromHtml("#2C3E50");
            
            // Login paneli oluştur
            panelLogin = new Panel
            {
                Size = new Size(400, 470),
                Location = new Point((this.ClientSize.Width - 400) / 2, (this.ClientSize.Height - 470) / 2),
                BackColor = Color.White,
                Padding = new Padding(40)
            };
            this.Controls.Add(panelLogin);
            panelLogin.BringToFront();

            // Başlık
            lblBaslik = new Label
            {
                Text = "⚽ FUTBOL TAKIMI",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#2C3E50"),
                AutoSize = true,
                Location = new Point(70, 40)
            };
            panelLogin.Controls.Add(lblBaslik);

            // Alt başlık
            lblAltBaslik = new Label
            {
                Text = "Yönetim Sistemi",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = ColorTranslator.FromHtml("#7F8C8D"),
                AutoSize = true,
                Location = new Point(130, 75)
            };
            panelLogin.Controls.Add(lblAltBaslik);

            // Kullanıcı Tipi Label ve RadioButton'lar
            lblKullaniciTipi = new Label
            {
                Text = "Giriş Tipi:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#2C3E50"),
                Location = new Point(40, 110),
                AutoSize = true
            };
            panelLogin.Controls.Add(lblKullaniciTipi);

            rbYonetici = new RadioButton
            {
                Text = "👔 Yönetici",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = ColorTranslator.FromHtml("#2C3E50"),
                Location = new Point(140, 108),
                AutoSize = true,
                Checked = true
            };
            panelLogin.Controls.Add(rbYonetici);

            rbAntrenor = new RadioButton
            {
                Text = "🏃 Antrenör",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = ColorTranslator.FromHtml("#2C3E50"),
                Location = new Point(250, 108),
                AutoSize = true
            };
            panelLogin.Controls.Add(rbAntrenor);

            // Label'ları güncelle
            lblKullaniciAdi.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblKullaniciAdi.ForeColor = ColorTranslator.FromHtml("#2C3E50");
            lblKullaniciAdi.Location = new Point(40, 150);

            lblSifre.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblSifre.ForeColor = ColorTranslator.FromHtml("#2C3E50");
            lblSifre.Location = new Point(40, 220);

            // TextBox'ları büyüt ve güzelleştir
            txtKullaniciAdi.Font = new Font("Segoe UI", 11);
            txtKullaniciAdi.Size = new Size(320, 35);
            txtKullaniciAdi.Location = new Point(40, 175);
            txtKullaniciAdi.BorderStyle = BorderStyle.FixedSingle;

            txtSifre.Font = new Font("Segoe UI", 11);
            txtSifre.Size = new Size(320, 35);
            txtSifre.Location = new Point(40, 245);
            txtSifre.BorderStyle = BorderStyle.FixedSingle;

            // Giriş butonu - yeşil tema
            btnGiris.Text = "GİRİŞ YAP";
            btnGiris.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnGiris.Size = new Size(320, 45);
            btnGiris.Location = new Point(40, 310);
            btnGiris.BackColor = ColorTranslator.FromHtml("#27AE60");
            btnGiris.ForeColor = Color.White;
            btnGiris.FlatStyle = FlatStyle.Flat;
            btnGiris.FlatAppearance.BorderSize = 0;
            btnGiris.Cursor = Cursors.Hand;
            btnGiris.MouseEnter += (s, e) => btnGiris.BackColor = ColorTranslator.FromHtml("#2ECC71");
            btnGiris.MouseLeave += (s, e) => btnGiris.BackColor = ColorTranslator.FromHtml("#27AE60");

            // İptal butonu - gri tema
            btnIptal.Text = "ÇIKIŞ";
            btnIptal.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            btnIptal.Size = new Size(320, 40);
            btnIptal.Location = new Point(40, 365);
            btnIptal.BackColor = ColorTranslator.FromHtml("#95A5A6");
            btnIptal.ForeColor = Color.White;
            btnIptal.FlatStyle = FlatStyle.Flat;
            btnIptal.FlatAppearance.BorderSize = 0;
            btnIptal.Cursor = Cursors.Hand;
            btnIptal.MouseEnter += (s, e) => btnIptal.BackColor = ColorTranslator.FromHtml("#7F8C8D");
            btnIptal.MouseLeave += (s, e) => btnIptal.BackColor = ColorTranslator.FromHtml("#95A5A6");

            // Label ve TextBox'ları panelin içine taşı
            panelLogin.Controls.Add(lblKullaniciAdi);
            panelLogin.Controls.Add(lblSifre);
            panelLogin.Controls.Add(txtKullaniciAdi);
            panelLogin.Controls.Add(txtSifre);
            panelLogin.Controls.Add(btnGiris);
            panelLogin.Controls.Add(btnIptal);
        }

        private void GirisForm_Load(object sender, EventArgs e)
        {
            // Veritabanı bağlantısını kontrol et
            if (!Data.DatabaseHelper.VerifyDatabaseConnection())
            {
                MessageBox.Show("PostgreSQL veritabanına bağlanılamadı!\n\nLütfen kontrol edin:\n• PostgreSQL servisi çalışıyor mu?\n• Veritabanı 'futbol_takimi_db' oluşturuldu mu?\n• App.config'de bağlantı bilgileri doğru mu?", 
                    "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Focus
            txtKullaniciAdi.Focus();
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = txtKullaniciAdi.Text.Trim();
            string sifre = txtSifre.Text;

            if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(sifre))
            {
                MessageBox.Show("Kullanıcı adı ve şifre giriniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Buton animasyonu
            btnGiris.Text = "Giriş yapılıyor...";
            btnGiris.Enabled = false;
            Application.DoEvents();

            // Seçilen kullanıcı tipine göre giriş yap
            if (rbYonetici.Checked)
            {
                // Yönetici girişi
                Yonetici yonetici = YoneticiService.Giris(kullaniciAdi, sifre);

                if (yonetici != null)
                {
                    this.Hide();
                    AnaForm anaForm = new AnaForm(yonetici);
                    anaForm.FormClosed += (s, args) => this.Close();
                    anaForm.Show();
                    return;
                }
            }
            else if (rbAntrenor.Checked)
            {
                // Antrenör girişi
                Antrenor antrenor = AntrenorService.Giris(kullaniciAdi, sifre);

                if (antrenor != null)
                {
                    this.Hide();
                    // Antrenör için de AnaForm kullan (geçici)
                    AnaForm anaForm = new AnaForm(new Yonetici 
                    { 
                        YoneticiID = antrenor.AntrenorID, 
                        KullaniciAdi = antrenor.KullaniciAdi,
                        Ad = antrenor.Ad,
                        Soyad = antrenor.Soyad,
                        KullaniciTipi = "Antrenor"
                    });
                    anaForm.FormClosed += (s, args) => this.Close();
                    anaForm.Show();
                    return;
                }
            }

            // Giriş başarısız
            btnGiris.Text = "GİRİŞ YAP";
            btnGiris.Enabled = true;
            MessageBox.Show("Kullanıcı adı veya şifre yanlış!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            txtSifre.Clear();
            txtSifre.Focus();
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void InitializeComponent()
        {
            this.lblKullaniciAdi = new System.Windows.Forms.Label();
            this.lblSifre = new System.Windows.Forms.Label();
            this.txtKullaniciAdi = new System.Windows.Forms.TextBox();
            this.txtSifre = new System.Windows.Forms.TextBox();
            this.btnGiris = new System.Windows.Forms.Button();
            this.btnIptal = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblKullaniciAdi
            // 
            this.lblKullaniciAdi.AutoSize = true;
            this.lblKullaniciAdi.Location = new System.Drawing.Point(47, 45);
            this.lblKullaniciAdi.Name = "lblKullaniciAdi";
            this.lblKullaniciAdi.Size = new System.Drawing.Size(84, 16);
            this.lblKullaniciAdi.TabIndex = 0;
            this.lblKullaniciAdi.Text = "Kullanıcı Adı:";
            // 
            // lblSifre
            // 
            this.lblSifre.AutoSize = true;
            this.lblSifre.Location = new System.Drawing.Point(47, 87);
            this.lblSifre.Name = "lblSifre";
            this.lblSifre.Size = new System.Drawing.Size(37, 16);
            this.lblSifre.TabIndex = 1;
            this.lblSifre.Text = "Şifre:";
            // 
            // txtKullaniciAdi
            // 
            this.txtKullaniciAdi.Location = new System.Drawing.Point(146, 42);
            this.txtKullaniciAdi.Name = "txtKullaniciAdi";
            this.txtKullaniciAdi.Size = new System.Drawing.Size(179, 22);
            this.txtKullaniciAdi.TabIndex = 1;
            // 
            // txtSifre
            // 
            this.txtSifre.Location = new System.Drawing.Point(146, 84);
            this.txtSifre.Name = "txtSifre";
            this.txtSifre.PasswordChar = '●';
            this.txtSifre.Size = new System.Drawing.Size(179, 22);
            this.txtSifre.TabIndex = 2;
            // 
            // btnGiris
            // 
            this.btnGiris.Location = new System.Drawing.Point(146, 136);
            this.btnGiris.Name = "btnGiris";
            this.btnGiris.Size = new System.Drawing.Size(84, 28);
            this.btnGiris.TabIndex = 3;
            this.btnGiris.Text = "Giriş";
            this.btnGiris.UseVisualStyleBackColor = true;
            this.btnGiris.Click += new System.EventHandler(this.btnGiris_Click);
            // 
            // btnIptal
            // 
            this.btnIptal.Location = new System.Drawing.Point(241, 136);
            this.btnIptal.Name = "btnIptal";
            this.btnIptal.Size = new System.Drawing.Size(84, 28);
            this.btnIptal.TabIndex = 4;
            this.btnIptal.Text = "İptal";
            this.btnIptal.UseVisualStyleBackColor = true;
            this.btnIptal.Click += new System.EventHandler(this.btnIptal_Click);
            // 
            // GirisForm
            // 
            this.AcceptButton = this.btnGiris;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 600);
            this.Controls.Add(this.btnIptal);
            this.Controls.Add(this.btnGiris);
            this.Controls.Add(this.txtSifre);
            this.Controls.Add(this.txtKullaniciAdi);
            this.Controls.Add(this.lblSifre);
            this.Controls.Add(this.lblKullaniciAdi);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GirisForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Futbol Takımı Yönetim Sistemi - Giriş";
            this.Load += new System.EventHandler(this.GirisForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblKullaniciAdi;
        private System.Windows.Forms.Label lblSifre;
        private System.Windows.Forms.TextBox txtKullaniciAdi;
        private System.Windows.Forms.TextBox txtSifre;
        private System.Windows.Forms.Button btnGiris;
        private System.Windows.Forms.Button btnIptal;
    }
} 
