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
    public partial class TheKHTT : Form
    {
        public TheKHTT(string maKH)
        {
            InitializeComponent();
            LoadThongTin(maKH);
        }
        private void LoadThongTin(string maKH)
        {
            string sqlKH = $"SELECT HoTen, DiemMuaHang FROM KhachHang WHERE MaKH = {maKH}";
            DataTable dtKH = chuoiketnoi.GetDataTable(sqlKH);

            if (dtKH.Rows.Count > 0)
            {
                txtTenKH.Text = dtKH.Rows[0]["HoTen"].ToString();
                int diem = Convert.ToInt32(dtKH.Rows[0]["DiemMuaHang"]);
                string hang = diem >= 5000 ? "Vàng" : diem >= 2000 ? "Bạc" : diem >= 1000 ? "Đồng" : "Không";
                string sqlRank = $"SELECT COUNT(*) + 1 FROM KhachHang WHERE DiemMuaHang > {diem}";
                int thuHangSo = Convert.ToInt32(chuoiketnoi.ExecuteScalar(sqlRank));


                txtThuHang.Text = hang == "Không" ? hang : $"{hang} ({thuHangSo})";
                string sqlThe = $"SELECT QuyenTang, ThoiHan FROM TheKhachHangThanThiet WHERE MaKH = {maKH}";
                DataTable dtThe = chuoiketnoi.GetDataTable(sqlThe);
                if (dtThe.Rows.Count > 0)
                {
                    txtQuyenTang.Text = dtThe.Rows[0]["QuyenTang"].ToString();
                    dtpHetHan.Value = Convert.ToDateTime(dtThe.Rows[0]["ThoiHan"]);
                }
            }
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void TheKHTT_Load(object sender, EventArgs e)
        {

        }

        private void bt_thoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtThuHang_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtThuHang_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}
