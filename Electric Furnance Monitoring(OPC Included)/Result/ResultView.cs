using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace Electric_Furnance_Monitoring_OPC_Included_
{
    public partial class ResultView : Form
    {
        MainForm main;
        ImageView imgView;
        SystemPropertyGrid property;
        SetThreshold thresholdForm;
        System.Configuration.Configuration config;

        public Label[] CAM1_LabelArray;
        public Color[] CAM1_POIConnected;
        public float[] CAM1_ThresholdTemp;
        bool[] CAM1_verify;
        public bool[] CAM1_isTempPM10;
        public bool[] CAM1_isTempUpper10;

        public Label[] CAM2_LabelArray;
        public Color[] CAM2_POIConnected;
        public float[] CAM2_ThresholdTemp;
        bool[] CAM2_verify;
        public bool[] CAM2_isTempPM10;
        public bool[] CAM2_isTempUpper10;

        public Color NotConnected;
        public Color Connected_NoWarning;
        public Color Connected_Warning;

        public ResultView(MainForm _main)
        {
            this.main = _main;
            imgView = (ImageView)main.ImageView_forPublicRef();
            property = (SystemPropertyGrid)main.customGrid_forPublicRef();
            thresholdForm = new SetThreshold(_main);

            InitializeComponent();

            ShowingAreaAdjust();

            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            CAM1_LabelArray = new Label[10];
            CAM1_POIConnected = new Color[10];
            CAM1_ThresholdTemp = new float[10];
            CAM1_verify = new bool[10];
            CAM1_isTempPM10 = new bool[10];
            CAM1_isTempUpper10 = new bool[10];

            CAM2_LabelArray = new Label[10];
            CAM2_POIConnected = new Color[10];
            CAM2_ThresholdTemp = new float[10];
            CAM2_verify = new bool[10];
            CAM2_isTempPM10 = new bool[10];
            CAM2_isTempUpper10 = new bool[10];

            NotConnected = Color.Gray;
            Connected_NoWarning = Color.Green;
            Connected_Warning = Color.Red;

            CAM1_AlarmInitialize();
            CAM2_AlarmInitialize();

            LoadConfiguration_Temperature();
        }

        private void ShowingAreaAdjust()
        {
            AlertToConnection.SplitterDistance = 750;
        }
        
        private void LoadConfiguration_Temperature()
        {
            string value = "";
            string appSetValue = "";

            // CAM1 ThresholdTemp Load
            for (int i = 0; i < 10; i++)
            {
                appSetValue = "CAM1_Threshold" + (i + 1).ToString();
                value = ConfigurationManager.AppSettings[appSetValue];
                CAM1_ThresholdTemp[i] = Convert.ToSingle(value);
            }
            // CAM2 ThresholdTemp Load
            for (int i = 0; i < 10; i++)
            {
                appSetValue = "CAM2_Threshold" + (i + 1).ToString();
                value = ConfigurationManager.AppSettings[appSetValue];
                CAM2_ThresholdTemp[i] = Convert.ToSingle(value);
            }
        }

        public void CAM1_AlarmInitialize()
        {
            #region CAM1Alarm

            CAM1_LabelArray[0] = c1_poi1;
            CAM1_LabelArray[1] = c1_poi2;
            CAM1_LabelArray[2] = c1_poi3;
            CAM1_LabelArray[3] = c1_poi4;
            CAM1_LabelArray[4] = c1_poi5;
            CAM1_LabelArray[5] = c1_poi6;
            CAM1_LabelArray[6] = c1_poi7;
            CAM1_LabelArray[7] = c1_poi8;
            CAM1_LabelArray[8] = c1_poi9;
            CAM1_LabelArray[9] = c1_poi10;

            c1_MainAlarm.ForeColor = NotConnected;
            for (int i = 0; i < 10; i++)
            {
                CAM1_LabelArray[i].ForeColor = NotConnected;
                CAM1_verify[i] = false;
                CAM1_isTempPM10[i] = false;
                CAM1_isTempUpper10[i] = false;
                CAM2_isTempPM10[i] = false;
                CAM2_isTempUpper10[i] = false;
            }

            #endregion
        }

        public void CAM2_AlarmInitialize()
        {
            #region CAM2Alarm

            CAM2_LabelArray[0] = c2_poi1;
            CAM2_LabelArray[1] = c2_poi2;
            CAM2_LabelArray[2] = c2_poi3;
            CAM2_LabelArray[3] = c2_poi4;
            CAM2_LabelArray[4] = c2_poi5;
            CAM2_LabelArray[5] = c2_poi6;
            CAM2_LabelArray[6] = c2_poi7;
            CAM2_LabelArray[7] = c2_poi8;
            CAM2_LabelArray[8] = c2_poi9;
            CAM2_LabelArray[9] = c2_poi10;

            c2_MainAlarm.ForeColor = NotConnected;
            for (int i = 0; i < 10; i++)
            {
                CAM2_LabelArray[i].ForeColor = NotConnected;
                CAM2_verify[i] = false;
            }

            #endregion
        }

        public void CAM1_DetectTempThreshold()
        {
            //VerifyOPC();

            if (imgView.CAM1_POICount != 0)
            {
                #region POIAlarm Control
                for (int i = 0; i < imgView.CAM1_POICount; i++)
                {
                    //if (imgView.CAM1_TemperatureArr[i] >= property.Threshold)
                    if (imgView.CAM1_TemperatureArr[i] >= CAM1_ThresholdTemp[i])
                    {
                        CAM1_verify[i] = true;
                        CAM1_LabelArray[i].ForeColor = Connected_Warning;
                    }
                    //else if (imgView.CAM1_TemperatureArr[i] < property.Threshold ||
                    else if (imgView.CAM1_TemperatureArr[i] < CAM1_ThresholdTemp[i] ||
                        imgView.CAM1_TemperatureArr[i] == 0)
                    {
                        CAM1_verify[i] = false;
                        CAM1_LabelArray[i].ForeColor = Connected_NoWarning;
                    }
                }

                for (int i = imgView.CAM1_POICount; i < 10; i++)
                {
                    CAM1_verify[i] = false;
                    CAM1_LabelArray[i].ForeColor = NotConnected;
                }

                #endregion

                #region MainAlarm Control
                if (CAM1_verify[0] == false && CAM1_verify[1] == false && CAM1_verify[2] == false && CAM1_verify[3] == false && CAM1_verify[4] == false &&
                    CAM1_verify[5] == false && CAM1_verify[6] == false && CAM1_verify[7] == false && CAM1_verify[8] == false && CAM1_verify[9] == false)
                {
                    c1_MainAlarm.ForeColor = Connected_NoWarning;
                }
                else
                {
                    c1_MainAlarm.ForeColor = Connected_Warning;
                }
                #endregion
            }

            // POI가 찍혀있었지만 모두 지워서 현재 화면상에 남아있는 POI가 없을 때에는
            else if (imgView.CAM1_POICount == 0)
            {
                // 제일 처음 초기화 상태로 되돌린다
                CAM1_AlarmInitialize();
            }
        }

        public void CAM2_DetectTempThreshold()
        {
            if (imgView.CAM2_POICount != 0)
            {
                for (int i = 0; i < imgView.CAM2_POICount; i++)
                {
                    //if (imgView.CAM2_TemperatureArr[i] >= property.Threshold)
                    if (imgView.CAM2_TemperatureArr[i] >= CAM2_ThresholdTemp[i])
                    {
                        CAM2_verify[i] = true;
                        CAM2_LabelArray[i].ForeColor = Connected_Warning;
                    }
                    //else if (imgView.CAM2_TemperatureArr[i] < property.Threshold ||
                    else if (imgView.CAM2_TemperatureArr[i] < CAM2_ThresholdTemp[i] ||
                        imgView.CAM2_TemperatureArr[i] == 0)
                    {
                        CAM2_verify[i] = false;
                        CAM2_LabelArray[i].ForeColor = Connected_NoWarning;
                    }
                }
                for (int i = imgView.CAM2_POICount; i < 10; i++)
                {
                    CAM2_verify[i] = false;
                    CAM2_LabelArray[i].ForeColor = NotConnected;
                }
                if (CAM2_verify[0] == false && CAM2_verify[1] == false && CAM2_verify[2] == false && CAM2_verify[3] == false && CAM2_verify[4] == false &&
                    CAM2_verify[5] == false && CAM2_verify[6] == false && CAM2_verify[7] == false && CAM2_verify[8] == false && CAM2_verify[9] == false)
                {
                    c2_MainAlarm.ForeColor = Connected_NoWarning;
                }
                else
                {
                    c2_MainAlarm.ForeColor = Connected_Warning;
                }

            }
            else if (imgView.CAM2_POICount == 0)
            {
                CAM2_AlarmInitialize();
            }
        }

        public void CAM1_DetectTemp_ForOPC()
        {
            for (int i = 0; i < imgView.CAM1_POICount; i++)
            {
                if (thresholdForm.CAM1_Threshold[i].Enabled == true)
                {
                    // 현재 온도가 Threshold의 +-10도 범위 안에 있으면
                    if (CAM1_ThresholdTemp[i] - 10 <= imgView.CAM1_TemperatureArr[i]
                        && imgView.CAM1_TemperatureArr[i] <= CAM1_ThresholdTemp[i] + 10)
                    {
                        CAM1_isTempPM10[i] = true;  // true
                        CAM1_isTempUpper10[i] = false;
                    }
                    // 현재 온도가 Threshold의 +10도보다 높으면
                    else if (CAM1_ThresholdTemp[i] + 10 < imgView.CAM1_TemperatureArr[i])
                    {
                        CAM1_isTempPM10[i] = false;
                        CAM1_isTempUpper10[i] = true;   // true
                    }
                    else
                    {
                        CAM1_isTempPM10[i] = false;     // 그도저도 아니면 false
                        CAM1_isTempUpper10[i] = false;
                    }
                }
            }
        }

        public void CAM2_DetectTemp_ForOPC()
        {
            for (int k = 0; k < imgView.CAM2_POICount; k++)
            {
                if (thresholdForm.CAM2_Threshold[k].Enabled == true)
                {
                    if (CAM2_ThresholdTemp[k] - 10 <= imgView.CAM2_TemperatureArr[k]
                        && imgView.CAM2_TemperatureArr[k] <= CAM2_ThresholdTemp[k] + 10)
                    {
                        CAM2_isTempPM10[k] = true;
                        CAM2_isTempUpper10[k] = false;
                    }
                    else if (CAM2_ThresholdTemp[k] + 10 < imgView.CAM2_TemperatureArr[k])
                    {
                        CAM2_isTempPM10[k] = false;
                        CAM2_isTempUpper10[k] = true;
                    }
                    else
                    {
                        CAM2_isTempPM10[k] = false;
                        CAM2_isTempUpper10[k] = false;
                    }
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            thresholdForm.ShowDialog();
        }

    }
}
