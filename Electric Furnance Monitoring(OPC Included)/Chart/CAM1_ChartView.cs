using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Electric_Furnance_Monitoring_OPC_Included_
{
    public partial class CAM1_ChartView : Form
    {
        MainForm main;
        ImageView imgView;
        int axisX_Count;
        private static int MAX_xCount;

        public CAM1_ChartView(MainForm _main)
        {
            this.main = _main;
            InitializeComponent();
            imgView = (ImageView)main.ImageView_forPublicRef();

            axisX_Count = 0;
            MAX_xCount = 50;
        }

        public void UpdateData()
        {
            if (imgView.CAM1_POICheckFlag)
            {
                CheckCurrentCount(imgView.CAM1_compPOICount, imgView.CAM1_POICount);
                imgView.CAM1_POICheckFlag = false;
            }

            if (imgView.CAM1_POICount != 0)
            {
                for (int i = 0; i < imgView.CAM1_POICount; i++)
                {
                    if (axTChart1.SeriesCount < imgView.CAM1_POICount) return;
                    if (axTChart1.Series(i).XValues.Count > MAX_xCount)
                    {
                        axTChart1.Series(i).Delete(0);
                    }
                    axTChart1.Series(i).AddXY(axisX_Count, imgView.CAM1_TemperatureArr[i], null, 0);
                }
                axisX_Count++;
            }

        }

        // series create & delete
        public void CheckCurrentCount(int POICount, int currentPOICount)
        {
            if (imgView.CAM1_POICheckFlag)
            {
                if (POICount < currentPOICount)
                {
                    for (int i = POICount; i < currentPOICount; i++)
                    {
                        string str = (i + 1).ToString();
                        axTChart1.AddSeries(TeeChart.ESeriesClass.scFastLine);
                        axTChart1.Series(i).Title = str;
                        axTChart1.Series(i).LegendTitle = str;

                    }
                    imgView.CAM1_compPOICount = imgView.CAM1_POICount;
                }
                else if (POICount > currentPOICount)
                {
                    for (int i = POICount - 1; i >= currentPOICount; i--)
                    {
                        axTChart1.Series(i).Clear();
                        axTChart1.RemoveSeries(i);
                    }
                    imgView.CAM1_compPOICount = imgView.CAM1_POICount;
                }
                imgView.CAM1_POICheckFlag = false;
            }
        }
    }
}