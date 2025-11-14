// DTO/SinhVienDTO.cs
using System;

namespace BTL_LTTQ.DTO
{
    public class SinhVienDTO
    {
        public string MaSV { get; set; }
        public string TenSV { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string QueQuan { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public string MaKhoa { get; set; }
        public string MaLop { get; set; }  // Lớp tín chỉ hiện tại
    }
}