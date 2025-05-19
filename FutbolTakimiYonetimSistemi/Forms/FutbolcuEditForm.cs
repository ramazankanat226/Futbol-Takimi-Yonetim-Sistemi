using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FutbolTakimiYonetimSistemi.Models;
using FutbolTakimiYonetimSistemi.Services;

namespace FutbolTakimiYonetimSistemi.Forms
{
    public partial class FutbolcuEditForm : Form
    {
        private int _futbolcuId = 0;
        private Futbolcu _futbolcu = null;
        private bool _isEditing = false;

        // Pozisyon listesi
        private readonly string[] _pozisyonlar = new string[]
        {
            "Kaleci",
            "Stoper",
            "Sol Bek",
            "Sağ Bek",
            "Defansif Orta Saha",
            "Merkez Orta Saha",
            "Ofansif Orta Saha",
            "Sol Kanat",
            "Sağ Kanat",
            "Forvet"
        };

        // Durum listesi
        private readonly string[] _durumlar = new string[]
        {
            "Aktif",
            "Sakat",
            "Cezalı"
        };

        public FutbolcuEditForm()
        {
            InitializeComponent();
            _isEditing = false;
        }

        public FutbolcuEditForm(int futbolcuId)
        {
            InitializeComponent();
            _futbolcuId = futbolcuId;
            _isEditing = true;
        }

