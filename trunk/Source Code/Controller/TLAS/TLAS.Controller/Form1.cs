using OPC;
using OPCDA.NET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TLAS.Common;

namespace TLAS.Controller
{
    public partial class Form1 : Form
    {

        string _connStr = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
        string ServerName { get { return ConfigurationManager.AppSettings["ServerName"]; } }
        Boolean isDebug { get { return Convert.ToBoolean(ConfigurationManager.AppSettings["Debugging"]); } }

        #region Declare All  Tag Readonly


        public static readonly string TagLPG_Bay01_RFID_Puched = ConfigurationManager.AppSettings["TagLPG_Bay01_RFID_Puched"],
        TagLPG_WB_RFIDPunched = ConfigurationManager.AppSettings["TagLPG_WB_RFIDPunched"],
        TagLPG_Bay01_LPGBatchComplete = ConfigurationManager.AppSettings["TagLPG_Bay01_LPGBatchComplete"],
        TagLPG_Bay01_LPGBatchValue = ConfigurationManager.AppSettings["TagLPG_Bay01_LPGBatchValue"],
        TagLPG_Bay01_CardData = ConfigurationManager.AppSettings["TagLPG_Bay01_CardData"],
        TagLPG_Bay01_CompID = ConfigurationManager.AppSettings["TagLPG_Bay01_CompID"],

        TagLPG_Bay02_RFID_Puched = ConfigurationManager.AppSettings["TagLPG_Bay02_RFID_Puched"],
        TagLPG_Bay02_LPGBatchComplete = ConfigurationManager.AppSettings["TagLPG_Bay02_LPGBatchComplete"],
        TagLPG_Bay02_LPGBatchValue = ConfigurationManager.AppSettings["TagLPG_Bay02_LPGBatchValue"],
        TagLPG_Bay02_CardData = ConfigurationManager.AppSettings["TagLPG_Bay02_CardData"],
        TagLPG_Bay02_CompID = ConfigurationManager.AppSettings["TagLPG_Bay02_CompID"],

        TagLPG_Bay03_RFID_Puched = ConfigurationManager.AppSettings["TagLPG_Bay03_RFID_Puched"],
        TagLPG_Bay03_LPGBatchComplete = ConfigurationManager.AppSettings["TagLPG_Bay03_LPGBatchComplete"],
        TagLPG_Bay03_LPGBatchValue = ConfigurationManager.AppSettings["TagLPG_Bay03_LPGBatchValue"],
        TagLPG_Bay03_CardData = ConfigurationManager.AppSettings["TagLPG_Bay03_CardData"],
        TagLPG_Bay03_CompID = ConfigurationManager.AppSettings["TagLPG_Bay03_CompID"],

        TagCON_Bay01_RFID_Puched = ConfigurationManager.AppSettings["TagCON_Bay01_RFID_Puched"],
        TagCON_Bay01_CONBatchComplete = ConfigurationManager.AppSettings["TagCON_Bay01_CONBatchComplete"],
        TagCON_Bay01_CONBatchValue = ConfigurationManager.AppSettings["TagCON_Bay01_CONBatchValue"],
        TagCON_Bay01_CardData = ConfigurationManager.AppSettings["TagCON_Bay01_CardData"],
        TagCON_Bay01_CompID = ConfigurationManager.AppSettings["TagCON_Bay01_CompID"],

        TagCON_Bay02_RFID_Puched = ConfigurationManager.AppSettings["TagCON_Bay02_RFID_Puched"],
        TagCON_Bay02_CONBatchComplete = ConfigurationManager.AppSettings["TagCON_Bay02_CONBatchComplete"],
        TagCON_Bay02_CONBatchValue = ConfigurationManager.AppSettings["TagCON_Bay02_CONBatchValue"],
        TagCON_Bay02_CardData = ConfigurationManager.AppSettings["TagCON_Bay02_CardData"],
        TagCON_Bay02_CompID = ConfigurationManager.AppSettings["TagCON_Bay02_CompID"],

        TagCON_Bay03_RFID_Puched = ConfigurationManager.AppSettings["TagCON_Bay03_RFID_Puched"],
        TagCON_Bay03_CONBatchComplete = ConfigurationManager.AppSettings["TagCON_Bay03_CONBatchComplete"],
        TagCON_Bay03_CONBatchValue = ConfigurationManager.AppSettings["TagCON_Bay03_CONBatchValue"],
        TagCON_Bay03_CardData = ConfigurationManager.AppSettings["TagCON_Bay03_CardData"],
        TagCON_Bay03_CompID = ConfigurationManager.AppSettings["TagCON_Bay03_CompID"],


