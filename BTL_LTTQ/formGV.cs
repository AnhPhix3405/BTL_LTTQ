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
    public partial class formGV : Form
    {

        ProcessDatabase processDatabase = new ProcessDatabase();
        public formGV()
        {
            InitializeComponent();
            this.Load += formGV_Load;
            dgvGV.Click += dgvGV_Click;
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += BtnXoa_Click;
        }

        private void formGV_Load(object sender, EventArgs e)
        {
            HienThiDuLieuGiangVien(); // load 1 lần chuẩn
            ResetValue();
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa giảng viên có mã: " + tbMaGV.Text + " không?",
                "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    string sql = "DELETE FROM GiangVien WHERE MaGV = '" + tbMaGV.Text.Trim() + "'";
                    processDatabase.CapNhatDuLieu(sql);

                    // Refresh lại grid
                    HienThiDuLieuGiangVien();
                    ResetValue();

                    MessageBox.Show("Xóa giảng viên thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (System.Data.SqlClient.SqlException sqlEx)
                {
                    if (sqlEx.Number == 547)
                        MessageBox.Show("Không thể xóa do ràng buộc khóa ngoại.", "Lỗi Khóa Ngoại");
                    else
                        MessageBox.Show("Lỗi SQL: " + sqlEx.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }


        private void tbTimKiemTheoTen_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (tbMaGV.Text.Trim() == "" || tbHoTenGV.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải nhập đầy đủ dữ liệu", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Kiểm tra trùng mã
            DataTable dt = processDatabase.DocBang("SELECT * FROM SanPham WHERE MaSP = '" + tbMaGV.Text + "'");
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("Mã sản phẩm này đã tồn tại, hãy nhập mã khác!", "Thông báo");
                return;
            }

            // Thực hiện thêm
            if (MessageBox.Show("Bạn có muốn update sản phẩm có mã: " + tbMaGV.Text + " không?",
                "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                processDatabase.CapNhatDuLieu(
                "INSERT INTO SanPham (MaSP, TenSP) VALUES ('" + tbMaGV.Text + "', N'" + tbHoTenGV.Text + "')");

                dgvGV.DataSource = processDatabase.DocBang("SELECT * FROM SanPham");
                ResetValue();

                btnXoa.Enabled = false;
                btnSua.Enabled = false;
                btnThem.Enabled = true;

            }
        }
        void ResetValue()
        {
            tbMaGV.Text = "";
            tbHoTenGV.Text = "";
            // Thêm reset cho các trường khác nếu bạn muốn (Ngaysinh, DiaChi, Email,...)
            tbMaGV.Enabled = true;

            // Thiết lập lại trạng thái nút
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void HienThiDuLieuGiangVien(string filter = "")
        {
            string sql = "SELECT * FROM GiangVien";
            if (!string.IsNullOrEmpty(filter))
                sql += " WHERE " + filter;

            DataTable dt = processDatabase.DocBang(sql);
            dgvGV.DataSource = dt;

            if (dgvGV.Columns.Count >= 6)
            {
                dgvGV.Columns[0].HeaderText = "ID";
                dgvGV.Columns[1].HeaderText = "Mã GV";
                dgvGV.Columns[2].HeaderText = "Tên GV";
                dgvGV.Columns[3].HeaderText = "Học vị";
                dgvGV.Columns[4].HeaderText = "Mã Khoa";
                dgvGV.Columns[5].HeaderText = "Mã TK";
            }
        }


        private void btnSua_Click(object sender, EventArgs e)
        {

        }

        private void dgvGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvGV_Click(object sender, EventArgs e)
        {
            if (dgvGV.CurrentRow != null)
            {
                tbMaGV.Text = dgvGV.CurrentRow.Cells[0].Value.ToString();
                tbHoTenGV.Text = dgvGV.CurrentRow.Cells[1].Value.ToString();
                tbMaGV.Enabled = true;
                tbHoTenGV.Enabled = true;
                btnXoa.Enabled = true;
                btnSua.Enabled = true;
                btnThem.Enabled = true;
            }
        }

        private void btnRefresh_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
