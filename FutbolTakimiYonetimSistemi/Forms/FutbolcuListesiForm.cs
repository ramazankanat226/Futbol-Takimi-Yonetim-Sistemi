using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FutbolTakimiYonetimSistemi.Models;
using FutbolTakimiYonetimSistemi.Services;

namespace FutbolTakimiYonetimSistemi.Forms
{
    public partial class FutbolcuListesiForm : Form
    {
        private List<Futbolcu> _futbolcular;

        public FutbolcuListesiForm()
        {
            InitializeComponent();
        }

        private void FutbolcuListesiForm_Load(object sender, EventArgs e)
        {
            // Formu tam ekran yap
            this.WindowState = FormWindowState.Maximized;
            
            YenileFutbolcuListesi();
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
            dgvFutbolcular.Columns["FutbolcuID"].Visible = false;
            dgvFutbolcular.Columns["Resim"].Visible = false;
            
            dgvFutbolcular.Columns["Ad"].HeaderText = "Ad";
            dgvFutbolcular.Columns["Soyad"].HeaderText = "Soyad";
            dgvFutbolcular.Columns["TamAd"].HeaderText = "Adı Soyadı";
            dgvFutbolcular.Columns["DogumTarihi"].HeaderText = "Doğum Tarihi";
            dgvFutbolcular.Columns["Yas"].HeaderText = "Yaş";
            dgvFutbolcular.Columns["Boy"].HeaderText = "Boy (cm)";
            dgvFutbolcular.Columns["Kilo"].HeaderText = "Kilo (kg)";
            dgvFutbolcular.Columns["Pozisyon"].HeaderText = "Pozisyon";
            dgvFutbolcular.Columns["FormaNo"].HeaderText = "Forma No";
            dgvFutbolcular.Columns["Maas"].HeaderText = "Maaş (₺)";
            dgvFutbolcular.Columns["Uyruk"].HeaderText = "Uyruk";
            dgvFutbolcular.Columns["SozlesmeBaslangic"].HeaderText = "Sözleşme Başlangıç";
            dgvFutbolcular.Columns["SozlesmeBitis"].HeaderText = "Sözleşme Bitiş";
            dgvFutbolcular.Columns["KalanSozlesmeSuresi"].HeaderText = "Kalan Süre";
            dgvFutbolcular.Columns["Durumu"].HeaderText = "Durum";

            // Format ayarları
            dgvFutbolcular.Columns["DogumTarihi"].DefaultCellStyle.Format = "dd.MM.yyyy";
            dgvFutbolcular.Columns["SozlesmeBaslangic"].DefaultCellStyle.Format = "dd.MM.yyyy";
            dgvFutbolcular.Columns["SozlesmeBitis"].DefaultCellStyle.Format = "dd.MM.yyyy";
            dgvFutbolcular.Columns["Maas"].DefaultCellStyle.Format = "N2";

            // Sütun genişliklerini ayarla
            dgvFutbolcular.AutoResizeColumns();
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
            FutbolcuEditForm futbolcuEditForm = new FutbolcuEditForm();
            futbolcuEditForm.FormClosed += (s, args) => YenileFutbolcuListesi();
            futbolcuEditForm.ShowDialog();
        }

        private void btnDuzenle_Click(object sender, EventArgs e)
        {
            DuzenleFutbolcu();
        }

        private void dgvFutbolcular_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
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
            this.dgvFutbolcular.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
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
            this.btnYenile.Location = new System.Drawing.Point(12, 12);
            this.btnYenile.Name = "btnYenile";
            this.btnYenile.Size = new System.Drawing.Size(135, 35);
            this.btnYenile.TabIndex = 3;
            this.btnYenile.Text = "Listeyi Yenile";
            this.btnYenile.UseVisualStyleBackColor = true;
            this.btnYenile.Click += new System.EventHandler(this.btnYenile_Click);
            // 
            // btnSil
            // 
            this.btnSil.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSil.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSil.Location = new System.Drawing.Point(849, 12);
            this.btnSil.Name = "btnSil";
            this.btnSil.Size = new System.Drawing.Size(70, 35);
            this.btnSil.TabIndex = 2;
            this.btnSil.Text = "Sil";
            this.btnSil.UseVisualStyleBackColor = true;
            this.btnSil.Click += new System.EventHandler(this.btnSil_Click);
            // 
            // btnDuzenle
            // 
            this.btnDuzenle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDuzenle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnDuzenle.Location = new System.Drawing.Point(925, 12);
            this.btnDuzenle.Name = "btnDuzenle";
            this.btnDuzenle.Size = new System.Drawing.Size(70, 35);
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
            this.btnYeniFutbolcu.Size = new System.Drawing.Size(70, 35);
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