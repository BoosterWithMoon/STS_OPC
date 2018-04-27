using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kepware.ClientAce.OpcDaClient;
using Kepware.ClientAce.OpcCmn;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Collections;

namespace Electric_Furnance_Monitoring_OPC_Included_
{
    #region StringEnumDefinition
    public class StringValue : System.Attribute
    {
        private string _value;

        public StringValue(string value) { _value = value; }
        public string Value { get { return _value; } }
    }

    public static class StringEnum
    {
        public static string GetStringValue(ReadItemIDs value)
        {
            string output = null;

            Type type = value.GetType();

            _FieldInfo fi = type.GetField(value.ToString());
            StringValue[] attrs = fi.GetCustomAttributes(typeof(StringValue), false) as StringValue[];

            if (attrs.Length > 0) { output = attrs[0].Value; }
            return output;
        }
    }
    #endregion

    public enum ReadingArrayNo
    {
        CurrentSteelNo = 0,     // DWORD
        CurrentAngle = 1,       // WORD
        CurrentSteelKind = 2,   // String

        // BOOL
        //Charging1_Progress = 3,
        //Melting1_Progress = 4,
        //Charging2_Progress = 5,
        //Melting2_Progress = 6,
        //Charging3_Progress = 7,
        //Melting3_Progress = 8,
        //Stand_Steel_Progress = 9,
        //Tapping_Progress = 10,
        //O2Lance_Blowing = 11
        //BoolArray = 3

        RunningProcess = 3,    // Byte
        O2Lance_Blowing=4,
    }

    // String Enum
    public enum ReadItemIDs
    {
        [StringValue(".R_CurrentSteelNo")]
        CurrentSteelNo,
        [StringValue(".R_Slope_Angle")]
        CurrentAngle,
        [StringValue(".R_CurrentSteelKind")]
        CurrentSteelKind,
        //[StringValue(".R_Charging1_Progress")]
        //Charging1_Progress,
        //[StringValue(".R_Melting1_Progress")]
        //Melting1_Progress,
        //[StringValue(".R_Charging2_Progress")]
        //Charging2_Progress,
        //[StringValue(".R_Melting2_Progress")]
        //Melting2_Progress,
        //[StringValue(".R_Charging3_Progress")]
        //Charging3_Progress,
        //[StringValue(".R_Melting3_Progress")]
        //Melting3_Progress,
        //[StringValue(".R_Stand_Steel_Progress")]
        //StandSteelProgress,
        //[StringValue(".R_Tapping_Progress")]
        //TappingProgress,
        //[StringValue(".R_O2_Lance_Blowing")]
        //O2LanceBlowing
        //[StringValue(".R_BoolArray")]
        //BooleanArray
        [StringValue(".R_Progress")]
        RunningProcess,
        [StringValue(".R_O2_Lance_Blowing")]
        O2LanceBlowing
    }

    public enum WritingArrayNo
    {
        CurrentAngle=0,

        CAM1_MaxTemp=1,
        CAM1_Threshold01=2,
        CAM1_Threshold02 = 3,
        CAM1_Threshold03 = 4,
        CAM1_Threshold04 = 5,
        CAM1_Threshold05 = 6,
        CAM1_Threshold06 = 7,
        CAM1_Threshold07 = 8,
        CAM1_Threshold08 = 9,
        CAM1_Threshold09 = 10,
        CAM1_Threshold10 = 11,
        CAM1_ROI01_Temp = 12,
        CAM1_ROI02_Temp = 13,
        CAM1_ROI03_Temp = 14,
        CAM1_ROI04_Temp = 15,
        CAM1_ROI05_Temp = 16,
        CAM1_ROI06_Temp = 17,
        CAM1_ROI07_Temp = 18,
        CAM1_ROI08_Temp = 19,
        CAM1_ROI09_Temp = 20,
        CAM1_ROI10_Temp = 21,

        CAM2_MaxTemp = 22,
        CAM2_Threshold01 = 23,
        CAM2_Threshold02 = 24,
        CAM2_Threshold03 = 25,
        CAM2_Threshold04 = 26,
        CAM2_Threshold05 = 27,
        CAM2_Threshold06 = 28,
        CAM2_Threshold07 = 29,
        CAM2_Threshold08 = 30,
        CAM2_Threshold09 = 31,
        CAM2_Threshold10 = 32,
        CAM2_ROI01_Temp = 33,
        CAM2_ROI02_Temp = 34,
        CAM2_ROI03_Temp = 35,
        CAM2_ROI04_Temp = 36,
        CAM2_ROI05_Temp = 37,
        CAM2_ROI06_Temp = 38,
        CAM2_ROI07_Temp = 39,
        CAM2_ROI08_Temp = 40,
        CAM2_ROI09_Temp = 41,
        CAM2_ROI10_Temp = 42,

        CAM1_ROI01_WAR = 43,
        CAM1_ROI02_WAR = 44,
        CAM1_ROI03_WAR = 45,
        CAM1_ROI04_WAR = 46,
        CAM1_ROI05_WAR = 47,
        CAM1_ROI06_WAR = 48,
        CAM1_ROI07_WAR = 49,
        CAM1_ROI08_WAR = 50,
        CAM1_ROI09_WAR = 51,
        CAM1_ROI10_WAR = 52,

        CAM2_ROI01_WAR = 53,
        CAM2_ROI02_WAR = 54,
        CAM2_ROI03_WAR = 55,
        CAM2_ROI04_WAR = 56,
        CAM2_ROI05_WAR = 57,
        CAM2_ROI06_WAR = 58,
        CAM2_ROI07_WAR = 59,
        CAM2_ROI08_WAR = 60,
        CAM2_ROI09_WAR = 61,
        CAM2_ROI10_WAR = 62,

