using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTL_LTTQ.DTO
{
    public class Subject
    {
        public int Id { get; set; }
        public string MaMH { get; set; }
        public string TenMH { get; set; }
        public int SoTC { get; set; }
        public int TietLT { get; set; }
        public int TietTH { get; set; }
        public string MaKhoa { get; set; }
        public string TenKhoa { get; set; } // Để hiển thị tên khoa

        public Subject()
        {
        }

        public Subject(int id, string maMH, string tenMH, int soTC, int tietLT, int tietTH, string maKhoa, string tenKhoa = "")
        {
            Id = id;
            MaMH = maMH;
            TenMH = tenMH;
            SoTC = soTC;
            TietLT = tietLT;
            TietTH = tietTH;
            MaKhoa = maKhoa;
            TenKhoa = tenKhoa;
        }
    }

    public class Department
    {
        public int Id { get; set; }
        public string MaKhoa { get; set; }
        public string TenKhoa { get; set; }

        public Department()
        {
        }

        public Department(int id, string maKhoa, string tenKhoa)
        {
            Id = id;
            MaKhoa = maKhoa;
            TenKhoa = tenKhoa;
        }
    }
}
