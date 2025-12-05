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
    public partial class btnCheck : Form
    {
        public btnCheck()
        {
            InitializeComponent();
        }

        private void TinhTongChenhLech()
        {
            decimal tong = 0;

            foreach (DataGridViewRow row in dgvSanPhamChonDoi.Rows)
            {
                if (row.IsNewRow) continue;

                decimal chenh = 0;
                decimal.TryParse(row.Cells[9].Value?.ToString(), out chenh);

                tong += chenh;
            }

            lblTongChenhLech.Text = tong.ToString("N0") + " VNĐ";
        }
      


        private void FormDoiHang_Load(object sender, EventArgs e)
        {
            dgvSanPhamDoi.AutoGenerateColumns = false;
            dgvSanPhamDoi.Columns.Clear();

            // 0
            dgvSanPhamDoi.Columns.Add("MaSP", "Mã sản phẩm");
            // 1
            dgvSanPhamDoi.Columns.Add("TenSP", "Tên sản phẩm");
            // 2
            dgvSanPhamDoi.Columns.Add("LoaiSP", "Loại sản phẩm");
            // 3
            dgvSanPhamDoi.Columns.Add("SoLuongMua", "Số lượng mua");
            // 4
            dgvSanPhamDoi.Columns.Add("SoLuongDoi", "Số lượng đổi");
            // 5
            dgvSanPhamDoi.Columns.Add("DonGia", "Đơn giá");
            // 6
            dgvSanPhamDoi.Columns.Add("ThanhTienDoi", "Thành tiền đổi");
            cbbHinhThucDoi.Items.Add("Đổi sang sản phẩm 1 - 1");
            cbbHinhThucDoi.Items.Add("Đổi sang sản phẩm khác loại nhưng giá trị tương đương");
            cbbHinhThucDoi.Items.Add("Đổi sang sản phẩm khác chênh lệch giá trị");

            // Không chọn mặc định
            cbbHinhThucDoi.SelectedIndex = -1;
            dgvSanPhamChonDoi.AutoGenerateColumns = false;
            dgvSanPhamChonDoi.Columns.Clear();

            // 0: Mã SP cũ
            dgvSanPhamChonDoi.Columns.Add("MaSPCu", "Mã SP (cũ)");
            // 1: Tên SP cũ
            dgvSanPhamChonDoi.Columns.Add("TenSPCu", "Tên SP (cũ)");
            // 2: Mã SP mới
            dgvSanPhamChonDoi.Columns.Add("MaSPMoi", "Mã SP (mới)");
            // 3: Tên SP mới
            dgvSanPhamChonDoi.Columns.Add("TenSPMoi", "Tên SP (mới)");
            // 4: Số lượng đổi
            dgvSanPhamChonDoi.Columns.Add("SoLuongDoi", "Số lượng đổi");
            // 5: Đơn giá cũ
            dgvSanPhamChonDoi.Columns.Add("DonGiaCu", "Đơn giá cũ");
            // 6: Đơn giá mới
            dgvSanPhamChonDoi.Columns.Add("DonGiaMoi", "Đơn giá mới");
            // 7: Thành tiền cũ
            dgvSanPhamChonDoi.Columns.Add("ThanhTienCu", "Thành tiền (cũ)");
            // 8: Thành tiền mới
            dgvSanPhamChonDoi.Columns.Add("ThanhTienMoi", "Thành tiền (mới)");
            // 9: Số tiền chênh lệch
            dgvSanPhamChonDoi.Columns.Add("SoTienChenhLech", "Chênh lệch");
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // 0. Kiểm tra đã nhập số lượng đổi chưa
                if (string.IsNullOrWhiteSpace(txtSoLuong.Text))
                {
                    MessageBox.Show("Bạn chưa nhập số lượng đổi!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoLuong.Focus();
                    return;
                }

                int soLuongDoi;
                if (!int.TryParse(txtSoLuong.Text, out soLuongDoi) || soLuongDoi <= 0)
                {
                    MessageBox.Show("Số lượng đổi phải là số nguyên dương!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoLuong.Focus();
                    return;
                }

                // 1. Kiểm tra đã chọn dòng nào bên dgvCTHD chưa
                if (dgvCTHD.CurrentRow == null || dgvCTHD.CurrentRow.Index < 0)
                {
                    MessageBox.Show("Bạn chưa chọn sản phẩm cần đổi trong hóa đơn!", "Thông báo",
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

                // 2. Kiểm tra số lượng đổi không vượt quá số lượng mua
                if (soLuongDoi > soLuongMua)
                {
                    MessageBox.Show("Số lượng đổi không được lớn hơn số lượng đã mua (" + soLuongMua + ")!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 3. (Tuỳ chọn) Không cho thêm trùng sản phẩm
                foreach (DataGridViewRow r in dgvSanPhamDoi.Rows)
                {
                    if (r.IsNewRow) continue;
                    if (r.Cells[0].Value != null && r.Cells[0].Value.ToString() == maSP)
                    {
                        MessageBox.Show("Sản phẩm này đã có trong danh sách đổi!\n" +
                                        "Bạn có thể sửa số lượng đổi trực tiếp trên bảng.",
                                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                decimal donGia = 0;
                decimal.TryParse(donGiaStr, out donGia);

                // Thành tiền đổi = đơn giá * số lượng đổi
                decimal thanhTienDoi = donGia * soLuongDoi;

                // 4. Thêm 1 dòng mới vào dgvSanPhamDoi
                int n = dgvSanPhamDoi.Rows.Add();
                dgvSanPhamDoi.Rows[n].Cells[0].Value = maSP;                       // Mã sản phẩm
                dgvSanPhamDoi.Rows[n].Cells[1].Value = tenSP;                      // Tên
                dgvSanPhamDoi.Rows[n].Cells[2].Value = loaiSP;                     // Loại
                dgvSanPhamDoi.Rows[n].Cells[3].Value = soLuongMua.ToString();      // Số lượng mua
                dgvSanPhamDoi.Rows[n].Cells[4].Value = soLuongDoi.ToString();      // Số lượng đổi (nhập từ txtSoLuong)
                dgvSanPhamDoi.Rows[n].Cells[5].Value = donGia.ToString();          // Đơn giá
                dgvSanPhamDoi.Rows[n].Cells[6].Value = thanhTienDoi.ToString();    // Thành tiền đổi

                MessageBox.Show("Đã thêm sản phẩm vào danh sách đổi!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Xóa textbox số lượng để lần sau nhập lại
                txtSoLuong.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm sản phẩm đổi: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void dgvSanPhamChonDoi_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4) // Cột số lượng đổi
            {
                // Tính lại thành tiền mới + chênh lệch cho dòng này
                DataGridViewRow row = dgvSanPhamChonDoi.Rows[e.RowIndex];

                int soLuongDoi = int.Parse(row.Cells[4].Value.ToString());
                decimal donGiaMoi = decimal.Parse(row.Cells[6].Value.ToString());
                decimal donGiaCu = decimal.Parse(row.Cells[5].Value.ToString());

                decimal thanhTienMoi = soLuongDoi * donGiaMoi;
                decimal thanhTienCu = soLuongDoi * donGiaCu;

                decimal chenh = thanhTienMoi - thanhTienCu;

                row.Cells[7].Value = thanhTienCu.ToString();
                row.Cells[8].Value = thanhTienMoi.ToString();
                row.Cells[9].Value = chenh.ToString();

                // Tính tổng lại
                TinhTongChenhLech();
            }
        }

        private void cbbHinhThucDoi_SelectedIndexChanged(object sender, EventArgs e)
        {
         
            if (cbbHinhThucDoi.SelectedItem == null)
                return;

            string hinhThuc = cbbHinhThucDoi.SelectedItem.ToString();

            // Nếu chưa chọn dòng nào trên hóa đơn thì không xử lý 2 trường hợp sau
            // (trường hợp 1-1 thì không cần sản phẩm mới)
            if (hinhThuc != "Đổi sang sản phẩm 1 - 1")
            {
                if (dgvCTHD.CurrentRow == null || dgvCTHD.CurrentRow.Index < 0)
                {
                    MessageBox.Show("Bạn chưa chọn sản phẩm cần đổi trong hóa đơn!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Xóa dữ liệu cũ của dgvSanPham
            dgvSanPham.DataSource = null;
            dgvSanPham.Rows.Clear();

            // ==========================
            // 1. ĐỔI SANG SẢN PHẨM 1 - 1
            // ==========================
            if (hinhThuc == "Đổi sang sản phẩm 1 - 1")
            {
                // Hình thức này không cần chọn sản phẩm khác,
                // chỉ cần chọn lý do đổi rồi lập phiếu
                // -> nên mình chỉ clear grid sản phẩm, không load gì thêm
                // Có thể ẩn dgvSanPham nếu bạn muốn, hoặc để trống:
                // dgvSanPham.Visible = false;

                MessageBox.Show("Hình thức đổi 1 - 1. Bạn chỉ cần chọn lý do đổi và lập phiếu.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Lấy thông tin sản phẩm gốc trên hóa đơn
            DataGridViewRow rowOld = dgvCTHD.CurrentRow;
            string maSPOld = rowOld.Cells[0].Value?.ToString();
            string donGiaOldStr = rowOld.Cells[4].Value?.ToString();    // Đơn giá cột 4

            decimal donGiaOld = 0;
            decimal.TryParse(donGiaOldStr, out donGiaOld);

            // ================================================
            // 2. ĐỔI SANG SP KHÁC LOẠI NHƯNG GIÁ TRỊ TƯƠNG ĐƯƠNG
            // ================================================
            if (hinhThuc == "Đổi sang sản phẩm khác loại nhưng giá trị tương đương")
            {
                // Ví dụ: chọn các SP có giá từ 90% đến 110% giá cũ
                decimal minGia = donGiaOld * 0.9m;
                decimal maxGia = donGiaOld * 1.1m;

                string sql =
                    "SELECT MaSP, TenSP, LoaiSP, SoLuongTon, DonGia " +
                    "FROM SanPham " +
                    "WHERE MaSP <> " + maSPOld + " " +
                    "AND SoLuongTon > 0 " +
                    "AND DonGia BETWEEN " +
                    minGia.ToString().Replace(',', '.') + " AND " +
                    maxGia.ToString().Replace(',', '.');

                chuoiketnoi.Chuoiketnoi(sql, dgvSanPham);

                if (dgvSanPham.Columns.Count >= 5)
                {
                    dgvSanPham.Columns[0].HeaderText = "Mã sản phẩm";
                    dgvSanPham.Columns[1].HeaderText = "Tên sản phẩm";
                    dgvSanPham.Columns[2].HeaderText = "Loại sản phẩm";
                    dgvSanPham.Columns[3].HeaderText = "Số lượng tồn";
                    dgvSanPham.Columns[4].HeaderText = "Đơn giá";
                }

                if (dgvSanPham.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy sản phẩm nào có giá trị tương đương.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Đã tải danh sách sản phẩm giá trị tương đương.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                return;
            }

            // ===================================
            // 3. ĐỔI SANG SẢN PHẨM KHÁC CHÊNH LỆCH GIÁ TRỊ
            // ===================================
            if (hinhThuc == "Đổi sang sản phẩm khác chênh lệch giá trị")
            {
                // Cho phép khách chọn bất kỳ sản phẩm nào còn tồn kho
                string sql =
                    "SELECT MaSP, TenSP, LoaiSP, SoLuongTon, DonGia " +
                    "FROM SanPham " +
                    "WHERE SoLuongTon > 0";

                chuoiketnoi.Chuoiketnoi(sql, dgvSanPham);

                if (dgvSanPham.Columns.Count >= 5)
                {
                    dgvSanPham.Columns[0].HeaderText = "Mã sản phẩm";
                    dgvSanPham.Columns[1].HeaderText = "Tên sản phẩm";
                    dgvSanPham.Columns[2].HeaderText = "Loại sản phẩm";
                    dgvSanPham.Columns[3].HeaderText = "Số lượng tồn";
                    dgvSanPham.Columns[4].HeaderText = "Đơn giá";
                }

                MessageBox.Show("Đã tải danh sách sản phẩm để đổi (có thể chênh lệch giá).",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void dgvSanPham_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                if (cbbHinhThucDoi.SelectedItem == null)
                {
                    MessageBox.Show("Bạn chưa chọn hình thức đổi!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string hinhThuc = cbbHinhThucDoi.SelectedItem.ToString();

                // Phải có sản phẩm gốc đang chọn trong hóa đơn
                if (dgvCTHD.CurrentRow == null || dgvCTHD.CurrentRow.Index < 0)
                {
                    MessageBox.Show("Bạn chưa chọn sản phẩm cần đổi trong hóa đơn!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ================== LẤY SẢN PHẨM CŨ TRÊN HÓA ĐƠN ==================
                DataGridViewRow rowOld = dgvCTHD.CurrentRow;
                string maSPCu = rowOld.Cells[0].Value?.ToString();
                string tenSPCu = rowOld.Cells[1].Value?.ToString();
                string soLuongMuaS = rowOld.Cells[3].Value?.ToString(); // số lượng mua trên hóa đơn
                string donGiaCuS = rowOld.Cells[4].Value?.ToString();
                string thanhTienCuS = rowOld.Cells[5].Value?.ToString();

                int soLuongMua = 0;
                int.TryParse(soLuongMuaS, out soLuongMua);

                decimal donGiaCu = 0;
                decimal.TryParse(donGiaCuS, out donGiaCu);

                decimal thanhTienCu = 0;
                decimal.TryParse(thanhTienCuS, out thanhTienCu);

                // ================== LẤY SẢN PHẨM MỚI TRONG DGV SANPHAM ==================
                DataGridViewRow rowNew = dgvSanPham.Rows[e.RowIndex];
                string maSPMoi = rowNew.Cells[0].Value?.ToString();
                string tenSPMoi = rowNew.Cells[1].Value?.ToString();
                string loaiSPMoi = rowNew.Cells[2].Value?.ToString();
                string soLuongTonS = rowNew.Cells[3].Value?.ToString(); // nếu bạn để cột 3 là tồn
                string donGiaMoiS = rowNew.Cells[4].Value?.ToString(); // cột 4 là đơn giá

                int soLuongTon = 0;
                int.TryParse(soLuongTonS, out soLuongTon);

                decimal donGiaMoi = 0;
                decimal.TryParse(donGiaMoiS, out donGiaMoi);

                // ================== NHỚ SỐ LƯỢNG Ở TRÊN ==================
                // Số lượng đổi mặc định = số lượng đã mua
                int soLuongDoi = soLuongMua;

                // Nếu tồn không đủ thì lấy tối đa = tồn
                if (soLuongTon < soLuongDoi)
                {
                    soLuongDoi = soLuongTon;
                    MessageBox.Show("Số lượng tồn sản phẩm mới không đủ, hệ thống tự động đổi theo số lượng tồn.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                decimal thanhTienMoi = donGiaMoi * soLuongDoi;
                decimal soTienChenhLech = thanhTienMoi - thanhTienCu;

                // ================== ADD VÀO DGV SANPHAMCHONDOI ==================
                int n = dgvSanPhamChonDoi.Rows.Add();
                dgvSanPhamChonDoi.Rows[n].Cells[0].Value = maSPCu;               // Mã SP cũ
                dgvSanPhamChonDoi.Rows[n].Cells[1].Value = tenSPCu;              // Tên SP cũ
                dgvSanPhamChonDoi.Rows[n].Cells[2].Value = maSPMoi;              // Mã SP mới
                dgvSanPhamChonDoi.Rows[n].Cells[3].Value = tenSPMoi;             // Tên SP mới
                dgvSanPhamChonDoi.Rows[n].Cells[4].Value = soLuongDoi.ToString();// Số lượng đổi
                dgvSanPhamChonDoi.Rows[n].Cells[5].Value = donGiaCu.ToString();  // Đơn giá cũ
                dgvSanPhamChonDoi.Rows[n].Cells[6].Value = donGiaMoi.ToString(); // Đơn giá mới
                dgvSanPhamChonDoi.Rows[n].Cells[7].Value = thanhTienCu.ToString(); // Thành tiền cũ
                dgvSanPhamChonDoi.Rows[n].Cells[8].Value = thanhTienMoi.ToString(); // Thành tiền mới
                dgvSanPhamChonDoi.Rows[n].Cells[9].Value = soTienChenhLech.ToString(); // Chênh lệch

                MessageBox.Show("Đã chọn sản phẩm mới để đổi.\nSố lượng đổi dùng theo số lượng trên hóa đơn.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TinhTongChenhLech();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn sản phẩm đổi: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {

        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSanPhamChonDoi.CurrentRow == null || dgvSanPhamChonDoi.CurrentRow.Index < 0)
                {
                    MessageBox.Show("Bạn chưa chọn sản phẩm cần xóa khỏi danh sách đổi!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int rowIndex = dgvSanPhamChonDoi.CurrentRow.Index;

                if (MessageBox.Show("Bạn có chắc muốn xóa sản phẩm này khỏi danh sách đổi không?",
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    dgvSanPhamChonDoi.Rows.RemoveAt(rowIndex);
                    MessageBox.Show("Đã xóa sản phẩm khỏi danh sách đổi.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa sản phẩm đổi: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_xoa_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (dgvSanPhamChonDoi.CurrentRow == null || dgvSanPhamChonDoi.CurrentRow.Index < 0)
                {
                    MessageBox.Show("Bạn chưa chọn sản phẩm cần xóa khỏi danh sách đổi!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int rowIndex = dgvSanPhamChonDoi.CurrentRow.Index;

                if (MessageBox.Show("Bạn có chắc muốn xóa sản phẩm này khỏi danh sách đổi không?",
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    dgvSanPhamChonDoi.Rows.RemoveAt(rowIndex);
                    MessageBox.Show("Đã xóa sản phẩm khỏi danh sách đổi.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa sản phẩm đổi: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXacNhanThanhToan_Click(object sender, EventArgs e)
        {
            try
            {
                int maHD;
                if (!int.TryParse(txtMaHD.Text, out maHD))
                {
                    MessageBox.Show("Không đọc được mã hóa đơn để cập nhật lại!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Chỉ cần cập nhật lại hóa đơn cho các trường hợp ĐỔI SANG SẢN PHẨM KHÁC...
                // (Đổi 1-1 cùng mã sản phẩm thì về bản chất hóa đơn không thay đổi)
                if (cbbHinhThucDoi.SelectedItem.ToString() != "Đổi sang sản phẩm 1 - 1")
                {
                    foreach (DataGridViewRow row in dgvSanPhamChonDoi.Rows)
                    {
                        if (row.IsNewRow) continue;

                        int maSPCu, maSPMoi, soLuongDoi;
                        decimal thanhTienCuDoi, thanhTienMoiDoi, donGiaMoi;

                        if (!int.TryParse(row.Cells[0].Value?.ToString(), out maSPCu)) continue;  // MaSPCu
                        if (!int.TryParse(row.Cells[2].Value?.ToString(), out maSPMoi)) continue; // MaSPMoi
                        if (!int.TryParse(row.Cells[4].Value?.ToString(), out soLuongDoi)) soLuongDoi = 1;

                        decimal.TryParse(row.Cells[7].Value?.ToString(), out thanhTienCuDoi);  // phần tiền cũ bị đổi
                        decimal.TryParse(row.Cells[8].Value?.ToString(), out thanhTienMoiDoi); // phần tiền mới
                        decimal.TryParse(row.Cells[6].Value?.ToString(), out donGiaMoi);        // đơn giá mới

                        // 6.1. XỬ LÝ SẢN PHẨM CŨ TRONG CHITIETHOADON
                        // Lấy SoLuong hiện tại
                        int soLuongCuTrongHD = 0;
                        decimal thanhTienCuTrongHD = 0;

                        string sqlGetOld =
                            "SELECT SoLuong, ThanhTien FROM ChiTietHoaDon " +
                            "WHERE MaHD = " + maHD + " AND MaSP = " + maSPCu;

                        using (SqlDataReader rdOld = chuoiketnoi.showtext(sqlGetOld))
                        {
                            if (rdOld.Read())
                            {
                                soLuongCuTrongHD = int.Parse(rdOld["SoLuong"].ToString());
                                thanhTienCuTrongHD = decimal.Parse(rdOld["ThanhTien"].ToString());
                            }
                        }

                        // Nếu không tìm thấy dòng cũ thì bỏ qua
                        if (soLuongCuTrongHD > 0)
                        {
                            if (soLuongCuTrongHD <= soLuongDoi)
                            {
                                // Xóa luôn dòng cũ
                                string sqlDel =
                                    "DELETE FROM ChiTietHoaDon WHERE MaHD = " + maHD +
                                    " AND MaSP = " + maSPCu;
                                chuoiketnoi.luu(sqlDel);
                            }
                            else
                            {
                                // Giảm số lượng, giảm thành tiền
                                // ThanhTien - phần tiền của số lượng bị đổi
                                string sqlUpdOld =
                                    "UPDATE ChiTietHoaDon SET " +
                                    "SoLuong = SoLuong - " + soLuongDoi + ", " +
                                    "ThanhTien = ThanhTien - " + thanhTienCuDoi.ToString().Replace(',', '.') +
                                    " WHERE MaHD = " + maHD + " AND MaSP = " + maSPCu;
                                chuoiketnoi.luu(sqlUpdOld);
                            }
                        }

                        // 6.2. THÊM / CỘNG DỒN SẢN PHẨM MỚI VÀO CHITIETHOADON
                        int countNew = 0;
                        string sqlCheckNew =
                            "SELECT COUNT(*) FROM ChiTietHoaDon " +
                            "WHERE MaHD = " + maHD + " AND MaSP = " + maSPMoi;

                        using (SqlDataReader rdCheckNew = chuoiketnoi.showtext(sqlCheckNew))
                        {
                            if (rdCheckNew.Read())
                                countNew = int.Parse(rdCheckNew[0].ToString());
                        }

                        if (countNew > 0)
                        {
                            // Đã có sẵn mã SP mới trong hóa đơn -> cộng dồn
                            string sqlUpdNew =
                                "UPDATE ChiTietHoaDon SET " +
                                "SoLuong = SoLuong + " + soLuongDoi + ", " +
                                "ThanhTien = ThanhTien + " + thanhTienMoiDoi.ToString().Replace(',', '.') +
                                " WHERE MaHD = " + maHD + " AND MaSP = " + maSPMoi;
                            chuoiketnoi.luu(sqlUpdNew);
                        }
                        else
                        {
                            // Chưa có -> thêm mới
                            string sqlInsNew =
                                "INSERT INTO ChiTietHoaDon(MaHD, MaSP, SoLuong, DonGia, ThanhTien, TinhThanhTien) VALUES(" +
                                maHD + ", " +
                                maSPMoi + ", " +
                                soLuongDoi + ", " +
                                donGiaMoi.ToString().Replace(',', '.') + ", " +
                                thanhTienMoiDoi.ToString().Replace(',', '.') + ", 1)";
                            chuoiketnoi.them_dl1(sqlInsNew);
                        }
                    }

                    // 6.3. CẬP NHẬT LẠI TỔNG TIỀN HÓA ĐƠN
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
                }

                MessageBox.Show("Lập phiếu đổi hàng và cập nhật lại hóa đơn thành công!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex2)
            {
                MessageBox.Show("Lỗi khi cập nhật lại hóa đơn sau đổi hàng: " + ex2.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSanPhamChonDoi.CurrentRow == null || dgvSanPhamChonDoi.CurrentRow.Index < 0)
                {
                    MessageBox.Show("Bạn chưa chọn sản phẩm cần xóa khỏi danh sách đổi!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                int rowIndex = dgvSanPhamChonDoi.CurrentRow.Index;

                if (MessageBox.Show("Bạn có chắc muốn xóa sản phẩm này khỏi danh sách đổi không?",
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    dgvSanPhamChonDoi.Rows.RemoveAt(rowIndex);
                    MessageBox.Show("Đã xóa sản phẩm khỏi danh sách đổi.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                 

                    TinhTongChenhLech();
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa sản phẩm đổi: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblTongChenhLech_Click(object sender, EventArgs e)
        {

        }

        private void txtSoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

