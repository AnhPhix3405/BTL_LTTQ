// DAL/MonHoc_DAL.cs
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BTL_LTTQ.DAL
{
    public class MonHoc_DAL
    {
        public DataTable GetDataByKhoa(string maKhoa)
        {
            string queryMH = "SELECT MaMH, TenMH FROM MonHoc";
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(maKhoa))
            {
                queryMH += " WHERE MaKhoa = @MaKhoa";
                parameters.Add(new SqlParameter("@MaKhoa", maKhoa));
            }

            return DatabaseConnection.ExecuteQuery(queryMH, parameters.ToArray());
        }
    }
}