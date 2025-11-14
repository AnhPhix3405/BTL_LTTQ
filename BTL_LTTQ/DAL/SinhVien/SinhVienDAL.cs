// DAL/SinhVienDAL.cs
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BTL_LTTQ.DTO;

namespace BTL_LTTQ.DAL
{
    public class SinhVienDAL
    {
        // LẤY TẤT CẢ SINH VIÊN (HOẶC THEO LỌC)
        public DataTable TimKiem(string maKhoa, string maLop, string maSV)
        {
            string query = @"
                SELECT DISTINCT 
                    sv.MaSV, sv.TenSV, sv.NgaySinh, sv.GioiTinh, sv.QueQuan, 
                    sv.SDT, sv.Email, k.TenKhoa, ISNULL(ltc.TenLop, N'') AS TenLop
                FROM SinhVien sv
                INNER JOIN Khoa k ON sv.MaKhoa = k.MaKhoa
                LEFT JOIN Diem d ON sv.MaSV = d.MaSV
                LEFT JOIN LopTinChi ltc ON d.MaLop = ltc.MaLop
                WHERE 1=1";

            var parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(maKhoa))
            {
                query += " AND sv.MaKhoa = @MaKhoa";
                parameters.Add(new SqlParameter("@MaKhoa", maKhoa));
            }

            if (!string.IsNullOrEmpty(maLop))
            {
                query += " AND d.MaLop = @MaLop";
                parameters.Add(new SqlParameter("@MaLop", maLop));
            }

            if (!string.IsNullOrEmpty(maSV))
            {
                query += " AND sv.MaSV LIKE @MaSV";
                parameters.Add(new SqlParameter("@MaSV", $"%{maSV}%"));
            }

            return DatabaseConnection.ExecuteQuery(query, parameters.ToArray());
        }

        public DataTable LayKhoa()
        {
            return DatabaseConnection.ExecuteQuery("SELECT MaKhoa, TenKhoa FROM Khoa");
        }

        public DataTable LayTatCaLopTinChi()
        {
            return DatabaseConnection.ExecuteQuery("SELECT MaLop, TenLop FROM LopTinChi");
        }

        public DataTable LayLopTinChiTheoKhoa(string maKhoa)
        {
            string query = @"
                SELECT DISTINCT ltc.MaLop, ltc.TenLop
                FROM LopTinChi ltc
                INNER JOIN MonHoc mh ON ltc.MaMH = mh.MaMH
                WHERE mh.MaKhoa = @MaKhoa";
            return DatabaseConnection.ExecuteQuery(query, new[] { new SqlParameter("@MaKhoa", maKhoa) });
        }

        // THÊM SINH VIÊN VÀO LỚP TÍN CHỈ
        public bool Them(SinhVienDTO sv)
        {
            // Kiểm tra trùng SV trong lớp
            string check = "SELECT COUNT(*) FROM Diem WHERE MaSV = @MaSV AND MaLop = @MaLop";
            if ((int)DatabaseConnection.ExecuteScalar(check, new[]
            {
                new SqlParameter("@MaSV", sv.MaSV),
                new SqlParameter("@MaLop", sv.MaLop)
            }) > 0) return false;

            // Nếu SV chưa tồn tại → thêm mới
            bool svMoi = (int)DatabaseConnection.ExecuteScalar(
                "SELECT COUNT(*) FROM SinhVien WHERE MaSV = @MaSV",
                new[] { new SqlParameter("@MaSV", sv.MaSV) }) == 0;

            if (svMoi)
            {
                string insertSV = @"
                    INSERT INTO SinhVien (MaSV, TenSV, NgaySinh, GioiTinh, QueQuan, SDT, Email, MaKhoa)
                    VALUES (@MaSV, @TenSV, @NgaySinh, @GioiTinh, @QueQuan, @SDT, @Email, @MaKhoa)";

                var pSV = new[]
                {
                    new SqlParameter("@MaSV", sv.MaSV),
                    new SqlParameter("@TenSV", sv.TenSV),
                    new SqlParameter("@NgaySinh", sv.NgaySinh),
                    new SqlParameter("@GioiTinh", sv.GioiTinh ?? (object)DBNull.Value),
                    new SqlParameter("@QueQuan", sv.QueQuan ?? (object)DBNull.Value),
                    new SqlParameter("@SDT", sv.SDT ?? (object)DBNull.Value),
                    new SqlParameter("@Email", sv.Email ?? (object)DBNull.Value),
                    new SqlParameter("@MaKhoa", sv.MaKhoa)
                };

                if (DatabaseConnection.ExecuteNonQuery(insertSV, pSV) <= 0)
                    return false;
            }

            // Thêm vào lớp tín chỉ
            string insertDiem = "INSERT INTO Diem (MaSV, MaLop) VALUES (@MaSV, @MaLop)";
            return DatabaseConnection.ExecuteNonQuery(insertDiem, new[]
            {
                new SqlParameter("@MaSV", sv.MaSV),
                new SqlParameter("@MaLop", sv.MaLop)
            }) > 0;
        }

