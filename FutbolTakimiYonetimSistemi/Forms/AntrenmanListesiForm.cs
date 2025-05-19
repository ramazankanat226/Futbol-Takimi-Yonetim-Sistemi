using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FutbolTakimiYonetimSistemi.Models;
using FutbolTakimiYonetimSistemi.Services;

namespace FutbolTakimiYonetimSistemi.Forms
{
    public partial class AntrenmanListesiForm : Form
    {
        private List<Antrenman> _antrenmanlar = new List<Antrenman>();

        public AntrenmanListesiForm()
        {
            InitializeComponent();
        }

        private void AntrenmanListesiForm_Load(object sender, EventArgs e)
        {
            comboBoxTur.Items.AddRange(new string[] { "Tümü", "Kondisyon", "Taktik", "Teknik" });
            comboBoxTur.SelectedIndex = 0;
            YenileAntrenmanListesi();
        }

        public void YenileAntrenmanListesi()
        {
            try
            {
                _antrenmanlar = AntrenmanService.GetAllAntrenmanlar();
                
                // Null kontrolü ekleyelim
                if (_antrenmanlar == null)
                {
                    _antrenmanlar = new List<Antrenman>();
                    MessageBox.Show("Antrenman listesi alınamadı. Boş liste kullanılacak.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                
                FiltreUygula();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Antrenman listesi yenilenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _antrenmanlar = new List<Antrenman>();
                listViewAntrenmanlar.Items.Clear();
                lblSonucSayisi.Text = "0 adet antrenman bulundu.";
            }
        }

        private void FiltreUygula()
        {
            // _antrenmanlar null ise yeni bir boş liste oluştur
            if (_antrenmanlar == null)
            {
                _antrenmanlar = new List<Antrenman>();
            }

            // Referans atama yerine yeni bir liste oluştur (kopya)
            List<Antrenman> filtreliAntrenmanlar = new List<Antrenman>(_antrenmanlar);

            // Tür filtresi uygulama
            if (comboBoxTur.SelectedIndex > 0 && comboBoxTur.SelectedItem != null) // 0 = Tümü
            {
                string seciliTur = comboBoxTur.SelectedItem.ToString();
                filtreliAntrenmanlar = filtreliAntrenmanlar.FindAll(a => a != null && a.Tur == seciliTur);
            }

            // Arama metni filtresi uygulama
            if (!string.IsNullOrEmpty(txtArama.Text))
            {
                string aramaMetni = txtArama.Text.ToLower();
                filtreliAntrenmanlar = filtreliAntrenmanlar.FindAll(a => 
                    a != null && ((a.Tur?.ToLower().Contains(aramaMetni) ?? false) || 
                    (a.Notlar?.ToLower().Contains(aramaMetni) ?? false)));
            }

            // DataGridView'i güncelle
            listViewAntrenmanlar.Items.Clear();
            
            foreach (var antrenman in filtreliAntrenmanlar)
            {
                try
                {
                    if (antrenman == null) continue;

                    ListViewItem item = new ListViewItem(antrenman.AntrenmanID.ToString());
                    item.SubItems.Add(antrenman.Tarih.ToShortDateString());
                    item.SubItems.Add(antrenman.BaslangicSaati.ToShortTimeString());
                    item.SubItems.Add(antrenman.BitisSaati.ToShortTimeString());
                    item.SubItems.Add(antrenman.TurSecenek);
                    item.Tag = antrenman;
                    listViewAntrenmanlar.Items.Add(item);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Bir antrenmanı listelerken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Sonuç sayısını güncelle
            lblSonucSayisi.Text = $"{listViewAntrenmanlar.Items.Count} adet antrenman bulundu.";
        }

        private void comboBoxTur_SelectedIndexChanged(object sender, EventArgs e)
        {
            FiltreUygula();
        }

        private void txtArama_TextChanged(object sender, EventArgs e)
        {
            FiltreUygula();
        }

        private void btnYenile_Click(object sender, EventArgs e)
        {
            YenileAntrenmanListesi();
        }

        private void btnYeniAntrenman_Click(object sender, EventArgs e)
        {
            try
            {
                AntrenmanEditForm antrenmanEditForm = new AntrenmanEditForm();
                if (antrenmanEditForm.ShowDialog() == DialogResult.OK)
                {
                    YenileAntrenmanListesi();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Yeni antrenman eklenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDuzenle_Click(object sender, EventArgs e)
        {
            if (listViewAntrenmanlar.SelectedItems.Count > 0)
            {
                Antrenman seciliAntrenman = (Antrenman)listViewAntrenmanlar.SelectedItems[0].Tag;
                AntrenmanEditForm antrenmanEditForm = new AntrenmanEditForm(seciliAntrenman);
                if (antrenmanEditForm.ShowDialog() == DialogResult.OK)
                {
                    YenileAntrenmanListesi();
                }
            }
            else
            {
                MessageBox.Show("Lütfen düzenlemek için bir antrenman seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnKatilim_Click(object sender, EventArgs e)
        {
            if (listViewAntrenmanlar.SelectedItems.Count > 0)
            {
                Antrenman seciliAntrenman = (Antrenman)listViewAntrenmanlar.SelectedItems[0].Tag;
                AntrenmanKatilimForm katilimForm = new AntrenmanKatilimForm(seciliAntrenman);
                if (katilimForm.ShowDialog() == DialogResult.OK)
                {
                    // Katılım bilgileri güncellendiyse listeyi yenile
                    YenileAntrenmanListesi();
                }
            }
            else
            {
                MessageBox.Show("Lütfen katılım için bir antrenman seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnKatilimGoruntule_Click(object sender, EventArgs e)
        {
            if (listViewAntrenmanlar.SelectedItems.Count > 0)
            {
                Antrenman seciliAntrenman = (Antrenman)listViewAntrenmanlar.SelectedItems[0].Tag;
                AntrenmanKatilimGoruntuleForm katilimGoruntuleForm = new AntrenmanKatilimGoruntuleForm(seciliAntrenman);
                katilimGoruntuleForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Lütfen katılımı görüntülemek için bir antrenman seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (listViewAntrenmanlar.SelectedItems.Count > 0)
            {
                Antrenman seciliAntrenman = (Antrenman)listViewAntrenmanlar.SelectedItems[0].Tag;
                DialogResult result = MessageBox.Show($"{seciliAntrenman.Tarih.ToShortDateString()} tarihli {seciliAntrenman.TurSecenek} antrenmanını silmek istediğinize emin misiniz?", 
                    "Antrenman Sil", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    if (AntrenmanService.SilAntrenman(seciliAntrenman.AntrenmanID))
                    {
                        YenileAntrenmanListesi();
                        MessageBox.Show("Antrenman başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Antrenman silinirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek için bir antrenman seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void listViewAntrenmanlar_DoubleClick(object sender, EventArgs e)
        {
            if (listViewAntrenmanlar.SelectedItems.Count > 0)
            {
                btnDuzenle_Click(sender, e);
            }
        }

        private void InitializeComponent()
        {
            this.listViewAntrenmanlar = new System.Windows.Forms.ListView();
            this.columnID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnTarih = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnBaslangicSaati = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnBitisSaati = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnTur = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblSonucSayisi = new System.Windows.Forms.Label();
            this.btnYenile = new System.Windows.Forms.Button();
            this.btnYeniAntrenman = new System.Windows.Forms.Button();
            this.btnDuzenle = new System.Windows.Forms.Button();
            this.btnKatilim = new System.Windows.Forms.Button();
            this.btnKatilimGoruntule = new System.Windows.Forms.Button();
            this.btnSil = new System.Windows.Forms.Button();
            this.comboBoxTur = new System.Windows.Forms.ComboBox();
            this.txtArama = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listViewAntrenmanlar
            // 
            this.listViewAntrenmanlar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewAntrenmanlar.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnID,
            this.columnTarih,
            this.columnBaslangicSaati,
            this.columnBitisSaati,
            this.columnTur});
            this.listViewAntrenmanlar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.listViewAntrenmanlar.FullRowSelect = true;
            this.listViewAntrenmanlar.GridLines = true;
            this.listViewAntrenmanlar.HideSelection = false;
            this.listViewAntrenmanlar.Location = new System.Drawing.Point(12, 76);
            this.listViewAntrenmanlar.Name = "listViewAntrenmanlar";
            this.listViewAntrenmanlar.Size = new System.Drawing.Size(958, 469);
            this.listViewAntrenmanlar.TabIndex = 0;
            this.listViewAntrenmanlar.UseCompatibleStateImageBehavior = false;
            this.listViewAntrenmanlar.View = System.Windows.Forms.View.Details;
            this.listViewAntrenmanlar.DoubleClick += new System.EventHandler(this.listViewAntrenmanlar_DoubleClick);
            // 
            // columnID
            // 
            this.columnID.Text = "ID";
            this.columnID.Width = 50;
            // 
            // columnTarih
            // 
            this.columnTarih.Text = "Tarih";
            this.columnTarih.Width = 120;
            // 
            // columnBaslangicSaati
            // 
            this.columnBaslangicSaati.Text = "Başlangıç Saati";
            this.columnBaslangicSaati.Width = 150;
            // 
            // columnBitisSaati
            // 
            this.columnBitisSaati.Text = "Bitiş Saati";
            this.columnBitisSaati.Width = 150;
            // 
            // columnTur
            // 
            this.columnTur.Text = "Tür";
            this.columnTur.Width = 150;
            // 
            // lblSonucSayisi
            // 
            this.lblSonucSayisi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSonucSayisi.AutoSize = true;
            this.lblSonucSayisi.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSonucSayisi.Location = new System.Drawing.Point(12, 555);
            this.lblSonucSayisi.Name = "lblSonucSayisi";
            this.lblSonucSayisi.Size = new System.Drawing.Size(184, 17);
            this.lblSonucSayisi.TabIndex = 1;
            this.lblSonucSayisi.Text = "0 adet antrenman bulundu.";
            // 
            // btnYenile
            // 
            this.btnYenile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnYenile.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnYenile.Location = new System.Drawing.Point(574, 551);
            this.btnYenile.Name = "btnYenile";
            this.btnYenile.Size = new System.Drawing.Size(90, 40);
            this.btnYenile.TabIndex = 2;
            this.btnYenile.Text = "Yenile";
            this.btnYenile.UseVisualStyleBackColor = true;
            this.btnYenile.Click += new System.EventHandler(this.btnYenile_Click);
            // 
            // btnYeniAntrenman
            // 
            this.btnYeniAntrenman.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnYeniAntrenman.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnYeniAntrenman.Location = new System.Drawing.Point(670, 551);
            this.btnYeniAntrenman.Name = "btnYeniAntrenman";
            this.btnYeniAntrenman.Size = new System.Drawing.Size(90, 40);
            this.btnYeniAntrenman.TabIndex = 3;
            this.btnYeniAntrenman.Text = "Yeni";
            this.btnYeniAntrenman.UseVisualStyleBackColor = true;
            this.btnYeniAntrenman.Click += new System.EventHandler(this.btnYeniAntrenman_Click);
            // 
            // btnDuzenle
            // 
            this.btnDuzenle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDuzenle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnDuzenle.Location = new System.Drawing.Point(766, 551);
            this.btnDuzenle.Name = "btnDuzenle";
            this.btnDuzenle.Size = new System.Drawing.Size(90, 40);
            this.btnDuzenle.TabIndex = 4;
            this.btnDuzenle.Text = "Düzenle";
            this.btnDuzenle.UseVisualStyleBackColor = true;
            this.btnDuzenle.Click += new System.EventHandler(this.btnDuzenle_Click);
            // 
            // btnKatilim
            // 
            this.btnKatilim.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKatilim.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnKatilim.Location = new System.Drawing.Point(765, 156);
            this.btnKatilim.Name = "btnKatilim";
            this.btnKatilim.Size = new System.Drawing.Size(205, 40);
            this.btnKatilim.TabIndex = 6;
            this.btnKatilim.Text = "Katılım Düzenle";
            this.btnKatilim.UseVisualStyleBackColor = true;
            this.btnKatilim.Click += new System.EventHandler(this.btnKatilim_Click);
            // 
            // btnKatilimGoruntule
            // 
            this.btnKatilimGoruntule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKatilimGoruntule.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnKatilimGoruntule.Location = new System.Drawing.Point(765, 202);
            this.btnKatilimGoruntule.Name = "btnKatilimGoruntule";
            this.btnKatilimGoruntule.Size = new System.Drawing.Size(205, 40);
            this.btnKatilimGoruntule.TabIndex = 7;
            this.btnKatilimGoruntule.Text = "Katılım Görüntüle";
            this.btnKatilimGoruntule.UseVisualStyleBackColor = true;
            this.btnKatilimGoruntule.Click += new System.EventHandler(this.btnKatilimGoruntule_Click);
            // 
            // btnSil
            // 
            this.btnSil.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSil.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSil.Location = new System.Drawing.Point(765, 248);
            this.btnSil.Name = "btnSil";
            this.btnSil.Size = new System.Drawing.Size(205, 40);
            this.btnSil.TabIndex = 8;
            this.btnSil.Text = "Sil";
            this.btnSil.UseVisualStyleBackColor = true;
            this.btnSil.Click += new System.EventHandler(this.btnSil_Click);
            // 
            // comboBoxTur
            // 
            this.comboBoxTur.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTur.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.comboBoxTur.FormattingEnabled = true;
            this.comboBoxTur.Location = new System.Drawing.Point(64, 16);
            this.comboBoxTur.Name = "comboBoxTur";
            this.comboBoxTur.Size = new System.Drawing.Size(150, 26);
            this.comboBoxTur.TabIndex = 9;
            this.comboBoxTur.SelectedIndexChanged += new System.EventHandler(this.comboBoxTur_SelectedIndexChanged);
            // 
            // txtArama
            // 
            this.txtArama.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtArama.Location = new System.Drawing.Point(306, 16);
            this.txtArama.Name = "txtArama";
            this.txtArama.Size = new System.Drawing.Size(209, 24);
            this.txtArama.TabIndex = 10;
            this.txtArama.TextChanged += new System.EventHandler(this.txtArama_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 18);
            this.label1.TabIndex = 11;
            this.label1.Text = "Tür:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Location = new System.Drawing.Point(242, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 18);
            this.label2.TabIndex = 12;
            this.label2.Text = "Ara:";
            // 
            // AntrenmanListesiForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1060, 603);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtArama);
            this.Controls.Add(this.comboBoxTur);
            this.Controls.Add(this.btnSil);
            this.Controls.Add(this.btnKatilimGoruntule);
            this.Controls.Add(this.btnKatilim);
            this.Controls.Add(this.btnDuzenle);
            this.Controls.Add(this.btnYeniAntrenman);
            this.Controls.Add(this.btnYenile);
            this.Controls.Add(this.lblSonucSayisi);
            this.Controls.Add(this.listViewAntrenmanlar);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "AntrenmanListesiForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Antrenman Listesi";
            this.Load += new System.EventHandler(this.AntrenmanListesiForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ListView listViewAntrenmanlar;
        private System.Windows.Forms.ColumnHeader columnID;
        private System.Windows.Forms.ColumnHeader columnTarih;
        private System.Windows.Forms.ColumnHeader columnBaslangicSaati;
        private System.Windows.Forms.ColumnHeader columnBitisSaati;
        private System.Windows.Forms.ColumnHeader columnTur;
        private System.Windows.Forms.Label lblSonucSayisi;
        private System.Windows.Forms.Button btnYenile;
        private System.Windows.Forms.Button btnYeniAntrenman;
        private System.Windows.Forms.Button btnDuzenle;
        private System.Windows.Forms.Button btnKatilim;
        private System.Windows.Forms.Button btnKatilimGoruntule;
        private System.Windows.Forms.Button btnSil;
        private System.Windows.Forms.ComboBox comboBoxTur;
        private System.Windows.Forms.TextBox txtArama;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
} 