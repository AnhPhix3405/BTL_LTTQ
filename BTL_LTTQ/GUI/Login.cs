using BTL_LTTQ.BLL;
using BTL_LTTQ.BLL.Login;
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
    public partial class Login : Form
    {
        private LoginBLL loginBLL;

        public Login()
        {
            InitializeComponent();
            loginBLL = new LoginBLL();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            // Thiết lập các sự kiện
            SetupEvents();

            // Thiết lập placeholder text
            SetupPlaceholderText();
        }

        private void SetupEvents()
        {
            // Sự kiện click button đăng nhập
            btnLogin.Click += BtnLogin_Click;

            // Sự kiện click label đóng
            lbClose.Click += LbClose_Click;

            // Sự kiện Enter để đăng nhập
            tbPassword.KeyPress += TbPassword_KeyPress;

            // Sự kiện focus textbox để xóa placeholder
            tbUserName.Enter += TbUserName_Enter;
            tbUserName.Leave += TbUserName_Leave;
        }

        private void SetupPlaceholderText()
        {
            // Thiết lập màu sắc placeholder
            if (tbUserName.Text == "User name")
            {
                tbUserName.ForeColor = Color.Gray;
            }
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string tenDangNhap = tbUserName.Text.Trim();
            string matKhau = tbPassword.Text;

            // Xử lý đăng nhập thông qua BLL
            bool loginSuccess = loginBLL.ProcessLogin(tenDangNhap, matKhau, this);

            if (loginSuccess)
            {
                // Đăng nhập thành công - BLL đã xử lý điều hướng
                // Form sẽ được ẩn trong BLL
            }
            // Nếu thất bại - BLL đã hiển thị thông báo lỗi
        }

        private void LbClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void TbPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Nhấn Enter để đăng nhập
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnLogin_Click(sender, e);
            }
        }

        private void TbUserName_Enter(object sender, EventArgs e)
        {
            // Xóa placeholder text khi focus
            if (tbUserName.Text == "User name")
            {
                tbUserName.Text = "";
                tbUserName.ForeColor = Color.White;
            }
        }

        private void TbUserName_Leave(object sender, EventArgs e)
        {
            // Hiện lại placeholder nếu rỗng
            if (string.IsNullOrEmpty(tbUserName.Text))
            {
                tbUserName.Text = "User name";
                tbUserName.ForeColor = Color.Gray;
            }
        }

        private void tbUserName_TextChanged(object sender, EventArgs e)
        {
            // Xử lý khi text thay đổi - có thể để trống hoặc thêm logic validation
        }
    }
}
