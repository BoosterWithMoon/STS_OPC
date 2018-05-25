using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using System.Configuration;
using System.Globalization;
using System.Runtime.CompilerServices;
using Kepware.ClientAce.OpcDaClient;
using Kepware.ClientAce.OpcCmn;
using System.Collections;
using STS.Core;

namespace Electric_Furnance_Monitoring_OPC_Included_
{
    public partial class MainForm : Form
    {
        #region ClassDeclare
        NewDeviceForm newDevice;

        SystemPropertyGrid customGrid;

        ImageView imgView;

        CAM1_ImageView c1_imgView;
        CAM2_ImageView c2_imgView;

        CAM1_ChartView c1_chartView;
        CAM2_ChartView c2_chartView;

        CAM1_DataGridView c1_gridView;
        CAM2_DataGridView c2_gridView;

        ResultView result;

        CustomOPC opc;
        OPCSetting opcSet;

        OpenFileDialog openDlg;

        Thread mThread;
        Thread mThread_two;

        Thread CAM1_DataView;
        Thread CAM2_DataView;

        AboutForm about;

        STS.Core.Calculation cal = new Calculation();
        STS.Core.Drawing draw = new Drawing();
        STS.Core.CoreLibrary core = new CoreLibrary();
        #endregion

        #region VariablesDeclare
        public uint DetectedDevices;
        public IntPtr[] pIRDX_Array;

        public ushort[] sizeX_Array;
        public ushort[] sizeY_Array;

        public float currentEmissivity;
        public float currentTransmittance;
        public float currentAmbientTemp;

        public float cMaxTemp;
        public float cMinTemp;
        public float Scale_MaxTemp;
        public float Scale_MinTemp;

        public uint NumOfDataRecord;
        public uint CurDataRecord;

        public float m_fps;
        public ushort m_avg;

        private bool isClosing;

        private bool isDrawnCAM1Image;
        private bool isDrawnCAM2Image;

        public uint acq_index;

        public string fileFullName;
        public ushort sizeX;
        public ushort sizeY;
        public ushort sizeX_2;
        public ushort sizeY_2;
        public float min, max;

        public bool Activate_DrawPOI;

        string NewIRDXFileName;
        string newRawDataFileName;
        string newResultDataFileName;
        string CAM2_NewIRDXFileName;
        string CAM2_newRawDataFileName;
        string CAM2_newResultDataFileName;
        string newOPCReadDataFileName;
        string newOPCWriteDataFileName;
        public bool isLoggingRunning;

        public System.Windows.Forms.Timer GraphUpdateTimer;
        public System.Windows.Forms.Timer dataplayerTimer;
        public System.Windows.Forms.Timer OPCTimer;
        public bool OPCActivated;

        private uint position;
        private uint numDataSet;

        bool isTimerRunning;

        public int IRDXFileCount;

        private static ushort DDAQ_MOTORFOCUS_CMD_NEAR_STEP = 4; //< move focus one step to near
        private static ushort DDAQ_MOTORFOCUS_CMD_FAR_STEP = 5; //< move focus one step to far

        [DllImport("kernel32.dll")]
        public static extern void Beep(int frequency, int duration);

        public Label[] ProgressLabel;

        public enum OpenMode : int
        {
            Online = 1,
            Simulation = 2,
            IRDX = 3
        }
        public OpenMode currentOpenMode;

        System.Windows.Forms.Timer LoggingTimer;
        public DateTime time;
        //FileStream Text_RawData;
        FileStream Text_ResultData;
        FileStream Text_OPCReadData;
        FileStream Text_OPCWriteData;
        //StreamWriter outputFile;
        StreamWriter outputFile_Result;
        StreamWriter outputFile_OPCRead;
        StreamWriter outputFile_OPCWrite;

        //FileStream c2_Text_RawData;
        FileStream c2_Text_ResultData;
        //StreamWriter c2_outputFile;
        StreamWriter c2_outputFile_Result;

        IntPtr irdxHandle_write;
        IntPtr c2_irdxHandle_write;
        int tickCount;

        System.Configuration.Configuration config;

        //public string POSCO_CAM1_SERIAL;
        //public string POSCO_CAM2_SERIAL;
        public string[] POSCO_SERIAL = new string[2];

        public float FloatMaxTemp;
        public float c2_FloatMaxTemp;

        #endregion

        public MainForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            
            #region ClassAllocation
            newDevice = new NewDeviceForm(this);

            customGrid = new SystemPropertyGrid(this);

            imgView = new ImageView(this);
            c1_imgView = new CAM1_ImageView(this);
            c2_imgView = new CAM2_ImageView(this);

            c1_chartView = new CAM1_ChartView(this);
            c2_chartView = new CAM2_ChartView(this);

            c1_gridView = new CAM1_DataGridView(this);
            c2_gridView = new CAM2_DataGridView(this);

            result = new ResultView(this);

            opc = new CustomOPC(this);
            opcSet = new OPCSetting(this);

            openDlg = new OpenFileDialog();

            mThread = new Thread(new ThreadStart(run));
            mThread_two = new Thread(new ThreadStart(run_two));

            CAM1_DataView = new Thread(new ThreadStart(CAM1_AllView));
            CAM2_DataView = new Thread(new ThreadStart(CAM2_AllView));

            about = new AboutForm(this);
            #endregion

            #region VariableInitialize
            DetectedDevices = 0;
            pIRDX_Array = new IntPtr[2];

            sizeX_Array = new ushort[2];
            sizeY_Array = new ushort[2];

            currentEmissivity = 1.0f;
            currentTransmittance = 1.0f;
            currentAmbientTemp = 25;

            cMaxTemp = 0;
            cMinTemp = 0;
            Scale_MaxTemp = 500;
            Scale_MinTemp = 0;

            NumOfDataRecord = 0;
            CurDataRecord = 0;

            m_fps = 0;
            m_avg = 0;

            isClosing = false;

            isDrawnCAM1Image = false;
            isDrawnCAM2Image = false;

            acq_index = 0;

            fileFullName = "";
            sizeX = 0;
            sizeY = 0;
            sizeX_2 = 0;
            sizeY_2 = 0;
            min = 0.0f;
            max = 0.0f;

            Activate_DrawPOI = false;

            NewIRDXFileName = "";
            newRawDataFileName = "";
            newResultDataFileName = "";
            CAM2_NewIRDXFileName = "";
            CAM2_newRawDataFileName = "";
            CAM2_newResultDataFileName = "";
            isLoggingRunning = false;

