using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace BTL_LTTQ
{
    internal class ProcessDatabase
    {
        SqlConnection sqlConnect;
        // Data Source = .; Initial Catalog = QuanLySanPham; Integrated Security = True; Connect Timeout = 30; Encrypt=True;Trust Server Certificate=True;Application Intent = ReadWrite; Multi Subnet Failover=False
        string s = "Data Source=LAPTOP-3JGSAUFN\\SQLEXPRESS01;Initial Catalog=QL_GiangDay;Integrated Security=True;";

        public void Ketnoi()
        {
            sqlConnect = new SqlConnection(s);
            if (sqlConnect.State != ConnectionState.Open)
            {
                //con.ConnectionString = s;
                sqlConnect.Open();
            }
        }

        public void Dongketnoi()
        {
            if (sqlConnect.State != ConnectionState.Closed)
            {
                sqlConnect.Close();
            }
        }

        public DataTable DocBang(string sql)
        {

            DataTable dataTable = new DataTable();
            Ketnoi();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, sqlConnect);
            sqlDataAdapter.Fill(dataTable);
            Dongketnoi();
            return dataTable;
        }

        public int CapNhatDuLieu(string sql)
        {
            Ketnoi();
            SqlCommand sqlcommand = new SqlCommand(sql, sqlConnect);
            int rowsAffected = sqlcommand.ExecuteNonQuery();
            Dongketnoi();
            return rowsAffected;
        }

    }
}
