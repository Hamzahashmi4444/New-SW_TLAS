using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLAS.WBconsole
{
    class Program
    {


        static String _connStr = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
        static String ServerName { get { return ConfigurationManager.AppSettings["ServerName"]; } }



        static void Main(string[] args)
        {

            int key =    Convert.ToInt32(args[0].ToString());
            int weight = Convert.ToInt32(args[1].ToString());

            string TagWB_BeaconLight = ConfigurationManager.AppSettings["LPG_WB_BeaconOnMS"];

            int ShipmentID = 0;

      //      Console.ReadKey();
            
            using (SqlConnection conn = new SqlConnection(_connStr))
            {

                try
                {
                    conn.Open();
                    String selectShipmentID = "Select [ID] from Shipment where (ShipmentStatusID=2 OR ShipmentStatusID=3) AND AccessCardID=" + key;
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = selectShipmentID;

                    if (cmd.ExecuteScalar() == null)
                    {
                        Console.WriteLine("No Shippment Exist for this RFID -> " + key);
                    }
                    else
                    {
                        ShipmentID = (Int32)cmd.ExecuteScalar();
                        //Shipment exist .search the WB table for entry with ShipmentID
                        String WBRowCount = "Select Count(*) from WeighBridge where ShipmentID=" + ShipmentID;
                        cmd.CommandText = WBRowCount;
                        Int32 RowCount = (Int32)cmd.ExecuteScalar();

                        if (RowCount == 0)
                        {
                            Console.WriteLine("No WB Entry Exist for ShipmentID -> " + ShipmentID);
                            Console.WriteLine("Creating Entry");
                            String VechicleCode = String.Empty;
                            string vehiclecodequery = @"SELECT VehicleCode FROM Shipment WHERE (ShipmentStatusID=2 OR ShipmentStatusID=3) AND ID=" + ShipmentID;
                            cmd.CommandText = vehiclecodequery;

                            if (cmd.ExecuteScalar() == null)
                            {

                            }
                            else
                            {
                                VechicleCode = (String)cmd.ExecuteScalar();

                            }

                            String _InsertWB = "INSERT INTO [dbo].[WeighBridge]([TareWeight],[Status],[ShipmentID],[VehicleCode]) VALUES(" + weight + "," + "'Weigh-In'," + ShipmentID + ",'" + VechicleCode + "')";
                            cmd.CommandText = _InsertWB;
                            cmd.ExecuteNonQuery();

                            string[] items = { TagWB_BeaconLight };
                            TLAS.Common.OPCBO OPcSrvr = new Common.OPCBO("RSLinx OPC Server", items);
                            OPcSrvr.WriteItem(TagWB_BeaconLight, 1);
                            OPcSrvr.CloseServer();
                            OPcSrvr = null;


                        }
                        else
                        {
                            String SelectTareWeight = "Select [TareWeight] from WeighBridge where ShipmentID=" + ShipmentID;
                            cmd.CommandText = SelectTareWeight;
                            if (cmd.ExecuteScalar() == null)
                            { }
                            else
                            {

                                Int32 _TareWeight = (Int32)cmd.ExecuteScalar();

                                if (_TareWeight == weight)
                                    Console.WriteLine("RFID Double Punched for Weigh-In: This weight already exist in database");
                                else
                                {

                                    String SelectloadedWeight = "Select [LoadedWeight] from WeighBridge where ShipmentID=" + key;
                                    cmd.CommandText = SelectloadedWeight;

                                    if (cmd.ExecuteScalar() == null)
                                    {
                                        //first time the loaded wait is empty....
                                        //so neeed to add it 

                                        String _UpdateWB = @"UPDATE [dbo].[WeighBridge] SET [LoadedWeight] =" + weight + ",[Status] = 'Weigh-Out',[ActualWeight] =" + (weight - _TareWeight) + ",[ModifiedDate] ='" + DateTime.Now.ToString() + "' WHERE ShipmentID=" + ShipmentID + "";
                                        cmd.CommandText = _UpdateWB;
                                        cmd.ExecuteNonQuery();
                                        Console.WriteLine("Added loaded weight and actal wright:  Status: Weigh-Out");

                                        string[] items = { TagWB_BeaconLight };
                                        TLAS.Common.OPCBO OPcSrvr = new Common.OPCBO("RSLinx OPC Server", items);
                                        OPcSrvr.WriteItem(TagWB_BeaconLight, 1);
                                        OPcSrvr.CloseServer();
                                        OPcSrvr = null;



                                    }
                                    else
                                    {
                                        Int32 _LoadedWeight = (Int32)cmd.ExecuteScalar();

                                        if (_LoadedWeight == weight)
                                            Console.WriteLine("RFID Double Punched for Weigh-out: This weight already exist in database");


                                    }
                                }
                            }

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