        TagLPG_WB_CardData = ConfigurationManager.AppSettings["TagLPG_WB_CardData"],
        TagLPG_WB_Val = ConfigurationManager.AppSettings["TagLPG_WB_Val"];



        string[] items = { TagLPG_Bay01_LPGBatchValue, TagLPG_Bay01_CardData, TagLPG_Bay01_CompID, TagLPG_WB_CardData, TagLPG_WB_Val, TagLPG_Bay01_RFID_Puched, TagLPG_WB_RFIDPunched, TagLPG_Bay01_LPGBatchComplete,
                         TagLPG_Bay02_RFID_Puched,TagLPG_Bay02_LPGBatchComplete,TagLPG_Bay02_LPGBatchValue,TagLPG_Bay02_CardData,TagLPG_Bay02_CompID,
                         TagLPG_Bay03_RFID_Puched,TagLPG_Bay03_LPGBatchComplete,TagLPG_Bay03_LPGBatchValue,TagLPG_Bay03_CardData,TagLPG_Bay03_CompID,
                         TagCON_Bay01_RFID_Puched,TagCON_Bay01_CONBatchComplete,TagCON_Bay01_CONBatchValue,TagCON_Bay01_CardData,TagCON_Bay01_CompID,
                         TagCON_Bay02_RFID_Puched,TagCON_Bay02_CONBatchComplete,TagCON_Bay02_CONBatchValue,TagCON_Bay02_CardData,TagCON_Bay02_CompID,
                         TagCON_Bay03_RFID_Puched,TagCON_Bay03_CONBatchComplete,TagCON_Bay03_CONBatchValue,TagCON_Bay03_CardData,TagCON_Bay03_CompID
                         };

        #endregion


        Timer Timer = new Timer();
        Timer Tmr_LPGBay02 = new Timer();
        Timer Tmr_LPGBay03 = new Timer();

        Timer Tmr_CONBay01 = new Timer();
        Timer Tmr_CONBay02 = new Timer();
        Timer Tmr_CONBay03 = new Timer();

        Boolean WBRFIDPunchedPrevState = false;
        Boolean WBBatchCompletePrevState = false;

        
        Boolean RFIDPunchedPrevState = false;
        Boolean BatchCompletePrevState = false;

        Boolean RFIDBAY02PunchedPrevState = false;
        Boolean BAY02BatchCompletePrevState = false;

        Boolean RFIDBAY03PunchedPrevState = false;
        Boolean BAY03BatchCompletePrevState = false;

        Boolean RFIDBAY04PunchedPrevState = false;
        Boolean BAY04BatchCompletePrevState = false;

        Boolean RFIDBAY05PunchedPrevState = false;
        Boolean BAY05BatchCompletePrevState = false;

        Boolean RFIDBAY06PunchedPrevState = false;
        Boolean BAY06BatchCompletePrevState = false;

        int UpdateRate = 100;  // ms  refresh

        OpcServer OpcSrv = null;

        TLAS.Common.OPCBO _OPCB0 = null;
        TLAS.Common.OPCBO _OPCB0LPGBay02 = null;
        TLAS.Common.OPCBO _OPCB0LPGBay03 = null;
        TLAS.Common.OPCBO _OPCB0CONBay01 = null;
        TLAS.Common.OPCBO _OPCB0CONBay02 = null;
        TLAS.Common.OPCBO _OPCB0CONBay03 = null;

        OpcGroup oGrp;
        //Variables for Line1
        OPCItemState[] ItemValues1 = new OPCItemState[4];
        OPCItemDef[] items1 = new OPCItemDef[4];
        OPCItemResult[] addRslt1;

        RefreshGroup AsyncRefrGroup = null;

        public Form1()
        {
            InitializeComponent();

            // NestleCIPService.CIPManager SAS = new NestleCIPService.CIPManager();
            // SAS.CIPManager_start();
        }

