// GUI/formPhanCongGV.cs
using BTL_LTTQ.BLL;
using BTL_LTTQ.DTO;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace BTL_LTTQ.GUI
{
    public partial class formPhanCongGV : Form
    {
        private PhanCongBLL bll = new PhanCongBLL();
        private PhanCongDTO currentPC = null;
        private DataTable dtPhanCong;

        public formPhanCongGV()
        {
            InitializeComponent();
            SetupDateTimePickers();
            SetupPlaceholder();
            SetupCaHocComboBox();  // THÊM MỚI
            SetupThuComboBox();    // THÊM MỚI
            Load += formPhanCongGV_Load;
            dgvPhanCong.CellClick += dgvPhanCong_CellClick;
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnLamMoi.Click += btnLamMoi_Click;
            btnTimKiem.Click += btnTimKiem_Click;

            cmbKhuVuc.SelectedIndexChanged += cmbKhuVuc_SelectedIndexChanged;
            cmbKhoa.SelectedIndexChanged += cmbKhoa_SelectedIndexChanged;
            cmbMonHoc.SelectedIndexChanged += cmbMonHoc_SelectedIndexChanged;
            cmbLopTC.SelectedIndexChanged += cmbLopTC_SelectedIndexChanged;
            cmbMaGv.SelectedIndexChanged += cmbMaGv_SelectedIndexChanged;
        }

        private void SetupDateTimePickers()
        {
            dtpNgayPC.Format = DateTimePickerFormat.Custom;
            dtpNgayPC.CustomFormat = "dd/MM/yyyy";

            dtpNgayBatDau.Format = DateTimePickerFormat.Custom;
            dtpNgayBatDau.CustomFormat = "dd/MM/yyyy";

            dtpNgayKetThuc.Format = DateTimePickerFormat.Custom;
            dtpNgayKetThuc.CustomFormat = "dd/MM/yyyy";
        }

        private void SetupPlaceholder()
        {
            txtTimKiem.Text = "Nhập mã PC, mã GV hoặc tên GV..."; // ✅ SỬA
            txtTimKiem.ForeColor = System.Drawing.Color.Gray;
            txtTimKiem.GotFocus += (s, e) =>
            {
                if (txtTimKiem.Text == "Nhập mã PC, mã GV hoặc tên GV...") // ✅ SỬA
                {
                    txtTimKiem.Text = "";
                    txtTimKiem.ForeColor = System.Drawing.Color.Black;
                }
            };
            txtTimKiem.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtTimKiem.Text))
                {
                    txtTimKiem.Text = "Nhập mã PC, mã GV hoặc tên GV..."; // ✅ SỬA
                    txtTimKiem.ForeColor = System.Drawing.Color.Gray;
                }
            };
        }

        private void formPhanCongGV_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            LoadComboBoxes();
            LoadData();
            txtNam.ReadOnly = true;
        }

        private void SetupDataGridView()
        {
            dgvPhanCong.AutoGenerateColumns = false;
            dgvPhanCong.Columns.Clear();

            var visibleCols = new[]
            {
                ("MaPC", "Mã PC"),
                ("TenGV", "Giảng viên"),
                ("TenLop", "Lớp tín chỉ"),
                ("Thu", "Thứ"),
                ("CaHoc", "Ca học"),
                ("NgayBatDau", "Từ ngày"),
                ("NgayKetThuc", "Đến ngày"),
                ("Phong", "Phòng"),
                ("Toa", "Tòa"),
                ("NamHoc", "Năm học")
            };

            foreach (var col in visibleCols)
            {
                var column = new DataGridViewTextBoxColumn
                {
                    Name = col.Item1,
                    HeaderText = col.Item2,
                    DataPropertyName = col.Item1,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                };
                if (col.Item1.Contains("Ngay")) column.DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvPhanCong.Columns.Add(column);
            }

            string[] hiddenCols = { "MaGV", "MaLop", "MaMH" };
            foreach (string name in hiddenCols)
            {
                dgvPhanCong.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = name,
                    DataPropertyName = name,
                    Visible = false
                });
            }
            
            // FORMAT HIỂN THỊ CHO CỘT THỨ VÀ CA
            dgvPhanCong.CellFormatting += dgvPhanCong_CellFormatting;
        }

        private void dgvPhanCong_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvPhanCong.Columns[e.ColumnIndex].Name == "Thu" && e.Value != null)
            {
                byte thu = Convert.ToByte(e.Value);
                string displayText = "Không xác định";
                
                if (thu == 2) displayText = "Thứ Hai";
                else if (thu == 3) displayText = "Thứ Ba";
                else if (thu == 4) displayText = "Thứ Tư";
                else if (thu == 5) displayText = "Thứ Năm";
                else if (thu == 6) displayText = "Thứ Sáu";
                else if (thu == 7) displayText = "Thứ Bảy";
                else if (thu == 8) displayText = "Chủ Nhật";
                
                e.Value = displayText;
                e.FormattingApplied = true;
            }
            else if (dgvPhanCong.Columns[e.ColumnIndex].Name == "CaHoc" && e.Value != null)
            {
                byte ca = Convert.ToByte(e.Value);
                e.Value = $"Ca {ca}";
                e.FormattingApplied = true;
            }
        }

        private void LoadComboBoxes()
        {
            var dtKhuVuc = bll.LayKhuVuc();
            DataRow drKV = dtKhuVuc.NewRow();
            drKV["MaKhuVuc"] = ""; drKV["TenKhuVuc"] = "-- Chọn tòa --";
            dtKhuVuc.Rows.InsertAt(drKV, 0);
            cmbKhuVuc.DataSource = dtKhuVuc;
            cmbKhuVuc.DisplayMember = "TenKhuVuc";
            cmbKhuVuc.ValueMember = "MaKhuVuc";

            var dtKhoa = bll.LayKhoa();
            DataRow drK = dtKhoa.NewRow();
            drK["MaKhoa"] = ""; drK["TenKhoa"] = "-- Chọn khoa --";
            dtKhoa.Rows.InsertAt(drK, 0);
            cmbKhoa.DataSource = dtKhoa;
            cmbKhoa.DisplayMember = "TenKhoa";
            cmbKhoa.ValueMember = "MaKhoa";

            var dtGV = bll.LayGiangVien();
            cmbMaGv.DataSource = dtGV.Copy();
            cmbMaGv.DisplayMember = "MaGV";
            cmbMaGv.ValueMember = "MaGV";

            cmbTenGV.DataSource = dtGV.Copy();
            cmbTenGV.DisplayMember = "TenGV";
            cmbTenGV.ValueMember = "MaGV";
            cmbTenGV.Enabled = false;
        }

        private void LoadData()
        {
            dtPhanCong = bll.LayTatCa();
            dgvPhanCong.DataSource = dtPhanCong;
            ClearInput();
        }

        private void cmbKhuVuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbKhuVuc.SelectedIndex <= 0) { cmbPhongHoc.DataSource = null; return; }
            string maKhuVuc = cmbKhuVuc.SelectedValue?.ToString();
            if (string.IsNullOrEmpty(maKhuVuc)) return;

            var dt = bll.LayPhongTheoKhuVuc(maKhuVuc);
            DataRow dr = dt.NewRow(); dr["MaPhong"] = "";
            dt.Rows.InsertAt(dr, 0);
            cmbPhongHoc.DataSource = dt;
            cmbPhongHoc.DisplayMember = "MaPhong";
            cmbPhongHoc.ValueMember = "MaPhong";
        }

        private void cmbKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbKhoa.SelectedIndex <= 0) { cmbMonHoc.DataSource = null; return; }
            string maKhoa = cmbKhoa.SelectedValue?.ToString();
            if (string.IsNullOrEmpty(maKhoa)) return;

            var dt = bll.LayMonHocTheoKhoa(maKhoa);
            DataRow dr = dt.NewRow(); dr["MaMH"] = ""; dr["TenMH"] = "-- Chọn môn --";
            dt.Rows.InsertAt(dr, 0);
            cmbMonHoc.DataSource = dt;
            cmbMonHoc.DisplayMember = "TenMH";
            cmbMonHoc.ValueMember = "MaMH";
        }

        private void cmbMonHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMonHoc.SelectedIndex <= 0) { cmbLopTC.DataSource = null; return; }
            string maMH = cmbMonHoc.SelectedValue?.ToString();
            if (string.IsNullOrEmpty(maMH)) return;

            var dt = bll.LayLopTinChiChuaPhanCong(maMH);
            DataRow dr = dt.NewRow(); dr["MaLop"] = ""; dr["TenLop"] = "-- Chọn lớp --";
            dt.Rows.InsertAt(dr, 0);
            cmbLopTC.DataSource = dt;
            cmbLopTC.DisplayMember = "TenLop";
            cmbLopTC.ValueMember = "MaLop";
        }

        private void cmbLopTC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLopTC.SelectedValue == null || cmbLopTC.SelectedIndex <= 0)
            {
                txtNam.Text = "";
                return;
            }

            string maLop = cmbLopTC.SelectedValue.ToString();
            if (string.IsNullOrEmpty(maLop)) return;

            string query = "SELECT NamHoc FROM LopTinChi WHERE MaLop = @MaLop";
            var dt = BTL_LTTQ.DAL.DatabaseConnection.ExecuteQuery(query, new[] { new SqlParameter("@MaLop", maLop) });
            txtNam.Text = dt.Rows.Count > 0 ? dt.Rows[0]["NamHoc"]?.ToString() : "";
        }

        private void cmbMaGv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMaGv.SelectedValue != null)
            {
                var dt = bll.LayGiangVien();
                var row = dt.Select($"MaGV = '{cmbMaGv.SelectedValue}'");
                if (row.Length > 0)
                    cmbTenGV.Text = row[0]["TenGV"].ToString();
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtMaPC.Text)) { MessageBox.Show("Vui lòng nhập Mã PC!"); txtMaPC.Focus(); return false; }
            if (cmbMaGv.SelectedValue == null) { MessageBox.Show("Vui lòng chọn Giảng viên!"); cmbMaGv.Focus(); return false; }
            if (cmbKhuVuc.SelectedIndex <= 0) { MessageBox.Show("Vui lòng chọn Tòa!"); cmbKhuVuc.Focus(); return false; }
            if (string.IsNullOrEmpty(cmbPhongHoc.SelectedValue?.ToString())) { MessageBox.Show("Vui lòng chọn Phòng!"); cmbPhongHoc.Focus(); return false; }
            if (cmbKhoa.SelectedIndex <= 0) { MessageBox.Show("Vui lòng chọn Khoa!"); cmbKhoa.Focus(); return false; }
            if (cmbMonHoc.SelectedIndex <= 0) { MessageBox.Show("Vui lòng chọn Môn học!"); cmbMonHoc.Focus(); return false; }
            if (cmbLopTC.SelectedIndex <= 0) { MessageBox.Show("Vui lòng chọn Lớp tín chỉ!"); cmbLopTC.Focus(); return false; }
            if (dtpNgayBatDau.Value.Date > dtpNgayKetThuc.Value.Date) { MessageBox.Show("Ngày bắt đầu phải nhỏ hơn ngày kết thúc!"); dtpNgayBatDau.Focus(); return false; }
            return true;
        }

        private void dgvPhanCong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dtPhanCong == null || e.RowIndex >= dtPhanCong.Rows.Count) return;

            DataRow row = dtPhanCong.Rows[e.RowIndex];

            currentPC = new PhanCongDTO
            {
                MaPC = row["MaPC"]?.ToString(),
                MaGV = row["MaGV"]?.ToString(),
                MaLop = row["MaLop"]?.ToString()
            };

            txtMaPC.Text = currentPC.MaPC;
            txtMaPC.Enabled = false;

            dtpNgayPC.Value = Convert.ToDateTime(row["NgayPC"]);
            dtpNgayBatDau.Value = Convert.ToDateTime(row["NgayBatDau"]);
            dtpNgayKetThuc.Value = Convert.ToDateTime(row["NgayKetThuc"]);

            // SET CA HỌC VÀ THỨ
            byte caHoc = Convert.ToByte(row["CaHoc"]);
            byte thu = Convert.ToByte(row["Thu"]);
            cmbCa.SelectedValue = caHoc;  // HOẶC cmbCaHoc
            cmbThu.SelectedValue = thu;

            // ... (giữ nguyên phần còn lại)
            var kvRow = bll.LayKhuVuc().Select($"TenKhuVuc = '{row["Toa"]}'").FirstOrDefault();
            if (kvRow != null)
            {
                cmbKhuVuc.SelectedValue = kvRow["MaKhuVuc"];
                var dtPhong = bll.LayPhongTheoKhuVuc(kvRow["MaKhuVuc"].ToString());
                DataRow dr = dtPhong.NewRow(); dr["MaPhong"] = "";
                dtPhong.Rows.InsertAt(dr, 0);
                cmbPhongHoc.DataSource = dtPhong;
                cmbPhongHoc.SelectedValue = row["Phong"];
            }

            txtNam.Text = row["NamHoc"]?.ToString();

            string maMH = row["MaMH"]?.ToString();
            if (!string.IsNullOrEmpty(maMH))
            {
                string queryKhoa = "SELECT MaKhoa FROM MonHoc WHERE MaMH = @MaMH";
                var dtKhoa = BTL_LTTQ.DAL.DatabaseConnection.ExecuteQuery(queryKhoa, new[] { new SqlParameter("@MaMH", maMH) });
                if (dtKhoa.Rows.Count > 0)
                {
                    string maKhoa = dtKhoa.Rows[0]["MaKhoa"]?.ToString();
                    if (!string.IsNullOrEmpty(maKhoa))
                    {
                        cmbKhoa.SelectedValue = maKhoa;

                        var dtMon = bll.LayMonHocTheoKhoa(maKhoa);
                        DataRow dr = dtMon.NewRow(); dr["MaMH"] = ""; dr["TenMH"] = "-- Chọn môn --";
                        dtMon.Rows.InsertAt(dr, 0);
                        cmbMonHoc.DataSource = dtMon;
                        cmbMonHoc.SelectedValue = maMH;

                        var dtLop = bll.LayLopTinChiDaPhanCong(maMH);
                        DataRow drLop = dtLop.NewRow(); drLop["MaLop"] = ""; drLop["TenLop"] = "-- Chọn lớp --";
                        dtLop.Rows.InsertAt(drLop, 0);
                        cmbLopTC.DataSource = dtLop;
                        cmbLopTC.SelectedValue = currentPC.MaLop;

                        cmbLopTC_SelectedIndexChanged(null, null);
                    }
                }
            }

            cmbMaGv.SelectedValue = currentPC.MaGV;
        }

        private PhanCongDTO GetInput()
        {
            return new PhanCongDTO
            {
                MaPC = txtMaPC.Text.Trim(),
                NgayPC = dtpNgayPC.Value.Date,
                NgayBatDau = dtpNgayBatDau.Value.Date,
                NgayKetThuc = dtpNgayKetThuc.Value.Date,
                CaHoc = (byte)cmbCa.SelectedValue,  // GIỮ NGUYÊN
                Thu = (byte)cmbThu.SelectedValue,
                MaPhong = cmbPhongHoc.SelectedValue?.ToString(),
                MaGV = cmbMaGv.SelectedValue?.ToString(),
                MaLop = cmbLopTC.SelectedValue?.ToString()
            };
        }

        private void ClearInput()
        {
            txtMaPC.Clear(); txtMaPC.Enabled = true;
            dtpNgayPC.Value = DateTime.Today;
            dtpNgayBatDau.Value = DateTime.Today;
            dtpNgayKetThuc.Value = DateTime.Today.AddDays(30);
            cmbCa.SelectedIndex = 0;  // HOẶC cmbCaHoc
            cmbThu.SelectedIndex = 0;
            cmbKhuVuc.SelectedIndex = 0;
            cmbPhongHoc.DataSource = null;
            cmbKhoa.SelectedIndex = 0;
            cmbMonHoc.DataSource = null;
            cmbLopTC.DataSource = null;
            txtNam.Text = "";
            cmbMaGv.Text = "";
            cmbTenGV.Text = "";
            currentPC = null;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            var pc = GetInput();

            if (bll.KiemTraMaPCTrung(pc.MaPC))
            {
                MessageBox.Show("Mã phân công đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (bll.KiemTraLopDaPhanCong(pc.MaLop))
            {
                MessageBox.Show($"LỚP TÍN CHỈ ĐÃ ĐƯỢC PHÂN CÔNG!\n\nLớp: {cmbLopTC.Text}\nMã lớp: {pc.MaLop}", "Lớp đã được phân công", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (bll.KiemTraTrungLichGV(pc.MaGV, pc.NgayBatDau, pc.NgayKetThuc, pc.Thu, pc.CaHoc))
            {
                MessageBox.Show($"LỊCH GIẢNG VIÊN TRÙNG!\n\nGiảng viên: {cmbTenGV.Text}\nThứ: {cmbThu.Text}\nCa: {cmbCa.Text}", "Lịch trùng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (bll.KiemTraTrungPhong(pc.MaPhong, pc.NgayBatDau, pc.NgayKetThuc, pc.Thu, pc.CaHoc))
            {
                MessageBox.Show($"PHÒNG ĐÃ ĐƯỢC SỬ DỤNG!\n\nPhòng: {pc.MaPhong}\nThứ: {cmbThu.Text}\nCa: {cmbCa.Text}", "Phòng trùng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (bll.Them(pc))
            {
                MessageBox.Show("THÊM THÀNH CÔNG!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            else
            {
                MessageBox.Show("Thêm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (currentPC == null)
            {
                MessageBox.Show("Vui lòng chọn phân công để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy thông tin để hiển thị
            var row = dgvPhanCong.CurrentRow;
            string tenGV = row?.Cells["TenGV"].Value?.ToString() ?? "";
            string tenLop = row?.Cells["TenLop"].Value?.ToString() ?? "";
            string phong = row?.Cells["Phong"].Value?.ToString() ?? "";

            DialogResult result = MessageBox.Show(
                $"BẠN CÓ CHẮC CHẮN MUỐN XÓA?\n\n" +
                $"Mã phân công: {currentPC.MaPC}\n" +
                $"Giảng viên: {tenGV}\n" +
                $"Lớp: {tenLop}\n" +
                $"Phòng: {phong}\n\n" +
                $"⚠ Sau khi xóa:\n" +
                $"- Lớp sẽ được đánh dấu 'Chưa phân công'\n" +
                $"- Có thể phân công lại cho giảng viên khác",
                "Xác nhận xóa phân công",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (bll.Xoa(currentPC.MaPC, currentPC.MaLop))
                {
                    MessageBox.Show(
                        $"XÓA PHÂN CÔNG THÀNH CÔNG!\n\n" +
                        $"Lớp '{tenLop}' đã được đánh dấu 'Chưa phân công'.\n" +
                        $"Bạn có thể phân công lại lớp này cho giảng viên khác.",
                        "Xóa thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show(
                        "Không thể xóa phân công!\n\n" +
                        "Vui lòng kiểm tra lại hoặc liên hệ quản trị viên.",
                        "Xóa thất bại",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (currentPC == null)
            {
                MessageBox.Show("Vui lòng chọn phân công để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput()) return;

            var pc = GetInput();
            pc.MaPC = currentPC.MaPC;

            if (pc.MaLop != currentPC.MaLop && bll.KiemTraLopDaPhanCong(pc.MaLop))
            {
                MessageBox.Show($"LỚP MỚI ĐÃ ĐƯỢC PHÂN CÔNG!\n\nLớp: {cmbLopTC.Text}", "Lớp đã được phân công", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (bll.KiemTraTrungLichGV(pc.MaGV, pc.NgayBatDau, pc.NgayKetThuc, pc.Thu, pc.CaHoc, pc.MaPC))
            {
                MessageBox.Show($"LỊCH GIẢNG VIÊN TRÙNG!\n\nGiảng viên: {cmbTenGV.Text}", "Lịch trùng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (bll.KiemTraTrungPhong(pc.MaPhong, pc.NgayBatDau, pc.NgayKetThuc, pc.Thu, pc.CaHoc, pc.MaPC))
            {
                MessageBox.Show($"PHÒNG ĐÃ ĐƯỢC SỬ DỤNG!\n\nPhòng: {pc.MaPhong}", "Phòng trùng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (bll.Sua(pc, currentPC.MaLop))
            {
                MessageBox.Show("SỬA THÀNH CÔNG!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            else
            {
                MessageBox.Show("Sửa thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e) => ClearInput();

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            if (keyword == "Nhập mã PC, mã GV hoặc tên GV..." || string.IsNullOrEmpty(keyword))
            {
                LoadData();
                return;
            }

            try
            {
                // Lọc theo MaPC, MaGV hoặc TenGV
                var filtered = dtPhanCong.AsEnumerable()
                    .Where(r => 
                    {
                        string maPC = r.Field<string>("MaPC") ?? "";
                        string maGV = r.Field<string>("MaGV") ?? "";
                        string tenGV = r.Field<string>("TenGV") ?? "";
                        
                        return maPC.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                               maGV.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                               tenGV.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
                    });

                if (filtered.Any())
                {
                    dgvPhanCong.DataSource = filtered.CopyToDataTable();
                    int count = filtered.Count();
                    MessageBox.Show(
                        $"Tìm thấy {count} kết quả với từ khóa: '{keyword}'", 
                        "Kết quả tìm kiếm", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        $"Không tìm thấy kết quả nào với từ khóa: '{keyword}'", 
                        "Kết quả tìm kiếm", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi khi tìm kiếm: {ex.Message}", 
                    "Lỗi", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                
                LoadData();
            }
        }

        private void SetupCaHocComboBox()
        {
            var dtCa = new DataTable();
            dtCa.Columns.Add("Value", typeof(byte));
            dtCa.Columns.Add("Display", typeof(string));
            
            dtCa.Rows.Add((byte)1, "Ca 1 (7:00 - 9:30)");
            dtCa.Rows.Add((byte)2, "Ca 2 (9:35 - 12:00)");
            dtCa.Rows.Add((byte)3, "Ca 3 (13:00 - 15:30)");
            dtCa.Rows.Add((byte)4, "Ca 4 (15:35 - 18:00)");
            dtCa.Rows.Add((byte)5, "Ca 5 (18:30 - 21:00)");
            
            // CHỌN TÊN ĐÚNG: cmbCa HOẶC cmbCaHoc (kiểm tra Designer)
            cmbCa.DataSource = dtCa;  // HOẶC cmbCaHoc
            cmbCa.DisplayMember = "Display";
            cmbCa.ValueMember = "Value";
            cmbCa.SelectedIndex = 0;
        }

        private void SetupThuComboBox()
        {
            var dtThu = new DataTable();
            dtThu.Columns.Add("Value", typeof(byte));
            dtThu.Columns.Add("Display", typeof(string));
            
            dtThu.Rows.Add((byte)2, "Thứ Hai");
            dtThu.Rows.Add((byte)3, "Thứ Ba");
            dtThu.Rows.Add((byte)4, "Thứ Tư");
            dtThu.Rows.Add((byte)5, "Thứ Năm");
            dtThu.Rows.Add((byte)6, "Thứ Sáu");
            dtThu.Rows.Add((byte)7, "Thứ Bảy");
            dtThu.Rows.Add((byte)8, "Chủ Nhật");
            
            cmbThu.DataSource = dtThu;
            cmbThu.DisplayMember = "Display";
            cmbThu.ValueMember = "Value";
            cmbThu.SelectedIndex = 0;
        }
    }
}