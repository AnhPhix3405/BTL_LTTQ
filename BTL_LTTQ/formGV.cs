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
            dgvGV.DataSource = processDatabase.DocBang("SELECT * FROM GiangVien");

            dgvGV.Columns[0].HeaderText = "Tên Giảng viên";
            dgvGV.Columns[1].HeaderText = "Mã Giảng viên";
            dgvGV.Columns[2].HeaderText = "Tên Giảng viên";
            dgvGV.Columns[3].HeaderText = "Học vi";
            dgvGV.Columns[4].HeaderText = "Mã khoa";
            dgvGV.Columns[5].HeaderText = "Mã TK";


            dgvGV.Columns[0].Width = 100;
            dgvGV.Columns[1].Width = 200;
            dgvGV.Columns[2].Width = 200;
            dgvGV.Columns[3].Width = 200;
            dgvGV.Columns[4].Width = 200;
            dgvGV.Columns[5].Width = 100;

            dgvGV.BackgroundColor = Color.LightBlue;

            btnXoa.Enabled = false;
            btnSua.Enabled = false;
            btnThem.Enabled = true;
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa sản phẩm có mã: " + tbMaGV.Text + " không?",
                "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                processDatabase.CapNhatDuLieu(
                    "DELETE FROM GiangVien WHERE MaGV = '" + tbMaGV.Text + "'");

                dgvGV.DataSource = processDatabase.DocBang("SELECT * FROM SanPham");
                ResetValue();

                btnXoa.Enabled = false;
                btnSua.Enabled = false;
                btnThem.Enabled = true;
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
            tbMaGV.Enabled = true;
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

        
    }
}
