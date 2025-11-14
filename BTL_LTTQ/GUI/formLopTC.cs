// formLopTC.cs (đã được refactor)
using BTL_LTTQ.BLL;
using BTL_LTTQ.DAL;
using BTL_LTTQ.DTO;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BTL_LTTQ
{
    public partial class formLopTC : Form
    {
        // Sửa tên class BLL
        private readonly LopTC_BLL bll = new LopTC_BLL();

        private const string placeholderMaLop = "nhập mã lớp";

        public formLopTC()
        {
            InitializeComponent();
        }

        #region Form Load

        private void formLopTC_Load(object sender, EventArgs e)
        {
            LoadDataGridView();
            LoadComboBoxes();
            AttachEventHandlers();
            ClearFields();
            SetPlaceholderText();
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
            cbbMaKhoa.SelectedIndexChanged += new EventHandler(cbbMaKhoa_SelectedIndexChanged);
        }

        #endregion

        #region Phương thức Helper (Tải dữ liệu, Xóa trường)

        private void LoadDataGridView()
        {
            try
            {
                DataTable dt = bll.LoadDanhSachLTC(); // Gọi BLL
                dgvSV.DataSource = dt;

                dgvSV.Columns["MaLop"].HeaderText = "Mã Lớp";
                dgvSV.Columns["MaMH"].HeaderText = "Mã Môn Học";
                dgvSV.Columns["TenMH"].HeaderText = "Tên Môn Học";
                dgvSV.Columns["MaKhoa"].HeaderText = "Mã Khoa";
                dgvSV.Columns["HocKy"].HeaderText = "Học Kỳ";
                dgvSV.Columns["NamHoc"].HeaderText = "Năm Học";
                dgvSV.Columns["TinhTrang"].HeaderText = "Tình Trạng";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadComboBoxes()
        {
            try
            {
                DataTable dtKhoa = bll.LoadDanhSachKhoa(); // Gọi BLL

                cbbMaKhoa.DataSource = dtKhoa;
                cbbMaKhoa.DisplayMember = "TenKhoa";
                cbbMaKhoa.ValueMember = "MaKhoa";
                cbbMaKhoa.SelectedIndex = -1;

                DataTable dtKhoaSearch = dtKhoa.Copy();
                DataRow tatCaRow = dtKhoaSearch.NewRow();
                tatCaRow["MaKhoa"] = "";
                tatCaRow["TenKhoa"] = "--- Tất cả khoa ---";
                dtKhoaSearch.Rows.InsertAt(tatCaRow, 0);

                cbbTimTheoKhoa.DataSource = dtKhoaSearch;
                cbbTimTheoKhoa.DisplayMember = "TenKhoa";
                cbbTimTheoKhoa.ValueMember = "MaKhoa";
                cbbTimTheoKhoa.SelectedIndex = 0;

                LoadMonHocComboBox(null);

                // QUAN TRỌNG: Đảm bảo bạn có control 'cbbTinhTrang' trong form designer
                if (this.Controls.Find("cbbTinhTrang", true).FirstOrDefault() is ComboBox cbbTinhTrangControl)
                {
                    cbbTinhTrangControl.Items.Clear();
                    cbbTinhTrangControl.Items.Add("Chưa phân công");
                    cbbTinhTrangControl.Items.Add("Đã phân công");
                    cbbTinhTrangControl.SelectedIndex = 0;
                }
                else
                {
                    // Nếu không tìm thấy control, báo lỗi để bạn biết
                    MessageBox.Show("Không tìm thấy control 'cbbTinhTrang' trên form designer.", "Lỗi Giao Diện", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách Khoa/Môn học: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMonHocComboBox(string maKhoa)
        {
            try
            {
                DataTable dtMonHoc = bll.LoadDanhSachMonHoc(maKhoa); // Gọi BLL
                cbbMaMon.DataSource = dtMonHoc;
                cbbMaMon.DisplayMember = "TenMH";
                cbbMaMon.ValueMember = "MaMH";
                cbbMaMon.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách Môn học: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            tbMaLop.Text = "";
            numericUpDownHocky.Value = 1;
            tbNamHoc.Text = "";
            cbbMaKhoa.SelectedIndex = -1;
            cbbMaMon.SelectedIndex = -1;

            // Tìm control 'cbbTinhTrang'
            if (this.Controls.Find("cbbTinhTrang", true).FirstOrDefault() is ComboBox cbbTinhTrangControl)
            {
                cbbTinhTrangControl.SelectedItem = "Chưa phân công";
            }

            tbMaLop.ReadOnly = false;
            tbMaLop.Focus();
        }

        private LopTC_DTO GetLopTinChiFromGUI() // Dùng LopTC_DTO
        {
            LopTC_DTO ltc = new LopTC_DTO(); // Dùng LopTC_DTO
            ltc.MaLop = tbMaLop.Text.Trim();
            ltc.MaMH = cbbMaMon.SelectedValue?.ToString();
            ltc.HocKy = (int)numericUpDownHocky.Value;

            int namHoc;
            if (int.TryParse(tbNamHoc.Text, out namHoc))
            {
                ltc.NamHoc = namHoc;
            }
            else
            {
                ltc.NamHoc = null;
            }

            // Tìm control 'cbbTinhTrang'
            if (this.Controls.Find("cbbTinhTrang", true).FirstOrDefault() is ComboBox cbbTinhTrangControl)
            {
                ltc.TinhTrang = cbbTinhTrangControl.SelectedItem?.ToString();
            }
            return ltc;
        }

        #endregion

        #region Sự kiện CRUD (Gọi BLL)

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbMaLop.Text))
            {
                MessageBox.Show("Mã lớp không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbMaLop.Focus();
                return;
            }
            if (cbbMaMon.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn môn học.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbMaMon.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(tbNamHoc.Text))
            {
                MessageBox.Show("Năm học không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbNamHoc.Focus();
                return;
            }
            int namHoc;
            if (!int.TryParse(tbNamHoc.Text, out namHoc))
            {
                MessageBox.Show("Năm học phải là một con số.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbNamHoc.Focus();
                return;
            }

            LopTC_DTO ltc = GetLopTinChiFromGUI(); // Dùng LopTC_DTO

            try
            {
                if (bll.ThemLTC(ltc))
                {
                    MessageBox.Show("Thêm lớp tín chỉ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataGridView();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Thêm lớp tín chỉ thất bại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    MessageBox.Show("Mã lớp này đã tồn tại. Vui lòng nhập mã khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (string.IsNullOrWhiteSpace(tbMaLop.Text) || tbMaLop.ReadOnly == false)
            {
                MessageBox.Show("Vui lòng chọn một lớp tín chỉ từ danh sách để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LopTC_DTO ltc = GetLopTinChiFromGUI(); // Dùng LopTC_DTO

            try
            {
                if (bll.SuaLTC(ltc))
                {
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataGridView();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy lớp để cập nhật hoặc dữ liệu không đổi.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbMaLop.Text) || tbMaLop.ReadOnly == false)
            {
                MessageBox.Show("Vui lòng chọn một lớp từ danh sách để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa Lớp Tín Chỉ (Mã: {tbMaLop.Text}) không?" +
                $"\n\nCẢNH BÁO: Toàn bộ dữ liệu Phân công giảng dạy và Điểm số của sinh viên liên quan đến lớp này cũng sẽ bị xóa vĩnh viễn.",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    if (bll.XoaLTC(tbMaLop.Text))
                    {
                        MessageBox.Show("Xóa lớp tín chỉ và các dữ liệu liên quan thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataGridView();
                        ClearFields();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy lớp để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            LoadDataGridView();

            tbTimKiemTheoTen.Text = placeholderMaLop;
            tbTimKiemTheoTen.ForeColor = Color.Gray;
            tbTimTheoNam.Text = "";
            cbbTimTheoKhoa.SelectedIndex = 0;
        }

        #endregion

        #region Sự kiện Tìm kiếm (Gọi BLL)

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = (tbTimKiemTheoTen.Text == placeholderMaLop) ? "" : tbTimKiemTheoTen.Text;
            string namHocTim = tbTimTheoNam.Text;
            string maKhoaTim = cbbTimTheoKhoa.SelectedValue?.ToString();

            try
            {
                DataTable dt = bll.TimKiemLTC(tuKhoa, namHocTim, maKhoaTim); // Gọi BLL
                dgvSV.DataSource = dt;

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy kết quả nào.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (FormatException fex)
            {
                MessageBox.Show(fex.Message, "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTatCa_Click(object sender, EventArgs e)
        {
            tbTimKiemTheoTen.Text = placeholderMaLop;
            tbTimKiemTheoTen.ForeColor = Color.Gray;
            tbTimTheoNam.Text = "";
            cbbTimTheoKhoa.SelectedIndex = 0;

            LoadDataGridView();
        }

        #endregion

        #region Sự kiện Giao diện (Không thay đổi)

        private void dgvSV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSV.Rows[e.RowIndex];

                tbMaLop.Text = row.Cells["MaLop"].Value?.ToString();
                tbNamHoc.Text = row.Cells["NamHoc"].Value?.ToString();

                if (row.Cells["HocKy"].Value != null && row.Cells["HocKy"].Value != DBNull.Value)
                {
                    numericUpDownHocky.Value = Convert.ToDecimal(row.Cells["HocKy"].Value);
                }
                else
                {
                    numericUpDownHocky.Value = 1;
                }

                string maKhoa = row.Cells["MaKhoa"].Value?.ToString();
                if (!string.IsNullOrEmpty(maKhoa))
                {
                    cbbMaKhoa.SelectedValue = maKhoa;
                }
                else
                {
                    cbbMaKhoa.SelectedIndex = -1;
                }

                string maMH = row.Cells["MaMH"].Value?.ToString();
                if (!string.IsNullOrEmpty(maMH))
                {
                    cbbMaMon.SelectedValue = maMH;
                }
                else
                {
                    cbbMaMon.SelectedIndex = -1;
                }

                string tinhTrang = row.Cells["TinhTrang"].Value?.ToString();
                // Tìm control 'cbbTinhTrang'
                if (this.Controls.Find("cbbTinhTrang", true).FirstOrDefault() is ComboBox cbbTinhTrangControl)
                {
                    if (tinhTrang == "Đã phân công")
                    {
                        cbbTinhTrangControl.SelectedItem = "Đã phân công";
                    }
                    else
                    {
                        cbbTinhTrangControl.SelectedItem = "Chưa phân công";
                    }
                }

                tbMaLop.ReadOnly = true;
            }
        }

        private void cbbMaKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbMaKhoa.SelectedValue != null)
            {
                string maKhoa = cbbMaKhoa.SelectedValue.ToString();
                LoadMonHocComboBox(maKhoa);
            }
            else
            {
                LoadMonHocComboBox(null);
            }
        }

        // --- Placeholder ---
        private void SetPlaceholderText()
        {
            tbTimKiemTheoTen.Text = placeholderMaLop;
            tbTimKiemTheoTen.ForeColor = Color.Gray;
        }

        private void tbTimKiemTheoTen_Enter(object sender, EventArgs e)
        {
            if (tbTimKiemTheoTen.Text == placeholderMaLop)
            {
                tbTimKiemTheoTen.Text = "";
                tbTimKiemTheoTen.ForeColor = Color.Black;
            }
        }

        private void tbTimKiemTheoTen_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbTimKiemTheoTen.Text))
            {
                tbTimKiemTheoTen.Text = placeholderMaLop;
                tbTimKiemTheoTen.ForeColor = Color.Gray;
            }
        }

        #endregion

        // Hàm này bạn có trong code gốc, tôi giữ lại
        private void tbMaLop_TextChanged(object sender, EventArgs e)
        {

        }
    }
}