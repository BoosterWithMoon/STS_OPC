using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxTeeChart;
using System.Threading;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using System.Configuration;
using System.Globalization;
using System.Runtime.CompilerServices;
using Kepware.ClientAce.OpcDaClient;
using Kepware.ClientAce.OpcCmn;

namespace Electric_Furnance_Monitoring_OPC_Included_
{
    public partial class MainForm : Form
    {

        //DaServerMgt daServer = new DaServerMgt();
        //public MainForm()
        //{
        //    InitializeComponent();
        //}

        NewDeviceForm newDevice;
        public uint DetectedDevices;
        public IntPtr[] pIRDX_Array;

        public ushort[] sizeX_Array;
        public ushort[] sizeY_Array;

        public float currentEmissivity;
        public float currentTransmittance;
        public float currentAmbientTemp;
        public float cMaxTemp = 0;
        public float Scale_MaxTemp = 0;
        public float cMinTemp = 0;
        public float Scale_MinTemp = 0;
        public uint NumOfDataRecord=0;
        public uint CurDataRecord = 0;
        public float m_fps=0;
        public ushort m_avg=0;

        private bool isClosing = false;

        SystemPropertyGrid customGrid;

        ImageView imgView;

        CAM1_ImageView c1_imgView;
        CAM2_ImageView c2_imgView;

        Thread mThread;
        Thread mThread_two;
        public uint acq_index = 0;

        

        CAM1_ChartView c1_chartView;
        CAM2_ChartView c2_chartView;

        CAM1_DataGridView c1_gridView;
        CAM2_DataGridView c2_gridView;

        ResultView result;

        CustomOPC opc;

        public bool Activate_DrawPOI = false;

        string NewIRDXFileName = "";
        string newRawDataFileName = "";
        string newResultDataFileName = "";
        string CAM2_NewIRDXFileName = "";
        string CAM2_newRawDataFileName = "";
        string CAM2_newResultDataFileName = "";
        public bool isLoggingRunning = false;

        public enum OpenMode :int
        {
            Online=1,
            Simulation=2,
            IRDX=3
        }
        public OpenMode currentOpenMode = OpenMode.IRDX;

        public bool OPCActivated = false;

        // OPC AREA ===================================================================
        //DaServerMgt DAServer = new DaServerMgt();
        //Kepware.ClientAce.OpcDaClient.ConnectInfo connectInfo = new Kepware.ClientAce.OpcDaClient.ConnectInfo();
        //ServerIdentifier[] availableOPCServers;
        //bool connectFailed;
        // =================================================================== OPC AREA

        public MainForm()
        {
            
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            newDevice = new NewDeviceForm(this);
            DetectedDevices = 0;
            pIRDX_Array = new IntPtr[2];
            sizeX_Array = new ushort[2];
            sizeY_Array = new ushort[2];
            currentEmissivity = 1.0f;
            currentTransmittance = 1.0f;

            customGrid = new SystemPropertyGrid(this);

            imgView = new ImageView(this);
            c1_imgView = new CAM1_ImageView(this);
            c2_imgView = new CAM2_ImageView(this);

            mThread = new Thread(new ThreadStart(run));
            mThread_two = new Thread(new ThreadStart(run_two));

            c1_chartView = new CAM1_ChartView(this);
            c2_chartView = new CAM2_ChartView(this);

            c1_gridView = new CAM1_DataGridView(this);
            c2_gridView = new CAM2_DataGridView(this);

            result = new ResultView(this);

            opc = new CustomOPC(this);
        }

        #region Publicize_AllocatedClass

        public object ImageView_forPublicRef() { return imgView; }

        public object customGrid_forPublicRef() { return customGrid; }

        public object CAM1_ImageView_forPublicRef() { return c1_imgView; }

        public object CAM2_ImageView_forPublicRef() { return c2_imgView; }

        public object CAM1_ChartView_forPublicRef() { return c1_chartView; }

        public object CAM2_ChartView_forPublicRef() { return c2_chartView; }

        public object CAM1_GridView_forPublicRef() { return c1_gridView; }

        public object CAM2_GridView_forPublicRef() { return c2_gridView; }

        public object ResultView_forPublicRef() { return result; }

        public object Thread1_forPublicRef() { return mThread; }

        public object Thread2_forPublicRef() { return mThread_two; }

        public object CustomOPC_forPublicRef() { return opc; }

        #endregion

        public void InitPropertyGrid()
        {
            propertyGrid1.PropertySort = PropertySort.Categorized;
            propertyGrid1.ToolbarVisible = false;
            propertyGrid1.SelectedObject = customGrid;
        }

        public void InitImageView()
        {
            //CAM1_ImageView c1_img = new CAM1_ImageView(this);
            c1_imgView.TopLevel = false;
            split_CAM1Info.Panel2.Controls.Add(c1_imgView);
            c1_imgView.Parent = split_CAM1Info.Panel2;
            c1_imgView.Dock = DockStyle.Fill;
            c1_imgView.Text = "";
            c1_imgView.ControlBox = false;
            c1_imgView.Show();

            c2_imgView.TopLevel = false;
            split_CAM2Info.Panel2.Controls.Add(c2_imgView);
            c2_imgView.Parent = split_CAM2Info.Panel2;
            c2_imgView.Dock = DockStyle.Fill;
            c2_imgView.Text = "";
            c2_imgView.ControlBox = false;
            c2_imgView.Show();
        }

