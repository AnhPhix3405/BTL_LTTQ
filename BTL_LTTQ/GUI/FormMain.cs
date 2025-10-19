using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTL_LTTQ
{
    public partial class FormMain : Form
    {
        private formMonHoc formMonHocInstance;
        private formGV formGVInstance;
        
        public FormMain()
        {
            InitializeComponent();
            LoadFormMonHoc();
            
            // Thêm sự kiện click cho panel Phân Công Giảng Viên
            panelPhanCongGiangVien.Click += PanelPhanCongGiangVien_Click;
            
            // Thêm sự kiện click cho panel Môn Học
            panelMonHoc.Click += PanelMonHoc_Click;
            
        }
        
        private void LoadFormMonHoc()
        {
            try
            {
                // Tạo instance formMonHoc
                formMonHocInstance = new formMonHoc();
                
                // Cấu hình formMonHoc như User Control
                formMonHocInstance.TopLevel = false;
                formMonHocInstance.FormBorderStyle = FormBorderStyle.None;
                formMonHocInstance.Dock = DockStyle.Fill;
                
                // Add vào panel (panelMain đã tạo trong Designer)
                panelMain.Controls.Clear();
                panelMain.Controls.Add(formMonHocInstance);
                formMonHocInstance.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void LoadFormGV()
        {
            try
            {
                // Tạo instance formGV
                formGVInstance = new formGV();
                
                // Cấu hình formGV như User Control
                formGVInstance.TopLevel = false;
                formGVInstance.FormBorderStyle = FormBorderStyle.None;
                formGVInstance.Dock = DockStyle.Fill;
                
                // Add vào panel (panelMain đã tạo trong Designer)
                panelMain.Controls.Clear();
                panelMain.Controls.Add(formGVInstance);
                formGVInstance.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void PanelPhanCongGiangVien_Click(object sender, EventArgs e)
        {
            LoadFormGV();
        }

        private void PanelMonHoc_Click(object sender, EventArgs e)
        {
            LoadFormMonHoc();
        }
    }
}