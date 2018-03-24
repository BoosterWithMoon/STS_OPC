using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kepware.ClientAce.OpcDaClient;
using Kepware.ClientAce.OpcCmn;
using System.Runtime.InteropServices;

namespace Electric_Furnance_Monitoring_OPC_Included_
{
   /* struct OPCData
    {
        #region ReadFromOPC
        public string Read_CurrentSteelNo;
        public string Read_CurrentAngle;
        public string Read_CurrentSteelKind;
        #endregion

        #region WriteToOPC
        public string CurrentAngle;
        public string[] CAM1_Threshold;
        public string[] CAM1_CurrentTemp;
        public string[] CAM2_Threshold;
        public string[] CAM2_CurrentTemp;
        public string CAM1_MaxTemp;
        public string CAM2_MaxTemp;
        #endregion
    }; */

    public enum ReadingArrayNo
    {
        CurrentSteelNo = 0,
        CurrentAngle = 1,
        CurrentSteelKind = 2
    }

    public enum WritingArrayNo
    {
        CurrentAngle=0,

        CAM1_MaxTemp=1,
        CAM1_Threshold1=2,
        CAM1_Threshold2 = 3,
        CAM1_Threshold3 = 4,
        CAM1_Threshold4 = 5,
        CAM1_Threshold5 = 6,
        CAM1_Threshold6 = 7,
        CAM1_Threshold7 = 8,
        CAM1_Threshold8 = 9,
        CAM1_Threshold9 = 10,
        CAM1_Threshold10 = 11,
        CAM1_CurrentTemp1 = 12,
        CAM1_CurrentTemp2 = 13,
        CAM1_CurrentTemp3 = 14,
        CAM1_CurrentTemp4 = 15,
        CAM1_CurrentTemp5 = 16,
        CAM1_CurrentTemp6 = 17,
        CAM1_CurrentTemp7 = 18,
        CAM1_CurrentTemp8 = 19,
        CAM1_CurrentTemp9 = 20,
        CAM1_CurrentTemp10 = 21,

        CAM2_MaxTemp = 22,
        CAM2_Threshold1 = 23,
        CAM2_Threshold2 = 24,
        CAM2_Threshold3 = 25,
        CAM2_Threshold4 = 26,
        CAM2_Threshold5 = 27,
        CAM2_Threshold6 = 28,
        CAM2_Threshold7 = 29,
        CAM2_Threshold8 = 30,
        CAM2_Threshold9 = 31,
        CAM2_Threshold10 = 32,
        CAM2_CurrentTemp1 = 33,
        CAM2_CurrentTemp2 = 34,
        CAM2_CurrentTemp3 = 35,
        CAM2_CurrentTemp4 = 36,
        CAM2_CurrentTemp5 = 37,
        CAM2_CurrentTemp6 = 38,
        CAM2_CurrentTemp7 = 39,
        CAM2_CurrentTemp8 = 40,
        CAM2_CurrentTemp9 = 41,
        CAM2_CurrentTemp10 = 42
    }

    public enum ClientHandleValue //Read=1        Write=2
    {
        #region ReadingHandle
        Read_CurrentSteelNo = 11,
        Read_CurrentAngle=12,
        Read_CurrentSteelKind=13,
        #endregion

        #region WritingHandle
        Write_CurrentAngle = 20,
        Write_CAM1_MaxTemp=21,
        Write_CAM2_MaxTemp=22,

        Write_CAM1_Threshold1=211,
        Write_CAM1_Threshold2 = 212,
        Write_CAM1_Threshold3 = 213,
        Write_CAM1_Threshold4 = 214,
        Write_CAM1_Threshold5 = 215,
        Write_CAM1_Threshold6 = 216,
        Write_CAM1_Threshold7 = 217,
        Write_CAM1_Threshold8 = 218,
        Write_CAM1_Threshold9 = 219,
        Write_CAM1_Threshold10 = 220,
        Write_CAM1_CurrentTemp1 = 221,
        Write_CAM1_CurrentTemp2 = 222,
        Write_CAM1_CurrentTemp3 = 223,
        Write_CAM1_CurrentTemp4 = 224,
        Write_CAM1_CurrentTemp5 = 225,
        Write_CAM1_CurrentTemp6 = 226,
        Write_CAM1_CurrentTemp7 = 227,
        Write_CAM1_CurrentTemp8 = 228,
        Write_CAM1_CurrentTemp9 = 229,
        Write_CAM1_CurrentTemp10 = 230,

