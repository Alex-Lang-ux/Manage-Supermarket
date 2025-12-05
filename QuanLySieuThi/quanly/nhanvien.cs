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

namespace QuanLySieuThi.quanly
{
    public partial class nhanvien : Form
    {
        public string chuoi = "SELECT NV.MaNV, NV.HoTen, CV.TenChucVu, NV.SoDienThoai, NV.DiaChi, NV.NgayVaoLam, NV.LuongCoBan, NV.MaChucVu " +
                       "FROM NhanVien NV LEFT JOIN ChucVu CV ON NV.MaChucVu = CV.MaChucVu";

        public nhanvien()
        {
            InitializeComponent();
            LoadNhanVien();
            LoadChucVu();
            dta1.AllowUserToAddRows = false;
        }
        private void LoadChucVu()
        {
            string sql = "SELECT MaChucVu, TenChucVu FROM ChucVu";
            DataTable dtChucVu = chuoiketnoi.GetDataTable(sql);

            cb_chucvu.DataSource = dtChucVu;
            cb_chucvu.DisplayMember = "TenChucVu";
            cb_chucvu.ValueMember = "MaChucVu";
            cb_chucvu.SelectedIndex = -1;
        }
        private void LoadSelectedRow()
        {
            if (dta1.Rows.Count == 0) return;

            if (dta1.CurrentRow == null)
                dta1.CurrentCell = dta1.Rows[0].Cells[0];

            int r = dta1.CurrentRow.Index;

            txt_manv.Text = dta1.Rows[r].Cells["MaNV"].Value?.ToString() ?? "";
            txt_tennv.Text = dta1.Rows[r].Cells["HoTen"].Value?.ToString() ?? "";
            txtsdt.Text = dta1.Rows[r].Cells["SoDienThoai"].Value?.ToString() ?? "";
            txt_diachi.Text = dta1.Rows[r].Cells["DiaChi"].Value?.ToString() ?? "";
            dtp_ngayvl.Value = Convert.ToDateTime(dta1.Rows[r].Cells["NgayVaoLam"].Value);
            txt_luong.Text = dta1.Rows[r].Cells["LuongCoBan"].Value?.ToString() ?? "";

            cb_chucvu.SelectedValue = dta1.Rows[r].Cells["MaChucVu"].Value ?? -1;

            txt_manv.Enabled = false;
            btn_them.Enabled = false;
            btn_sua.Enabled = true;
            btn_xoa.Enabled = true;
        }


        private void ClearForm()
        {
            txt_manv.Text = "";
            txt_tennv.Text = "";
            txtsdt.Text = "";
            txt_diachi.Text = "";
            dtp_ngayvl.Value = DateTime.Today;
            txt_luong.Text = "";
            cb_chucvu.SelectedIndex = -1;

            txt_manv.Enabled = true;
            btn_them.Enabled = true;
            btn_sua.Enabled = false;
            btn_xoa.Enabled = false;

            dta1.ClearSelection();
        }


        public void clear()
        {
            if (dta1.Rows.Count == 0)
            {
                ClearForm();
                return;
            }

            LoadSelectedRow();
        }

        private void LoadNhanVien()
        {
            chuoiketnoi.Chuoiketnoi(chuoi, dta1);
            dta1.Columns[0].HeaderText = "Mã NV";
            dta1.Columns[1].HeaderText = "Họ tên";
            dta1.Columns[2].HeaderText = "Chức vụ";
            dta1.Columns[3].HeaderText = "Số ĐT";
            dta1.Columns[4].HeaderText = "Địa chỉ";
            dta1.Columns[5].HeaderText = "Ngày vào làm";
            dta1.Columns[6].HeaderText = "Lương cơ bản";
        }
        private void nhanvien_Load(object sender, EventArgs e)
        {
            LoadChucVu();
            LoadNhanVien();
            clear();
        }

        private void dta1_Click(object sender, EventArgs e)
        {
            int r = dta1.CurrentRow.Index;
            txt_manv.Text = dta1.Rows[r].Cells[0].Value.ToString();
            txt_tennv.Text = dta1.Rows[r].Cells[1].Value.ToString();
            cb_chucvu.SelectedValue = dta1.Rows[r].Cells["MaChucVu"].Value ?? -1;
            txtsdt.Text = dta1.Rows[r].Cells[3].Value.ToString();
            txt_diachi.Text = dta1.Rows[r].Cells[4].Value.ToString();
            dtp_ngayvl.Value = Convert.ToDateTime(dta1.Rows[r].Cells[5].Value);
            txt_luong.Text = dta1.Rows[r].Cells[6].Value.ToString();
            txt_manv.Enabled = false;
            btn_them.Enabled = false;
            btn_sua.Enabled = true;
            btn_xoa.Enabled = true;
        }

