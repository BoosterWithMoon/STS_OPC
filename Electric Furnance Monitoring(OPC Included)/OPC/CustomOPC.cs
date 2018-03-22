using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kepware.ClientAce.OpcDaClient;
using Kepware.ClientAce.OpcCmn;
using System.Runtime.InteropServices;

namespace Electric_Furnance_Monitoring_OPC_Included_
{
    class CustomOPC
    {
        MainForm main;

        DaServerMgt DAServer;
        ConnectInfo opcConnectInfo;
        ServerIdentifier[] availableOPCServers;
        bool connectFailed;
        OpcServerEnum serverEnum;
        string nodeName = "localhost";
        bool returnAllServers = false;
        public bool detected = false;
        int detectedIndex = 0;
        ServerCategory[] serverCategories = { ServerCategory.OPCDA };


        public CustomOPC(MainForm _main)
        {
            this.main = _main;
            DAServer = new DaServerMgt();
            opcConnectInfo = new ConnectInfo();
            serverEnum = new OpcServerEnum();
        }


        public void OPC_ClassTest()
        {
            DaServerMgt DAServer = new DaServerMgt();
        }

        public object OPCClass_forPublicRef() { return DAServer; }

        public void ServerDetection()
        {
            try
            {
                serverEnum.EnumComServer(nodeName, returnAllServers, serverCategories, out availableOPCServers);
                if (availableOPCServers.GetLength(0) > 0)
                {
                    for(int i=0; i<availableOPCServers.GetLength(0); i++)
                    {
                        if (availableOPCServers[i].ProgID == "Kepware.KEPServerEX.V6")
                        {
                            detected = true;
                            detectedIndex = i;
                            break;
                        }
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(nodeName + "에 연결할 수 있는 OPC 서버가 없습니다.");
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }

        public void ServerConnection()
        {
            if (detected == false)
            {
                System.Windows.Forms.MessageBox.Show("OPC 서버에 연결할 수 없습니다.");
                return;
            }
            else if (detected == true)
            {
                string url = availableOPCServers[detectedIndex].Url;
                int clientHandle = 1;
                opcConnectInfo.LocalId = "en";
                opcConnectInfo.KeepAliveTime = 1000;
                // Connection 성공 이후에 연결 Error가 발생 하면 서버 연결 재시도를 할 것인가?
                opcConnectInfo.RetryAfterConnectionError = true;
                // 초기 Connection에 실패했을 경우에 연결 재시도를 할 것인가?
                opcConnectInfo.RetryInitialConnection = false;

                try
                {
                    DAServer.Connect(url, clientHandle, ref opcConnectInfo, out connectFailed);
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("Handled Connect Exception.\n" + e.Message);
                    connectFailed = true;
                }

                if (connectFailed == true)
                {
                    System.Windows.Forms.MessageBox.Show("OPC 서버 연결에 실패하였습니다.", "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("OPC 서버 연결 성공", "OPC Connection", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
                }
                DAServer.ReadCompleted += new DaServerMgt.ReadCompletedEventHandler(daServerMgt_ReadCompleted);
            }
        }

        public void ServerDisconnection()
        {
            try
            {
                if (DAServer.IsConnected)
                {
                    DAServer.Disconnect();
                    System.Windows.Forms.MessageBox.Show("OPC 서버 연결 해제에 성공하였습니다.", "OPC Connection", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Handled Disconnect Exception.\n" + e.Message);
            }
        }

        public void OPC_Write()
        {
            if (main.OPC_textBox2.Text == "")
            {
                System.Windows.Forms.MessageBox.Show("write할 데이터가 없음");
                return;
            }
            ReturnCode returnCode;

            ItemIdentifier[] itemIdentifiers = new ItemIdentifier[1];
            itemIdentifiers[0] = new ItemIdentifier();
            itemIdentifiers[0].ItemName = "Channel2.TestDevice.No2";
            itemIdentifiers[0].ClientHandle = 0;

            // The itemValues array contains the values we wish to write to the items.
            ItemValue[] itemValues = new ItemValue[1];
            itemValues[0] = new ItemValue();
            itemValues[0].Value = System.Convert.ToInt32(main.OPC_textBox2.Text);

            int TransID = RandomNumber(65535, 1);

            // Call the Write API method:
            returnCode = DAServer.WriteAsync(TransID, ref itemIdentifiers, itemValues);

            // Handle result:
            if (returnCode != ReturnCode.SUCCEEDED)
            {
                System.Windows.Forms.MessageBox.Show("Async Write failed with a result of " + returnCode.ToString()/*System.Convert.ToString(itemIdentifiers[0].ResultID.Code) + "\r\n" + "Description: " + itemIdentifiers[0].ResultID.Description*/);
            }
        }

        public void OPC_Read()
        {
            ReturnCode returnCode;

            int maxAge = 0;
            int TransID = RandomNumber(65535, 1);

            ItemIdentifier[] itemIdentifiers = new ItemIdentifier[1];
            itemIdentifiers[0] = new ItemIdentifier();
            itemIdentifiers[0].ItemName = "Channel2.TestDevice.No2";
            itemIdentifiers[0].ClientHandle = 0;

            // Call the Read API method:
            returnCode = DAServer.ReadAsync(TransID, maxAge, ref itemIdentifiers);

            // Handle result:
            // Check result
            if (returnCode != ReturnCode.SUCCEEDED)
            {
                System.Windows.Forms.MessageBox.Show("ReadAsync failed with error: " + System.Convert.ToString(itemIdentifiers[0].ResultID.Code, 16) + "\r\n" + "Description: " + itemIdentifiers[0].ResultID.Description);
            }
        }

        public void daServerMgt_ReadCompleted(int transactionHandle, bool allQualitiesGood, bool noErrors, ItemValueCallback[] itemValues)
        {
            DaServerMgt.ReadCompletedEventHandler RCevHndlr = new DaServerMgt.ReadCompletedEventHandler(ReadCompleted);
            IAsyncResult returnValue;
            object[] RCevHndlrArray = new object[4];
            RCevHndlrArray[0] = transactionHandle;
            RCevHndlrArray[1] = allQualitiesGood;
            RCevHndlrArray[2] = noErrors;
            RCevHndlrArray[3] = itemValues;
            returnValue = main.BeginInvoke(RCevHndlr, RCevHndlrArray);
        }

        public void ReadCompleted(int transactionHandle, bool allQualitiesGood, bool noErrors, ItemValueCallback[] itemValues)
        {
            int itemIndex = (int)itemValues[0].ClientHandle;
            if (itemValues[0].ResultID.Succeeded)
            {
                if (itemValues[0].Value == null)
                {
                    main.OPC_textBox1.Text = "Unknown";
                }
                else
                {
                    main.OPC_textBox1.Text = itemValues[0].Value.ToString();
                }
            }
        }

        private int RandomNumber(int MaxNumber, int MinNumber)
        {
            //initialize random number generator
            Random r = new Random(System.DateTime.Now.Millisecond);

            //if passed incorrect arguments, swap them
            //can also throw exception or return 0

            if (MinNumber > MaxNumber)
            {
                int t = MinNumber;
                MinNumber = MaxNumber;
                MaxNumber = t;
            }

            return r.Next(MinNumber, MaxNumber);
        }

    }
}