        CAM1_ROI01_ALM = 63,
        CAM1_ROI02_ALM = 64,
        CAM1_ROI03_ALM = 65,
        CAM1_ROI04_ALM = 66,
        CAM1_ROI05_ALM = 67,
        CAM1_ROI06_ALM = 68,
        CAM1_ROI07_ALM = 69,
        CAM1_ROI08_ALM = 70,
        CAM1_ROI09_ALM = 71,
        CAM1_ROI10_ALM = 72,

        CAM2_ROI01_ALM = 73,
        CAM2_ROI02_ALM = 74,
        CAM2_ROI03_ALM = 75,
        CAM2_ROI04_ALM = 76,
        CAM2_ROI05_ALM = 77,
        CAM2_ROI06_ALM = 78,
        CAM2_ROI07_ALM = 79,
        CAM2_ROI08_ALM = 80,
        CAM2_ROI09_ALM = 81,
        CAM2_ROI10_ALM = 82,
    }

    public enum ClientHandleValue //Read=1        Write=2
    {
        #region ReadingHandle
        Read_CurrentSteelNo = 101,
        Read_CurrentAngle=102,
        Read_CurrentSteelKind=103,

        Read_Charging1_Progress = 104,
        Read_Melting1_Progress = 105,
        Read_Charging2_Progress = 106,
        Read_Melting2_Progress = 107,
        Read_Charging3_Progress = 108,
        Read_Melting3_Progress = 109,
        Read_Stand_Steel_Progress = 110,
        Read_Tapping_Progress = 111,
        Read_O2Lance_Blowing = 112,

        Read_BoolArray = 113,

        Read_RunningProcess=114,
        
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
        Write_CAM2_CurrentTemp10 = 250,

        Write_CAM1_ROI01_WAR = 251,
        Write_CAM1_ROI02_WAR = 252,
        Write_CAM1_ROI03_WAR = 253,
        Write_CAM1_ROI04_WAR = 254,
        Write_CAM1_ROI05_WAR = 255,
        Write_CAM1_ROI06_WAR = 256,
        Write_CAM1_ROI07_WAR = 257,
        Write_CAM1_ROI08_WAR = 258,
        Write_CAM1_ROI09_WAR = 259,
        Write_CAM1_ROI10_WAR = 260,

        Write_CAM2_ROI01_WAR = 261,
        Write_CAM2_ROI02_WAR = 262,
        Write_CAM2_ROI03_WAR = 263,
        Write_CAM2_ROI04_WAR = 264,
        Write_CAM2_ROI05_WAR = 265,
        Write_CAM2_ROI06_WAR = 266,
        Write_CAM2_ROI07_WAR = 267,
        Write_CAM2_ROI08_WAR = 268,
        Write_CAM2_ROI09_WAR = 269,
        Write_CAM2_ROI10_WAR = 270,

        Write_CAM1_ROI01_ALM = 271,
        Write_CAM1_ROI02_ALM = 272,
        Write_CAM1_ROI03_ALM = 273,
        Write_CAM1_ROI04_ALM = 274,
        Write_CAM1_ROI05_ALM = 275,
        Write_CAM1_ROI06_ALM = 276,
        Write_CAM1_ROI07_ALM = 277,
        Write_CAM1_ROI08_ALM = 278,
        Write_CAM1_ROI09_ALM = 279,
        Write_CAM1_ROI10_ALM = 280,

        Write_CAM2_ROI01_ALM = 281,
        Write_CAM2_ROI02_ALM = 282,
        Write_CAM2_ROI03_ALM = 283,
        Write_CAM2_ROI04_ALM = 284,
        Write_CAM2_ROI05_ALM = 285,
        Write_CAM2_ROI06_ALM = 286,
        Write_CAM2_ROI07_ALM = 287,
        Write_CAM2_ROI08_ALM = 288,
        Write_CAM2_ROI09_ALM = 289,
        Write_CAM2_ROI10_ALM = 290
        #endregion
    }

    class CustomOPC
    {
        MainForm main;

        DaServerMgt DAServer;
        ConnectInfo opcConnectInfo;
        ServerIdentifier[] availableOPCServers;
        public bool connectFailed;
        OpcServerEnum serverEnum;

        bool returnAllServers;
        public bool detected;
        int detectedIndex;
        // OPC DA 지정
        ServerCategory[] serverCategories = { ServerCategory.OPCDA };

        // 연결시킬 OPC 태그가 존재하는 채널 / Device ID
        public string Channel;
        public string Device;

        // Kepware가 설치된 컴퓨터 노드 이름
        public string nodeName;

        // Read 또는 Write할 OPC 태그의 개수
        public int ReadTagCount;
        public int WriteTagCount;

        public int CurrentAngle;

        ResultView result;
        ImageView imgView;

        ItemIdentifier[] Read_itemIdentifiers;
        ItemIdentifier[] Write_itemIdentifiers;
        ItemValue[] Write_itemValues;

        BitArray ChargingStatus;
        bool O2LanceResult;

        public CustomOPC(MainForm _main)
        {
            this.main = _main;
            LoadOPCConfiguration();

            DAServer = new DaServerMgt();
            opcConnectInfo = new ConnectInfo();
            connectFailed = true;
            serverEnum = new OpcServerEnum();

            returnAllServers = false;
            detected = false;
            detectedIndex = 0;

            ChargingStatus = new BitArray(8, false);
            O2LanceResult = false;

            result = (ResultView)main.ResultView_forPublicRef();
            imgView = (ImageView)main.ImageView_forPublicRef();

            Read_itemIdentifiers = new ItemIdentifier[ReadTagCount];

            Write_itemIdentifiers = new ItemIdentifier[WriteTagCount];
            Write_itemValues = new ItemValue[WriteTagCount];
        }

