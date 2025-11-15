// BLL/PhanCongBLL.cs
using BTL_LTTQ.DAL;
using BTL_LTTQ.DTO;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BTL_LTTQ.BLL
{
    public class PhanCongBLL
    {
        private PhanCongDAL dal = new PhanCongDAL();

        public DataTable LayTatCa() => dal.LayTatCa();

        public bool Them(PhanCongDTO pc)
        {
            if (dal.KiemTraLopDaPhanCong(pc.MaLop))
            {
                return false;
            }

            bool result = dal.Them(pc);
            
            if (result)
            {
                dal.CapNhatTinhTrangLop(pc.MaLop, true);
            }

            return result;
        }

        public bool Sua(PhanCongDTO pc, string maLopCu)
        {
            if (pc.MaLop != maLopCu && dal.KiemTraLopDaPhanCong(pc.MaLop))
            {
                return false;
            }

            bool result = dal.Sua(pc, maLopCu);

            if (result && pc.MaLop != maLopCu)
            {
                dal.CapNhatTinhTrangLop(maLopCu, false);
                dal.CapNhatTinhTrangLop(pc.MaLop, true);
            }

            return result;
        }

        public bool Xoa(string maPC, string maLop)
        {
            bool result = dal.Xoa(maPC, maLop);

            if (result)
            {
                dal.CapNhatTinhTrangLop(maLop, false);
            }

            return result;
        }

        public bool KiemTraMaPCTrung(string maPC) => dal.KiemTraMaPCTrung(maPC);

        // ✅ SỬA: Đổi TimeSpan → byte
        public bool KiemTraTrungLichGV(string maGV, DateTime? ngayBD, DateTime? ngayKT, byte thu, byte caHoc, string maPC = null)
            => dal.KiemTraTrungLichGV(maGV, ngayBD, ngayKT, thu, caHoc, maPC);

        // ✅ SỬA: Đổi TimeSpan → byte
        public bool KiemTraTrungPhong(string maPhong, DateTime? ngayBD, DateTime? ngayKT, byte thu, byte caHoc, string maPC = null)
            => dal.KiemTraTrungPhong(maPhong, ngayBD, ngayKT, thu, caHoc, maPC);

        public bool KiemTraLopDaPhanCong(string maLop) => dal.KiemTraLopDaPhanCong(maLop);

        // === CÁC PHƯƠNG THỨC HỖ TRỢ ===
        public DataTable LayKhuVuc()
        {
            string query = "SELECT MaKhuVuc, TenKhuVuc FROM KhuVuc";
            return DatabaseConnection.ExecuteQuery(query);
        }

        public DataTable LayPhongTheoKhuVuc(string maKhuVuc)
        {
            string query = "SELECT MaPhong FROM PhongHoc WHERE MaKhuVuc = @MaKhuVuc";
            return DatabaseConnection.ExecuteQuery(query, new[] { new SqlParameter("@MaKhuVuc", maKhuVuc) });
        }

        public DataTable LayKhoa()
        {
            string query = "SELECT MaKhoa, TenKhoa FROM Khoa";
            return DatabaseConnection.ExecuteQuery(query);
        }

        public DataTable LayMonHocTheoKhoa(string maKhoa)
        {
            string query = "SELECT MaMH, TenMH FROM MonHoc WHERE MaKhoa = @MaKhoa";
            return DatabaseConnection.ExecuteQuery(query, new[] { new SqlParameter("@MaKhoa", maKhoa) });
        }

        public DataTable LayMonHoc()
        {
            string query = "SELECT MaMH, TenMH FROM MonHoc";
            return DatabaseConnection.ExecuteQuery(query);
        }

        public DataTable LayGiangVien()
        {
            string query = "SELECT MaGV, TenGV FROM GiangVien";
            return DatabaseConnection.ExecuteQuery(query);
        }

        public DataTable LayLopTinChiChuaPhanCong(string maMH)
        {
            string query = @"
                SELECT ltc.MaLop, ltc.TenLop 
                FROM LopTinChi ltc
                WHERE ltc.MaMH = @MaMH
                  AND ltc.TinhTrang = 0";
            return DatabaseConnection.ExecuteQuery(query, new[] { new SqlParameter("@MaMH", maMH) });
        }

        public DataTable LayLopTinChiDaPhanCong(string maMH)
        {
            string query = @"
                SELECT ltc.MaLop, ltc.TenLop 
                FROM LopTinChi ltc
                WHERE ltc.MaMH = @MaMH
                  AND ltc.TinhTrang = 1";
            return DatabaseConnection.ExecuteQuery(query, new[] { new SqlParameter("@MaMH", maMH) });
        }
    }
}