        private void btn_them_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txt_tennv.Text) || cb_chucvu.SelectedIndex < 0 ||
                 string.IsNullOrWhiteSpace(txtsdt.Text) || string.IsNullOrWhiteSpace(txt_diachi.Text))
            {
                MessageBox.Show("Bạn chưa nhập đầy đủ thông tin!");
                return;
            }

            string sql = "INSERT INTO NhanVien (HoTen, MaChucVu, SoDienThoai, DiaChi, NgayVaoLam, LuongCoBan) " +
                         "VALUES (@HoTen, @MaChucVu, @SoDienThoai, @DiaChi, @NgayVaoLam, @LuongCoBan)";

            using (var con = new SqlConnection(chuoiketnoi.sqlcon))
            {
                con.Open();
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@HoTen", txt_tennv.Text);
                    cmd.Parameters.AddWithValue("@MaChucVu", cb_chucvu.SelectedValue);
                    cmd.Parameters.AddWithValue("@SoDienThoai", txtsdt.Text);
                    cmd.Parameters.AddWithValue("@DiaChi", txt_diachi.Text);
                    cmd.Parameters.AddWithValue("@NgayVaoLam", dtp_ngayvl.Value);
                    cmd.Parameters.AddWithValue("@LuongCoBan", Convert.ToDecimal(txt_luong.Text));

                    cmd.ExecuteNonQuery();
                }
            }

            LoadNhanVien();
            clear();
        }

        private void txtsdt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsControl(e.KeyChar) && !Char.IsNumber(e.KeyChar))
                e.Handled = true;
        }

        private void btn_sua_Click(object sender, EventArgs e)
        {
            string sql = "UPDATE NhanVien SET HoTen=@HoTen, MaChucVu=@MaChucVu, SoDienThoai=@SoDienThoai, DiaChi=@DiaChi, " +
                          "NgayVaoLam=@NgayVaoLam, LuongCoBan=@LuongCoBan WHERE MaNV=@MaNV";

            using (var con = new SqlConnection(chuoiketnoi.sqlcon))
            {
                con.Open();
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@HoTen", txt_tennv.Text);
                    cmd.Parameters.AddWithValue("@MaChucVu", cb_chucvu.SelectedValue);
                    cmd.Parameters.AddWithValue("@SoDienThoai", txtsdt.Text);
                    cmd.Parameters.AddWithValue("@DiaChi", txt_diachi.Text);
                    cmd.Parameters.AddWithValue("@NgayVaoLam", dtp_ngayvl.Value);
                    cmd.Parameters.AddWithValue("@LuongCoBan", txt_luong.Text);
                    cmd.Parameters.AddWithValue("@MaNV", txt_manv.Text);

                    cmd.ExecuteNonQuery();
                }
            }

            LoadNhanVien();
            clear();
        }

        private void btn_nhaplai_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            string sql = "DELETE FROM NhanVien WHERE MaNV=@MaNV";
            using (var con = new SqlConnection(chuoiketnoi.sqlcon))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        string sqlTaiKhoan = "DELETE FROM TaiKhoan WHERE MaNV=@MaNV";
                        using (var cmdTk = new SqlCommand(sqlTaiKhoan, con, tran))
                        {
                            cmdTk.Parameters.AddWithValue("@MaNV", txt_manv.Text);
                            cmdTk.ExecuteNonQuery();
                        }
                        string sqlNV = "DELETE FROM NhanVien WHERE MaNV=@MaNV";
                        using (var cmdNv = new SqlCommand(sqlNV, con, tran))
                        {
                            cmdNv.Parameters.AddWithValue("@MaNV", txt_manv.Text);
                            cmdNv.ExecuteNonQuery();
                        }

                        tran.Commit();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 547)
                        {
                            MessageBox.Show(
                                "Không thể xóa nhân viên này vì còn dữ liệu liên quan trong bảng khác",
                                "Lỗi xóa dữ liệu",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            );
                        }
                        else
                        {
                            MessageBox.Show($"Lỗi SQL: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            LoadNhanVien();
            clear();

        }

        private void btn_ex_Click(object sender, EventArgs e)
        {

            string duongdan = "D:\\Github\\Project\\Csharp_QuanLySieuThi\\excel\\taikhoan\\";
            string tenfile = "QLnhanvien";
            XuatExecl.exportecxel(dta1, duongdan, tenfile);
            MessageBox.Show("Xuất file thành công ", "Thông báo ", MessageBoxButtons.OK);
            MessageBox.Show("Duong dan file dc luu :" + duongdan + MessageBoxButtons.OK);
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                this.Close();
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            string load1 = "Select * from nhanvien where tennv like N'%" + txt_search.Text + "%' ";
            chuoiketnoi.timkiem(load1, dta1);
            clear();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void txt_manv_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void cb_chucvu_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
