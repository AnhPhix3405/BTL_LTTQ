// formSV.cs
using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BTL_LTTQ.BLL;
using BTL_LTTQ.DTO;

namespace BTL_LTTQ
{
    public partial class formSV : Form
    {
        private SinhVienBLL svBLL = new SinhVienBLL();
        private string maLopHienTai = "";

        public formSV()
        {
            InitializeComponent();
            CauHinhPlaceholder();
            CauHinhNgaySinh();
        }

        private void formSV_Load(object sender, EventArgs e)
        {
            LoadKhoa();
            LoadTatCaLopTinChi();
            LoadTatCaSinhVien();
            SetupDataGridView();
            KhoiTaoTrangThaiThemMoi();

            // Gắn sự kiện
            cmbTimTheoKhoa.SelectedIndexChanged += cmbTimTheoKhoa_SelectedIndexChanged;
            cmbTimTheoLop.SelectedIndexChanged += cmbTimTheoLop_SelectedIndexChanged;
            tbTimKiemTheoMa.TextChanged += tbTimKiemTheoMa_TextChanged;
            dgvSV.CellDoubleClick += dgvSV_CellDoubleClick;
            dgvSV.CellClick += dgvSV_CellClick;
            btnTimKiem.Click += btnTimKiem_Click;
            btnTatCa.Click += btnTatCa_Click;
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnLamMoi.Click += btnLamMoi_Click;
        }

        #region CẤU HÌNH
        private void CauHinhNgaySinh()
        {
            dtpNgaySinh.Format = DateTimePickerFormat.Custom;
            dtpNgaySinh.CustomFormat = "dd/MM/yyyy";
            dtpNgaySinh.ShowUpDown = false;
        }

