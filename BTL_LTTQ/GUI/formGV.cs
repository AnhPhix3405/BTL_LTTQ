// Giả sử file này nằm ở BTL_LTTQ/GUI/formGV.cs
// Nếu nó ở gốc, hãy xóa ".GUI" khỏi namespace
// và cập nhật using cho BLL và DTO
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using BTL_LTTQ.BLL; // Thêm BLL
using BTL_LTTQ.DTO; // Thêm DTO
using System.Data.SqlClient; // Cần giữ lại để bắt SqlException

// namespace BTL_LTTQ (Nếu file ở gốc)
namespace BTL_LTTQ // (Nếu file ở trong thư mục GUI)
{
    public partial class formGV : Form
    {
        // Lớp BLL
        private readonly GiangVien_BLL bll = new GiangVien_BLL();

        // Constants
        private const string placeholderTimKiem = "nhập tên, mã gv hoặc email";
        private const string placeholderNgaySinh = "dd/MM/yyyy";
        private const string dateFormat = "dd/MM/yyyy";

        public formGV()
        {
            InitializeComponent();
        }

        #region Form Load

        private void formGV_Load(object sender, EventArgs e)
        {
            LoadDataGridView();
            LoadComboBoxes();
            AttachEventHandlers();
            SetPlaceholderText();
            ClearFields();
        }

        private void AttachEventHandlers()
        {
            btnThem.Click += new EventHandler(btnThem_Click);
            btnSua.Click += new EventHandler(btnSua_Click);
            btnXoa.Click += new EventHandler(btnXoa_Click);
            btnRefresh.Click += new EventHandler(btnRefresh_Click);
            btnTimKiem.Click += new EventHandler(btnTimKiem_Click);
            btnTatCa.Click += new EventHandler(btnTatCa_Click);
            dgvSV.CellClick += new DataGridViewCellEventHandler(dgvSV_CellClick);
            tbTimKiemTheoTen.Enter += new EventHandler(tbTimKiemTheoTen_Enter);
            tbTimKiemTheoTen.Leave += new EventHandler(tbTimKiemTheoTen_Leave);
            tbNgaysinh.Enter += new EventHandler(tbNgaysinh_Enter);
            tbNgaysinh.Leave += new EventHandler(tbNgaysinh_Leave);
        }

        #endregion

        #region Phương thức Helper (Tải dữ liệu, Xóa trường)