        Write_CAM2_Threshold1 = 231,
        Write_CAM2_Threshold2 = 232,
        Write_CAM2_Threshold3 = 233,
        Write_CAM2_Threshold4 = 234,
        Write_CAM2_Threshold5 = 235,
        Write_CAM2_Threshold6 = 236,
        Write_CAM2_Threshold7 = 237,
        Write_CAM2_Threshold8 = 238,
        Write_CAM2_Threshold9 = 239,
        Write_CAM2_Threshold10 = 240,
        Write_CAM2_CurrentTemp1 = 241,
        Write_CAM2_CurrentTemp2 = 242,
        Write_CAM2_CurrentTemp3 = 243,
        Write_CAM2_CurrentTemp4 = 244,
        Write_CAM2_CurrentTemp5 = 245,
        Write_CAM2_CurrentTemp6 = 246,
        Write_CAM2_CurrentTemp7 = 247,
        Write_CAM2_CurrentTemp8 = 248,
        Write_CAM2_CurrentTemp9 = 249,
        Write_CAM2_CurrentTemp10 = 250
        #endregion
    }

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

        //ClientHandleValue handleValue;

        // 연결시킬 OPC 태그가 존재하는 채널 / Device ID
        private static string Channel = "Channel4";
        private static string Device = ".Device1";

        // Read 또는 Write할 OPC 태그의 개수
        private static int ReadTagCount = 3;
        private static int WriteTagCount = 43;

        public int CurrentAngle;

        ResultView result;
        ImageView imgView;

        ItemIdentifier[] Read_itemIdentifiers;

        ItemIdentifier[] Write_itemIdentifiers;
        ItemValue[] Write_itemValues;

        public System.Threading.Timer DataTransferTimer;

        public CustomOPC(MainForm _main)
        {
            this.main = _main;
            DAServer = new DaServerMgt();
            opcConnectInfo = new ConnectInfo();
            serverEnum = new OpcServerEnum();

            result = (ResultView)main.ResultView_forPublicRef();
            imgView = (ImageView)main.ImageView_forPublicRef();

            Read_itemIdentifiers = new ItemIdentifier[ReadTagCount];

            Write_itemIdentifiers = new ItemIdentifier[WriteTagCount];
            Write_itemValues = new ItemValue[WriteTagCount];
        }

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

