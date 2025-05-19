using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FutbolTakimiYonetimSistemi.Models;
using FutbolTakimiYonetimSistemi.Services;

namespace FutbolTakimiYonetimSistemi.Forms
{
    // Özel key-value sınıfı oluşturuyoruz, böylece ToString metodunu ezebiliriz
    public class FutbolcuItem
    {
        public int FutbolcuID { get; set; }
        public string DisplayText { get; set; }

        public FutbolcuItem(int futbolcuID, string displayText)
        {
            FutbolcuID = futbolcuID;
            DisplayText = displayText;
        }

        public override string ToString()
        {
            return DisplayText;
        }
    }

    public partial class AntrenmanKatilimForm : Form
    {
        private Antrenman _antrenman;
        private List<Futbolcu> _futbolcular;
        private Dictionary<int, bool> _katilimDurumu = new Dictionary<int, bool>();

        public AntrenmanKatilimForm(Antrenman antrenman)
        {
            InitializeComponent();
            _antrenman = antrenman;
        }

        private void AntrenmanKatilimForm_Load(object sender, EventArgs e)
        {
            Text = $"{_antrenman.Tarih.ToShortDateString()} - {_antrenman.TurSecenek} Katılım";
            lblAntrenmanBilgisi.Text = $"Antrenman: {_antrenman.Tarih.ToShortDateString()} - {_antrenman.TurSecenek}";
            lblSaat.Text = $"Saat: {_antrenman.BaslangicSaati.ToShortTimeString()} - {_antrenman.BitisSaati.ToShortTimeString()}";

            YukleFutbolcular();
        }

        private void YukleFutbolcular()
        {
            // Tüm futbolcuları getir
            _futbolcular = FutbolcuService.GetAllFutbolcular();

            // Bu antrenmana ait katılım bilgilerini getir
            List<FutbolcuAntrenman> katilimlar = FutbolcuAntrenmanService.GetByAntrenmanId(_antrenman.AntrenmanID);
            
            // Mevcut katılım bilgilerini dictionary'e ekle
            foreach (var katilim in katilimlar)
            {
                _katilimDurumu[katilim.FutbolcuID] = katilim.Katilim;
            }

            // CheckedListBox'a futbolcuları ekle
            checkedListBoxFutbolcular.Items.Clear();
            foreach (var futbolcu in _futbolcular)
            {
                // Özel sınıfımızı kullanarak yeni bir FutbolcuItem nesnesi oluşturuyoruz
                FutbolcuItem item = new FutbolcuItem(
                    futbolcu.FutbolcuID, 
                    $"{futbolcu.FormaNo} - {futbolcu.TamAd}"
                );
                
                // Listeye ekle
                int index = checkedListBoxFutbolcular.Items.Add(item);
                
                // Eğer katılım bilgisi varsa işaretle
                if (_katilimDurumu.ContainsKey(futbolcu.FutbolcuID) && _katilimDurumu[futbolcu.FutbolcuID])
                {
                    checkedListBoxFutbolcular.SetItemChecked(index, true);
                }
            }

            // Seçili futbolcu sayısını güncelle
            GuncelleSeciliSayisi();
        }

        private void GuncelleSeciliSayisi()
        {
            int seciliSayi = checkedListBoxFutbolcular.CheckedItems.Count;
            lblSeciliSayisi.Text = $"Seçili: {seciliSayi} / {checkedListBoxFutbolcular.Items.Count}";
        }

        private void checkedListBoxFutbolcular_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // İşlem sonrası seçili futbolcu sayısını güncelle
            BeginInvoke(new Action(() => GuncelleSeciliSayisi()));

            // Futbolcu ID'sini al
            int futbolcuId;
            var item = checkedListBoxFutbolcular.Items[e.Index];
            
