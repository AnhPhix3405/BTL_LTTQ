// DAL/Khoa_DAL.cs
using System.Data;

namespace BTL_LTTQ.DAL
{
    public class Khoa_DAL
    {
        public DataTable GetData()
        {
            string query = "SELECT MaKhoa, TenKhoa FROM Khoa";
            return DatabaseConnection.ExecuteQuery(query);
        }
    }
}