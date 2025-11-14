// Trong file BLL/Diem/DiemBLL.cs
// Sửa lỗi cú pháp, logic nghiệp vụ và cập nhật hàm Xoa/Sua để dùng khóa chính kép

using System;
using System.Data;
using BTL_LTTQ.DAL.Diem;
using BTL_LTTQ.DTO;

namespace BTL_LTTQ.BLL.Diem
{
    internal class DiemBLL
    {
        private DiemDAL dal = new DiemDAL();

        public DataTable LayDanhSach()
        {
            return dal.GetAll();
        }

        public string ThemDiem(Score diem)
        {
            // Bỏ kiem tra TenSV vi da xoa khoi DTO
            if (string.IsNullOrWhiteSpace(diem.MaSV) ||
                string.IsNullOrWhiteSpace(diem.MaLop))
                return "Mã lớp hoặc Mã sinh viên không được để trống.";

            // Kiểm tra giá trị điểm hợp lệ (0-10)
            if (diem.DiemCc < 0 || diem.DiemCc > 10 ||
                diem.DiemGK < 0 || diem.DiemGK > 10 ||
                diem.DiemThi < 0 || diem.DiemThi > 10)
                return "Điểm phải nằm trong khoảng từ 0 đến 10.";

            // CheckExist nay phai dung MaLop va MaSV
            if (dal.CheckExist(diem.MaLop, diem.MaSV))
                return "Sinh viên này đã có điểm trong lớp này!";

            return dal.Insert(diem) ? "Thêm thành công!" : "Thêm thất bại!";
        }

        public string SuaDiem(Score diem)
        {
            // Kiểm tra khóa chính
            if (string.IsNullOrWhiteSpace(diem.MaSV) || string.IsNullOrWhiteSpace(diem.MaLop))
                return "Không tìm thấy Mã lớp hoặc Mã sinh viên để cập nhật.";

            // Kiểm tra giá trị điểm hợp lệ (0-10)
            if (diem.DiemCc < 0 || diem.DiemCc > 10 ||
                diem.DiemGK < 0 || diem.DiemGK > 10 ||
                diem.DiemThi < 0 || diem.DiemThi > 10)
                return "Điểm phải nằm trong khoảng từ 0 đến 10.";

            return dal.Update(diem) ? "Sửa thành công!" : "Sửa thất bại!";
        }

        // **Đã sửa lỗi**: Đổi tên hàm và thêm MaLop để xóa chính xác
        public string XoaDiem(string MaLop, string MaSV)
        {
            // Can kiem tra ban ghi co ton tai hay khong truoc khi xoa
            if (!dal.CheckExist(MaLop, MaSV))
                return "Không tìm thấy bản ghi điểm này để xóa!";

            return dal.Delete(MaLop, MaSV) ? "Xóa thành công!" : "Xóa thất bại!";
        }
    }
}