using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FutbolTakimiYonetimSistemi.Models;
using FutbolTakimiYonetimSistemi.Services;

namespace FutbolTakimiYonetimSistemi.Forms
{
    public partial class AntrenmanKatilimGoruntuleForm : Form
    {
        private Antrenman _antrenman;
        private List<FutbolcuAntrenman> _katilimlar;

        public AntrenmanKatilimGoruntuleForm(Antrenman antrenman)
        {
            InitializeComponent();
            _antrenman = antrenman;
        }

        private void AntrenmanKatilimGoruntuleForm_Load(object sender, EventArgs e)
        {
            Text = $"{_antrenman.Tarih.ToShortDateString()} - {_antrenman.TurSecenek} Katılım Listesi";
            lblAntrenmanBilgisi.Text = $"Antrenman: {_antrenman.Tarih.ToShortDateString()} - {_antrenman.TurSecenek}";
            lblSaat.Text = $"Saat: {_antrenman.BaslangicSaati.ToShortTimeString()} - {_antrenman.BitisSaati.ToShortTimeString()}";

            YukleKatilimlar();
        }

        private void YukleKatilimlar()
        {
            try
            {
                // Bu antrenmana ait katılım bilgilerini getir
                _katilimlar = FutbolcuAntrenmanService.GetByAntrenmanId(_antrenman.AntrenmanID);
                
                listViewKatilimlar.Items.Clear();
                
                int katilanSayisi = 0;
                foreach (var katilim in _katilimlar)
                {
                    if (katilim.Futbolcu == null) continue;

                    ListViewItem item = new ListViewItem(katilim.Futbolcu.FormaNo.ToString());
                    item.SubItems.Add(katilim.Futbolcu.TamAd);
                    item.SubItems.Add(katilim.KatilimText);
                    
                    // Katılım durumuna göre satır rengini değiştir
                    if (katilim.Katilim)
                    {
                        item.BackColor = Color.LightGreen;
                        katilanSayisi++;
                    }
                    else
                    {
                        item.BackColor = Color.LightPink;
                    }
                    
                    listViewKatilimlar.Items.Add(item);
                }
                
                lblKatilimOzeti.Text = $"Toplam: {_katilimlar.Count} Oyuncu, Katılan: {katilanSayisi}, Katılmayan: {_katilimlar.Count - katilanSayisi}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Katılım listesi yüklenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDuzenle_Click(object sender, EventArgs e)
        {
            // Katılım düzenleme formunu açıyoruz
            AntrenmanKatilimForm katilimForm = new AntrenmanKatilimForm(_antrenman);
            if (katilimForm.ShowDialog() == DialogResult.OK)
            {
                // Düzenleme sonrası listeyi yenile
                YukleKatilimlar();
            }
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InitializeComponent()
        {
            this.lblAntrenmanBilgisi = new System.Windows.Forms.Label();
            this.lblSaat = new System.Windows.Forms.Label();
            this.listViewKatilimlar = new System.Windows.Forms.ListView();
            this.columnFormaNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnAdSoyad = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnKatilimDurumu = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnDuzenle = new System.Windows.Forms.Button();
            this.btnKapat = new System.Windows.Forms.Button();
            this.lblKatilimOzeti = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblAntrenmanBilgisi
            // 
            this.lblAntrenmanBilgisi.AutoSize = true;
            this.lblAntrenmanBilgisi.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblAntrenmanBilgisi.Location = new System.Drawing.Point(12, 12);
            this.lblAntrenmanBilgisi.Name = "lblAntrenmanBilgisi";
            this.lblAntrenmanBilgisi.Size = new System.Drawing.Size(95, 20);
            this.lblAntrenmanBilgisi.TabIndex = 0;
            this.lblAntrenmanBilgisi.Text = "Antrenman";
            // 
            // lblSaat
            // 
            this.lblSaat.AutoSize = true;
            this.lblSaat.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSaat.Location = new System.Drawing.Point(13, 42);
            this.lblSaat.Name = "lblSaat";
            this.lblSaat.Size = new System.Drawing.Size(39, 18);
            this.lblSaat.TabIndex = 1;
            this.lblSaat.Text = "Saat";
            // 
            // listViewKatilimlar
            // 
            this.listViewKatilimlar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewKatilimlar.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnFormaNo,
            this.columnAdSoyad,
            this.columnKatilimDurumu});
            this.listViewKatilimlar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.listViewKatilimlar.FullRowSelect = true;
            this.listViewKatilimlar.GridLines = true;
            this.listViewKatilimlar.HideSelection = false;
            this.listViewKatilimlar.Location = new System.Drawing.Point(16, 76);
            this.listViewKatilimlar.Name = "listViewKatilimlar";
            this.listViewKatilimlar.Size = new System.Drawing.Size(456, 310);
            this.listViewKatilimlar.TabIndex = 2;
            this.listViewKatilimlar.UseCompatibleStateImageBehavior = false;
            this.listViewKatilimlar.View = System.Windows.Forms.View.Details;
            // 
            // columnFormaNo
            // 
            this.columnFormaNo.Text = "Forma No";
            this.columnFormaNo.Width = 80;
            // 
            // columnAdSoyad
            // 
            this.columnAdSoyad.Text = "Ad Soyad";
            this.columnAdSoyad.Width = 220;
            // 
            // columnKatilimDurumu
            // 
            this.columnKatilimDurumu.Text = "Durumu";
            this.columnKatilimDurumu.Width = 120;
            // 
            // btnDuzenle
            // 
            this.btnDuzenle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDuzenle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnDuzenle.Location = new System.Drawing.Point(255, 402);
            this.btnDuzenle.Name = "btnDuzenle";
            this.btnDuzenle.Size = new System.Drawing.Size(120, 36);
            this.btnDuzenle.TabIndex = 5;
            this.btnDuzenle.Text = "Düzenle";
            this.btnDuzenle.UseVisualStyleBackColor = true;
            this.btnDuzenle.Click += new System.EventHandler(this.btnDuzenle_Click);
            // 
            // btnKapat
            // 
            this.btnKapat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKapat.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnKapat.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnKapat.Location = new System.Drawing.Point(381, 402);
            this.btnKapat.Name = "btnKapat";
            this.btnKapat.Size = new System.Drawing.Size(91, 36);
            this.btnKapat.TabIndex = 6;
            this.btnKapat.Text = "Kapat";
            this.btnKapat.UseVisualStyleBackColor = true;
            this.btnKapat.Click += new System.EventHandler(this.btnKapat_Click);
            // 
            // lblKatilimOzeti
            // 
            this.lblKatilimOzeti.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblKatilimOzeti.AutoSize = true;
            this.lblKatilimOzeti.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblKatilimOzeti.Location = new System.Drawing.Point(16, 402);
            this.lblKatilimOzeti.Name = "lblKatilimOzeti";
            this.lblKatilimOzeti.Size = new System.Drawing.Size(161, 17);
            this.lblKatilimOzeti.TabIndex = 7;
            this.lblKatilimOzeti.Text = "Toplam: 0, Katılan: 0";
            // 
            // AntrenmanKatilimGoruntuleForm
            // 
            this.AcceptButton = this.btnDuzenle;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnKapat;
            this.ClientSize = new System.Drawing.Size(484, 450);
            this.Controls.Add(this.lblKatilimOzeti);
            this.Controls.Add(this.btnKapat);
            this.Controls.Add(this.btnDuzenle);
            this.Controls.Add(this.listViewKatilimlar);
            this.Controls.Add(this.lblSaat);
            this.Controls.Add(this.lblAntrenmanBilgisi);
            this.MinimumSize = new System.Drawing.Size(500, 480);
            this.Name = "AntrenmanKatilimGoruntuleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Antrenman Katılım Listesi";
            this.Load += new System.EventHandler(this.AntrenmanKatilimGoruntuleForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblAntrenmanBilgisi;
        private System.Windows.Forms.Label lblSaat;
        private System.Windows.Forms.ListView listViewKatilimlar;
        private System.Windows.Forms.ColumnHeader columnFormaNo;
        private System.Windows.Forms.ColumnHeader columnAdSoyad;
        private System.Windows.Forms.ColumnHeader columnKatilimDurumu;
        private System.Windows.Forms.Button btnDuzenle;
        private System.Windows.Forms.Button btnKapat;
        private System.Windows.Forms.Label lblKatilimOzeti;
    }
} 