        public void InitGridView()
        {
            c1_gridView.TopLevel = false;
            split_CAM1ChartGrid.Panel2.Controls.Add(c1_gridView);
            c1_gridView.Parent = this.split_CAM1ChartGrid.Panel2;
            c1_gridView.Dock = System.Windows.Forms.DockStyle.Fill;
            c1_gridView.Text = "";
            c1_gridView.ControlBox = false;
            c1_gridView.dataGridView1.RowHeadersVisible = false;
            c1_gridView.Show();

            c2_gridView.TopLevel = false;
            split_CAM2ChartGrid.Panel2.Controls.Add(c2_gridView);
            c2_gridView.Parent = split_CAM2ChartGrid.Panel2;
            c2_gridView.Dock = System.Windows.Forms.DockStyle.Fill;
            c2_gridView.Text = "";
            c2_gridView.ControlBox = false;
            c2_gridView.dataGridView1.RowHeadersVisible = false;
            c2_gridView.Show();
        }

        public void InitChart()
        {
            //chartView.DrawTest();
            c1_chartView.TopLevel = false;
            split_CAM1ChartGrid.Panel1.Controls.Add(c1_chartView);
            c1_chartView.Parent = split_CAM1ChartGrid.Panel1;
            c1_chartView.Dock = DockStyle.Fill;
            c1_chartView.Text = "";
            c1_chartView.ControlBox = false;
            c1_chartView.Show();

            c2_chartView.TopLevel = false;
            split_CAM2ChartGrid.Panel1.Controls.Add(c2_chartView);
            c2_chartView.Parent = split_CAM2ChartGrid.Panel1;
            c2_chartView.Dock = DockStyle.Fill;
            c2_chartView.Text = "";
            c2_chartView.ControlBox = false;
            c2_chartView.Show();
        }

        public void InitResultView()
        {
            result.TopLevel = false;
            split_ViewToInfo.Panel2.Controls.Add(result);
            result.Parent = split_ViewToInfo.Panel2;
            result.Dock = DockStyle.Fill;
            result.Text = "";
            result.ControlBox = false;
            result.Show();
        }

