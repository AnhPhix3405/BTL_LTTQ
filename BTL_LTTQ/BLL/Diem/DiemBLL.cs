using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (string.IsNullOrWhiteSpace(diem.MaSV) ||
                string.IsNullOrWhiteSpace(diem.TenSV) ||
                string.IsNullOrWhiteSpace(diem.MaLop))
                return "Không được để trống.";

            // kiểm tra giá trị điểm hợp lệ
            if (diem.DiemCc < 0 || diem.DiemGK < 0 || diem.DiemThi < 0)
                return "Điểm không được âm.";

            if (dal.CheckExist(diem.MaSV))
                return "Sinh viên này đã có điểm!";

            return dal.Insert(diem) ? "Thêm thành công!" : "Thêm thất bại!";
        }

        public string SuaDiem(Score diem)
        {
            if (Double.IsNullOrWhiteSpace(diem.DiemCc))
                return "Điểm chuyên cần không được để trống.";
            if (Double.IsNullOrWhiteSpace(diem.DiemGK))
                return "Điểm giữa kì không được để trống.";
            if (Double.IsNullOrWhiteSpace(diem.DiemThi))
                return "Điểm Thi không được để trống.";

            return dal.Update(diem) ? "Sửa thành công!" : "Sửa thất bại!";
        }

        public string XoaSanPham(string MaSV)
        {
            return dal.Delete(MaSV) ? "Xóa thành công!" : "Xóa thất bại!";
        }
    }
}