        /// <summary>
        /// Add given item to OPC items group
        /// </summary>
        /// <param name="itemName"></param>
        private void AddItemToGroup(string itemName)
        {
            try
            {
                int rtc;
                rtc = AsyncRefrGroup.Add(itemName);

                if (HRESULTS.Failed(rtc))
                {

                    DBLogging.InsertLogs("Exception: AddItemToGroup", false, "ReadWriteGroup.GetErrorString(rtc)    " + itemName, _connStr);
                }
            }
            catch (Exception ex)
            {
                DBLogging.InsertLogs("Exception: AddItemToGroup", false, "ReadWriteGroup.GetErrorString(rtc)    " + ex.Message, _connStr);
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                _OPCB0 = new OPCBO(ServerName, items);


                Timer.Interval = 4000; // specify interval time as you want
                Timer.Tick += new EventHandler(timer_Tick);
                Timer.Start();


                Tmr_LPGBay02.Interval = 4000;
                Tmr_LPGBay02.Tick += Tmr_LPGBay02_Tick;
                Tmr_LPGBay02.Start();

                Tmr_LPGBay03.Interval = 4000;
                Tmr_LPGBay03.Tick += Tmr_LPGBay03_Tick;
                Tmr_LPGBay03.Start();

                Tmr_CONBay01.Interval = 4000;
                Tmr_CONBay01.Tick += Tmr_CONBay01_Tick;
                Tmr_CONBay01.Start();

                Tmr_CONBay02.Interval = 4000;
                Tmr_CONBay02.Tick += Tmr_CONBay02_Tick;
                Tmr_CONBay02.Start();

                Tmr_CONBay03.Interval = 4000;
                Tmr_CONBay03.Tick += Tmr_CONBay03_Tick;
                Tmr_CONBay03.Start();


                //     string RFIDCardNumb = "1011";
                //     string WBWeight = "234";

                //   OpcSrv = new OpcServer();
                // int rtc = OpcSrv.Connect("Rockwell-PC", ServerName);
                //  if (HRESULTS.Failed(rtc))
                //    throw new OPCException(rtc);

                //                string rslt1 = AddMyRefreshGroup1(OpcSrv);   //Line 1

                label1.Text = "Started";
                label2.Text = "";
            }
            catch (Exception ex)
            {
                label3.Text = ex.Message;
                //    DBLogging.InsertLogs("Exception", false, ex.Message, _connStr);
                OpcSrv = null;

            }

        }


        private void timer_Tick(object sender, EventArgs e)
        {

            String isRFIDPunched = _OPCB0.ReadStringItem(TagLPG_WB_RFIDPunched);

            if (!string.IsNullOrEmpty(isRFIDPunched) && isRFIDPunched == "1" && WBRFIDPunchedPrevState == false)
            {
                label3.Text += isRFIDPunched;
                WBRFIDPunchedPrevState = true;

                String weightvalue = _OPCB0.ReadStringItem(TagLPG_WB_Val);
                String cardvalue = _OPCB0.ReadStringItem(TagLPG_WB_CardData);



                string cmdexePath = @"C:\Users\Administrator\Documents\visual studio 2013\Projects\TLAS\TLAS.WBconsole\bin\Debug\TLAS.WBconsole.exe";

                //notice the quotes around the below string...
                string cmdArguments = String.Format("{0}{1}{2}", cardvalue, " ", weightvalue);
                ProcessStartInfo psi = new ProcessStartInfo(cmdexePath, cmdArguments);
                Process p = new Process();
                p.StartInfo = psi;
                p.Start();


                label2.Text += weightvalue;
                label1.Text += cardvalue;
            }
            else if (!string.IsNullOrEmpty(isRFIDPunched) && isRFIDPunched == "0" && WBRFIDPunchedPrevState == true)
            {
                WBRFIDPunchedPrevState = false;

            }


            //  bay01 card punched


            String isBay01Punched = _OPCB0.ReadStringItem(TagLPG_Bay01_RFID_Puched);

            if (!string.IsNullOrEmpty(isBay01Punched) && isBay01Punched == "1" && RFIDPunchedPrevState == false)
            {
                label3.Text += isBay01Punched;
                RFIDPunchedPrevState = true;

                String cardvalue = _OPCB0.ReadStringItem(TagLPG_Bay01_CardData);

                //  key:1002 weight:1099 batchcomp:false comptid:6

                string cmdexePath = @"C:\Users\Administrator\Documents\visual studio 2013\Projects\TLAS\TLAS.BAYS\bin\x86\Debug\TLAS.BAYS.exe";

                //notice the quotes around the below string...
                string cmdArguments = String.Format("{0}{1}{2}", "key:" + cardvalue, " ", "weight:0 batchcomp:false comptid:0 baycode:Bay01" );
                ProcessStartInfo psi = new ProcessStartInfo(cmdexePath, cmdArguments);
                Process p = new Process();
                p.StartInfo = psi;
                p.Start();
                label1.Text += cardvalue;
            }
            else if (!string.IsNullOrEmpty(isBay01Punched) && isBay01Punched == "0" && RFIDPunchedPrevState == true)
            {
                RFIDPunchedPrevState = false;

            }


            //  bay01 batch complete


            String isBay01batchcomplete = _OPCB0.ReadStringItem(TagLPG_Bay01_LPGBatchComplete);

            if (!string.IsNullOrEmpty(isBay01batchcomplete) && isBay01batchcomplete == "1" && BatchCompletePrevState == false)
            {
                label3.Text += isBay01batchcomplete;
                BatchCompletePrevState = true;

                String cardvalue = _OPCB0.ReadStringItem(TagLPG_Bay01_CardData);
                String BCUWeight = _OPCB0.ReadStringItem(TagLPG_Bay01_LPGBatchValue);
                string compid = _OPCB0.ReadStringItem(TagLPG_Bay01_CompID);

                //  key:1002 weight:1099 batchcomp:false comptid:6

                string cmdexePath = @"C:\Users\Administrator\Documents\visual studio 2013\Projects\TLAS\TLAS.BAYS\bin\x86\Debug\TLAS.BAYS.exe";

                //notice the quotes around the below string...
                string cmdArguments = String.Format("{0}{1}{2}", "key:" + cardvalue, " ", "weight:" + BCUWeight + " batchcomp:true baycode:Bay01 comptid:" + compid + "");
                ProcessStartInfo psi = new ProcessStartInfo(cmdexePath, cmdArguments);
                Process p = new Process();
                p.StartInfo = psi;
                p.Start();
                label1.Text += cardvalue;
            }
            else if (!string.IsNullOrEmpty(isBay01batchcomplete) && isBay01batchcomplete == "0" && BatchCompletePrevState == true)
            {
                BatchCompletePrevState = false;

            }

        }