            GraphUpdateTimer = new System.Windows.Forms.Timer();
            dataplayerTimer = new System.Windows.Forms.Timer();
            OPCTimer = new System.Windows.Forms.Timer();

            OPCActivated = false;

            position = 0;
            numDataSet = 0;

            isTimerRunning = false;

            IRDXFileCount = 0;

            ProgressLabel = new Label[9];

            currentOpenMode = OpenMode.IRDX;

            LoggingTimer = new System.Windows.Forms.Timer();
            irdxHandle_write = new IntPtr();
            c2_irdxHandle_write = new IntPtr();
            tickCount = 0;

            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            FloatMaxTemp = 0.0f;
            c2_FloatMaxTemp = 0.0f;
            #endregion
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadConfiguration();

            InitPropertyGrid();

            // CAMERA #1,2 status
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

            split_ViewToInfo.Visible = false;

            toolStripSeparator3.Visible = false;
            toolStripSeparator4.Visible = false;
            toolStripSeparator5.Visible = false;
            toolStripSeparator6.Visible = false;

            // program buttons
            PreviousRecord_toolStripButton.Visible = false;     // Previous Record
            NextRecord_toolStripButton.Visible = false;     // Next Record
            KeepMoving_toolStripButton.Visible = false;     // Play Record
            Pause_toolStripButton.Visible = false;          // Pause Record

            DrawPOI_toolStripButton.Visible = false;   // Draw POI
            DeletePOI_toolStripButton.Visible = false;   // Delete POI
            MovePOI_toolStripButton.Visible = false;   // Move POI

            LogStart_toolStripButton.Visible = false;   // Log Start
            LogStop_toolStripButton.Visible = false;   // Log Stop

            MoveFocus_FarStep.Visible = false;      // Move DeviceFocus
            MoveFocus_NearStep.Visible = false;

            // menu items
            previousRecordToolStripMenuItem.Enabled = false;
            nextRecordToolStripMenuItem.Enabled = false;
            playToolStripMenuItem.Enabled = false;
            stopToolStripMenuItem1.Enabled = false;

            drawROIToolStripMenuItem.Enabled = false;
            deleteROIToolStripMenuItem.Enabled = false;
            moveROIToolStripMenuItem.Enabled = false;

            startToolStripMenuItem.Enabled = false;
            stopToolStripMenuItem.Enabled = false;

            moveToNearStepToolStripMenuItem.Enabled = false;
            moveToFarStepToolStripMenuItem.Enabled = false;

            OPCSettingToolStripMenuItem.Enabled = false;

            //groupBox_CamTemp.Visible = false;
            //groupBox_DetectorTemp.Visible = false;
            panel1.Visible = false;
            panel_ScaleBar.Visible = false;

            propertyGrid1.Visible = false;

            ViewAdjust();

            //opc.ServerDetection();
            //opc.ServerConnection();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isLoggingRunning)
            {
                DeviceLoggingStop();
            }

            SaveConfiguration();

            if (OPCActivated)
            {
                OPCTimer.Stop();
                OPCActivated = false;
            }

            opc.ServerDisconnection();

            isClosing = true;

            Thread.Sleep(3);

            // 스레드 중에 하나라도 돌고있으면 우선 강제종료
            if (mThread.IsAlive || mThread_two.IsAlive || CAM1_DataView.IsAlive || CAM2_DataView.IsAlive)
            {
                mThread.Abort();
                mThread_two.Abort();
                CAM1_DataView.Abort();
                CAM2_DataView.Abort();
            }

            DIASDAQ.DDAQ_DEVICE_DO_STOP(1);
            DIASDAQ.DDAQ_DEVICE_DO_STOP(2);

            if (DetectedDevices != 0)
            {
                DIASDAQ.DDAQ_DEVICE_DO_CLOSE(1);
                DIASDAQ.DDAQ_DEVICE_DO_CLOSE(2);
            }

            c1_chartView.axTChart1.Dispose();
            c2_chartView.axTChart1.Dispose();
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            ViewAdjust();
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

        public object Support_Thread1_forPublicRef() { return CAM1_DataView; }

        public object Support_Thread2_forPublicRef() { return CAM2_DataView; }

        #endregion


        #region InitView
        private void ViewAdjust()
        {
            // 전체 ImageView 영역 width 조정
            if (this.Width - propertyGrid1.Width - 310 < 0)
            {
                return;
            }
            else
            {
                split_ViewToInfo.SplitterDistance = this.Width - propertyGrid1.Width - 315 - panel_ScaleBar.Width;
            }

            // 카메라별 ImageView 영역 Width 조정
            split_CamToCam.Width = split_ViewToInfo.Panel1.Width / 2;

            // CAM #1 영역 Height 조정
            split_CAM1Info.SplitterDistance = 140;
            split_CAM1ChartGrid.SplitterDistance = 255;

            split_CAM2Info.SplitterDistance = split_CAM1Info.Panel1.Height;
            split_CAM2ChartGrid.SplitterDistance = split_CAM1ChartGrid.Panel1.Height;
        }

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

            ProgressLabel[0] = label_Charging1;
            ProgressLabel[1] = label_Melting1;
            ProgressLabel[2] = label_Charging2;
            ProgressLabel[3] = label_Melting2;
            ProgressLabel[4] = label_Charging3;
            ProgressLabel[5] = label_Melting3;
            ProgressLabel[6] = label_StandSteel;
            ProgressLabel[7] = label_Tapping;
            ProgressLabel[8] = label_O2Lance;

            for(int i=0; i<9; i++)
            {
                ProgressLabel[i].ForeColor = result.NotConnected;
            }
        }
        #endregion
        

        #region Thread
        private static EventWaitHandle ThreadOne_WFSO = new EventWaitHandle(false, EventResetMode.AutoReset);
        private static EventWaitHandle ThreadTwo_WFSO = new EventWaitHandle(false, EventResetMode.AutoReset);
        private void run()  // Current: 320L
        {
            bool newDataReady = false;

            while (true)
            {
                Thread.Sleep(10);

                VerifyOPC();
                if (currentOpenMode == OpenMode.Online && pictureBox_ScaleBar.Image == null)
                {
                    imgView.DrawScaleBar(pIRDX_Array[0], pictureBox_ScaleBar);
                }

                core.DoTransfer(newDataReady, isClosing, DetectedDevices, CAM1_CameraTemp, CAM1_DetectorTemp);

                imgView.DrawImage(pIRDX_Array[0], c1_imgView.pictureBox1);
                CompareMaxTemperature(imgView.CAM1_TemperatureArr);
                result.CAM1_DetectTemp_ForOPC();

                isDrawnCAM1Image = true;

                ThreadOne_WFSO.Set();
            }
        }

