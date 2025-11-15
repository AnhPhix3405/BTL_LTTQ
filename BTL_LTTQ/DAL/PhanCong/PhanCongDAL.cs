// DAL/PhanCongDAL.cs
using BTL_LTTQ.DTO;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BTL_LTTQ.DAL
{
    public class PhanCongDAL
    {
        public DataTable LayTatCa()
        {
            string query = @"
                SELECT 
                    pc.MaPC, pc.NgayPC, pc.NgayBatDau, pc.NgayKetThuc,
                    pc.CaHoc, pc.Thu,
                    pc.MaPhong AS Phong, kv.TenKhuVuc AS Toa,
                    gv.MaGV, gv.TenGV, ltc.MaLop, ltc.TenLop, ltc.MaMH, ltc.NamHoc
                FROM PhanCongGiangDay pc
                INNER JOIN PhongHoc ph ON pc.MaPhong = ph.MaPhong
                INNER JOIN KhuVuc kv ON ph.MaKhuVuc = kv.MaKhuVuc
                INNER JOIN GiangVien gv ON pc.MaGV = gv.MaGV
                INNER JOIN LopTinChi ltc ON pc.MaLop = ltc.MaLop";

            return DatabaseConnection.ExecuteQuery(query);
        }

        public bool Them(PhanCongDTO pc)
        {
            string query = @"
                INSERT INTO PhanCongGiangDay 
                (MaPC, NgayPC, NgayBatDau, NgayKetThuc, CaHoc, Thu, MaPhong, MaGV, MaLop)
                VALUES (@MaPC, @NgayPC, @NgayBatDau, @NgayKetThuc, @CaHoc, @Thu, @MaPhong, @MaGV, @MaLop)";

            var parameters = new[]
            {
                new SqlParameter("@MaPC", pc.MaPC),
                new SqlParameter("@NgayPC", pc.NgayPC ?? (object)DBNull.Value),
                new SqlParameter("@NgayBatDau", pc.NgayBatDau ?? (object)DBNull.Value),
                new SqlParameter("@NgayKetThuc", pc.NgayKetThuc ?? (object)DBNull.Value),
                new SqlParameter("@CaHoc", pc.CaHoc),
                new SqlParameter("@Thu", pc.Thu),
                new SqlParameter("@MaPhong", pc.MaPhong ?? (object)DBNull.Value),
                new SqlParameter("@MaGV", pc.MaGV ?? (object)DBNull.Value),
                new SqlParameter("@MaLop", pc.MaLop ?? (object)DBNull.Value)
            };

            return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool Sua(PhanCongDTO pc, string maLopCu)
        {
            string query = @"
                UPDATE PhanCongGiangDay 
                SET NgayPC = @NgayPC, NgayBatDau = @NgayBatDau, NgayKetThuc = @NgayKetThuc,
                    CaHoc = @CaHoc, Thu = @Thu, MaPhong = @MaPhong, 
                    MaGV = @MaGV, MaLop = @MaLop
                WHERE MaPC = @MaPC";

            var parameters = new[]
            {
                new SqlParameter("@MaPC", pc.MaPC),
                new SqlParameter("@NgayPC", pc.NgayPC ?? (object)DBNull.Value),
                new SqlParameter("@NgayBatDau", pc.NgayBatDau ?? (object)DBNull.Value),
                new SqlParameter("@NgayKetThuc", pc.NgayKetThuc ?? (object)DBNull.Value),
                new SqlParameter("@CaHoc", pc.CaHoc),
                new SqlParameter("@Thu", pc.Thu),
                new SqlParameter("@MaPhong", pc.MaPhong ?? (object)DBNull.Value),
                new SqlParameter("@MaGV", pc.MaGV ?? (object)DBNull.Value),
                new SqlParameter("@MaLop", pc.MaLop ?? (object)DBNull.Value)
            };

            return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool Xoa(string maPC, string maLop)
        {
            string query = "DELETE FROM PhanCongGiangDay WHERE MaPC = @MaPC";
            return DatabaseConnection.ExecuteNonQuery(query, new[] { new SqlParameter("@MaPC", maPC) }) > 0;
        }

        public bool KiemTraMaPCTrung(string maPC)
        {
            string query = "SELECT COUNT(*) FROM PhanCongGiangDay WHERE MaPC = @MaPC";
            return Convert.ToInt32(DatabaseConnection.ExecuteScalar(query, new[] { new SqlParameter("@MaPC", maPC) })) > 0;
        }

        public bool KiemTraLopDaPhanCong(string maLop)
        {
            string query = "SELECT TinhTrang FROM LopTinChi WHERE MaLop = @MaLop";
            var result = DatabaseConnection.ExecuteScalar(query, new[] { new SqlParameter("@MaLop", maLop) });
            return result != null && Convert.ToBoolean(result);
        }

        public void CapNhatTinhTrangLop(string maLop, bool daPhanCong)
        {
            string query = "UPDATE LopTinChi SET TinhTrang = @TinhTrang WHERE MaLop = @MaLop";
            DatabaseConnection.ExecuteNonQuery(query, new[]
            {
                new SqlParameter("@MaLop", maLop),
                new SqlParameter("@TinhTrang", daPhanCong ? 1 : 0)
            });
        }

        public bool KiemTraTrungLichGV(string maGV, DateTime? ngayBD, DateTime? ngayKT, byte thu, byte caHoc, string maPC = null)
        {
            string query = @"
                SELECT COUNT(*) FROM PhanCongGiangDay 
                WHERE MaGV = @MaGV 
                  AND (@MaPC IS NULL OR MaPC <> @MaPC)
                  AND ((@NgayBD BETWEEN NgayBatDau AND NgayKetThuc) 
                       OR (@NgayKT BETWEEN NgayBatDau AND NgayKetThuc)
                       OR (NgayBatDau BETWEEN @NgayBD AND @NgayKT))
                  AND Thu = @Thu
                  AND CaHoc = @CaHoc";

            var parameters = new[]
            {
                new SqlParameter("@MaGV", maGV),
                new SqlParameter("@MaPC", maPC ?? (object)DBNull.Value),
                new SqlParameter("@NgayBD", ngayBD ?? (object)DBNull.Value),
                new SqlParameter("@NgayKT", ngayKT ?? (object)DBNull.Value),
                new SqlParameter("@Thu", thu),
                new SqlParameter("@CaHoc", caHoc)
            };

            return Convert.ToInt32(DatabaseConnection.ExecuteScalar(query, parameters)) > 0;
        }

        public bool KiemTraTrungPhong(string maPhong, DateTime? ngayBD, DateTime? ngayKT, byte thu, byte caHoc, string maPC = null)
        {
            string query = @"
                SELECT COUNT(*) FROM PhanCongGiangDay 
                WHERE MaPhong = @MaPhong 
                  AND (@MaPC IS NULL OR MaPC <> @MaPC)
                  AND ((@NgayBD BETWEEN NgayBatDau AND NgayKetThuc) 
                       OR (@NgayKT BETWEEN NgayBatDau AND NgayKetThuc)
                       OR (NgayBatDau BETWEEN @NgayBD AND @NgayKT))
                  AND Thu = @Thu
                  AND CaHoc = @CaHoc";

            var parameters = new[]
            {
                new SqlParameter("@MaPhong", maPhong),
                new SqlParameter("@MaPC", maPC ?? (object)DBNull.Value),
                new SqlParameter("@NgayBD", ngayBD ?? (object)DBNull.Value),
                new SqlParameter("@NgayKT", ngayKT ?? (object)DBNull.Value),
                new SqlParameter("@Thu", thu),
                new SqlParameter("@CaHoc", caHoc)
            };

            return Convert.ToInt32(DatabaseConnection.ExecuteScalar(query, parameters)) > 0;
        }
    }
}