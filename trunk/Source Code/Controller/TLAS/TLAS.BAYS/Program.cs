using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using TLAS.Common;

namespace TLAS.BAYS
{
    class Program
    {

        //     static  String _connStr = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
        //    static String ServerName { get { return ConfigurationManager.AppSettings["ServerName"]; } }



        static void Main(string[] args)
        {

            //     Console.ReadKey();

            string RFIDKey = string.Empty;
            string weightvalue = string.Empty;
            bool isBatchComplete = true;// false;
            Int32 CompartmentID = 1;
            string bayid = string.Empty;

            Int32 ShipmentID = 0;
            string loadno = string.Empty;
            string compartmentid = string.Empty;
            string ordercode = string.Empty;


            string TagLPG_Bay01_PresetVal = ConfigurationManager.AppSettings["TagLPG_Bay01_PresetVal"],
            TagLPG_Bay01_LoadNo = ConfigurationManager.AppSettings["TagLPG_Bay01_LoadNo"],
            TagLPG_Bay01_LPGBatchValue = ConfigurationManager.AppSettings["TagLPG_Bay01_LPGBatchValue"],
            TagLPG_Bay01_CompID = ConfigurationManager.AppSettings["TagLPG_Bay01_CompID"],

            TagLPG_Bay02_PresetVal = ConfigurationManager.AppSettings["TagLPG_Bay02_PresetVal"],
            TagLPG_Bay02_LoadNo = ConfigurationManager.AppSettings["TagLPG_Bay02_LoadNo"],
            TagLPG_Bay02_LPGBatchValue = ConfigurationManager.AppSettings["TagLPG_Bay02_LPGBatchValue"],
            TagLPG_Bay02_CompID = ConfigurationManager.AppSettings["TagLPG_Bay02_CompID"],

            TagLPG_Bay03_PresetVal = ConfigurationManager.AppSettings["TagLPG_Bay03_PresetVal"],
            TagLPG_Bay03_LoadNo = ConfigurationManager.AppSettings["TagLPG_Bay03_LoadNo"],
            TagLPG_Bay03_LPGBatchValue = ConfigurationManager.AppSettings["TagLPG_Bay03_LPGBatchValue"],
            TagLPG_Bay03_CompID = ConfigurationManager.AppSettings["TagLPG_Bay03_CompID"],

            TagCON_Bay01_PresetVal = ConfigurationManager.AppSettings["TagCON_Bay01_PresetVal"],
            TagCON_Bay01_LoadNo = ConfigurationManager.AppSettings["TagCON_Bay01_LoadNo"],
            TagCON_Bay01_CONBatchValue = ConfigurationManager.AppSettings["TagCON_Bay01_CONBatchValue"],
            TagCON_Bay01_CompID = ConfigurationManager.AppSettings["TagCON_Bay01_CompID"],

            TagCON_Bay02_PresetVal = ConfigurationManager.AppSettings["TagCON_Bay02_PresetVal"],
            TagCON_Bay02_LoadNo = ConfigurationManager.AppSettings["TagCON_Bay02_LoadNo"],
            TagCON_Bay02_CONBatchValue = ConfigurationManager.AppSettings["TagCON_Bay02_CONBatchValue"],
            TagCON_Bay02_CompID = ConfigurationManager.AppSettings["TagCON_Bay02_CompID"],

            TagCON_Bay03_PresetVal = ConfigurationManager.AppSettings["TagCON_Bay03_PresetVal"],
            TagCON_Bay03_LoadNo = ConfigurationManager.AppSettings["TagCON_Bay03_LoadNo"],
            TagCON_Bay03_CONBatchValue = ConfigurationManager.AppSettings["TagCON_Bay03_CONBatchValue"],
            TagCON_Bay03_CompID = ConfigurationManager.AppSettings["TagCON_Bay03_CompID"];




            string[] items = { TagLPG_Bay01_PresetVal, TagLPG_Bay01_LoadNo, TagLPG_Bay01_LPGBatchValue, TagLPG_Bay01_CompID,                             
                               TagLPG_Bay02_PresetVal, TagLPG_Bay02_LoadNo, TagLPG_Bay02_LPGBatchValue, TagLPG_Bay02_CompID,
                               TagLPG_Bay03_PresetVal, TagLPG_Bay03_LoadNo, TagLPG_Bay03_LPGBatchValue, TagLPG_Bay03_CompID
                              ,TagCON_Bay01_PresetVal, TagCON_Bay01_LoadNo, TagCON_Bay01_CONBatchValue, TagCON_Bay01_CompID
                              ,TagCON_Bay02_PresetVal, TagCON_Bay02_LoadNo, TagCON_Bay02_CONBatchValue, TagCON_Bay02_CompID
                              ,TagCON_Bay03_PresetVal, TagCON_Bay03_LoadNo, TagCON_Bay03_CONBatchValue, TagCON_Bay03_CompID };


            foreach (string val in args)
            {
                if (val.Contains("key"))
                {
                    RFIDKey = val.Split(new char[] { ':' })[1];
                }

                if (val.Contains("weight"))
                {
                    weightvalue = val.Split(new char[] { ':' })[1];
                }

                if (val.Contains("batchcomp"))
                {
                    isBatchComplete = Convert.ToBoolean(val.Split(new char[] { ':' })[1]);
                }

                if (val.Contains("comptid"))
                {
                    CompartmentID = Convert.ToInt32(val.Split(new char[] { ':' })[1]);
                }

                if (val.Contains("baycode"))
                {
                    bayid = val.Split(new char[] { ':' })[1];
                }


            }


            string PresetTag = string.Empty;
            string Loadid = string.Empty;
            string comptid = string.Empty;
            string batchvalue = string.Empty;

            switch (bayid)
            {
                case "Bay01":
                    PresetTag = TagLPG_Bay01_PresetVal;
                    Loadid = TagLPG_Bay01_LoadNo;
                    comptid = TagLPG_Bay01_CompID;
                    batchvalue = TagLPG_Bay01_LPGBatchValue;
                    break;

                case "Bay02":
                    PresetTag = TagLPG_Bay02_PresetVal;
                    Loadid = TagLPG_Bay02_LoadNo;
                    comptid = TagLPG_Bay02_CompID;
                    batchvalue = TagLPG_Bay02_LPGBatchValue;
                    break;


                case "Bay03":
                    PresetTag = TagLPG_Bay03_PresetVal;
                    Loadid = TagLPG_Bay03_LoadNo;
                    comptid = TagLPG_Bay03_CompID;
                    batchvalue = TagLPG_Bay03_LPGBatchValue;
                    break;


                case "Bay04":
                    PresetTag = TagCON_Bay01_PresetVal;
                    Loadid = TagCON_Bay01_LoadNo;
                    comptid = TagCON_Bay01_CompID;
                    batchvalue = TagCON_Bay01_CONBatchValue;
                    break;


                case "Bay05":
                    PresetTag = TagCON_Bay02_PresetVal;
                    Loadid = TagCON_Bay02_LoadNo;
                    comptid = TagCON_Bay02_CompID;
                    batchvalue = TagCON_Bay02_CONBatchValue;
                    break;


                case "Bay06":
                    PresetTag = TagCON_Bay03_PresetVal;
                    Loadid = TagCON_Bay03_LoadNo;
                    comptid = TagCON_Bay03_CompID;
                    batchvalue = TagCON_Bay03_CONBatchValue;
                    break;



            }


            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString))
            {

                try
                {
                    conn.Open();

                    String selectShipmentID = @"SELECT Shipment.ID as ShipmentID
FROM            [Order] INNER JOIN
                         Shipment ON [Order].OrderID = Shipment.OrderID
						 where (ShipmentStatusID=2 OR ShipmentStatusID=3) and AccessCardID=" + RFIDKey;

                    String selectOrderCode = @"SELECT [Order].OrderID
FROM            [Order] INNER JOIN
                         Shipment ON [Order].OrderID = Shipment.OrderID
						 where (ShipmentStatusID=2 OR ShipmentStatusID=3)  and AccessCardID=" + RFIDKey;

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = selectShipmentID;

                    if (cmd.ExecuteScalar() == null)
                    {
                        Console.WriteLine("No Shippment Exist for RFID -> " + RFIDKey);
                    }
                    else
                    {
                        ShipmentID = (Int32)cmd.ExecuteScalar();
                        //Shipment exist .search the order table for entry ordercode



                        if (isBatchComplete == false)
                        {
                            cmd.CommandText = selectOrderCode;
                            ordercode = cmd.ExecuteScalar().ToString();

                            DataTable dt = new DataTable();
                            string qurey = "SELECT top(1) * FROM ShipmentCompartment where ActualBCUQuantity is null AND PlannedQuantity IS NOT null AND  ShipmentCompartment.ShipmentID=" + ShipmentID + "order by CompartmentCode asc";
                            SqlDataAdapter sptr = new SqlDataAdapter(qurey, conn);
                            sptr.Fill(dt);

                            foreach (DataRow dr in dt.Rows)
                            {
                                loadno = dr["PlannedQuantity"].ToString();
                                compartmentid = dr["CompartmentCode"].ToString();
                            }

                            try
                            {
                                // TODO 
                                // write from opc loadno / order no and compt id and wait for the complated but
                                OPCBO OpcInstance = new OPCBO("RSLinx OPC Server", items);

                                OpcInstance.WriteItem(Loadid, Convert.ToInt32(ordercode));
                                OpcInstance.WriteStringItem(comptid, compartmentid);

                                int presetvalue = Convert.ToInt32(loadno);
                                OpcInstance.WriteItem(PresetTag, presetvalue);

                                string selectstatusquery = @"Select [ShipmentStatusID] from [dbo].[Shipment] WHERE ID=" + ShipmentID;
                                int statusID = 0;
                                cmd.CommandText = selectstatusquery;
                                
                                if (cmd.ExecuteScalar() != null)
                                {
                                    statusID = (Int32)cmd.ExecuteScalar();
                                }

                                if (statusID == 2 || statusID == 4)
                                {
                                    string updatestatusquery = @"UPDATE [dbo].[Shipment] SET [ShipmentStatusID] =3 WHERE ID=" + ShipmentID;
                                    cmd.CommandText = updatestatusquery;
                                    cmd.ExecuteNonQuery();
                                    Console.WriteLine("Status: Checked-in");
                                }

                                OpcInstance.CloseServer();
                            }
                            catch { }

                        }
                        else
                        {
                            //batch complete take the compt id and shipment id and attack!!!!

                            string updatequery = @"UPDATE [dbo].[ShipmentCompartment] SET [ActualBCUQuantity] =" + weightvalue + ",[AccessCardKey] =" + RFIDKey + ",[ModifiedDate] ='" + DateTime.Now.ToString() + "',[BayName] = 'BAY01' where ShipmentID=" + ShipmentID + " AND ActualBCUQuantity is null AND PlannedQuantity IS NOT null AND CompartmentCode=" + CompartmentID;
                            cmd.CommandText = updatequery;
                            cmd.ExecuteNonQuery();
                            Console.WriteLine("Added BCU Batch Value -SUCESSFULLY");

                        }

                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {

                    //  Console.ReadKey();
                }

            }
        }
    }


}
