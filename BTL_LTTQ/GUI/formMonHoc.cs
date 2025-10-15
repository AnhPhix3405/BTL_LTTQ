using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BTL_LTTQ.BLL.Subject;
using BTL_LTTQ.DTO;

namespace BTL_LTTQ
{
    public partial class formMonHoc : Form
    {
        private SubjectBLL subjectBLL;
        private List<DTO.Subject> currentSubjects;

        public formMonHoc()
        {
            InitializeComponent();
            subjectBLL = new SubjectBLL();
            currentSubjects = new List<DTO.Subject>();
            
            // Khởi tạo form
            InitializeForm();
        }

        private void InitializeForm()
        {
            try
            {
                // Load danh sách khoa vào ComboBox
                LoadDepartments();
                
                // Load tất cả môn học
                LoadAllSubjects();
                
                // Thiết lập DataGridView
                SetupDataGridView();
                
                // Gán sự kiện
                AttachEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo form: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDepartments()
        {
            try
            {
                List<Department> departments = subjectBLL.GetAllDepartments();
                
                // Thêm item "Tất cả" vào đầu danh sách
                cbbMaKhoa.Items.Clear();
                cbbMaKhoa.Items.Add(new { MaKhoa = "", TenKhoa = "-- Chọn khoa --" });
                
                cbbTimTheoKhoa.Items.Clear();
                cbbTimTheoKhoa.Items.Add(new { MaKhoa = "", TenKhoa = "-- Tất cả khoa --" });
                
                foreach (Department dept in departments)
                {
                    var item = new { MaKhoa = dept.MaKhoa, TenKhoa = $"{dept.MaKhoa} - {dept.TenKhoa}" };
                    cbbMaKhoa.Items.Add(item);
                    cbbTimTheoKhoa.Items.Add(item);
                }
                
                cbbMaKhoa.DisplayMember = "TenKhoa";
                cbbMaKhoa.ValueMember = "MaKhoa";
                cbbMaKhoa.SelectedIndex = 0;
                
                cbbTimTheoKhoa.DisplayMember = "TenKhoa";
                cbbTimTheoKhoa.ValueMember = "MaKhoa";
                cbbTimTheoKhoa.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load danh sách khoa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAllSubjects()
        {
            try
            {
                currentSubjects = subjectBLL.GetAllSubjects();
                DisplaySubjects(currentSubjects);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load danh sách môn học: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplaySubjects(List<DTO.Subject> subjects)
        {
            try
            {
                dgvSV.DataSource = null;
                
                var displayData = subjects.Select(s => new
                {
                    MaMonHoc = s.MaMH,
                    TenMonHoc = s.TenMH,
                    SoTinChi = s.SoTC,
                    TietLyThuyết = s.TietLT,
                    TietThucHanh = s.TietTH,
                    MaKhoa = s.MaKhoa,
                    TenKhoa = s.TenKhoa
                }).ToList();
                
                dgvSV.DataSource = displayData;
                
                // Căn chỉnh cột
                if (dgvSV.Columns.Count > 0)
                {
                    dgvSV.Columns["MaMonHoc"].HeaderText = "Mã MH";
                    dgvSV.Columns["TenMonHoc"].HeaderText = "Tên Môn Học";
                    dgvSV.Columns["SoTinChi"].HeaderText = "Số TC";
                    dgvSV.Columns["TietLyThuyết"].HeaderText = "Tiết LT";
                    dgvSV.Columns["TietThucHanh"].HeaderText = "Tiết TH";
                    dgvSV.Columns["MaKhoa"].HeaderText = "Mã Khoa";
                    dgvSV.Columns["TenKhoa"].HeaderText = "Tên Khoa";
                    
                    dgvSV.Columns["MaMonHoc"].Width = 80;
                    dgvSV.Columns["TenMonHoc"].Width = 200;
                    dgvSV.Columns["SoTinChi"].Width = 70;
                    dgvSV.Columns["TietLyThuyết"].Width = 70;
                    dgvSV.Columns["TietThucHanh"].Width = 70;
                    dgvSV.Columns["MaKhoa"].Width = 80;
                    dgvSV.Columns["TenKhoa"].Width = 150;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi hiển thị dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupDataGridView()
        {
            dgvSV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSV.MultiSelect = false;
            dgvSV.ReadOnly = true;
            dgvSV.AllowUserToAddRows = false;
            dgvSV.AllowUserToDeleteRows = false;
        }

        private void AttachEvents()
        {
            // Sự kiện click cho các nút
            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnRefresh.Click += BtnRefresh_Click;
            btnTimKiem.Click += BtnTimKiem_Click;
            btnTatCa.Click += BtnTatCa_Click;
            
            // Sự kiện click trên DataGridView
            dgvSV.CellClick += DgvSV_CellClick;
        }

        private void DgvSV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && dgvSV.Rows[e.RowIndex].DataBoundItem != null)
                {
                    var selectedSubject = currentSubjects[e.RowIndex];
                    
                    tbMaMon.Text = selectedSubject.MaMH;
                    tbTenMon.Text = selectedSubject.TenMH;
                    tbSoTC.Text = selectedSubject.SoTC.ToString();
                    tbTietLT.Text = selectedSubject.TietLT.ToString();
                    tbTietTH.Text = selectedSubject.TietTH.ToString();
                    
                    // Set ComboBox khoa
                    for (int i = 0; i < cbbMaKhoa.Items.Count; i++)
                    {
                        dynamic item = cbbMaKhoa.Items[i];
                        if (item.MaKhoa == selectedSubject.MaKhoa)
                        {
                            cbbMaKhoa.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn dòng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateInput())
                {
                    var newSubject = new DTO.Subject
                    {
                        MaMH = tbMaMon.Text.Trim(),
                        TenMH = tbTenMon.Text.Trim(),
                        SoTC = int.Parse(tbSoTC.Text.Trim()),
                        TietLT = int.Parse(tbTietLT.Text.Trim()),
                        TietTH = int.Parse(tbTietTH.Text.Trim()),
                        MaKhoa = ((dynamic)cbbMaKhoa.SelectedItem).MaKhoa
                    };

                    if (subjectBLL.AddSubject(newSubject))
                    {
                        MessageBox.Show("Thêm môn học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAllSubjects();
                        ClearInput();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm môn học: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbMaMon.Text))
                {
                    MessageBox.Show("Vui lòng chọn môn học để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (ValidateInput())
                {
                    var updateSubject = new DTO.Subject
                    {
                        MaMH = tbMaMon.Text.Trim(),
                        TenMH = tbTenMon.Text.Trim(),
                        SoTC = int.Parse(tbSoTC.Text.Trim()),
                        TietLT = int.Parse(tbTietLT.Text.Trim()),
                        TietTH = int.Parse(tbTietTH.Text.Trim()),
                        MaKhoa = ((dynamic)cbbMaKhoa.SelectedItem).MaKhoa
                    };

                    if (subjectBLL.UpdateSubject(updateSubject))
                    {
                        MessageBox.Show("Cập nhật môn học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAllSubjects();
                        ClearInput();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật môn học: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbMaMon.Text))
                {
                    MessageBox.Show("Vui lòng chọn môn học để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa môn học '{tbTenMon.Text}'?", 
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (subjectBLL.DeleteSubject(tbMaMon.Text.Trim()))
                    {
                        MessageBox.Show("Xóa môn học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAllSubjects();
                        ClearInput();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa môn học: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadAllSubjects();
            ClearInput();
        }

        private void BtnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string searchText = tbTimKiemTheoTen.Text.Trim();
                string maKhoa = ((dynamic)cbbTimTheoKhoa.SelectedItem)?.MaKhoa ?? "";

                if (string.IsNullOrEmpty(searchText) && string.IsNullOrEmpty(maKhoa))
                {
                    LoadAllSubjects();
                    return;
                }

                List<DTO.Subject> searchResults = new List<DTO.Subject>();

                if (!string.IsNullOrEmpty(searchText))
                {
                    // Thử tìm theo mã trước (nếu text ngắn và không có dấu cách)
                    if (searchText.Length <= 10 && !searchText.Contains(" "))
                    {
                        // Có thể là mã môn học
                        var codeResults = subjectBLL.SearchSubjectsByCode(searchText);
                        if (codeResults.Count > 0)
                        {
                            searchResults = codeResults;
                        }
                        else
                        {
                            // Nếu không tìm thấy theo mã, thử tìm theo tên
                            searchResults = subjectBLL.SearchSubjectsByName(searchText);
                        }
                    }
                    else
                    {
                        // Có thể là tên môn học
                        searchResults = subjectBLL.SearchSubjectsByName(searchText);
                    }
                }
                else
                {
                    // Chỉ tìm theo khoa
                    searchResults = subjectBLL.GetSubjectsByDepartment(maKhoa);
                }

                // Lọc thêm theo khoa nếu có
                if (!string.IsNullOrEmpty(maKhoa) && !string.IsNullOrEmpty(searchText))
                {
                    searchResults = searchResults.Where(s => s.MaKhoa.Equals(maKhoa, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                DisplaySubjects(searchResults);
                
                // Hiển thị thông báo kết quả
                if (searchResults.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy kết quả nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnTatCa_Click(object sender, EventArgs e)
        {
            LoadAllSubjects();
            tbTimKiemTheoTen.Clear();
            cbbTimTheoKhoa.SelectedIndex = 0;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(tbMaMon.Text.Trim()))
            {
                MessageBox.Show("Mã môn học không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbMaMon.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(tbTenMon.Text.Trim()))
            {
                MessageBox.Show("Tên môn học không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbTenMon.Focus();
                return false;
            }

            if (!int.TryParse(tbSoTC.Text.Trim(), out int soTC) || soTC <= 0)
            {
                MessageBox.Show("Số tín chỉ phải là số nguyên dương!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbSoTC.Focus();
                return false;
            }

            if (!int.TryParse(tbTietLT.Text.Trim(), out int tietLT) || tietLT < 0)
            {
                MessageBox.Show("Số tiết lý thuyết phải là số nguyên không âm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbTietLT.Focus();
                return false;
            }

            if (!int.TryParse(tbTietTH.Text.Trim(), out int tietTH) || tietTH < 0)
            {
                MessageBox.Show("Số tiết thực hành phải là số nguyên không âm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbTietTH.Focus();
                return false;
            }

            if (cbbMaKhoa.SelectedIndex <= 0)
            {
                MessageBox.Show("Vui lòng chọn khoa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbMaKhoa.Focus();
                return false;
            }

            return true;
        }

        private void ClearInput()
        {
            tbMaMon.Clear();
            tbTenMon.Clear();
            tbSoTC.Clear();
            tbTietLT.Clear();
            tbTietTH.Clear();
            cbbMaKhoa.SelectedIndex = 0;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
