// BLL/LopTC_BLL.cs
using BTL_LTTQ.DAL;
using BTL_LTTQ.DTO;
using System;
using System.Data;

namespace BTL_LTTQ.BLL
{
    // Class: LopTC_BLL (Theo format)
    public class LopTC_BLL
    {
        // Sửa tên các class DAL
        private readonly LopTC_DAL dal = new LopTC_DAL();
        private readonly Khoa_DAL khoaDal = new Khoa_DAL();
        private readonly MonHoc_DAL monHocDal = new MonHoc_DAL();

        public DataTable LoadDanhSachLTC()
        {
            return dal.GetData();
        }

        public DataTable LoadDanhSachKhoa()
        {
            return khoaDal.GetData();
        }

        public DataTable LoadDanhSachMonHoc(string maKhoa)
        {
            return monHocDal.GetDataByKhoa(maKhoa);
        }

        public DataTable TimKiemLTC(string maLop, string namHocStr, string maKhoa)
        {
            int? namHoc = null;
            if (!string.IsNullOrWhiteSpace(namHocStr))
            {
                int tempNamHoc;
                if (int.TryParse(namHocStr, out tempNamHoc))
                {
                    namHoc = tempNamHoc;
                }
                else
                {
                    throw new FormatException("Năm học tìm kiếm phải là một con số.");
                }
            }
            return dal.SearchData(maLop, namHoc, maKhoa);
        }

        public bool ThemLTC(LopTC_DTO ltc) // Dùng LopTC_DTO
        {
            if (string.IsNullOrWhiteSpace(ltc.MaLop))
                return false;

            ltc.TinhTrang = "Chưa phân công";

            int result = dal.Insert(ltc);
            return result > 0;
        }

        public bool SuaLTC(LopTC_DTO ltc) // Dùng LopTC_DTO
        {
            if (string.IsNullOrWhiteSpace(ltc.MaLop))
                return false;

            int result = dal.Update(ltc);
            return result > 0;
        }

        public bool XoaLTC(string maLop)
        {
            if (string.IsNullOrWhiteSpace(maLop))
                return false;

            try
            {
                dal.DeletePhanCongByMaLop(maLop);
                dal.DeleteDiemByMaLop(maLop);

                int result = dal.Delete(maLop);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa Lớp Tín Chỉ: " + ex.Message);
            }
        }
    }
}