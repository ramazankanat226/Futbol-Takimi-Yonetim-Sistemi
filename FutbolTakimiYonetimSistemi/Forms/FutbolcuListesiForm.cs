using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FutbolTakimiYonetimSistemi.Models;
using FutbolTakimiYonetimSistemi.Services;
using FutbolTakimiYonetimSistemi.Utils;

namespace FutbolTakimiYonetimSistemi.Forms
{
    public partial class FutbolcuListesiForm : Form
    {
        private List<Futbolcu> _futbolcular;
        private bool _saltOkunur;

        public FutbolcuListesiForm(bool saltOkunur = false)
        {
            InitializeComponent();
            _saltOkunur = saltOkunur;
        }

        private void FutbolcuListesiForm_Load(object sender, EventArgs e)
        {
            // Formu tam ekran yap
            this.WindowState = FormWindowState.Maximized;
            
            // Modern stil uygula
            UygulaModernStiller();
            
            // Salt okunur modda ise butonları kapat
            if (_saltOkunur)
            {
                btnYeniFutbolcu.Enabled = false;
                btnDuzenle.Enabled = false;
                btnSil.Enabled = false;
                
                btnYeniFutbolcu.BackColor = ColorTranslator.FromHtml("#95A5A6");
                btnDuzenle.BackColor = ColorTranslator.FromHtml("#95A5A6");
                btnSil.BackColor = ColorTranslator.FromHtml("#95A5A6");
                
                btnYeniFutbolcu.Text = "🔒 YETKİ YOK";
                btnDuzenle.Text = "🔒 YETKİ YOK";
                btnSil.Text = "🔒 YETKİ YOK";
                
                this.Text = "Futbolcu Listesi (Salt Okunur - Antrenör)";
            }
            
            YenileFutbolcuListesi();
        }

        private void UygulaModernStiller()
        {
            // Form arkaplan
            FormStilleri.ModernForm(this);

            // DataGridView modern stil
            FormStilleri.ModernDataGridView(dgvFutbolcular);

            // Paneller
            pnlArama.BackColor = Color.White;
            pnlArama.Padding = new Padding(15);
            pnlIslemler.BackColor = FormStilleri.Renkler.AcikArkaplan;
            pnlIslemler.Padding = new Padding(15, 10, 15, 10);

            // Arama kutusu ve label
            FormStilleri.ModernLabel(lblArama);
            FormStilleri.ModernTextBox(txtArama);
            FormStilleri.ModernButon(btnAra, "mavi");
            btnAra.Text = "🔍 ARA";

            // İşlem butonları
            FormStilleri.ModernButon(btnYeniFutbolcu, "yesil");
            btnYeniFutbolcu.Text = "+ YENİ FUTBOLCU";

            FormStilleri.ModernButon(btnDuzenle, "turuncu");
            btnDuzenle.Text = "✏️ DÜZENLE";

            FormStilleri.ModernButon(btnSil, "kirmizi");
            btnSil.Text = "🗑️ SİL";

            FormStilleri.ModernButon(btnYenile, "gri");
            btnYenile.Text = "🔄 YENİLE";
        }

        public void YenileFutbolcuListesi()
        {
            _futbolcular = FutbolcuService.GetAllFutbolcular();
            FutbolculariListele();
        }

        private void FutbolculariListele()
        {
            dgvFutbolcular.DataSource = null;
            
            if (_futbolcular != null && _futbolcular.Count > 0)
            {
                dgvFutbolcular.DataSource = _futbolcular;
                DuzenleDataGrid();
            }
        }

