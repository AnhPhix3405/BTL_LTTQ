// formSV.Designer.cs
using System.Drawing;
using System.Windows.Forms;

namespace BTL_LTTQ
{
    partial class formSV
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.dgvSV = new System.Windows.Forms.DataGridView();
            this.cmbTimTheoKhoa = new System.Windows.Forms.ComboBox();
            this.cmbTimTheoLop = new System.Windows.Forms.ComboBox();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.tbMaSV = new System.Windows.Forms.TextBox();
            this.tbHoTen = new System.Windows.Forms.TextBox();
            this.dtpNgaySinh = new System.Windows.Forms.DateTimePicker();
            this.rdoNam = new System.Windows.Forms.RadioButton();
            this.rdoNu = new System.Windows.Forms.RadioButton();
            this.tbDiaChi = new System.Windows.Forms.TextBox();
            this.tbSoDt = new System.Windows.Forms.TextBox();
            this.tbEmail = new System.Windows.Forms.TextBox();
            this.cmbKhoa = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnTatCa = new System.Windows.Forms.Button();
            this.btnTimKiem = new System.Windows.Forms.Button();
            this.tbTimKiemTheoMa = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cmbMaLopTC = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSV)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvSV
            // 
            this.dgvSV.AllowUserToAddRows = false;
            this.dgvSV.AllowUserToDeleteRows = false;
            this.dgvSV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSV.Location = new System.Drawing.Point(13, 392);
            this.dgvSV.Name = "dgvSV";
            this.dgvSV.ReadOnly = true;
            this.dgvSV.RowHeadersWidth = 51;
            this.dgvSV.Size = new System.Drawing.Size(1437, 287);
            this.dgvSV.TabIndex = 11;
            // 
            // cmbTimTheoKhoa
            // 
            this.cmbTimTheoKhoa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTimTheoKhoa.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.cmbTimTheoKhoa.FormattingEnabled = true;
            this.cmbTimTheoKhoa.Location = new System.Drawing.Point(87, 25);
            this.cmbTimTheoKhoa.Name = "cmbTimTheoKhoa";
            this.cmbTimTheoKhoa.Size = new System.Drawing.Size(297, 28);
            this.cmbTimTheoKhoa.TabIndex = 4;
            // 
            // cmbTimTheoLop
            // 
            this.cmbTimTheoLop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTimTheoLop.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.cmbTimTheoLop.FormattingEnabled = true;
            this.cmbTimTheoLop.Location = new System.Drawing.Point(468, 25);
            this.cmbTimTheoLop.Name = "cmbTimTheoLop";
            this.cmbTimTheoLop.Size = new System.Drawing.Size(297, 28);
            this.cmbTimTheoLop.TabIndex = 7;
            // 
            // btnThem
            // 
            this.btnThem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnThem.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnThem.Location = new System.Drawing.Point(1174, 145);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(110, 55);
            this.btnThem.TabIndex = 57;
            this.btnThem.Text = "Thêm";
            this.btnThem.UseVisualStyleBackColor = false;
            // 
            // btnSua
            // 
            this.btnSua.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnSua.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSua.Location = new System.Drawing.Point(871, 145);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(110, 55);
            this.btnSua.TabIndex = 59;
            this.btnSua.Text = "Sửa";
            this.btnSua.UseVisualStyleBackColor = false;
            // 
            // btnXoa
            // 
            this.btnXoa.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnXoa.Location = new System.Drawing.Point(1312, 145);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(110, 55);
            this.btnXoa.TabIndex = 58;
            this.btnXoa.Text = "Xóa";
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnLamMoi.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnLamMoi.Location = new System.Drawing.Point(1031, 145);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(110, 55);
            this.btnLamMoi.TabIndex = 56;
            this.btnLamMoi.Text = "Làm Mới";
            this.btnLamMoi.UseVisualStyleBackColor = false;
            // 
            // tbMaSV
            // 
            this.tbMaSV.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tbMaSV.Location = new System.Drawing.Point(114, 23);
            this.tbMaSV.Name = "tbMaSV";
            this.tbMaSV.Size = new System.Drawing.Size(283, 26);
            this.tbMaSV.TabIndex = 3;
            // 
            // tbHoTen
            // 
            this.tbHoTen.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tbHoTen.Location = new System.Drawing.Point(114, 75);
            this.tbHoTen.Name = "tbHoTen";
            this.tbHoTen.Size = new System.Drawing.Size(283, 26);
            this.tbHoTen.TabIndex = 9;
            // 
            // dtpNgaySinh
            // 
            this.dtpNgaySinh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F);
            this.dtpNgaySinh.Location = new System.Drawing.Point(114, 125);
            this.dtpNgaySinh.Name = "dtpNgaySinh";
            this.dtpNgaySinh.Size = new System.Drawing.Size(283, 27);
            this.dtpNgaySinh.TabIndex = 64;
            // 
            // rdoNam
            // 
            this.rdoNam.AutoSize = true;
            this.rdoNam.Location = new System.Drawing.Point(4, 20);
            this.rdoNam.Name = "rdoNam";
            this.rdoNam.Size = new System.Drawing.Size(57, 20);
            this.rdoNam.TabIndex = 12;
            this.rdoNam.Text = "Nam";
            this.rdoNam.UseVisualStyleBackColor = true;
            // 
            // rdoNu
            // 
            this.rdoNu.AutoSize = true;
            this.rdoNu.Location = new System.Drawing.Point(75, 20);
            this.rdoNu.Name = "rdoNu";
            this.rdoNu.Size = new System.Drawing.Size(45, 20);
            this.rdoNu.TabIndex = 13;
            this.rdoNu.Text = "Nữ";
            this.rdoNu.UseVisualStyleBackColor = true;
            // 
            // tbDiaChi
            // 
            this.tbDiaChi.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tbDiaChi.Location = new System.Drawing.Point(545, 22);
            this.tbDiaChi.Name = "tbDiaChi";
            this.tbDiaChi.Size = new System.Drawing.Size(263, 26);
            this.tbDiaChi.TabIndex = 5;
            // 
            // tbSoDt
            // 
            this.tbSoDt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tbSoDt.Location = new System.Drawing.Point(545, 74);
            this.tbSoDt.Name = "tbSoDt";
            this.tbSoDt.Size = new System.Drawing.Size(263, 26);
            this.tbSoDt.TabIndex = 13;
            // 
            // tbEmail
            // 
            this.tbEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tbEmail.Location = new System.Drawing.Point(545, 128);
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.Size = new System.Drawing.Size(263, 26);
            this.tbEmail.TabIndex = 15;
            // 
            // cmbKhoa
            // 
            this.cmbKhoa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKhoa.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.cmbKhoa.FormattingEnabled = true;
            this.cmbKhoa.Location = new System.Drawing.Point(1013, 21);
            this.cmbKhoa.Name = "cmbKhoa";
            this.cmbKhoa.Size = new System.Drawing.Size(271, 28);
            this.cmbKhoa.TabIndex = 61;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnTatCa);
            this.panel1.Controls.Add(this.btnTimKiem);
            this.panel1.Controls.Add(this.tbTimKiemTheoMa);
            this.panel1.Controls.Add(this.cmbTimTheoLop);
            this.panel1.Controls.Add(this.cmbTimTheoKhoa);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(13, 59);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1437, 71);
            this.panel1.TabIndex = 8;
            // 
            // btnTatCa
            // 
            this.btnTatCa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnTatCa.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnTatCa.ForeColor = System.Drawing.SystemColors.Info;
            this.btnTatCa.Location = new System.Drawing.Point(1322, 19);
            this.btnTatCa.Name = "btnTatCa";
            this.btnTatCa.Size = new System.Drawing.Size(100, 43);
            this.btnTatCa.TabIndex = 10;
            this.btnTatCa.Text = "Tất Cả";
            this.btnTatCa.UseVisualStyleBackColor = false;
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.BackColor = System.Drawing.Color.DarkCyan;
            this.btnTimKiem.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnTimKiem.ForeColor = System.Drawing.SystemColors.InactiveBorder;
            this.btnTimKiem.Location = new System.Drawing.Point(1194, 19);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(100, 43);
            this.btnTimKiem.TabIndex = 9;
            this.btnTimKiem.Text = "Tìm";
            this.btnTimKiem.UseVisualStyleBackColor = false;
            // 
            // tbTimKiemTheoMa
            // 
            this.tbTimKiemTheoMa.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tbTimKiemTheoMa.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tbTimKiemTheoMa.Location = new System.Drawing.Point(830, 27);
            this.tbTimKiemTheoMa.Name = "tbTimKiemTheoMa";
            this.tbTimKiemTheoMa.Size = new System.Drawing.Size(338, 26);
            this.tbTimKiemTheoMa.TabIndex = 8;
            this.tbTimKiemTheoMa.Text = "Nhập mã sinh viên...";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label4.Location = new System.Drawing.Point(410, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 20);
            this.label4.TabIndex = 2;
            this.label4.Text = "Lớp:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label3.Location = new System.Drawing.Point(19, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 20);
            this.label3.TabIndex = 1;
            this.label3.Text = "Khoa:";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.cmbMaLopTC);
            this.panel2.Controls.Add(this.label17);
            this.panel2.Controls.Add(this.cmbKhoa);
            this.panel2.Controls.Add(this.label14);
            this.panel2.Controls.Add(this.btnSua);
            this.panel2.Controls.Add(this.btnXoa);
            this.panel2.Controls.Add(this.btnThem);
            this.panel2.Controls.Add(this.btnLamMoi);
            this.panel2.Controls.Add(this.tbEmail);
            this.panel2.Controls.Add(this.label13);
            this.panel2.Controls.Add(this.tbSoDt);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.tbHoTen);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.dtpNgaySinh);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.tbDiaChi);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.tbMaSV);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Location = new System.Drawing.Point(13, 155);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1437, 227);
            this.panel2.TabIndex = 10;
            // 
            // cmbMaLopTC
            // 
            this.cmbMaLopTC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMaLopTC.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.cmbMaLopTC.FormattingEnabled = true;
            this.cmbMaLopTC.Location = new System.Drawing.Point(1013, 73);
            this.cmbMaLopTC.Name = "cmbMaLopTC";
            this.cmbMaLopTC.Size = new System.Drawing.Size(271, 28);
            this.cmbMaLopTC.TabIndex = 25;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label17.Location = new System.Drawing.Point(867, 76);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(92, 20);
            this.label17.TabIndex = 24;
            this.label17.Text = "Lớp tín chỉ:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label14.Location = new System.Drawing.Point(867, 26);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(52, 20);
            this.label14.TabIndex = 60;
            this.label14.Text = "Khoa:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label13.Location = new System.Drawing.Point(464, 128);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(56, 20);
            this.label13.TabIndex = 16;
            this.label13.Text = "Email:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label11.Location = new System.Drawing.Point(464, 76);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 20);
            this.label11.TabIndex = 12;
            this.label11.Text = "Số đt:";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.rdoNu);
            this.panel3.Controls.Add(this.rdoNam);
            this.panel3.Location = new System.Drawing.Point(110, 160);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(256, 57);
            this.panel3.TabIndex = 11;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label10.Location = new System.Drawing.Point(19, 180);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(76, 20);
            this.label10.TabIndex = 10;
            this.label10.Text = "Giới tính:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label9.Location = new System.Drawing.Point(19, 78);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 20);
            this.label9.TabIndex = 8;
            this.label9.Text = "Họ tên:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label8.Location = new System.Drawing.Point(19, 130);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 20);
            this.label8.TabIndex = 6;
            this.label8.Text = "Ngày sinh:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label7.Location = new System.Drawing.Point(464, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 20);
            this.label7.TabIndex = 4;
            this.label7.Text = "Địa chỉ:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label5.Location = new System.Drawing.Point(19, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 20);
            this.label5.TabIndex = 2;
            this.label5.Text = "Mã:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label1.Location = new System.Drawing.Point(318, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(690, 46);
            this.label1.TabIndex = 6;
            this.label1.Text = "Quản lý Sinh Viên Theo Lớp Tín Chỉ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.label2.Location = new System.Drawing.Point(32, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 26);
            this.label2.TabIndex = 12;
            this.label2.Text = "Tìm kiếm";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.label6.Location = new System.Drawing.Point(32, 139);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(170, 26);
            this.label6.TabIndex = 9;
            this.label6.Text = "Thông tin chi tiết";
            // 
            // formSV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1469, 688);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dgvSV);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "formSV";
            this.Text = "formSV";
            this.Load += new System.EventHandler(this.formSV_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSV)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSV;
        private System.Windows.Forms.ComboBox cmbTimTheoKhoa;
        private System.Windows.Forms.ComboBox cmbTimTheoLop;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.TextBox tbMaSV;
        private System.Windows.Forms.TextBox tbHoTen;
        private System.Windows.Forms.DateTimePicker dtpNgaySinh;
        private System.Windows.Forms.RadioButton rdoNam;
        private System.Windows.Forms.RadioButton rdoNu;
        private System.Windows.Forms.TextBox tbDiaChi;
        private System.Windows.Forms.TextBox tbSoDt;
        private System.Windows.Forms.TextBox tbEmail;
        private System.Windows.Forms.ComboBox cmbKhoa;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cmbMaLopTC;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button btnTimKiem;
        private System.Windows.Forms.TextBox tbTimKiemTheoMa;
        private System.Windows.Forms.Button btnTatCa;
    }
}