        // SỬA THÔNG TIN CÁ NHÂN (KHÔNG ĐỔI LỚP)
        public bool Sua(SinhVienDTO sv)
        {
            string query = @"
                UPDATE SinhVien SET 
                    TenSV = @TenSV, NgaySinh = @NgaySinh, GioiTinh = @GioiTinh,
                    QueQuan = @QueQuan, SDT = @SDT, Email = @Email
                WHERE MaSV = @MaSV";

            var p = new[]
            {
                new SqlParameter("@MaSV", sv.MaSV),
                new SqlParameter("@TenSV", sv.TenSV),
                new SqlParameter("@NgaySinh", sv.NgaySinh),
                new SqlParameter("@GioiTinh", sv.GioiTinh ?? (object)DBNull.Value),
                new SqlParameter("@QueQuan", sv.QueQuan ?? (object)DBNull.Value),
                new SqlParameter("@SDT", sv.SDT ?? (object)DBNull.Value),
                new SqlParameter("@Email", sv.Email ?? (object)DBNull.Value)
            };

            return DatabaseConnection.ExecuteNonQuery(query, p) > 0;
        }

        // DAL/SinhVienDAL.cs
        public bool Xoa(string maSV, string maLop)
        {
            try
            {
                int result = DatabaseConnection.ExecuteNonQuery(
                    "DELETE FROM Diem WHERE MaSV = @MaSV AND MaLop = @MaLop",
                    new[] { new SqlParameter("@MaSV", maSV), new SqlParameter("@MaLop", maLop) });

                // Nếu SV không còn lớp nào → xóa luôn
                if ((int)DatabaseConnection.ExecuteScalar("SELECT COUNT(*) FROM Diem WHERE MaSV = @MaSV",
                    new[] { new SqlParameter("@MaSV", maSV) }) == 0)
                {
                    DatabaseConnection.ExecuteNonQuery("DELETE FROM SinhVien WHERE MaSV = @MaSV",
                        new[] { new SqlParameter("@MaSV", maSV) });
                }

                return result > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool KiemTraSVTrongLop(string maSV, string maLop)
        {
            string query = "SELECT COUNT(*) FROM Diem WHERE MaSV = @MaSV AND MaLop = @MaLop";
            var result = DatabaseConnection.ExecuteScalar(query,
                new[] { new SqlParameter("@MaSV", maSV), new SqlParameter("@MaLop", maLop) });
            return Convert.ToInt32(result) > 0;
        }

        public SinhVienDTO LaySinhVienTheoMa(string maSV)
        {
            string query = "SELECT * FROM SinhVien WHERE MaSV = @MaSV";
            var dt = DatabaseConnection.ExecuteQuery(query, new[] { new SqlParameter("@MaSV", maSV) });
            if (dt.Rows.Count == 0) return null;

            var row = dt.Rows[0];
            return new SinhVienDTO
            {
                MaSV = row["MaSV"].ToString(),
                TenSV = row["TenSV"].ToString(),
                NgaySinh = Convert.ToDateTime(row["NgaySinh"]),
                GioiTinh = row["GioiTinh"].ToString(),
                QueQuan = row["QueQuan"].ToString(),
                SDT = row["SDT"].ToString(),
                Email = row["Email"].ToString(),
                MaKhoa = row["MaKhoa"].ToString()
            };
        }
    }
}