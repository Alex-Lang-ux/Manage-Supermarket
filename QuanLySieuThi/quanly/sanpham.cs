using System;
using System.Data;
using System.Windows.Forms;
using System.Globalization;

namespace QuanLySieuThi.quanly
{
    public partial class sanpham : Form
    {
        // Lấy đúng các cột trong bảng SanPham
        public string chuoi = @"
            SELECT 
                MaSP,
                TenSP,
                DonGia,
                SoLuongTon,
                SoLuongToiThieu,
                LoaiSP,
                CapNhatSoLuong,
                KiemTraTonKho
            FROM SanPham";

        public sanpham()
        {
            InitializeComponent();
            LoadDataAndInitialize();
        }

        private void LoadDataAndInitialize()
        {
            chuoiketnoi.Chuoiketnoi(chuoi, dta1);
            clear();
        }

        // Tạo mã sản phẩm mới = MAX(MaSP) + 1
        private int TaoMaSPMoi()
        {
            int nextId = 1;
            try
            {
                string sql_max = "SELECT ISNULL(MAX(MaSP), 0) + 1 FROM SanPham";
                using (var rd = chuoiketnoi.showtext(sql_max))
                {
                    if (rd != null && rd.Read())
                    {
                        int.TryParse(rd[0].ToString(), out nextId);
                    }
                }
            }
            catch
            {
                // nếu lỗi thì để 1
            }

            if (nextId <= 0) nextId = 1;
            return nextId;
        }

        // ====== CLEAR / RESET FORM ======
        public void clear()
        {
            // Set header cho DataGridView nếu đã có cột
            if (dta1.Columns.Count >= 8)
            {
                dta1.Columns[0].HeaderText = "Mã SP"; dta1.Columns[0].Width = 60;
                dta1.Columns[1].HeaderText = "Tên sản phẩm"; dta1.Columns[1].Width = 150;
                dta1.Columns[2].HeaderText = "Đơn giá"; dta1.Columns[2].Width = 100;
                dta1.Columns[3].HeaderText = "Tồn kho"; dta1.Columns[3].Width = 80;
                dta1.Columns[4].HeaderText = "Tồn tối thiểu"; dta1.Columns[4].Width = 90;
                dta1.Columns[5].HeaderText = "Loại SP"; dta1.Columns[5].Width = 120;
                dta1.Columns[6].HeaderText = "Cập nhật SL"; dta1.Columns[6].Width = 90;
                dta1.Columns[7].HeaderText = "Kiểm tra tồn"; dta1.Columns[7].Width = 90;
            }

            // Gán mã SP mới
            txt_masp.Text = TaoMaSPMoi().ToString();

            // Xóa control
            txt_tensp.Text = "";
            txt_dongia.Text = "";
            txt_solg.Text = "";
            txt_soluongtoithieu.Text = "";
            txt_loaisp.Text = "";
            txt_search.Text = "";

            // Trạng thái nút
            txt_masp.Enabled = false;
            btn_them.Enabled = true;
            bnt_sua.Enabled = false;
            btn_xoa.Enabled = false;

            // Đếm số dòng
            int rowCount = dta1.Rows.Count;
            if (rowCount > 0 && dta1.Rows[rowCount - 1].IsNewRow)
                rowCount--;

            lbl_kq.Text = rowCount + " loại";

            txt_tensp.Focus();
        }

        private void sanpham_Load(object sender, EventArgs e)
        {
        }

        // ====== SEARCH ======
        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            string search_query = $@"
                SELECT 
                    MaSP,
                    TenSP,
                    DonGia,
                    SoLuongTon,
                    SoLuongToiThieu,
                    LoaiSP,
                    CapNhatSoLuong,
                    KiemTraTonKho
                FROM SanPham
                WHERE TenSP LIKE N'%{txt_search.Text}%'";

            chuoiketnoi.timkiem(search_query, dta1);

            int rowCount = dta1.Rows.Count;
            if (rowCount > 0 && dta1.Rows[rowCount - 1].IsNewRow)
                rowCount--;

            lbl_kq.Text = rowCount + " loại";
        }

