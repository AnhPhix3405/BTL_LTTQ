using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTL_LTTQ.DAL.Diem
{
    internal class DiemDAL
    {
        private string connectionString = "Data Source=LAPTOP-LHTIMU8S;Initial Catalog=QuanLySanPham;Integrated Security=True;";
        private SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
        public DataTable GetAll()
        {
            using (SqlConnection conn=GetConnection())
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Diem", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        public bool Insert(Diem diem)
        {
            using (SqlConnection conn = GetConnection())
            {
                string query = "INSERT INTO Diem (MaLop, MaSV,DiemCc,DiemGK,DiemThi) VALUES (@MaLop, @MaSV,@DiemCc,@DiemGK,@DiemThi)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaLop", diem.MaLop);
                cmd.Parameters.AddWithValue("@MaSV", diem.MaSV);
                cmd.Parameters.AddWithValue("@DiemCc", diem.DiemCc);
                cmd.Parameters.AddWithValue("@DiemGK", diem.DiemGK);
                cmd.Parameters.AddWithValue("@DiemThi", diem.DiemThi);              
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public bool Update(Diem diem)//chua fix
        {
            using (SqlConnection conn = GetConnection())
            {
                string query = "UPDATE SanPham SET DiemCc = @DiemCc, DiemGK=@DiemGK, DiemThi=@DiemThi WHERE MaSV = @MaSV";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DiemCc", diem.DiemCc);
                cmd.Parameters.AddWithValue("@DiemGK", diem.DiemGK);
                cmd.Parameters.AddWithValue("@DiemThi", diem.DiemThi);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public bool Delete(string MaSV)
        {
            using (SqlConnection conn = GetConnection())
            {
                string query = "DELETE FROM Diem WHERE MaSV = @MaSV";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSV", MaSV);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public bool CheckExist(string MaSV)
        {
            using (SqlConnection conn = GetConnection())
            {
                string query = "SELECT COUNT(*) FROM Diem WHERE MaSV = @MaSV";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSV", MaSV);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

    }
}
