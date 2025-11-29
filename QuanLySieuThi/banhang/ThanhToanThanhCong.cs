using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLySieuThi.Properties;
using static QuanLySieuThi.XuatExecl;

namespace QuanLySieuThi.banhang
{
    public partial class ThanhToanThanhCong : Form
    {
        public ThanhToanThanhCong()
        {
            InitializeComponent();
        }
        private int _maHD;
        private DataGridView _dgvChiTiet;
        private string _tongTienText;
        private string _tienNhanText;
        private string _tienThoiText;
        private string _hinhThucTT;

        public ThanhToanThanhCong(
            int maHD,
            DataGridView dgvChiTiet,
            string tongTienText,
            string tienNhanText,
            string tienThoiText,
            string hinhThucThanhToan)
        {
            InitializeComponent();
            _maHD = maHD;
            _dgvChiTiet = dgvChiTiet;
            _tongTienText = tongTienText;
            _tienNhanText = tienNhanText;
            _tienThoiText = tienThoiText;
            _hinhThucTT = hinhThucThanhToan;
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
           
        }

        private void btn_in_Click(object sender, EventArgs e)
        {
            if (_dgvChiTiet == null || _dgvChiTiet.Rows.Count <= 1)
            {
                MessageBox.Show("Không có dữ liệu chi tiết hóa đơn để xuất!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel file (*.xlsx)|*.xlsx";
            sfd.FileName = "HoaDon_" + _maHD + ".xlsx";

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            string basePath = Path.Combine(
                Path.GetDirectoryName(sfd.FileName),
                Path.GetFileNameWithoutExtension(sfd.FileName));

            // thông tin siêu thị – bạn sửa lại cho đúng
            string tenSieuThi = "SIÊU THỊ SIEUTHI";
            string diaChi = "123 Đường ABC, Quận XYZ, TP.HCM";

            // gọi hàm xuất Excel + PDF + mở PDF
            XuatHoaDon.ExportToExcelAndPdf(
                _dgvChiTiet,
                basePath,
                tenSieuThi,
                diaChi,
                _maHD.ToString(),
                DateTime.Now,
                _hinhThucTT,
                _tongTienText,
                _tienNhanText,
                _tienThoiText);

            MessageBox.Show("Đã xuất hóa đơn Excel và PDF!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        
        }

        private void ThanhToanThanhCong_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn thoát không?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                this.Close();
            }
        }
    }
}
