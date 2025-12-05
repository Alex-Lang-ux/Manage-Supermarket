using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLySieuThi.banhang
{
    public partial class FormTraHang : Form
    {
        public FormTraHang()
        {
            InitializeComponent();
        }

        private void btnThemSP_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaHD.Text))
                {
                    MessageBox.Show("Bạn chưa nhập mã hóa đơn!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int maHD;
                if (!int.TryParse(txtMaHD.Text, out maHD))
                {
                    MessageBox.Show("Mã hóa đơn phải là số nguyên!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 1. Kiểm tra hóa đơn có tồn tại không
                string sqlCheck = "SELECT COUNT(*) FROM HoaDon WHERE MaHD = " + maHD;
                int soHoaDon = 0;
                using (SqlDataReader rd = chuoiketnoi.showtext(sqlCheck))
                {
                    if (rd.Read())
                        soHoaDon = int.Parse(rd[0].ToString());
                }

                if (soHoaDon == 0)
                {
                    MessageBox.Show("Hóa đơn không tồn tại!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    dgvCTHD.Rows.Clear();
                    lblNgayLap.Text = "";
                    return;
                }

                // 1.1. Lấy ngày lập hóa đơn
                string sqlNgay = "SELECT NgayLap FROM HoaDon WHERE MaHD = " + maHD;
                DateTime ngayLap = DateTime.Now;

                using (SqlDataReader rdNgay = chuoiketnoi.showtext(sqlNgay))
                {
                    if (rdNgay.Read())
                    {
                        ngayLap = Convert.ToDateTime(rdNgay[0]);
                        lblNgayLap.Text = ngayLap.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        lblNgayLap.Text = "";
                    }
                }

                // ================================
                // 1.2. KIỂM TRA HẠN ĐỔI TRẢ (30 NGÀY)
                // ================================
                TimeSpan khoangCach = DateTime.Now - ngayLap;

                if (khoangCach.TotalDays > 30)
                {
                    MessageBox.Show(
                        "Hóa đơn đã quá 30 ngày kể từ ngày mua (" +
                        ngayLap.ToString("dd/MM/yyyy") +
                        "). Không thể thực hiện đổi trả!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    dgvCTHD.Rows.Clear();   // Không cho load chi tiết
                    return;
                }

                // 2. Load chi tiết hóa đơn
                string sqlLoadCT =
                    "SELECT cthd.MaSP, sp.TenSP, sp.LoaiSP, cthd.SoLuong, cthd.DonGia, cthd.ThanhTien " +
                    "FROM ChiTietHoaDon cthd " +
                    "INNER JOIN SanPham sp ON cthd.MaSP = sp.MaSP " +
                    "WHERE cthd.MaHD = " + maHD;

                chuoiketnoi.Chuoiketnoi(sqlLoadCT, dgvCTHD);

                // 3. Kiểm tra chi tiết
                if (dgvCTHD.Rows.Count == 0)
                {
                    MessageBox.Show("Hóa đơn tồn tại nhưng không có chi tiết!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 4. Set header
                dgvCTHD.Columns[0].HeaderText = "Mã sản phẩm";
                dgvCTHD.Columns[1].HeaderText = "Tên sản phẩm";
                dgvCTHD.Columns[2].HeaderText = "Loại sản phẩm";
                dgvCTHD.Columns[3].HeaderText = "Số lượng mua";
                dgvCTHD.Columns[4].HeaderText = "Đơn giá";
                dgvCTHD.Columns[5].HeaderText = "Thành tiền";

                MessageBox.Show("Tìm thấy hóa đơn. Vui lòng chọn sản phẩm cần đổi!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi kiểm tra hóa đơn: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
