using OPC;
using OPCDA;
using OPCDA.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLAS.Common
{

    public class OPCBO
    {
        OpcServer OpcSrver = null;
        SyncIOGroup readGroup;
        //   HelperClass hf = new HelperClass();
        string OpcServerName;

        public OPCBO(string OPCServerName, string[] items)
        {
            OpcServerName = OPCServerName;
            ConnectToServer();
            AddRead(items);
        }


        /// <summary>
        /// This function connects to the OPC Server, provided to it
        /// </summary>
        private void ConnectToServer()
        {
            try
            {   // Creating new instance of OPC server
                OpcSrver = new OpcServer();
                // Try to connect with the OPC server
                int rtc = OpcSrver.Connect("Rockwell-PC", OpcServerName);
                // Check if connection is failed then throw exception.
                if (HRESULTS.Failed(rtc))
                    throw new OPCException(rtc);
            }
            catch (Exception ex)
            {
                // Exception occure so, set OPC server object to null.
                OpcSrver = null;
                //     hf.LogException("ConnectionToServer: " + ex.ToString());
            }
        }





        public void CloseServer()
        {
            OpcSrver.Disconnect();
        }




        /// <summary>
        /// This function adds all tag items provided to it.
        /// </summary>
        /// <param name="items"></param>
        private void AddRead(string[] items)
        {
            try
            {
                if (OpcSrver != null)
                {
                    readGroup = new SyncIOGroup(OpcSrver);
                    // Add all provided items to ReadWrite Group
                    for (int i = 0; i < items.Length; i++)
                        readGroup.Add(items[i]);
                }
            }
            catch (Exception ex)
            {
                //       hf.LogException("Unable to add item: " + ex.ToString());
            }
        }


        /// <summary>
        /// This function reads a tag value from OPC, read will be from Device, Return ERROR in case of any error 
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public string ReadStringItem(String tagName)
        {

            ItemDef ItemData = new ItemDef();
            ItemData = readGroup.Item(tagName);
            OPCItemState state = new OPCItemState();

            // Read from device
            OPCDATASOURCE dsrc = OPCDATASOURCE.OPC_DS_CACHE;   //??????????
            int rtc = readGroup.Read(dsrc, ItemData, out state);

            if (HRESULTS.Succeeded(rtc))		// read from OPC server successful
            {
                if (state != null)
                {
                    if (HRESULTS.Succeeded(state.Error))	// item read successful
                    {
                        return state.DataValue.ToString();
                    }
                    else			// the item could not be read
                    {
                        //           hf.LogException("Item cannot be readable: " + tagName);
                        return "ERROR";
                    }
                }
                else       // State not valid
                {
                    //      hf.LogException("Item cannot be readable due to state: " + tagName);
                    return "ERROR";
                }
            }
            else		// OPC server read error
            {
                //     hf.LogException("Item cannot be readable due to OPC: " + tagName);
                return "ERROR";
            }

        }

        /// <summary>
        /// This function reads a analog integer tag value from OPC, read will be from Device, Return ERROR in case of any error 
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public int ReadAnalogIntegerItem(String tagName)
        {
            ItemDef ItemData;
            ItemData = readGroup.Item(tagName);
            OPCItemState state = new OPCItemState();

            // Read from device
            OPCDATASOURCE dsrc = OPCDATASOURCE.OPC_DS_DEVICE;
            int rtc = readGroup.Read(dsrc, ItemData, out state);

            if (HRESULTS.Succeeded(rtc))		// read from OPC server successful
            {
                if (state != null)
                {
                    if (HRESULTS.Succeeded(state.Error))	// item read successful
                    {
                        return Convert.ToInt32(state.DataValue.ToString());
                    }
                    else			// the item could not be read
                    {
                        //          hf.LogException("Item cannot be readable: " + tagName);
                        return -10000;
                    }
                }
                else       // State not valid
                {
                    //     hf.LogException("Item cannot be readable due to state: " + tagName);
                    return -10000;
                }
            }
            else		// OPC server read error
            {
                //     hf.LogException("Item cannot be readable due to OPC: " + tagName);
                return -10000;
            }

        }

        /// <summary>
        /// This function reads a analog double tag value from OPC, read will be from Device, Return ERROR in case of any error 
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public double ReadAnalogDoubleItem(String tagName)
        {
            ItemDef ItemData;
            ItemData = readGroup.Item(tagName);
            OPCItemState state = new OPCItemState();

            // Read from device
            OPCDATASOURCE dsrc = OPCDATASOURCE.OPC_DS_DEVICE;
            int rtc = readGroup.Read(dsrc, ItemData, out state);

            if (HRESULTS.Succeeded(rtc))		// read from OPC server successful
            {
                if (state != null)
                {
                    if (HRESULTS.Succeeded(state.Error))	// item read successful
                    {
                        return Convert.ToDouble(state.DataValue.ToString());
                    }
                    else			// the item could not be read
                    {
                        //      hf.LogException("Item cannot be readable: " + tagName);
                        return -10000;
                    }
                }
                else       // State not valid
                {
                    //     hf.LogException("Item cannot be readable due to State: " + tagName);
                    return -10000;
                }
            }
            else		// OPC server read error
            {
                //    hf.LogException("Item cannot be readable due to OPC: " + tagName);
                return -10000;
            }

        }

        /// <summary>
        /// This function reads a analog double tag value from OPC, read will be from Device, Return ERROR in case of any error 
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public int ReadBooleanItem(String tagName)
        {
            ItemDef ItemData;
            ItemData = readGroup.Item(tagName);
            OPCItemState state = new OPCItemState();

            // Read from device
            OPCDATASOURCE dsrc = OPCDATASOURCE.OPC_DS_DEVICE;
            int rtc = readGroup.Read(dsrc, ItemData, out state);

            if (HRESULTS.Succeeded(rtc))		// read from OPC server successful
            {
                if (state != null)
                {
                    if (HRESULTS.Succeeded(state.Error))	// item read successful
                    {
                        if (Convert.ToBoolean(state.DataValue) == true)
                            return 1;
                        else
                            return 0;
                    }
                    else			// the item could not be read
                    {
                        //      hf.LogException("Item cannot be readable: " + tagName);
                        return -10000;
                    }
                }
                else       // State not valid
                {
                    //    hf.LogException("Item cannot be readable due to State: " + tagName);
                    return -10000;
                }
            }
            else		// OPC server read error
            {
                //    hf.LogException("Item cannot be readable due to OPC: " + tagName);
                return -10000;
            }

        }




        public string WriteItem(String tagName,Int32 value)
        {

            ItemDef ItemData = new ItemDef();
            ItemData = readGroup.Item(tagName);
            OPCItemState state = new OPCItemState();

            int rtc = readGroup.Write(ItemData, value);

            if (HRESULTS.Succeeded(rtc))		// read from OPC server successful
            {
                if (state != null)
                {
                    if (HRESULTS.Succeeded(state.Error))	// item read successful
                    {
                        return "Success";
                    }
                    else			// the item could not be read
                    {
                        //           hf.LogException("Item cannot be readable: " + tagName);
                        return "ERROR";
                    }
                }
                else       // State not valid
                {
                    //      hf.LogException("Item cannot be readable due to state: " + tagName);
                    return "ERROR";
                }
            }
            else		// OPC server read error
            {
                //     hf.LogException("Item cannot be readable due to OPC: " + tagName);
                return "ERROR";
            }

        }


        public string WriteStringItem(String tagName, String value)
        {

            ItemDef ItemData = new ItemDef();
            ItemData = readGroup.Item(tagName);
            OPCItemState state = new OPCItemState();

            int rtc = readGroup.Write(ItemData, value);

            if (HRESULTS.Succeeded(rtc))		// read from OPC server successful
            {
                if (state != null)
                {
                    if (HRESULTS.Succeeded(state.Error))	// item read successful
                    {
                        return "Success";
                    }
                    else			// the item could not be read
                    {
                        //           hf.LogException("Item cannot be readable: " + tagName);
                        return "ERROR";
                    }
                }
                else       // State not valid
                {
                    //      hf.LogException("Item cannot be readable due to state: " + tagName);
                    return "ERROR";
                }
            }
            else		// OPC server read error
            {
                //     hf.LogException("Item cannot be readable due to OPC: " + tagName);
                return "ERROR";
            }

        }

    
    }


}
