namespace QuanLySieuThi.quanly
{
    partial class sanpham
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl_kq = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_in = new System.Windows.Forms.Button();
            this.btn_them = new System.Windows.Forms.Button();
            this.bnt_sua = new System.Windows.Forms.Button();
            this.btn_Thoat = new System.Windows.Forms.Button();
            this.btn_xoa = new System.Windows.Forms.Button();
            this.btn_reset = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.dta1 = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txt_soluongtoithieu = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_loaisp = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_tensp = new System.Windows.Forms.TextBox();
            this.txt_solg = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txt_dongia = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_hsd = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_masp = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dta1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_kq
            // 
            this.lbl_kq.AutoSize = true;
            this.lbl_kq.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_kq.Location = new System.Drawing.Point(1291, 487);
            this.lbl_kq.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_kq.Name = "lbl_kq";
            this.lbl_kq.Size = new System.Drawing.Size(130, 22);
            this.lbl_kq.TabIndex = 112;
            this.lbl_kq.Text = "........................";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_in);
            this.groupBox2.Controls.Add(this.btn_them);
            this.groupBox2.Controls.Add(this.bnt_sua);
            this.groupBox2.Controls.Add(this.btn_Thoat);
            this.groupBox2.Controls.Add(this.btn_xoa);
            this.groupBox2.Controls.Add(this.btn_reset);
            this.groupBox2.Font = new System.Drawing.Font("Tahoma", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(937, 106);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(540, 359);
            this.groupBox2.TabIndex = 110;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Thực Hiện Công việc";
            // 
            // btn_in
            // 
            this.btn_in.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_in.Image = global::QuanLySieuThi.Properties.Resources.Ziggy19_Microsoft_Office_Mac_Tilt_Excel;
            this.btn_in.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btn_in.Location = new System.Drawing.Point(287, 64);
            this.btn_in.Margin = new System.Windows.Forms.Padding(4);
            this.btn_in.Name = "btn_in";
            this.btn_in.Size = new System.Drawing.Size(148, 52);
            this.btn_in.TabIndex = 5;
            this.btn_in.Text = "In";
            this.btn_in.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_in.UseVisualStyleBackColor = true;
            this.btn_in.Click += new System.EventHandler(this.btn_in_Click);
            // 
            // btn_them
            // 
            this.btn_them.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_them.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_them.Image = global::QuanLySieuThi.Properties.Resources.Awicons_Vista_Artistic_Add__1_;
            this.btn_them.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btn_them.Location = new System.Drawing.Point(61, 64);
            this.btn_them.Margin = new System.Windows.Forms.Padding(4);
            this.btn_them.Name = "btn_them";
            this.btn_them.Size = new System.Drawing.Size(148, 54);
            this.btn_them.TabIndex = 0;
            this.btn_them.Text = "Thêm";
            this.btn_them.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_them.UseVisualStyleBackColor = true;
            this.btn_them.Click += new System.EventHandler(this.btn_them_Click);
            // 
            // bnt_sua
            // 
            this.bnt_sua.Enabled = false;
            this.bnt_sua.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bnt_sua.Image = global::QuanLySieuThi.Properties.Resources.Oxygen_Icons_org_Oxygen_Actions_document_edit;
            this.bnt_sua.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.bnt_sua.Location = new System.Drawing.Point(61, 142);
            this.bnt_sua.Margin = new System.Windows.Forms.Padding(4);
            this.bnt_sua.Name = "bnt_sua";
            this.bnt_sua.Size = new System.Drawing.Size(148, 54);
            this.bnt_sua.TabIndex = 1;
            this.bnt_sua.Text = "Sửa";
            this.bnt_sua.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bnt_sua.UseVisualStyleBackColor = true;
            this.bnt_sua.Click += new System.EventHandler(this.bnt_sua_Click);
            // 
            // btn_Thoat
            // 
            this.btn_Thoat.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Thoat.Image = global::QuanLySieuThi.Properties.Resources.Hopstarter_Sleek_Xp_Software_Windows_Close_Program;
            this.btn_Thoat.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btn_Thoat.Location = new System.Drawing.Point(287, 229);
            this.btn_Thoat.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Thoat.Name = "btn_Thoat";
            this.btn_Thoat.Size = new System.Drawing.Size(148, 54);
            this.btn_Thoat.TabIndex = 6;
            this.btn_Thoat.Text = "Thoát";
            this.btn_Thoat.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_Thoat.UseVisualStyleBackColor = true;
            this.btn_Thoat.Click += new System.EventHandler(this.btn_Thoat_Click);
            // 
            // btn_xoa
            // 
            this.btn_xoa.Enabled = false;
            this.btn_xoa.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_xoa.Image = global::QuanLySieuThi.Properties.Resources.Custom_Icon_Design_Pretty_Office_8_Trash_can;
            this.btn_xoa.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btn_xoa.Location = new System.Drawing.Point(61, 229);
            this.btn_xoa.Margin = new System.Windows.Forms.Padding(4);
            this.btn_xoa.Name = "btn_xoa";
            this.btn_xoa.Size = new System.Drawing.Size(148, 54);
            this.btn_xoa.TabIndex = 2;
            this.btn_xoa.Text = "Xóa";
            this.btn_xoa.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_xoa.UseVisualStyleBackColor = true;
            this.btn_xoa.Click += new System.EventHandler(this.btn_xoa_Click);
            // 
            // btn_reset
            // 
            this.btn_reset.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_reset.Image = global::QuanLySieuThi.Properties.Resources.Oxygen_Icons_org_Oxygen_Actions_document_revert;
            this.btn_reset.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btn_reset.Location = new System.Drawing.Point(287, 142);
            this.btn_reset.Margin = new System.Windows.Forms.Padding(4);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(148, 54);
            this.btn_reset.TabIndex = 4;
            this.btn_reset.Text = "Nhập lại";
            this.btn_reset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_reset.UseVisualStyleBackColor = true;
            this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(1076, 487);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(207, 22);
            this.label15.TabIndex = 111;
            this.label15.Text = "Số lượng loại sản phẩm: ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.label5.Location = new System.Drawing.Point(588, 40);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(422, 48);
            this.label5.TabIndex = 113;
            this.label5.Text = "Thông tin sản phẩm";
            // 
            // txt_search
            // 
            this.txt_search.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_search.Location = new System.Drawing.Point(231, 478);
            this.txt_search.Margin = new System.Windows.Forms.Padding(4);
            this.txt_search.Multiline = true;
            this.txt_search.Name = "txt_search";
            this.txt_search.Size = new System.Drawing.Size(193, 34);
            this.txt_search.TabIndex = 108;
            this.txt_search.TextChanged += new System.EventHandler(this.txt_search_TextChanged);
            // 
            // dta1
            // 
            this.dta1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dta1.Location = new System.Drawing.Point(181, 520);
            this.dta1.Margin = new System.Windows.Forms.Padding(4);
            this.dta1.Name = "dta1";
            this.dta1.RowHeadersWidth = 51;
            this.dta1.Size = new System.Drawing.Size(1066, 276);
            this.dta1.TabIndex = 106;
            this.dta1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dta1_CellContentClick);
            this.dta1.Click += new System.EventHandler(this.dta1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txt_soluongtoithieu);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txt_loaisp);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txt_tensp);
            this.groupBox1.Controls.Add(this.txt_solg);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txt_dongia);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txt_hsd);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txt_masp);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(13, 109);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(894, 359);
            this.groupBox1.TabIndex = 105;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông tin sản phẩm";
            // 
            // txt_soluongtoithieu
            // 
            this.txt_soluongtoithieu.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_soluongtoithieu.Location = new System.Drawing.Point(216, 220);
            this.txt_soluongtoithieu.Margin = new System.Windows.Forms.Padding(4);
            this.txt_soluongtoithieu.Name = "txt_soluongtoithieu";
            this.txt_soluongtoithieu.Size = new System.Drawing.Size(90, 30);
            this.txt_soluongtoithieu.TabIndex = 45;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(31, 226);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(168, 24);
            this.label4.TabIndex = 44;
            this.label4.Text = "Số lượng tối thiểu";
            // 
            // txt_loaisp
            // 
            this.txt_loaisp.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_loaisp.Location = new System.Drawing.Point(211, 169);
            this.txt_loaisp.Margin = new System.Windows.Forms.Padding(4);
            this.txt_loaisp.Name = "txt_loaisp";
            this.txt_loaisp.Size = new System.Drawing.Size(195, 30);
            this.txt_loaisp.TabIndex = 43;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(31, 169);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(146, 24);
            this.label3.TabIndex = 42;
            this.label3.Text = "Loại sản phẩm ";
            // 
            // txt_tensp
            // 
            this.txt_tensp.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_tensp.Location = new System.Drawing.Point(211, 103);
            this.txt_tensp.Margin = new System.Windows.Forms.Padding(4);
            this.txt_tensp.Name = "txt_tensp";
            this.txt_tensp.Size = new System.Drawing.Size(195, 30);
            this.txt_tensp.TabIndex = 41;
            // 
            // txt_solg
            // 
            this.txt_solg.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_solg.Location = new System.Drawing.Point(644, 39);
            this.txt_solg.Margin = new System.Windows.Forms.Padding(4);
            this.txt_solg.Name = "txt_solg";
            this.txt_solg.Size = new System.Drawing.Size(104, 30);
            this.txt_solg.TabIndex = 5;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(496, 169);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(79, 24);
            this.label10.TabIndex = 40;
            this.label10.Text = "Đơn giá";
            // 
            // txt_dongia
            // 
            this.txt_dongia.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_dongia.Location = new System.Drawing.Point(644, 169);
            this.txt_dongia.Margin = new System.Windows.Forms.Padding(4);
            this.txt_dongia.Name = "txt_dongia";
            this.txt_dongia.Size = new System.Drawing.Size(204, 30);
            this.txt_dongia.TabIndex = 8;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(496, 112);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(123, 24);
            this.label9.TabIndex = 38;
            this.label9.Text = "Hạn sử dụng";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(532, 48);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 24);
            this.label8.TabIndex = 35;
            this.label8.Text = "Số lượng";
            // 
            // txt_hsd
            // 
            this.txt_hsd.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_hsd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.txt_hsd.Location = new System.Drawing.Point(644, 103);
            this.txt_hsd.Margin = new System.Windows.Forms.Padding(4);
            this.txt_hsd.Name = "txt_hsd";
            this.txt_hsd.Size = new System.Drawing.Size(204, 30);
            this.txt_hsd.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(31, 48);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 24);
            this.label1.TabIndex = 12;
            this.label1.Text = "Mã sản phẩm";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(31, 112);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 24);
            this.label2.TabIndex = 13;
            this.label2.Text = "Tên sản phẩm";
            // 
            // txt_masp
            // 
            this.txt_masp.Enabled = false;
            this.txt_masp.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_masp.Location = new System.Drawing.Point(211, 39);
            this.txt_masp.Margin = new System.Windows.Forms.Padding(4);
            this.txt_masp.Name = "txt_masp";
            this.txt_masp.Size = new System.Drawing.Size(195, 30);
            this.txt_masp.TabIndex = 0;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(32, 494);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(191, 22);
            this.label11.TabIndex = 109;
            this.label11.Text = "Tìm kiếm tên sản phẩm";
            // 
            // sanpham
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1505, 805);
            this.Controls.Add(this.lbl_kq);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txt_search);
            this.Controls.Add(this.dta1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label11);
            this.Name = "sanpham";
            this.Text = "sanpham";
            this.Load += new System.EventHandler(this.sanpham_Load);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dta1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_kq;
        private System.Windows.Forms.GroupBox groupBox2;
        internal System.Windows.Forms.Button btn_in;
        private System.Windows.Forms.Button btn_them;
        private System.Windows.Forms.Button bnt_sua;
        private System.Windows.Forms.Button btn_Thoat;
        private System.Windows.Forms.Button btn_xoa;
        private System.Windows.Forms.Button btn_reset;
        private System.Windows.Forms.Label label15;
        internal System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.DataGridView dta1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txt_tensp;
        private System.Windows.Forms.TextBox txt_solg;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txt_dongia;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker txt_hsd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_masp;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txt_loaisp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_soluongtoithieu;
        private System.Windows.Forms.Label label4;
    }
}