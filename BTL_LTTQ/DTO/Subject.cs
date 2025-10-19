using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTL_LTTQ.DTO
{
    public class Subject
    {
        public string MaMH { get; set; }
        public string TenMH { get; set; }
        public int SoTC { get; set; }
        public int TietLT { get; set; }
        public int TietTH { get; set; }
        public string HeSoDiem { get; set; }
        public string MaKhoa { get; set; }
        public string TenKhoa { get; set; } // Để hiển thị tên khoa

        public Subject()
        {
        }

        public Subject(string maMH, string tenMH, int soTC, int tietLT, int tietTH, string heSoDiem, string maKhoa, string tenKhoa = "")
        {
            MaMH = maMH;
            TenMH = tenMH;
            SoTC = soTC;
            TietLT = tietLT;
            TietTH = tietTH;
            HeSoDiem = heSoDiem;
            MaKhoa = maKhoa;
            TenKhoa = tenKhoa;
        }
    }

    public class Department
    {
        public string MaKhoa { get; set; }
        public string TenKhoa { get; set; }

        public Department()
        {
        }

        public Department(string maKhoa, string tenKhoa)
        {
            MaKhoa = maKhoa;
            TenKhoa = tenKhoa;
        }
    }
}
