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
        CustomOPC opc;

        Thread thr1, thr2;
        Thread support_thr1;
        Thread support_thr2;

        public bool isDetected = true;
        public string[] DeviceID;
        uint NDF_DetectedDevices;           // CS1690 Warning

        private string CAM1_SerialNo = "";
        private string CAM2_SerialNo = "";

        public NewDeviceForm(MainForm _main)
        {
            this.main = _main;
            InitializeComponent();
        }

        private void NewDeviceForm_Load(object sender, EventArgs e)
        {

        }

        private void NewDeviceForm_KeyDown(object sender, KeyEventArgs e)
        {
            //esc키를 누르면
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();   // 그냥 닫음
            }
            // enter키를 누르면...
            else if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }

        public void DeviceDetection()
        {
            Cursor.Current = Cursors.WaitCursor;
            NDF_DetectedDevices = DIASDAQ.DDAQ_DEVICE_DO_DETECTION();
            //Cursor.Current = Cursors.Default;

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

        private void GetDeviceType(uint DeviceNo, IntPtr irdxHandle)
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

        private void GetDeviceID(uint DeviceNo, IntPtr irdxHandle)
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
                CAM1_SerialNo = result.Substring(serialTemp, 8);
                main.CAM1_Serial.Text = CAM1_SerialNo.ToString();
            }
            else
            {
                CAM2_SerialNo = result.Substring(serialTemp, 8);
                main.CAM2_Serial.Text = CAM2_SerialNo.ToString();
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

        // Accept
        private void button1_Click(object sender, EventArgs e)
        {
            // Configuration 검증
            if (main.POSCO_CAM1_SERIAL == "" || main.POSCO_CAM2_SERIAL == "")
            {
                MessageBox.Show("프로그램을 시작할 수 없습니다. \n\nSerial Numer 정보를 확인할 수 없습니다.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DIASDAQ.DDAQ_DEVICE_DO_STOP(1);
                DIASDAQ.DDAQ_DEVICE_DO_CLOSE(1);
                DIASDAQ.DDAQ_DEVICE_DO_STOP(NDF_DetectedDevices);
                DIASDAQ.DDAQ_DEVICE_DO_CLOSE(NDF_DetectedDevices);
                main.DetectedDevices = 0;
                NDF_DetectedDevices = 0;
                main.pIRDX_Array[0] = IntPtr.Zero;
                main.pIRDX_Array[1] = IntPtr.Zero;
                Close();
                return;
            }

            if (!(CAM1_SerialNo == main.POSCO_CAM1_SERIAL && CAM2_SerialNo == main.POSCO_CAM2_SERIAL) ||
                !(CAM1_SerialNo == main.POSCO_CAM2_SERIAL && CAM2_SerialNo == main.POSCO_CAM1_SERIAL))
            //if (false)
            {
                MessageBox.Show("프로그램을 시작할 수 없습니다. \n\n감지된 " + NDF_DetectedDevices + "개의 장비 중 적어도 한 개 이상의 장비가\n연결이 성립될 수 없습니다.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DIASDAQ.DDAQ_DEVICE_DO_STOP(1);
                DIASDAQ.DDAQ_DEVICE_DO_CLOSE(1);
                DIASDAQ.DDAQ_DEVICE_DO_STOP(NDF_DetectedDevices);
                DIASDAQ.DDAQ_DEVICE_DO_CLOSE(NDF_DetectedDevices);
                main.DetectedDevices = 0;
                NDF_DetectedDevices = 0;
                main.pIRDX_Array[0] = IntPtr.Zero;
                main.pIRDX_Array[1] = IntPtr.Zero;
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
                DIASDAQ.DDAQ_IRDX_PALLET_SET_BAR(main.pIRDX_Array[i], 0, 256);
                DIASDAQ.DDAQ_IRDX_SCALE_GET_MINMAX(main.pIRDX_Array[i], ref min, ref max);
                DIASDAQ.DDAQ_IRDX_SCALE_SET_MINMAX(main.pIRDX_Array[i], min, max);
                DIASDAQ.DDAQ_IRDX_ACQUISITION_GET_AVERAGING(main.pIRDX_Array[i], ref avg);
            }

            // CAMERA #1 Thread ID Registering
            /// Get 기본 Thread ID Value
            uint nThreadID = (uint)Thread.CurrentThread.ManagedThreadId;
            /// Throw Thread ID                    
            if (DIASDAQ.DDAQ_DEVICE_SET_MSGTHREAD(NDF_DetectedDevices, nThreadID) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
                return;
            /// Default Frequency
            if (DIASDAQ.DDAQ_IRDX_ACQUISITION_SET_AVERAGING(main.pIRDX_Array[0], 1) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
                return;
            /// Device do start
            if (DIASDAQ.DDAQ_DEVICE_DO_START(NDF_DetectedDevices) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
                return;

            if (NDF_DetectedDevices != 1)
            {
                // CAMERA #2 thread id registering
                uint nThreadID_two = (uint)Thread.CurrentThread.ManagedThreadId;
                if (DIASDAQ.DDAQ_DEVICE_SET_MSGTHREAD(1, nThreadID_two) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
                    return;
                if (DIASDAQ.DDAQ_IRDX_ACQUISITION_SET_AVERAGING(main.pIRDX_Array[1], 1) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
                    return;
                if (DIASDAQ.DDAQ_DEVICE_DO_START(1) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
                    return;
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

            main.OPCSettingToolStripMenuItem.Enabled = true;

            main.toolStripSeparator3.Visible = true;
            main.toolStripSeparator4.Visible = true;
            main.toolStripSeparator5.Visible = true;
            main.toolStripSeparator6.Visible = true;

            //main.groupBox_CamTemp.Visible = true;
            //main.groupBox_DetectorTemp.Visible = true;
            main.panel1.Visible = true;

            main.propertyGrid1.Visible = true;

            thr1 = (Thread)main.Thread1_forPublicRef();
            thr1.Start();

            support_thr1 = (Thread)main.Support_Thread1_forPublicRef();
            support_thr1.Start();

            if (NDF_DetectedDevices != 1)
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

            opc = (CustomOPC)main.CustomOPC_forPublicRef();

            if (opc.connectFailed == false)
            //if(opc.connectFailed==true)
            {
                // OPC 데이터 송수신 활성화
                main.OPCActivated = true;
                main.InitOPCTimer();
            }

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

    }
}