        void Tmr_LPGBay02_Tick(object sender, EventArgs e)
        {

            //  bay02 card punched

            String isBay02Punched = _OPCB0.ReadStringItem(TagLPG_Bay02_RFID_Puched);

            if (!string.IsNullOrEmpty(isBay02Punched) && isBay02Punched == "1" && RFIDBAY02PunchedPrevState == false)
            {
                label3.Text += isBay02Punched;
                RFIDBAY02PunchedPrevState = true;

                String cardvalue = _OPCB0.ReadStringItem(TagLPG_Bay02_CardData);

                //  key:1002 weight:1099 batchcomp:false comptid:6

                string cmdexePath = @"C:\Users\Administrator\Documents\visual studio 2013\Projects\TLAS\TLAS.BAYS\bin\x86\Debug\TLAS.BAYS.exe";

                //notice the quotes around the below string...
                string cmdArguments = String.Format("{0}{1}{2}", "key:" + cardvalue, " ", "weight:0 baycode:Bay02 batchcomp:false comptid:0");
                ProcessStartInfo psi = new ProcessStartInfo(cmdexePath, cmdArguments);
                Process p = new Process();
                p.StartInfo = psi;
                p.Start();
                label1.Text += cardvalue;
            }
            else if (!string.IsNullOrEmpty(isBay02Punched) && isBay02Punched == "0" && RFIDBAY02PunchedPrevState == true)
            {
                RFIDBAY02PunchedPrevState = false;

            }


            //  bay02 batch complete


            String isBay02batchcomplete = _OPCB0.ReadStringItem(TagLPG_Bay02_LPGBatchComplete);

            if (!string.IsNullOrEmpty(isBay02batchcomplete) && isBay02batchcomplete == "1" && BAY02BatchCompletePrevState == false)
            {
                label3.Text += isBay02batchcomplete;
                BAY02BatchCompletePrevState = true;

                String cardvalue = _OPCB0.ReadStringItem(TagLPG_Bay02_CardData);
                String BCUWeight = _OPCB0.ReadStringItem(TagLPG_Bay02_LPGBatchValue);
                string compid = _OPCB0.ReadStringItem(TagLPG_Bay02_CompID);

                //  key:1002 weight:1099 batchcomp:false comptid:6

                string cmdexePath = @"C:\Users\Administrator\Documents\visual studio 2013\Projects\TLAS\TLAS.BAYS\bin\x86\Debug\TLAS.BAYS.exe";

                //notice the quotes around the below string...
                string cmdArguments = String.Format("{0}{1}{2}", "key:" + cardvalue, " ", "weight:" + BCUWeight + " batchcomp:true baycode:Bay02 comptid:" + compid + "");
                ProcessStartInfo psi = new ProcessStartInfo(cmdexePath, cmdArguments);
                Process p = new Process();
                p.StartInfo = psi;
                p.Start();
                label1.Text += cardvalue;
            }
            else if (!string.IsNullOrEmpty(isBay02batchcomplete) && isBay02batchcomplete == "0" && BAY02BatchCompletePrevState == true)
            {
                BAY02BatchCompletePrevState = false;

            }

        }

