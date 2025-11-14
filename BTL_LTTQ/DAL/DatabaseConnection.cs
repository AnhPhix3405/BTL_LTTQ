using System;
using System.Data;
using System.Data.SqlClient;

namespace BTL_LTTQ.DAL
{
    public class DatabaseConnection
    {   
        private static string connectionString = @"Data Source=LAPTOP-3JGSAUFN\SQLEXPRESS01;Initial Catalog=QL_GiangDay;Integrated Security=True;";
        
        // Có thể thêm method để set connection string từ bên ngoài nếu cần
        public static void SetConnectionString(string connString)
        {
            connectionString = connString;
        }
        
        // Phương thức tạo connection mới
        public static SqlConnection GetConnection()
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                return connection;
            }
            catch (Exception ex)
            {
                throw new Exception("Không thể kết nối database: " + ex.Message);
            }
        }
        
        // Phương thức thực thi query có trả về dữ liệu
        public static DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            DataTable dataTable = new DataTable();
            
            using (SqlConnection connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi thực thi query: " + ex.Message);
                }
            }
            
            return dataTable;
        }
        
        // Phương thức thực thi command (Insert, Update, Delete)
        public static int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        
                        return command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Chỉ cần "throw;" để ném lại lỗi gốc
                    throw;
                }
            }
        }
        
        // Phương thức thực thi scalar (trả về 1 giá trị)
        public static object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        
                        return command.ExecuteScalar();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi thực thi scalar: " + ex.Message);
                }
            }
        }
        
        // Phương thức kiểm tra kết nối
        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    connection.Open();
                    return connection.State == ConnectionState.Open;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
