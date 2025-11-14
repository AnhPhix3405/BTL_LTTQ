// BLL/GiangVien_BLL.cs
using BTL_LTTQ.DAL;
using BTL_LTTQ.DTO;
using System;
using System.Data;

namespace BTL_LTTQ.BLL
{
    public class GiangVien_BLL
    {
        private readonly GiangVien_DAL dal = new GiangVien_DAL();
        private readonly Khoa_DAL khoaDal = new Khoa_DAL();

        /// <summary>
        /// Lấy danh sách Giảng viên (gọi DAL)
        /// </summary>
        public DataTable LoadDanhSachGV()
        {
            return dal.GetData();
        }

        /// <summary>
        /// Lấy danh sách Khoa (gọi DAL)
        /// </summary>
        public DataTable LoadDanhSachKhoa()
        {
            return khoaDal.GetData();
        }

        /// <summary>
        /// Tìm kiếm Giảng viên (gọi DAL)
        /// </summary>
        public DataTable TimKiemGV(string tuKhoa, string maKhoa)
        {
            return dal.SearchData(tuKhoa, maKhoa);
        }

        /// <summary>
        /// Thêm Giảng viên (gọi DAL)
        /// </summary>
        public bool ThemGV(GiangVien_DTO gv)
        {
            // Kiểm tra nghiệp vụ (ví dụ)
            if (string.IsNullOrWhiteSpace(gv.MaGV))
                return false;

            int result = dal.Insert(gv);
            return result > 0;
        }

        /// <summary>
        /// Sửa Giảng viên (gọi DAL)
        /// </summary>
        public bool SuaGV(GiangVien_DTO gv)
        {
            if (string.IsNullOrWhiteSpace(gv.MaGV))
                return false;

            int result = dal.Update(gv);
            return result > 0;
        }

        /// <summary>
        /// Xóa Giảng viên (Logic nghiệp vụ nằm ở đây)
        /// </summary>
        public bool XoaGV(string maGV)
        {
            if (string.IsNullOrWhiteSpace(maGV))
                return false;

            // Logic: Phải xóa Tài khoản và Phân công trước khi xóa Giảng viên
            try
            {
                dal.DeleteTaiKhoanByMaGV(maGV);
                dal.DeletePhanCongByMaGV(maGV);

                // Xóa Giảng viên
                int result = dal.Delete(maGV);
                return result > 0;
            }
            catch (Exception ex)
            {
                // Ném lỗi ra để GUI bắt
                throw new Exception("Lỗi khi xóa Giảng viên: " + ex.Message);
            }
        }
    }
}