        void Tmr_LPGBay03_Tick(object sender, EventArgs e)
        {

            //  bay03 card punched

            String isBay03Punched = _OPCB0.ReadStringItem(TagLPG_Bay03_RFID_Puched);

            if (!string.IsNullOrEmpty(isBay03Punched) && isBay03Punched == "1" && RFIDBAY03PunchedPrevState == false)
            {
                label3.Text += isBay03Punched;
                RFIDBAY03PunchedPrevState = true;

                String cardvalue = _OPCB0.ReadStringItem(TagLPG_Bay03_CardData);

                //  key:1002 weight:1099 batchcomp:false comptid:6

                string cmdexePath = @"C:\Users\Administrator\Documents\visual studio 2013\Projects\TLAS\TLAS.BAYS\bin\x86\Debug\TLAS.BAYS.exe";

                //notice the quotes around the below string...
                string cmdArguments = String.Format("{0}{1}{2}", "key:" + cardvalue, " ", "weight:0 batchcomp:false baycode:Bay03 comptid:0");
                ProcessStartInfo psi = new ProcessStartInfo(cmdexePath, cmdArguments);
                Process p = new Process();
                p.StartInfo = psi;
                p.Start();
                label1.Text += cardvalue;
            }
            else if (!string.IsNullOrEmpty(isBay03Punched) && isBay03Punched == "0" && RFIDBAY03PunchedPrevState == true)
            {
                RFIDBAY03PunchedPrevState = false;

            }


            //  bay03 batch complete


            String isBay03batchcomplete = _OPCB0.ReadStringItem(TagLPG_Bay03_LPGBatchComplete);

            if (!string.IsNullOrEmpty(isBay03batchcomplete) && isBay03batchcomplete == "1" && BAY03BatchCompletePrevState == false)
            {
                label3.Text += isBay03batchcomplete;
                BAY03BatchCompletePrevState = true;

                String cardvalue = _OPCB0.ReadStringItem(TagLPG_Bay03_CardData);
                String BCUWeight = _OPCB0.ReadStringItem(TagLPG_Bay03_LPGBatchValue);
                string compid = _OPCB0.ReadStringItem(TagLPG_Bay03_CompID);

                //  key:1002 weight:1099 batchcomp:false comptid:6

                string cmdexePath = @"C:\Users\Administrator\Documents\visual studio 2013\Projects\TLAS\TLAS.BAYS\bin\x86\Debug\TLAS.BAYS.exe";

                //notice the quotes around the below string...
                string cmdArguments = "key:" + cardvalue+ " weight:" + BCUWeight + " batchcomp:true baycode:Bay03 comptid:" + compid ;
               
                ProcessStartInfo psi = new ProcessStartInfo(cmdexePath, cmdArguments);
                Process p = new Process();
                p.StartInfo = psi;
                p.Start();
                label1.Text += cardvalue;
            }
            else if (!string.IsNullOrEmpty(isBay03batchcomplete) && isBay03batchcomplete == "0" && BAY03BatchCompletePrevState == true)
            {
                BAY03BatchCompletePrevState = false;

            }


        }

