using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Electric_Furnance_Monitoring_OPC_Included_
{
    public partial class NewDeviceForm : Form
    {
        MainForm main;
        SystemPropertyGrid grid;
        //CustomOPC opc;
        STS.Core.CoreLibrary core;

        Thread thr1, thr2;
        Thread support_thr1;
        Thread support_thr2;
        public bool isDetected = false;
        public bool isConnectedDevices = false;
        public string[] DeviceID;
        uint NDF_DetectedDevices;           // CS1690 Warning
        private string[] DetectedSerialNo;

        public NewDeviceForm(MainForm _main)
        {
            this.main = _main;

            InitializeComponent();

            core = new STS.Core.CoreLibrary();
            isDetected = false;
            isConnectedDevices = false;
            DetectedSerialNo = new string[2];
        }

        private void NewDeviceForm_Load(object sender, EventArgs e)
        {

        }

        private void NewDeviceForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();   // 그냥 닫음
            }
            else if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }

        public void DeviceDetection()
        {
            Cursor.Current = Cursors.WaitCursor;
            NDF_DetectedDevices = DIASDAQ.DDAQ_DEVICE_DO_DETECTION();

            if (NDF_DetectedDevices == 0)
            {
                Cursor.Current = Cursors.Default;
                isDetected = false;
                MessageBox.Show("새 연결을 만들 카메라가 감지되지 않습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                MessageBox.Show(NDF_DetectedDevices.ToString() + "개의 카메라가 감지 되었습니다.", "New Device Connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                isDetected = true;
            }
        }

        public void GetDeviceType(uint DeviceNo, IntPtr irdxHandle)
        {
            StringBuilder s = new StringBuilder(20);
            uint type = 0;
            DIASDAQ.DDAQ_DEVICE_TYPE id = 0;

            DIASDAQ.DDAQ_DEVICE_GET_ID(DeviceNo, ref id, ref type);
            string s2 = "";
            switch (type)
            {
                case (uint)DIASDAQ.DDAQ_DEVICE_TYPE.VIEW_100:
                    s2 = DIASDAQ.DDAQ_DEVICE_TYPE.VIEW_100.ToString();
                    break;
                case (uint)DIASDAQ.DDAQ_DEVICE_TYPE.VIEW_256:
                    s2 = DIASDAQ.DDAQ_DEVICE_TYPE.VIEW_256.ToString();
                    break;
                case (uint)DIASDAQ.DDAQ_DEVICE_TYPE.VIEW_320:
                    s2 = DIASDAQ.DDAQ_DEVICE_TYPE.VIEW_320.ToString();
                    break;
                case (uint)DIASDAQ.DDAQ_DEVICE_TYPE.MIDAS:
                    s2 = DIASDAQ.DDAQ_DEVICE_TYPE.MIDAS.ToString();
                    break;
                case (uint)DIASDAQ.DDAQ_DEVICE_TYPE.MODULE_128:
                    s2 = DIASDAQ.DDAQ_DEVICE_TYPE.MODULE_128.ToString();
                    break;
                case (uint)DIASDAQ.DDAQ_DEVICE_TYPE.LINE_128:
                    s2 = DIASDAQ.DDAQ_DEVICE_TYPE.LINE_128.ToString();
                    break;
                case (uint)DIASDAQ.DDAQ_DEVICE_TYPE.LINE_256:
                    s2 = DIASDAQ.DDAQ_DEVICE_TYPE.LINE_256.ToString();
                    break;
                case (uint)DIASDAQ.DDAQ_DEVICE_TYPE.HZK_160:
                    s2 = DIASDAQ.DDAQ_DEVICE_TYPE.HZK_160.ToString();
                    break;
                case (uint)DIASDAQ.DDAQ_DEVICE_TYPE.HZK_256:
                    s2 = DIASDAQ.DDAQ_DEVICE_TYPE.HZK_256.ToString();
                    break;
                case 52:
                    s2 = "VIEW_512";
                    break;
                default:
                    s2 = DIASDAQ.DDAQ_DEVICE_TYPE.NO.ToString();
                    break;
            }
            s.Clear();
            s.Append("PYRO" + s2);

            if (irdxHandle == main.pIRDX_Array[0])
                main.CAM1_Type.Text = s.ToString();
            else
                main.CAM2_Type.Text = s.ToString();
        }

        public void GetDeviceID(uint DeviceNo, IntPtr irdxHandle)
        {
            StringBuilder s = new StringBuilder(64);
            byte[] tempDeviceID = new byte[64];
            char[] ch_tempDeviceID = new char[64];

            s.Clear();
            DIASDAQ.DDAQ_DEVICE_GET_IDSTRING(DeviceNo, tempDeviceID, 64);
            for (int i = 0; i < 64; i++)
            {
                ch_tempDeviceID[i] = (char)tempDeviceID[i];
                string temp = ch_tempDeviceID[i].ToString();
                s.Append(temp);
            }
            string result = s.ToString();
            int serialTemp = result.IndexOf("C");
            if (irdxHandle == main.pIRDX_Array[0])
            {
                DetectedSerialNo[0] = result.Substring(serialTemp, 8);
                main.CAM1_Serial.Text = DetectedSerialNo[0].ToString();
            }
            else
            {
                DetectedSerialNo[1] = result.Substring(serialTemp, 8);
                main.CAM2_Serial.Text = DetectedSerialNo[1].ToString();
            }
        }

        public void ReadyToRun()
        {
            main.DetectedDevices = NDF_DetectedDevices;

            DeviceID = new string[NDF_DetectedDevices];

            byte[] tempDeviceID = new byte[64];
            char[] ch_tempDeviceID = new char[64];
            StringBuilder result = new StringBuilder(45);

            // CAMERA #1 (320L)
            //if(DIASDAQ.DDAQ_DEVICE_DO_OPEN(NDF_DetectedDevices, null) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
            //{
            //    return;
            //}
            DIASDAQ.DDAQ_DEVICE_DO_OPEN(NDF_DetectedDevices, null);
            DIASDAQ.DDAQ_DEVICE_GET_IRDX(NDF_DetectedDevices, ref main.pIRDX_Array[0]);

            GetDeviceType(NDF_DetectedDevices, main.pIRDX_Array[0]);

            DIASDAQ.DDAQ_DEVICE_GET_IDSTRING(NDF_DetectedDevices, tempDeviceID, 64);
            for (int i = 0; i < 64; i++)
            {
                ch_tempDeviceID[i] = (char)tempDeviceID[i];
                string tmp = ch_tempDeviceID[i].ToString();
                result.Append(tmp);
            }
            string forComboBox = result.ToString();
            DeviceID[0] = forComboBox;
            comboBox1.Items.Add(DeviceID[0]);
            comboBox1.SelectedIndex = 0;

            GetDeviceID(NDF_DetectedDevices, main.pIRDX_Array[0]);

            // CAMERA #2 (512N)
            if (NDF_DetectedDevices == 1) return;

            result.Clear();

            DIASDAQ.DDAQ_DEVICE_DO_OPEN(1, null);
            DIASDAQ.DDAQ_DEVICE_GET_IRDX(1, ref main.pIRDX_Array[1]);

            GetDeviceType(1, main.pIRDX_Array[1]);

            DIASDAQ.DDAQ_DEVICE_GET_IDSTRING(1, tempDeviceID, 64);
            for (int i = 0; i < 64; i++)
            {
                ch_tempDeviceID[i] = (char)tempDeviceID[i];
                string tmp = ch_tempDeviceID[i].ToString();
                result.Append(tmp);
            }
            forComboBox = result.ToString();
            DeviceID[1] = forComboBox;
            comboBox2.Items.Add(DeviceID[1]);
            comboBox2.SelectedIndex = 0;

            GetDeviceID(1, main.pIRDX_Array[1]);
        }

        /*private bool VerifyingCamSerial()
        {
            // XML Configuration 정보 검증
            if (main.POSCO_CAM1_SERIAL == "" || main.POSCO_CAM2_SERIAL == "")
            {
                ErrorCode = "Cannot find the serial number in configuration.";
                ErrorLine = 219;
                DIASDAQ.DDAQ_DEVICE_DO_STOP(1);
                DIASDAQ.DDAQ_DEVICE_DO_CLOSE(1);
                DIASDAQ.DDAQ_DEVICE_DO_STOP(NDF_DetectedDevices);
                DIASDAQ.DDAQ_DEVICE_DO_CLOSE(NDF_DetectedDevices);
                main.DetectedDevices = 0;
                NDF_DetectedDevices = 0;
                main.pIRDX_Array[0] = IntPtr.Zero;
                main.pIRDX_Array[1] = IntPtr.Zero;
                return false;
            }
            else if (NDF_DetectedDevices == 2 &&
                (CAM1_SerialNo != main.POSCO_CAM1_SERIAL && CAM1_SerialNo != main.POSCO_CAM2_SERIAL) ||
                (CAM2_SerialNo != main.POSCO_CAM1_SERIAL && CAM2_SerialNo != main.POSCO_CAM2_SERIAL)
                )
            {
                ErrorCode = "Serial number comparison failed.";
                ErrorLine = 233;
                DIASDAQ.DDAQ_DEVICE_DO_STOP(1);
                DIASDAQ.DDAQ_DEVICE_DO_CLOSE(1);
                DIASDAQ.DDAQ_DEVICE_DO_STOP(NDF_DetectedDevices);
                DIASDAQ.DDAQ_DEVICE_DO_CLOSE(NDF_DetectedDevices);
                main.DetectedDevices = 0;
                NDF_DetectedDevices = 0;
                main.pIRDX_Array[0] = IntPtr.Zero;
                main.pIRDX_Array[1] = IntPtr.Zero;
                return false;
            }
            else if (NDF_DetectedDevices == 1 &&
                (CAM1_SerialNo != main.POSCO_CAM1_SERIAL) || (CAM1_SerialNo != main.POSCO_CAM2_SERIAL))
            {
                ErrorCode = "Serial number comparison failed.";
                ErrorLine = 250;
                DIASDAQ.DDAQ_DEVICE_DO_STOP(NDF_DetectedDevices);
                DIASDAQ.DDAQ_DEVICE_DO_CLOSE(NDF_DetectedDevices);
                main.DetectedDevices = 0;
                NDF_DetectedDevices = 0;
                main.pIRDX_Array[0] = IntPtr.Zero;
                return false;
            }
            return true;
        }*/

        // Accept Button
        private void button1_Click(object sender, EventArgs e)
        {
            object[] received = core.VerifyingCamSerial(main.pIRDX_Array, main.POSCO_SERIAL, NDF_DetectedDevices, DetectedSerialNo);

            if (Convert.ToBoolean(received[1]) == false)
            {
                MessageBox.Show("프로그램을 시작할 수 없습니다.\n\nReason: " + received[0].ToString(), "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                main.DetectedDevices = 0;
                NDF_DetectedDevices = 0;
                Close();
                return;
            }

            for (uint i = 0; i < NDF_DetectedDevices; i++)
            {
                DIASDAQ.DDAQ_IRDX_PIXEL_GET_SIZE(main.pIRDX_Array[i], ref main.sizeX_Array[i], ref main.sizeY_Array[i]);

                DIASDAQ.COLORREF color = new DIASDAQ.COLORREF();
                color.Red = 0; color.Green = 0; color.Blue = 0;

                DIASDAQ.DDAQ_SET_TEMPPRECISION(1);      // scaling bar에서 max,min 온도 사이 값 표현 편차
                ushort avg = 0;
                float min = 0, max = 0;
                DIASDAQ.DDAQ_IRDX_OBJECT_SET_EMISSIVITY(main.pIRDX_Array[i], main.currentEmissivity);
                DIASDAQ.DDAQ_IRDX_OBJECT_SET_TRANSMISSION(main.pIRDX_Array[i], main.currentTransmittance);
                DIASDAQ.DDAQ_IRDX_PALLET_SET_BAR(main.pIRDX_Array[i], DIASDAQ.DDAQ_PALLET.HOTMETAL, 256);
                DIASDAQ.DDAQ_IRDX_SCALE_GET_MINMAX(main.pIRDX_Array[i], ref min, ref max);
                DIASDAQ.DDAQ_IRDX_SCALE_SET_MINMAX(main.pIRDX_Array[i], min, max);
                DIASDAQ.DDAQ_IRDX_ACQUISITION_GET_AVERAGING(main.pIRDX_Array[i], ref avg);
            }

            ///Thread ID Registering
            uint nThreadID = (uint)Thread.CurrentThread.ManagedThreadId;        /// Get Thread ID Value
            core.RegisterThread(main.pIRDX_Array[0], NDF_DetectedDevices, nThreadID);

            if (NDF_DetectedDevices != 1)
            {
                uint nThreadID_two = (uint)Thread.CurrentThread.ManagedThreadId;
                core.RegisterThread(main.pIRDX_Array[1], 1, nThreadID_two);
            }

            main.currentOpenMode = MainForm.OpenMode.Online;

            grid = (SystemPropertyGrid)main.customGrid_forPublicRef();
            grid.GetAttributesInfo(main.pIRDX_Array[0]);
            main.propertyGrid1.Refresh();

            main.InitImageView();
            main.InitChart();
            main.InitGridView();
            main.InitResultView();

            main.split_ViewToInfo.Visible = true;

            main.label1.Visible = true;
            main.label3.Visible = true;
            main.label5.Visible = true;
            main.textBox1.Visible = true;
            main.textBox3.Visible = true;

            main.DrawPOI_toolStripButton.Visible = true;   // Draw POI
            main.drawROIToolStripMenuItem.Enabled = true;
            main.DeletePOI_toolStripButton.Visible = true;   // Delete POI
            main.deleteROIToolStripMenuItem.Enabled = true;
            main.MovePOI_toolStripButton.Visible = true;   // Move POI
            main.moveROIToolStripMenuItem.Enabled = true;

            main.LogStart_toolStripButton.Visible = true;
            main.LogStop_toolStripButton.Visible = true;
            main.LogStop_toolStripButton.Enabled = false;
            main.startToolStripMenuItem.Enabled = true;

            main.MoveFocus_FarStep.Visible = true;
            main.MoveFocus_NearStep.Visible = true;
            main.moveToFarStepToolStripMenuItem.Enabled = true;
            main.moveToNearStepToolStripMenuItem.Enabled = true;

            main.dataRecordToolStripMenuItem.Visible = false;

            main.OPCSettingToolStripMenuItem.Enabled = true;

            main.toolStripSeparator3.Visible = true;
            main.toolStripSeparator4.Visible = true;
            main.toolStripSeparator5.Visible = true;
            main.toolStripSeparator6.Visible = true;

            main.panel1.Visible = true;
            main.panel_ScaleBar.Visible = true;

            main.propertyGrid1.Visible = true;

            thr1 = (Thread)main.Thread1_forPublicRef();
            thr1.Start();

            support_thr1 = (Thread)main.Support_Thread1_forPublicRef();
            support_thr1.Start();

            isConnectedDevices = true;

            if (NDF_DetectedDevices == 2)
            {
                main.label2.Visible = true;
                main.label4.Visible = true;
                main.label6.Visible = true;
                main.textBox2.Visible = true;
                main.textBox4.Visible = true;
                thr2 = (Thread)main.Thread2_forPublicRef();
                thr2.Start();

                support_thr2 = (Thread)main.Support_Thread2_forPublicRef();
                support_thr2.Start();
            }

            LoadPOIData();

            ImageView imgView = (ImageView)main.ImageView_forPublicRef();
            imgView.CAM1_POICheckFlag = true;
            imgView.CAM2_POICheckFlag = true;
            main.InitGraphTimer();

            this.Close();
        }

        // Cancel
        private void button2_Click(object sender, EventArgs e)
        {
            if (isDetected) isDetected = false;
            DIASDAQ.DDAQ_DEVICE_DO_CLOSE(1);
            DIASDAQ.DDAQ_DEVICE_DO_CLOSE(2);
            Close();
        }

        private void LoadPOIData()
        {
            ImageView imgView = (ImageView)main.ImageView_forPublicRef();

            string[] arrX = System.Configuration.ConfigurationManager.AppSettings["CAM1_pointX"].Split(',');
            string[] arrY = System.Configuration.ConfigurationManager.AppSettings["CAM1_pointY"].Split(',');
            int pointCount=0;
            for (int i = 0; i < arrX.Count(); i++)
            {
                if (Convert.ToInt32(arrX[i]) != 0)
                {
                    pointCount++;
                }
            }
            imgView.CAM1_POICount = pointCount;

            for (int i = 0; i < imgView.CAM1_POICount; i++)
            {
                imgView.CAM1_ClickedPosition[i].X = Convert.ToInt32(arrX[i]);
                imgView.CAM1_ClickedPosition[i].Y = Convert.ToInt32(arrY[i]);
            }

            if (NDF_DetectedDevices == 2)
            {
                pointCount = 0;
                arrX = System.Configuration.ConfigurationManager.AppSettings["CAM2_pointX"].Split(',');
                arrY = System.Configuration.ConfigurationManager.AppSettings["CAM2_pointY"].Split(',');
                for(int i=0; i<arrX.Count(); i++)
                {
                    if (Convert.ToInt32(arrX[i])!=0)
                    {
                        pointCount++;
                    }
                }
                imgView.CAM2_POICount = pointCount;
                for(int i=0; i<imgView.CAM2_POICount; i++)
                {
                    imgView.CAM2_ClickedPosition[i].X = Convert.ToInt32(arrX[i]);
                    imgView.CAM2_ClickedPosition[i].Y = Convert.ToInt32(arrY[i]);
                }
            }
        }
    }
}