        private void run_two()  // Current: 512N
        {
            bool newDataReady = false;

            while (true)
            {
                Thread.Sleep(10);

                core.DoTransfer(newDataReady, isClosing, 1, CAM2_CameraTemp, CAM2_DetectorTemp);

                //if (DIASDAQ.DDAQ_DEVICE_GET_NEWDATAREADY(1, ref newDataReady) != DIASDAQ.DDAQ_ERROR.NO_ERROR)       /// 카메라가 새로운 데이터를 받을 준비가 되었을 시
                //{
                //    DIASDAQ.DDAQ_DEVICE_DO_STOP(1);
                //    return;
                //}
                //if (newDataReady && !isClosing)
                //{
                //    if (DIASDAQ.DDAQ_DEVICE_DO_UPDATEDATA(1) != DIASDAQ.DDAQ_ERROR.NO_ERROR)                        /// 새로운 데이터가 있으며 종료의 명령이 없을 시
                //        return;                                                                                     /// UpDate Data !!

                //    //if (img != null) img.Dispose();                                                                 /// 메모리 관리를 위하여 Dispose.

                //    float fTemp = 0.0f;
                //    bool bTemp = false;
                //    DIASDAQ.DDAQ_DEVICE_GET_CAMERATEMP(1, ref fTemp, ref bTemp);
                //    CAM2_CameraTemp.Text = fTemp.ToString("N1") + "℃";

                //    DIASDAQ.DDAQ_DEVICE_GET_DETECTORTEMP(1, ref fTemp, ref bTemp);
                //    CAM2_DetectorTemp.Text = fTemp.ToString("N1") + "℃";

                    imgView.CAM2_DrawImage(pIRDX_Array[1], c2_imgView.pictureBox1);

                CAM2_CompareMaxTemperature(imgView.CAM2_TemperatureArr);
                result.CAM2_DetectTemp_ForOPC();
       

                isDrawnCAM2Image = true;

                    //if (DIASDAQ.DDAQ_DEVICE_DO_ENABLE_NEXTMSG(1) != DIASDAQ.DDAQ_ERROR.NO_ERROR)                    /// 카메라가 새로운 데이터를 받을 수 있도록 Do Enable
                    //    return;

                    ThreadTwo_WFSO.Set();
                //}

            }
        }

        private void CAM1_AllView()
        {
            while (true)
            {
                Thread.Sleep(10);
                ThreadOne_WFSO.WaitOne();

                if (!isDrawnCAM1Image)
                {
                    continue;
                }

                else if (isDrawnCAM1Image)
                {
                    c1_gridView.RefreshGrid();
                    result.CAM1_DetectTempThreshold();
                    isDrawnCAM1Image = false;
                }
            }
        }

        private void CAM2_AllView()
        {
            while (true)
            {
                Thread.Sleep(10);
                ThreadTwo_WFSO.WaitOne();

                if (!isDrawnCAM2Image)
                {
                    continue;
                }
                else if (isDrawnCAM2Image)
                {
                    c2_gridView.CAM2_RefreshGrid();
                    result.CAM2_DetectTempThreshold();
                    isDrawnCAM2Image = false;
                }
            }
        }
        #endregion


        #region ToolStripButton
        private void 새로만들기ToolStripButton_Click(object sender, EventArgs e)
        {
            OpenNewDevice();
        }

        private void 열기ToolStripButton_Click(object sender, EventArgs e)
        {
            OpenIRDX();
        }

        private void 도움말ToolStripButton_Click(object sender, EventArgs e)
        {
            OpenAbout();
        }

        private void PreviousRecord_toolStripButton_Click(object sender, EventArgs e)
        {
            DIASDAQ.DDAQ_IRDX_FILE_GET_CURDATASET(pIRDX_Array[0], ref position);
            //MessageBox.Show(position.ToString());
            if (position == 0)
            {
                // position이 이미 시작점에 있다면 아무것도 하지 않음
            }
            else
            {
                DIASDAQ.DDAQ_IRDX_FILE_SET_CURDATASET(pIRDX_Array[0], position - 1);
                position--;
            }
            CurDataRecord = position;

            float pData = 0;
            uint bufferSize = 0;
            DIASDAQ.DDAQ_IRDX_PIXEL_GET_DATA(pIRDX_Array[0], ref pData, bufferSize);

            imgView.DrawImage(pIRDX_Array[0], c1_imgView.pictureBox1);

            customGrid.GetAttributesInfo(pIRDX_Array[0]);
        }

        private void NextRecord_toolStripButton_Click(object sender, EventArgs e)
        {
            DIASDAQ.DDAQ_IRDX_FILE_GET_NUMDATASETS(pIRDX_Array[0], ref numDataSet);

            DIASDAQ.DDAQ_IRDX_FILE_GET_CURDATASET(pIRDX_Array[0], ref position);

            if (position + 1 == numDataSet)
            {
                // position이 DataSet의 끝까지 갔다면 제일 첫 프레임으로 보냄
                position = 0;
                dataplayerTimer.Stop();

                float pData = 0;
                uint bufferSize = 0;
                DIASDAQ.DDAQ_IRDX_PIXEL_GET_DATA(pIRDX_Array[0], ref pData, bufferSize);

                imgView.DrawImage(pIRDX_Array[0], c1_imgView.pictureBox1);

                cal.CalculateCurrentTemp(pIRDX_Array[0], imgView.CAM1_POICount, imgView.CAM1_ClickedPosition, imgView.CAM1_TemperatureArr);

                c1_chartView.UpdateData();
                c1_gridView.RefreshGrid();
                result.CAM1_DetectTempThreshold();

                customGrid.GetAttributesInfo(pIRDX_Array[0]);
            }
            else
            {
                DIASDAQ.DDAQ_IRDX_FILE_SET_CURDATASET(pIRDX_Array[0], position + 1);
                position++;

                CurDataRecord = position;

                float pData = 0;
                uint bufferSize = 0;
                DIASDAQ.DDAQ_IRDX_PIXEL_GET_DATA(pIRDX_Array[0], ref pData, bufferSize);

                imgView.DrawImage(pIRDX_Array[0], c1_imgView.pictureBox1);
                cal.CalculateCurrentTemp(pIRDX_Array[0], imgView.CAM1_POICount, imgView.CAM1_ClickedPosition, imgView.CAM1_TemperatureArr);

                c1_chartView.UpdateData();
                c1_gridView.RefreshGrid();
                result.CAM1_DetectTempThreshold();

                for (int i = 0; i < imgView.CAM1_POICount; i++)
                {
                    imgView.CAM2_ClickedPosition[i] = imgView.CAM1_ClickedPosition[i];
                }

                customGrid.GetAttributesInfo(pIRDX_Array[0]);
            }
        }

