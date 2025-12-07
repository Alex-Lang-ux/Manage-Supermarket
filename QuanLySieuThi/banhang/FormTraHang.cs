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
            this.Load += FormTraHang_Load;
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // 0. Kiểm tra đã nhập số lượng trả chưa
                if (string.IsNullOrWhiteSpace(txtSoLuongTra.Text))
                {
                    MessageBox.Show("Bạn chưa nhập số lượng trả!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoLuongTra.Focus();
                    return;
                }

                int soLuongTra;
                if (!int.TryParse(txtSoLuongTra.Text, out soLuongTra) || soLuongTra <= 0)
                {
                    MessageBox.Show("Số lượng trả phải là số nguyên dương!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoLuongTra.Focus();
                    return;
                }

                // 1. Kiểm tra đã chọn dòng nào bên dgvCTHD chưa
                if (dgvCTHD.CurrentRow == null || dgvCTHD.CurrentRow.Index < 0)
                {
                    MessageBox.Show("Bạn chưa chọn sản phẩm cần trả trong hóa đơn!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow rowSrc = dgvCTHD.CurrentRow;

                // Lấy thông tin từ dgvCTHD
                string maSP = rowSrc.Cells[0].Value?.ToString();
                string tenSP = rowSrc.Cells[1].Value?.ToString();
                string loaiSP = rowSrc.Cells[2].Value?.ToString();
                string soLuongMuaStr = rowSrc.Cells[3].Value?.ToString();
                string donGiaStr = rowSrc.Cells[4].Value?.ToString();

                if (string.IsNullOrEmpty(maSP))
                {
                    MessageBox.Show("Dòng bạn chọn không hợp lệ!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int soLuongMua = 0;
                int.TryParse(soLuongMuaStr, out soLuongMua);

                // 2. Kiểm tra số lượng trả không vượt quá số lượng mua
                if (soLuongTra > soLuongMua)
                {
                    MessageBox.Show("Số lượng trả không được lớn hơn số lượng đã mua (" + soLuongMua + ")!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 3. Không cho thêm trùng sản phẩm trong danh sách trả
                foreach (DataGridViewRow r in dgvSanPhamTra.Rows)
                {
                    if (r.IsNewRow) continue;
                    if (r.Cells[0].Value != null && r.Cells[0].Value.ToString() == maSP)
                    {
                        MessageBox.Show("Sản phẩm này đã có trong danh sách trả!\n" +
                                        "Bạn có thể sửa số lượng trả trực tiếp trên bảng.",
                                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                decimal donGia = 0;
                decimal.TryParse(donGiaStr, out donGia);

                // Thành tiền trả = đơn giá * số lượng trả
                decimal thanhTienTra = donGia * soLuongTra;

                // 4. Thêm 1 dòng mới vào dgvSanPhamTra
                int n = dgvSanPhamTra.Rows.Add();
                dgvSanPhamTra.Rows[n].Cells[0].Value = maSP;                       // Mã sản phẩm
                dgvSanPhamTra.Rows[n].Cells[1].Value = tenSP;                      // Tên
                dgvSanPhamTra.Rows[n].Cells[2].Value = loaiSP;                     // Loại
                dgvSanPhamTra.Rows[n].Cells[3].Value = soLuongMua.ToString();      // Số lượng mua
                dgvSanPhamTra.Rows[n].Cells[4].Value = soLuongTra.ToString();      // Số lượng trả
                dgvSanPhamTra.Rows[n].Cells[5].Value = donGia.ToString();          // Đơn giá
                dgvSanPhamTra.Rows[n].Cells[6].Value = thanhTienTra.ToString();    // Thành tiền trả

                MessageBox.Show("Đã thêm sản phẩm vào danh sách trả!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Xóa textbox số lượng để lần sau nhập lại
                txtSoLuongTra.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm sản phẩm trả: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXacNhanThanhToan_Click(object sender, EventArgs e)
        {

            try
            {
                // ================== 0. KIỂM TRA CƠ BẢN ==================
                if (string.IsNullOrWhiteSpace(txtMaHD.Text))
                {
                    MessageBox.Show("Bạn chưa nhập mã hóa đơn!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int maHD;
                if (!int.TryParse(txtMaHD.Text, out maHD))
                {
                    MessageBox.Show("Mã hóa đơn không hợp lệ!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra có sản phẩm nào trong danh sách trả chưa
                bool coDong = dgvSanPhamTra.Rows.Cast<DataGridViewRow>()
                                                .Any(r => !r.IsNewRow);
                if (!coDong)
                {
                    MessageBox.Show("Chưa có sản phẩm nào trong danh sách trả!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lý do trả hàng (nếu có combobox)
                string lyDo = "Trả hàng";
                if (cbbLyDoTra != null && cbbLyDoTra.SelectedItem != null)
                    lyDo = cbbLyDoTra.SelectedItem.ToString();

                // Phương thức hoàn tiền: CHỈ TIỀN MẶT
                string phuongThuc = "Tiền mặt";

                // ================== 1. LẤY TỔNG TIỀN CŨ CỦA HÓA ĐƠN ==================
                decimal tongCu = 0;
                string sqlGetTongCu =
                    "SELECT ISNULL(TongTien,0) FROM HoaDon WHERE MaHD = " + maHD;

                using (SqlDataReader rdOldTong = chuoiketnoi.showtext(sqlGetTongCu))
                {
                    if (rdOldTong.Read())
                        tongCu = decimal.Parse(rdOldTong[0].ToString());
                }

                // ================== 2. LẤY MÃ PHIẾU TRẢ HÀNG MỚI ==================
                int maPhieu = 1;
                string sqlGetMa =
                    "SELECT ISNULL(MAX(MaPhieu),0) + 1 FROM PhieuTraHang";

                using (SqlDataReader rd = chuoiketnoi.showtext(sqlGetMa))
                {
                    if (rd.Read())
                        maPhieu = int.Parse(rd[0].ToString());
                }

                string ngayLapStr = DateTime.Now.ToString("yyyy-MM-dd");
                int hopLeThoiGian = 1; // bạn đã kiểm tra 30 ngày khi check hóa đơn rồi
                int lapPhieuTra = 1;
                string maNV = "NULL"; // Nếu chưa có mã NV đăng nhập thì để NULL

                // ================== 3. THÊM VÀO PHIEUTRAHANG ==================
                // Lưu ý: bảng PhieuTraHang của bạn phải có cột PhuongThuc, MaHD như mình đã hướng dẫn trước đó
                string insertPhieu =
                    "INSERT INTO PhieuTraHang(MaPhieu, NgayLap, LyDo, PhuongThuc, " +
                    "KiemTraThoiGianHopLe, LapPhieuTra, MaNV, MaHD) VALUES(" +
                    maPhieu + ", '" +
                    ngayLapStr + "', N'" +
                    lyDo.Replace("'", "''") + "', N'" +
                    phuongThuc + "', " +
                    hopLeThoiGian + ", " +
                    lapPhieuTra + ", " +
                    maNV + ", " +
                    maHD + ")";

                chuoiketnoi.them_dl1(insertPhieu);

                // ================== 4. THÊM CHI TIẾT PHIẾU TRẢ HÀNG ==================
                string ngayTraStr = DateTime.Now.ToString("yyyy-MM-dd");
                decimal tongTienHoanLai = 0;

                foreach (DataGridViewRow row in dgvSanPhamTra.Rows)
                {
                    if (row.IsNewRow) continue;

                    int maSP, soLuongTra;
                    decimal soTienTra;

                    if (!int.TryParse(row.Cells[0].Value?.ToString(), out maSP))
                        continue;

                    if (!int.TryParse(row.Cells[4].Value?.ToString(), out soLuongTra))
                        continue;

                    if (!decimal.TryParse(row.Cells[6].Value?.ToString(), out soTienTra))
                        soTienTra = 0;

                    tongTienHoanLai += soTienTra;

                    string tinhTrang = "";
                    // Nếu bạn đã thêm cột tình trạng hàng trong dgvSanPhamTra:
                    // tinhTrang = row.Cells["TinhTrangHang"].Value?.ToString() ?? "";

                    string insertCT =
                        "INSERT INTO ChiTietPhieuTraHang(MaPhieu, MaSP, SoLuongTra, NgayTra, SoTienTra, TinhTrangHang) " +
                        "VALUES(" +
                        maPhieu + ", " +
                        maSP + ", " +
                        soLuongTra + ", '" +
                        ngayTraStr + "', " +
                        soTienTra.ToString().Replace(',', '.') + ", N'" +
                        tinhTrang.Replace("'", "''") + "')";

                    chuoiketnoi.them_dl1(insertCT);
                }

                // ================== 5. CẬP NHẬT CHITIETHOADON + TỒN KHO ==================
                foreach (DataGridViewRow row in dgvSanPhamTra.Rows)
                {
                    if (row.IsNewRow) continue;

                    int maSP, soLuongTra;
                    decimal thanhTienTra;

                    if (!int.TryParse(row.Cells[0].Value?.ToString(), out maSP))
                        continue;

                    if (!int.TryParse(row.Cells[4].Value?.ToString(), out soLuongTra))
                        continue;

                    if (!decimal.TryParse(row.Cells[6].Value?.ToString(), out thanhTienTra))
                        thanhTienTra = 0;

                    // 5.1. Cập nhật chi tiết hóa đơn
                    int soLuongCuTrongHD = 0;
                    decimal thanhTienCuTrongHD = 0;

                    string sqlGetOld =
                        "SELECT SoLuong, ThanhTien FROM ChiTietHoaDon " +
                        "WHERE MaHD = " + maHD + " AND MaSP = " + maSP;

                    using (SqlDataReader rdOld = chuoiketnoi.showtext(sqlGetOld))
                    {
                        if (rdOld.Read())
                        {
                            soLuongCuTrongHD = int.Parse(rdOld["SoLuong"].ToString());
                            thanhTienCuTrongHD = decimal.Parse(rdOld["ThanhTien"].ToString());
                        }
                    }

                    if (soLuongCuTrongHD > 0)
                    {
                        if (soLuongCuTrongHD <= soLuongTra)
                        {
                            // Trả hết -> xóa dòng chi tiết
                            string sqlDel =
                                "DELETE FROM ChiTietHoaDon WHERE MaHD = " + maHD +
                                " AND MaSP = " + maSP;
                            chuoiketnoi.luu(sqlDel);
                        }
                        else
                        {
                            // Trả một phần -> giảm số lượng & thành tiền theo phần trả
                            string sqlUpdOld =
                                "UPDATE ChiTietHoaDon SET " +
                                "SoLuong = SoLuong - " + soLuongTra + ", " +
                                "ThanhTien = ThanhTien - " + thanhTienTra.ToString().Replace(',', '.') +
                                " WHERE MaHD = " + maHD + " AND MaSP = " + maSP;
                            chuoiketnoi.luu(sqlUpdOld);
                        }
                    }

                    // 5.2. Cập nhật tồn kho: trả hàng về kho -> tăng SoLuongTon
                    string sqlUpTon =
                        "UPDATE SanPham SET SoLuongTon = SoLuongTon + " + soLuongTra +
                        " WHERE MaSP = " + maSP;
                    chuoiketnoi.luu(sqlUpTon);
                }

                // ================== 6. CẬP NHẬT LẠI TỔNG TIỀN HÓA ĐƠN ==================
                decimal tongMoi = 0;
                string sqlTong =
                    "SELECT SUM(ThanhTien) FROM ChiTietHoaDon WHERE MaHD = " + maHD;

                using (SqlDataReader rdTong = chuoiketnoi.showtext(sqlTong))
                {
                    if (rdTong.Read() && rdTong[0] != DBNull.Value)
                        tongMoi = decimal.Parse(rdTong[0].ToString());
                }

                string sqlUpdateHD =
                    "UPDATE HoaDon SET TongTien = " +
                    tongMoi.ToString().Replace(',', '.') +
                    " WHERE MaHD = " + maHD;
                chuoiketnoi.luu(sqlUpdateHD);

               
                decimal delta = tongMoi - tongCu;

                if (delta < 0)
                {
                    decimal hoanLai = Math.Abs(delta);
                    MessageBox.Show(
                        "Lập phiếu trả hàng thành công!\n" +
                        "Siêu thị cần hoàn lại cho khách: " +
                        hoanLai.ToString("N0") + " VNĐ (TIỀN MẶT).",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (delta == 0)
                {
                    MessageBox.Show(
                        "Lập phiếu trả hàng thành công!\nKhông có chênh lệch tiền.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    
                    MessageBox.Show(
                        "Lập phiếu trả hàng thành công!\n(Lưu ý: tổng tiền hóa đơn tăng, vui lòng kiểm tra lại nghiệp vụ.)",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

               
                ResetFormTraHang();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lập phiếu trả hàng: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormTraHang_Load(object sender, EventArgs e)
        {
            dgvSanPhamTra.AutoGenerateColumns = false;
            dgvSanPhamTra.Columns.Clear();

            dgvSanPhamTra.Columns.Add("MaSP", "Mã sản phẩm");    // 0
            dgvSanPhamTra.Columns.Add("TenSP", "Tên sản phẩm");   // 1
            dgvSanPhamTra.Columns.Add("LoaiSP", "Loại sản phẩm");  // 2
            dgvSanPhamTra.Columns.Add("SLMua", "Số lượng mua");   // 3
            dgvSanPhamTra.Columns.Add("SLTra", "Số lượng trả");   // 4
            dgvSanPhamTra.Columns.Add("DonGia", "Đơn giá");
            dgvSanPhamTra.Columns.Add("ThanhTien", "Thành tiền trả");
        }

        private void dgvCTHD_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void ResetFormTraHang()
        {
            // Xóa input
            txtMaHD.Clear();

            // Reset lý do nếu có
            if (cbbLyDoTra != null)
                cbbLyDoTra.SelectedIndex = -1;

            // Xóa chi tiết hóa đơn
            dgvCTHD.DataSource = null;
            dgvCTHD.Rows.Clear();

            // Xóa danh sách sản phẩm trả
            dgvSanPhamTra.DataSource = null;
            dgvSanPhamTra.Rows.Clear();
        }


    }
}
