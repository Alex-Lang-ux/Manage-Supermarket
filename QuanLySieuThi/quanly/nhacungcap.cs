using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLySieuThi.quanly
{
    public partial class nhacungcap : Form
    {
        // Bảng mới: NhaCungCap
        public string chuoi = "Select * from NhaCungCap";

        public nhacungcap()
        {
            InitializeComponent();
            chuoiketnoi.Chuoiketnoi(chuoi, dta1);
            clear();
        }

        public void clear()
        {
            // Cột NhaCungCap: MaNCC, TenNCC, SoDienThoai, Email, DiaChi
            dta1.Columns[0].HeaderText = "Mã nhà cung cấp"; dta1.Columns[0].Width = 150;
            dta1.Columns[1].HeaderText = "Tên nhà cung cấp"; dta1.Columns[1].Width = 160;
            dta1.Columns[2].HeaderText = "Số điện thoại";
            dta1.Columns[3].HeaderText = "Email";
            dta1.Columns[4].HeaderText = "Địa chỉ";

            txt_manv.Text = "";
            txt_tennv.Text = "";
            txt_diachi.Text = "";
            txt_congno.Text = ""; // dùng làm Email
            txt_sdt.Text = "";

            btn_xoa.Enabled = false;
            btn_sua.Enabled = false;
            btn_them.Enabled = true;
        }

        private void nhacungcap_Load(object sender, EventArgs e)
        {

        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            // txt_congno bây giờ là Email
            if (txt_tennv.Text == "" || txt_diachi.Text == "" || txt_sdt.Text == "" || txt_congno.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập đầy đủ thông tin!", "Error", MessageBoxButtons.OK);
            }
            else
            {
                // Giả định MaNCC là IDENTITY trong DB
                string sql1 = "INSERT INTO NhaCungCap (TenNCC, SoDienThoai, Email, DiaChi) " +
                              "VALUES (N'" + txt_tennv.Text + "', '" + txt_sdt.Text + "', '" + txt_congno.Text + "', N'" + txt_diachi.Text + "')";
                chuoiketnoi.them_dl(sql1, dta1);
                chuoiketnoi.Chuoiketnoi(chuoi, dta1);
                clear();
            }
        }

        private void btn_sua_Click(object sender, EventArgs e)
        {
            // Cập nhật theo cột mới
            string sql = "UPDATE NhaCungCap SET " +
                         "TenNCC = N'" + txt_tennv.Text + "', " +
                         "DiaChi = N'" + txt_diachi.Text + "', " +
                         "SoDienThoai = '" + txt_sdt.Text + "', " +
                         "Email = '" + txt_congno.Text + "'" +
                         " WHERE MaNCC = '" + txt_manv.Text + "'";

            chuoiketnoi.Execute1(sql);
            chuoiketnoi.Chuoiketnoi(chuoi, dta1);
            clear();
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            string sql = "DELETE FROM NhaCungCap WHERE MaNCC = '" + txt_manv.Text + "'";
            chuoiketnoi.Execute(sql);
            chuoiketnoi.Chuoiketnoi(chuoi, dta1);
            clear();
        }

        private void btn_nhaplai_Click(object sender, EventArgs e)
        {
            clear();
            btn_them.Enabled = true;
        }

        private void btn_ex_Click(object sender, EventArgs e)
        {
            string duongdan = @"C:\Demo\SupermarketManagement-main\excel";
            string tenfile = "QuanLyNhaCungCap";
            XuatExecl.exportecxel(dta1, duongdan, tenfile);
            MessageBox.Show("Xuất file thành công", "Thông báo ", MessageBoxButtons.OK);
            MessageBox.Show("Đường dẫn file được lưu: " + duongdan + MessageBoxButtons.OK);
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                this.Close();
        }

        private void dta1_Click(object sender, EventArgs e)
        {
            int curow = dta1.CurrentRow.Index;

            // Map cột mới:
            // 0: MaNCC, 1: TenNCC, 2: SoDienThoai, 3: Email, 4: DiaChi
            txt_manv.Text = dta1.Rows[curow].Cells[0].Value.ToString();
            txt_tennv.Text = dta1.Rows[curow].Cells[1].Value.ToString();
            txt_sdt.Text = dta1.Rows[curow].Cells[2].Value.ToString();
            txt_congno.Text = dta1.Rows[curow].Cells[3].Value.ToString(); // Email
            txt_diachi.Text = dta1.Rows[curow].Cells[4].Value.ToString();

            txt_manv.Enabled = false;
            btn_them.Enabled = false;
            btn_sua.Enabled = true;
            btn_xoa.Enabled = true;
        }

        private void txt_sdt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsControl(e.KeyChar) && !Char.IsNumber(e.KeyChar))
                e.Handled = true;
        }

        
        private void txt_congno_KeyPress(object sender, KeyPressEventArgs e)
        {
          
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            string load1 = "SELECT * FROM NhaCungCap WHERE TenNCC LIKE N'%" + txt_search.Text + "%'";
            chuoiketnoi.timkiem(load1, dta1);
            clear();
        }
    }
}
