// DTO/GiangVien_DTO.cs
using System;

namespace BTL_LTTQ.DTO
{
    public class GiangVien_DTO
    {
        public string MaGV { get; set; }
        public string TenGV { get; set; }
        public string GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string DiaChi { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public string HocHam { get; set; }
        public string HocVi { get; set; }
        public string MaKhoa { get; set; }
    }
}