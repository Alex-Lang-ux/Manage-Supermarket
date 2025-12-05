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
    public partial class khachhang : Form
    {
        public string chuoi = "SELECT * FROM KhachHang";
        public khachhang()
        {
            InitializeComponent();
            chuoiketnoi.Chuoiketnoi(chuoi, dta1);
            clear();
            LoadKhachHang();
            btnXemThe.Visible = false;
            btnXemThe.Enabled = false;
        }
        private void LoadKhachHang()
        {
            string sqlKH = "SELECT * FROM KhachHang ORDER BY DiemMuaHang DESC";
            DataTable dtKH = chuoiketnoi.GetDataTable(sqlKH);

            if (!dtKH.Columns.Contains("KhachHangThanThiet"))
                dtKH.Columns.Add("KhachHangThanThiet", typeof(string));
            int rank = 1;
            foreach (DataRow row in dtKH.Rows)
            {
                int diem = row["DiemMuaHang"] != DBNull.Value ? Convert.ToInt32(row["DiemMuaHang"]) : 0;
                string makh = row["MaKH"].ToString();
                string hang = "";

                if (diem >= 5000)
                    hang = "Vàng";
                else if (diem >= 2000)
                    hang = "Bạc";
                else if (diem >= 1000)
                    hang = "Đồng";

                row["KhachHangThanThiet"] = string.IsNullOrEmpty(hang) ? "Không" : hang;

                if (!string.IsNullOrEmpty(hang))
                {
                    string checkSql = $"SELECT COUNT(*) FROM TheKhachHangThanThiet WHERE MaKH = '{makh}'";
                    int count = chuoiketnoi.ExecuteScalar(checkSql);

                    int days = hang == "Đồng" ? 30 : hang == "Bạc" ? 40 : 50;

                    if (count == 0)
                    {
                        string insertSql = $"INSERT INTO TheKhachHangThanThiet " +
                                           $"(MaKH, ThuHangThanhVien, QuyenTang, ThoiHan, LapThe, InThe, TraCu) " +
                                           $"VALUES ('{makh}', '{hang}', '', DATEADD(DAY,{days},GETDATE()), 0, 0, 0)";
                        chuoiketnoi.luu(insertSql);
                    }
                }
            }

            dta1.DataSource = dtKH;
        }


        private string TaoMaKH()
        {
            const string chars = "0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private void khachhang_Load(object sender, EventArgs e)
        {

        }
        public void clear()
        {
            dta1.Columns[0].HeaderText = "Mã KH";
            dta1.Columns[1].HeaderText = "Họ tên";
            dta1.Columns[2].HeaderText = "SĐT";
            dta1.Columns[3].HeaderText = "Email";
            dta1.Columns[4].HeaderText = "Địa chỉ";
            dta1.Columns[5].HeaderText = "Ngày mua gần đây";
            dta1.Columns[6].HeaderText = "Điểm";
            dta1.Columns[7].HeaderText = "Cập nhật";
            dta1.Columns[8].HeaderText = "Tích điểm";

            txt_makh.Text = "";
            txt_tenkh.Text = "";
            txt_sdt.Text = "";
            txt_email.Text = "";
            txt_diachi.Text = "";

            btn_them.Enabled = true;
            btn_sua.Enabled = false;
            btn_xoa.Enabled = false;
        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            txt_makh.Text = TaoMaKH();

            if (string.IsNullOrWhiteSpace(txt_tenkh.Text) ||
                string.IsNullOrWhiteSpace(txt_sdt.Text) ||
                string.IsNullOrWhiteSpace(txt_email.Text) ||
                string.IsNullOrWhiteSpace(txt_diachi.Text))
            {
                MessageBox.Show("Bạn chưa nhập đủ thông tin!");
                return;
            }

            try
            {
                string sql = "INSERT INTO KhachHang " +
                    "(MaKH, HoTen, SoDienThoai, Email, DiaChi,  DiemMuaHang, CapNhatThongTin, TichDiem) " +
                    "VALUES (@MaKH, @HoTen, @SoDienThoai, @Email, @DiaChi,  0, 0, 0)";

                using (var con = new SqlConnection(chuoiketnoi.sqlcon))
                {
                    con.Open();
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@MaKH", txt_makh.Text);
                        cmd.Parameters.AddWithValue("@HoTen", txt_tenkh.Text);
                        cmd.Parameters.AddWithValue("@SoDienThoai", txt_sdt.Text);
                        cmd.Parameters.AddWithValue("@Email", txt_email.Text);
                        cmd.Parameters.AddWithValue("@DiaChi", txt_diachi.Text);
                        cmd.ExecuteNonQuery();
                    }
                }

                chuoiketnoi.Chuoiketnoi(chuoi, dta1);
                clear();
                MessageBox.Show("Thêm thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm khách hàng: " + ex.Message);
            }
        }

        private void dta1_Click(object sender, EventArgs e)
        {
            int r = dta1.CurrentRow.Index;

            txt_makh.Text = dta1.Rows[r].Cells[0].Value.ToString();
            txt_tenkh.Text = dta1.Rows[r].Cells[1].Value.ToString();
            txt_sdt.Text = dta1.Rows[r].Cells[2].Value.ToString();
            txt_email.Text = dta1.Rows[r].Cells[3].Value.ToString();
            txt_diachi.Text = dta1.Rows[r].Cells[4].Value.ToString();

            txt_makh.Enabled = false;
            btn_them.Enabled = false;
            btn_sua.Enabled = true;
            btn_xoa.Enabled = true;

            string hang = dta1.Rows[r].Cells["KhachHangThanThiet"].Value.ToString();
            string maKH = txt_makh.Text;
            btnXemThe.Visible = false;
            btnXemThe.Enabled = false;

            if (hang != "Không")
            {
                string sqlCheck = $"SELECT COUNT(*) FROM TheKhachHangThanThiet WHERE MaKH = '{maKH}'";
                int count = chuoiketnoi.ExecuteScalar(sqlCheck);

                if (count > 0)
                {
                    btnXemThe.Visible = true;
                    btnXemThe.Enabled = true;
                }
            }
        }

        private void txt_sdt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsControl(e.KeyChar) && !Char.IsNumber(e.KeyChar))
                e.Handled = true;
        }


        private void btn_sua_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txt_tenkh.Text) ||
                string.IsNullOrWhiteSpace(txt_sdt.Text) ||
                string.IsNullOrWhiteSpace(txt_email.Text) ||
                string.IsNullOrWhiteSpace(txt_diachi.Text))
            {
                MessageBox.Show("Bạn chưa nhập đủ thông tin!");
                return;
            }

            try
            {
                string sql = "UPDATE KhachHang SET " +
                             "HoTen=@HoTen, SoDienThoai=@SoDienThoai, Email=@Email, DiaChi=@DiaChi " +
                             "WHERE MaKH=@MaKH";

                using (var con = new SqlConnection(chuoiketnoi.sqlcon))
                {
                    con.Open();
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@HoTen", txt_tenkh.Text);
                        cmd.Parameters.AddWithValue("@SoDienThoai", txt_sdt.Text);
                        cmd.Parameters.AddWithValue("@Email", txt_email.Text);
                        cmd.Parameters.AddWithValue("@DiaChi", txt_diachi.Text);
                        cmd.Parameters.AddWithValue("@MaKH", txt_makh.Text);

                        cmd.ExecuteNonQuery();
                    }
                }

                chuoiketnoi.Chuoiketnoi(chuoi, dta1);
                clear();
                MessageBox.Show("Cập nhật thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật khách hàng: " + ex.Message);
            }
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            string sql = "DELETE FROM KhachHang WHERE MaKH = " + txt_makh.Text;

            chuoiketnoi.Execute(sql);
            chuoiketnoi.Chuoiketnoi(chuoi, dta1);
            clear();
        }

        private void btn_nhaplai_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void btn_ex_Click(object sender, EventArgs e)
        {
            string duongdan = "D:\\Github\\Project\\Csharp_QuanLySieuThi\\excel\\taikhoan\\";
            string tenfile = "Thongtin_khachhang";
            XuatExecl.exportecxel(dta1, duongdan, tenfile);
            MessageBox.Show("Xuất file thành công ", "Thông báo ", MessageBoxButtons.OK);
            MessageBox.Show("Duong dan file dc luu :" + duongdan + MessageBoxButtons.OK);
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Thoát?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                this.Close();
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            string sql =
                 "SELECT * FROM KhachHang WHERE HoTen LIKE N'%" + txt_timkiem.Text + "%'";

            chuoiketnoi.timkiem(sql, dta1);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dta1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txt_email_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_sdt_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int r = dta1.CurrentRow.Index;
            string maKH = dta1.Rows[r].Cells["MaKH"].Value.ToString();

            TheKHTT f = new TheKHTT(maKH);
            f.ShowDialog();
        }
    }

}
