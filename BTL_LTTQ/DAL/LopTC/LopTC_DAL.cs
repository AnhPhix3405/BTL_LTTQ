// DAL/LopTC_DAL.cs
using BTL_LTTQ.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BTL_LTTQ.DAL
{
    // Class: LopTC_DAL (Theo format)
    public class LopTC_DAL
    {
        public DataTable GetData()
        {
            string query = @"
                SELECT 
                    LTC.MaLop, LTC.MaMH, MH.TenMH, MH.MaKhoa, 
                    LTC.HocKy, LTC.NamHoc, LTC.TinhTrang 
                FROM LopTinChi AS LTC
                JOIN MonHoc AS MH ON LTC.MaMH = MH.MaMH";
            return DatabaseConnection.ExecuteQuery(query);
        }

        public DataTable SearchData(string maLop, int? namHoc, string maKhoa)
        {
            string baseQuery = @"
                SELECT 
                    LTC.MaLop, LTC.MaMH, MH.TenMH, MH.MaKhoa, 
                    LTC.HocKy, LTC.NamHoc, LTC.TinhTrang 
                FROM LopTinChi AS LTC
                JOIN MonHoc AS MH ON LTC.MaMH = MH.MaMH
                WHERE 1=1";

            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(maLop))
            {
                baseQuery += " AND (LTC.MaLop LIKE @MaLop)";
                parameters.Add(new SqlParameter("@MaLop", "%" + maLop + "%"));
            }
            if (namHoc.HasValue)
            {
                baseQuery += " AND LTC.NamHoc = @NamHoc";
                parameters.Add(new SqlParameter("@NamHoc", namHoc.Value));
            }
            if (!string.IsNullOrWhiteSpace(maKhoa))
            {
                baseQuery += " AND MH.MaKhoa = @MaKhoa";
                parameters.Add(new SqlParameter("@MaKhoa", maKhoa));
            }

            return DatabaseConnection.ExecuteQuery(baseQuery, parameters.ToArray());
        }

        public int Insert(LopTC_DTO ltc) // Dùng LopTC_DTO
        {
            string query = @"INSERT INTO LopTinChi (MaLop, MaMH, HocKy, NamHoc, TinhTrang) 
                             VALUES (@MaLop, @MaMH, @HocKy, @NamHoc, @TinhTrang)";

            SqlParameter[] parameters = {
                new SqlParameter("@MaLop", ltc.MaLop),
                new SqlParameter("@MaMH", ltc.MaMH),
                new SqlParameter("@HocKy", (object)ltc.HocKy ?? DBNull.Value),
                new SqlParameter("@NamHoc", (object)ltc.NamHoc ?? DBNull.Value),
                new SqlParameter("@TinhTrang", ltc.TinhTrang)
            };
            return DatabaseConnection.ExecuteNonQuery(query, parameters);
        }

        public int Update(LopTC_DTO ltc) // Dùng LopTC_DTO
        {
            string query = @"UPDATE LopTinChi SET 
                                MaMH = @MaMH, HocKy = @HocKy, 
                                NamHoc = @NamHoc, TinhTrang = @TinhTrang
                             WHERE MaLop = @MaLop";

            SqlParameter[] parameters = {
                new SqlParameter("@MaMH", ltc.MaMH),
                new SqlParameter("@HocKy", (object)ltc.HocKy ?? DBNull.Value),
                new SqlParameter("@NamHoc", (object)ltc.NamHoc ?? DBNull.Value),
                new SqlParameter("@TinhTrang", ltc.TinhTrang),
                new SqlParameter("@MaLop", ltc.MaLop)
            };
            return DatabaseConnection.ExecuteNonQuery(query, parameters);
        }

        public int Delete(string maLop)
        {
            string query = "DELETE FROM LopTinChi WHERE MaLop = @MaLop";
            SqlParameter[] param = { new SqlParameter("@MaLop", maLop) };
            return DatabaseConnection.ExecuteNonQuery(query, param);
        }

        public void DeletePhanCongByMaLop(string maLop)
        {
            string query = "DELETE FROM PhanCongGiangDay WHERE MaLop = @MaLop";
            SqlParameter[] param = { new SqlParameter("@MaLop", maLop) };
            DatabaseConnection.ExecuteNonQuery(query, param);
        }

        public void DeleteDiemByMaLop(string maLop)
        {
            string query = "DELETE FROM Diem WHERE MaLop = @MaLop";
            SqlParameter[] param = { new SqlParameter("@MaLop", maLop) };
            DatabaseConnection.ExecuteNonQuery(query, param);
        }
    }
}