        void Tmr_CONBay01_Tick(object sender, EventArgs e)
        {

            //  bay04 card punched    Condensate..


            String isBay04Punched = _OPCB0.ReadStringItem(TagCON_Bay01_RFID_Puched);

            if (!string.IsNullOrEmpty(isBay04Punched) && isBay04Punched == "1" && RFIDBAY04PunchedPrevState == false)
            {
                label3.Text += isBay04Punched;
                RFIDBAY04PunchedPrevState = true;

                String cardvalue = _OPCB0.ReadStringItem(TagCON_Bay01_CardData);

                //  key:1002 weight:1099 batchcomp:false comptid:6

                string cmdexePath = @"C:\Users\Administrator\Documents\visual studio 2013\Projects\TLAS\TLAS.BAYS\bin\x86\Debug\TLAS.BAYS.exe";

                //notice the quotes around the below string...
                string cmdArguments = String.Format("{0}{1}{2}", "key:" + cardvalue, " ", "weight:0 batchcomp:false baycode:Bay04 comptid:0");
                ProcessStartInfo psi = new ProcessStartInfo(cmdexePath, cmdArguments);
                Process p = new Process();
                p.StartInfo = psi;
                p.Start();
                label1.Text += cardvalue;
            }
            else if (!string.IsNullOrEmpty(isBay04Punched) && isBay04Punched == "0" && RFIDBAY04PunchedPrevState == true)
            {
                RFIDBAY04PunchedPrevState = false;

            }


            //  bay01 batch complete


            String isBay04batchcomplete = _OPCB0.ReadStringItem(TagCON_Bay01_CONBatchComplete);

            if (!string.IsNullOrEmpty(isBay04batchcomplete) && isBay04batchcomplete == "1" && BAY04BatchCompletePrevState == false)
            {
                label3.Text += isBay04batchcomplete;
                BAY04BatchCompletePrevState = true;

                String cardvalue = _OPCB0.ReadStringItem(TagCON_Bay01_CardData);
                String BCUWeight = _OPCB0.ReadStringItem(TagCON_Bay01_CONBatchValue);
                string compid = _OPCB0.ReadStringItem(TagCON_Bay01_CompID);

                //  key:1002 weight:1099 batchcomp:false comptid:6

                string cmdexePath = @"C:\Users\Administrator\Documents\visual studio 2013\Projects\TLAS\TLAS.BAYS\bin\x86\Debug\TLAS.BAYS.exe";

                //notice the quotes around the below string...
                string cmdArguments = String.Format("{0}{1}{2}", "key:" + cardvalue, " ", "weight:" + BCUWeight + " batchcomp:true baycode:Bay04 comptid:" + compid + "");
                ProcessStartInfo psi = new ProcessStartInfo(cmdexePath, cmdArguments);
                Process p = new Process();
                p.StartInfo = psi;
                p.Start();
                label1.Text += cardvalue;
            }
            else if (!string.IsNullOrEmpty(isBay04batchcomplete) && isBay04batchcomplete == "0" && BAY04BatchCompletePrevState == true)
            {
                BAY04BatchCompletePrevState = false;

            }


        }

        void Tmr_CONBay02_Tick(object sender, EventArgs e)
        {

            //  bay05 card punched


            String isBay05Punched = _OPCB0.ReadStringItem(TagCON_Bay02_RFID_Puched);

            if (!string.IsNullOrEmpty(isBay05Punched) && isBay05Punched == "1" && RFIDBAY05PunchedPrevState == false)
            {
                label3.Text += isBay05Punched;
                RFIDBAY05PunchedPrevState = true;

                String cardvalue = _OPCB0.ReadStringItem(TagCON_Bay02_CardData);

                //  key:1002 weight:1099 batchcomp:false comptid:6

                string cmdexePath = @"C:\Users\Administrator\Documents\visual studio 2013\Projects\TLAS\TLAS.BAYS\bin\x86\Debug\TLAS.BAYS.exe";

                //notice the quotes around the below string...
                string cmdArguments = String.Format("{0}{1}{2}", "key:" + cardvalue, " ", "weight:0 batchcomp:false baycode:Bay05 comptid:0");
                ProcessStartInfo psi = new ProcessStartInfo(cmdexePath, cmdArguments);
                Process p = new Process();
                p.StartInfo = psi;
                p.Start();
                label1.Text += cardvalue;
            }
            else if (!string.IsNullOrEmpty(isBay05Punched) && isBay05Punched == "0" && RFIDBAY05PunchedPrevState == true)
            {
                RFIDBAY05PunchedPrevState = false;

            }


            //  bay05 batch complete


            String isBay05batchcomplete = _OPCB0.ReadStringItem(TagCON_Bay02_CONBatchComplete);

            if (!string.IsNullOrEmpty(isBay05batchcomplete) && isBay05batchcomplete == "1" && BAY05BatchCompletePrevState == false)
            {
                label3.Text += isBay05batchcomplete;
                BAY05BatchCompletePrevState = true;

                String cardvalue = _OPCB0.ReadStringItem(TagCON_Bay02_CardData);
                String BCUWeight = _OPCB0.ReadStringItem(TagCON_Bay02_CONBatchValue);
                string compid = _OPCB0.ReadStringItem(TagCON_Bay02_CompID);

                //  key:1002 weight:1099 batchcomp:false comptid:6

                string cmdexePath = @"C:\Users\Administrator\Documents\visual studio 2013\Projects\TLAS\TLAS.BAYS\bin\x86\Debug\TLAS.BAYS.exe";

                //notice the quotes around the below string...
                string cmdArguments = String.Format("{0}{1}{2}", "key:" + cardvalue, " ", "weight:" + BCUWeight + " batchcomp:true baycode:Bay05 comptid:" + compid + "");
                ProcessStartInfo psi = new ProcessStartInfo(cmdexePath, cmdArguments);
                Process p = new Process();
                p.StartInfo = psi;
                p.Start();
                label1.Text += cardvalue;
            }
            else if (!string.IsNullOrEmpty(isBay05batchcomplete) && isBay05batchcomplete == "0" && BAY05BatchCompletePrevState == true)
            {
                BAY05BatchCompletePrevState = false;

            }



        }

