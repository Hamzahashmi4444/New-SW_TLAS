using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace TLAS.Common
{
    public class GroupHelper
    {

        //Properties   

        private Boolean _isCIPStarted = false;

        private Boolean _isCasticCirculation = false;
        private Boolean _isAcidCirculation = false;

        private String _Remarks = "InProcess";
        private DateTime? _StartDatetime = null;
        private DateTime? _EndDatetime = null;
        private DateTime? _ReUsdWtrStarDatetime = null;
        private DateTime? _ReUsdWtrEndDatetime = null;
        private DateTime? _RawWtrStartDatetime = null;
        private DateTime? _RawWtrEndDatetime = null;
        private DateTime? _RawWtrStartDatetime1 = null;
        private DateTime? _RawWtrEndDatetime1 = null;

        public int LastCIPStep { get; set; }
        public int RowID { get; set; }
        public String cCCondctivity { get; set; }
        public String cCTemp { get; set; }
        public String cCFlowRate { get; set; }
        public String aCCondctivity { get; set; }
        public String aCTemp { get; set; }
        public String aCFlowRate { get; set; }
        public String RawWtrCondctivity { get; set; }
        public String ObjectCode { get; set; }
        public String CIPGroup { get; set; }
        public String CIPType { get; set; }


        public DateTime? StartDatetime
        {
            get { return _StartDatetime; }
            set { _StartDatetime = value; }
        }
        public DateTime? EndDatetime
        {
            get { return _EndDatetime; }
            set { _EndDatetime = value; }
        }

        public DateTime? ReUsdWtrStarDatetime
        {
            get { return _ReUsdWtrStarDatetime; }
            set { _ReUsdWtrStarDatetime = value; }
        }
        public DateTime? ReUsdWtrEndDatetime
        {
            get { return _ReUsdWtrEndDatetime; }
            set { _ReUsdWtrEndDatetime = value; }
        }

        public DateTime? RawWtrStartDatetime
        {
            get { return _RawWtrStartDatetime; }
            set { _RawWtrStartDatetime = value; }
        }
        public DateTime? RawWtrEndDatetime
        {
            get { return _RawWtrEndDatetime; }
            set { _RawWtrEndDatetime = value; }
        }

        public DateTime? RawWtrStartDatetime1
        {
            get { return _RawWtrStartDatetime1; }
            set { _RawWtrStartDatetime1 = value; }
        }
        public DateTime? RawWtrEndDatetime1
        {
            get { return _RawWtrEndDatetime1; }
            set { _RawWtrEndDatetime1 = value; }
        }


        public String Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }
        public Boolean IsCIPStarted
        {
            get { return _isCIPStarted; }
            set { _isCIPStarted = value; }
        }

        public Boolean IsCasticCirculation
        {
            get { return _isCasticCirculation; }
            set { _isCasticCirculation = value; }
        }
        public Boolean IsAcidCirculation
        {
            get { return _isAcidCirculation; }
            set { _isAcidCirculation = value; }
        }

        public int InsertRecod(GroupHelper GrpHlprObj, string _connStr)
        {
            int RecordID = -1;

            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "InsertCipRecord";

                    cmd.Parameters.AddWithValue("StartDatetime", GrpHlprObj.StartDatetime);
                    cmd.Parameters.AddWithValue("EndDatetime", GrpHlprObj.EndDatetime ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("ReUsdWtrStarDatetime", GrpHlprObj.ReUsdWtrStarDatetime ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("ReUsdWtrEndDatetime", GrpHlprObj.ReUsdWtrEndDatetime ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("cCCondctivity", GrpHlprObj.cCCondctivity ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("cCTemp", GrpHlprObj.cCTemp ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("cCFlowRate", GrpHlprObj.cCFlowRate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("RawWtrStartDatetime", GrpHlprObj.RawWtrStartDatetime ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("RawWtrEndDatetime", GrpHlprObj.RawWtrEndDatetime ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("aCCondctivity", GrpHlprObj.aCCondctivity ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("aCTemp", GrpHlprObj.aCTemp ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("aCFlowRate", GrpHlprObj.aCFlowRate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("RawWtrCondctivity", GrpHlprObj.RawWtrCondctivity ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("RawWtrStartDatetime1", GrpHlprObj.RawWtrStartDatetime1 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("RawWtrEndDatetime1", GrpHlprObj.RawWtrEndDatetime1 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Remarks", GrpHlprObj.Remarks);
                    cmd.Parameters.AddWithValue("ObjectCode", GrpHlprObj.ObjectCode);
                    cmd.Parameters.AddWithValue("ObjectGroup", GrpHlprObj.CIPGroup);
                    cmd.Parameters.AddWithValue("CIPType", GrpHlprObj.CIPType);

                    RecordID = (int)(decimal)cmd.ExecuteScalar();
                }
                conn.Close();



            }

            return RecordID;
        }

        public void UpdateRecod(GroupHelper GrpHlprObj, string _connStr)
        {

            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "UpdateCipRecord";

                    cmd.Parameters.AddWithValue("EndDatetime", GrpHlprObj.EndDatetime ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("ReUsdWtrStarDatetime", GrpHlprObj.ReUsdWtrStarDatetime ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("ReUsdWtrEndDatetime", GrpHlprObj.ReUsdWtrEndDatetime ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("cCCondctivity", GrpHlprObj.cCCondctivity ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("cCTemp", GrpHlprObj.cCTemp ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("cCFlowRate", GrpHlprObj.cCFlowRate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("RawWtrStartDatetime", GrpHlprObj.RawWtrStartDatetime ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("RawWtrEndDatetime", RawWtrEndDatetime ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("aCCondctivity", GrpHlprObj.aCCondctivity ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("aCTemp", GrpHlprObj.aCTemp ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("aCFlowRate", GrpHlprObj.aCFlowRate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("RawWtrCondctivity", GrpHlprObj.RawWtrCondctivity ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("RawWtrStartDatetime1", GrpHlprObj.RawWtrStartDatetime1 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("RawWtrEndDatetime1", GrpHlprObj.RawWtrEndDatetime1 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Remarks", GrpHlprObj.Remarks ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("ID", GrpHlprObj.RowID);
                    cmd.Parameters.AddWithValue("CIPType", GrpHlprObj.CIPType);

                    cmd.ExecuteNonQuery();
                }
                conn.Close();

            }
        }


    }
}
