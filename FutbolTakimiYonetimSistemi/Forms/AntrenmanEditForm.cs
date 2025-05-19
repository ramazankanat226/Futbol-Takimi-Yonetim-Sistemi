using System;
using System.Windows.Forms;
using FutbolTakimiYonetimSistemi.Models;
using FutbolTakimiYonetimSistemi.Services;

namespace FutbolTakimiYonetimSistemi.Forms
{
    public partial class AntrenmanEditForm : Form
    {
        private Antrenman _antrenman;
        private bool _isEditMode = false;

        public AntrenmanEditForm()
        {
            InitializeComponent();
            _antrenman = new Antrenman
            {
                Tarih = DateTime.Today,
                BaslangicSaati = DateTime.Today.AddHours(9),
                BitisSaati = DateTime.Today.AddHours(11)
            };
        }

        public AntrenmanEditForm(Antrenman antrenman)
        {
            InitializeComponent();
            _antrenman = antrenman;
            _isEditMode = true;
        }

        private void AntrenmanEditForm_Load(object sender, EventArgs e)
        {
            // Tür combobox'ını doldur
            comboBoxTur.Items.AddRange(new string[] { "Kondisyon", "Taktik", "Teknik" });
            
            if (_isEditMode)
            {
                Text = "Antrenman Düzenle";
                YukleAntrenmanVerileri();
            }
            else
            {
                Text = "Yeni Antrenman";
                comboBoxTur.SelectedIndex = 0;
            }
        }

        private void YukleAntrenmanVerileri()
        {
            datePickerTarih.Value = _antrenman.Tarih;
            dateTimePickerBaslangic.Value = _antrenman.BaslangicSaati;
            dateTimePickerBitis.Value = _antrenman.BitisSaati;
            
            // Tür combobox'ında ilgili değeri seç
            for (int i = 0; i < comboBoxTur.Items.Count; i++)
            {
                if (comboBoxTur.Items[i].ToString() == _antrenman.Tur)
                {
                    comboBoxTur.SelectedIndex = i;
                    break;
                }
            }
            
            txtNot.Text = _antrenman.Notlar;
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                return;
            }