        // ====== CLICK LÊN DGV ======
        private void dta1_Click(object sender, EventArgs e)
        {
            if (dta1.CurrentRow == null || dta1.CurrentRow.Index == dta1.Rows.Count - 1)
                return;

            try
            {
                int row = dta1.CurrentRow.Index;

                txt_masp.Text = dta1.Rows[row].Cells[0].Value.ToString(); // MaSP
                txt_tensp.Text = dta1.Rows[row].Cells[1].Value.ToString(); // TenSP
                txt_dongia.Text = dta1.Rows[row].Cells[2].Value.ToString(); // DonGia
                txt_solg.Text = dta1.Rows[row].Cells[3].Value.ToString(); // SoLuongTon
                txt_soluongtoithieu.Text = dta1.Rows[row].Cells[4].Value.ToString(); // SoLuongToiThieu
                txt_loaisp.Text = dta1.Rows[row].Cells[5].Value.ToString(); // LoaiSP

                btn_them.Enabled = false;
                bnt_sua.Enabled = true;
                btn_xoa.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu sản phẩm: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ====== NÚT THÊM ======
        private void btn_them_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_tensp.Text) ||
                string.IsNullOrWhiteSpace(txt_dongia.Text) ||
                string.IsNullOrWhiteSpace(txt_solg.Text) ||
                string.IsNullOrWhiteSpace(txt_soluongtoithieu.Text))
            {
                MessageBox.Show("Bạn chưa nhập đầy đủ Tên SP, Đơn giá, Số lượng, Số lượng tối thiểu!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int maSP;
                if (!int.TryParse(txt_masp.Text, out maSP))
                {
                    MessageBox.Show("Mã sản phẩm không hợp lệ!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal donGia;
                int soLuongTon;
                int soLuongToiThieu;

                if (!decimal.TryParse(txt_dongia.Text, out donGia))
                {
                    MessageBox.Show("Đơn giá không hợp lệ!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txt_solg.Text, out soLuongTon) || soLuongTon < 0)
                {
                    MessageBox.Show("Số lượng tồn không hợp lệ!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txt_soluongtoithieu.Text, out soLuongToiThieu) || soLuongToiThieu < 0)
                {
                    MessageBox.Show("Số lượng tối thiểu không hợp lệ!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string tenSpSql = txt_tensp.Text.Replace("'", "''");
                string loaiSpSql = txt_loaisp.Text.Replace("'", "''");
                string donGiaSql = donGia.ToString(CultureInfo.InvariantCulture);

                // Thêm mới: CapNhatSoLuong = 1, KiemTraTonKho = 1
                string sql = $@"
INSERT INTO SanPham
    (MaSP, TenSP, DonGia, SoLuongTon, SoLuongToiThieu, LoaiSP, CapNhatSoLuong, KiemTraTonKho)
VALUES
    ({maSP}, N'{tenSpSql}', {donGiaSql}, {soLuongTon}, {soLuongToiThieu},
     N'{loaiSpSql}', 1, 1)";

                chuoiketnoi.luu(sql);
                chuoiketnoi.Chuoiketnoi(chuoi, dta1);
                clear();

                MessageBox.Show("Thêm sản phẩm thành công!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm sản phẩm: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ====== NÚT SỬA ======
        private void bnt_sua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_masp.Text))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int maSP;
                if (!int.TryParse(txt_masp.Text, out maSP))
                {
                    MessageBox.Show("Mã sản phẩm không hợp lệ!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal donGia;
                int soLuongTon;
                int soLuongToiThieu;

                if (!decimal.TryParse(txt_dongia.Text, out donGia))
                {
                    MessageBox.Show("Đơn giá không hợp lệ!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txt_solg.Text, out soLuongTon) || soLuongTon < 0)
                {
                    MessageBox.Show("Số lượng tồn không hợp lệ!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txt_soluongtoithieu.Text, out soLuongToiThieu) || soLuongToiThieu < 0)
                {
                    MessageBox.Show("Số lượng tối thiểu không hợp lệ!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string tenSpSql = txt_tensp.Text.Replace("'", "''");
                string loaiSpSql = txt_loaisp.Text.Replace("'", "''");
                string donGiaSql = donGia.ToString(CultureInfo.InvariantCulture);

                string sql = $@"
UPDATE SanPham SET
    TenSP            = N'{tenSpSql}',
    DonGia           = {donGiaSql},
    SoLuongTon       = {soLuongTon},
    SoLuongToiThieu  = {soLuongToiThieu},
    LoaiSP           = N'{loaiSpSql}'
WHERE MaSP = {maSP}";

                chuoiketnoi.Execute1(sql);   // có sẵn thông báo trong hàm này
                chuoiketnoi.Chuoiketnoi(chuoi, dta1);
                clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa sản phẩm: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ====== NÚT XÓA ======
        private void btn_xoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_masp.Text))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maSP;
            if (!int.TryParse(txt_masp.Text, out maSP))
            {
                MessageBox.Show("Mã sản phẩm không hợp lệ!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa sản phẩm Mã SP: {maSP}?",
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    string sql = $"DELETE FROM SanPham WHERE MaSP = {maSP}";
                    chuoiketnoi.luu(sql);
                    chuoiketnoi.Chuoiketnoi(chuoi, dta1);
                    clear();

                    MessageBox.Show("Xóa sản phẩm thành công!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa sản phẩm: " + ex.Message +
                        "\nCó thể sản phẩm đang được dùng trong hóa đơn / phiếu khác.",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ====== NÚT IN (EXPORT EXCEL) ======
        private void btn_in_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Excel Files|*.xlsx|All Files|*.*";
            dlg.DefaultExt = "xlsx";
            dlg.FileName = "ThongTinSanPham";
            dlg.Title = "Chọn nơi lưu và đặt tên file báo cáo sản phẩm";

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

        private void btn_reset_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void btn_Thoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?",
                    "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                this.Close();
            }
        }

        // Chỉ cho nhập số
        private void txt_dongian_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void txt_solg_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void txt_soluongtoithieu_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void dta1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void label11_Click(object sender, EventArgs e) { }
        private void groupBox1_Enter(object sender, EventArgs e) { }
    }
}
