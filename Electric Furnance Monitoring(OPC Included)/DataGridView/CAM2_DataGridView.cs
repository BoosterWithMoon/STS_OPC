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
    public partial class CAM2_DataGridView : Form
    {
        MainForm main;
        ImageView imgView;
        private int CAM2_ColCount = 0;

        public CAM2_DataGridView(MainForm _main)
        {
            this.main = _main;
            InitializeComponent();
            imgView = (ImageView)main.ImageView_forPublicRef();
        }

        private delegate void CAM2_RefreshTemperatureGrid();
        public void CAM2_RefreshGrid()
        {
            if (dataGridView1.InvokeRequired)
            {
                CAM2_RefreshTemperatureGrid c2_rtg = new CAM2_RefreshTemperatureGrid(CAM2_RefreshGrid);
                dataGridView1.Invoke(c2_rtg);
            }
            else
            {
                CAM2_ShowTemperatureOnGrid();
            }
        }

        public void CAM2_ShowTemperatureOnGrid()
        {
            if (imgView.CAM2_POICount == 0)
            {
                dataGridView1.Columns.Clear();
                return;
            }
            dataGridView1.Columns.Clear();
            dataGridView1.ColumnCount = imgView.CAM2_POICount;

            for (int i = 0; i < imgView.CAM2_POICount; i++)
            {
                string temp = (i + 1).ToString();
                dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns[i].Name = "#" + temp;
                dataGridView1[(i), 0].Value = imgView.CAM2_TemperatureArr[i].ToString("N1") + "℃";
            }
        }

    }
}