        private void KeepMoving_toolStripButton_Click(object sender, EventArgs e)
        {
            if (isTimerRunning == true) { }
            else
            {
                InitTimerForPlayer();
            }
        }

        private void Pause_toolStripButton_Click(object sender, EventArgs e)
        {
            dataplayerTimer.Stop();
            isTimerRunning = false;
        }

        private void DrawPOI_toolStripButton_Click(object sender, EventArgs e)
        {
            Activate_DrawPOI = true;
            DrawPOI_toolStripButton.Checked = true;
            MovePOI_toolStripButton.Checked = false;
            DeletePOI_toolStripButton.Checked = false;
        }

        private void MovePOI_toolStripButton_Click(object sender, EventArgs e)
        {
            Activate_DrawPOI = false;
            DrawPOI_toolStripButton.Checked = false;
            MovePOI_toolStripButton.Checked = true;
            DeletePOI_toolStripButton.Checked = false;
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
        }

        private void LogStop_toolStripButton_Click(object sender, EventArgs e)
        {
            DeviceLoggingStop();
        }

        private void MoveToNearStep()
        {
            uint tempDeviceNo = 0;
            if (imgView.isCAM1Focused && !imgView.isCAM2Focused)
            {
                tempDeviceNo = DetectedDevices;     // 현재 뒤쪽 IP인 320L
            }
            else if (!imgView.isCAM1Focused && imgView.isCAM2Focused)
            {
                tempDeviceNo = 1;                          // 현재 앞쪽 IP인 512N
            }

            if (DIASDAQ.DDAQ_DEVICE_DO_MOTORFOCUS(tempDeviceNo, DDAQ_MOTORFOCUS_CMD_NEAR_STEP) == DIASDAQ.DDAQ_ERROR.NO_MOTORFOCUS)
            {
                MessageBox.Show("Device가 Motorfocus를 지원하지 않습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void MoveToFarStep()
        {
            uint tempDeviceNo = 0;
            if (imgView.isCAM1Focused && !imgView.isCAM2Focused)
            {
                tempDeviceNo = DetectedDevices;     // 현재 뒤쪽 IP인 320L
            }
            else if (!imgView.isCAM1Focused && imgView.isCAM2Focused)
            {
                tempDeviceNo = 1;                          // 현재 앞쪽 IP인 512N
            }

            if (DIASDAQ.DDAQ_DEVICE_DO_MOTORFOCUS(tempDeviceNo, DDAQ_MOTORFOCUS_CMD_FAR_STEP) == DIASDAQ.DDAQ_ERROR.NO_MOTORFOCUS)
            {
                MessageBox.Show("Device가 Motorfocus를 지원하지 않습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void NearStep_toolStripButtonClick(object sender, EventArgs e)
        {
            MoveToNearStep();
        }

        private void FarStep_toolStripButtonClick(object sender, EventArgs e)
        {
            MoveToFarStep();
        }
        #endregion


        #region ToolStripMenuItem
        private void newOnlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenNewDevice();
        }

        private void newSimulationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSimulation();
        }

        private void openIRDXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenIRDX();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeviceLoggingStart();
            LogStart_toolStripButton.Enabled = false;
            startToolStripMenuItem.Enabled = false;
            LogStop_toolStripButton.Enabled = true;
            stopToolStripMenuItem.Enabled = true;
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeviceLoggingStop();
            LogStart_toolStripButton.Enabled = true;
            startToolStripMenuItem.Enabled = true;
            LogStop_toolStripButton.Enabled = false;
            stopToolStripMenuItem.Enabled = false;
        }

        private void moveToNearStepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoveToNearStep();
        }

        private void moveToFarStepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoveToFarStep();
        }

        private void previousRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PreviousRecord_toolStripButton_Click(sender, e);
        }