        /// <summary>
        /// Tải dữ liệu lên Grid (gọi BLL)
        /// </summary>
        private void LoadDataGridView()
        {
            try
            {
                DataTable dt = bll.LoadDanhSachGV(); // Gọi BLL
                dgvSV.DataSource = dt;

                // Tùy chỉnh tên cột
                dgvSV.Columns["MaGV"].HeaderText = "Mã GV";
                dgvSV.Columns["TenGV"].HeaderText = "Họ Tên";
                dgvSV.Columns["GioiTinh"].HeaderText = "Giới Tính";
                dgvSV.Columns["NgaySinh"].HeaderText = "Ngày Sinh";
                dgvSV.Columns["DiaChi"].HeaderText = "Địa Chỉ";
                dgvSV.Columns["SDT"].HeaderText = "Số ĐT";
                dgvSV.Columns["Email"].HeaderText = "Email";
                dgvSV.Columns["HocHam"].HeaderText = "Học Hàm";
                dgvSV.Columns["HocVi"].HeaderText = "Học Vị";
                dgvSV.Columns["MaKhoa"].HeaderText = "Mã Khoa";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Tải ComboBox (gọi BLL)
        /// </summary>
        private void LoadComboBoxes()
        {
            try
            {
                DataTable dtKhoa = bll.LoadDanhSachKhoa(); // Gọi BLL

                // 1. ComboBox cho chi tiết (cbbMaKhoa)
                cbbMaKhoa.DataSource = dtKhoa;
                cbbMaKhoa.DisplayMember = "TenKhoa";
                cbbMaKhoa.ValueMember = "MaKhoa";
                cbbMaKhoa.SelectedIndex = -1;

                // 2. ComboBox cho tìm kiếm (cbbTimTheoKhoa)
                DataTable dtKhoaSearch = dtKhoa.Copy();
                DataRow tatCaRow = dtKhoaSearch.NewRow();
                tatCaRow["MaKhoa"] = "";
                tatCaRow["TenKhoa"] = "--- Chọn tất cả khoa ---";
                dtKhoaSearch.Rows.InsertAt(tatCaRow, 0);

                cbbTimTheoKhoa.DataSource = dtKhoaSearch;
                cbbTimTheoKhoa.DisplayMember = "TenKhoa";
                cbbTimTheoKhoa.ValueMember = "MaKhoa";
                cbbTimTheoKhoa.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách khoa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xóa trắng các trường nhập liệu
        /// </summary>
        private void ClearFields()
        {
            tbMaGV.Text = "";
            tbHoTen.Text = "";
            tbNgaysinh.Text = placeholderNgaySinh;
            tbNgaysinh.ForeColor = Color.Gray;
            tbDiaChi.Text = "";
            tbSoDt.Text = "";
            tbEmail.Text = "";
            tbHocHam.Text = "";
            tbHocVi.Text = "";
            cbbMaKhoa.SelectedIndex = -1;
            rdoNam.Checked = true;
            rdoNu.Checked = false;
            tbMaGV.ReadOnly = false;
            tbMaGV.Focus();
        }

        /// <summary>
        /// Helper: Lấy dữ liệu từ Form để tạo DTO
        /// </summary>
        private GiangVien_DTO GetGiangVienFromGUI()
        {
            GiangVien_DTO gv = new GiangVien_DTO();
            gv.MaGV = tbMaGV.Text.Trim();
            gv.TenGV = tbHoTen.Text.Trim();
            gv.GioiTinh = rdoNam.Checked ? "Nam" : (rdoNu.Checked ? "Nữ" : null);

            DateTime ngaySinh;
            if (DateTime.TryParseExact(tbNgaysinh.Text, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ngaySinh))
            {
                gv.NgaySinh = ngaySinh;
            }
            else
            {
                gv.NgaySinh = null;
            }

            gv.DiaChi = string.IsNullOrWhiteSpace(tbDiaChi.Text) ? null : tbDiaChi.Text.Trim();
            gv.SDT = string.IsNullOrWhiteSpace(tbSoDt.Text) ? null : tbSoDt.Text.Trim();
            gv.Email = string.IsNullOrWhiteSpace(tbEmail.Text) ? null : tbEmail.Text.Trim();
            gv.HocHam = string.IsNullOrWhiteSpace(tbHocHam.Text) ? null : tbHocHam.Text.Trim();
            gv.HocVi = string.IsNullOrWhiteSpace(tbHocVi.Text) ? null : tbHocVi.Text.Trim();
            gv.MaKhoa = cbbMaKhoa.SelectedValue?.ToString();

            return gv;
        }

        #endregion

        #region Sự kiện CRUD (Gọi BLL)

        private void btnThem_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra đầu vào
            if (string.IsNullOrWhiteSpace(tbMaGV.Text))
            {
                MessageBox.Show("Mã giảng viên không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbMaGV.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(tbHoTen.Text))
            {
                MessageBox.Show("Tên giảng viên không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbHoTen.Focus();
                return;
            }
            if (cbbMaKhoa.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn khoa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbMaKhoa.Focus();
                return;
            }
            // Kiểm tra ngày sinh nếu được nhập
            if (!string.IsNullOrWhiteSpace(tbNgaysinh.Text) && tbNgaysinh.Text != placeholderNgaySinh)
            {
                DateTime ngaySinh;
                if (!DateTime.TryParseExact(tbNgaysinh.Text, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ngaySinh))
                {
                    MessageBox.Show($"Ngày sinh không hợp lệ. Vui lòng nhập theo định dạng {placeholderNgaySinh}.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tbNgaysinh.Focus();
                    return;
                }
            }

            // 2. Lấy dữ liệu từ GUI
            GiangVien_DTO gv = GetGiangVienFromGUI();

            // 3. Gọi BLL
            try
            {
                if (bll.ThemGV(gv))
                {
                    MessageBox.Show("Thêm giảng viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataGridView();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Thêm giảng viên thất bại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (SqlException ex) // Bắt lỗi SQL cụ thể
            {
                if (ex.Number == 2627 || ex.Number == 2601) // Lỗi trùng khóa chính
                {
                    MessageBox.Show("Mã giảng viên này đã tồn tại. Vui lòng nhập mã khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Lỗi khi thêm: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi không xác định: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra đầu vào
            if (string.IsNullOrWhiteSpace(tbMaGV.Text) || tbMaGV.ReadOnly == false)
            {
                MessageBox.Show("Vui lòng chọn một giảng viên từ danh sách để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // (Thêm các kiểm tra khác nếu cần, tương tự như btnThem)

            // 2. Lấy dữ liệu từ GUI
            GiangVien_DTO gv = GetGiangVienFromGUI();

            // 3. Gọi BLL
            try
            {
                if (bll.SuaGV(gv))
                {
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataGridView();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy giảng viên để cập nhật hoặc dữ liệu không đổi.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra
            if (string.IsNullOrWhiteSpace(tbMaGV.Text) || tbMaGV.ReadOnly == false)
            {
                MessageBox.Show("Vui lòng chọn một giảng viên từ danh sách để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Xác nhận
            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa giảng viên '{tbHoTen.Text}' (Mã: {tbMaGV.Text}) không?" +
                $"\n\nCẢNH BÁO: Toàn bộ Tài khoản và lịch Phân công giảng dạy liên quan đến giảng viên này cũng sẽ bị xóa vĩnh viễn.",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                // 3. Gọi BLL
                try
                {
                    if (bll.XoaGV(tbMaGV.Text))
                    {
                        MessageBox.Show("Xóa giảng viên và các dữ liệu liên quan thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataGridView();
                        ClearFields();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy giảng viên để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi trong quá trình xóa: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ClearFields();
            LoadDataGridView(); // Tải lại toàn bộ dữ liệu gốc

            // Reset cả ô tìm kiếm
            tbTimKiemTheoTen.Text = placeholderTimKiem;
            tbTimKiemTheoTen.ForeColor = Color.Gray;
            cbbTimTheoKhoa.SelectedIndex = 0;
        }

        #endregion

        #region Sự kiện Tìm kiếm (Gọi BLL)

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = (tbTimKiemTheoTen.Text == placeholderTimKiem) ? "" : tbTimKiemTheoTen.Text;
            string maKhoa = cbbTimTheoKhoa.SelectedValue?.ToString();

            try
            {
                DataTable dt = bll.TimKiemGV(tuKhoa, maKhoa); // Gọi BLL
                dgvSV.DataSource = dt;

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy kết quả nào.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTatCa_Click(object sender, EventArgs e)
        {
            // Reset các trường tìm kiếm
            tbTimKiemTheoTen.Text = placeholderTimKiem;
            tbTimKiemTheoTen.ForeColor = Color.Gray;
            cbbTimTheoKhoa.SelectedIndex = 0;

            // Tải lại toàn bộ dữ liệu
            LoadDataGridView();
        }

        #endregion

        #region Sự kiện Giao diện (Không thay đổi)

        private void dgvSV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Đảm bảo click vào một hàng hợp lệ (không phải header)
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSV.Rows[e.RowIndex];

                // Lấy dữ liệu từ hàng và gán vào các control
                tbMaGV.Text = row.Cells["MaGV"].Value?.ToString();
                tbHoTen.Text = row.Cells["TenGV"].Value?.ToString();

                // Xử lý Giới tính
                string gioiTinh = row.Cells["GioiTinh"].Value?.ToString();
                if (gioiTinh == "Nữ")
                {
                    rdoNu.Checked = true;
                }
                else
                {
                    rdoNam.Checked = true; // Mặc định là Nam nếu không phải Nữ hoặc là NULL
                }

                // Xử lý ngày sinh
                if (row.Cells["NgaySinh"].Value != null && row.Cells["NgaySinh"].Value != DBNull.Value)
                {
                    // Chuyển sang format "dd/MM/yyyy"
                    tbNgaysinh.Text = Convert.ToDateTime(row.Cells["NgaySinh"].Value).ToString(dateFormat);
                    tbNgaysinh.ForeColor = Color.Black;
                }
                else
                {
                    tbNgaysinh.Text = placeholderNgaySinh;
                    tbNgaysinh.ForeColor = Color.Gray;
                }

                tbDiaChi.Text = row.Cells["DiaChi"].Value?.ToString();
                tbSoDt.Text = row.Cells["SDT"].Value?.ToString();
                tbEmail.Text = row.Cells["Email"].Value?.ToString();
                tbHocHam.Text = row.Cells["HocHam"].Value?.ToString();
                tbHocVi.Text = row.Cells["HocVi"].Value?.ToString();

                // Chọn ComboBox Khoa
                string maKhoa = row.Cells["MaKhoa"].Value?.ToString();
                if (!string.IsNullOrEmpty(maKhoa))
                {
                    cbbMaKhoa.SelectedValue = maKhoa;
                }
                else
                {
                    cbbMaKhoa.SelectedIndex = -1;
                }

                // Khóa không cho sửa Mã GV (Khóa chính)
                tbMaGV.ReadOnly = true;
            }
        }

        // --- Các phương thức xử lý Placeholder (văn bản mờ) ---
        private void SetPlaceholderText()
        {
            tbTimKiemTheoTen.Text = placeholderTimKiem;
            tbTimKiemTheoTen.ForeColor = Color.Gray;
            tbNgaysinh.Text = placeholderNgaySinh;
            tbNgaysinh.ForeColor = Color.Gray;
        }

        private void tbTimKiemTheoTen_Enter(object sender, EventArgs e)
        {
            if (tbTimKiemTheoTen.Text == placeholderTimKiem)
            {
                tbTimKiemTheoTen.Text = "";
                tbTimKiemTheoTen.ForeColor = Color.Black;
            }
        }

        private void tbTimKiemTheoTen_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbTimKiemTheoTen.Text))
            {
                tbTimKiemTheoTen.Text = placeholderTimKiem;
                tbTimKiemTheoTen.ForeColor = Color.Gray;
            }
        }

        private void tbNgaysinh_Enter(object sender, EventArgs e)
        {
            if (tbNgaysinh.Text == placeholderNgaySinh)
            {
                tbNgaysinh.Text = "";
                tbNgaysinh.ForeColor = Color.Black;
            }
        }

        private void tbNgaysinh_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbNgaysinh.Text))
            {
                tbNgaysinh.Text = placeholderNgaySinh;
                tbNgaysinh.ForeColor = Color.Gray;
            }
        }

        #endregion
    }
}