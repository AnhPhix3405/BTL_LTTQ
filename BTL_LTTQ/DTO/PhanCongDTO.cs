using System;

namespace BTL_LTTQ.DTO
{
    public class PhanCongDTO
    {
        public string MaPC { get; set; }
        public DateTime? NgayPC { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public byte CaHoc { get; set; }      // 1..5
        public byte Thu { get; set; }        // 2..8
        public string MaPhong { get; set; }
        public string MaGV { get; set; }
        public string MaLop { get; set; }
    }
}