            if (item is FutbolcuItem futbolcuItem)
            {
                futbolcuId = futbolcuItem.FutbolcuID;
            }
            else
            {
                // Değer FutbolcuItem değilse, ToString ile futbolcu bilgisini gösterelim ve hata oluşmasın
                MessageBox.Show($"Öğe bilgisi alınamadı: {item}", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Dictionary'yi güncelle
            _katilimDurumu[futbolcuId] = e.NewValue == CheckState.Checked;
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                if (FutbolcuAntrenmanService.KaydetKatilim(_antrenman.AntrenmanID, _katilimDurumu))
                {
                    MessageBox.Show("Katılım bilgileri başarıyla kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    YukleFutbolcular(); // Kayıt sonrası katılım listesini yenile
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Katılım bilgileri kaydedilirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Beklenmeyen bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTumunuSec_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxFutbolcular.Items.Count; i++)
            {
                checkedListBoxFutbolcular.SetItemChecked(i, true);
                
                // Futbolcu ID'sini al ve dictionary'ye ekle
                var item = checkedListBoxFutbolcular.Items[i];
                if (item is FutbolcuItem futbolcuItem)
                {
                    _katilimDurumu[futbolcuItem.FutbolcuID] = true;
                }
            }
        }

        private void btnTumunuKaldir_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxFutbolcular.Items.Count; i++)
            {
                checkedListBoxFutbolcular.SetItemChecked(i, false);
                
                // Futbolcu ID'sini al ve dictionary'ye ekle
                var item = checkedListBoxFutbolcular.Items[i];
                if (item is FutbolcuItem futbolcuItem)
                {
                    _katilimDurumu[futbolcuItem.FutbolcuID] = false;
                }
            }
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void InitializeComponent()
        {
            this.lblAntrenmanBilgisi = new System.Windows.Forms.Label();
            this.lblSaat = new System.Windows.Forms.Label();
            this.checkedListBoxFutbolcular = new System.Windows.Forms.CheckedListBox();
            this.btnTumunuSec = new System.Windows.Forms.Button();
            this.btnTumunuKaldir = new System.Windows.Forms.Button();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.btnIptal = new System.Windows.Forms.Button();
            this.lblSeciliSayisi = new System.Windows.Forms.Label();
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
            // checkedListBoxFutbolcular
            // 
            this.checkedListBoxFutbolcular.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxFutbolcular.CheckOnClick = true;
            this.checkedListBoxFutbolcular.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.checkedListBoxFutbolcular.FormattingEnabled = true;
            this.checkedListBoxFutbolcular.Location = new System.Drawing.Point(16, 76);
            this.checkedListBoxFutbolcular.Name = "checkedListBoxFutbolcular";
            this.checkedListBoxFutbolcular.Size = new System.Drawing.Size(356, 289);
            this.checkedListBoxFutbolcular.TabIndex = 2;
            this.checkedListBoxFutbolcular.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxFutbolcular_ItemCheck);
            // 
            // btnTumunuSec
            // 
            this.btnTumunuSec.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTumunuSec.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnTumunuSec.Location = new System.Drawing.Point(16, 381);
            this.btnTumunuSec.Name = "btnTumunuSec";
            this.btnTumunuSec.Size = new System.Drawing.Size(120, 30);
            this.btnTumunuSec.TabIndex = 3;
            this.btnTumunuSec.Text = "Tümünü Seç";
            this.btnTumunuSec.UseVisualStyleBackColor = true;
            this.btnTumunuSec.Click += new System.EventHandler(this.btnTumunuSec_Click);
            // 
            // btnTumunuKaldir
            // 
            this.btnTumunuKaldir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTumunuKaldir.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnTumunuKaldir.Location = new System.Drawing.Point(142, 381);
            this.btnTumunuKaldir.Name = "btnTumunuKaldir";
            this.btnTumunuKaldir.Size = new System.Drawing.Size(120, 30);
            this.btnTumunuKaldir.TabIndex = 4;
            this.btnTumunuKaldir.Text = "Tümünü Kaldır";
            this.btnTumunuKaldir.UseVisualStyleBackColor = true;
            this.btnTumunuKaldir.Click += new System.EventHandler(this.btnTumunuKaldir_Click);
            // 
            // btnKaydet
            // 
            this.btnKaydet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKaydet.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnKaydet.Location = new System.Drawing.Point(282, 422);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(90, 36);
            this.btnKaydet.TabIndex = 5;
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = true;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // btnIptal
            // 
            this.btnIptal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIptal.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnIptal.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnIptal.Location = new System.Drawing.Point(186, 422);
            this.btnIptal.Name = "btnIptal";
            this.btnIptal.Size = new System.Drawing.Size(90, 36);
            this.btnIptal.TabIndex = 6;
            this.btnIptal.Text = "İptal";
            this.btnIptal.UseVisualStyleBackColor = true;
            this.btnIptal.Click += new System.EventHandler(this.btnIptal_Click);
            // 
            // lblSeciliSayisi
            // 
            this.lblSeciliSayisi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSeciliSayisi.AutoSize = true;
            this.lblSeciliSayisi.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSeciliSayisi.Location = new System.Drawing.Point(16, 432);
            this.lblSeciliSayisi.Name = "lblSeciliSayisi";
            this.lblSeciliSayisi.Size = new System.Drawing.Size(60, 17);
            this.lblSeciliSayisi.TabIndex = 7;
            this.lblSeciliSayisi.Text = "Seçili: 0";
            // 
            // AntrenmanKatilimForm
            // 
            this.AcceptButton = this.btnKaydet;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnIptal;
            this.ClientSize = new System.Drawing.Size(384, 470);
            this.Controls.Add(this.lblSeciliSayisi);
            this.Controls.Add(this.btnIptal);
            this.Controls.Add(this.btnKaydet);
            this.Controls.Add(this.btnTumunuKaldir);
            this.Controls.Add(this.btnTumunuSec);
            this.Controls.Add(this.checkedListBoxFutbolcular);
            this.Controls.Add(this.lblSaat);
            this.Controls.Add(this.lblAntrenmanBilgisi);
            this.MinimumSize = new System.Drawing.Size(400, 500);
            this.Name = "AntrenmanKatilimForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Antrenman Katılım";
            this.Load += new System.EventHandler(this.AntrenmanKatilimForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblAntrenmanBilgisi;
        private System.Windows.Forms.Label lblSaat;
        private System.Windows.Forms.CheckedListBox checkedListBoxFutbolcular;
        private System.Windows.Forms.Button btnTumunuSec;
        private System.Windows.Forms.Button btnTumunuKaldir;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.Button btnIptal;
        private System.Windows.Forms.Label lblSeciliSayisi;
    }
} 