        private void LoadOPCConfiguration()
        {
            string value = "";
            value = ConfigurationManager.AppSettings["OPC_Channel"];
            Channel = value;
            value = ConfigurationManager.AppSettings["OPC_Device"];
            Device = value;
            value = ConfigurationManager.AppSettings["OPC_Endpoint"];
            nodeName = value;

            value = ConfigurationManager.AppSettings["OPC_ReadTagCount"];
            ReadTagCount = Convert.ToInt32(value);
            value = ConfigurationManager.AppSettings["OPC_WriteTagCount"];
            WriteTagCount = Convert.ToInt32(value);
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
                //exceptionOccured = true;
            }
        }

        public void ServerConnection()
        {
            if (detected == false /*&& exceptionOccured == false*/)
            {
                System.Windows.Forms.MessageBox.Show("OPC 서버 연결에 실패하였습니다.", "OPC Connection", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            else if (detected == true)
            {
                string url = availableOPCServers[detectedIndex].Url;
                int clientHandle = 1;
                opcConnectInfo.LocalId = "en";
                opcConnectInfo.KeepAliveTime = 1000;
                // Connection 성공 이후에 연결 Error가 발생 하면 서버 연결 재시도를 할 것인가?
                opcConnectInfo.RetryAfterConnectionError = false;
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
                    System.Windows.Forms.MessageBox.Show("OPC 서버 연결에 실패하였습니다.", "OPC Connection", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
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
            Write_itemIdentifiers[(int)WritingArrayNo.CurrentAngle].ItemName = Channel + Device + ".Slope_Angle";
            Write_itemIdentifiers[(int)WritingArrayNo.CurrentAngle].ClientHandle = ClientHandleValue.Write_CurrentAngle;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_MaxTemp].ItemName = Channel + Device + ".CAM1_MaxTemp";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_MaxTemp].ClientHandle = ClientHandleValue.Write_CAM1_MaxTemp;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold01].ItemName = Channel + Device + ".CAM1_Threshold01";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold01].ClientHandle = ClientHandleValue.Write_CAM1_Threshold1;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold02].ItemName = Channel + Device + ".CAM1_Threshold02";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold02].ClientHandle = ClientHandleValue.Write_CAM1_Threshold2;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold03].ItemName = Channel + Device + ".CAM1_Threshold03";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold03].ClientHandle = ClientHandleValue.Write_CAM1_Threshold3;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold04].ItemName = Channel + Device + ".CAM1_Threshold04";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold04].ClientHandle = ClientHandleValue.Write_CAM1_Threshold4;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold05].ItemName = Channel + Device + ".CAM1_Threshold05";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold05].ClientHandle = ClientHandleValue.Write_CAM1_Threshold5;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold06].ItemName = Channel + Device + ".CAM1_Threshold06";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold06].ClientHandle = ClientHandleValue.Write_CAM1_Threshold6;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold07].ItemName = Channel + Device + ".CAM1_Threshold07";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold07].ClientHandle = ClientHandleValue.Write_CAM1_Threshold7;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold08].ItemName = Channel + Device + ".CAM1_Threshold08";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold08].ClientHandle = ClientHandleValue.Write_CAM1_Threshold8;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold09].ItemName = Channel + Device + ".CAM1_Threshold09";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold09].ClientHandle = ClientHandleValue.Write_CAM1_Threshold9;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold10].ItemName = Channel + Device + ".CAM1_Threshold10";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_Threshold10].ClientHandle = ClientHandleValue.Write_CAM1_Threshold10;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI01_Temp].ItemName = Channel + Device + ".CAM1_ROI01_Temp";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI01_Temp].ClientHandle = ClientHandleValue.Write_CAM1_CurrentTemp1;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI02_Temp].ItemName = Channel + Device + ".CAM1_ROI02_Temp";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI02_Temp].ClientHandle = ClientHandleValue.Write_CAM1_CurrentTemp2;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI03_Temp].ItemName = Channel + Device + ".CAM1_ROI03_Temp";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI03_Temp].ClientHandle = ClientHandleValue.Write_CAM1_CurrentTemp3;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI04_Temp].ItemName = Channel + Device + ".CAM1_ROI04_Temp";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI04_Temp].ClientHandle = ClientHandleValue.Write_CAM1_CurrentTemp4;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI05_Temp].ItemName = Channel + Device + ".CAM1_ROI05_Temp";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI05_Temp].ClientHandle = ClientHandleValue.Write_CAM1_CurrentTemp5;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI06_Temp].ItemName = Channel + Device + ".CAM1_ROI06_Temp";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI06_Temp].ClientHandle = ClientHandleValue.Write_CAM1_CurrentTemp6;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI07_Temp].ItemName = Channel + Device + ".CAM1_ROI07_Temp";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI07_Temp].ClientHandle = ClientHandleValue.Write_CAM1_CurrentTemp7;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI08_Temp].ItemName = Channel + Device + ".CAM1_ROI08_Temp";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI08_Temp].ClientHandle = ClientHandleValue.Write_CAM1_CurrentTemp8;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI09_Temp].ItemName = Channel + Device + ".CAM1_ROI09_Temp";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI09_Temp].ClientHandle = ClientHandleValue.Write_CAM1_CurrentTemp9;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI10_Temp].ItemName = Channel + Device + ".CAM1_ROI10_Temp";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI10_Temp].ClientHandle = ClientHandleValue.Write_CAM1_CurrentTemp10;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_MaxTemp].ItemName = Channel + Device + ".CAM2_MaxTemp";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_MaxTemp].ClientHandle = ClientHandleValue.Write_CAM2_MaxTemp;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold01].ItemName = Channel + Device + ".CAM2_Threshold01";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold01].ClientHandle = ClientHandleValue.Write_CAM2_Threshold1;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold02].ItemName = Channel + Device + ".CAM2_Threshold02";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold02].ClientHandle = ClientHandleValue.Write_CAM2_Threshold2;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold03].ItemName = Channel + Device + ".CAM2_Threshold03";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold03].ClientHandle = ClientHandleValue.Write_CAM2_Threshold3;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold04].ItemName = Channel + Device + ".CAM2_Threshold04";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold04].ClientHandle = ClientHandleValue.Write_CAM2_Threshold4;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold05].ItemName = Channel + Device + ".CAM2_Threshold05";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold05].ClientHandle = ClientHandleValue.Write_CAM2_Threshold5;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold06].ItemName = Channel + Device + ".CAM2_Threshold06";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold06].ClientHandle = ClientHandleValue.Write_CAM2_Threshold6;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold07].ItemName = Channel + Device + ".CAM2_Threshold07";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold07].ClientHandle = ClientHandleValue.Write_CAM2_Threshold7;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold08].ItemName = Channel + Device + ".CAM2_Threshold08";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold08].ClientHandle = ClientHandleValue.Write_CAM2_Threshold8;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold09].ItemName = Channel + Device + ".CAM2_Threshold09";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold09].ClientHandle = ClientHandleValue.Write_CAM2_Threshold9;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold10].ItemName = Channel + Device + ".CAM2_Threshold10";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_Threshold10].ClientHandle = ClientHandleValue.Write_CAM2_Threshold10;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI01_Temp].ItemName = Channel + Device + ".CAM2_ROI01_Temp";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI01_Temp].ClientHandle = ClientHandleValue.Write_CAM2_CurrentTemp1;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI02_Temp].ItemName = Channel + Device + ".CAM2_ROI02_Temp";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI02_Temp].ClientHandle = ClientHandleValue.Write_CAM2_CurrentTemp2;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI03_Temp].ItemName = Channel + Device + ".CAM2_ROI03_Temp";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI03_Temp].ClientHandle = ClientHandleValue.Write_CAM2_CurrentTemp3;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI04_Temp].ItemName = Channel + Device + ".CAM2_ROI04_Temp";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI04_Temp].ClientHandle = ClientHandleValue.Write_CAM2_CurrentTemp4;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI05_Temp].ItemName = Channel + Device + ".CAM2_ROI05_Temp";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI05_Temp].ClientHandle = ClientHandleValue.Write_CAM2_CurrentTemp5;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI06_Temp].ItemName = Channel + Device + ".CAM2_ROI06_Temp";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI06_Temp].ClientHandle = ClientHandleValue.Write_CAM2_CurrentTemp6;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI07_Temp].ItemName = Channel + Device + ".CAM2_ROI07_Temp";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI07_Temp].ClientHandle = ClientHandleValue.Write_CAM2_CurrentTemp7;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI08_Temp].ItemName = Channel + Device + ".CAM2_ROI08_Temp";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI08_Temp].ClientHandle = ClientHandleValue.Write_CAM2_CurrentTemp8;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI09_Temp].ItemName = Channel + Device + ".CAM2_ROI09_Temp";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI09_Temp].ClientHandle = ClientHandleValue.Write_CAM2_CurrentTemp9;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI10_Temp].ItemName = Channel + Device + ".CAM2_ROI10_Temp";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI10_Temp].ClientHandle = ClientHandleValue.Write_CAM2_CurrentTemp10;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI01_WAR].ItemName = Channel + Device + ".CAM1_ROI01_WAR";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI01_WAR].ClientHandle = ClientHandleValue.Write_CAM1_ROI01_WAR;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI02_WAR].ItemName = Channel + Device + ".CAM1_ROI02_WAR";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI02_WAR].ClientHandle = ClientHandleValue.Write_CAM1_ROI02_WAR;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI03_WAR].ItemName = Channel + Device + ".CAM1_ROI03_WAR";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI03_WAR].ClientHandle = ClientHandleValue.Write_CAM1_ROI03_WAR;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI04_WAR].ItemName = Channel + Device + ".CAM1_ROI04_WAR";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI04_WAR].ClientHandle = ClientHandleValue.Write_CAM1_ROI04_WAR;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI05_WAR].ItemName = Channel + Device + ".CAM1_ROI05_WAR";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI05_WAR].ClientHandle = ClientHandleValue.Write_CAM1_ROI05_WAR;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI06_WAR].ItemName = Channel + Device + ".CAM1_ROI06_WAR";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI06_WAR].ClientHandle = ClientHandleValue.Write_CAM1_ROI06_WAR;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI07_WAR].ItemName = Channel + Device + ".CAM1_ROI07_WAR";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI07_WAR].ClientHandle = ClientHandleValue.Write_CAM1_ROI07_WAR;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI08_WAR].ItemName = Channel + Device + ".CAM1_ROI08_WAR";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI08_WAR].ClientHandle = ClientHandleValue.Write_CAM1_ROI08_WAR;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI09_WAR].ItemName = Channel + Device + ".CAM1_ROI09_WAR";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI09_WAR].ClientHandle = ClientHandleValue.Write_CAM1_ROI09_WAR;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI10_WAR].ItemName = Channel + Device + ".CAM1_ROI10_WAR";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI10_WAR].ClientHandle = ClientHandleValue.Write_CAM1_ROI10_WAR;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI01_WAR].ItemName = Channel + Device + ".CAM2_ROI01_WAR";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI01_WAR].ClientHandle = ClientHandleValue.Write_CAM2_ROI01_WAR;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI02_WAR].ItemName = Channel + Device + ".CAM2_ROI02_WAR";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI02_WAR].ClientHandle = ClientHandleValue.Write_CAM2_ROI02_WAR;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI03_WAR].ItemName = Channel + Device + ".CAM2_ROI03_WAR";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI03_WAR].ClientHandle = ClientHandleValue.Write_CAM2_ROI03_WAR;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI04_WAR].ItemName = Channel + Device + ".CAM2_ROI04_WAR";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI04_WAR].ClientHandle = ClientHandleValue.Write_CAM2_ROI04_WAR;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI05_WAR].ItemName = Channel + Device + ".CAM2_ROI05_WAR";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI05_WAR].ClientHandle = ClientHandleValue.Write_CAM2_ROI05_WAR;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI06_WAR].ItemName = Channel + Device + ".CAM2_ROI06_WAR";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI06_WAR].ClientHandle = ClientHandleValue.Write_CAM2_ROI06_WAR;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI07_WAR].ItemName = Channel + Device + ".CAM2_ROI07_WAR";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI07_WAR].ClientHandle = ClientHandleValue.Write_CAM2_ROI07_WAR;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI08_WAR].ItemName = Channel + Device + ".CAM2_ROI08_WAR";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI08_WAR].ClientHandle = ClientHandleValue.Write_CAM2_ROI08_WAR;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI09_WAR].ItemName = Channel + Device + ".CAM2_ROI09_WAR";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI09_WAR].ClientHandle = ClientHandleValue.Write_CAM2_ROI09_WAR;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI10_WAR].ItemName = Channel + Device + ".CAM2_ROI10_WAR";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI10_WAR].ClientHandle = ClientHandleValue.Write_CAM2_ROI10_WAR;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI01_ALM].ItemName = Channel + Device + ".CAM1_ROI01_ALM";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI01_ALM].ClientHandle = ClientHandleValue.Write_CAM1_ROI01_ALM;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI02_ALM].ItemName = Channel + Device + ".CAM1_ROI02_ALM";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI02_ALM].ClientHandle = ClientHandleValue.Write_CAM1_ROI02_ALM;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI03_ALM].ItemName = Channel + Device + ".CAM1_ROI03_ALM";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI03_ALM].ClientHandle = ClientHandleValue.Write_CAM1_ROI03_ALM;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI04_ALM].ItemName = Channel + Device + ".CAM1_ROI04_ALM";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI04_ALM].ClientHandle = ClientHandleValue.Write_CAM1_ROI04_ALM;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI05_ALM].ItemName = Channel + Device + ".CAM1_ROI05_ALM";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI05_ALM].ClientHandle = ClientHandleValue.Write_CAM1_ROI05_ALM;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI06_ALM].ItemName = Channel + Device + ".CAM1_ROI06_ALM";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI06_ALM].ClientHandle = ClientHandleValue.Write_CAM1_ROI06_ALM;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI07_ALM].ItemName = Channel + Device + ".CAM1_ROI07_ALM";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI07_ALM].ClientHandle = ClientHandleValue.Write_CAM1_ROI07_ALM;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI08_ALM].ItemName = Channel + Device + ".CAM1_ROI08_ALM";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI08_ALM].ClientHandle = ClientHandleValue.Write_CAM1_ROI08_ALM;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI09_ALM].ItemName = Channel + Device + ".CAM1_ROI09_ALM";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI09_ALM].ClientHandle = ClientHandleValue.Write_CAM1_ROI09_ALM;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI10_ALM].ItemName = Channel + Device + ".CAM1_ROI10_ALM";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM1_ROI10_ALM].ClientHandle = ClientHandleValue.Write_CAM1_ROI10_ALM;

            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI01_ALM].ItemName = Channel + Device + ".CAM2_ROI01_ALM";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI01_ALM].ClientHandle = ClientHandleValue.Write_CAM2_ROI01_ALM;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI02_ALM].ItemName = Channel + Device + ".CAM2_ROI02_ALM";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI02_ALM].ClientHandle = ClientHandleValue.Write_CAM2_ROI02_ALM;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI03_ALM].ItemName = Channel + Device + ".CAM2_ROI03_ALM";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI03_ALM].ClientHandle = ClientHandleValue.Write_CAM2_ROI03_ALM;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI04_ALM].ItemName = Channel + Device + ".CAM2_ROI04_ALM";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI04_ALM].ClientHandle = ClientHandleValue.Write_CAM2_ROI04_ALM;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI05_ALM].ItemName = Channel + Device + ".CAM2_ROI05_ALM";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI05_ALM].ClientHandle = ClientHandleValue.Write_CAM2_ROI05_ALM;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI06_ALM].ItemName = Channel + Device + ".CAM2_ROI06_ALM";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI06_ALM].ClientHandle = ClientHandleValue.Write_CAM2_ROI06_ALM;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI07_ALM].ItemName = Channel + Device + ".CAM2_ROI07_ALM";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI07_ALM].ClientHandle = ClientHandleValue.Write_CAM2_ROI07_ALM;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI08_ALM].ItemName = Channel + Device + ".CAM2_ROI08_ALM";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI08_ALM].ClientHandle = ClientHandleValue.Write_CAM2_ROI08_ALM;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI09_ALM].ItemName = Channel + Device + ".CAM2_ROI09_ALM";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI09_ALM].ClientHandle = ClientHandleValue.Write_CAM2_ROI09_ALM;
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI10_ALM].ItemName = Channel + Device + ".CAM2_ROI10_ALM";
            Write_itemIdentifiers[(int)WritingArrayNo.CAM2_ROI10_ALM].ClientHandle = ClientHandleValue.Write_CAM2_ROI10_ALM;


            for (int i = 0; i < WriteTagCount; i++)
            {
                Write_itemValues[i] = new ItemValue();
            }

            Write_itemValues[(int)WritingArrayNo.CurrentAngle].Value = CurrentAngle;
            Write_itemValues[(int)WritingArrayNo.CAM1_MaxTemp].Value = main.FloatMaxTemp;
            Write_itemValues[(int)WritingArrayNo.CAM1_Threshold01].Value = result.CAM1_ThresholdTemp[0];
            Write_itemValues[(int)WritingArrayNo.CAM1_Threshold02].Value = result.CAM1_ThresholdTemp[1];
            Write_itemValues[(int)WritingArrayNo.CAM1_Threshold03].Value = result.CAM1_ThresholdTemp[2];
            Write_itemValues[(int)WritingArrayNo.CAM1_Threshold04].Value = result.CAM1_ThresholdTemp[3];
            Write_itemValues[(int)WritingArrayNo.CAM1_Threshold05].Value = result.CAM1_ThresholdTemp[4];
            Write_itemValues[(int)WritingArrayNo.CAM1_Threshold06].Value = result.CAM1_ThresholdTemp[5];
            Write_itemValues[(int)WritingArrayNo.CAM1_Threshold07].Value = result.CAM1_ThresholdTemp[6];
            Write_itemValues[(int)WritingArrayNo.CAM1_Threshold08].Value = result.CAM1_ThresholdTemp[7];
            Write_itemValues[(int)WritingArrayNo.CAM1_Threshold09].Value = result.CAM1_ThresholdTemp[8];
            Write_itemValues[(int)WritingArrayNo.CAM1_Threshold10].Value = result.CAM1_ThresholdTemp[9];
            Write_itemValues[(int)WritingArrayNo.CAM1_ROI01_Temp].Value = imgView.CAM1_TemperatureArr[0];
            Write_itemValues[(int)WritingArrayNo.CAM1_ROI02_Temp].Value = imgView.CAM1_TemperatureArr[1];
            Write_itemValues[(int)WritingArrayNo.CAM1_ROI03_Temp].Value = imgView.CAM1_TemperatureArr[2];
            Write_itemValues[(int)WritingArrayNo.CAM1_ROI04_Temp].Value = imgView.CAM1_TemperatureArr[3];
            Write_itemValues[(int)WritingArrayNo.CAM1_ROI05_Temp].Value = imgView.CAM1_TemperatureArr[4];
            Write_itemValues[(int)WritingArrayNo.CAM1_ROI06_Temp].Value = imgView.CAM1_TemperatureArr[5];
            Write_itemValues[(int)WritingArrayNo.CAM1_ROI07_Temp].Value = imgView.CAM1_TemperatureArr[6];
            Write_itemValues[(int)WritingArrayNo.CAM1_ROI08_Temp].Value = imgView.CAM1_TemperatureArr[7];
            Write_itemValues[(int)WritingArrayNo.CAM1_ROI09_Temp].Value = imgView.CAM1_TemperatureArr[8];
            Write_itemValues[(int)WritingArrayNo.CAM1_ROI10_Temp].Value = imgView.CAM1_TemperatureArr[9];

            Write_itemValues[(int)WritingArrayNo.CAM2_MaxTemp].Value = main.c2_FloatMaxTemp;
            Write_itemValues[(int)WritingArrayNo.CAM2_Threshold01].Value = result.CAM2_ThresholdTemp[0];
            Write_itemValues[(int)WritingArrayNo.CAM2_Threshold02].Value = result.CAM2_ThresholdTemp[1];
            Write_itemValues[(int)WritingArrayNo.CAM2_Threshold03].Value = result.CAM2_ThresholdTemp[2];
            Write_itemValues[(int)WritingArrayNo.CAM2_Threshold04].Value = result.CAM2_ThresholdTemp[3];
            Write_itemValues[(int)WritingArrayNo.CAM2_Threshold05].Value = result.CAM2_ThresholdTemp[4];
            Write_itemValues[(int)WritingArrayNo.CAM2_Threshold06].Value = result.CAM2_ThresholdTemp[5];
            Write_itemValues[(int)WritingArrayNo.CAM2_Threshold07].Value = result.CAM2_ThresholdTemp[6];
            Write_itemValues[(int)WritingArrayNo.CAM2_Threshold08].Value = result.CAM2_ThresholdTemp[7];
            Write_itemValues[(int)WritingArrayNo.CAM2_Threshold09].Value = result.CAM2_ThresholdTemp[8];
            Write_itemValues[(int)WritingArrayNo.CAM2_Threshold10].Value = result.CAM2_ThresholdTemp[9];
            Write_itemValues[(int)WritingArrayNo.CAM2_ROI01_Temp].Value = imgView.CAM2_TemperatureArr[0];
            Write_itemValues[(int)WritingArrayNo.CAM2_ROI02_Temp].Value = imgView.CAM2_TemperatureArr[1];
            Write_itemValues[(int)WritingArrayNo.CAM2_ROI03_Temp].Value = imgView.CAM2_TemperatureArr[2];
            Write_itemValues[(int)WritingArrayNo.CAM2_ROI04_Temp].Value = imgView.CAM2_TemperatureArr[3];
            Write_itemValues[(int)WritingArrayNo.CAM2_ROI05_Temp].Value = imgView.CAM2_TemperatureArr[4];
            Write_itemValues[(int)WritingArrayNo.CAM2_ROI06_Temp].Value = imgView.CAM2_TemperatureArr[5];
            Write_itemValues[(int)WritingArrayNo.CAM2_ROI07_Temp].Value = imgView.CAM2_TemperatureArr[6];
            Write_itemValues[(int)WritingArrayNo.CAM2_ROI08_Temp].Value = imgView.CAM2_TemperatureArr[7];
            Write_itemValues[(int)WritingArrayNo.CAM2_ROI09_Temp].Value = imgView.CAM2_TemperatureArr[8];
            Write_itemValues[(int)WritingArrayNo.CAM2_ROI10_Temp].Value = imgView.CAM2_TemperatureArr[9];

            Write_itemValues[(int)WritingArrayNo.CAM1_ROI01_WAR].Value = result.CAM1_isTempPM10[0];
            Write_itemValues[(int)WritingArrayNo.CAM1_ROI02_WAR].Value = result.CAM1_isTempPM10[1];
            Write_itemValues[(int)WritingArrayNo.CAM1_ROI03_WAR].Value = result.CAM1_isTempPM10[2];
            Write_itemValues[(int)WritingArrayNo.CAM1_ROI04_WAR].Value = result.CAM1_isTempPM10[3];
            Write_itemValues[(int)WritingArrayNo.CAM1_ROI05_WAR].Value = result.CAM1_isTempPM10[4];
            Write_itemValues[(int)WritingArrayNo.CAM1_ROI06_WAR].Value = result.CAM1_isTempPM10[5];
            Write_itemValues[(int)WritingArrayNo.CAM1_ROI07_WAR].Value = result.CAM1_isTempPM10[6];
            Write_itemValues[(int)WritingArrayNo.CAM1_ROI08_WAR].Value = result.CAM1_isTempPM10[7];
            Write_itemValues[(int)WritingArrayNo.CAM1_ROI09_WAR].Value = result.CAM1_isTempPM10[8];
            Write_itemValues[(int)WritingArrayNo.CAM1_ROI10_WAR].Value = result.CAM1_isTempPM10[9];

            Write_itemValues[(int)WritingArrayNo.CAM2_ROI01_WAR].Value = result.CAM2_isTempPM10[0];
            Write_itemValues[(int)WritingArrayNo.CAM2_ROI02_WAR].Value = result.CAM2_isTempPM10[1];
            Write_itemValues[(int)WritingArrayNo.CAM2_ROI03_WAR].Value = result.CAM2_isTempPM10[2];
            Write_itemValues[(int)WritingArrayNo.CAM2_ROI04_WAR].Value = result.CAM2_isTempPM10[3];
            Write_itemValues[(int)WritingArrayNo.CAM2_ROI05_WAR].Value = result.CAM2_isTempPM10[4];
            Write_itemValues[(int)WritingArrayNo.CAM2_ROI06_WAR].Value = result.CAM2_isTempPM10[5];
            Write_itemValues[(int)WritingArrayNo.CAM2_ROI07_WAR].Value = result.CAM2_isTempPM10[6];
            Write_itemValues[(int)WritingArrayNo.CAM2_ROI08_WAR].Value = result.CAM2_isTempPM10[7];
            Write_itemValues[(int)WritingArrayNo.CAM2_ROI09_WAR].Value = result.CAM2_isTempPM10[8];
            Write_itemValues[(int)WritingArrayNo.CAM2_ROI10_WAR].Value = result.CAM2_isTempPM10[9];

            Write_itemValues[(int)WritingArrayNo.CAM1_ROI01_ALM].Value = result.CAM1_isTempUpper10[0];
            Write_itemValues[(int)WritingArrayNo.CAM1_ROI02_ALM].Value = result.CAM1_isTempUpper10[1];
            Write_itemValues[(int)WritingArrayNo.CAM1_ROI03_ALM].Value = result.CAM1_isTempUpper10[2];
            Write_itemValues[(int)WritingArrayNo.CAM1_ROI04_ALM].Value = result.CAM1_isTempUpper10[3];
            Write_itemValues[(int)WritingArrayNo.CAM1_ROI05_ALM].Value = result.CAM1_isTempUpper10[4];
            Write_itemValues[(int)WritingArrayNo.CAM1_ROI06_ALM].Value = result.CAM1_isTempUpper10[5];
            Write_itemValues[(int)WritingArrayNo.CAM1_ROI07_ALM].Value = result.CAM1_isTempUpper10[6];
            Write_itemValues[(int)WritingArrayNo.CAM1_ROI08_ALM].Value = result.CAM1_isTempUpper10[7];
            Write_itemValues[(int)WritingArrayNo.CAM1_ROI09_ALM].Value = result.CAM1_isTempUpper10[8];
            Write_itemValues[(int)WritingArrayNo.CAM1_ROI10_ALM].Value = result.CAM1_isTempUpper10[9];

            Write_itemValues[(int)WritingArrayNo.CAM2_ROI01_ALM].Value = result.CAM2_isTempUpper10[0];
            Write_itemValues[(int)WritingArrayNo.CAM2_ROI02_ALM].Value = result.CAM2_isTempUpper10[1];
            Write_itemValues[(int)WritingArrayNo.CAM2_ROI03_ALM].Value = result.CAM2_isTempUpper10[2];
            Write_itemValues[(int)WritingArrayNo.CAM2_ROI04_ALM].Value = result.CAM2_isTempUpper10[3];
            Write_itemValues[(int)WritingArrayNo.CAM2_ROI05_ALM].Value = result.CAM2_isTempUpper10[4];
            Write_itemValues[(int)WritingArrayNo.CAM2_ROI06_ALM].Value = result.CAM2_isTempUpper10[5];
            Write_itemValues[(int)WritingArrayNo.CAM2_ROI07_ALM].Value = result.CAM2_isTempUpper10[6];
            Write_itemValues[(int)WritingArrayNo.CAM2_ROI08_ALM].Value = result.CAM2_isTempUpper10[7];
            Write_itemValues[(int)WritingArrayNo.CAM2_ROI09_ALM].Value = result.CAM2_isTempUpper10[8];
            Write_itemValues[(int)WritingArrayNo.CAM2_ROI10_ALM].Value = result.CAM2_isTempUpper10[9];
        }

        public void OPC_Write()
        {
            ReturnCode returnCode;

            WriteDataDefinition();

            int TransID = RandomNumber(65535, 1);

            // Call the Write API method:
            returnCode = DAServer.WriteAsync(TransID, ref Write_itemIdentifiers, Write_itemValues);

            // Handle result:
            if (returnCode != ReturnCode.SUCCEEDED)
            {
                //System.Windows.Forms.MessageBox.Show("Async Write failed with a result of " + returnCode.ToString());
            }
        }

        private void ReadDataDefintion()
        {
            for (int i = 0; i < ReadTagCount; i++)
            {
                Read_itemIdentifiers[i] = new ItemIdentifier();
            }
            Read_itemIdentifiers[(int)ReadingArrayNo.CurrentSteelNo].ItemName = Channel + Device + StringEnum.GetStringValue(ReadItemIDs.CurrentSteelNo);
            Read_itemIdentifiers[(int)ReadingArrayNo.CurrentSteelNo].ClientHandle = ClientHandleValue.Read_CurrentSteelNo;

            Read_itemIdentifiers[(int)ReadingArrayNo.CurrentAngle].ItemName = Channel + Device + StringEnum.GetStringValue(ReadItemIDs.CurrentAngle);
            Read_itemIdentifiers[(int)ReadingArrayNo.CurrentAngle].ClientHandle = ClientHandleValue.Read_CurrentAngle;

            Read_itemIdentifiers[(int)ReadingArrayNo.CurrentSteelKind].ItemName = Channel + Device + StringEnum.GetStringValue(ReadItemIDs.CurrentSteelKind);
            Read_itemIdentifiers[(int)ReadingArrayNo.CurrentSteelKind].ClientHandle = ClientHandleValue.Read_CurrentSteelKind;

            Read_itemIdentifiers[(int)ReadingArrayNo.RunningProcess].ItemName = Channel + Device + StringEnum.GetStringValue(ReadItemIDs.RunningProcess);
            Read_itemIdentifiers[(int)ReadingArrayNo.RunningProcess].ClientHandle = ClientHandleValue.Read_RunningProcess;

            Read_itemIdentifiers[(int)ReadingArrayNo.O2Lance_Blowing].ItemName = Channel + Device + StringEnum.GetStringValue(ReadItemIDs.O2LanceBlowing);
            Read_itemIdentifiers[(int)ReadingArrayNo.O2Lance_Blowing].ClientHandle = ClientHandleValue.Read_O2Lance_Blowing;
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
                //System.Windows.Forms.MessageBox.Show("ReadAsync failed with error: " + returnCode.ToString());
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
            string temp = "";

            // DWORD = System.Uint32(uint), WORD = System.UInt16(ushort), 문자형 = System.String, BooleanArray = System.Boolean[]
            ReadingResult[0] = itemValues[(int)ReadingArrayNo.CurrentAngle].Value;
            ReadingResult[1] = itemValues[(int)ReadingArrayNo.CurrentSteelKind].Value;
            ReadingResult[2] = itemValues[(int)ReadingArrayNo.CurrentSteelNo].Value;
            ReadingResult[3] = itemValues[(int)ReadingArrayNo.RunningProcess].Value;
            ReadingResult[4] = itemValues[(int)ReadingArrayNo.O2Lance_Blowing].Value;

            //string b1 = ReadingResult[0].GetType().ToString();
            //string b2 = ReadingResult[1].GetType().ToString();
            //string b3 = ReadingResult[2].GetType().ToString();
            //string b4 = ReadingResult[3].GetType().ToString();

            // DataType을 비교해서
            for (int i = 0; i < itemValues.Length; i++)
            {
                /*if (ReadingResult[i].GetType().ToString() == "System.Boolean[]") { ChargingStatus = (bool[])ReadingResult[i]; }
                else*/
                if (ReadingResult[i].GetType().ToString() == "System.String") { result.textBox_CurrentSteelKind.Text = ReadingResult[i].ToString(); }
                else if (ReadingResult[i].GetType().ToString() == "System.UInt16")
                {
                    CurrentAngle = Convert.ToUInt16(ReadingResult[i]);
                    main.textBox_SlopeAngle.Text = CurrentAngle.ToString() + "º";
                }
                else if (ReadingResult[i].GetType().ToString() == "System.UInt32")
                {
                    main.textBox1.Text = ReadingResult[i].ToString();
                    main.textBox2.Text = ReadingResult[i].ToString();
                }
                else if (ReadingResult[i].GetType().ToString() == "System.Byte")
                {
                    int inttemp = Convert.ToInt32(ReadingResult[i]);        // Byte를 받아서
                    temp = Convert.ToString(inttemp, 2);        // 2진수 변환
                }
                else if (ReadingResult[i].GetType().ToString() == "System.Boolean") O2LanceResult = Convert.ToBoolean(ReadingResult[i]);
            }

            // byte 후처리
            string aa = "";
            int substringTemp = 0;
            for (int i = 0; i < 8; i++)
            {
                ChargingStatus[i] = false;
            }
            for (int i = ChargingStatus.Length - temp.Length; i < ChargingStatus.Length; i++)       // 변환한 2진수 집어넣기
            {
                aa = temp.Substring(substringTemp, 1);
                if (aa == "1") ChargingStatus[i] = true;
                else ChargingStatus[i] = false;
                substringTemp++;
            }

            for (int i = 0; i < 8; i++)
            {
                if (ChargingStatus[i] == true) main.ProgressLabel[i].ForeColor = result.Connected_NoWarning;
                else main.ProgressLabel[i].ForeColor = result.NotConnected;
            }
            if (O2LanceResult == true) main.ProgressLabel[8].ForeColor = result.Connected_NoWarning;
            else main.ProgressLabel[8].ForeColor = result.NotConnected;
        }

        private int RandomNumber(int MaxNumber, int MinNumber)
        {
            Random r = new Random(System.DateTime.Now.Millisecond);

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