        private void DuzenleDataGrid()
        {
            // Gereksiz sütunları gizle
            dgvFutbolcular.Columns["FutbolcuID"].Visible = false;
            dgvFutbolcular.Columns["Resim"].Visible = false;
            dgvFutbolcular.Columns["Notlar"].Visible = false;
            dgvFutbolcular.Columns["Ad"].Visible = false;  // TamAd varken gereksiz
            dgvFutbolcular.Columns["Soyad"].Visible = false;  // TamAd varken gereksiz
            
            // Header metinleri
            dgvFutbolcular.Columns["TamAd"].HeaderText = "Adı Soyadı";
            dgvFutbolcular.Columns["DogumTarihi"].HeaderText = "Doğum Tarihi";
            dgvFutbolcular.Columns["Yas"].HeaderText = "Yaş";
            dgvFutbolcular.Columns["Boy"].HeaderText = "Boy";
            dgvFutbolcular.Columns["Kilo"].HeaderText = "Kilo";
            dgvFutbolcular.Columns["Pozisyon"].HeaderText = "Pozisyon";
            dgvFutbolcular.Columns["FormaNo"].HeaderText = "No";
            dgvFutbolcular.Columns["Maas"].HeaderText = "Maaş (₺)";
            dgvFutbolcular.Columns["Uyruk"].HeaderText = "Uyruk";
            dgvFutbolcular.Columns["SozlesmeBaslangic"].HeaderText = "Sözleşme Baş.";
            dgvFutbolcular.Columns["SozlesmeBitis"].HeaderText = "Sözleşme Bit.";
            dgvFutbolcular.Columns["KalanSozlesmeSuresi"].HeaderText = "Kalan Süre";
            dgvFutbolcular.Columns["Durumu"].HeaderText = "Durum";

            // Format ayarları
            dgvFutbolcular.Columns["DogumTarihi"].DefaultCellStyle.Format = "dd.MM.yyyy";
            dgvFutbolcular.Columns["SozlesmeBaslangic"].DefaultCellStyle.Format = "dd.MM.yyyy";
            dgvFutbolcular.Columns["SozlesmeBitis"].DefaultCellStyle.Format = "dd.MM.yyyy";
            dgvFutbolcular.Columns["Maas"].DefaultCellStyle.Format = "N2";

            // Sütun genişlikleri (responsive)
            dgvFutbolcular.Columns["TamAd"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvFutbolcular.Columns["TamAd"].Width = 150;
            dgvFutbolcular.Columns["TamAd"].MinimumWidth = 120;
            
            dgvFutbolcular.Columns["DogumTarihi"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvFutbolcular.Columns["DogumTarihi"].Width = 100;
            
            dgvFutbolcular.Columns["Yas"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvFutbolcular.Columns["Yas"].Width = 50;
            
            dgvFutbolcular.Columns["Boy"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvFutbolcular.Columns["Boy"].Width = 60;
            
            dgvFutbolcular.Columns["Kilo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvFutbolcular.Columns["Kilo"].Width = 60;
            
            dgvFutbolcular.Columns["Pozisyon"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvFutbolcular.Columns["Pozisyon"].Width = 90;
            
            dgvFutbolcular.Columns["FormaNo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvFutbolcular.Columns["FormaNo"].Width = 45;
            
            dgvFutbolcular.Columns["Maas"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvFutbolcular.Columns["Maas"].Width = 100;
            
            dgvFutbolcular.Columns["Uyruk"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvFutbolcular.Columns["Uyruk"].Width = 80;
            
            dgvFutbolcular.Columns["SozlesmeBaslangic"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvFutbolcular.Columns["SozlesmeBaslangic"].Width = 110;
            
            dgvFutbolcular.Columns["SozlesmeBitis"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvFutbolcular.Columns["SozlesmeBitis"].Width = 110;
            
            // Bu sütunlar responsive (kalan alanı paylaşırlar)
            dgvFutbolcular.Columns["KalanSozlesmeSuresi"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvFutbolcular.Columns["KalanSozlesmeSuresi"].FillWeight = 30;
            dgvFutbolcular.Columns["KalanSozlesmeSuresi"].MinimumWidth = 80;
            
            dgvFutbolcular.Columns["Durumu"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvFutbolcular.Columns["Durumu"].FillWeight = 25;
            dgvFutbolcular.Columns["Durumu"].MinimumWidth = 80;

            // Satırları duruma göre renklendir
            foreach (DataGridViewRow row in dgvFutbolcular.Rows)
            {
                if (row.Cells["Durumu"].Value != null)
                {
                    string durum = row.Cells["Durumu"].Value.ToString();
                    FormStilleri.RenklendirilmisSatir(row, durum);
                }
            }
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            string aramaMetni = txtArama.Text.Trim();
            if (!string.IsNullOrEmpty(aramaMetni))
            {
                _futbolcular = FutbolcuService.FutbolcuAra(aramaMetni);
            }
            else
            {
                _futbolcular = FutbolcuService.GetAllFutbolcular();
            }
            
            FutbolculariListele();
        }

        private void txtArama_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAra_Click(sender, e);
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void btnYeniFutbolcu_Click(object sender, EventArgs e)
        {
            if (_saltOkunur)
            {
                MessageBox.Show("Antrenörler futbolcu ekleyemez!", "Yetki Yok", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            FutbolcuEditForm futbolcuEditForm = new FutbolcuEditForm();
            futbolcuEditForm.FormClosed += (s, args) => YenileFutbolcuListesi();
            futbolcuEditForm.ShowDialog();
        }

        private void btnDuzenle_Click(object sender, EventArgs e)
        {
            if (_saltOkunur)
            {
                MessageBox.Show("Antrenörler futbolcu düzenleyemez!", "Yetki Yok", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DuzenleFutbolcu();
        }

        private void dgvFutbolcular_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (_saltOkunur)
                {
                    MessageBox.Show("Sadece görüntüleme modundasınız.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                DuzenleFutbolcu();
            }
        }

        private void DuzenleFutbolcu()
        {
            if (dgvFutbolcular.CurrentRow != null)
            {
                int futbolcuId = (int)dgvFutbolcular.CurrentRow.Cells["FutbolcuID"].Value;
                FutbolcuEditForm futbolcuEditForm = new FutbolcuEditForm(futbolcuId);
                futbolcuEditForm.FormClosed += (s, args) => YenileFutbolcuListesi();
                futbolcuEditForm.ShowDialog();
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (_saltOkunur)
            {
                MessageBox.Show("Antrenörler futbolcu silemez!", "Yetki Yok", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvFutbolcular.CurrentRow != null)
            {
                int futbolcuId = (int)dgvFutbolcular.CurrentRow.Cells["FutbolcuID"].Value;
                string futbolcuAd = dgvFutbolcular.CurrentRow.Cells["TamAd"].Value.ToString();

                DialogResult dr = MessageBox.Show($"{futbolcuAd} adlı futbolcuyu silmek istediğinize emin misiniz?", 
                    "Futbolcu Sil", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    bool sonuc = FutbolcuService.SilFutbolcu(futbolcuId);
                    if (sonuc)
                    {
                        MessageBox.Show("Futbolcu başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        YenileFutbolcuListesi();
                    }
                }
            }
        }

        private void btnYenile_Click(object sender, EventArgs e)
        {
            txtArama.Clear();
            YenileFutbolcuListesi();
        }

        private void InitializeComponent()
        {
            this.dgvFutbolcular = new System.Windows.Forms.DataGridView();
            this.pnlArama = new System.Windows.Forms.Panel();
            this.btnAra = new System.Windows.Forms.Button();
            this.txtArama = new System.Windows.Forms.TextBox();
            this.lblArama = new System.Windows.Forms.Label();
            this.pnlIslemler = new System.Windows.Forms.Panel();
            this.btnYenile = new System.Windows.Forms.Button();
            this.btnSil = new System.Windows.Forms.Button();
            this.btnDuzenle = new System.Windows.Forms.Button();
            this.btnYeniFutbolcu = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFutbolcular)).BeginInit();
            this.pnlArama.SuspendLayout();
            this.pnlIslemler.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvFutbolcular
            // 
            this.dgvFutbolcular.AllowUserToAddRows = false;
            this.dgvFutbolcular.AllowUserToDeleteRows = false;
            this.dgvFutbolcular.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvFutbolcular.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            this.dgvFutbolcular.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFutbolcular.Location = new System.Drawing.Point(12, 72);
            this.dgvFutbolcular.MultiSelect = false;
            this.dgvFutbolcular.Name = "dgvFutbolcular";
            this.dgvFutbolcular.ReadOnly = true;
            this.dgvFutbolcular.RowHeadersWidth = 51;
            this.dgvFutbolcular.RowTemplate.Height = 24;
            this.dgvFutbolcular.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFutbolcular.Size = new System.Drawing.Size(1058, 417);
            this.dgvFutbolcular.TabIndex = 0;
            this.dgvFutbolcular.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFutbolcular_CellDoubleClick);
            // 
            // pnlArama
            // 
            this.pnlArama.Controls.Add(this.btnAra);
            this.pnlArama.Controls.Add(this.txtArama);
            this.pnlArama.Controls.Add(this.lblArama);
            this.pnlArama.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlArama.Location = new System.Drawing.Point(0, 0);
            this.pnlArama.Name = "pnlArama";
            this.pnlArama.Size = new System.Drawing.Size(1082, 66);
            this.pnlArama.TabIndex = 1;
            // 
            // btnAra
            // 
            this.btnAra.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnAra.Location = new System.Drawing.Point(424, 18);
            this.btnAra.Name = "btnAra";
            this.btnAra.Size = new System.Drawing.Size(95, 35);
            this.btnAra.TabIndex = 2;
            this.btnAra.Text = "Ara";
            this.btnAra.UseVisualStyleBackColor = true;
            this.btnAra.Click += new System.EventHandler(this.btnAra_Click);
            // 
            // txtArama
            // 
            this.txtArama.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtArama.Location = new System.Drawing.Point(158, 22);
            this.txtArama.Name = "txtArama";
            this.txtArama.Size = new System.Drawing.Size(248, 26);
            this.txtArama.TabIndex = 1;
            this.txtArama.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtArama_KeyDown);
            // 
            // lblArama
            // 
            this.lblArama.AutoSize = true;
            this.lblArama.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblArama.Location = new System.Drawing.Point(12, 25);
            this.lblArama.Name = "lblArama";
            this.lblArama.Size = new System.Drawing.Size(140, 20);
            this.lblArama.TabIndex = 0;
            this.lblArama.Text = "Futbolcu Adı/Pozisyon:";
            // 
            // pnlIslemler
            // 
            this.pnlIslemler.Controls.Add(this.btnYenile);
            this.pnlIslemler.Controls.Add(this.btnSil);
            this.pnlIslemler.Controls.Add(this.btnDuzenle);
            this.pnlIslemler.Controls.Add(this.btnYeniFutbolcu);
            this.pnlIslemler.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlIslemler.Location = new System.Drawing.Point(0, 495);
            this.pnlIslemler.Name = "pnlIslemler";
            this.pnlIslemler.Size = new System.Drawing.Size(1082, 58);
            this.pnlIslemler.TabIndex = 2;
            // 
            // btnYenile
            // 
            this.btnYenile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnYenile.Location = new System.Drawing.Point(581, 12);
            this.btnYenile.Name = "btnYenile";
            this.btnYenile.Size = new System.Drawing.Size(130, 35);
            this.btnYenile.TabIndex = 3;
            this.btnYenile.Text = "Listeyi Yenile";
            this.btnYenile.UseVisualStyleBackColor = true;
            this.btnYenile.Click += new System.EventHandler(this.btnYenile_Click);
            // 
            // btnSil
            // 
            this.btnSil.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSil.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSil.Location = new System.Drawing.Point(721, 12);
            this.btnSil.Name = "btnSil";
            this.btnSil.Size = new System.Drawing.Size(120, 35);
            this.btnSil.TabIndex = 2;
            this.btnSil.Text = "Sil";
            this.btnSil.UseVisualStyleBackColor = true;
            this.btnSil.Click += new System.EventHandler(this.btnSil_Click);
            // 
            // btnDuzenle
            // 
            this.btnDuzenle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDuzenle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnDuzenle.Location = new System.Drawing.Point(851, 12);
            this.btnDuzenle.Name = "btnDuzenle";
            this.btnDuzenle.Size = new System.Drawing.Size(140, 35);
            this.btnDuzenle.TabIndex = 1;
            this.btnDuzenle.Text = "Düzenle";
            this.btnDuzenle.UseVisualStyleBackColor = true;
            this.btnDuzenle.Click += new System.EventHandler(this.btnDuzenle_Click);
            // 
            // btnYeniFutbolcu
            // 
            this.btnYeniFutbolcu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnYeniFutbolcu.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnYeniFutbolcu.Location = new System.Drawing.Point(1001, 12);
            this.btnYeniFutbolcu.Name = "btnYeniFutbolcu";
            this.btnYeniFutbolcu.Size = new System.Drawing.Size(140, 35);
            this.btnYeniFutbolcu.TabIndex = 0;
            this.btnYeniFutbolcu.Text = "Yeni";
            this.btnYeniFutbolcu.UseVisualStyleBackColor = true;
            this.btnYeniFutbolcu.Click += new System.EventHandler(this.btnYeniFutbolcu_Click);
            // 
            // FutbolcuListesiForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1082, 553);
            this.Controls.Add(this.pnlIslemler);
            this.Controls.Add(this.pnlArama);
            this.Controls.Add(this.dgvFutbolcular);
            this.MinimumSize = new System.Drawing.Size(1100, 600);
            this.WindowState = FormWindowState.Maximized;
            this.Name = "FutbolcuListesiForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Futbolcu Listesi";
            this.Load += new System.EventHandler(this.FutbolcuListesiForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFutbolcular)).EndInit();
            this.pnlArama.ResumeLayout(false);
            this.pnlArama.PerformLayout();
            this.pnlIslemler.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView dgvFutbolcular;
        private System.Windows.Forms.Panel pnlArama;
        private System.Windows.Forms.Button btnAra;
        private System.Windows.Forms.TextBox txtArama;
        private System.Windows.Forms.Label lblArama;
        private System.Windows.Forms.Panel pnlIslemler;
        private System.Windows.Forms.Button btnSil;
        private System.Windows.Forms.Button btnDuzenle;
        private System.Windows.Forms.Button btnYeniFutbolcu;
        private System.Windows.Forms.Button btnYenile;
    }
} 