        private void WriteDataDefinition()
        {
            for(int i=0; i<WriteTagCount; i++)
            {
                Write_itemIdentifiers[i] = new ItemIdentifier();
            }
            Write_itemIdentifiers[(int)WritingArrayNo.CurrentAngle].ItemName = Channel + Device + ".CurrentAngle";
            Write_itemIdentifiers[(int)WritingArrayNo.CurrentAngle].ClientHandle = ClientHandleValue.Write_CurrentAngle;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_MaxTemp].ItemName = Channel + Device + ".CAM1_MaxTemp";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_MaxTemp].ClientHandle = ClientHandleValue.Write_CAM1_MaxTemp;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold1].ItemName = Channel + Device + ".CAM1_Threshold1";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold1].ClientHandle = ClientHandleValue.Write_CAM1_Threshold1;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold2].ItemName = Channel + Device + ".CAM1_Threshold2";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold2].ClientHandle = ClientHandleValue.Write_CAM1_Threshold2;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold3].ItemName = Channel + Device + ".CAM1_Threshold3";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold3].ClientHandle = ClientHandleValue.Write_CAM1_Threshold3;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold4].ItemName = Channel + Device + ".CAM1_Threshold4";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold4].ClientHandle = ClientHandleValue.Write_CAM1_Threshold4;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold5].ItemName = Channel + Device + ".CAM1_Threshold5";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold5].ClientHandle = ClientHandleValue.Write_CAM1_Threshold5;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold6].ItemName = Channel + Device + ".CAM1_Threshold6";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold6].ClientHandle = ClientHandleValue.Write_CAM1_Threshold6;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold7].ItemName = Channel + Device + ".CAM1_Threshold7";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold7].ClientHandle = ClientHandleValue.Write_CAM1_Threshold7;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold8].ItemName = Channel + Device + ".CAM1_Threshold8";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold8].ClientHandle = ClientHandleValue.Write_CAM1_Threshold8;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold9].ItemName = Channel + Device + ".CAM1_Threshold9";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold9].ClientHandle = ClientHandleValue.Write_CAM1_Threshold9;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold10].ItemName = Channel + Device + ".CAM1_Threshold10";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold10].ClientHandle = ClientHandleValue.Write_CAM1_Threshold10;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_CurrentTemp1].ItemName = Channel + Device + ".CAM1_CurrentTemp1";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_CurrentTemp1].ClientHandle = ClientHandleValue.Write_CAM1_CurrentTemp1;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_CurrentTemp2].ItemName = Channel + Device + ".CAM1_CurrentTemp2";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_CurrentTemp2].ClientHandle = ClientHandleValue.Write_CAM1_CurrentTemp2;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_CurrentTemp3].ItemName = Channel + Device + ".CAM1_CurrentTemp3";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_CurrentTemp3].ClientHandle = ClientHandleValue.Write_CAM1_CurrentTemp3;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_CurrentTemp4].ItemName = Channel + Device + ".CAM1_CurrentTemp4";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_CurrentTemp4].ClientHandle = ClientHandleValue.Write_CAM1_CurrentTemp4;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_CurrentTemp5].ItemName = Channel + Device + ".CAM1_CurrentTemp5";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_CurrentTemp5].ClientHandle = ClientHandleValue.Write_CAM1_CurrentTemp5;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_CurrentTemp6].ItemName = Channel + Device + ".CAM1_CurrentTemp6";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_CurrentTemp6].ClientHandle = ClientHandleValue.Write_CAM1_CurrentTemp6;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_CurrentTemp7].ItemName = Channel + Device + ".CAM1_CurrentTemp7";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_CurrentTemp7].ClientHandle = ClientHandleValue.Write_CAM1_CurrentTemp7;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_CurrentTemp8].ItemName = Channel + Device + ".CAM1_CurrentTemp8";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_CurrentTemp8].ClientHandle = ClientHandleValue.Write_CAM1_CurrentTemp8;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_CurrentTemp9].ItemName = Channel + Device + ".CAM1_CurrentTemp9";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_CurrentTemp9].ClientHandle = ClientHandleValue.Write_CAM1_CurrentTemp9;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_CurrentTemp10].ItemName = Channel + Device + ".CAM1_CurrentTemp10";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_CurrentTemp10].ClientHandle = ClientHandleValue.Write_CAM1_CurrentTemp10;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_MaxTemp].ItemName = Channel + Device + ".CAM2_MaxTemp";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_MaxTemp].ClientHandle = ClientHandleValue.Write_CAM2_MaxTemp;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold1].ItemName = Channel + Device + ".CAM2_Threshold1";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold1].ClientHandle = ClientHandleValue.Write_CAM2_Threshold1;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold2].ItemName = Channel + Device + ".CAM2_Threshold2";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold2].ClientHandle = ClientHandleValue.Write_CAM2_Threshold2;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold3].ItemName = Channel + Device + ".CAM2_Threshold3";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold3].ClientHandle = ClientHandleValue.Write_CAM2_Threshold3;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold4].ItemName = Channel + Device + ".CAM2_Threshold4";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold4].ClientHandle = ClientHandleValue.Write_CAM2_Threshold4;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold5].ItemName = Channel + Device + ".CAM2_Threshold5";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold5].ClientHandle = ClientHandleValue.Write_CAM2_Threshold5;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold6].ItemName = Channel + Device + ".CAM2_Threshold6";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold6].ClientHandle = ClientHandleValue.Write_CAM2_Threshold6;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold7].ItemName = Channel + Device + ".CAM2_Threshold7";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold7].ClientHandle = ClientHandleValue.Write_CAM2_Threshold7;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold8].ItemName = Channel + Device + ".CAM2_Threshold8";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold8].ClientHandle = ClientHandleValue.Write_CAM2_Threshold8;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold9].ItemName = Channel + Device + ".CAM2_Threshold9";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold9].ClientHandle = ClientHandleValue.Write_CAM2_Threshold9;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold10].ItemName = Channel + Device + ".CAM2_Threshold10";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold10].ClientHandle = ClientHandleValue.Write_CAM2_Threshold10;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_CurrentTemp1].ItemName = Channel + Device + ".CAM2_CurrentTemp1";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_CurrentTemp1].ClientHandle = ClientHandleValue.Write_CAM2_CurrentTemp1;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_CurrentTemp2].ItemName = Channel + Device + ".CAM2_CurrentTemp2";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_CurrentTemp2].ClientHandle = ClientHandleValue.Write_CAM2_CurrentTemp2;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_CurrentTemp3].ItemName = Channel + Device + ".CAM2_CurrentTemp3";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_CurrentTemp3].ClientHandle = ClientHandleValue.Write_CAM2_CurrentTemp3;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_CurrentTemp4].ItemName = Channel + Device + ".CAM2_CurrentTemp4";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_CurrentTemp4].ClientHandle = ClientHandleValue.Write_CAM2_CurrentTemp4;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_CurrentTemp5].ItemName = Channel + Device + ".CAM2_CurrentTemp5";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_CurrentTemp5].ClientHandle = ClientHandleValue.Write_CAM2_CurrentTemp5;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_CurrentTemp6].ItemName = Channel + Device + ".CAM2_CurrentTemp6";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_CurrentTemp6].ClientHandle = ClientHandleValue.Write_CAM2_CurrentTemp6;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_CurrentTemp7].ItemName = Channel + Device + ".CAM2_CurrentTemp7";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_CurrentTemp7].ClientHandle = ClientHandleValue.Write_CAM2_CurrentTemp7;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_CurrentTemp8].ItemName = Channel + Device + ".CAM2_CurrentTemp8";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_CurrentTemp8].ClientHandle = ClientHandleValue.Write_CAM2_CurrentTemp8;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_CurrentTemp9].ItemName = Channel + Device + ".CAM2_CurrentTemp9";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_CurrentTemp9].ClientHandle = ClientHandleValue.Write_CAM2_CurrentTemp9;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_CurrentTemp10].ItemName = Channel + Device + ".CAM2_CurrentTemp10";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_CurrentTemp10].ClientHandle = ClientHandleValue.Write_CAM2_CurrentTemp10;


            for (int i=0; i<WriteTagCount; i++)
            {
                Write_itemValues[i] = new ItemValue();
            }
            Write_itemValues[(int)WritingArrayNo.CurrentAngle].Value = CurrentAngle;
            Write_itemValues[(int)WritingArrayNo.CAM1_MaxTemp].Value = main.FloatMaxTemp;
            Write_itemValues[(int)WritingArrayNo.CAM1_Threshold1].Value = result.CAM1_ThresholdTemp[0];
            Write_itemValues[(int)WritingArrayNo.CAM1_Threshold2].Value = result.CAM1_ThresholdTemp[1];
            Write_itemValues[(int)WritingArrayNo.CAM1_Threshold3].Value = result.CAM1_ThresholdTemp[2];
            Write_itemValues[(int)WritingArrayNo.CAM1_Threshold4].Value = result.CAM1_ThresholdTemp[3];
            Write_itemValues[(int)WritingArrayNo.CAM1_Threshold5].Value = result.CAM1_ThresholdTemp[4];
            Write_itemValues[(int)WritingArrayNo.CAM1_Threshold6].Value = result.CAM1_ThresholdTemp[5];
            Write_itemValues[(int)WritingArrayNo.CAM1_Threshold7].Value = result.CAM1_ThresholdTemp[6];
            Write_itemValues[(int)WritingArrayNo.CAM1_Threshold8].Value = result.CAM1_ThresholdTemp[7];
            Write_itemValues[(int)WritingArrayNo.CAM1_Threshold9].Value = result.CAM1_ThresholdTemp[8];
            Write_itemValues[(int)WritingArrayNo.CAM1_Threshold10].Value = result.CAM1_ThresholdTemp[9];
            Write_itemValues[(int)WritingArrayNo.CAM1_CurrentTemp1].Value = imgView.CAM1_TemperatureArr[0];
            Write_itemValues[(int)WritingArrayNo.CAM1_CurrentTemp2].Value = imgView.CAM1_TemperatureArr[1];
            Write_itemValues[(int)WritingArrayNo.CAM1_CurrentTemp3].Value = imgView.CAM1_TemperatureArr[2];
            Write_itemValues[(int)WritingArrayNo.CAM1_CurrentTemp4].Value = imgView.CAM1_TemperatureArr[3];
            Write_itemValues[(int)WritingArrayNo.CAM1_CurrentTemp5].Value = imgView.CAM1_TemperatureArr[4];
            Write_itemValues[(int)WritingArrayNo.CAM1_CurrentTemp6].Value = imgView.CAM1_TemperatureArr[5];
            Write_itemValues[(int)WritingArrayNo.CAM1_CurrentTemp7].Value = imgView.CAM1_TemperatureArr[6];
            Write_itemValues[(int)WritingArrayNo.CAM1_CurrentTemp8].Value = imgView.CAM1_TemperatureArr[7];
            Write_itemValues[(int)WritingArrayNo.CAM1_CurrentTemp9].Value = imgView.CAM1_TemperatureArr[8];
            Write_itemValues[(int)WritingArrayNo.CAM1_CurrentTemp10].Value = imgView.CAM1_TemperatureArr[9];
            Write_itemValues[(int)WritingArrayNo.CAM2_MaxTemp].Value = main.c2_FloatMaxTemp;
            Write_itemValues[(int)WritingArrayNo.CAM2_Threshold1].Value = result.CAM2_ThresholdTemp[0];
            Write_itemValues[(int)WritingArrayNo.CAM2_Threshold2].Value = result.CAM2_ThresholdTemp[1];
            Write_itemValues[(int)WritingArrayNo.CAM2_Threshold3].Value = result.CAM2_ThresholdTemp[2];
            Write_itemValues[(int)WritingArrayNo.CAM2_Threshold4].Value = result.CAM2_ThresholdTemp[3];
            Write_itemValues[(int)WritingArrayNo.CAM2_Threshold5].Value = result.CAM2_ThresholdTemp[4];
            Write_itemValues[(int)WritingArrayNo.CAM2_Threshold6].Value = result.CAM2_ThresholdTemp[5];
            Write_itemValues[(int)WritingArrayNo.CAM2_Threshold7].Value = result.CAM2_ThresholdTemp[6];
            Write_itemValues[(int)WritingArrayNo.CAM2_Threshold8].Value = result.CAM2_ThresholdTemp[7];
            Write_itemValues[(int)WritingArrayNo.CAM2_Threshold9].Value = result.CAM2_ThresholdTemp[8];
            Write_itemValues[(int)WritingArrayNo.CAM2_Threshold10].Value = result.CAM2_ThresholdTemp[9];
            Write_itemValues[(int)WritingArrayNo.CAM2_CurrentTemp1].Value = imgView.CAM2_TemperatureArr[0];
            Write_itemValues[(int)WritingArrayNo.CAM2_CurrentTemp2].Value = imgView.CAM2_TemperatureArr[1];
            Write_itemValues[(int)WritingArrayNo.CAM2_CurrentTemp3].Value = imgView.CAM2_TemperatureArr[2];
            Write_itemValues[(int)WritingArrayNo.CAM2_CurrentTemp4].Value = imgView.CAM2_TemperatureArr[3];
            Write_itemValues[(int)WritingArrayNo.CAM2_CurrentTemp5].Value = imgView.CAM2_TemperatureArr[4];
            Write_itemValues[(int)WritingArrayNo.CAM2_CurrentTemp6].Value = imgView.CAM2_TemperatureArr[5];
            Write_itemValues[(int)WritingArrayNo.CAM2_CurrentTemp7].Value = imgView.CAM2_TemperatureArr[6];
            Write_itemValues[(int)WritingArrayNo.CAM2_CurrentTemp8].Value = imgView.CAM2_TemperatureArr[7];
            Write_itemValues[(int)WritingArrayNo.CAM2_CurrentTemp9].Value = imgView.CAM2_TemperatureArr[8];
            Write_itemValues[(int)WritingArrayNo.CAM2_CurrentTemp10].Value = imgView.CAM2_TemperatureArr[9];
        }

        public void OPC_Write()
        {
            //if (main.OPC_textBox2.Text == "")
            //{
            //    System.Windows.Forms.MessageBox.Show("write할 데이터가 없음");
            //    return;
            //}
            ReturnCode returnCode;

            // Write는 이렇게 한쌍 ==============================================
            // Write 해줄 태그 지정
            //ItemIdentifier[] itemIdentifiers = new ItemIdentifier[1];
            //itemIdentifiers[0] = new ItemIdentifier();
            ////itemIdentifiers[0].ItemName = "Channel2.TestDevice.No2";
            //itemIdentifiers[0].ItemName = Channel + Device + ".CurrentAngle";
            //itemIdentifiers[0].ClientHandle = ClientHandleValue.Write_CurrentAngle;

            //// 지정해준 태그에 Write하고자 하는 데이터 지정
            //ItemValue[] itemValues = new ItemValue[1];
            //itemValues[0] = new ItemValue();
            //itemValues[0].Value = System.Convert.ToInt32(CurrentAngle);
            // ============================================== Write는 이렇게 한쌍
            WriteDataDefinition();

            int TransID = RandomNumber(65535, 1);

            // Call the Write API method:
            returnCode = DAServer.WriteAsync(TransID, ref Write_itemIdentifiers, Write_itemValues);

            // Handle result:
            if (returnCode != ReturnCode.SUCCEEDED)
            {
                System.Windows.Forms.MessageBox.Show("Async Write failed with a result of " + returnCode.ToString()/*System.Convert.ToString(itemIdentifiers[0].ResultID.Code) + "\r\n" + "Description: " + itemIdentifiers[0].ResultID.Description*/);
            }
        }

        private void ReadDataDefintion()
        {
            for(int i=0; i<ReadTagCount; i++)
            {
                Read_itemIdentifiers[i] = new ItemIdentifier();
            }
            Read_itemIdentifiers[(int)ReadingArrayNo.CurrentSteelNo].ItemName = Channel + Device + ".R_CurrentSteelNo";
            Read_itemIdentifiers[(int)ReadingArrayNo.CurrentSteelNo].ClientHandle = ClientHandleValue.Read_CurrentSteelNo;

            Read_itemIdentifiers[(int)ReadingArrayNo.CurrentAngle].ItemName = Channel + Device + ".R_CurrentAngle";
            Read_itemIdentifiers[(int)ReadingArrayNo.CurrentAngle].ClientHandle = ClientHandleValue.Read_CurrentAngle;

            Read_itemIdentifiers[(int)ReadingArrayNo.CurrentSteelKind].ItemName = Channel + Device + ".R_CurrentSteelKind";
            Read_itemIdentifiers[(int)ReadingArrayNo.CurrentSteelKind].ClientHandle = ClientHandleValue.Read_CurrentSteelKind;
        }

        public void OPC_Read()
        {
            ReturnCode returnCode;

            ReadDataDefintion();

            int maxAge = 0;
            int TransID = RandomNumber(65535, 1);

            // Call the Read API method:
            returnCode = DAServer.ReadAsync(TransID, maxAge, ref Read_itemIdentifiers);

            // Check result for called API
            if (returnCode != ReturnCode.SUCCEEDED)
            {
                System.Windows.Forms.MessageBox.Show("ReadAsync failed with error: " + returnCode.ToString() /*System.Convert.ToString(itemIdentifiers[0].ResultID.Code, 16) + "\r\n" + "Description: " + itemIdentifiers[0].ResultID.Description*/);
            }                
        }

        private void daServerMgt_ReadCompleted(int transactionHandle, bool allQualitiesGood, bool noErrors, ItemValueCallback[] itemValues)
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

        private void ReadCompleted(int transactionHandle, bool allQualitiesGood, bool noErrors, ItemValueCallback[] itemValues)
        {
            object[] ReadingResult = new object[itemValues.Length];

            ReadingResult[0] = itemValues[(int)ReadingArrayNo.CurrentAngle].Value;
            ReadingResult[1] = itemValues[(int)ReadingArrayNo.CurrentSteelKind].Value;
            ReadingResult[2] = itemValues[(int)ReadingArrayNo.CurrentSteelNo].Value;

            // integer형은 System.UInt16, string형은 System.String으로 받아와진다.
            //string b1 = ReadingResult[0].GetType().ToString();
            //string b2 = ReadingResult[1].GetType().ToString();
            //string b3 = ReadingResult[2].GetType().ToString();

            // DataType을 비교해서
            for(int i=0; i<itemValues.Length; i++)
            {
                if(ReadingResult[i].GetType().ToString() == "System.String")    // string형이면 강종이고
                {
                    result.textBox_CurrentSteelKind.Text = ReadingResult[i].ToString();
                }
                else if(ReadingResult[i].GetType().ToString() == "System.UInt16" &&     // ushort형인데 90보다 값이 작으면 현재 각도
                    Convert.ToUInt16(ReadingResult[i]) <= 90)
                {
                    CurrentAngle = Convert.ToUInt16(ReadingResult[i]);
                }
                else if(ReadingResult[i].GetType().ToString() == "System.UInt16" &&     // ushort형인데 나머지 숫자값이면 강번으로 처리
                    Convert.ToUInt16(ReadingResult[i]) > 90)
                {
                    main.textBox1.Text = ReadingResult[i].ToString();
                    main.textBox2.Text = ReadingResult[i].ToString();
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

        //System.Threading.Timer DataTransferTimer = new System.Threading.Timer(new System.Threading.TimerCallback(WriteCallback), null, 0, 1000);
        public bool isOPCTransferring = false;

        static void WriteCallback(object state)
        {
            
        }


    }
}
