using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLySieuThi.banhang
{
    public partial class QuetQr : Form
    {
        private decimal _soTien;
        public QuetQr()
        {
            InitializeComponent();
        }

        private void timerCheckPayment_Tick(object sender, EventArgs e)
        {

        }
        public QuetQr(decimal soTien)
        {
            InitializeComponent();
            _soTien = soTien;
        }
        private void QuetQr_Load(object sender, EventArgs e)
        {
            
            lblGiaTien.Text = _soTien.ToString("N0", new CultureInfo("vi-VN")) + " VNĐ";
          

            LoadVietQR();
        }
        private void LoadVietQR()
        {
            try
            {
                // GIẢ LẬP QR bằng VietQR QuickLink (không cần đăng ký gì)
                // Mẫu: https://img.vietqr.io/image/<bank>-<account>-compact2.jpg?amount=1000&addInfo=NoiDung&accountName=Ten
                string bankCode = "vietinbank";       // mã ngân hàng, dùng đại cho demo
                string accountNo = "101253246";        // số tài khoản
                string template = "compact2";

                long amount = (long)_soTien;             // VietQR nhận số nguyên

                string addInfo = Uri.EscapeDataString("Quet ma");
                string accountName = Uri.EscapeDataString("SIEUTHI");

                string url = $"https://img.vietqr.io/image/{bankCode}-{accountNo}-{template}.jpg" +
                             $"?amount={amount}&addInfo={addInfo}&accountName={accountName}";

                using (var wc = new WebClient())
                {
                    byte[] data = wc.DownloadData(url);
                    using (var ms = new MemoryStream(data))
                    {
                        picQR.Image = Image.FromStream(ms);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được QR: " + ex.Message, "Lỗi", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void picQR_Click(object sender, EventArgs e)
        {

        }
    }
}
