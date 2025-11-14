// DTO/PhanCongDTO.cs
using System;

namespace BTL_LTTQ.DTO
{
    public class PhanCongDTO
    {
        public string MaPC { get; set; }
        public DateTime? NgayPC { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public TimeSpan GioBatDau { get; set; }
        public TimeSpan GioKetThuc { get; set; }
        public string MaPhong { get; set; }
        public string MaGV { get; set; }
        public string MaLop { get; set; }
    }
}