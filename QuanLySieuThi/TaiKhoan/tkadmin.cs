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

namespace QuanLySieuThi
{


    public partial class tkadmin : Form
    {
        string sqlSelect = "SELECT MaAdmin, TenDangNhap, MatKhau, HoTen, Email, SoDienThoai, NgayTao, QuyenHan FROM Admin";
        public tkadmin()
        {
            InitializeComponent();
            LoadAdmin();
            ClearForm();
            dta1.AllowUserToAddRows = false;
            dtpNgayTao.Enabled = false;
        }

        private void LoadAdmin()
        {
            chuoiketnoi.Chuoiketnoi(sqlSelect, dta1);
            dta1.Columns["MaAdmin"].HeaderText = "Mã Admin";
            dta1.Columns["TenDangNhap"].HeaderText = "Tài khoản";
            dta1.Columns["MatKhau"].HeaderText = "Mật khẩu";
            dta1.Columns["HoTen"].HeaderText = "Họ tên";
            dta1.Columns["Email"].HeaderText = "Email";
            dta1.Columns["SoDienThoai"].HeaderText = "SĐT";
            dta1.Columns["NgayTao"].HeaderText = "Ngày tạo";
            dta1.Columns["QuyenHan"].HeaderText = "Quyền hạn";
        }

        private void ClearForm()
        {
            txt_tk.Clear();
            txt_mk.Clear();
            txt_hoten.Clear();
            txt_email.Clear();
            txt_sdt.Clear();
            dtpNgayTao.Value = DateTime.Today;

            txt_tk.Enabled = true;
            txt_mk.Enabled = true;
            dtpNgayTao.Enabled = true;

            btn_them.Enabled = true;
            btn_sua.Enabled = false;
            btn_xoa.Enabled = false;

            dta1.ClearSelection();
        }
        private string GachaSoMa()
        {
            Random rnd = new Random();
            string code;

            using (SqlConnection con = new SqlConnection(chuoiketnoi.sqlcon))
            {
                con.Open();

                do
                {
                    code = "";
                    for (int i = 0; i < 6; i++)
                        code += rnd.Next(0, 10).ToString();
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Admin WHERE MaAdmin=@code", con);
                    cmd.Parameters.AddWithValue("@code", code);
                    int count = (int)cmd.ExecuteScalar();

                    if (count == 0)
                        break;
                }
                while (true);
            }

            return code;
        }
        private void btn_them_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_tk.Text) || string.IsNullOrWhiteSpace(txt_mk.Text))
            {
                MessageBox.Show("Tài khoản và mật khẩu không được bỏ trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string newCode = GachaSoMa();

            string sql = "INSERT INTO Admin (MaAdmin, TenDangNhap, MatKhau, HoTen, Email, SoDienThoai, NgayTao, QuyenHan) " +
                         "VALUES (@ma, @tk, @mk, @hoten, @email, @sdt, @ngaytao, @quyen)";

            using (SqlConnection con = new SqlConnection(chuoiketnoi.sqlcon))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@ma", newCode);
                    cmd.Parameters.AddWithValue("@tk", txt_tk.Text);
                    cmd.Parameters.AddWithValue("@mk", txt_mk.Text);
                    cmd.Parameters.AddWithValue("@hoten", txt_hoten.Text);
                    cmd.Parameters.AddWithValue("@email", txt_email.Text);
                    cmd.Parameters.AddWithValue("@sdt", txt_sdt.Text);
                    cmd.Parameters.AddWithValue("@ngaytao", dtpNgayTao.Value);
                    cmd.Parameters.AddWithValue("@quyen", "Admin");

                    cmd.ExecuteNonQuery();
                }
            }

            LoadAdmin();
            ClearForm();
        }

        private void btn_nhaplai_Click(object sender, EventArgs e)
        {
            ClearForm();
            txt_tk.Enabled = true;
            btn_them.Enabled = true;
            btn_xoa.Enabled = false;
            btn_sua.Enabled = false;
        }

        private void btn_sua_Click(object sender, EventArgs e)
        {
            string sql = "UPDATE Admin SET MatKhau=@mk, HoTen=@hoten, Email=@email, SoDienThoai=@sdt WHERE TenDangNhap=@tk";

            using (SqlConnection con = new SqlConnection(chuoiketnoi.sqlcon))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@mk", txt_mk.Text);
                    cmd.Parameters.AddWithValue("@hoten", txt_hoten.Text);
                    cmd.Parameters.AddWithValue("@email", txt_email.Text);
                    cmd.Parameters.AddWithValue("@sdt", txt_sdt.Text);
                    cmd.Parameters.AddWithValue("@tk", txt_tk.Text);

                    cmd.ExecuteNonQuery();
                }
            }

            LoadAdmin();
            ClearForm();

        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            int countAdmin = 0;
            using (SqlConnection con = new SqlConnection(chuoiketnoi.sqlcon))
            {
                con.Open();
                SqlCommand cmdCount = new SqlCommand("SELECT COUNT(*) FROM Admin", con);
                countAdmin = (int)cmdCount.ExecuteScalar();
            }

            if (countAdmin <= 1)
            {
                MessageBox.Show("Không thể xóa tài khoản này vì phải luôn có ít nhất 1 Admin.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa tài khoản này?", "Xác nhận", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string sql = "DELETE FROM Admin WHERE TenDangNhap=@tk";
                using (SqlConnection con = new SqlConnection(chuoiketnoi.sqlcon))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@tk", txt_tk.Text);
                        cmd.ExecuteNonQuery();
                    }
                }

                LoadAdmin();
                ClearForm();
            }
        }


        private void btn_ex_Click(object sender, EventArgs e)
        {
            string duongdan = "C:\\Users\\HPs\\Downloads\\BaiTapLon_QLsieuthi\\excel\\taikhoan\\";
            string tenfile = "TaiKhoanAdmin";
            XuatExecl.exportecxel(dta1, duongdan, tenfile);
            MessageBox.Show("Xuất file thành công ", "Thông báo ", MessageBoxButtons.OK);
            MessageBox.Show("Duong dan file dc luu :" + duongdan + MessageBoxButtons.OK);
        }

        private void dta1_Click(object sender, EventArgs e)
        {
            if (dta1.CurrentRow == null) return;
            int r = dta1.CurrentRow.Index;

            txt_tk.Text = dta1.Rows[r].Cells["TenDangNhap"].Value.ToString();
            txt_mk.Text = dta1.Rows[r].Cells["MatKhau"].Value.ToString();
            txt_hoten.Text = dta1.Rows[r].Cells["HoTen"].Value.ToString();
            txt_email.Text = dta1.Rows[r].Cells["Email"].Value.ToString();
            txt_sdt.Text = dta1.Rows[r].Cells["SoDienThoai"].Value.ToString();
            dtpNgayTao.Value = Convert.ToDateTime(dta1.Rows[r].Cells["NgayTao"].Value);

            txt_tk.Enabled = false;
            txt_mk.Enabled = true;
            dtpNgayTao.Enabled = false;

            btn_them.Enabled = false;
            btn_sua.Enabled = true;
            btn_xoa.Enabled = true;
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Bạn có chắc chắn muốn thoát không ?", "Thông báo ", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                this.Close();
            }
        }

        private void tkadmin_Load(object sender, EventArgs e)
        {

        }

        private void dta1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void txt_sdt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txt_sdt_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
