using System;
using System.Data;
using System.Data.SqlClient;

namespace BTL_LTTQ.DAL
{
    public class LoginDAL
    {
        private string connectionString = @"Data Source=EMPHI\SQLEXPRESS;Initial Catalog=QL_GiangDay;Integrated Security=True";

        // Phương thức kiểm tra đăng nhập
        public bool Login(string tenDangNhap, string matKhau)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    string query = "SELECT COUNT(*) FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap AND MatKhau = @MatKhau";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TenDangNhap", tenDangNhap);
                        command.Parameters.AddWithValue("@MatKhau", matKhau);
                        
                        int count = (int)command.ExecuteScalar();
                        
                        return count > 0; // Trả về true nếu tìm thấy tài khoản
                    }
                }
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
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    string query = "SELECT Id, TenDangNhap, LoaiTaiKhoan FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap AND MatKhau = @MatKhau";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TenDangNhap", tenDangNhap);
                        command.Parameters.AddWithValue("@MatKhau", matKhau);
                        
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        
                        if (dt.Rows.Count > 0)
                            return dt.Rows[0];
                        else
                            return null;
                    }
                }
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
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    string query = "SELECT COUNT(*) FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TenDangNhap", tenDangNhap);
                        
                        int count = (int)command.ExecuteScalar();
                        
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi kiểm tra tài khoản: " + ex.Message);
            }
        }
    }
}