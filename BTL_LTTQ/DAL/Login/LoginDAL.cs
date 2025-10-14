using System;
using System.Data;
using System.Data.SqlClient;

namespace BTL_LTTQ.DAL
{
    public class LoginDAL
    {
        // Phương thức kiểm tra đăng nhập
        public bool Login(string tenDangNhap, string matKhau)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap AND MatKhau = @MatKhau";
                
                SqlParameter[] parameters = {
                    new SqlParameter("@TenDangNhap", tenDangNhap),
                    new SqlParameter("@MatKhau", matKhau)
                };
                
                int count = Convert.ToInt32(DatabaseConnection.ExecuteScalar(query, parameters));
                
                return count > 0; // Trả về true nếu tìm thấy tài khoản
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                throw new Exception("Lỗi khi kiểm tra đăng nhập: " + ex.Message);
            }
        }

        // Phương thức lấy thông tin tài khoản sau khi đăng nhập thành công
        public DataRow GetUserInfo(string tenDangNhap, string matKhau)
        {
            try
            {
                string query = "SELECT Id, TenDangNhap, LoaiTaiKhoan FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap AND MatKhau = @MatKhau";
                
                SqlParameter[] parameters = {
                    new SqlParameter("@TenDangNhap", tenDangNhap),
                    new SqlParameter("@MatKhau", matKhau)
                };
                
                DataTable dt = DatabaseConnection.ExecuteQuery(query, parameters);
                
                if (dt.Rows.Count > 0)
                    return dt.Rows[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin tài khoản: " + ex.Message);
            }
        }

        // Phương thức kiểm tra tên đăng nhập có tồn tại không
        public bool CheckUserExists(string tenDangNhap)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap";
                
                SqlParameter[] parameters = {
                    new SqlParameter("@TenDangNhap", tenDangNhap)
                };
                
                int count = Convert.ToInt32(DatabaseConnection.ExecuteScalar(query, parameters));
                
                return count > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi kiểm tra tài khoản: " + ex.Message);
            }
        }
    }
}