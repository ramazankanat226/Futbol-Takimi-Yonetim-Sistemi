using System;
using System.Windows.Forms;
using FutbolTakimiYonetimSistemi.Models;

namespace FutbolTakimiYonetimSistemi.Forms
{
    public partial class AnaForm : Form
    {
        private Yonetici _girisYapanYonetici;

        public AnaForm(Yonetici yonetici)
        {
            InitializeComponent();
            _girisYapanYonetici = yonetici;
        }

        private void AnaForm_Load(object sender, EventArgs e)
        {
            lblHosgeldiniz.Text = $"Hoş Geldiniz, {_girisYapanYonetici.TamAd}";
            this.Text = $"Futbol Takımı Yönetim Sistemi - {_girisYapanYonetici.KullaniciAdi}";
        }

        private void btnFutbolcular_Click(object sender, EventArgs e)
        {
            try
            {
                FutbolcuListesiForm futbolcuListesiForm = new FutbolcuListesiForm();
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
            this.Close();
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
            this.btnFutbolcular.Anchor = System.Windows.Forms.AnchorStyles.None;
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
            this.btnYeniFutbolcu.Anchor = System.Windows.Forms.AnchorStyles.None;
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
            this.btnAntrenmanlar.Anchor = System.Windows.Forms.AnchorStyles.None;
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
            this.btnCikis.Location = new System.Drawing.Point(700, 390);
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
            this.ClientSize = new System.Drawing.Size(850, 450);
            this.Controls.Add(this.btnCikis);
            this.Controls.Add(this.btnAntrenmanlar);
            this.Controls.Add(this.btnYeniFutbolcu);
            this.Controls.Add(this.btnFutbolcular);
            this.Controls.Add(this.lblHosgeldiniz);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MinimumSize = new System.Drawing.Size(800, 450);
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