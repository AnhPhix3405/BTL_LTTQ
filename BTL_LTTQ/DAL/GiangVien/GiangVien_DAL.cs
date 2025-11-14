// DAL/GiangVien_DAL.cs
using BTL_LTTQ.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BTL_LTTQ.DAL
{
    public class GiangVien_DAL
    {
        /// <summary>
        /// Lấy toàn bộ Giảng viên
        /// </summary>
        public DataTable GetData()
        {
            string query = "SELECT MaGV, TenGV, GioiTinh, NgaySinh, DiaChi, SDT, Email, HocHam, HocVi, MaKhoa FROM GiangVien";
            return DatabaseConnection.ExecuteQuery(query);
        }

        /// <summary>
        /// Tìm kiếm Giảng viên
        /// </summary>
        public DataTable SearchData(string tuKhoa, string maKhoa)
        {
            string query = "SELECT MaGV, TenGV, GioiTinh, NgaySinh, DiaChi, SDT, Email, HocHam, HocVi, MaKhoa FROM GiangVien WHERE 1=1";
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                query += " AND (TenGV LIKE @TuKhoa OR MaGV LIKE @TuKhoa OR Email LIKE @TuKhoa)";
                parameters.Add(new SqlParameter("@TuKhoa", "%" + tuKhoa + "%"));
            }
            if (!string.IsNullOrWhiteSpace(maKhoa))
            {
                query += " AND MaKhoa = @MaKhoa";
                parameters.Add(new SqlParameter("@MaKhoa", maKhoa));
            }
            return DatabaseConnection.ExecuteQuery(query, parameters.ToArray());
        }

        /// <summary>
        /// Thêm Giảng viên
        /// </summary>
        public int Insert(GiangVien_DTO gv)
        {
            string query = @"INSERT INTO GiangVien (MaGV, TenGV, GioiTinh, NgaySinh, DiaChi, SDT, Email, HocHam, HocVi, MaKhoa) 
                             VALUES (@MaGV, @TenGV, @GioiTinh, @NgaySinh, @DiaChi, @SDT, @Email, @HocHam, @HocVi, @MaKhoa)";

            SqlParameter[] parameters = {
                new SqlParameter("@MaGV", gv.MaGV),
                new SqlParameter("@TenGV", gv.TenGV),
                new SqlParameter("@GioiTinh", (object)gv.GioiTinh ?? DBNull.Value),
                new SqlParameter("@NgaySinh", (object)gv.NgaySinh ?? DBNull.Value),
                new SqlParameter("@DiaChi", (object)gv.DiaChi ?? DBNull.Value),
                new SqlParameter("@SDT", (object)gv.SDT ?? DBNull.Value),
                new SqlParameter("@Email", (object)gv.Email ?? DBNull.Value),
                new SqlParameter("@HocHam", (object)gv.HocHam ?? DBNull.Value),
                new SqlParameter("@HocVi", (object)gv.HocVi ?? DBNull.Value),
                new SqlParameter("@MaKhoa", (object)gv.MaKhoa ?? DBNull.Value)
            };
            return DatabaseConnection.ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// Cập nhật Giảng viên
        /// </summary>
        public int Update(GiangVien_DTO gv)
        {
            string query = @"UPDATE GiangVien SET 
                                TenGV = @TenGV, GioiTinh = @GioiTinh, NgaySinh = @NgaySinh, DiaChi = @DiaChi, 
                                SDT = @SDT, Email = @Email, HocHam = @HocHam, HocVi = @HocVi, MaKhoa = @MaKhoa 
                             WHERE MaGV = @MaGV";

            SqlParameter[] parameters = {
                new SqlParameter("@TenGV", gv.TenGV),
                new SqlParameter("@GioiTinh", (object)gv.GioiTinh ?? DBNull.Value),
                new SqlParameter("@NgaySinh", (object)gv.NgaySinh ?? DBNull.Value),
                new SqlParameter("@DiaChi", (object)gv.DiaChi ?? DBNull.Value),
                new SqlParameter("@SDT", (object)gv.SDT ?? DBNull.Value),
                new SqlParameter("@Email", (object)gv.Email ?? DBNull.Value),
                new SqlParameter("@HocHam", (object)gv.HocHam ?? DBNull.Value),
                new SqlParameter("@HocVi", (object)gv.HocVi ?? DBNull.Value),
                new SqlParameter("@MaKhoa", (object)gv.MaKhoa ?? DBNull.Value),
                new SqlParameter("@MaGV", gv.MaGV) // Tham số cho WHERE
            };
            return DatabaseConnection.ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// Xóa Giảng viên
        /// </summary>
        public int Delete(string maGV)
        {
            string query = "DELETE FROM GiangVien WHERE MaGV = @MaGV";
            SqlParameter[] param = { new SqlParameter("@MaGV", maGV) };
            return DatabaseConnection.ExecuteNonQuery(query, param);
        }

        /// <summary>
        /// Xóa dữ liệu liên quan (Tài khoản)
        /// </summary>
        public void DeleteTaiKhoanByMaGV(string maGV)
        {
            string query = "DELETE FROM TaiKhoan WHERE MaGV = @MaGV";
            SqlParameter[] param = { new SqlParameter("@MaGV", maGV) };
            DatabaseConnection.ExecuteNonQuery(query, param);
        }

        /// <summary>
        /// Xóa dữ liệu liên quan (Phân công)
        /// </summary>
        public void DeletePhanCongByMaGV(string maGV)
        {
            string query = "DELETE FROM PhanCongGiangDay WHERE MaGV = @MaGV";
            SqlParameter[] param = { new SqlParameter("@MaGV", maGV) };
            DatabaseConnection.ExecuteNonQuery(query, param);
        }
    }


}