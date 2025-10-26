using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BTL_LTTQ.BLL.Diem; // Import BLL
using BTL_LTTQ.DTO;       // Import DTO

namespace BTL_LTTQ
{
    public partial class formDiem : Form
    {
        // Khai báo đối tượng BLL để giao tiếp với tầng nghiệp vụ
        private DiemBLL diemBLL = new DiemBLL();

        public formDiem()
        {
            InitializeComponent();
            LoadData(); // Tải dữ liệu khi form khởi tạo
            // Thêm sự kiện Click cho các nút (giả sử tên nút là btnThem, btnSua, etc.)
            btn_ThemDiem.Click += BtnThem_Click;
            btn_SuaDiem.Click += BtnSua_Click;
            btn_XoaDiem.Click += BtnXoa_Click;
            btn_LamMoiDiem.Click += BtnLamMoi_Click;
            btnTimKiem.Click += BtnTim_Click;
            btnTatCa.Click += BtnTatCa_Click;

            // Thêm sự kiện cho DataGridView khi chọn một hàng
            dgvDiem.CellClick += DataGridView1_CellClick;
        }

        // ----------------------------------------------------------------------
        // CÁC HÀM TIỆN ÍCH
        // ----------------------------------------------------------------------

        /// <summary>
        /// Tải dữ liệu điểm lên DataGridView
        /// </summary>
        private void LoadData()
        {
            try
            {
                DataTable dt = diemBLL.LayDanhSach();
                // Giả sử DataGridView của bạn có tên là dataGridView1
                dgvDiem.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Lấy dữ liệu từ các control trên Form và tạo đối tượng Score
        /// </summary>
        private Score GetScoreFromInputs()
        {
            double diemCC, diemGK, diemThi;

            // Cố gắng chuyển đổi chuỗi điểm thành số double, nếu không thành công thì đặt là 0
            Double.TryParse(txtDiemCC.Text, out diemCC);
            Double.TryParse(txtDiemGK.Text, out diemGK);
            Double.TryParse(txtDiemThi.Text, out diemThi);

            return new Score
            {
                MaLop = txtMaLop.Text.Trim(),
                MaSV = txtMaSV.Text.Trim(),
                DiemCc = diemCC,
                DiemGK = diemGK,
                DiemThi = diemThi
            };
        }

        /// <summary>
        /// Xóa trắng các trường nhập liệu
        /// </summary>
        private void ClearInputs()
        {
            txtMaLop.Clear();
            txtMaSV.Clear();
            txtDiemCC.Text = "0";
            txtDiemGK.Text = "0";
            txtDiemThi.Text = "0";
            txtMaLop.Focus();
        }

        // ----------------------------------------------------------------------
        // CÁC SỰ KIỆN CLICK NÚT
        // ----------------------------------------------------------------------

        private void BtnLamMoi_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            Score newScore = GetScoreFromInputs();
            string result = diemBLL.ThemDiem(newScore);

            MessageBox.Show(result);
            if (result.Contains("Thành công"))
            {
                LoadData();
                ClearInputs();
            }
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            Score scoreToUpdate = GetScoreFromInputs();
            string result = diemBLL.SuaDiem(scoreToUpdate);

            MessageBox.Show(result);
            if (result.Contains("Thành công"))
            {
                LoadData();
                ClearInputs();
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            // Cần Mã Lớp và Mã SV để xóa chính xác
            string maLop = txtMaLop.Text.Trim();
            string maSV = txtMaSV.Text.Trim();

            if (string.IsNullOrEmpty(maLop) || string.IsNullOrEmpty(maSV))
            {
                MessageBox.Show("Vui lòng chọn hoặc nhập Mã Lớp và Mã SV cần xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa điểm của SV {maSV} trong lớp {maLop}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string result = diemBLL.XoaDiem(maLop, maSV);
                MessageBox.Show(result);
                if (result.Contains("Thành công"))
                {
                    LoadData();
                    ClearInputs();
                }
            }
        }

        // **Lưu ý**: Để sử dụng hàm tìm kiếm này, bạn cần thêm hàm LayDanhSachTheoMaLop(string maLop) vào DiemDAL và DiemBLL
        private void BtnTim_Click(object sender, EventArgs e)
        {
            string maLop = txtTimKiemTheoMa.Text.Trim(); // Giả sử ô textbox tìm kiếm tên là txtTimKiemMaLop
            if (string.IsNullOrWhiteSpace(maLop))
            {
                MessageBox.Show("Vui lòng nhập Mã lớp để tìm kiếm.", "Cảnh báo");
                return;
            }

            // Bạn cần tự định nghĩa hàm này trong BLL và DAL
            // DataTable dt = diemBLL.LayDanhSachTheoMaLop(maLop); 
            // dataGridView1.DataSource = dt;

            MessageBox.Show("Chức năng tìm kiếm chưa được triển khai đầy đủ. Vui lòng thêm hàm vào BLL/DAL.");
        }

        private void BtnTatCa_Click(object sender, EventArgs e)
        {
            LoadData(); // Load lại toàn bộ dữ liệu
        }

        // ----------------------------------------------------------------------
        // SỰ KIỆN CHỌN HÀNG TRONG DATAGRIDVIEW
        // ----------------------------------------------------------------------

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDiem.Rows[e.RowIndex];

                // Đổ dữ liệu từ hàng được chọn vào các TextBox
                txtMaLop.Text = row.Cells["MaLop"].Value.ToString();
                txtMaSV.Text = row.Cells["MaSV"].Value.ToString();
                txtDiemCC.Text = row.Cells["DiemChuyenCan"].Value.ToString();
                txtDiemGK.Text = row.Cells["DiemGiuaKy"].Value.ToString();
                txtDiemThi.Text = row.Cells["DiemThi"].Value.ToString();
            }
        }

        private void txtDiemThi_TextChanged(object sender, EventArgs e)
        {

        }
    }
}