            try
            {
                // Form verilerini antrenman nesnesine aktar
                _antrenman.Tarih = datePickerTarih.Value;
                _antrenman.BaslangicSaati = dateTimePickerBaslangic.Value;
                _antrenman.BitisSaati = dateTimePickerBitis.Value;
                _antrenman.Tur = comboBoxTur.SelectedItem?.ToString() ?? "";
                _antrenman.Notlar = string.IsNullOrWhiteSpace(txtNot.Text) ? null : txtNot.Text;

                bool success = false;
                if (_isEditMode)
                {
                    success = AntrenmanService.GuncelleAntrenman(_antrenman);
                    if (success)
                    {
                        MessageBox.Show("Antrenman başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DialogResult = DialogResult.OK;
                    }
                }
                else
                {
                    success = AntrenmanService.EkleAntrenman(_antrenman);
                    if (success)
                    {
                        MessageBox.Show("Antrenman başarıyla eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DialogResult = DialogResult.OK;
                    }
                }

                if (!success)
                {
                    MessageBox.Show("İşlem sırasında bir hata oluştu. Lütfen tüm alanları kontrol edip tekrar deneyin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Beklenmeyen bir hata oluştu: {ex.Message}\n\n{ex.StackTrace}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateForm()
        {
            if (comboBoxTur.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen antrenman türünü seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxTur.Focus();
                return false;
            }

            if (dateTimePickerBaslangic.Value >= dateTimePickerBitis.Value)
            {
                MessageBox.Show("Başlangıç saati, bitiş saatinden önce olmalıdır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dateTimePickerBaslangic.Focus();
                return false;
            }

            return true;
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.datePickerTarih = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerBaslangic = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerBitis = new System.Windows.Forms.DateTimePicker();
            this.comboBoxTur = new System.Windows.Forms.ComboBox();
            this.txtNot = new System.Windows.Forms.TextBox();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.btnIptal = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(22, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tarih:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Location = new System.Drawing.Point(22, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "Başlangıç Saati:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label3.Location = new System.Drawing.Point(22, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "Bitiş Saati:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.Location = new System.Drawing.Point(22, 148);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 18);
            this.label4.TabIndex = 3;
            this.label4.Text = "Antrenman Türü:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label5.Location = new System.Drawing.Point(22, 188);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 18);
            this.label5.TabIndex = 4;
            this.label5.Text = "Notlar:";
            // 
            // datePickerTarih
            // 
            this.datePickerTarih.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.datePickerTarih.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePickerTarih.Location = new System.Drawing.Point(158, 28);
            this.datePickerTarih.Name = "datePickerTarih";
            this.datePickerTarih.Size = new System.Drawing.Size(200, 24);
            this.datePickerTarih.TabIndex = 5;
            // 
            // dateTimePickerBaslangic
            // 
            this.dateTimePickerBaslangic.CustomFormat = "HH:mm";
            this.dateTimePickerBaslangic.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.dateTimePickerBaslangic.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerBaslangic.Location = new System.Drawing.Point(158, 68);
            this.dateTimePickerBaslangic.Name = "dateTimePickerBaslangic";
            this.dateTimePickerBaslangic.ShowUpDown = true;
            this.dateTimePickerBaslangic.Size = new System.Drawing.Size(200, 24);
            this.dateTimePickerBaslangic.TabIndex = 6;
            // 
            // dateTimePickerBitis
            // 
            this.dateTimePickerBitis.CustomFormat = "HH:mm";
            this.dateTimePickerBitis.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.dateTimePickerBitis.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerBitis.Location = new System.Drawing.Point(158, 108);
            this.dateTimePickerBitis.Name = "dateTimePickerBitis";
            this.dateTimePickerBitis.ShowUpDown = true;
            this.dateTimePickerBitis.Size = new System.Drawing.Size(200, 24);
            this.dateTimePickerBitis.TabIndex = 7;
            // 
            // comboBoxTur
            // 
            this.comboBoxTur.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTur.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.comboBoxTur.FormattingEnabled = true;
            this.comboBoxTur.Location = new System.Drawing.Point(158, 148);
            this.comboBoxTur.Name = "comboBoxTur";
            this.comboBoxTur.Size = new System.Drawing.Size(200, 26);
            this.comboBoxTur.TabIndex = 8;
            // 
            // txtNot
            // 
            this.txtNot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNot.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtNot.Location = new System.Drawing.Point(158, 188);
            this.txtNot.Multiline = true;
            this.txtNot.Name = "txtNot";
            this.txtNot.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNot.Size = new System.Drawing.Size(414, 185);
            this.txtNot.TabIndex = 9;
            // 
            // btnKaydet
            // 
            this.btnKaydet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKaydet.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnKaydet.Location = new System.Drawing.Point(482, 389);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(90, 40);
            this.btnKaydet.TabIndex = 10;
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = true;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // btnIptal
            // 
            this.btnIptal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIptal.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnIptal.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnIptal.Location = new System.Drawing.Point(386, 389);
            this.btnIptal.Name = "btnIptal";
            this.btnIptal.Size = new System.Drawing.Size(90, 40);
            this.btnIptal.TabIndex = 11;
            this.btnIptal.Text = "İptal";
            this.btnIptal.UseVisualStyleBackColor = true;
            this.btnIptal.Click += new System.EventHandler(this.btnIptal_Click);
            // 
            // AntrenmanEditForm
            // 
            this.AcceptButton = this.btnKaydet;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnIptal;
            this.ClientSize = new System.Drawing.Size(584, 441);
            this.Controls.Add(this.btnIptal);
            this.Controls.Add(this.btnKaydet);
            this.Controls.Add(this.txtNot);
            this.Controls.Add(this.comboBoxTur);
            this.Controls.Add(this.dateTimePickerBitis);
            this.Controls.Add(this.dateTimePickerBaslangic);
            this.Controls.Add(this.datePickerTarih);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(600, 480);
            this.Name = "AntrenmanEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Antrenman";
            this.Load += new System.EventHandler(this.AntrenmanEditForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker datePickerTarih;
        private System.Windows.Forms.DateTimePicker dateTimePickerBaslangic;
        private System.Windows.Forms.DateTimePicker dateTimePickerBitis;
        private System.Windows.Forms.ComboBox comboBoxTur;
        private System.Windows.Forms.TextBox txtNot;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.Button btnIptal;
    }
} 