        void Tmr_CONBay03_Tick(object sender, EventArgs e)
        {

            //  bay06 card punched


            String isBay06Punched = _OPCB0.ReadStringItem(TagCON_Bay03_RFID_Puched);

            if (!string.IsNullOrEmpty(isBay06Punched) && isBay06Punched == "1" && RFIDBAY06PunchedPrevState == false)
            {
                label3.Text += isBay06Punched;
                RFIDBAY06PunchedPrevState = true;

                String cardvalue = _OPCB0.ReadStringItem(TagCON_Bay03_CardData);

                //  key:1002 weight:1099 batchcomp:false comptid:6

                string cmdexePath = @"C:\Users\Administrator\Documents\visual studio 2013\Projects\TLAS\TLAS.BAYS\bin\x86\Debug\TLAS.BAYS.exe";

                //notice the quotes around the below string...
                string cmdArguments = String.Format("{0}{1}{2}", "key:" + cardvalue, " ", "weight:0 batchcomp:false baycode:Bay06 comptid:0");
                ProcessStartInfo psi = new ProcessStartInfo(cmdexePath, cmdArguments);
                Process p = new Process();
                p.StartInfo = psi;
                p.Start();
                label1.Text += cardvalue;
            }
            else if (!string.IsNullOrEmpty(isBay06Punched) && isBay06Punched == "0" && RFIDBAY06PunchedPrevState == true)
            {
                RFIDBAY06PunchedPrevState = false;

            }


            //  bay06 batch complete


            String isBay06batchcomplete = _OPCB0.ReadStringItem(TagCON_Bay03_CONBatchComplete);

            if (!string.IsNullOrEmpty(isBay06batchcomplete) && isBay06batchcomplete == "1" && BAY06BatchCompletePrevState == false)
            {
                label3.Text += isBay06batchcomplete;
                BAY06BatchCompletePrevState = true;

                String cardvalue = _OPCB0.ReadStringItem(TagCON_Bay03_CardData);
                String BCUWeight = _OPCB0.ReadStringItem(TagCON_Bay03_CONBatchValue);
                string compid = _OPCB0.ReadStringItem(TagCON_Bay03_CompID);

                //  key:1002 weight:1099 batchcomp:false comptid:6

                string cmdexePath = @"C:\Users\Administrator\Documents\visual studio 2013\Projects\TLAS\TLAS.BAYS\bin\x86\Debug\TLAS.BAYS.exe";

                //notice the quotes around the below string...
                string cmdArguments = String.Format("{0}{1}{2}", "key:" + cardvalue, " ", "weight:" + BCUWeight + " batchcomp:true baycode:Bay06 comptid:" + compid + "");
                ProcessStartInfo psi = new ProcessStartInfo(cmdexePath, cmdArguments);
                Process p = new Process();
                p.StartInfo = psi;
                p.Start();
                label1.Text += cardvalue;
            }
            else if (!string.IsNullOrEmpty(isBay06batchcomplete) && isBay06batchcomplete == "0" && BAY06BatchCompletePrevState == true)
            {
                BAY06BatchCompletePrevState = false;

            }


        }


