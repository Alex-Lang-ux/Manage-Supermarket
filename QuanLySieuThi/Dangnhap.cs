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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace QuanLySieuThi
{
    public partial class Dangnhap : Form
    {
        public static string sqlcon = @"Data Source=LAPTOP-VM2HOL2M\SQLEXPRESS;Initial Catalog=QuanLySieuThi;Integrated Security=True";
        public static SqlConnection mycon;
        public static SqlCommand com;
        public static SqlDataAdapter ad;
        public static DataTable dt;
        public static SqlCommandBuilder bd;

        // Biến đếm số lần đăng nhập sai
        private int failedAttempts = 0;

        public Dangnhap()
        {
            InitializeComponent();
        }
        public static void Chuoiketnoi(string chuoi, DataGridView db1)
        {
            try
            {

                ad = new SqlDataAdapter(chuoi, sqlcon);
                dt = new DataTable();
                bd = new SqlCommandBuilder(ad);
                ad.Fill(dt);
                db1.DataSource = dt;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể kết nối " + ex, "Thông báo ! ");

            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxHienThiMatKhau.Checked) { txt_mk.UseSystemPasswordChar = true; } else txt_mk.UseSystemPasswordChar = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_tk.Text))
            {
                MessageBox.Show("Bạn chưa nhập tên tài khoản!", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrWhiteSpace(txt_mk.Text))
            {
                MessageBox.Show("Bạn chưa nhập mật khẩu!", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(sqlcon))
                {
                    con.Open();
                    string sqlAdmin = $"SELECT COUNT(*) FROM Admin WHERE TenDangNhap='{txt_tk.Text}' AND MatKhau='{txt_mk.Text}'";
                    int a = ExecuteCount(sqlAdmin, con, txt_tk.Text.Trim(), txt_mk.Text.Trim());
                    string sqlNV = $"SELECT COUNT(*) FROM TaiKhoan WHERE TenDangNhap='{txt_tk.Text}' AND MatKhau='{txt_mk.Text}'";
                    int b = ExecuteCount(sqlNV, con, txt_tk.Text.Trim(), txt_mk.Text.Trim());

                    if (a > 0)
                    {
                        // Đăng nhập thành công -> reset lại số lần nhập sai
                        failedAttempts = 0;

                        MessageBox.Show("Bạn đã đăng nhập vào tài khoản Admin", "Thông báo", MessageBoxButtons.OK);
                        OpenMainForm("Admin");
                    }
                    else if (b > 0)
                    {
                        // Đăng nhập thành công -> reset lại số lần nhập sai
                        failedAttempts = 0;

                        MessageBox.Show("Bạn đã đăng nhập vào tài khoản Nhân Viên", "Thông báo", MessageBoxButtons.OK);
                        OpenMainForm("Nhân viên");
                    }
                    else
                    {
                        // Đăng nhập sai
                        failedAttempts++;

                        if (failedAttempts >= 5)
                        {
                            MessageBox.Show("Bạn đã nhập sai 5 lần. Chương trình sẽ kết thúc!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                            return;
                        }
                        else
                        {
                            int conLai = 5 - failedAttempts;
                            MessageBox.Show("Tên đăng nhập hoặc mật khẩu sai! Bạn còn " + conLai + " lần thử.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đăng nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK);
            }

        }
        private int ExecuteCount(string sql, SqlConnection con, string tk, string mk)
        {
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@tk", tk);
                cmd.Parameters.AddWithValue("@mk", mk);
                return (int)cmd.ExecuteScalar();
            }
        }
        private void OpenMainForm(string quyen)
        {
            main2 mainForm = new main2();
            mainForm.Show();
            this.Hide();
            mainForm.lb_quyen.Text = quyen;
            //switch (quyen)
            //{
            //    case "Admin":
            //        mainForm.bh_phieunhap.Visible = false;
            //        mainForm.bh_xuatle.Visible = false;
            //        break;
            //    case "Nhân viên":
            //        mainForm.mn_admin.Visible = false;
            //        mainForm.mn_tkquanly.Visible = false;
            //        mainForm.mn_nhanvien.Visible = false;
            //        mainForm.ql_phieunhap.Visible = false;

            //        mainForm.ql_nhanvien.Visible = false;
            //        break;
            //    case "Quản Lý":
            //        mainForm.mn_admin.Visible = false;
            //        mainForm.mn_tkquanly.Visible = false;
            //        mainForm.mn_nhanvien.Visible = false;
            //        mainForm.bh_phieunhap.Visible = false;
            //        mainForm.bh_xuatle.Visible = false;
            //        break;
            //}
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                this.Close();
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Dangnhap_Load(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {


        }

        private void txt_tk_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