        private void OpenNewDevice()
        {
            newDevice.DeviceDetection();
            if (newDevice.isDetected == true)
            {
                if (newDevice.isDetected == false) return;
                else
                {
                    Cursor.Current = Cursors.WaitCursor;
                    newDevice.ReadyToRun();
                    Cursor.Current = Cursors.Default;

                    newDevice.ShowDialog();
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


        private void DisconnectOPCServer()
        {
            // Call Disconnect API method:
            //try
            //{
            //    if (DAServer.IsConnected)
            //    {
            //        DAServer.Disconnect();
            //    }
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show("Handled Disconnect exception. Reason: " + e.Message);
            //}
        }

        private void AsyncRead_Click(object sender, EventArgs e)
        {
            


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
            returnValue = BeginInvoke(RCevHndlr, RCevHndlrArray);
        }

        public void ReadCompleted(int transactionHandle, bool allQualitiesGood, bool noErrors, ItemValueCallback[] itemValues)
        {
            int itemIndex = (int)itemValues[0].ClientHandle;

            if (itemValues[0].ResultID.Succeeded)
            {
                if (itemValues[0].Value == null)
                {
                    OPC_textBox1.Text = "Unknown";
                }
                else
                {
                    OPC_textBox1.Text = itemValues[0].Value.ToString();
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadConfiguration();

            InitPropertyGrid();

            OPC_textBox1.Visible = false;
            OPC_textBox2.Visible = false;
            button1.Visible = false;
            button2.Visible = false;

            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;

            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;

            //label_CurrentSteelKind.Visible = false;
            //textBox_CurrentSteelKind.Visible = false;

            PreviousRecord_toolStripButton.Enabled = false;     // Previous Record
            previousRecordToolStripMenuItem.Enabled = false;
            NextRecord_toolStripButton.Enabled = false;     // Next Record
            nextRecordToolStripMenuItem.Enabled = false;
            KeepMoving_toolStripButton.Enabled = false;     // Play Record
            playToolStripMenuItem.Enabled = false;
            Pause_toolStripButton.Enabled = false;          // Pause Record
            stopToolStripMenuItem1.Enabled = false;

            DrawPOI_toolStripButton.Enabled = false;   // Draw POI
            drawROIToolStripMenuItem.Enabled = false;
            DeletePOI_toolStripButton.Enabled = false;   // Delete POI
            deleteROIToolStripMenuItem.Enabled = false;
            MovePOI_toolStripButton.Enabled = false;   // Move POI
            moveROIToolStripMenuItem.Enabled = false;

            LogStart_toolStripButton.Enabled = false;   // Log Start
            startToolStripMenuItem.Enabled = false;
            LogStop_toolStripButton.Enabled = false;   // Log Stop
            stopToolStripMenuItem.Enabled = false;

            ViewAdjust();

            opc.ServerDetection();
            opc.ServerConnection();
            // OPC AREA ===================================================================================
            //OpcServerEnum serverEnum = new OpcServerEnum();
            //string nodeName = "localhost";
            //bool returnAllServers = false;
            //bool detected = false;
            //int detectedIndex = 0;
            //ServerCategory[] serverCategories = { ServerCategory.OPCDA };

            //// Kepware 서버탐지
            //try
            //{
            //    serverEnum.EnumComServer(nodeName, returnAllServers, serverCategories, out availableOPCServers);
            //    if (availableOPCServers.GetLength(0) > 0)
            //    {
            //        for (int i = 0; i < availableOPCServers.GetLength(0); i++)
            //        {
            //            // detect된 서버 list 중에서 kepware가 있는지 탐색
            //            if (availableOPCServers[i].ProgID == "Kepware.KEPServerEX.V6")
            //            {
            //                detected = true;
            //                detectedIndex = i;
            //                break;
            //            }
            //        }
            //    }
            //    else // kepware가 없으면 메세지박스
            //    {
            //        MessageBox.Show("No OPC servers found at " + nodeName);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    //throw;
            //}

            //// 서버 탐지에 성공하면 바로 연결
            //string url = availableOPCServers[detectedIndex].Url;
            //int clientHandle = 1;
            //ConnectInfo connectInfo = new ConnectInfo();
            //connectInfo.LocalId = "en";
            //connectInfo.KeepAliveTime = 1000; // ms
            //connectInfo.RetryAfterConnectionError = true;
            //connectInfo.RetryInitialConnection = false;

            //try
            //{
            //    DAServer.Connect(url, clientHandle, ref connectInfo, out connectFailed);
            //    MessageBox.Show("Connect succeed");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Handled Connect exception. Reason: " + ex.Message);

            //    // Make sure following code knows connection failed:
            //    connectFailed = true;
            //}

            //// Handle result:
            //if (connectFailed)
            //{
            //    // Tell user connection attempt failed:
            //    MessageBox.Show("Connect failed");
            //}

            //DAServer.ReadCompleted += new DaServerMgt.ReadCompletedEventHandler(daServerMgt_ReadCompleted);
        
    }

        private void button2_Click(object sender, EventArgs e)
        {
            opc.OPC_Write();
            //if (OPC_textBox2.Text == "")
            //{
            //    MessageBox.Show("write할 데이터가 없음");
            //    return;
            //}
            //ReturnCode returnCode;

            //ItemIdentifier[] itemIdentifiers = new ItemIdentifier[1];
            //itemIdentifiers[0] = new ItemIdentifier();
            //itemIdentifiers[0].ItemName = "Channel2.TestDevice.No2";
            //itemIdentifiers[0].ClientHandle = 0;

            //// The itemValues array contains the values we wish to write to the items.
            //ItemValue[] itemValues = new ItemValue[1];
            //itemValues[0] = new ItemValue();
            //itemValues[0].Value = System.Convert.ToInt32(OPC_textBox2.Text);

            //int TransID = RandomNumber(65535, 1);

            //// Call the Write API method:
            //returnCode = DAServer.WriteAsync(TransID, ref itemIdentifiers, itemValues);

            //// Handle result:
            //if (returnCode != ReturnCode.SUCCEEDED)
            //{
            //    MessageBox.Show("Async Write failed with a result of " + returnCode.ToString()/*System.Convert.ToString(itemIdentifiers[0].ResultID.Code) + "\r\n" + "Description: " + itemIdentifiers[0].ResultID.Description*/);
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            opc.OPC_Read();
            //ReturnCode returnCode;

            //int maxAge = 0;
            //int TransID = RandomNumber(65535, 1);

            //ItemIdentifier[] itemIdentifiers = new ItemIdentifier[1];
            //itemIdentifiers[0] = new ItemIdentifier();
            //itemIdentifiers[0].ItemName = "Channel2.TestDevice.No2";
            //itemIdentifiers[0].ClientHandle = 0;

            //// Call the Read API method:
            //returnCode = DAServer.ReadAsync(TransID, maxAge, ref itemIdentifiers);

            //// Handle result:
            //// Check result
            //if (returnCode != ReturnCode.SUCCEEDED)
            //{
            //    MessageBox.Show("ReadAsync failed with error: " + System.Convert.ToString(itemIdentifiers[0].ResultID.Code, 16) + "\r\n" + "Description: " + itemIdentifiers[0].ResultID.Description);
            //}
        }

        // =================================================================================== OPC AREA


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfiguration();

            //DisconnectOPCServer();
            if (OPCActivated)
            {
                OPCTimer.Stop();
                OPCActivated = false;
            }

            opc.ServerDisconnection();

            isClosing = true;

            Thread.Sleep(3);

            // 스레드 둘중에 하나라도 돌고있으면 먼저 둘다 죽이고 시작하자
            if (mThread.IsAlive || mThread_two.IsAlive)
            {
                mThread.Abort();
                mThread_two.Abort();
            }

            DIASDAQ.DDAQ_DEVICE_DO_STOP(1);
            DIASDAQ.DDAQ_DEVICE_DO_STOP(2);

            if (DetectedDevices != 0)
            {
                DIASDAQ.DDAQ_DEVICE_DO_CLOSE(1);
                DIASDAQ.DDAQ_DEVICE_DO_CLOSE(2);
            }

            System.Threading.Thread.Sleep(100);

            c1_chartView.axTChart1.Dispose();
            c2_chartView.axTChart1.Dispose();
        }

        private void newOnlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenNewDevice();
        }


        #region Thread

        //[MethodImpl(MethodImplOptions.Synchronized)]
        private void run()  // Current: 320L
        {
            bool newDataReady = false;

            while (true)
            //while(thr1_waitHandle.WaitOne())
            {
                Thread.Sleep(10);

                if (DIASDAQ.DDAQ_DEVICE_GET_NEWDATAREADY(DetectedDevices, ref newDataReady) != DIASDAQ.DDAQ_ERROR.NO_ERROR)/// 카메라가 새로운 데이터를 받을 준비가 되었을 시
                {
                    DIASDAQ.DDAQ_DEVICE_DO_STOP(DetectedDevices);
                    return;
                }

                if (newDataReady && !isClosing)
                {
                    if (DIASDAQ.DDAQ_DEVICE_DO_UPDATEDATA(DetectedDevices) != DIASDAQ.DDAQ_ERROR.NO_ERROR)                 /// 새로운 데이터가 있으며 종료의 명령이 없을 시
                        return;                                                                                                /// UpDate Data !!

                    imgView.CalculateCurrentTemp(pIRDX_Array[0], imgView.CAM1_POICount, imgView.CAM1_ClickedPosition, imgView.CAM1_TemperatureArr);
                    imgView.DrawImage(pIRDX_Array[0], c1_imgView.pictureBox1);
                    //if (img != null) img.Dispose();                                                                            /// 메모리 관리를 위하여 Dispose.
                    c1_chartView.UpdateData();
                    ////c1_gridView.ShowTemperatureOnGrid();
                    c1_gridView.RefreshGrid();
                    result.CAM1_DetectTempThreshold();
                    ////customGrid.SafeRefresh_PropertyGrid();
                    //customGrid.SafeRefresh_Time();
                    //if (!CAM1_UpdateThr.IsAlive)
                    //{
                    //    CAM1_UpdateThr.Start();
                    //}
                    //else if (CAM1_UpdateThr.IsAlive)
                    //{
                    //    Resume();
                    //}

                    //c1_chartView.UpdateData();
                    //c1_gridView.RefreshGrid();
                    ////result.DetectTempThreshold();
                    //result.CAM1_DetectTempThreshold();

                    CompareMaxTemperature(imgView.CAM1_TemperatureArr);

                    acq_index++;

                    if (DIASDAQ.DDAQ_DEVICE_DO_ENABLE_NEXTMSG(DetectedDevices) != DIASDAQ.DDAQ_ERROR.NO_ERROR)             /// 카메라가 새로운 데이터를 받을 수 있도록 Do Enable
                        return;
                }
            }


        }
        private void run_two()  // Current: 512N
        {
            bool newDataReady = false;

            while (true)
            {
                Thread.Sleep(10);

                if (DIASDAQ.DDAQ_DEVICE_GET_NEWDATAREADY(1, ref newDataReady) != DIASDAQ.DDAQ_ERROR.NO_ERROR)       /// 카메라가 새로운 데이터를 받을 준비가 되었을 시
                {
                    DIASDAQ.DDAQ_DEVICE_DO_STOP(1);
                    return;
                }
                if (newDataReady && !isClosing)
                {
                    if (DIASDAQ.DDAQ_DEVICE_DO_UPDATEDATA(1) != DIASDAQ.DDAQ_ERROR.NO_ERROR)                        /// 새로운 데이터가 있으며 종료의 명령이 없을 시
                        return;                                                                                     /// UpDate Data !!

                    //Bitmap bmp = GET_BITMAP(pIRDX_Array[1]);// GET_BITMAP_TOW(hIRDX_two);                                                         /// 카메라의 핸들값을 넘겨 화면에 뿌려줌
                    //bmp = new Bitmap((Image)bmp, new Size(c2_img.pictureBox1.Width, c2_img.pictureBox1.Height));
                    //Image img = c2_img.pictureBox1.Image;
                    //c2_img.pictureBox1.Image = (Image)bmp;

                    //if (img != null) img.Dispose();                                                                 /// 메모리 관리를 위하여 Dispose.

                    imgView.CalculateCurrentTemp(pIRDX_Array[1], imgView.CAM2_POICount, imgView.CAM2_ClickedPosition, imgView.CAM2_TemperatureArr);
                    //imageView.CAM2_DrawImage(pIRDX_Array[1], c2_img.pictureBox1, imageView.CAM2_ClickedPosition, imageView.CAM2_POICount);
                    //imageView.Test_DrawImage(pIRDX_Array[1], c2_img.pictureBox1, imageView.CAM2_ClickedPosition, imageView.CAM2_POICount);
                    imgView.CAM2_DrawImage(pIRDX_Array[1], c2_imgView.pictureBox1, imgView.CAM2_ClickedPosition, imgView.CAM2_POICount);

                    c2_chartView.UpdateData();
                    c2_gridView.CAM2_RefreshGrid();
                    ////result.DetectTempThreshold();
                    result.CAM2_DetectTempThreshold();

                    CAM2_CompareMaxTemperature(imgView.CAM2_TemperatureArr);
                    VerifyOPC();

                    if (DIASDAQ.DDAQ_DEVICE_DO_ENABLE_NEXTMSG(1) != DIASDAQ.DDAQ_ERROR.NO_ERROR)                    /// 카메라가 새로운 데이터를 받을 수 있도록 Do Enable
                        return;
                }

            }
        }

        private static Bitmap GET_BITMAP(IntPtr hIRDX)
        {
            IntPtr pbitsImage = new IntPtr();
            IntPtr bmiImage = new IntPtr();
            ushort width = 0, height = 0;
            if (DIASDAQ.DDAQ_IRDX_IMAGE_GET_BITMAP(hIRDX, ref width, ref height, out pbitsImage, out bmiImage) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
            {
                return null; // failure
            }

            MethodInfo mi = typeof(Bitmap).GetMethod("FromGDIplus", BindingFlags.Static | BindingFlags.NonPublic);

            if (mi == null)
            {
                return null; // permission problem 
            }

            IntPtr pBmp = IntPtr.Zero;
            int status = DIASDAQ.GDIPLUS_GdipCreateBitmapFromGdiDib(bmiImage, pbitsImage, out pBmp);

            if ((status == 0) && (pBmp != IntPtr.Zero))
            {
                return (Bitmap)mi.Invoke(null, new object[] { pBmp }); // success 
            }
            else
            {
                return null; // failure
            }
        }

        //private void run_update()
        //{
        //    while (_pauseEvent.WaitOne())
        //    {
        //        c1_chartView.UpdateData();
        //        c1_gridView.RefreshGrid();
        //        result.CAM1_DetectTempThreshold();
        //        Pause();
        //        Thread.Sleep(100);
        //        _pauseEvent.WaitOne();
        //    }
        //}



        #endregion

        private void MainForm_Shown(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
        }

        private void 새로만들기ToolStripButton_Click(object sender, EventArgs e)
        {
            OpenNewDevice();
        }

        private void 열기ToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void 도움말ToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void PreviousRecord_toolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void NextRecord_toolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void KeepMoving_toolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void Pause_toolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void DrawPOI_toolStripButton_Click(object sender, EventArgs e)
        {
            Activate_DrawPOI = true;
        }

        private void MovePOI_toolStripButton_Click(object sender, EventArgs e)
        {
            Activate_DrawPOI = false;
        }

        private void DeletePOI_toolStripButton_Click(object sender, EventArgs e)
        {
            Activate_DrawPOI = false;
            imgView.DeletePOI_InArray();
            if (imgView.isCAM1Focused)
            {
                imgView.CAM1_POICheckFlag = true;
            }
            else if (imgView.isCAM2Focused)
            {
                imgView.CAM2_POICheckFlag = true;
            }
        }

        private void LogStart_toolStripButton_Click(object sender, EventArgs e)
        {
            DeviceLoggingStart();
            LogStart_toolStripButton.Enabled = false;
            LogStop_toolStripButton.Enabled = true;
        }

        private void LogStop_toolStripButton_Click(object sender, EventArgs e)
        {
            DeviceLoggingStop();
            LogStart_toolStripButton.Enabled = true;
            LogStop_toolStripButton.Enabled = false;
        }

        public float FloatMaxTemp = 0.0f;
        public void CompareMaxTemperature(float[] TemperatureArray)
        {
            FloatMaxTemp = 0.0f;
            string MaxTemp = "";
            //if (imageView.CAM1_TemperatureArr)
            //for(int i=0; i<TemperatureArray.Length-1; i++)
            if (imgView.CAM1_POICount == 0)
            {
                FloatMaxTemp = 0.0f;
                MaxTemp = FloatMaxTemp.ToString("N1") + "℃";
                textBox3.Text = MaxTemp;
                return;
            }

            for (int i = 0; i < imgView.CAM1_POICount; i++)
            {
                if (FloatMaxTemp < TemperatureArray[i])
                {
                    FloatMaxTemp = TemperatureArray[i];
                }
            }
            MaxTemp = FloatMaxTemp.ToString("N1") + "℃";
            textBox3.Text = MaxTemp;
        }

        public float c2_FloatMaxTemp = 0.0f;
        public void CAM2_CompareMaxTemperature(float[] TemperatureArray)
        {
            c2_FloatMaxTemp = 0.0f;
            string MaxTemp = "";
            //if (imageView.CAM1_TemperatureArr)
            if (imgView.CAM2_POICount == 0)
            {
                c2_FloatMaxTemp = 0.0f;
                MaxTemp = c2_FloatMaxTemp.ToString("N1") + "℃";
                textBox4.Text = MaxTemp;
                return;
            }

            for (int i = 0; i < imgView.CAM2_POICount; i++)
            {
                if (c2_FloatMaxTemp < TemperatureArray[i])
                {
                    c2_FloatMaxTemp = TemperatureArray[i];
                }
            }
            MaxTemp = c2_FloatMaxTemp.ToString("N1") + "℃";
            textBox4.Text = MaxTemp;
        }

        #region DataLoggingControl

        private string GetNewDataFileName(int SaveType, IntPtr irdxHandle)
        {
            string newRawDataFolderName = "";
            string newResultDataFolderName = "";
            string strFileName = "";
            uint id = 0;
            DIASDAQ.DDAQ_DEVICE_TYPE type = DIASDAQ.DDAQ_DEVICE_TYPE.NO;

            DIASDAQ.DDAQ_IRDX_DEVICE_GET_ID(irdxHandle, ref id, ref type);

            DateTime time = DateTime.Now;
            string currentTime = time.ToString("yyyyMMdd_HHmmss");
            string currentTime_DateOnly = time.ToString("yyMMdd");
            strFileName = "[" + id.ToString() + "]";

            DirectoryInfo VerifyRawFolder = new DirectoryInfo(customGrid.RawData_Location);
            DirectoryInfo VerifyResultFolder = new DirectoryInfo(customGrid.ResultData_Location);

            bool isRawFolderExist = VerifyRawFolder.Exists;
            bool isResultFolderExist = VerifyResultFolder.Exists;

            //bool isRawFolderExist = File.Exists(customGrid.RawData_Location);
            //bool isResultFolderExist = File.Exists(customGrid.ResultData_Location);


            if (!isRawFolderExist || !isResultFolderExist)
            {
                string appPath = Application.StartupPath;
                switch (SaveType)
                {
                    case 0:
                        customGrid.RawData_Location = appPath;
                        customGrid.RawData_Location += "\\RawData";
                        Directory.CreateDirectory(customGrid.RawData_Location);
                        newRawDataFolderName = customGrid.RawData_Location + "\\" + currentTime_DateOnly;
                        Directory.CreateDirectory(newRawDataFolderName);
                        break;
                    case 1:
                        customGrid.RawData_Location = appPath;
                        customGrid.RawData_Location += "\\RawData";
                        Directory.CreateDirectory(customGrid.RawData_Location);
                        newRawDataFolderName = customGrid.RawData_Location + "\\" + currentTime_DateOnly;
                        Directory.CreateDirectory(newRawDataFolderName);
                        break;
                    case 2:
                        customGrid.ResultData_Location = appPath;
                        customGrid.ResultData_Location += "\\ResultData";
                        Directory.CreateDirectory(customGrid.ResultData_Location);
                        newResultDataFolderName = customGrid.ResultData_Location + "\\" + currentTime_DateOnly;
                        Directory.CreateDirectory(newResultDataFolderName);
                        break;
                }
            }
            if (isRawFolderExist)
            {
                //newRawDataFolderName = customGrid.RawData_Location + "\\" + currentTime;
                //Directory.CreateDirectory(newRawDataFolderName);
                newRawDataFolderName = customGrid.RawData_Location + "\\RawData";
                Directory.CreateDirectory(newRawDataFolderName);
                //newRawDataFolderName += currentTime_DateOnly;
                newRawDataFolderName = newRawDataFolderName + "\\" + currentTime_DateOnly;
                Directory.CreateDirectory(newRawDataFolderName);
            }
            if (isResultFolderExist)
            {
                //newResultDataFolderName = customGrid.ResultData_Location+ "\\" + currentTime;
                //Directory.CreateDirectory(newResultDataFolderName);
                newResultDataFolderName = customGrid.ResultData_Location + "\\ResultData";
                Directory.CreateDirectory(newResultDataFolderName);
                //newResultDataFolderName += currentTime_DateOnly;
                newResultDataFolderName = newResultDataFolderName + "\\" + currentTime_DateOnly;
                Directory.CreateDirectory(newResultDataFolderName);
            }

            switch (SaveType)
            {
                case 0:
                    strFileName = strFileName + currentTime + ".irdx";
                    strFileName = newRawDataFolderName + "\\" + strFileName;
                    break;
                case 1:
                    //strFileName = strFileName + time.ToString() + ".txt";
                    strFileName = strFileName + currentTime + ".txt";
                    strFileName = newRawDataFolderName + "\\" + strFileName;
                    //strFileName = "[" + id.ToString() + "]" + " Raw";
                    break;
                case 2:
                    strFileName = strFileName + currentTime + ".txt";
                    strFileName = newResultDataFolderName + "\\" + strFileName;
                    //strFileName = "[" + id.ToString() + "]" + " Result";
                    break;
            }

            return strFileName;
        }

        System.Windows.Forms.Timer LoggingTimer = new System.Windows.Forms.Timer();
        FileStream Text_RawData;
        FileStream Text_ResultData;
        StreamWriter outputFile;
        StreamWriter outputFile_Result;

        FileStream c2_Text_RawData;
        FileStream c2_Text_ResultData;
        StreamWriter c2_outputFile;
        StreamWriter c2_outputFile_Result;

        private void DeviceLoggingStart()
        {
            if (isLoggingRunning)
            {
                // 이미 데이터 로깅이 진행 중임
                MessageBox.Show("이미 진행 중인 Data Logging 세션이 존재합니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!isLoggingRunning)
            {
                isLoggingRunning = true;

                propertyGrid1.Enabled = false;      // Propertygrid Disable
                DrawPOI_toolStripButton.Enabled = false;   // Draw POI Disable
                MovePOI_toolStripButton.Enabled = false;   // Move POI Disable
                DeletePOI_toolStripButton.Enabled = false;   // Delete POI Disable

                int IRDX = 0, RawData = 1, ResultData = 2;

                NewIRDXFileName = GetNewDataFileName(IRDX, pIRDX_Array[0]);
                newRawDataFileName = GetNewDataFileName(RawData, pIRDX_Array[0]);
                newResultDataFileName = GetNewDataFileName(ResultData, pIRDX_Array[0]);

                Text_RawData = new FileStream(newRawDataFileName, FileMode.Append, FileAccess.Write);
                Text_ResultData = new FileStream(newResultDataFileName, FileMode.Append, FileAccess.Write);

                outputFile = new StreamWriter(Text_RawData);
                outputFile_Result = new StreamWriter(Text_ResultData);

                //if (imageView.CAM1_POICount == 0)
                //{
                //    MessageBox.Show("저장할 데이터를 확인하세요.\n저장 프로세스가 시작되지 않았습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    DeviceLoggingStop();
                //    return;
                //}

                string legend = "Index\t";
                for (int k = 0; k < imgView.CAM1_POICount; k++)
                {
                    legend = legend + "POI #" + (k + 1).ToString() + "\tTemperature\t";
                }
                outputFile_Result.WriteLine(legend);

                // 카메라가 두대 붙어있으면 한번 더 하면 됨
                if (pIRDX_Array[1] != IntPtr.Zero)
                {
                    CAM2_NewIRDXFileName = GetNewDataFileName(IRDX, pIRDX_Array[1]);
                    CAM2_newRawDataFileName = GetNewDataFileName(RawData, pIRDX_Array[1]);
                    CAM2_newResultDataFileName = GetNewDataFileName(ResultData, pIRDX_Array[1]);

                    c2_Text_RawData = new FileStream(CAM2_newRawDataFileName, FileMode.Append, FileAccess.Write);
                    c2_Text_ResultData = new FileStream(CAM2_newResultDataFileName, FileMode.Append, FileAccess.Write);

                    c2_outputFile = new StreamWriter(c2_Text_RawData);
                    c2_outputFile_Result = new StreamWriter(c2_Text_ResultData);

                    string legend2 = "Index\t";
                    for (int k = 0; k < imgView.CAM2_POICount; k++)
                    {
                        legend2 = legend2 + "POI #" + (k + 1).ToString() + "\tTemperature\t";
                    }
                    c2_outputFile_Result.WriteLine(legend2);
                }

                LoggingTimer.Interval = 1000;   // ms 간격으로 tick 동작
                LoggingTimer.Tick += T_Tick;    // tick이 하나씩 돌때마다 T_Tick 함수 작동
                LoggingTimer.Start();
            }
        }

        IntPtr irdxHandle_write = new IntPtr();
        IntPtr c2_irdxHandle_write = new IntPtr();
        int tickCount = 0;

        private void T_Tick(object sender, EventArgs e)
        {
            string test = "TEXT WRITING TEST: RawData\n";
            string test2 = "TEXT WRITING TEST: ResultData\n";

            // 어떤 데이터를 Append해서 텍스트파일에 쓸 것인지 써주면 됨
            // index    poi #   temp    poi #   temp    ...
            test2 = tickCount.ToString();
            for (int i = 0; i < imgView.CAM1_POICount; i++)
            {
                test2 = test2 + "\t\t" + imgView.CAM1_TemperatureArr[i];
            }
            outputFile_Result.WriteLine(test2);

            if (irdxHandle_write == IntPtr.Zero) // write용 irdxHandle이 없으면 새로 만들고
                DIASDAQ.DDAQ_IRDX_FILE_OPEN_WRITE(NewIRDXFileName, true, ref irdxHandle_write);
            else if (irdxHandle_write != IntPtr.Zero)   // 이미 있으면 이어붙이면 됨
                DIASDAQ.DDAQ_IRDX_FILE_ADD_IRDX(irdxHandle_write, pIRDX_Array[0]);

            // 카메라가 두대 붙어있으면 똑같이 더하면 됨
            if (pIRDX_Array[1] != IntPtr.Zero)
            {
                test2 = tickCount.ToString();
                for (int i = 0; i < imgView.CAM2_POICount; i++)
                {
                    test2 = test2 + "\t\t" + imgView.CAM2_TemperatureArr[i];
                }
                c2_outputFile_Result.WriteLine(test2);
                if (c2_irdxHandle_write == IntPtr.Zero)
                    DIASDAQ.DDAQ_IRDX_FILE_OPEN_WRITE(CAM2_NewIRDXFileName, true, ref c2_irdxHandle_write);
                else if (c2_irdxHandle_write != IntPtr.Zero)
                    DIASDAQ.DDAQ_IRDX_FILE_ADD_IRDX(c2_irdxHandle_write, pIRDX_Array[1]);
            }

            tickCount++;
        }



        private void DeviceLoggingStop()
        {
            isLoggingRunning = false;
            LoggingTimer.Stop();    // tick timer 정지 시키고
            DIASDAQ.DDAQ_IRDX_FILE_CLOSE(irdxHandle_write); // 쓰고있던 irdx파일 닫고
            irdxHandle_write = IntPtr.Zero;     // 쓰기용 irdx handle 초기화
            tickCount = 0;  // tick count 초기화

            propertyGrid1.Enabled = true;       // Propertygrid Enable
            DrawPOI_toolStripButton.Enabled = true;   // Draw POI Enable
            MovePOI_toolStripButton.Enabled = true;   // Mobe POI Enable
            DeletePOI_toolStripButton.Enabled = true;   // Delete POI Enable

            // 텍스트파일 다 썼으니까 닫자
            outputFile.Close();
            outputFile_Result.Close();
            Text_RawData.Close();
            Text_ResultData.Close();

            if (DetectedDevices == 2)
            {
                DIASDAQ.DDAQ_IRDX_FILE_CLOSE(c2_irdxHandle_write);
                c2_irdxHandle_write = IntPtr.Zero;
                c2_outputFile.Close();
                c2_outputFile_Result.Close();
                c2_Text_RawData.Close();
                c2_Text_ResultData.Close();
            }
        }

        #endregion

        #region ConfigurationControl

        System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        private void LoadConfiguration()
        {
            string value = "";
            value = ConfigurationManager.AppSettings["Emissivity"];
            customGrid.Emissivity = Convert.ToSingle(value);
            value = ConfigurationManager.AppSettings["Transmission"];
            customGrid.Transmission = Convert.ToSingle(value);
            value = ConfigurationManager.AppSettings["AmbientTemp"];
            customGrid.AmbientTemperature = Convert.ToSingle(value);

            value = ConfigurationManager.AppSettings["Maximum"];
            cMaxTemp = Convert.ToSingle(value);
            value = ConfigurationManager.AppSettings["Minimum"];
            cMinTemp = Convert.ToSingle(value);

            value = ConfigurationManager.AppSettings["RawData_Location"];
            customGrid.RawData_Location = value;
            value = ConfigurationManager.AppSettings["ResultData_Location"];
            customGrid.ResultData_Location = value;
        }

        private void SaveConfiguration()
        {
            // Saving PropertyGrid
            config.AppSettings.Settings["Emissivity"].Value = customGrid.Emissivity.ToString();
            config.AppSettings.Settings["Transmission"].Value = customGrid.Transmission.ToString();
            config.AppSettings.Settings["AmbientTemp"].Value = customGrid.AmbientTemperature.ToString();
            config.AppSettings.Settings["Maximum"].Value = customGrid.Maximum.ToString();
            config.AppSettings.Settings["Minimum"].Value = customGrid.Minimun.ToString();
            config.AppSettings.Settings["RawData_Location"].Value = customGrid.RawData_Location;
            config.AppSettings.Settings["ResultData_Location"].Value = customGrid.ResultData_Location;
            //config.AppSettings.Settings["Threshold"].Value = customGrid.Threshold.ToString();

            // Saving POI Temperature
            string appSettingValue = "";
            for (int i = 0; i < 10; i++)
            {
                appSettingValue = "CAM1_Threshold" + (i + 1).ToString();
                config.AppSettings.Settings[appSettingValue].Value = result.CAM1_ThresholdTemp[i].ToString();
                appSettingValue = "CAM2_Threshold" + (i + 1).ToString();
                config.AppSettings.Settings[appSettingValue].Value = result.CAM2_ThresholdTemp[i].ToString();
            }

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }


        #endregion


        private void ViewAdjust()
        {
            // 전체 ImageView 영역 width 조정
            split_ViewToInfo.SplitterDistance = 1920 - propertyGrid1.Width - 310;

            // 카메라별 ImageView 영역 Width 조정
            split_CamToCam.Width = split_ViewToInfo.Panel1.Width / 2;

            // CAM #1 영역 Height 조정
            split_CAM1Info.SplitterDistance = 70;
            split_CAM1ChartGrid.SplitterDistance = 265;
            split_CAM2Info.SplitterDistance = split_CAM1Info.Panel1.Height;
            split_CAM2ChartGrid.SplitterDistance = split_CAM1ChartGrid.Panel1.Height;
        }

        private void VerifyOPC()
        {
            if (opc.detected)
            {
                result.OPCConnectAlarm.ForeColor = Color.Green;
            }
            else
            {
                result.OPCConnectAlarm.ForeColor = Color.Red;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        System.Windows.Forms.Timer OPCTimer = new System.Windows.Forms.Timer();
        private void OPC_DataSending(object sender, EventArgs e)
        {
            if (OPCActivated)
            {
                opc.OPC_Read();
                opc.OPC_Write();
            }
            else
            {
                InitOPCTimer();
            }
            
        }

        public void InitOPCTimer()
        {
            OPCTimer.Interval = 1000;
            OPCTimer.Tick += new EventHandler(OPC_DataSending);
            OPCTimer.Start();
        }

    }
}
