using BTL_LTTQ.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BTL_LTTQ.DAL.Diem
{
    internal class DiemDAL
    {
        private string connectionString = "Data Source=LAPTOP-LHTIMU8S;Initial Catalog=QL_GiangDay;Integrated Security=True;";
        private SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        // Lấy toàn bộ danh sách điểm
        public DataTable GetAll()
        {
            using (SqlConnection conn = GetConnection())
            {
                string query = @"SELECT d.MaSV, sv.TenSV, d.MaLop, d.DiemCC, d.DiemGK, d.DiemThi, d.DiemKTHP
                                 FROM Diem d 
                                 JOIN SinhVien sv ON d.MaSV = sv.MaSV";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // Thêm mới điểm
        public bool Insert(Score diem)
        {
            using (SqlConnection conn = GetConnection())
            {
                // Khi chèn điểm, ta cần tính luôn DiemKTHP dựa trên hệ số trong bảng MonHoc
                string query = @"
                    INSERT INTO Diem (MaLop, MaSV, DiemCC, DiemGK, DiemThi, DiemKTHP)
                    SELECT 
                        @MaLop, 
                        @MaSV, 
                        @DiemCc, 
                        @DiemGK, 
                        @DiemThi,
                        ROUND(((@DiemCc * 0.1 + @DiemGK * 0.9) * mh.HeSoDQT + @DiemThi * mh.HeSoThi), 2)
                    FROM LopTinChi ltc
                    JOIN MonHoc mh ON ltc.MaMH = mh.MaMH
                    WHERE ltc.MaLop = @MaLop";

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

        // Cập nhật điểm
        public bool Update(Score diem)
        {
            using (SqlConnection conn = GetConnection())
            {
                // Khi sửa điểm, tự động tính lại DiemKTHP
                string query = @"
                    UPDATE d
                    SET 
                        d.DiemCC = @DiemCc,
                        d.DiemGK = @DiemGK,
                        d.DiemThi = @DiemThi,
                        d.DiemKTHP = ROUND(((@DiemCc * 0.1 + @DiemGK * 0.9) * mh.HeSoDQT + @DiemThi * mh.HeSoThi), 2)
                    FROM Diem d
                    JOIN LopTinChi ltc ON d.MaLop = ltc.MaLop
                    JOIN MonHoc mh ON ltc.MaMH = mh.MaMH
                    WHERE d.MaLop = @MaLop AND d.MaSV = @MaSV";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DiemCc", diem.DiemCc);
                cmd.Parameters.AddWithValue("@DiemGK", diem.DiemGK);
                cmd.Parameters.AddWithValue("@DiemThi", diem.DiemThi);
                cmd.Parameters.AddWithValue("@MaLop", diem.MaLop);
                cmd.Parameters.AddWithValue("@MaSV", diem.MaSV);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Xóa điểm
        public bool Delete(string MaSV, string MaLop)
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

        // Kiểm tra sinh viên đã có điểm trong lớp chưa
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
        // Tìm kiếm theo mã lớp, mã SV (có thể kết hợp hoặc chỉ 1 trong 2)
        public DataTable Search(string maLop, string maSV)
        {
            using (SqlConnection conn = GetConnection())
            {
                string query = @"SELECT d.MaSV, sv.TenSV, d.MaLop, d.DiemCC, d.DiemGK, d.DiemThi, d.DiemKTHP
                         FROM Diem d
                         JOIN SinhVien sv ON d.MaSV = sv.MaSV
                         WHERE ( @MaLop IS NULL OR @MaLop = '' OR d.MaLop = @MaLop )
                           AND ( @MaSV IS NULL OR @MaSV = '' OR d.MaSV = @MaSV )";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaLop", string.IsNullOrWhiteSpace(maLop) ? (object)DBNull.Value : maLop);
                cmd.Parameters.AddWithValue("@MaSV", string.IsNullOrWhiteSpace(maSV) ? (object)DBNull.Value : maSV);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }


    }
}
