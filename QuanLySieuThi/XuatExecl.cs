using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Data;
using System.Data.SqlClient;
using app = Microsoft.Office.Interop.Excel.Application;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
namespace QuanLySieuThi
{
    class XuatExecl
    {

        public static void exportecxel(DataGridView g, string duongdan, string tenfile)
        {
            duongdan = @"C:\Demo\SupermarketManagement-main\excel";
            app obj = new app();
            obj.Application.Workbooks.Add(Type.Missing);
            obj.Columns.ColumnWidth = 25;
            for (int i = 1; i < g.Columns.Count + 1; i++)
            {
                obj.Cells[1, i] = g.Columns[i - 1].HeaderText;
            }
            for (int i = 0; i < g.Rows.Count; i++)
                for (int j = 0; j < g.Columns.Count; j++)
                {
                    if (g.Rows[i].Cells[j].Value != null)
                    {
                        obj.Cells[i + 2, j + 1] = g.Rows[i].Cells[j].Value;
                    }
                }
            obj.Range["A1", "M100"].Font.Name = "Times New Roman";
            obj.Range["A1", "M100"].HorizontalAlignment = 3;
            obj.ActiveWorkbook.SaveCopyAs(duongdan + tenfile + ".xlsx");
            obj.ActiveWorkbook.Saved = true;
            //obj.Quit();
        }


        public static void export_phieu(DataGridView g, string duongdan, string tenfile, string solg)
        {
            duongdan = @"C:\Demo\SupermarketManagement-main\excel";
            app obj = new app();
            obj.Application.Workbooks.Add(Type.Missing);
            obj.Columns.ColumnWidth = 25;


            for (int i = 1; i < g.Columns.Count + 1; i++)
            {
                obj.Cells[1, i] = g.Columns[i - 1].HeaderText;
            }
            for (int i = 0; i < g.Rows.Count; i++)
                for (int j = 0; j < g.Columns.Count; j++)
                {
                    if (g.Rows[i].Cells[j].Value != null)
                    {
                        obj.Cells[i + 2, j + 1] = g.Rows[i].Cells[j].Value;
                    }
                }
            obj.Cells[g.Rows.Count + 2, g.Columns.Count - 1] = "Số lượng : ";
            obj.Cells[g.Rows.Count + 2, g.Columns.Count] = " " + solg + "";

            obj.Range["A1", "M100"].Font.Name = "Times New Roman";
            obj.Range["A1", "M100"].HorizontalAlignment = 3;
            obj.ActiveWorkbook.SaveCopyAs(duongdan + tenfile + ".xlsx");
            obj.ActiveWorkbook.Saved = true;
            //obj.Quit();
        }