        private void button2_Click(object sender, EventArgs e)
        {

            try
            {
                Timer.Stop();
                Tmr_CONBay01.Stop();
                Tmr_CONBay02.Stop();
                Tmr_CONBay03.Stop();
                Tmr_LPGBay02.Stop();
                Tmr_LPGBay03.Stop();

                //   oGrp.Remove(true);    // remove the group
                // OpcSrv.Disconnect();  // release the OPC server
                // OpcSrv = null;
            }
            catch (Exception ex)
            {
                //      DBLogging.InsertLogs("Exception: CIPManager_Stop", false, ex.Message + "  " + ex.InnerException, _connStr);
            }

            label1.Text = "";
            label2.Text = "Stopped!";


        }


        private string AddMyRefreshGroup1(OpcServer OpcSrv)
        {
            float deadBand = 90.0F;
            try
            {      // create group with 2 sec update rate
                oGrp = OpcSrv.AddGroup("Line1", true, UpdateRate, ref deadBand, 0, 0);
            }
            catch
            {
                DBLogging.InsertLogs("Exception: AddMyRefreshGroup1", false, "Group could not be added", _connStr);
                return "Group could not be added";
            }

            oGrp.DataChanged += new DataChangeEventHandler(DataChangedHandler);
            oGrp.AdviseIOPCDataCallback();


            //client handle as item index
            items1[0] = new OPCItemDef(TagLPG_Bay01_RFID_Puched, true, 0, VarEnum.VT_BOOL);
            items1[1] = new OPCItemDef(TagLPG_WB_RFIDPunched, true, 1, VarEnum.VT_BOOL);
            items1[2] = new OPCItemDef(TagLPG_Bay01_LPGBatchComplete, true, 2, VarEnum.VT_BOOL);

            int rtc;

            rtc = oGrp.AddItems(items1, out addRslt1);

            if (HRESULTS.Failed(rtc))
            {
                return "Error at AddItem";
            }
            for (int i = 0; i < addRslt1.Length; ++i)
            {
                ItemValues1[i] = new OPCItemState();

            }

            return "";
        }

        //-----------------------------------------------------------------
        // this function handles data change callbacks due to the periodic refresh request
        // by the group.
        // The data is assiociated with the items by the client handle, that is specif(ied
        // in the add item function call.

        private void DataChangedHandler(object sender, DataChangeEventArgs e)
        {
            try
            {

                int i, hnd;

                //   OPC_QUALITY_STATUS qs;
                //   DateTime dt;
                for (i = 0; i < e.sts.Length; ++i)
                {
                    if (HRESULTS.Succeeded(e.sts[i].Error))
                    {

                        hnd = e.sts[i].HandleClient;
                        OPCItemDef tagItemDef = items1[hnd];
                        // int _tagValue = Convert.ToInt32(e.sts[i].DataValue);

                        if (tagItemDef != null)
                        {

                            if (tagItemDef.ItemID.Contains(TagLPG_WB_RFIDPunched) && Convert.ToInt32(e.sts[i].DataValue) == 1) //  Start
                            {
                                String RFIDCardNumber = _OPCB0.ReadStringItem(TagLPG_WB_CardData);
                                if (!string.IsNullOrEmpty(RFIDCardNumber))
                                {
                                    string WBWeight = _OPCB0.ReadStringItem(TagLPG_WB_Val);
                                    if (!string.IsNullOrEmpty(WBWeight))
                                    {
                                        string cmdexePath = @"C:\Users\Administrator\Documents\visual studio 2013\Projects\TLAS\TLAS.WBconsole\bin\DebugTLAS.WBconsole.exe";

                                        //notice the quotes around the below string...
                                        string cmdArguments = String.Format("{0},{1}", RFIDCardNumber, WBWeight);
                                        ProcessStartInfo psi = new ProcessStartInfo(cmdexePath, cmdArguments);
                                        Process p = new Process();
                                        p.StartInfo = psi;
                                        p.Start();
                                    }
                                }
                            }

                            if (tagItemDef.ItemID.Contains(TagLPG_Bay01_RFID_Puched) && Convert.ToInt32(e.sts[i].DataValue) == 1) //  Start
                            {

                            }

                            if (tagItemDef.ItemID.Contains(TagLPG_Bay01_LPGBatchComplete) && Convert.ToInt32(e.sts[i].DataValue) == 1) //  Start
                            {

                            }


                        }
                    }
                }
            }
            catch (Exception exe)
            {
                //  DBLogging.InsertLogs("Exception: Line1", false, exe.Message + " " + exe.InnerException, _connStr);
            }
        }

    }
}
