using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLySieuThi.banhang
{
    public partial class PhieuNhap : Form
    {
        // Lấy thông tin chi tiết phiếu nhập + SP + NV + NCC
        public string chuoi = @"
SELECT 
    ctn.MaCTPN,           -- 0
    ctn.MaPhieu,          -- 1
    ctn.MaSP,             -- 2
    sp.TenSP,             -- 3
    ctn.SoLuong,          -- 4
    ctn.GiaNhap,          -- 5
    ctn.MaNV,             -- 6
    nv.HoTen AS TenNV,    -- 7
    ctn.NgayTao,          -- 8
    ctn.ThanhTien,        -- 9
    ctn.CongNo,           -- 10
    ctn.TrangThai,        -- 11
    pn.MaNCC,             -- 12
    ncc.TenNCC            -- 13
FROM ChiTietPhieuNhap ctn
INNER JOIN PhieuNhapHang pn ON ctn.MaPhieu = pn.MaPhieu
INNER JOIN SanPham sp ON ctn.MaSP = sp.MaSP
INNER JOIN NhanVienKho nk ON ctn.MaNV = nk.MaNV
INNER JOIN NhanVien nv ON nk.MaNV = nv.MaNV
INNER JOIN NhaCungCap ncc ON pn.MaNCC = ncc.MaNCC";

        // Danh sách sản phẩm để chọn
        public string chuoi1 = "SELECT MaSP, TenSP, DonGia, SoLuongTon, LoaiSP FROM SanPham";

        public int a = 0;            // số lượng tồn SP đang chọn
        public decimal key = 0;      // thành tiền hiện tại

        public PhieuNhap()
        {
            InitializeComponent();

            // Load dữ liệu ban đầu
            chuoiketnoi.Chuoiketnoi(chuoi, dta1);
            chuoiketnoi.Chuoiketnoi(chuoi1, dta2);
            TenBang();
            LoadNCC();
            LoadNhanVien();
            HookEvents();
            ClearForm();
        }

        // Đặt header cho 2 DataGridView
        public void TenBang()
        {
            // dta2: danh sách sản phẩm
            if (dta2.Columns.Count >= 5)
            {
                dta2.Columns[0].HeaderText = "Mã SP"; dta2.Columns[0].Width = 100;
                dta2.Columns[1].HeaderText = "Tên SP"; dta2.Columns[1].Width = 150;
                dta2.Columns[2].HeaderText = "Đơn giá bán"; dta2.Columns[2].Width = 110;
                dta2.Columns[3].HeaderText = "SL tồn"; dta2.Columns[3].Width = 100;
                dta2.Columns[4].HeaderText = "Loại SP"; dta2.Columns[4].Width = 110;
            }

            // dta1: Chi tiết phiếu nhập
            if (dta1.Columns.Count >= 14)
            {
                dta1.Columns[0].HeaderText = "Mã CTPN";
                dta1.Columns[1].HeaderText = "Mã phiếu";
                dta1.Columns[2].HeaderText = "Mã SP";
                dta1.Columns[3].HeaderText = "Tên SP";
                dta1.Columns[4].HeaderText = "Số lượng";
                dta1.Columns[5].HeaderText = "Giá nhập";
                dta1.Columns[6].HeaderText = "Mã NV";
                dta1.Columns[7].HeaderText = "Tên NV";
                dta1.Columns[8].HeaderText = "Ngày nhập";
                dta1.Columns[9].HeaderText = "Thành tiền";
                dta1.Columns[10].HeaderText = "Công nợ";
                dta1.Columns[11].HeaderText = "Trạng thái";
                dta1.Columns[12].HeaderText = "Mã NCC";
                dta1.Columns[13].HeaderText = "Tên NCC";
            }

            int sc = dta1.Rows.Count;
            double soPhieu = (sc > 0 && dta1.Rows[sc - 1].IsNewRow) ? sc - 1 : sc;
            lbl_kq.Text = soPhieu + " Phiếu";
        }

        // Clear form / reset trạng thái
        public void ClearForm()
        {
            txt_mhd.Text = "";
            txt_mathuoc.Text = "";
            txt_solg.Text = "";
            txt_gianhap.Text = "";
            txt_tongtien.Text = "";
            txt_congno.Text = "";
            txt_search.Text = "";
            txt_ngaynhap.Value = DateTime.Now;

            if (txt_tennv != null) txt_tennv.SelectedIndex = -1;
            if (txt_mancc != null) txt_mancc.SelectedIndex = -1;

            btn_them.Enabled = true;
            bnt_sua.Enabled = false;
            btn_xoa.Enabled = false;
        }

        // Load combobox NCC
        private void LoadNCC()
        {
            string load_mancc = "SELECT MaNCC, TenNCC FROM NhaCungCap";
            chuoiketnoi.xulycbx(load_mancc, txt_mancc);
            txt_mancc.DisplayMember = "MaNCC";
            txt_mancc.ValueMember = "MaNCC";
            txt_mancc.SelectedIndex = -1;
        }

        // Load combobox Nhân viên kho
        private void LoadNhanVien()
        {
            string load_nhanvien = @"
                SELECT nk.MaNV, nv.HoTen
                FROM NhanVienKho nk
                INNER JOIN NhanVien nv ON nk.MaNV = nv.MaNV";

            chuoiketnoi.xulycbx(load_nhanvien, txt_tennv);
            txt_tennv.DisplayMember = "HoTen";
            txt_tennv.ValueMember = "MaNV";
            txt_tennv.SelectedIndex = -1;
        }

        // Sinh mã số nguyên ngẫu nhiên 6 chữ số, đảm bảo không trùng trong 1 bảng
        private int GenerateUniqueId(string tableName, string columnName)
        {
            Random rd = new Random();
            int result = 0;

            while (true)
            {
                int candidate = rd.Next(100000, 999999);
                string sql = $"SELECT COUNT(*) FROM {tableName} WHERE {columnName} = {candidate}";
                object obj = chuoiketnoi.ExecuteScalar(sql);
                int cnt = 0;
                if (obj != null && obj != DBNull.Value)
                    int.TryParse(obj.ToString(), out cnt);

                if (cnt == 0)
                {
                    result = candidate;
                    break;
                }
            }

            return result;
        }

        // Cập nhật txt_tongtien = SoLuong * GiaNhap
        private void UpdateThanhTien()
        {
            if (!int.TryParse(txt_solg.Text.Trim(), out int soLuong) || soLuong < 0)
            {
                txt_tongtien.Text = "0";
                key = 0;
                return;
            }

            if (!decimal.TryParse(txt_gianhap.Text.Trim(), out decimal giaNhap) || giaNhap < 0)
            {
                txt_tongtien.Text = "0";
                key = 0;
                return;
            }

            decimal thanhTien = soLuong * giaNhap;
            key = thanhTien;
            txt_tongtien.Text = thanhTien.ToString("F2");
        }

        private void HookEvents()
        {
            txt_solg.TextChanged += (s, e) => UpdateThanhTien();
            txt_gianhap.TextChanged += (s, e) => UpdateThanhTien();
        }

        // ================== EVENT ==================

        private void PhieuNhap_Load(object sender, EventArgs e)
        {
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            string load1 =
                "SELECT MaSP, TenSP, DonGia, SoLuongTon, LoaiSP " +
                "FROM SanPham WHERE TenSP LIKE N'%" + txt_search.Text + "%'";

            chuoiketnoi.timkiem(load1, dta2);
            TenBang();
        }

        // Chọn 1 dòng trong dta2 => đổ mã SP + giá nhập
        private void dta2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dta2.Rows.Count - 1)
            {
                txt_mathuoc.Text = dta2.Rows[e.RowIndex].Cells[0].Value?.ToString();
                txt_gianhap.Text = dta2.Rows[e.RowIndex].Cells[2].Value?.ToString();
            }
        }

        private void dta2_Click(object sender, EventArgs e)
        {
            // nếu bạn gắn event Click thay vì CellClick
            if (dta2.CurrentRow != null && dta2.CurrentRow.Index < dta2.Rows.Count - 1)
            {
                int row = dta2.CurrentRow.Index;
                txt_mathuoc.Text = dta2.Rows[row].Cells[0].Value?.ToString();
                txt_gianhap.Text = dta2.Rows[row].Cells[2].Value?.ToString();
            }
        }

        // Chọn một dòng chi tiết phiếu trong dta1 => đổ lên form
        private void dta1_Click(object sender, EventArgs e)
        {
            if (dta1.CurrentRow == null || dta1.CurrentRow.Index < 0)
                return;

            int row = dta1.CurrentRow.Index;
            if (row >= dta1.Rows.Count - 1) return;

            try
            {
                txt_mhd.Text = dta1.Rows[row].Cells[1].Value?.ToString(); // MaPhieu
                txt_mathuoc.Text = dta1.Rows[row].Cells[2].Value?.ToString(); // MaSP
                txt_solg.Text = dta1.Rows[row].Cells[4].Value?.ToString(); // SoLuong
                txt_gianhap.Text = dta1.Rows[row].Cells[5].Value?.ToString(); // GiaNhap
                txt_tongtien.Text = dta1.Rows[row].Cells[9].Value?.ToString(); // ThanhTien
                txt_congno.Text = dta1.Rows[row].Cells[10].Value?.ToString(); // CongNo

                // Ngày nhập
                if (DateTime.TryParse(dta1.Rows[row].Cells[8].Value?.ToString(), out DateTime ngay))
                    txt_ngaynhap.Value = ngay;
                else
                    txt_ngaynhap.Value = DateTime.Now;

                // MaNV
                if (int.TryParse(dta1.Rows[row].Cells[6].Value?.ToString(), out int maNV))
                    txt_tennv.SelectedValue = maNV;
                else
                    txt_tennv.SelectedIndex = -1;

                // MaNCC
                if (int.TryParse(dta1.Rows[row].Cells[12].Value?.ToString(), out int maNCC))
                    txt_mancc.SelectedValue = maNCC;
                else
                    txt_mancc.SelectedIndex = -1;

                // TrangThai ở cột 11
                string trangThai = dta1.Rows[row].Cells[11].Value?.ToString() ?? "False";

                if (string.Equals(trangThai, "True", StringComparison.OrdinalIgnoreCase))
                {
                    // Đã chốt
                    bnt_sua.Enabled = false;
                    btn_xoa.Enabled = false;
                    btn_them.Enabled = true;
                }
                else
                {
                    // Chưa chốt
                    bnt_sua.Enabled = true;
                    btn_xoa.Enabled = true;
                    btn_them.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu dòng phiếu: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Lấy tồn kho của mã SP hiện tại
        private void txt_mathuoc_TextChanged(object sender, EventArgs e)
        {
            string solg = "SELECT SoLuongTon FROM SanPham WHERE MaSP ='" + txt_mathuoc.Text + "'";
            using (SqlDataReader rd3 = chuoiketnoi.showtext(solg))
            {
                if (rd3 != null && rd3.Read())
                {
                    int.TryParse(rd3[0].ToString(), out a);
                }
            }
        }

        // NÚT THÊM (tạo mới Phiếu nhập & Chi tiết)
        private void btn_them_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra input
            if (string.IsNullOrWhiteSpace(txt_mathuoc.Text))
            {
                MessageBox.Show("Thiếu Mã sản phẩm!", "Lỗi"); txt_mathuoc.Focus(); return;
            }
            if (string.IsNullOrWhiteSpace(txt_solg.Text))
            {
                MessageBox.Show("Thiếu Số lượng!", "Lỗi"); txt_solg.Focus(); return;
            }
            if (string.IsNullOrWhiteSpace(txt_gianhap.Text))
            {
                MessageBox.Show("Thiếu Giá nhập!", "Lỗi"); txt_gianhap.Focus(); return;
            }
            if (string.IsNullOrWhiteSpace(txt_congno.Text))
            {
                MessageBox.Show("Thiếu Công nợ!", "Lỗi"); txt_congno.Focus(); return;
            }
            if (txt_mancc.SelectedValue == null)
            {
                MessageBox.Show("Thiếu Nhà cung cấp!", "Lỗi"); txt_mancc.Focus(); return;
            }
            if (txt_tennv.SelectedValue == null)
            {
                MessageBox.Show("Thiếu Nhân viên!", "Lỗi"); txt_tennv.Focus(); return;
            }

            if (!int.TryParse(txt_solg.Text.Trim(), out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng không hợp lệ!", "Lỗi"); txt_solg.Focus(); return;
            }
            if (!decimal.TryParse(txt_gianhap.Text.Trim(), out decimal giaNhap) || giaNhap <= 0)
            {
                MessageBox.Show("Giá nhập không hợp lệ!", "Lỗi"); txt_gianhap.Focus(); return;
            }
            if (!decimal.TryParse(txt_congno.Text.Trim(), out decimal congNo) || congNo < 0)
            {
                MessageBox.Show("Công nợ không hợp lệ!", "Lỗi"); txt_congno.Focus(); return;
            }

            int maSP;
            if (!int.TryParse(txt_mathuoc.Text.Trim(), out maSP))
            {
                MessageBox.Show("Mã SP phải là số!", "Lỗi"); txt_mathuoc.Focus(); return;
            }

            int maNCC = Convert.ToInt32(txt_mancc.SelectedValue);
            int maNV = Convert.ToInt32(txt_tennv.SelectedValue);

            // 2. Sinh mã phiếu & mã CTPN
            int maPhieu = GenerateUniqueId("PhieuNhapHang", "MaPhieu");
            int maCTPN = GenerateUniqueId("ChiTietPhieuNhap", "MaCTPN");

            txt_mhd.Text = maPhieu.ToString(); // hiển thị lên form

            decimal thanhTien = soLuong * giaNhap;

            using (SqlConnection conn = new SqlConnection(chuoiketnoi.sqlcon))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();

                try
                {
                    // 2.1. Insert PhieuNhapHang
                    string insertPhieu = @"
INSERT INTO PhieuNhapHang
    (MaPhieu, NgayTao, TongSoLuong, TonTien, DonGiaNhap, CapNhatTonKho, MaNCC, MaNV)
VALUES
    (@MaPhieu, @NgayTao, @TongSoLuong, @TonTien, @DonGiaNhap, @CapNhatTonKho, @MaNCC, @MaNV)";

                    using (SqlCommand cmd = new SqlCommand(insertPhieu, conn, tran))
                    {
                        cmd.Parameters.AddWithValue("@MaPhieu", maPhieu);
                        cmd.Parameters.AddWithValue("@NgayTao", txt_ngaynhap.Value.Date);
                        cmd.Parameters.AddWithValue("@TongSoLuong", soLuong);
                        cmd.Parameters.AddWithValue("@TonTien", thanhTien);
                        cmd.Parameters.AddWithValue("@DonGiaNhap", giaNhap);
                        cmd.Parameters.AddWithValue("@CapNhatTonKho", 0); // 0: chưa cập nhật kho
                        cmd.Parameters.AddWithValue("@MaNCC", maNCC);
                        cmd.Parameters.AddWithValue("@MaNV", maNV);
                        cmd.ExecuteNonQuery();
                    }

                    // 2.2. Insert ChiTietPhieuNhap
                    string insertCt = @"
INSERT INTO ChiTietPhieuNhap
    (MaCTPN, MaPhieu, MaSP, SoLuong, GiaNhap, MaNV, NgayTao, ThanhTien, CongNo, TrangThai)
VALUES
    (@MaCTPN, @MaPhieu, @MaSP, @SoLuong, @GiaNhap, @MaNV, @NgayTao, @ThanhTien, @CongNo, @TrangThai)";

                    using (SqlCommand cmd = new SqlCommand(insertCt, conn, tran))
                    {
                        cmd.Parameters.AddWithValue("@MaCTPN", maCTPN);
                        cmd.Parameters.AddWithValue("@MaPhieu", maPhieu);
                        cmd.Parameters.AddWithValue("@MaSP", maSP);
                        cmd.Parameters.AddWithValue("@SoLuong", soLuong);
                        cmd.Parameters.AddWithValue("@GiaNhap", giaNhap);
                        cmd.Parameters.AddWithValue("@MaNV", maNV);
                        cmd.Parameters.AddWithValue("@NgayTao", txt_ngaynhap.Value.Date);
                        cmd.Parameters.AddWithValue("@ThanhTien", thanhTien);
                        cmd.Parameters.AddWithValue("@CongNo", congNo);
                        cmd.Parameters.AddWithValue("@TrangThai", "False");
                        cmd.ExecuteNonQuery();
                    }

                    tran.Commit();

                    chuoiketnoi.Chuoiketnoi(chuoi, dta1);
                    chuoiketnoi.Chuoiketnoi(chuoi1, dta2);
                    TenBang();
                    ClearForm();

                    MessageBox.Show("Thêm phiếu nhập thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    try { tran.Rollback(); } catch { }
                    MessageBox.Show("Lỗi khi thêm phiếu nhập: " + ex.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // NÚT SỬA: update lại CTPN (không đụng tồn kho)
        private void bnt_sua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_mhd.Text))
            {
                MessageBox.Show("Bạn chưa chọn phiếu nhập cần sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txt_mathuoc.Text.Trim(), out int maSP))
            {
                MessageBox.Show("Mã SP không hợp lệ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txt_solg.Text.Trim(), out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng không hợp lệ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txt_gianhap.Text.Trim(), out decimal giaNhap) || giaNhap <= 0)
            {
                MessageBox.Show("Giá nhập không hợp lệ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txt_congno.Text.Trim(), out decimal congNo) || congNo < 0)
            {
                MessageBox.Show("Công nợ không hợp lệ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal thanhTien = soLuong * giaNhap;
            int maPhieu = int.Parse(txt_mhd.Text.Trim());

            string update_ct = @"
UPDATE ChiTietPhieuNhap SET
    SoLuong   = @SL,
    GiaNhap   = @GN,
    NgayTao   = @Ngay,
    ThanhTien = @TT,
    CongNo    = @CN
WHERE MaPhieu = @MaPhieu AND MaSP = @MaSP";

            using (SqlConnection conn = new SqlConnection(chuoiketnoi.sqlcon))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(update_ct, conn))
                {
                    cmd.Parameters.AddWithValue("@SL", soLuong);
                    cmd.Parameters.AddWithValue("@GN", giaNhap);
                    cmd.Parameters.AddWithValue("@Ngay", txt_ngaynhap.Value.Date);
                    cmd.Parameters.AddWithValue("@TT", thanhTien);
                    cmd.Parameters.AddWithValue("@CN", congNo);
                    cmd.Parameters.AddWithValue("@MaPhieu", maPhieu);
                    cmd.Parameters.AddWithValue("@MaSP", maSP);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Cập nhật chi tiết phiếu thành công!", "Thông báo");
            chuoiketnoi.Chuoiketnoi(chuoi, dta1);
            TenBang();
        }

        // NÚT XÓA: xóa CTPN theo MaPhieu + MaSP
        private void btn_xoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_mhd.Text) ||
                string.IsNullOrWhiteSpace(txt_mathuoc.Text))
            {
                MessageBox.Show("Bạn chưa chọn đầy đủ Mã phiếu và Mã SP để xóa!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa chi tiết phiếu này?",
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            string sql = "DELETE FROM ChiTietPhieuNhap WHERE MaPhieu = @MaPhieu AND MaSP = @MaSP";

            using (SqlConnection conn = new SqlConnection(chuoiketnoi.sqlcon))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaPhieu", txt_mhd.Text.Trim());
                    cmd.Parameters.AddWithValue("@MaSP", txt_mathuoc.Text.Trim());
                    cmd.ExecuteNonQuery();
                }
            }

            chuoiketnoi.Chuoiketnoi(chuoi, dta1);
            TenBang();
            ClearForm();
            MessageBox.Show("Xóa chi tiết phiếu thành công!", "Thông báo");
        }

        // NÚT LƯU: nếu bạn chỉ muốn cập nhật lại SoLuong + GiaNhap
        private void btn_luu_Click(object sender, EventArgs e)
        {
            // nếu không dùng nút này thì có thể để trống
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            ClearForm();
            btn_them.Enabled = true;
        }

        private void btn_in_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx|All Files|*.*",
                DefaultExt = "xlsx",
                FileName = "PhieuNhapQL",
                Title = "Chọn nơi lưu và đặt tên file báo cáo Phiếu Nhập"
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fullPath = dlg.FileName;
                    string folder = System.IO.Path.GetDirectoryName(fullPath) + "\\";
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(fullPath);

                    XuatExecl.export_phieu(dta1, folder, fileName, lbl_kq.Text);
                    MessageBox.Show("Xuất file thành công tại:\n" + fullPath,
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất file: " + ex.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btn_Thoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn thoát không?",
                    "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                this.Close();
            }
        }

        private void txt_congno_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsControl(e.KeyChar) && !Char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void txt_solg_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsControl(e.KeyChar) && !Char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        // Các event trống để tránh lỗi designer nếu đã gắn
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void groupBox2_Enter(object sender, EventArgs e) { }
        private void dta1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dta2_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void txt_tongtien_TextChanged(object sender, EventArgs e) { }
        private void txt_mancc_SelectedIndexChanged(object sender, EventArgs e) { }
        private void textBox1_Leave(object sender, EventArgs e) { }
        private void button1_Click(object sender, EventArgs e)
        {
            // mở form sản phẩm nếu bạn cần
            quanly.sanpham f = new quanly.sanpham();
            f.Show();
        }
    }
}