        private void CauHinhPlaceholder()
        {
            tbTimKiemTheoMa.GotFocus += (s, e) =>
            {
                if (tbTimKiemTheoMa.Text == "Nhập mã sinh viên...")
                {
                    tbTimKiemTheoMa.Text = "";
                    tbTimKiemTheoMa.ForeColor = System.Drawing.SystemColors.WindowText;
                }
            };
            tbTimKiemTheoMa.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(tbTimKiemTheoMa.Text))
                {
                    tbTimKiemTheoMa.Text = "Nhập mã sinh viên...";
                    tbTimKiemTheoMa.ForeColor = System.Drawing.SystemColors.GrayText;
                }
            };
        }
        #endregion

        #region DATA GRID VIEW
        private void SetupDataGridView()
        {
            dgvSV.AutoGenerateColumns = false;
            dgvSV.Columns.Clear();

            string[] colNames = { "MaSV", "TenSV", "GioiTinh", "NgaySinh", "QueQuan", "SDT", "Email", "TenKhoa", "TenLop" };
            string[] headers = { "Mã SV", "Họ tên", "Giới tính", "Ngày sinh", "Quê quán", "SĐT", "Email", "Khoa", "Lớp tín chỉ" };

            for (int i = 0; i < colNames.Length; i++)
            {
                var col = new DataGridViewTextBoxColumn
                {
                    Name = colNames[i],
                    HeaderText = headers[i],
                    DataPropertyName = colNames[i],
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                };
                if (colNames[i] == "NgaySinh")
                    col.DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvSV.Columns.Add(col);
            }
        }
        #endregion

        #region LOAD DỮ LIỆU
        private void LoadKhoa()
        {
            var dt = svBLL.LayKhoa();
            DataRow empty = dt.NewRow();
            empty["MaKhoa"] = DBNull.Value;
            empty["TenKhoa"] = "-- Chọn khoa --";
            dt.Rows.InsertAt(empty, 0);

            var dsKhoa = dt.Copy();

            cmbTimTheoKhoa.DataSource = dsKhoa;
            cmbTimTheoKhoa.DisplayMember = "TenKhoa";
            cmbTimTheoKhoa.ValueMember = "MaKhoa";

            cmbKhoa.DataSource = dt.Copy();
            cmbKhoa.DisplayMember = "TenKhoa";
            cmbKhoa.ValueMember = "MaKhoa";

            cmbTimTheoKhoa.SelectedIndex = 0;
            cmbKhoa.SelectedIndex = 0;
        }

        private void LoadTatCaLopTinChi()
        {
            var dt = svBLL.LayTatCaLopTinChi();
            DataRow empty = dt.NewRow();
            empty["MaLop"] = DBNull.Value;
            empty["TenLop"] = "-- Chọn lớp --";
            dt.Rows.InsertAt(empty, 0);

            cmbTimTheoLop.DataSource = dt;
            cmbTimTheoLop.DisplayMember = "TenLop";
            cmbTimTheoLop.ValueMember = "MaLop";
            cmbTimTheoLop.SelectedIndex = 0;
        }

        private void LoadTatCaSinhVien()
        {
            var dt = svBLL.TimKiem("", "", "");
            HienThiDanhSach(dt);
        }

        private void HienThiDanhSach(DataTable dt)
        {
            dgvSV.DataSource = dt;
        }
        #endregion

        #region ĐỒNG BỘ KHOA & LỚP
        private void DongBoKhoaLopChiTiet()
        {
            if (cmbTimTheoKhoa.SelectedValue != null)
            {
                string maKhoa = cmbTimTheoKhoa.SelectedValue.ToString();
                if (!string.IsNullOrEmpty(maKhoa) && maKhoa != "System.DBNull")
                {
                    cmbKhoa.SelectedValue = maKhoa;
                }
            }

            if (cmbTimTheoLop.SelectedValue != null)
            {
                string maLop = cmbTimTheoLop.SelectedValue.ToString();
                if (!string.IsNullOrEmpty(maLop) && maLop != "System.DBNull")
                {
                    maLopHienTai = maLop;

                    string maKhoa = cmbTimTheoKhoa.SelectedValue?.ToString() ?? "";
                    DataTable dtLop = string.IsNullOrEmpty(maKhoa)
                        ? svBLL.LayTatCaLopTinChi()
                        : svBLL.LayLopTinChiTheoKhoa(maKhoa);

                    cmbMaLopTC.DataSource = dtLop;
                    cmbMaLopTC.DisplayMember = "TenLop";
                    cmbMaLopTC.ValueMember = "MaLop";
                    cmbMaLopTC.SelectedValue = maLop;
                    cmbMaLopTC.Enabled = false;
                }
            }
        }

        private void DongBoTuSinhVienLenTimKiem(string tenKhoa, string tenLop)
        {
            for (int i = 0; i < cmbTimTheoKhoa.Items.Count; i++)
            {
                var item = cmbTimTheoKhoa.Items[i] as DataRowView;
                if (item != null && item["TenKhoa"].ToString() == tenKhoa)
                {
                    cmbTimTheoKhoa.SelectedIndex = i;
                    break;
                }
            }

            string maKhoa = cmbTimTheoKhoa.SelectedValue?.ToString() ?? "";
            if (!string.IsNullOrEmpty(maKhoa) && maKhoa != "System.DBNull")
            {
                var dtLop = svBLL.LayLopTinChiTheoKhoa(maKhoa);
                DataRow empty = dtLop.NewRow();
                empty["MaLop"] = DBNull.Value;
                empty["TenLop"] = "-- Chọn lớp --";
                dtLop.Rows.InsertAt(empty, 0);

                cmbTimTheoLop.DataSource = dtLop;
                cmbTimTheoLop.DisplayMember = "TenLop";
                cmbTimTheoLop.ValueMember = "MaLop";

                var found = dtLop.Select($"TenLop = '{tenLop.Replace("'", "''")}'");
                if (found.Length > 0)
                    cmbTimTheoLop.SelectedValue = found[0]["MaLop"];
            }
        }
        #endregion

        #region LỌC THEO KHOA & LỚP
        private void cmbTimTheoKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTimTheoKhoa.SelectedIndex <= 0 || cmbTimTheoKhoa.SelectedValue == null) return;

            string maKhoa = cmbTimTheoKhoa.SelectedValue.ToString();
            if (string.IsNullOrEmpty(maKhoa) || maKhoa == "System.DBNull") return;

            var dtLop = svBLL.LayLopTinChiTheoKhoa(maKhoa);
            DataRow empty = dtLop.NewRow();
            empty["MaLop"] = DBNull.Value;
            empty["TenLop"] = "-- Chọn lớp --";
            dtLop.Rows.InsertAt(empty, 0);

            cmbTimTheoLop.DataSource = dtLop;
            cmbTimTheoLop.DisplayMember = "TenLop";
            cmbTimTheoLop.ValueMember = "MaLop";
            cmbTimTheoLop.SelectedIndex = 0;

            TimKiemTuDong();
            DongBoKhoaLopChiTiet();
        }

        private void cmbTimTheoLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            maLopHienTai = cmbTimTheoLop.SelectedValue?.ToString() ?? "";
            TimKiemTuDong();
            DongBoKhoaLopChiTiet();
        }

        private void tbTimKiemTheoMa_TextChanged(object sender, EventArgs e)
        {
            if (tbTimKiemTheoMa.Text == "Nhập mã sinh viên..." || string.IsNullOrWhiteSpace(tbTimKiemTheoMa.Text))
                return;
            TimKiemTuDong();
        }

        private void TimKiemTuDong()
        {
            string maKhoa = cmbTimTheoKhoa.SelectedValue?.ToString() ?? "";
            string maLop = cmbTimTheoLop.SelectedValue?.ToString() ?? "";
            string maSV = tbTimKiemTheoMa.Text.Trim();

            if (maSV == "Nhập mã sinh viên...") maSV = "";
            if (maKhoa == "System.DBNull") maKhoa = "";
            if (maLop == "System.DBNull") maLop = "";

            var dt = svBLL.TimKiem(maKhoa, maLop, maSV);
            HienThiDanhSach(dt);
        }
        #endregion

        #region NÚT TÌM & TẤT CẢ
        private void btnTimKiem_Click(object sender, EventArgs e) => TimKiemTuDong();

        private void btnTatCa_Click(object sender, EventArgs e)
        {
            cmbTimTheoKhoa.SelectedIndex = 0;
            LoadTatCaLopTinChi();
            tbTimKiemTheoMa.Text = "Nhập mã sinh viên...";
            tbTimKiemTheoMa.ForeColor = System.Drawing.SystemColors.GrayText;
            LoadTatCaSinhVien();
            KhoiTaoTrangThaiThemMoi();
        }
        #endregion

        #region VALIDATE
        private bool ValidateInput()
        {
            if (!string.IsNullOrWhiteSpace(tbSoDt.Text))
            {
                if (!Regex.IsMatch(tbSoDt.Text.Trim(), @"^\d{10}$"))
                {
                    MessageBox.Show("Số điện thoại phải đúng 10 chữ số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tbSoDt.Focus();
                    return false;
                }
            }

            if (!string.IsNullOrWhiteSpace(tbEmail.Text))
            {
                if (!Regex.IsMatch(tbEmail.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    MessageBox.Show("Email phải có định dạng: abc@domain.com", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tbEmail.Focus();
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region TRẠNG THÁI FORM
        private void KhoiTaoTrangThaiThemMoi()
        {
            tbMaSV.Clear(); tbMaSV.Enabled = true;
            tbHoTen.Clear();
            dtpNgaySinh.Value = DateTime.Now;
            rdoNam.Checked = true;
            tbDiaChi.Clear(); tbSoDt.Clear(); tbEmail.Clear();

            // Giữ lớp đang chọn → khóa
            if (!string.IsNullOrEmpty(maLopHienTai) && maLopHienTai != "System.DBNull")
            {
                cmbMaLopTC.Enabled = false;
            }
            else
            {
                cmbMaLopTC.Enabled = true;
            }
        }

        private void KhoiTaoTrangThaiSua()
        {
            tbMaSV.Enabled = false;
            cmbKhoa.Enabled = false;
            cmbMaLopTC.Enabled = false;
        }
        #endregion

        #region THÊM SINH VIÊN – KIỂM TRA THÔNG MINH
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(maLopHienTai) || maLopHienTai == "System.DBNull")
            {
                MessageBox.Show("Vui lòng chọn lớp tín chỉ ở phần tìm kiếm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maSV = tbMaSV.Text.Trim();
            if (string.IsNullOrWhiteSpace(maSV))
            {
                MessageBox.Show("Nhập Mã SV!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 1. Kiểm tra SV đã có trong lớp hiện tại chưa?
            if (svBLL.KiemTraSVTrongLop(maSV, maLopHienTai))
            {
                MessageBox.Show($"Sinh viên {maSV} đã tồn tại trong lớp này!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbMaSV.Focus();
                return;
            }

            // 2. Kiểm tra SV có tồn tại ở lớp khác không?
            var svTonTai = svBLL.LaySinhVienTheoMa(maSV);
            if (svTonTai != null)
            {
                // Đã có ở lớp khác → hỏi thêm và điền toàn bộ dữ liệu
                string msg = $"Sinh viên mã {maSV} đã tồn tại:\n" +
                             $"- Tên: {svTonTai.TenSV}\n" +
                             $"- Giới tính: {svTonTai.GioiTinh}\n" +
                             $"- Ngày sinh: {svTonTai.NgaySinh:dd/MM/yyyy}\n" +
                             $"- Quê quán: {svTonTai.QueQuan}\n" +
                             $"- SĐT: {svTonTai.SDT}\n" +
                             $"- Email: {svTonTai.Email}\n\n" +
                             $"Bạn có muốn thêm sinh viên này vào lớp hiện tại không?";

                if (MessageBox.Show(msg, "Xác nhận thêm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Điền toàn bộ thông tin vào form
                    tbHoTen.Text = svTonTai.TenSV;
                    dtpNgaySinh.Value = svTonTai.NgaySinh ?? DateTime.Now;
                    rdoNam.Checked = svTonTai.GioiTinh == "Nam";
                    rdoNu.Checked = svTonTai.GioiTinh != "Nam";
                    tbDiaChi.Text = svTonTai.QueQuan;
                    tbSoDt.Text = svTonTai.SDT;
                    tbEmail.Text = svTonTai.Email;

                    // Đồng bộ khoa nếu khác
                    if (!string.IsNullOrEmpty(svTonTai.MaKhoa))
                    {
                        cmbKhoa.SelectedValue = svTonTai.MaKhoa;
                    }
                }
                else
                {
                    tbMaSV.Focus();
                    return;
                }
            }
            else
            {
                // Sinh viên mới → validate đầy đủ
                if (string.IsNullOrWhiteSpace(tbHoTen.Text))
                {
                    MessageBox.Show("Nhập Họ tên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidateInput()) return;
            }

            // Tạo đối tượng SV (dùng dữ liệu từ form)
            var sv = new SinhVienDTO
            {
                MaSV = maSV,
                TenSV = tbHoTen.Text.Trim(),
                NgaySinh = dtpNgaySinh.Value,
                GioiTinh = rdoNam.Checked ? "Nam" : "Nữ",
                QueQuan = tbDiaChi.Text.Trim(),
                SDT = tbSoDt.Text.Trim(),
                Email = tbEmail.Text.Trim(),
                MaKhoa = cmbKhoa.SelectedValue?.ToString() ?? "",
                MaLop = maLopHienTai
            };

            if (svBLL.Them(sv))
            {
                MessageBox.Show("Thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TimKiemTuDong();
                KhoiTaoTrangThaiThemMoi();
            }
            else
            {
                MessageBox.Show("Thêm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region SỬA / XÓA
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbMaSV.Text))
            {
                MessageBox.Show("Chọn sinh viên để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput()) return;

            var sv = new SinhVienDTO
            {
                MaSV = tbMaSV.Text.Trim(),
                TenSV = tbHoTen.Text.Trim(),
                NgaySinh = dtpNgaySinh.Value,
                GioiTinh = rdoNam.Checked ? "Nam" : "Nữ",
                QueQuan = tbDiaChi.Text.Trim(),
                SDT = tbSoDt.Text.Trim(),
                Email = tbEmail.Text.Trim()
            };

            if (svBLL.Sua(sv))
            {
                MessageBox.Show("Sửa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TimKiemTuDong();
            }
            else
            {
                MessageBox.Show("Sửa thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(maLopHienTai) || maLopHienTai == "System.DBNull" || string.IsNullOrWhiteSpace(tbMaSV.Text))
            {
                MessageBox.Show("Chọn lớp và sinh viên để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Xóa sinh viên {tbMaSV.Text} khỏi lớp?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (svBLL.Xoa(tbMaSV.Text.Trim(), maLopHienTai))
                {
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TimKiemTuDong();
                    KhoiTaoTrangThaiThemMoi();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            KhoiTaoTrangThaiThemMoi(); // LỚP DƯỚI KHÔNG SỬA ĐƯỢC
        }
        #endregion

        #region CLICK SINH VIÊN
        private void dgvSV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            LoadChiTietSinhVien(e.RowIndex);
        }

        private void dgvSV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            LoadChiTietSinhVien(e.RowIndex);
        }

        private void LoadChiTietSinhVien(int rowIndex)
        {
            var row = dgvSV.Rows[rowIndex];

            tbMaSV.Text = row.Cells["MaSV"].Value?.ToString() ?? "";
            tbHoTen.Text = row.Cells["TenSV"].Value?.ToString() ?? "";
            dtpNgaySinh.Value = row.Cells["NgaySinh"].Value is DateTime dt ? dt : DateTime.Today;
            rdoNam.Checked = row.Cells["GioiTinh"].Value?.ToString() == "Nam";
            rdoNu.Checked = !rdoNam.Checked;
            tbDiaChi.Text = row.Cells["QueQuan"].Value?.ToString() ?? "";
            tbSoDt.Text = row.Cells["SDT"].Value?.ToString() ?? "";
            tbEmail.Text = row.Cells["Email"].Value?.ToString() ?? "";

            string tenKhoa = row.Cells["TenKhoa"].Value?.ToString() ?? "";
            string tenLop = row.Cells["TenLop"].Value?.ToString() ?? "";

            if (!string.IsNullOrEmpty(tenKhoa) && !string.IsNullOrEmpty(tenLop))
            {
                DongBoTuSinhVienLenTimKiem(tenKhoa, tenLop);
            }

            DongBoKhoaLopChiTiet();
            KhoiTaoTrangThaiSua();
        }
        #endregion
    }
}