        private void FutbolcuEditForm_Load(object sender, EventArgs e)
        {
            // Combobox'ları doldur
            cmbPozisyon.Items.AddRange(_pozisyonlar);
            cmbDurumu.Items.AddRange(_durumlar);

            if (_isEditing)
            {
                this.Text = "Futbolcu Düzenle";
                btnKaydet.Text = "Güncelle";
                
                // Futbolcu bilgilerini getir
                _futbolcu = FutbolcuService.GetById(_futbolcuId);
                if (_futbolcu != null)
                {
                    FutbolcuVerileriniDoldur();
                }
                else
                {
                    MessageBox.Show("Futbolcu bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            else
            {
                this.Text = "Yeni Futbolcu Ekle";
                btnKaydet.Text = "Kaydet";
                
                // Varsayılan değerler
                dtpDogumTarihi.Value = DateTime.Now.AddYears(-20);
                dtpSozlesmeBaslangic.Value = DateTime.Now;
                dtpSozlesmeBitis.Value = DateTime.Now.AddYears(3);
                nudBoy.Value = 180;
                nudKilo.Value = 75;
                nudFormaNo.Value = 1;
                nudMaas.Value = 10000;
                cmbPozisyon.SelectedIndex = 0;
                cmbDurumu.SelectedIndex = 0;
            }
        }

        private void FutbolcuVerileriniDoldur()
        {
            txtAd.Text = _futbolcu.Ad;
            txtSoyad.Text = _futbolcu.Soyad;
            dtpDogumTarihi.Value = _futbolcu.DogumTarihi;
            nudBoy.Value = _futbolcu.Boy;
            nudKilo.Value = _futbolcu.Kilo;
            
            // Pozisyon seç
            int pozisyonIndex = Array.IndexOf(_pozisyonlar, _futbolcu.Pozisyon);
            cmbPozisyon.SelectedIndex = pozisyonIndex >= 0 ? pozisyonIndex : 0;
            
            nudFormaNo.Value = _futbolcu.FormaNo;
            nudMaas.Value = _futbolcu.Maas;
            dtpSozlesmeBaslangic.Value = _futbolcu.SozlesmeBaslangic;
            dtpSozlesmeBitis.Value = _futbolcu.SozlesmeBitis;
            txtUyruk.Text = _futbolcu.Uyruk;
            
            // Durum seç
            int durumIndex = Array.IndexOf(_durumlar, _futbolcu.Durumu);
            cmbDurumu.SelectedIndex = durumIndex >= 0 ? durumIndex : 0;
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {
                Futbolcu futbolcu = new Futbolcu
                {
                    Ad = txtAd.Text.Trim(),
                    Soyad = txtSoyad.Text.Trim(),
                    DogumTarihi = dtpDogumTarihi.Value,
                    Boy = (int)nudBoy.Value,
                    Kilo = (int)nudKilo.Value,
                    Pozisyon = cmbPozisyon.SelectedItem.ToString(),
                    FormaNo = (int)nudFormaNo.Value,
                    Maas = nudMaas.Value,
                    SozlesmeBaslangic = dtpSozlesmeBaslangic.Value,
                    SozlesmeBitis = dtpSozlesmeBitis.Value,
                    Uyruk = txtUyruk.Text.Trim(),
                    Durumu = cmbDurumu.SelectedItem.ToString()
                };

                bool sonuc;
                if (_isEditing)
                {
                    futbolcu.FutbolcuID = _futbolcuId;
                    sonuc = FutbolcuService.GuncelleFutbolcu(futbolcu);
                    if (sonuc)
                    {
                        MessageBox.Show("Futbolcu başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                }
                else
                {
                    sonuc = FutbolcuService.EkleFutbolcu(futbolcu);
                    if (sonuc)
                    {
                        MessageBox.Show("Futbolcu başarıyla eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (chkKaydetveKapat.Checked)
                        {
                            this.Close();
                        }
                        else
                        {
                            // Formu sıfırla
                            ClearInputs();
                        }
                    }
                }
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtAd.Text))
            {
                MessageBox.Show("Lütfen futbolcunun adını giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAd.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSoyad.Text))
            {
                MessageBox.Show("Lütfen futbolcunun soyadını giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoyad.Focus();
                return false;
            }

            if (dtpDogumTarihi.Value > DateTime.Now)
            {
                MessageBox.Show("Doğum tarihi gelecekte olamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpDogumTarihi.Focus();
                return false;
            }

            if (dtpSozlesmeBaslangic.Value > dtpSozlesmeBitis.Value)
            {
                MessageBox.Show("Sözleşme başlangıç tarihi bitiş tarihinden sonra olamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpSozlesmeBaslangic.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtUyruk.Text))
            {
                MessageBox.Show("Lütfen futbolcunun uyruğunu giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUyruk.Focus();
                return false;
            }

            if (cmbPozisyon.SelectedIndex < 0)
            {
                MessageBox.Show("Lütfen bir pozisyon seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbPozisyon.Focus();
                return false;
            }

            if (cmbDurumu.SelectedIndex < 0)
            {
                MessageBox.Show("Lütfen futbolcunun durumunu seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbDurumu.Focus();
                return false;
            }

            return true;
        }

        private void ClearInputs()
        {
            txtAd.Clear();
            txtSoyad.Clear();
            dtpDogumTarihi.Value = DateTime.Now.AddYears(-20);
            nudBoy.Value = 180;
            nudKilo.Value = 75;
            cmbPozisyon.SelectedIndex = 0;
            nudFormaNo.Value = 1;
            nudMaas.Value = 10000;
            dtpSozlesmeBaslangic.Value = DateTime.Now;
            dtpSozlesmeBitis.Value = DateTime.Now.AddYears(3);
            txtUyruk.Clear();
            cmbDurumu.SelectedIndex = 0;
            txtAd.Focus();
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InitializeComponent()
        {
            this.lblAd = new System.Windows.Forms.Label();
            this.lblSoyad = new System.Windows.Forms.Label();
            this.lblDogumTarihi = new System.Windows.Forms.Label();
            this.lblBoy = new System.Windows.Forms.Label();
            this.lblKilo = new System.Windows.Forms.Label();
            this.lblPozisyon = new System.Windows.Forms.Label();
            this.lblFormaNo = new System.Windows.Forms.Label();
            this.lblMaas = new System.Windows.Forms.Label();
            this.lblSozlesmeBaslangic = new System.Windows.Forms.Label();
            this.lblSozlesmeBitis = new System.Windows.Forms.Label();
            this.lblUyruk = new System.Windows.Forms.Label();
            this.lblDurumu = new System.Windows.Forms.Label();
            this.txtAd = new System.Windows.Forms.TextBox();
            this.txtSoyad = new System.Windows.Forms.TextBox();
            this.dtpDogumTarihi = new System.Windows.Forms.DateTimePicker();
            this.nudBoy = new System.Windows.Forms.NumericUpDown();
            this.nudKilo = new System.Windows.Forms.NumericUpDown();
            this.cmbPozisyon = new System.Windows.Forms.ComboBox();
            this.nudFormaNo = new System.Windows.Forms.NumericUpDown();
            this.nudMaas = new System.Windows.Forms.NumericUpDown();
            this.dtpSozlesmeBaslangic = new System.Windows.Forms.DateTimePicker();
            this.dtpSozlesmeBitis = new System.Windows.Forms.DateTimePicker();
            this.txtUyruk = new System.Windows.Forms.TextBox();
            this.cmbDurumu = new System.Windows.Forms.ComboBox();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.btnIptal = new System.Windows.Forms.Button();
            this.chkKaydetveKapat = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudBoy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudKilo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFormaNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaas)).BeginInit();
            this.SuspendLayout();
            // 
            // lblAd
            // 
            this.lblAd.AutoSize = true;
            this.lblAd.Location = new System.Drawing.Point(22, 26);
            this.lblAd.Name = "lblAd";
            this.lblAd.Size = new System.Drawing.Size(29, 16);
            this.lblAd.TabIndex = 0;
            this.lblAd.Text = "Ad:";
            // 
            // lblSoyad
            // 
            this.lblSoyad.AutoSize = true;
            this.lblSoyad.Location = new System.Drawing.Point(22, 61);
            this.lblSoyad.Name = "lblSoyad";
            this.lblSoyad.Size = new System.Drawing.Size(53, 16);
            this.lblSoyad.TabIndex = 1;
            this.lblSoyad.Text = "Soyad:";
            // 
            // lblDogumTarihi
            // 
            this.lblDogumTarihi.AutoSize = true;
            this.lblDogumTarihi.Location = new System.Drawing.Point(22, 96);
            this.lblDogumTarihi.Name = "lblDogumTarihi";
            this.lblDogumTarihi.Size = new System.Drawing.Size(91, 16);
            this.lblDogumTarihi.TabIndex = 2;
            this.lblDogumTarihi.Text = "Doğum Tarihi:";
            // 
            // lblBoy
            // 
            this.lblBoy.AutoSize = true;
            this.lblBoy.Location = new System.Drawing.Point(22, 131);
            this.lblBoy.Name = "lblBoy";
            this.lblBoy.Size = new System.Drawing.Size(64, 16);
            this.lblBoy.TabIndex = 3;
            this.lblBoy.Text = "Boy (cm):";
            // 
            // lblKilo
            // 
            this.lblKilo.AutoSize = true;
            this.lblKilo.Location = new System.Drawing.Point(22, 166);
            this.lblKilo.Name = "lblKilo";
            this.lblKilo.Size = new System.Drawing.Size(60, 16);
            this.lblKilo.TabIndex = 4;
            this.lblKilo.Text = "Kilo (kg):";
            // 
            // lblPozisyon
            // 
            this.lblPozisyon.AutoSize = true;
            this.lblPozisyon.Location = new System.Drawing.Point(22, 201);
            this.lblPozisyon.Name = "lblPozisyon";
            this.lblPozisyon.Size = new System.Drawing.Size(65, 16);
            this.lblPozisyon.TabIndex = 5;
            this.lblPozisyon.Text = "Pozisyon:";
            // 
            // lblFormaNo
            // 
            this.lblFormaNo.AutoSize = true;
            this.lblFormaNo.Location = new System.Drawing.Point(22, 236);
            this.lblFormaNo.Name = "lblFormaNo";
            this.lblFormaNo.Size = new System.Drawing.Size(70, 16);
            this.lblFormaNo.TabIndex = 6;
            this.lblFormaNo.Text = "Forma No:";
            // 
            // lblMaas
            // 
            this.lblMaas.AutoSize = true;
            this.lblMaas.Location = new System.Drawing.Point(350, 26);
            this.lblMaas.Name = "lblMaas";
            this.lblMaas.Size = new System.Drawing.Size(44, 16);
            this.lblMaas.TabIndex = 7;
            this.lblMaas.Text = "Maaş:";
            // 
            // lblSozlesmeBaslangic
            // 
            this.lblSozlesmeBaslangic.AutoSize = true;
            this.lblSozlesmeBaslangic.Location = new System.Drawing.Point(350, 61);
            this.lblSozlesmeBaslangic.Name = "lblSozlesmeBaslangic";
            this.lblSozlesmeBaslangic.Size = new System.Drawing.Size(131, 16);
            this.lblSozlesmeBaslangic.TabIndex = 8;
            this.lblSozlesmeBaslangic.Text = "Sözleşme Başlangıç:";
            // 
            // lblSozlesmeBitis
            // 
            this.lblSozlesmeBitis.AutoSize = true;
            this.lblSozlesmeBitis.Location = new System.Drawing.Point(350, 96);
            this.lblSozlesmeBitis.Name = "lblSozlesmeBitis";
            this.lblSozlesmeBitis.Size = new System.Drawing.Size(97, 16);
            this.lblSozlesmeBitis.TabIndex = 9;
            this.lblSozlesmeBitis.Text = "Sözleşme Bitiş:";
            // 
            // lblUyruk
            // 
            this.lblUyruk.AutoSize = true;
            this.lblUyruk.Location = new System.Drawing.Point(350, 131);
            this.lblUyruk.Name = "lblUyruk";
            this.lblUyruk.Size = new System.Drawing.Size(46, 16);
            this.lblUyruk.TabIndex = 10;
            this.lblUyruk.Text = "Uyruk:";
            // 
            // lblDurumu
            // 
            this.lblDurumu.AutoSize = true;
            this.lblDurumu.Location = new System.Drawing.Point(350, 166);
            this.lblDurumu.Name = "lblDurumu";
            this.lblDurumu.Size = new System.Drawing.Size(57, 16);
            this.lblDurumu.TabIndex = 11;
            this.lblDurumu.Text = "Durumu:";
            // 
            // txtAd
            // 
            this.txtAd.Location = new System.Drawing.Point(119, 23);
            this.txtAd.Name = "txtAd";
            this.txtAd.Size = new System.Drawing.Size(190, 22);
            this.txtAd.TabIndex = 0;
            // 
            // txtSoyad
            // 
            this.txtSoyad.Location = new System.Drawing.Point(119, 58);
            this.txtSoyad.Name = "txtSoyad";
            this.txtSoyad.Size = new System.Drawing.Size(190, 22);
            this.txtSoyad.TabIndex = 1;
            // 
            // dtpDogumTarihi
            // 
            this.dtpDogumTarihi.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDogumTarihi.Location = new System.Drawing.Point(119, 93);
            this.dtpDogumTarihi.Name = "dtpDogumTarihi";
            this.dtpDogumTarihi.Size = new System.Drawing.Size(190, 22);
            this.dtpDogumTarihi.TabIndex = 2;
            // 
            // nudBoy
            // 
            this.nudBoy.Location = new System.Drawing.Point(119, 129);
            this.nudBoy.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.nudBoy.Minimum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.nudBoy.Name = "nudBoy";
            this.nudBoy.Size = new System.Drawing.Size(120, 22);
            this.nudBoy.TabIndex = 3;
            this.nudBoy.Value = new decimal(new int[] {
            180,
            0,
            0,
            0});
            // 
            // nudKilo
            // 
            this.nudKilo.Location = new System.Drawing.Point(119, 164);
            this.nudKilo.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.nudKilo.Minimum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.nudKilo.Name = "nudKilo";
            this.nudKilo.Size = new System.Drawing.Size(120, 22);
            this.nudKilo.TabIndex = 4;
            this.nudKilo.Value = new decimal(new int[] {
            75,
            0,
            0,
            0});
            // 
            // cmbPozisyon
            // 
            this.cmbPozisyon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPozisyon.FormattingEnabled = true;
            this.cmbPozisyon.Location = new System.Drawing.Point(119, 198);
            this.cmbPozisyon.Name = "cmbPozisyon";
            this.cmbPozisyon.Size = new System.Drawing.Size(190, 24);
            this.cmbPozisyon.TabIndex = 5;
            // 
            // nudFormaNo
            // 
            this.nudFormaNo.Location = new System.Drawing.Point(119, 234);
            this.nudFormaNo.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nudFormaNo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFormaNo.Name = "nudFormaNo";
            this.nudFormaNo.Size = new System.Drawing.Size(120, 22);
            this.nudFormaNo.TabIndex = 6;
            this.nudFormaNo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudMaas
            // 
            this.nudMaas.DecimalPlaces = 2;
            this.nudMaas.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudMaas.Location = new System.Drawing.Point(484, 23);
            this.nudMaas.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nudMaas.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudMaas.Name = "nudMaas";
            this.nudMaas.Size = new System.Drawing.Size(212, 22);
            this.nudMaas.TabIndex = 7;
            this.nudMaas.ThousandsSeparator = true;
            this.nudMaas.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // dtpSozlesmeBaslangic
            // 
            this.dtpSozlesmeBaslangic.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpSozlesmeBaslangic.Location = new System.Drawing.Point(484, 58);
            this.dtpSozlesmeBaslangic.Name = "dtpSozlesmeBaslangic";
            this.dtpSozlesmeBaslangic.Size = new System.Drawing.Size(212, 22);
            this.dtpSozlesmeBaslangic.TabIndex = 8;
            // 
            // dtpSozlesmeBitis
            // 
            this.dtpSozlesmeBitis.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpSozlesmeBitis.Location = new System.Drawing.Point(484, 93);
            this.dtpSozlesmeBitis.Name = "dtpSozlesmeBitis";
            this.dtpSozlesmeBitis.Size = new System.Drawing.Size(212, 22);
            this.dtpSozlesmeBitis.TabIndex = 9;
            // 
            // txtUyruk
            // 
            this.txtUyruk.Location = new System.Drawing.Point(484, 128);
            this.txtUyruk.Name = "txtUyruk";
            this.txtUyruk.Size = new System.Drawing.Size(212, 22);
            this.txtUyruk.TabIndex = 10;
            this.txtUyruk.Text = "Türkiye";
            // 
            // cmbDurumu
            // 
            this.cmbDurumu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDurumu.FormattingEnabled = true;
            this.cmbDurumu.Location = new System.Drawing.Point(484, 163);
            this.cmbDurumu.Name = "cmbDurumu";
            this.cmbDurumu.Size = new System.Drawing.Size(212, 24);
            this.cmbDurumu.TabIndex = 11;
            // 
            // btnKaydet
            // 
            this.btnKaydet.Location = new System.Drawing.Point(484, 232);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(99, 39);
            this.btnKaydet.TabIndex = 13;
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = true;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // btnIptal
            // 
            this.btnIptal.Location = new System.Drawing.Point(597, 232);
            this.btnIptal.Name = "btnIptal";
            this.btnIptal.Size = new System.Drawing.Size(99, 39);
            this.btnIptal.TabIndex = 14;
            this.btnIptal.Text = "İptal";
            this.btnIptal.UseVisualStyleBackColor = true;
            this.btnIptal.Click += new System.EventHandler(this.btnIptal_Click);
            // 
            // chkKaydetveKapat
            // 
            this.chkKaydetveKapat.AutoSize = true;
            this.chkKaydetveKapat.Location = new System.Drawing.Point(350, 201);
            this.chkKaydetveKapat.Name = "chkKaydetveKapat";
            this.chkKaydetveKapat.Size = new System.Drawing.Size(175, 20);
            this.chkKaydetveKapat.TabIndex = 12;
            this.chkKaydetveKapat.Text = "Kaydettikten sonra kapat";
            this.chkKaydetveKapat.UseVisualStyleBackColor = true;
            // 
            // FutbolcuEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 296);
            this.Controls.Add(this.chkKaydetveKapat);
            this.Controls.Add(this.btnIptal);
            this.Controls.Add(this.btnKaydet);
            this.Controls.Add(this.cmbDurumu);
            this.Controls.Add(this.txtUyruk);
            this.Controls.Add(this.dtpSozlesmeBitis);
            this.Controls.Add(this.dtpSozlesmeBaslangic);
            this.Controls.Add(this.nudMaas);
            this.Controls.Add(this.nudFormaNo);
            this.Controls.Add(this.cmbPozisyon);
            this.Controls.Add(this.nudKilo);
            this.Controls.Add(this.nudBoy);
            this.Controls.Add(this.dtpDogumTarihi);
            this.Controls.Add(this.txtSoyad);
            this.Controls.Add(this.txtAd);
            this.Controls.Add(this.lblDurumu);
            this.Controls.Add(this.lblUyruk);
            this.Controls.Add(this.lblSozlesmeBitis);
            this.Controls.Add(this.lblSozlesmeBaslangic);
            this.Controls.Add(this.lblMaas);
            this.Controls.Add(this.lblFormaNo);
            this.Controls.Add(this.lblPozisyon);
            this.Controls.Add(this.lblKilo);
            this.Controls.Add(this.lblBoy);
            this.Controls.Add(this.lblDogumTarihi);
            this.Controls.Add(this.lblSoyad);
            this.Controls.Add(this.lblAd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FutbolcuEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Futbolcu Ekle/Düzenle";
            this.Load += new System.EventHandler(this.FutbolcuEditForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudBoy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudKilo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFormaNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblAd;
        private System.Windows.Forms.Label lblSoyad;
        private System.Windows.Forms.Label lblDogumTarihi;
        private System.Windows.Forms.Label lblBoy;
        private System.Windows.Forms.Label lblKilo;
        private System.Windows.Forms.Label lblPozisyon;
        private System.Windows.Forms.Label lblFormaNo;
        private System.Windows.Forms.Label lblMaas;
        private System.Windows.Forms.Label lblSozlesmeBaslangic;
        private System.Windows.Forms.Label lblSozlesmeBitis;
        private System.Windows.Forms.Label lblUyruk;
        private System.Windows.Forms.Label lblDurumu;
        private System.Windows.Forms.TextBox txtAd;
        private System.Windows.Forms.TextBox txtSoyad;
        private System.Windows.Forms.DateTimePicker dtpDogumTarihi;
        private System.Windows.Forms.NumericUpDown nudBoy;
        private System.Windows.Forms.NumericUpDown nudKilo;
        private System.Windows.Forms.ComboBox cmbPozisyon;
        private System.Windows.Forms.NumericUpDown nudFormaNo;
        private System.Windows.Forms.NumericUpDown nudMaas;
        private System.Windows.Forms.DateTimePicker dtpSozlesmeBaslangic;
        private System.Windows.Forms.DateTimePicker dtpSozlesmeBitis;
        private System.Windows.Forms.TextBox txtUyruk;
        private System.Windows.Forms.ComboBox cmbDurumu;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.Button btnIptal;
        private System.Windows.Forms.CheckBox chkKaydetveKapat;
    }
} 