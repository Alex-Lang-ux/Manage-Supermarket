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
    public partial class banhang : Form
    {
        public int a = 0; // số lượng tồn hiện tại (trong db)
        public int y = 0; // số lượng bán (trong grid dta2)

        public banhang()
        {
            InitializeComponent();
        }

        private void banhang_Load(object sender, EventArgs e)
        {

        }

        private void txt_timkiem_TextChanged(object sender, EventArgs e)
        {
            // Dùng bảng SanPham mới: MaSP, TenSP, LoaiSP, SoLuongTon, DonGia
            string load1 = "Select MaSP,TenSP,LoaiSP,SoLuongTon,DonGia from SanPham where TenSP like N'%" + txt_timkiem.Text + "%' ";
            chuoiketnoi.timkiem(load1, db1);

            db1.Columns[0].HeaderText = "Mã sản phẩm"; db1.Columns[0].Width = 120;
            db1.Columns[1].HeaderText = "Tên sản phẩm"; db1.Columns[1].Width = 150;
            db1.Columns[2].HeaderText = "Loại sản phẩm"; db1.Columns[2].Width = 120;
            db1.Columns[3].HeaderText = "Số lượng tồn"; db1.Columns[3].Width = 100;
            db1.Columns[4].HeaderText = "Đơn giá"; db1.Columns[4].Width = 100;
        }

        private void db1_Click(object sender, EventArgs e)
        {
            int curow = db1.CurrentRow.Index;

            // MaSP, TenSP, LoaiSP, SoLuongTon, DonGia
            txt_mathuoc.Text = db1.Rows[curow].Cells[0].Value.ToString(); // MaSP
            txt_tenthuoc.Text = db1.Rows[curow].Cells[1].Value.ToString(); // TenSP
            txt_dangthuoc.Text = db1.Rows[curow].Cells[2].Value.ToString(); // LoaiSP
            txt_thuoctrongkho.Text = db1.Rows[curow].Cells[3].Value.ToString(); // SoLuongTon
            txt_gia.Text = db1.Rows[curow].Cells[4].Value.ToString(); // DonGia
        }

        private void dta2_Click(object sender, EventArgs e)
        {
            try
            {
                btn_Add.Enabled = false;
                txt_slgban.Enabled = false;
                btn_Xoa.Enabled = true;
                int curow = dta2.CurrentRow.Index;

                txt_mathuoc.Text = dta2.Rows[curow].Cells[0].Value.ToString();
                txt_tenthuoc.Text = dta2.Rows[curow].Cells[1].Value.ToString();
                txt_dangthuoc.Text = dta2.Rows[curow].Cells[2].Value.ToString();
                txt_gia.Text = dta2.Rows[curow].Cells[3].Value.ToString();
                txt_slgban.Text = dta2.Rows[curow].Cells[4].Value.ToString();
                y = int.Parse(txt_slgban.Text);

                // Lấy lại số lượng tồn trong SanPham
                string solg = "Select SoLuongTon from SanPham where MaSP ='" + txt_mathuoc.Text + "'";
                SqlDataReader rd3 = chuoiketnoi.showtext(solg);
                while (rd3.Read())
                {
                    a = int.Parse(rd3[0].ToString());
                }

            }
            catch
            {
                MessageBox.Show("Trống!", "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            if (txt_dangthuoc.Text == "" || txt_gia.Text == "" || txt_mathuoc.Text == "" ||
    txt_tenthuoc.Text == "" || txt_thuoctrongkho.Text == "" || txt_slgban.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập đầy đủ thông tin", "Thông Báo", MessageBoxButtons.OK);
            }
            else
            {
                // Đổi sang decimal/int + TryParse cho an toàn
                decimal gia;
                int slBan;
                int slTon;
                decimal chietKhau;

                if (!decimal.TryParse(txt_gia.Text, out gia))
                {
                    MessageBox.Show("Đơn giá không đúng định dạng số!", "Thông báo", MessageBoxButtons.OK);
                    return;
                }

                if (!int.TryParse(txt_slgban.Text, out slBan))
                {
                    MessageBox.Show("Số lượng bán không hợp lệ!", "Thông báo", MessageBoxButtons.OK);
                    return;
                }

                if (!int.TryParse(txt_thuoctrongkho.Text, out slTon))
                {
                    MessageBox.Show("Số lượng tồn không hợp lệ!", "Thông báo", MessageBoxButtons.OK);
                    return;
                }

                if (txt_chietKhau.Text == "")
                {
                    MessageBox.Show("Bạn chưa nhập chiết khấu!", "Thông báo", MessageBoxButtons.OK);
                    return;
                }

                if (!decimal.TryParse(txt_chietKhau.Text, out chietKhau))
                {
                    MessageBox.Show("Chiết khấu không đúng định dạng số!", "Thông báo", MessageBoxButtons.OK);
                    return;
                }

                if (slBan > slTon)
                {
                    MessageBox.Show("Số lượng sản phẩm trong kho không đủ để bán\nBạn hãy nhập thêm hàng", "Thông báo ", MessageBoxButtons.OK);
                }
                else
                {
                    int slConLai = slTon - slBan;
                    decimal thanhTienDong = gia * slBan;

                    int n = dta2.Rows.Add();
                    decimal tongThanhTien = 0;

                    dta2.Rows[n].Cells[0].Value = txt_mathuoc.Text;
                    dta2.Rows[n].Cells[1].Value = txt_tenthuoc.Text;
                    dta2.Rows[n].Cells[2].Value = txt_dangthuoc.Text;
                    dta2.Rows[n].Cells[3].Value = gia.ToString();          // lưu lại giá
                    dta2.Rows[n].Cells[4].Value = slBan.ToString();         // số lượng bán
                    dta2.Rows[n].Cells[5].Value = thanhTienDong.ToString(); // thành tiền dòng

                    // Update lại số lượng tồn trong SanPham
                    string sql1 = "Update SanPham set SoLuongTon ='" + slConLai.ToString() +
                                  "' WHERE MaSP ='" + dta2.Rows[n].Cells[0].Value.ToString() + "'";
                    chuoiketnoi.luu(sql1);

                    // Load lại sản phẩm theo MaSP
                    string load1 = "Select MaSP,TenSP,LoaiSP,SoLuongTon,DonGia from SanPham where MaSP ='" +
                                   dta2.Rows[n].Cells[0].Value.ToString() + "' ";
                    chuoiketnoi.Chuoiketnoi(load1, db1);

                    // Clear textbox
                    txt_dangthuoc.Text = "";
                    txt_gia.Text = "";
                    txt_mathuoc.Text = "";
                    txt_tenthuoc.Text = "";
                    txt_thuoctrongkho.Text = "";
                    txt_slgban.Text = "";

                    int sc = dta2.Rows.Count;

                    for (int i = 0; i < sc - 1; i++)
                    {
                        decimal tt;
                        if (decimal.TryParse(dta2.Rows[i].Cells[5].Value.ToString(), out tt))
                        {
                            tongThanhTien += tt;
                        }
                    }

                    decimal m = tongThanhTien - tongThanhTien * chietKhau / 100;
                    lb_tien.Text = m.ToString() + " VNĐ";
                }
            }

        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            try
            {
                int slSauKhiTraLai = a + y;

                // Trả lại số lượng tồn trong SanPham
                string sql1 = "Update SanPham set SoLuongTon ='" + slSauKhiTraLai.ToString() + "'   WHERE MaSP ='" + txt_mathuoc.Text + "'";
                chuoiketnoi.Execute(sql1);

                // Load lại theo MaSP
                string load1 = "Select MaSP,TenSP,LoaiSP,SoLuongTon,DonGia from SanPham   where  MaSP ='" + txt_mathuoc.Text + "' ";
                chuoiketnoi.Chuoiketnoi(load1, db1);

                int seleRow = dta2.CurrentCell.RowIndex;
                dta2.Rows.RemoveAt(seleRow);

                double tongThanhTien = 0;
                int sc = dta2.Rows.Count;

                txt_dangthuoc.Text = "";
                txt_gia.Text = "";
                txt_mathuoc.Text = "";
                txt_tenthuoc.Text = "";
                txt_thuoctrongkho.Text = "";
                txt_slgban.Text = "";

                btn_Xoa.Enabled = false;

                for (int i = 0; i < sc - 1; i++)
                {
                    tongThanhTien += float.Parse(dta2.Rows[i].Cells[5].Value.ToString());
                }

                btn_Add.Enabled = true;
                txt_slgban.Enabled = true;
                btn_Xoa.Enabled = false;

                double g = double.Parse(txt_chietKhau.Text.ToString());
                double m = tongThanhTien - tongThanhTien * g / 100;
                lb_tien.Text = m.ToString();
            }
            catch
            {
                MessageBox.Show("Bạn chưa chọn sản phẩm để xóa!", "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            txt_dangthuoc.Text = "";
            txt_gia.Text = "";
            txt_mathuoc.Text = "";
            txt_tenthuoc.Text = "";
            txt_thuoctrongkho.Text = "";
            txt_slgban.Text = "";

            btn_Xoa.Enabled = false;
            txt_slgban.Enabled = true;
            btn_Add.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn thoát không?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                this.Close();
            }
        }

        private void TongTien_Click(object sender, EventArgs e)
        {
            try
            {
                if (txttim.Text == "")
                {
                    MessageBox.Show("Bạn chưa nhập tên file!", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    double tongThanhTien = 0;
                    string s = "";
                    int sc = dta2.Rows.Count;

                    for (int i = 0; i < sc - 1; i++)
                    {
                        s += dta2.Rows[i].Cells[1].Value.ToString() + " : " +
                             dta2.Rows[i].Cells[4].Value.ToString() + "    ,   ";

                        tongThanhTien += double.Parse(dta2.Rows[i].Cells[5].Value.ToString());
                    }

                    double g = double.Parse(txt_chietKhau.Text.ToString());
                    double m = tongThanhTien - tongThanhTien * g / 100;

                    // ================== LƯU VÀO HÓA ĐƠN MỚI ==================
                    // Lấy mã hóa đơn mới (MaHD)
                    int maHD = 1;
                    string sqlMaHd = "SELECT ISNULL(MAX(MaHD),0)+1 FROM HoaDon";
                    SqlDataReader rd = chuoiketnoi.showtext(sqlMaHd);
                    if (rd.Read())
                    {
                        maHD = int.Parse(rd[0].ToString());
                    }

                    int apDungGiamGia = g > 0 ? 1 : 0;

                    // Chỗ MaNV, MaKH hiện tại để NULL (DB cho phép), sau này bạn có thể sửa lại theo form
                    string insertHD =
                        "Insert into HoaDon(MaHD,NgayLap,TongTien,PhuongThucThanhToan,TinhTongTien,GhiChu,ApDungGiamGia,MaNV,MaKH) " +
                        "values(" +
                        maHD + ", '" + date1.Value + "', " + m.ToString().Replace(',', '.') +
                        ", N'Tiền mặt', 1, N'', " + apDungGiamGia + ", NULL, NULL)";

                    chuoiketnoi.them_dl1(insertHD);

                    // Thêm chi tiết hóa đơn
                    for (int i = 0; i < sc - 1; i++)
                    {
                        int maSP = int.Parse(dta2.Rows[i].Cells[0].Value.ToString());
                        int soLuong = int.Parse(dta2.Rows[i].Cells[4].Value.ToString());
                        double donGia = double.Parse(dta2.Rows[i].Cells[3].Value.ToString());
                        double thanhTien = double.Parse(dta2.Rows[i].Cells[5].Value.ToString());

                        string insertCT =
                            "Insert into ChiTietHoaDon(MaHD,MaSP,SoLuong,DonGia,ThanhTien,TinhThanhTien) " +
                            "values(" +
                            maHD + "," +
                            maSP + "," +
                            soLuong + "," +
                            donGia.ToString().Replace(',', '.') + "," +
                            thanhTien.ToString().Replace(',', '.') + ",1)";

                        chuoiketnoi.them_dl1(insertCT);
                    }

                    // ================== XUẤT EXCEL NHƯ CŨ ==================
                    string duongdan = "C:\\Users\\HPs\\Downloads\\BaiTapLon_QLsieuthi\\excel\\NhapNhieu";
                    string tenfile = txttim.Text.ToString();
                    XuatExecl.nhapnhieu(dta2, duongdan, tenfile, lb_tien.Text, lb_Tile.Text, txt_chietKhau.Text);

                    MessageBox.Show("Xuất file thành công", "Thông báo", MessageBoxButtons.OK);
                    MessageBox.Show("Đường dẫn file được lưu: " + duongdan, "Thông báo", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txt_slgban_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsControl(e.KeyChar) && !Char.IsNumber(e.KeyChar))
                e.Handled = true;
        }

        private void txt_chietKhau_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsControl(e.KeyChar) && !Char.IsNumber(e.KeyChar))
                e.Handled = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbPTTT.SelectedItem == null)
                return;

            if (cbbPTTT.SelectedItem.ToString() == "Chuyển khoản ngân hàng")
            {
                if (string.IsNullOrWhiteSpace(lb_tien.Text))
                {
                    MessageBox.Show("Bạn chưa tính tổng tiền!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tách số tiền: "150.000 VNĐ" -> 150000
                string text = lb_tien.Text.ToUpper()
                                           .Replace("VNĐ", "")
                                           .Replace("VND", "")
                                           .Trim();

                text = text.Replace(".", "").Replace(",", "");

                if (!decimal.TryParse(text, out decimal soTien))
                {
                    MessageBox.Show("Không đọc được số tiền!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Mở form QR thanh toán
                QuetQr f = new QuetQr(soTien);
                f.ShowDialog();
            }
        }

        private void txtTienNhan_TextChanged(object sender, EventArgs e)
        {
         
            if (string.IsNullOrWhiteSpace(lb_tien.Text))
            {
                txtTienThoi.Text = "";
                return;
            }

            
            string tongTienText = lb_tien.Text.ToUpper()
                                              .Replace("VNĐ", "")
                                              .Replace("VND", "")
                                              .Replace(".", "")
                                              .Replace(",", "")
                                              .Trim();

            decimal tongTien;
            if (!decimal.TryParse(tongTienText, out tongTien))
            {
                txtTienThoi.Text = "";
                return;
            }

            
            string tienNhanText = txtTienNhan.Text.Replace(".", "").Replace(",", "");

            decimal tienNhan;
            if (!decimal.TryParse(tienNhanText, out tienNhan))
            {
                txtTienThoi.Text = "";
                return;
            }

          
            decimal tienThoi = tienNhan - tongTien;

            if (tienThoi < 0)
            {
                txtTienThoi.Text = "Chưa đủ";
            }
            else
            {
                txtTienThoi.Text = tienThoi.ToString("N0") + " VNĐ";
            }
        }

        private void btnXacNhanThanhToan_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Kiểm tra giỏ hàng
                if (dta2.Rows.Count <= 1)
                {
                    MessageBox.Show("Chưa có sản phẩm nào trong hóa đơn!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. Lấy tổng tiền từ lb_tien (vd: "150.000 VNĐ")
                if (string.IsNullOrWhiteSpace(lb_tien.Text))
                {
                    MessageBox.Show("Chưa có tổng tiền!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string tongTienText = lb_tien.Text.ToUpper()
                                                  .Replace("VNĐ", "")
                                                  .Replace("VND", "")
                                                  .Trim();

                tongTienText = tongTienText.Replace(".", "").Replace(",", "");

                decimal tongTien;
                if (!decimal.TryParse(tongTienText, out tongTien))
                {
                    MessageBox.Show("Tổng tiền không hợp lệ!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 3. Kiểm tra tiền nhận (cho thanh toán tiền mặt / chuyển khoản đều được)
                if (string.IsNullOrWhiteSpace(txtTienNhan.Text))
                {
                    MessageBox.Show("Bạn chưa nhập tiền khách đưa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string tienNhanText = txtTienNhan.Text.Replace(".", "").Replace(",", "");
                decimal tienNhan;
                if (!decimal.TryParse(tienNhanText, out tienNhan))
                {
                    MessageBox.Show("Tiền khách đưa không hợp lệ!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (tienNhan < tongTien)
                {
                    MessageBox.Show("Tiền khách đưa chưa đủ!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 4. Xác định hình thức thanh toán
                string phuongThuc = "Tiền mặt";
                if (cbbPTTT != null && cbbPTTT.SelectedItem != null)
                {
                    phuongThuc = cbbPTTT.SelectedItem.ToString();
                }

                // 5. Chiết khấu
                decimal giam = 0;
                if (!string.IsNullOrWhiteSpace(txt_chietKhau.Text))
                    decimal.TryParse(txt_chietKhau.Text, out giam);

                int apDungGiamGia = giam > 0 ? 1 : 0;

                // 6. Lấy mã hóa đơn mới
                int maHD = 1;
                string sqlMaHd = "SELECT ISNULL(MAX(MaHD),0) + 1 FROM HoaDon";
                using (SqlDataReader rd = chuoiketnoi.showtext(sqlMaHd))
                {
                    if (rd.Read())
                        maHD = int.Parse(rd[0].ToString());
                }

                // 7. Thêm vào bảng HoaDon
                // MaNV, MaKH hiện để NULL, bạn có thể sửa lại lấy từ textbox nếu có
                string insertHD =
                    "INSERT INTO HoaDon(MaHD, NgayLap, TongTien, PhuongThucThanhToan, " +
                    "TinhTongTien, GhiChu, ApDungGiamGia, MaNV, MaKH) VALUES (" +
                    maHD + ", " +
                    "'" + date1.Value.ToString("yyyy-MM-dd") + "', " +
                    tongTien.ToString().Replace(',', '.') + ", " +
                    "N'" + phuongThuc + "', " +
                    "1, " +
                    "N'', " +
                    apDungGiamGia + ", " +
                    "NULL, NULL)";

                chuoiketnoi.them_dl1(insertHD);

                // 8. Thêm từng dòng vào ChiTietHoaDon
                int sc = dta2.Rows.Count;
                for (int i = 0; i < sc - 1; i++)
                {
                    int maSP = int.Parse(dta2.Rows[i].Cells[0].Value.ToString());
                    int soLuong = int.Parse(dta2.Rows[i].Cells[4].Value.ToString());

                    decimal donGia = decimal.Parse(dta2.Rows[i].Cells[3].Value.ToString());
                    decimal thanhTien = decimal.Parse(dta2.Rows[i].Cells[5].Value.ToString());

                    string insertCT =
                        "INSERT INTO ChiTietHoaDon(MaHD, MaSP, SoLuong, DonGia, ThanhTien, TinhThanhTien) VALUES (" +
                        maHD + ", " +
                        maSP + ", " +
                        soLuong + ", " +
                        donGia.ToString().Replace(',', '.') + ", " +
                        thanhTien.ToString().Replace(',', '.') + ", 1)";

                    chuoiketnoi.them_dl1(insertCT);
                }

                // 9. Mở form "Thanh toán thành công" để xuất Excel nếu cần
                // Lấy thông tin để in hóa đơn
                 tongTienText = lb_tien.Text;
                 tienNhanText = txtTienNhan.Text;
                string tienThoiText = txtTienThoi.Text;
                string hinhThucTT = "Tiền mặt";
                if (cbbPTTT.SelectedItem != null)
                    hinhThucTT = cbbPTTT.SelectedItem.ToString();

                // mở form thông báo
                ThanhToanThanhCong f = new ThanhToanThanhCong(
                    maHD,
                    dta2,
                    tongTienText,
                    tienNhanText,
                    tienThoiText,
                    hinhThucTT);

                f.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thanh toán: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}

