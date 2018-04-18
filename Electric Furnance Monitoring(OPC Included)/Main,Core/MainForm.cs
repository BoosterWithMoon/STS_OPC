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
        public bool isLoggingRunning;
        System.Windows.Forms.Timer dataplayerTimer;

        public System.Windows.Forms.Timer OPCTimer;
        public bool OPCActivated;

        private uint position;
        private uint numDataSet;

        bool isTimerRunning;

        public int IRDXFileCount;

        //private static ushort DDAQ_MOTORFOCUS_CMD_EXIST = 0;    //< check if available
        //private static ushort DDAQ_MOTORFOCUS_CMD_STOP = 1;     //< stop any motion
        //private static ushort DDAQ_MOTORFOCUS_CMD_NEAR = 2;     //< move focus to near
        //private static ushort DDAQ_MOTORFOCUS_CMD_FAR = 3;      //< move focus to far
        private static ushort DDAQ_MOTORFOCUS_CMD_NEAR_STEP = 4; //< move focus one step to near
        private static ushort DDAQ_MOTORFOCUS_CMD_FAR_STEP = 5; //< move focus one step to far
        //private static ushort DDAQ_MOTORFOCUS_CMD_NEAR_STEP_BIG = 6; //< move focus one big step to near
        //private static ushort DDAQ_MOTORFOCUS_CMD_FAR_STEP_BIG = 7;   //< move focus one big step to far

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
        FileStream Text_RawData;
        FileStream Text_ResultData;
        StreamWriter outputFile;
        StreamWriter outputFile_Result;

        FileStream c2_Text_RawData;
        FileStream c2_Text_ResultData;
        StreamWriter c2_outputFile;
        StreamWriter c2_outputFile_Result;

        IntPtr irdxHandle_write;
        IntPtr c2_irdxHandle_write;
        int tickCount;

        System.Configuration.Configuration config;

        public string POSCO_CAM1_SERIAL;
        public string POSCO_CAM2_SERIAL;

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
            Scale_MaxTemp = 0;
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

            propertyGrid1.Visible = false;

            ViewAdjust();

            opc.ServerDetection();
            opc.ServerConnection();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfiguration();

            if (OPCActivated)
            {
                OPCTimer.Stop();
                OPCActivated = false;
            }

            opc.ServerDisconnection();

            isClosing = true;

            Thread.Sleep(3);

            // 스레드 둘중에 하나라도 돌고있으면 먼저 둘다 죽이고 시작하자
            if (mThread.IsAlive || mThread_two.IsAlive || CAM1_DataView.IsAlive || CAM2_DataView.IsAlive/* || propertyGridUpdate.IsAlive*/)
            {
                mThread.Abort();
                mThread_two.Abort();
                CAM1_DataView.Abort();
                CAM2_DataView.Abort();
                //propertyGridUpdate.Abort();
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
            //split_ViewToInfo.SplitterDistance = 1920 - propertyGrid1.Width - 310;
            //split_ViewToInfo.SplitterDistance = Screen.PrimaryScreen.Bounds.Width - propertyGrid1.Width - 310;
            split_ViewToInfo.SplitterDistance = this.Width - propertyGrid1.Width - 310;
            
            // 카메라별 ImageView 영역 Width 조정
            split_CamToCam.Width = split_ViewToInfo.Panel1.Width / 2;

            // CAM #1 영역 Height 조정
            split_CAM1Info.SplitterDistance = 140;
            split_CAM1ChartGrid.SplitterDistance = 265;

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

                if (DIASDAQ.DDAQ_DEVICE_GET_NEWDATAREADY(DetectedDevices, ref newDataReady) != DIASDAQ.DDAQ_ERROR.NO_ERROR)/// 카메라가 새로운 데이터를 받을 준비가 되었을 시
                {
                    DIASDAQ.DDAQ_DEVICE_DO_STOP(DetectedDevices);
                    return;
                }

                if (newDataReady && !isClosing)
                {
                    if (DIASDAQ.DDAQ_DEVICE_DO_UPDATEDATA(DetectedDevices) != DIASDAQ.DDAQ_ERROR.NO_ERROR)                 /// 새로운 데이터가 있으며 종료의 명령이 없을 시
                        return;                                                                                                /// UpDate Data !!

                    float fTemp = 0.0f;
                    bool bTemp = false;
                    DIASDAQ.DDAQ_DEVICE_GET_CAMERATEMP(DetectedDevices, ref fTemp, ref bTemp);
                    CAM1_CameraTemp.Text = fTemp.ToString("N1") + "℃";

                    DIASDAQ.DDAQ_DEVICE_GET_DETECTORTEMP(DetectedDevices, ref fTemp, ref bTemp);
                    CAM1_DetectorTemp.Text = fTemp.ToString("N1") + "℃";

                    //imgView.CalculateCurrentTemp(pIRDX_Array[0], imgView.CAM1_POICount, imgView.CAM1_ClickedPosition, imgView.CAM1_TemperatureArr);
                    imgView.DrawImage(pIRDX_Array[0], c1_imgView.pictureBox1);
                    //if (img != null) img.Dispose();                                                                            /// 메모리 관리를 위하여 Dispose.
                    
                    acq_index++;
                    CompareMaxTemperature(imgView.CAM1_TemperatureArr);
                    result.CAM1_DetectTemp_ForOPC();

                    VerifyOPC();
                    //propertyGrid1.Invalidate();
                    //customGrid.UpdateDataSet();
                    //propertyGrid1.Refresh();
                    isDrawnCAM1Image = true;

                    if (DIASDAQ.DDAQ_DEVICE_DO_ENABLE_NEXTMSG(DetectedDevices) != DIASDAQ.DDAQ_ERROR.NO_ERROR)             /// 카메라가 새로운 데이터를 받을 수 있도록 Do Enable
                        return;

                    //ThreadOne_WFSO.Set();
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

                    //if (img != null) img.Dispose();                                                                 /// 메모리 관리를 위하여 Dispose.

                    float fTemp = 0.0f;
                    bool bTemp = false;
                    DIASDAQ.DDAQ_DEVICE_GET_CAMERATEMP(1, ref fTemp, ref bTemp);
                    CAM2_CameraTemp.Text = fTemp.ToString("N1") + "℃";

                    DIASDAQ.DDAQ_DEVICE_GET_DETECTORTEMP(1, ref fTemp, ref bTemp);
                    CAM2_DetectorTemp.Text = fTemp.ToString("N1") + "℃";


                    //imgView.CalculateCurrentTemp(pIRDX_Array[1], imgView.CAM2_POICount, imgView.CAM2_ClickedPosition, imgView.CAM2_TemperatureArr);
                    //imgView.CAM2_DrawImage(pIRDX_Array[1], c2_imgView.pictureBox1, imgView.CAM2_ClickedPosition, imgView.CAM2_POICount);
                    imgView.CAM2_DrawImage(pIRDX_Array[1], c2_imgView.pictureBox1);

                    CAM2_CompareMaxTemperature(imgView.CAM2_TemperatureArr);
                    result.CAM2_DetectTemp_ForOPC();
                    //VerifyOPC();

                    isDrawnCAM2Image = true;

                    if (DIASDAQ.DDAQ_DEVICE_DO_ENABLE_NEXTMSG(1) != DIASDAQ.DDAQ_ERROR.NO_ERROR)                    /// 카메라가 새로운 데이터를 받을 수 있도록 Do Enable
                        return;

                    //ThreadTwo_WFSO.Set();
                }

            }
        }

        private void CAM1_AllView()
        {
            while (true)
            {
                Thread.Sleep(10);
                //ThreadOne_WFSO.WaitOne();

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
                //ThreadTwo_WFSO.WaitOne();

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

            //imgView.DrawImage(pIRDX_Array[0], c2_imgView.pictureBox1);
            imgView.DrawImage(pIRDX_Array[0], c1_imgView.pictureBox1);

            customGrid.GetAttributesInfo(pIRDX_Array[0]);
        }

        private void NextRecord_toolStripButton_Click(object sender, EventArgs e)
        {
            DIASDAQ.DDAQ_IRDX_FILE_GET_NUMDATASETS(pIRDX_Array[0], ref numDataSet);

            DIASDAQ.DDAQ_IRDX_FILE_GET_CURDATASET(pIRDX_Array[0], ref position);
            //MessageBox.Show(position.ToString());

            if (position + 1 == numDataSet)
            {
                // position이 DataSet의 끝까지 갔다면 제일 첫 프레임으로 보냄
                position = 0;
                dataplayerTimer.Stop();

                float pData = 0;
                uint bufferSize = 0;
                DIASDAQ.DDAQ_IRDX_PIXEL_GET_DATA(pIRDX_Array[0], ref pData, bufferSize);

                //imgView.DrawImage(pIRDX_Array[0], c2_imgView.pictureBox1);
                imgView.DrawImage(pIRDX_Array[0], c1_imgView.pictureBox1);

                imgView.CalculateCurrentTemp(pIRDX_Array[0], imgView.CAM1_POICount, imgView.CAM1_ClickedPosition, imgView.CAM1_TemperatureArr);
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

                //imgView.DrawImage(pIRDX_Array[0], c2_imgView.pictureBox1);
                imgView.DrawImage(pIRDX_Array[0], c1_imgView.pictureBox1);

                imgView.CalculateCurrentTemp(pIRDX_Array[0], imgView.CAM1_POICount, imgView.CAM1_ClickedPosition, imgView.CAM1_TemperatureArr);
                c1_chartView.UpdateData();
                c1_gridView.RefreshGrid();
                result.CAM1_DetectTempThreshold();

                for (int i = 0; i < imgView.CAM1_POICount; i++)
                {
                    //imageView.CAM1_ClickedPosition[i] = imageView.CAM2_ClickedPosition[i];
                    imgView.CAM2_ClickedPosition[i] = imgView.CAM1_ClickedPosition[i];
                }
                //imgView.CAM2_POICount = imgView.CAM1_POICount;
                //imgView.CalculateCurrentTemp(pIRDX_Array[0], imgView.CAM2_POICount, imgView.CAM2_ClickedPosition, imgView.CAM2_TemperatureArr);
                //c2_chartView.UpdateData();
                //c2_gridView.CAM2_RefreshGrid();
                //result.CAM2_DetectTempThreshold();

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
                newRawDataFolderName = customGrid.RawData_Location + "\\RawData";
                Directory.CreateDirectory(newRawDataFolderName);
                newRawDataFolderName = newRawDataFolderName + "\\" + currentTime_DateOnly;
                Directory.CreateDirectory(newRawDataFolderName);
            }
            if (isResultFolderExist)
            {
                newResultDataFolderName = customGrid.ResultData_Location + "\\ResultData";
                Directory.CreateDirectory(newResultDataFolderName);
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
                    strFileName = strFileName + currentTime + ".txt";
                    strFileName = newRawDataFolderName + "\\" + strFileName;
                    break;
                case 2:
                    strFileName = strFileName + currentTime + ".txt";
                    strFileName = newResultDataFolderName + "\\" + strFileName;
                    break;
            }

            return strFileName;
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


        private void T_Tick(object sender, EventArgs e)
        {
            //string test = "TEXT WRITING TEST: RawData\n";
            string test2 = "";

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

        private void LoadConfiguration()
        {
            string value = "";
            value = ConfigurationManager.AppSettings["Serial1"];
            POSCO_CAM1_SERIAL = value;
            value = ConfigurationManager.AppSettings["Serial2"];
            POSCO_CAM2_SERIAL = value;

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

            config.AppSettings.Settings["OPC_Channel"].Value = opc.Channel;
            config.AppSettings.Settings["OPC_Device"].Value = opc.Device;
            config.AppSettings.Settings["OPC_Endpoint"].Value = opc.nodeName;

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
        #endregion


        #region CompareMaxTempValue
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
            else
            {
                InitOPCTimer();
            }

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

                label1.Text = "IRDX Mode";
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

                LogStart_toolStripButton.Visible = true;
                LogStop_toolStripButton.Visible = true;

                imgView.DrawImage(pIRDX_Array[0], c1_imgView.pictureBox1);

                customGrid.GetAttributesInfo(pIRDX_Array[0]);
                propertyGrid1.Refresh();

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

            DIASDAQ.DDAQ_DEVICE_DO_OPENSIMULATION(1, fileFullName);     // DevNo1: 512N
            DIASDAQ.DDAQ_DEVICE_DO_OPENSIMULATION(2, fileFullName);     // DevNo2: 320L

            // IRDX handle 받아오기
            DIASDAQ.DDAQ_DEVICE_GET_IRDX(1, ref pIRDX_Array[0]); // 512
            DIASDAQ.DDAQ_DEVICE_GET_IRDX(2, ref pIRDX_Array[1]); // 320

            if (DIASDAQ.DDAQ_IRDX_PIXEL_GET_SIZE(pIRDX_Array[0], ref sizeX, ref sizeY) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
                return;
            if (DIASDAQ.DDAQ_IRDX_PIXEL_GET_SIZE(pIRDX_Array[1], ref sizeX_2, ref sizeY_2) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
                return;

            DIASDAQ.COLORREF color = new DIASDAQ.COLORREF();
            color.Red = 0; color.Blue = 0; color.Green = 0;
            //-----------------------------------------
            DIASDAQ.DDAQ_SET_TEMPPRECISION(0);
            ushort avg = 0;
            DIASDAQ.DDAQ_IRDX_OBJECT_SET_EMISSIVITY(pIRDX_Array[0], currentEmissivity);                          /// 두번째 카메라 핵심 프로퍼티 설정
            DIASDAQ.DDAQ_IRDX_OBJECT_SET_TRANSMISSION(pIRDX_Array[0], currentTransmittance);
            DIASDAQ.DDAQ_IRDX_PALLET_SET_BAR(pIRDX_Array[0], 0, 256);
            DIASDAQ.DDAQ_IRDX_SCALE_GET_MINMAX(pIRDX_Array[0], ref min, ref max);
            DIASDAQ.DDAQ_IRDX_SCALE_SET_MINMAX(pIRDX_Array[0], min, max);
            DIASDAQ.DDAQ_IRDX_ACQUISITION_GET_AVERAGING(pIRDX_Array[0], ref avg);

            DIASDAQ.DDAQ_IRDX_OBJECT_SET_EMISSIVITY(pIRDX_Array[1], currentEmissivity);                          /// 두번째 카메라 핵심 프로퍼티 설정
            DIASDAQ.DDAQ_IRDX_OBJECT_SET_TRANSMISSION(pIRDX_Array[1], currentTransmittance);
            DIASDAQ.DDAQ_IRDX_PALLET_SET_BAR(pIRDX_Array[1], 0, 256);
            DIASDAQ.DDAQ_IRDX_SCALE_GET_MINMAX(pIRDX_Array[1], ref min, ref max);
            DIASDAQ.DDAQ_IRDX_SCALE_SET_MINMAX(pIRDX_Array[1], min, max);
            DIASDAQ.DDAQ_IRDX_ACQUISITION_GET_AVERAGING(pIRDX_Array[1], ref avg);

            uint nThreadID = (uint)Thread.CurrentThread.ManagedThreadId;                        /// 기본 Thread ID Value get
            uint nThreadID2 = (uint)Thread.CurrentThread.ManagedThreadId;

            // THROW THREADS ID
            if (DIASDAQ.DDAQ_DEVICE_SET_MSGTHREAD(1, nThreadID) != DIASDAQ.DDAQ_ERROR.NO_ERROR)   /// 스레드 ID 등록
                return;
            if (DIASDAQ.DDAQ_DEVICE_SET_MSGTHREAD(2, nThreadID2) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
                return;

            // SET ACQUISITION FREQUENCY
            if (DIASDAQ.DDAQ_IRDX_ACQUISITION_SET_AVERAGING(pIRDX_Array[0], 2) != DIASDAQ.DDAQ_ERROR.NO_ERROR)               /// Default ACQ_Frequency 8 으로 설정
                return;
            if (DIASDAQ.DDAQ_IRDX_ACQUISITION_SET_AVERAGING(pIRDX_Array[1], 2) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
                return;

            if (DIASDAQ.DDAQ_DEVICE_DO_START(1) != DIASDAQ.DDAQ_ERROR.NO_ERROR)                   /// 각 카메라 Do Start!!
                return;
            if (DIASDAQ.DDAQ_DEVICE_DO_START(2) != DIASDAQ.DDAQ_ERROR.NO_ERROR) return;

            DIASDAQ.DDAQ_DEVICE_DO_ENABLE_NEXTMSG(1);
            DIASDAQ.DDAQ_DEVICE_DO_ENABLE_NEXTMSG(2);

            //customGrid.GetAttributesInfo();
            //propertyGrid1.Refresh();

            InitImageView();
            InitChart();
            InitGridView();

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

            customGrid.GetAttributesInfo(pIRDX_Array[0]);

            mThread.Start();
            mThread_two.Start();

            InitResultView();

            OPCActivated = true;
            InitOPCTimer();
        }
        #endregion


        // IRDX Frame keepmoving timer
        private void InitTimerForPlayer()
        {
            dataplayerTimer.Interval = 10;  // ms
            dataplayerTimer.Tick += new EventHandler(NextRecord_toolStripButton_Click);

            dataplayerTimer.Start();
            isTimerRunning = true;
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

        private void OpenAbout()
        {
            about.ShowDialog();
        }
    }
}
