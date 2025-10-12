using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BTL_LTTQ.DAL;
using BTL_LTTQ.BLL.Session;

namespace BTL_LTTQ.BLL.Login
{
    public class LoginBLL
    {
        private LoginDAL loginDAL;

        public LoginBLL()
        {
            loginDAL = new LoginDAL();
        }

        // Phương thức chính xử lý đăng nhập
        public bool ProcessLogin(string tenDangNhap, string matKhau, Form currentForm)
        {
            try
            {
                // Validate input
                if (!ValidateInput(tenDangNhap, matKhau))
                {
                    return false;
                }

                // Kiểm tra đăng nhập qua DAL
                bool isValidLogin = loginDAL.Login(tenDangNhap, matKhau);

                if (isValidLogin)
                {
                    // Lấy thông tin user từ database
                    DataRow userInfo = loginDAL.GetUserInfo(tenDangNhap, matKhau);

                    if (userInfo != null)
                    {
                        // Lưu thông tin vào session
                        SaveUserSession(userInfo);

                        // Điều hướng sang FormMain
                        NavigateToMainForm(currentForm);

                        return true;
                    }
                    else
                    {
                        ShowErrorMessage("Không thể lấy thông tin tài khoản!");
                        return false;
                    }
                }
                else
                {
                    ShowErrorMessage("Tên đăng nhập hoặc mật khẩu không đúng!");
                    return false;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi khi đăng nhập: " + ex.Message);
                return false;
            }
        }

        // Validate input data
        private bool ValidateInput(string tenDangNhap, string matKhau)
        {
            if (string.IsNullOrEmpty(tenDangNhap) || tenDangNhap.Trim() == "User name")
            {
                ShowErrorMessage("Vui lòng nhập tên đăng nhập!");
                return false;
            }

            if (string.IsNullOrEmpty(matKhau))
            {
                ShowErrorMessage("Vui lòng nhập mật khẩu!");
                return false;
            }

            if (tenDangNhap.Trim().Length < 3)
            {
                ShowErrorMessage("Tên đăng nhập phải có ít nhất 3 ký tự!");
                return false;
            }

            if (matKhau.Length < 3)
            {
                ShowErrorMessage("Mật khẩu phải có ít nhất 3 ký tự!");
                return false;
            }

            return true;
        }

        // Lưu thông tin user vào session
        private void SaveUserSession(DataRow userInfo)
        {
            int id = Convert.ToInt32(userInfo["Id"]);
            string tenDangNhap = userInfo["TenDangNhap"].ToString();
            string loaiTaiKhoan = userInfo["LoaiTaiKhoan"].ToString();

            UserSession.SetUserSession(id, tenDangNhap, loaiTaiKhoan);
        }

        // Điều hướng sang FormMain
        private void NavigateToMainForm(Form currentForm)
        {
            try
            {
                // Tạo và hiển thị FormMain
                BTL_LTTQ.FormMain mainForm = new BTL_LTTQ.FormMain();
                mainForm.Show();

                // Ẩn form login hiện tại
                currentForm.Hide();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi khi chuyển form: " + ex.Message);
            }
        }

        // Hiển thị thông báo lỗi
        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // Hiển thị thông báo thành công
        private void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Kiểm tra tài khoản có tồn tại không
        public bool CheckUserExists(string tenDangNhap)
        {
            try
            {
                return loginDAL.CheckUserExists(tenDangNhap);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi khi kiểm tra tài khoản: " + ex.Message);
                return false;
            }
        }

        // Đăng xuất
        public void Logout(Form currentForm)
        {
            try
            {
                // Clear session
                UserSession.ClearSession();

                // Quay về form login
                BTL_LTTQ.Login loginForm = new BTL_LTTQ.Login();
                loginForm.Show();

                // Đóng form hiện tại
                currentForm.Close();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi khi đăng xuất: " + ex.Message);
            }
        }
    }
}