        public static void nhapnhieu(DataGridView g, string duongdan, string tenfile, string s, string tile, string chietkhau)
        {
            duongdan = @"C:\Demo\SupermarketManagement-main\excel";
            app obj = new app();
            obj.Application.Workbooks.Add(Type.Missing);
            obj.Columns.ColumnWidth = 25;



            for (int i = 1; i < g.Columns.Count + 1; i++)
            {
                obj.Cells[1, i] = g.Columns[i - 1].HeaderText;
            }
            for (int i = 0; i < g.Rows.Count; i++)
                for (int j = 0; j < g.Columns.Count; j++)
                {
                    if (g.Rows[i].Cells[j].Value != null)
                    {
                        obj.Cells[i + 2, j + 1] = g.Rows[i].Cells[j].Value;
                    }
                }
            obj.Cells[g.Rows.Count + 2, g.Columns.Count - 1] = "Chiếu khấu : ";
            obj.Cells[g.Rows.Count + 2, g.Columns.Count] = " " + chietkhau + " %";
            obj.Cells[g.Rows.Count + 3, g.Columns.Count - 1] = "Tổng Tiền : ";
            obj.Cells[g.Rows.Count + 3, g.Columns.Count] = " " + s;
            obj.Range["A1", "M100"].Font.Name = "Times New Roman";
            obj.Range["A1", "M100"].HorizontalAlignment = 3;
            obj.ActiveWorkbook.SaveCopyAs(duongdan + tenfile + ".xlsx");
            obj.ActiveWorkbook.Saved = true;
            //obj.Quit();
        }
        public static class XuatHoaDon
        {
            // dgv: dta2 (chi tiết hóa đơn)
            // basePath: đường dẫn + tên file KHÔNG có đuôi (vd: C:\...\HoaDon_1)
            public static void ExportToExcelAndPdf(
                DataGridView dgv,
                string basePath,
                string tenSieuThi,
                string diaChi,
                string maHD,
                DateTime ngayLap,
                string hinhThucTT,
                string tongTienText,
                string tienNhanText,
                string tienThoiText)
            {
                Excel.Application app = null;
                Excel.Workbook wb = null;
                Excel.Worksheet ws = null;

                try
                {
                    app = new Excel.Application();
                    app.Visible = false;

                    wb = app.Workbooks.Add();
                    ws = (Excel.Worksheet)wb.ActiveSheet;
                    ws.Name = "HoaDon";

                    int row = 1;

                    // ====== HEADER SIÊU THỊ ======
                    ws.Range["A" + row, "E" + row].Merge();
                    ws.Range["A" + row].Value = tenSieuThi.ToUpper();
                    ws.Range["A" + row].Font.Bold = true;
                    ws.Range["A" + row].Font.Size = 16;
                    row++;

                    ws.Range["A" + row, "E" + row].Merge();
                    ws.Range["A" + row].Value = diaChi;
                    ws.Range["A" + row].Font.Italic = true;
                    row += 2;

                    // ====== TIÊU ĐỀ HÓA ĐƠN ======
                    ws.Range["A" + row, "E" + row].Merge();
                    ws.Range["A" + row].Value = "HÓA ĐƠN BÁN HÀNG";
                    ws.Range["A" + row].Font.Bold = true;
                    ws.Range["A" + row].Font.Size = 14;
                    ws.Range["A" + row].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    row += 2;

                    // ====== THÔNG TIN CHUNG ======
                    ws.Cells[row, 1] = "Mã hóa đơn:";
                    ws.Cells[row, 2] = maHD;
                    row++;

                    ws.Cells[row, 1] = "Ngày lập:";
                    ws.Cells[row, 2] = ngayLap.ToString("dd/MM/yyyy HH:mm");
                    row++;

                    ws.Cells[row, 1] = "Hình thức thanh toán:";
                    ws.Cells[row, 2] = hinhThucTT;
                    row++;

                    ws.Cells[row, 1] = "Tiền khách đưa:";
                    ws.Cells[row, 2] = tienNhanText;
                    row++;

                    ws.Cells[row, 1] = "Tiền thối lại:";
                    ws.Cells[row, 2] = tienThoiText;
                    row += 2;

                    // ====== BẢNG SẢN PHẨM ======
                    int startTableRow = row;

                    ws.Cells[row, 1] = "Mã SP";
                    ws.Cells[row, 2] = "Tên sản phẩm";
                    ws.Cells[row, 3] = "Số lượng";
                    ws.Cells[row, 4] = "Đơn giá";
                    ws.Cells[row, 5] = "Thành tiền";

                    Excel.Range headerRange = ws.Range["A" + row, "E" + row];
                    headerRange.Font.Bold = true;
                    row++;

                    // ghi dữ liệu từ dgv (dta2)
                    for (int i = 0; i < dgv.Rows.Count - 1; i++) // bỏ dòng trắng cuối
                    {
                        ws.Cells[row, 1] = dgv.Rows[i].Cells[0].Value; // MaSP
                        ws.Cells[row, 2] = dgv.Rows[i].Cells[1].Value; // TenSP
                        ws.Cells[row, 3] = dgv.Rows[i].Cells[4].Value; // SoLuong
                        ws.Cells[row, 4] = dgv.Rows[i].Cells[3].Value; // DonGia
                        ws.Cells[row, 5] = dgv.Rows[i].Cells[5].Value; // ThanhTien
                        row++;
                    }

                    int endTableRow = row - 1;

                    // kẻ khung bảng
                    Excel.Range tableRange = ws.Range["A" + startTableRow, "E" + endTableRow];
                    tableRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                    // ====== TỔNG TIỀN ======
                    row += 1;
                    ws.Cells[row, 4] = "Tổng cộng:";
                    ws.Cells[row, 5] = tongTienText;
                    ws.Cells[row, 4].Font.Bold = true;
                    ws.Cells[row, 5].Font.Bold = true;

                    // căn chỉnh
                    ws.Columns["A:E"].AutoFit();

                    // ====== LƯU EXCEL + PDF ======
                    string excelPath = basePath + ".xlsx";
                    string pdfPath = basePath + ".pdf";

                    wb.SaveAs(excelPath);
                    wb.ExportAsFixedFormat(
                        Excel.XlFixedFormatType.xlTypePDF,
                        pdfPath,
                        Excel.XlFixedFormatQuality.xlQualityStandard,
                        true, true);

                    // mở luôn PDF
                    System.Diagnostics.Process.Start(pdfPath);
                }
                finally
                {
                    if (wb != null) wb.Close();
                    if (app != null) app.Quit();
                }
            }
        }
    }
}