        private void nextRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NextRecord_toolStripButton_Click(sender, e);
        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeepMoving_toolStripButton_Click(sender, e);
        }

        private void stopToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Pause_toolStripButton_Click(sender, e);
        }

        private void drawROIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrawPOI_toolStripButton_Click(sender, e);
        }

        private void moveROIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MovePOI_toolStripButton_Click(sender, e);
        }

        private void deleteROIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeletePOI_toolStripButton_Click(sender, e);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            도움말ToolStripButton_Click(sender, e);
        }

        private void oPCSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            opcSet.ShowDialog();
        }

        #endregion


        #region DataLoggingControl
        private string GetNewDataFileName(int SaveType, IntPtr irdxHandle)
        {
            time = DateTime.Now;
            string path = core.GetNewDataFileName(time, SaveType, irdxHandle, customGrid.RawData, customGrid.ResultData, config, propertyGrid1);
            return path;
        }

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

                LogStart_toolStripButton.Enabled = false;
                LogStop_toolStripButton.Enabled = true;

                propertyGrid1.Enabled = false;      // Propertygrid Disable
                DrawPOI_toolStripButton.Enabled = false;   // Draw POI Disable
                MovePOI_toolStripButton.Enabled = false;   // Move POI Disable
                DeletePOI_toolStripButton.Enabled = false;   // Delete POI Disable

                int IRDX = 0, RawData = 1, ResultData = 2, OPCRead = 3, OPCWrite = 4;

                NewIRDXFileName = GetNewDataFileName(IRDX, pIRDX_Array[0]);
                newRawDataFileName = GetNewDataFileName(RawData, pIRDX_Array[0]);
                newResultDataFileName = GetNewDataFileName(ResultData, pIRDX_Array[0]);
                newOPCReadDataFileName = GetNewDataFileName(OPCRead, pIRDX_Array[0]);
                newOPCWriteDataFileName = GetNewDataFileName(OPCWrite, pIRDX_Array[0]);

                Text_ResultData = new FileStream(newResultDataFileName, FileMode.Append, FileAccess.Write);
                outputFile_Result = new StreamWriter(Text_ResultData);

                string legend = "Index\t";
                for (int k = 0; k < imgView.CAM1_POICount; k++)
                {
                    legend = legend + "POI #" + (k + 1).ToString() + "\tTemperature\t";
                }
                outputFile_Result.WriteLine(legend);

                if (OPCActivated && OPCTimerActivated)
                {
                    Text_OPCReadData = new FileStream(newOPCReadDataFileName, FileMode.Append, FileAccess.Write);
                    Text_OPCWriteData = new FileStream(newOPCWriteDataFileName, FileMode.Append, FileAccess.Write);

                    outputFile_OPCRead = new StreamWriter(Text_OPCReadData);
                    outputFile_OPCWrite = new StreamWriter(Text_OPCWriteData);

                    // put OPCRead textfile legend
                    legend = "Index\t";
                    legend = SetTextfileLegend_OPCRead(legend);
                    outputFile_OPCRead.WriteLine(legend);

                    // put OPCWrite textfile legend
                    legend = "Index\t";
                    legend = SetTextfileLegend_OPCWrite(legend);
                    outputFile_OPCWrite.WriteLine(legend);
                }

                // 카메라가 두대 붙어있으면 한번 더 하면 됨
                if (pIRDX_Array[1] != IntPtr.Zero)
                {
                    CAM2_NewIRDXFileName = GetNewDataFileName(IRDX, pIRDX_Array[1]);
                    CAM2_newRawDataFileName = GetNewDataFileName(RawData, pIRDX_Array[1]);
                    CAM2_newResultDataFileName = GetNewDataFileName(ResultData, pIRDX_Array[1]);

                    c2_Text_ResultData = new FileStream(CAM2_newResultDataFileName, FileMode.Append, FileAccess.Write);
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

        private string SetTextfileLegend_OPCRead(string legend)
        {
            legend = legend + "SteelNo\t";
            legend = legend + "Slope Angle\t";
            legend = legend + "Charging1\t";
            legend = legend + "Melting1\t";
            legend = legend + "Charging2\t";
            legend = legend + "Melting2\t";
            legend = legend + "Charging3\t";
            legend = legend + "Melting3\t";
            legend = legend + "StandSteel\t";
            legend = legend + "Tapping\t";
            legend = legend + "O2LanceBlowing\t";

            return legend;
        }

        private string SetTextfileLegend_OPCWrite(string legend)
        {
            legend = legend + "Slope Angle\t";

            legend = legend + "CAM1_MaxTemp\t";

            legend = legend + "CAM1_Threshold01\t";
            legend = legend + "CAM1_Threshold02\t";
            legend = legend + "CAM1_Threshold03\t";
            legend = legend + "CAM1_Threshold04\t";
            legend = legend + "CAM1_Threshold05\t";
            legend = legend + "CAM1_Threshold06\t";
            legend = legend + "CAM1_Threshold07\t";
            legend = legend + "CAM1_Threshold08\t";
            legend = legend + "CAM1_Threshold09\t";
            legend = legend + "CAM1_Threshold10\t";

            legend = legend + "CAM1_ROI01Temp\t";
            legend = legend + "CAM1_ROI02Temp\t";
            legend = legend + "CAM1_ROI03Temp\t";
            legend = legend + "CAM1_ROI04Temp\t";
            legend = legend + "CAM1_ROI05Temp\t";
            legend = legend + "CAM1_ROI06Temp\t";
            legend = legend + "CAM1_ROI07Temp\t";
            legend = legend + "CAM1_ROI08Temp\t";
            legend = legend + "CAM1_ROI09Temp\t";
            legend = legend + "CAM1_ROI10Temp\t";

            legend = legend + "CAM2_MaxTemp\t";

            legend = legend + "CAM2_Threshold01\t";
            legend = legend + "CAM2_Threshold02\t";
            legend = legend + "CAM2_Threshold03\t";
            legend = legend + "CAM2_Threshold04\t";
            legend = legend + "CAM2_Threshold05\t";
            legend = legend + "CAM2_Threshold06\t";
            legend = legend + "CAM2_Threshold07\t";
            legend = legend + "CAM2_Threshold08\t";
            legend = legend + "CAM2_Threshold09\t";
            legend = legend + "CAM2_Threshold10\t";

            legend = legend + "CAM2_ROI01Temp\t";
            legend = legend + "CAM2_ROI02Temp\t";
            legend = legend + "CAM2_ROI03Temp\t";
            legend = legend + "CAM2_ROI04Temp\t";
            legend = legend + "CAM2_ROI05Temp\t";
            legend = legend + "CAM2_ROI06Temp\t";
            legend = legend + "CAM2_ROI07Temp\t";
            legend = legend + "CAM2_ROI08Temp\t";
            legend = legend + "CAM2_ROI09Temp\t";
            legend = legend + "CAM2_ROI10Temp\t";

            legend = legend + "CAM1_ROI01_Warning\t";
            legend = legend + "CAM1_ROI02_Warning\t";
            legend = legend + "CAM1_ROI03_Warning\t";
            legend = legend + "CAM1_ROI04_Warning\t";
            legend = legend + "CAM1_ROI05_Warning\t";
            legend = legend + "CAM1_ROI06_Warning\t";
            legend = legend + "CAM1_ROI07_Warning\t";
            legend = legend + "CAM1_ROI08_Warning\t";
            legend = legend + "CAM1_ROI09_Warning\t";
            legend = legend + "CAM1_ROI10_Warning\t";

            legend = legend + "CAM2_ROI01_Warning\t";
            legend = legend + "CAM2_ROI02_Warning\t";
            legend = legend + "CAM2_ROI03_Warning\t";
            legend = legend + "CAM2_ROI04_Warning\t";
            legend = legend + "CAM2_ROI05_Warning\t";
            legend = legend + "CAM2_ROI06_Warning\t";
            legend = legend + "CAM2_ROI07_Warning\t";
            legend = legend + "CAM2_ROI08_Warning\t";
            legend = legend + "CAM2_ROI09_Warning\t";
            legend = legend + "CAM2_ROI10_Warning\t";

            legend = legend + "CAM1_ROI01_Alarm\t";
            legend = legend + "CAM1_ROI02_Alarm\t";
            legend = legend + "CAM1_ROI03_Alarm\t";
            legend = legend + "CAM1_ROI04_Alarm\t";
            legend = legend + "CAM1_ROI05_Alarm\t";
            legend = legend + "CAM1_ROI06_Alarm\t";
            legend = legend + "CAM1_ROI07_Alarm\t";
            legend = legend + "CAM1_ROI08_Alarm\t";
            legend = legend + "CAM1_ROI09_Alarm\t";
            legend = legend + "CAM1_ROI10_Alarm\t";

            legend = legend + "CAM2_ROI01_Alarm\t";
            legend = legend + "CAM2_ROI02_Alarm\t";
            legend = legend + "CAM2_ROI03_Alarm\t";
            legend = legend + "CAM2_ROI04_Alarm\t";
            legend = legend + "CAM2_ROI05_Alarm\t";
            legend = legend + "CAM2_ROI06_Alarm\t";
            legend = legend + "CAM2_ROI07_Alarm\t";
            legend = legend + "CAM2_ROI08_Alarm\t";
            legend = legend + "CAM2_ROI09_Alarm\t";
            legend = legend + "CAM2_ROI10_Alarm\t";

            return legend;
        }

        private string PutText_OPCRead(string str)
        {
            str = str + opc.CurrentSteelNo.ToString() + "\t";
            str = str + opc.CurrentAngle.ToString() + "\t";
            str = str + opc.ChargingStatus[0].ToString() + "\t";
            str = str + opc.ChargingStatus[1].ToString() + "\t";
            str = str + opc.ChargingStatus[2].ToString() + "\t";
            str = str + opc.ChargingStatus[3].ToString() + "\t";
            str = str + opc.ChargingStatus[4].ToString() + "\t";
            str = str + opc.ChargingStatus[5].ToString() + "\t";
            str = str + opc.ChargingStatus[6].ToString() + "\t";
            str = str + opc.ChargingStatus[7].ToString() + "\t";
            str = str + opc.O2LanceResult.ToString() + "\t";

            return str;
        }

        private string PutText_OPCWrite(string str)
        {
            object[] valueArray = new object[opc.WriteTagCount];

            for(int k=0; k<opc.WriteTagCount; k++)
            {
                valueArray[k] = opc.Write_itemValues[k].Value;
            }

            for (int k = 0; k < opc.WriteTagCount; k++)
            {
                str = str + valueArray[k].ToString() + "\t";
            }

            return str;
        }

        private void T_Tick(object sender, EventArgs e)
        {
            AutodelSequence();      // 데이터 자동 삭제 SEQUENCE

            DateTime currentTime = DateTime.Now;
            if (time.Day != currentTime.Day)
            {
                DeviceLoggingStop();
                DeviceLoggingStart();
            }

            //string test = "TEXT WRITING TEST: RawData\n";
            string data = "";

            // 어떤 데이터를 Append해서 텍스트파일에 쓸 것인지 써주면 됨
            // index    poi #   temp    poi #   temp    ...
            data = tickCount.ToString();
            for (int i = 0; i < imgView.CAM1_POICount; i++)
            {
                data = data + "\t\t" + imgView.CAM1_TemperatureArr[i].ToString("N2");
            }
            outputFile_Result.WriteLine(data);

            // Write OPC_READ data
            data = tickCount.ToString()+"\t";
            data = PutText_OPCRead(data);
            outputFile_OPCRead.WriteLine(data);

            // Write OPC_WRITE data
            data = tickCount.ToString()+"\t";
            data = PutText_OPCWrite(data);
            outputFile_OPCWrite.WriteLine(data);

            if (irdxHandle_write == IntPtr.Zero) // write용 irdxHandle이 없으면 새로 만들고
                DIASDAQ.DDAQ_IRDX_FILE_OPEN_WRITE(NewIRDXFileName, true, ref irdxHandle_write);
            else if (irdxHandle_write != IntPtr.Zero)   // 이미 있으면 이어붙이면 됨
                DIASDAQ.DDAQ_IRDX_FILE_ADD_IRDX(irdxHandle_write, pIRDX_Array[0]);

            // 카메라가 두대 붙어있으면 같은 로직 반복 실행
            if (pIRDX_Array[1] != IntPtr.Zero)
            {
                data = tickCount.ToString();
                for (int i = 0; i < imgView.CAM2_POICount; i++)
                {
                    data = data + "\t\t" + imgView.CAM2_TemperatureArr[i].ToString("N2");
                }
                c2_outputFile_Result.WriteLine(data);
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

            LogStart_toolStripButton.Enabled = true;
            LogStop_toolStripButton.Enabled = false;

            LoggingTimer.Stop();    // tick timer 정지 시키고
            DIASDAQ.DDAQ_IRDX_FILE_CLOSE(irdxHandle_write); // 쓰고있던 irdx파일 닫고
            irdxHandle_write = IntPtr.Zero;     // 쓰기용 irdx handle 초기화
            tickCount = 0;  // tick count 초기화

            propertyGrid1.Enabled = true;       // Propertygrid Enable
            DrawPOI_toolStripButton.Enabled = true;   // Draw POI Enable
            MovePOI_toolStripButton.Enabled = true;   // Mobe POI Enable
            DeletePOI_toolStripButton.Enabled = true;   // Delete POI Enable

            // 텍스트파일 다 썼으니까 닫자
            outputFile_Result.Close();
            Text_ResultData.Close();

            outputFile_OPCRead.Close();
            outputFile_OPCWrite.Close();
            Text_OPCReadData.Close();
            Text_OPCWriteData.Close();

            if (DetectedDevices == 2)
            {
                DIASDAQ.DDAQ_IRDX_FILE_CLOSE(c2_irdxHandle_write);
                c2_irdxHandle_write = IntPtr.Zero;
                c2_outputFile_Result.Close();
                c2_Text_ResultData.Close();
            }
        }
        #endregion


        #region ConfigurationControl
        private void LoadConfiguration()
        {
            string value = "";
            value = ConfigurationManager.AppSettings["Serial1"];
            //POSCO_CAM1_SERIAL = value;
            POSCO_SERIAL[0] = value;
            value = ConfigurationManager.AppSettings["Serial2"];
            //POSCO_CAM2_SERIAL = value;
            POSCO_SERIAL[1] = value;

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
            customGrid.RawData= value;
            value = ConfigurationManager.AppSettings["ResultData_Location"];
            customGrid.ResultData= value;

            
        }

        private void SaveConfiguration()
        {
            // Saving PropertyGrid
            config.AppSettings.Settings["Emissivity"].Value = customGrid.Emissivity.ToString();
            config.AppSettings.Settings["Transmission"].Value = customGrid.Transmission.ToString();
            config.AppSettings.Settings["AmbientTemp"].Value = customGrid.AmbientTemperature.ToString();
            config.AppSettings.Settings["Maximum"].Value = customGrid.Maximum.ToString();
            config.AppSettings.Settings["Minimum"].Value = customGrid.Minimum.ToString();
            config.AppSettings.Settings["RawData_Location"].Value = customGrid.RawData;
            config.AppSettings.Settings["ResultData_Location"].Value = customGrid.ResultData;

            // Saving POI Threshold Temperature
            string appSettingValue = "";
            for (int i = 0; i < 10; i++)
            {
                appSettingValue = "CAM1_Threshold" + (i + 1).ToString();
                config.AppSettings.Settings[appSettingValue].Value = result.CAM1_ThresholdTemp[i].ToString();
                appSettingValue = "CAM2_Threshold" + (i + 1).ToString();
                config.AppSettings.Settings[appSettingValue].Value = result.CAM2_ThresholdTemp[i].ToString();
            }

            config.AppSettings.Settings["OPC_Channel"].Value = opc.Channel;
            config.AppSettings.Settings["OPC_Device"].Value = opc.Device;
            config.AppSettings.Settings["OPC_Endpoint"].Value = opc.nodeName;

            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");
        }
        #endregion


        #region CompareMaxTempValue
        public void CompareMaxTemperature(float[] TemperatureArray)
        {
            FloatMaxTemp = 0.0f;
            string MaxTemp = "";
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
        #endregion


        #region OPCFunctions
        // Verify OPC Connection, Activity
        private void VerifyOPC()
        {
            if (opc.detected)
                result.OPCConnectAlarm.ForeColor = Color.Green;
            else
                result.OPCConnectAlarm.ForeColor = Color.Red;

            if (OPCTimerActivated && OPCActivated)
                result.OPCActiveAlarm.ForeColor = Color.Green;
            else
                result.OPCActiveAlarm.ForeColor = Color.Red;
        }

        private void OPC_DataSending(object sender, EventArgs e)
        {
            if (OPCActivated && opc.connectFailed == false)
            {
                opc.OPC_Read();
                opc.OPC_Write();
            }
            //else InitOPCTimer();
        }

        public void InitGraphTimer()
        {
            GraphUpdateTimer.Interval = 1000;
            GraphUpdateTimer.Tick += new EventHandler(GraphUpdateTimer_Tick);
            GraphUpdateTimer.Start();
        }

        private void GraphUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (mThread.IsAlive)
            {
                c1_chartView.UpdateData();
            }
            if (mThread_two.IsAlive)
            {
                c2_chartView.UpdateData();
            }
        }

        public bool OPCTimerActivated = false;
        public void InitOPCTimer()
        {
            if (OPCTimerActivated == false)
            {
                OPCTimer.Interval = 1000;
                OPCTimer.Tick += new EventHandler(OPC_DataSending);
                OPCTimerActivated = true;
                OPCTimer.Start();
            }
        }
        #endregion


        #region OpenIRDX
        private void OpenIRDX()
        {
            IRDXFileCount++;

            // OpenFileDialog setting
            openDlg.Title = "Open Simulation";
            openDlg.Filter = "IRDX Files(*.irdx)|*.irdx";
            DialogResult dr = openDlg.ShowDialog();

            // dialog에서 파일을 열었을 때
            if (dr == DialogResult.OK)
            {
                // GET FULL FILE PATH
                fileFullName = openDlg.FileName;
            }
            // 파일을 열지 않고 닫으려고 할 때
            else if (dr == DialogResult.Cancel)
            {
                return;
            }
            //MessageBox.Show(fileFullName);

            // 첫 번째 IRDX 파일을 open할때
            if (IRDXFileCount < 2)
            {
                currentOpenMode = OpenMode.IRDX;

                // IRDX handle 받아오기
                DIASDAQ.DDAQ_IRDX_FILE_OPEN_READ(fileFullName, true, ref pIRDX_Array[0]);

                ushort tempX = 0, tempY = 0;
                DIASDAQ.DDAQ_IRDX_PIXEL_GET_SIZE(pIRDX_Array[0], ref tempX, ref tempY);

                sizeX = tempX; sizeY = tempY;

                InitImageView();
                InitChart();
                InitGridView();
                InitResultView();

                label_Progress.Text = "IRDX Mode";
                groupBox_SlopeAngle.Visible = false;
                groupBox_Charging1.Visible = false;
                groupBox_Melting1.Visible = false;
                groupBox_Charging2.Visible = false;
                groupBox_Melting2.Visible = false;
                groupBox_Charging3.Visible = false;
                groupBox_Melting3.Visible = false;
                groupBox_StandSteel.Visible = false;
                groupBox_Tapping.Visible = false;
                groupBox_O2Lance.Visible = false;

                //label1.Text = "IRDX Mode";
                label1.Visible = true;

                split_ViewToInfo.Visible = true;
                panel1.Visible = true;
                propertyGrid1.Visible = true;

                textBox1.Visible = true;
                textBox2.Visible = true;
                textBox3.Visible = true;
                textBox4.Visible = true;

                toolStripSeparator3.Visible = true;
                toolStripSeparator4.Visible = true;
                toolStripSeparator5.Visible = true;
                toolStripSeparator6.Visible = true;

                PreviousRecord_toolStripButton.Visible = true;
                NextRecord_toolStripButton.Visible = true;
                KeepMoving_toolStripButton.Visible = true;
                Pause_toolStripButton.Visible = true;

                previousRecordToolStripMenuItem.Enabled = true;
                nextRecordToolStripMenuItem.Enabled = true;
                playToolStripMenuItem.Enabled = true;
                stopToolStripMenuItem1.Enabled = true;

                DrawPOI_toolStripButton.Visible = true;
                MovePOI_toolStripButton.Visible = true;
                DeletePOI_toolStripButton.Visible = true;
                drawROIToolStripMenuItem.Enabled = true;
                moveROIToolStripMenuItem.Enabled = true;
                deleteROIToolStripMenuItem.Enabled = true;

                imgView.DrawImage(pIRDX_Array[0], c1_imgView.pictureBox1);

                customGrid.GetAttributesInfo(pIRDX_Array[0]);
                propertyGrid1.Refresh();

                result.OPCConnectAlarm.ForeColor = result.NotConnected;
                result.OPCActiveAlarm.ForeColor = result.NotConnected;
            }
            else if (IRDXFileCount == 2)
            {
                // IRDX handle 받아오기
                DIASDAQ.DDAQ_IRDX_FILE_OPEN_READ(fileFullName, true, ref pIRDX_Array[1]);

                ushort tempX_2 = 0, tempY_2 = 0;
                DIASDAQ.DDAQ_IRDX_PIXEL_GET_SIZE(pIRDX_Array[1], ref tempX_2, ref tempY_2);

                sizeX_2 = tempX_2; sizeY_2 = tempY_2;

                imgView.DrawImage(pIRDX_Array[1], c2_imgView.pictureBox1);
            }
        }

        private void InitTimerForPlayer()       // IRDX Frame keepmoving timer
        {
            dataplayerTimer.Interval = 10;  // ms
            dataplayerTimer.Tick += new EventHandler(NextRecord_toolStripButton_Click);

            dataplayerTimer.Start();
            isTimerRunning = true;
        }
        #endregion


        #region OpenSimulation
        private void OpenSimulation()
        {
            // OpenFileDialog setting
            openDlg.Title = "Open Simulation";
            openDlg.Filter = "IRD Files(*.ird)|*.ird";
            DialogResult dr = openDlg.ShowDialog();

            // dialog에서 파일을 열었을 때
            if (dr == DialogResult.OK)
            {
                // GET FULL FILE PATH
                fileFullName = openDlg.FileName;
            }
            // 파일을 열지 않고 닫으려고 할 때
            else if (dr == DialogResult.Cancel)
            {
                return;
            }

            DetectedDevices = 2;

            core.GetReadyForSimulator(1, fileFullName, currentEmissivity, currentTransmittance, ref pIRDX_Array[0], ref sizeX, ref sizeY, ref min, ref max);
            core.GetReadyForSimulator(2, fileFullName, currentEmissivity, currentTransmittance, ref pIRDX_Array[1], ref sizeX_2, ref sizeY_2, ref min, ref max);

            label_Progress.Text = "Simulation Mode";
            groupBox_SlopeAngle.Visible = false;
            groupBox_Charging1.Visible = false;
            groupBox_Melting1.Visible = false;
            groupBox_Charging2.Visible = false;
            groupBox_Melting2.Visible = false;
            groupBox_Charging3.Visible = false;
            groupBox_Melting3.Visible = false;
            groupBox_StandSteel.Visible = false;
            groupBox_Tapping.Visible = false;
            groupBox_O2Lance.Visible = false;

            InitImageView();
            InitChart();
            InitGridView();
            InitResultView();

            split_ViewToInfo.Visible = true;
            panel1.Visible = true;
            propertyGrid1.Visible = true;

            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            label6.Visible = true;

            textBox1.Visible = true;
            textBox2.Visible = true;
            textBox3.Visible = true;
            textBox4.Visible = true;

            toolStripSeparator3.Visible = true;

            DrawPOI_toolStripButton.Visible = true;
            MovePOI_toolStripButton.Visible = true;
            DeletePOI_toolStripButton.Visible = true;

            panel_ScaleBar.Visible = true;

            mThread.Start();
            mThread_two.Start();

            CAM1_DataView.Start();
            CAM2_DataView.Start();

            newDevice.GetDeviceID(1, pIRDX_Array[0]);
        }
        #endregion


        #region File/Folder AutoDelete
        private short SequenceCount = 0;
        public void AutodelSequence()
        {
            string dirBase = ConfigurationManager.AppSettings["DataSaveBase"];
            DriveInfo drive = new DriveInfo(dirBase);
            long diskFreeSpace = drive.TotalFreeSpace;
            long diskTotalSpace = drive.TotalSize;

            // if (diskFreeSpace < 1073741824)      // 디스크 여유 공간이 1GB 미만일때 동작
            if(diskFreeSpace < (diskTotalSpace * 0.1))      // 디스크 여유공간이 전체 공간의 10% 미만일때 동작
            {
                if (isLoggingRunning == true && SequenceCount < 3)  // 안물어보고 우선 자동삭제
                {
                    DeviceLoggingStop();
                    Delete_OldestFolder_Raw();
                    Delete_OldestFolder_Result();
                    DeviceLoggingStart();
                    SequenceCount++;
                }
                else if(isLoggingRunning==true && SequenceCount >= 3)
                {
                    DeviceLoggingStop();
                    if (MessageBox.Show("디스크 용량이 부족합니다.\n데이터 자동 삭제 시퀀스를 시작 하시겠습니까?", "Capacity Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        Delete_OldestFolder_Raw();
                        Delete_OldestFolder_Result();
                        MessageBox.Show("데이터 삭제가 완료되었습니다.\nData Logging을 재 시작합니다.", "Capacity Clear", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        SequenceCount = 0;
                        DeviceLoggingStart();
                    }
                    else
                    {
                        MessageBox.Show("디스크 용량이 부족하여 Data Logging이 중단 되었습니다.\n디스크의 여유 공간을 확보 후 재시도 하십시오.", "Capacity Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        SequenceCount = 0;
                    }
                }
            }

        }

        private void Delete_OldestFolder_Raw()
        {
            string oldest_RawDir = customGrid.RawData.ToString();
            oldest_RawDir = oldest_RawDir + "RawData";
            core.DeleteOldestFolder(oldest_RawDir);
        }

        private void Delete_OldestFolder_Result()
        {
            string oldest_ResultDir = customGrid.RawData.ToString();
            oldest_ResultDir = oldest_ResultDir + "ResultData";
            core.DeleteOldestFolder(oldest_ResultDir);
        }
        #endregion
             
        private void OpenNewDevice()
        {
            if (newDevice.isConnectedDevices == true)
            {
                MessageBox.Show("이미 탐지 되었거나 동작 중인 카메라가 있습니다.", "Detection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            newDevice.DeviceDetection();
            if (newDevice.isDetected == false) return;
            else
            {
                Cursor.Current = Cursors.WaitCursor;
                newDevice.ReadyToRun();
                Cursor.Current = Cursors.Default;

                newDevice.ShowDialog();
            }
        }

        private void OpenAbout()
        {
            about.ShowDialog();
        }


    }

    /*public partial class ToolStripMenuItem : ToolStripControlHost
    {
        public ToolStripMenuItem(): base(CreateControlInstance())
        {
        }

        public TrackBar TrackBar
        {
            get
            {
                return Control as TrackBar;
            }
        }

        private static Control CreateControlInstance()
        {
            TrackBar t = new TrackBar();
            t.AutoSize = false;
            return t;
        }

        [DefaultValue(0)]
        public int Value
        {
            get { return TrackBar.Value; }
            set { TrackBar.Value = value; }
        }

        protected override void OnSubscribeControlEvents(Control control)
        {
            base.OnSubscribeControlEvents(control);
            TrackBar trackBar = control as TrackBar;
            TrackBar.ValueChanged += new EventHandler(trackBar_ValueChanged);
        }

        protected override void OnUnsubscribeControlEvents(Control control)
        {
            base.OnUnsubscribeControlEvents(control);
            TrackBar trackBar = control as TrackBar;
            TrackBar.ValueChanged -= new EventHandler(trackBar_ValueChanged);
        }

        void trackBar_ValueChanged(object sender, EventArgs e)
        {
            if(this.ValueChanged != null)
            {
                ValueChanged(sender, e);
            }
        }

        public event EventHandler ValueChanged;

        protected override Size DefaultSize
        {
            get
            {
                return new Size(200, 16);
            }
        }
    }*/


}
