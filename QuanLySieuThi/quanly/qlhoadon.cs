using System;
using System.Data;
using System.Windows.Forms;

namespace QuanLySieuThi.quanly
{
    public partial class qlhoadon : Form
    {
        public qlhoadon()
        {
            InitializeComponent();

            // ĐẢM BẢO chắc chắn gắn event, dù designer có quên
            dgvHoaDon.CellClick += dgvHoaDon_CellClick;
        }

        private void qlhoadon_Load(object sender, EventArgs e)
        {
            LoadDanhSachHoaDon();
        }

        //================= LOAD DANH SÁCH HÓA ĐƠN =================
        private void LoadDanhSachHoaDon()
        {
            try
            {
                string sql =
                    "SELECT MaHD, NgayLap, TongTien, PhuongThucThanhToan, GhiChu " +
                    "FROM HoaDon";

                chuoiketnoi.Chuoiketnoi(sql, dgvHoaDon);

                dgvHoaDon.Columns[0].HeaderText = "Mã HĐ";
                dgvHoaDon.Columns[1].HeaderText = "Ngày lập";
                dgvHoaDon.Columns[2].HeaderText = "Tổng tiền";
                dgvHoaDon.Columns[3].HeaderText = "Hình thức TT";
                dgvHoaDon.Columns[4].HeaderText = "Ghi chú";

                dgvHoaDon.Columns[0].Width = 80;
                dgvHoaDon.Columns[1].Width = 120;
                dgvHoaDon.Columns[2].Width = 120;
                dgvHoaDon.Columns[3].Width = 150;
                dgvHoaDon.Columns[4].Width = 200;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load danh sách hóa đơn: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //============= HÀM CHỐNG NULL -> "Không có" =============
        private string SafeValue(object value)
        {
            if (value == null)
                return "Không có";

            string txt = value.ToString().Trim();
            if (string.IsNullOrEmpty(txt))
                return "Không có";

            return txt;
        }

        //================= LOAD CHI TIẾT HÓA ĐƠN =================
        private void LoadChiTietHoaDon(int maHD)
        {
            try
            {
                string sql =
                    "SELECT c.MaSP, s.TenSP, c.SoLuong, c.DonGia, c.ThanhTien " +
                    "FROM ChiTietHoaDon c " +
                    "JOIN SanPham s ON c.MaSP = s.MaSP " +
                    "WHERE c.MaHD = " + maHD;

                chuoiketnoi.Chuoiketnoi(sql, dgvChiTietHoaDon);

                dgvChiTietHoaDon.Columns[0].HeaderText = "Mã SP";
                dgvChiTietHoaDon.Columns[1].HeaderText = "Tên sản phẩm";
                dgvChiTietHoaDon.Columns[2].HeaderText = "Số lượng";
                dgvChiTietHoaDon.Columns[3].HeaderText = "Đơn giá";
                dgvChiTietHoaDon.Columns[4].HeaderText = "Thành tiền";

                dgvChiTietHoaDon.Columns[0].Width = 80;
                dgvChiTietHoaDon.Columns[1].Width = 200;
                dgvChiTietHoaDon.Columns[2].Width = 80;
                dgvChiTietHoaDon.Columns[3].Width = 120;
                dgvChiTietHoaDon.Columns[4].Width = 120;

                // DEBUG: cho bạn thấy thực sự có load
                // MessageBox.Show("Đã load " + (dgvChiTietHoaDon.Rows.Count - 1) + " dòng chi tiết.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load chi tiết hóa đơn: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //================= CLICK VÀO DÒNG HÓA ĐƠN =================
        private void dgvHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvHoaDon.Rows[e.RowIndex];

            // Mã HĐ
            txtMaHD.Text = SafeValue(row.Cells[0].Value);

            // Ngày lập
            DateTime ngayLap;
            if (DateTime.TryParse(row.Cells[1].Value?.ToString(), out ngayLap))
                dte_NgayLap.Value = ngayLap;
            else
                dte_NgayLap.Value = DateTime.Now;

            // Tổng tiền
            txt_TongTien.Text = SafeValue(row.Cells[2].Value);

            // PT thanh toán
            txtPTTT.Text = SafeValue(row.Cells[3].Value);

            // Nhân viên / Khách hàng tạm thời
            txtNhanVien.Text = "Không có";
            txtKhachHang.Text = "Không có";

            // Lấy mã HĐ dạng int rồi load chi tiết
            int maHD;
            if (int.TryParse(row.Cells[0].Value?.ToString(), out maHD))
            {
                LoadChiTietHoaDon(maHD);
            }
            else
            {
                MessageBox.Show("Mã hóa đơn không hợp lệ, không thể tải chi tiết!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_in_Click(object sender, EventArgs e)
        {

        }

        private void bnt_sua_Click(object sender, EventArgs e)
        {

        }
    }
}
