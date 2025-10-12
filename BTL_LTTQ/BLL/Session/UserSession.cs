using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTL_LTTQ.BLL.Session
{
    public static class UserSession
    {
        // Thông tin tài khoản đang đăng nhập
        public static int Id { get; set; }
        public static string TenDangNhap { get; set; }
        public static string LoaiTaiKhoan { get; set; }
        public static DateTime LoginTime { get; set; }

        // Kiểm tra trạng thái đăng nhập
        public static bool IsLoggedIn => !string.IsNullOrEmpty(TenDangNhap);

        // Đăng nhập - Lưu thông tin session
        public static void SetUserSession(int id, string tenDangNhap, string loaiTaiKhoan)
        {
            Id = id;
            TenDangNhap = tenDangNhap;
            LoaiTaiKhoan = loaiTaiKhoan;
            LoginTime = DateTime.Now;
        }

        // Đăng xuất - Xóa thông tin session
        public static void ClearSession()
        {
            Id = 0;
            TenDangNhap = null;
            LoaiTaiKhoan = null;
            LoginTime = DateTime.MinValue;
        }

        // Kiểm tra quyền admin
        public static bool IsAdmin()
        {
            return LoaiTaiKhoan?.ToLower() == "admin" || LoaiTaiKhoan?.ToLower() == "quản trị";
        }

        // Kiểm tra quyền user
        public static bool IsUser()
        {
            return LoaiTaiKhoan?.ToLower() == "user" || LoaiTaiKhoan?.ToLower() == "người dùng";
        }

        // Lấy thông tin hiển thị
        public static string GetDisplayInfo()
        {
            if (IsLoggedIn)
                return $"Xin chào: {TenDangNhap} ({LoaiTaiKhoan})";
            return "Chưa đăng nhập";
        }

        // Lấy thời gian đăng nhập
        public static string GetLoginTimeInfo()
        {
            if (IsLoggedIn)
                return $"Đăng nhập lúc: {LoginTime:dd/MM/yyyy HH:mm:ss}";
            return "";
        }

        // Kiểm tra session có hết hạn không (ví dụ: 8 tiếng)
        public static bool IsSessionExpired(int hoursLimit = 8)
        {
            if (!IsLoggedIn) return true;
            
            TimeSpan timeDifference = DateTime.Now - LoginTime;
            return timeDifference.TotalHours > hoursLimit;
        }
    }
}
