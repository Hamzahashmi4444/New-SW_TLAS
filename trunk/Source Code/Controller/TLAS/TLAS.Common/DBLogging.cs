using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace TLAS.Common
{
   public static class DBLogging
    {
        
        public static void InsertLogs(string ErrorType, Boolean isreport, string message, string _connStr )
        {
          
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "InsertLogs";

                    cmd.Parameters.AddWithValue("Message", message);
                    cmd.Parameters.AddWithValue("ErrorType", ErrorType);
                    cmd.Parameters.AddWithValue("isReport", isreport);
                   
                    cmd.ExecuteNonQuery();
                }
                conn.Close();

            }

        
        }
        
    }
}
