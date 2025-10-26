using BTL_LTTQ.DTO;
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
        private string connectionString = "Data Source=LAPTOP-LHTIMU8S;Initial Catalog=QL_GiangDay;Integrated Security=True;";
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
        public bool Insert(Score diem)
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

        public bool Update(Score diem)
        {
            using (SqlConnection conn = GetConnection())
            {
                string query = "UPDATE Diem SET DiemChuyenCan = @DiemChuyenCan, DiemGiuaKy = @DiemGiuaKy, DiemThi = @DiemThi WHERE MaLop = @MaLop AND MaSV = @MaSV";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@DiemChuyenCan", diem.DiemCc);
                cmd.Parameters.AddWithValue("@DiemGiuaKy", diem.DiemGK);
                cmd.Parameters.AddWithValue("@DiemThi", diem.DiemThi);

                // Them tham so cho dieu kien WHERE
                cmd.Parameters.AddWithValue("@MaLop", diem.MaLop);
                cmd.Parameters.AddWithValue("@MaSV", diem.MaSV);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public bool Delete(string MaSV,string MaLop)
        {
            using (SqlConnection conn = GetConnection())
            {
                string query = "DELETE FROM Diem WHERE MaLop = @MaLop AND MaSV = @MaSV";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSV", MaSV);
                cmd.Parameters.AddWithValue("@MaLop", MaLop);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public bool CheckExist(string MaLop, string MaSV)
        {
            using (SqlConnection conn = GetConnection())
            {
                string query = "SELECT COUNT(*) FROM Diem WHERE MaLop = @MaLop AND MaSV = @MaSV";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaLop", MaLop);
                cmd.Parameters.AddWithValue("@MaSV", MaSV);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

    }
}
