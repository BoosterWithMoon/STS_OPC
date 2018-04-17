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
    public partial class CAM1_DataGridView : Form
    {
        MainForm main;
        ImageView imgView;
        //private int colCount = 0;

        public CAM1_DataGridView(MainForm _main)
        {
            this.main = _main;
            InitializeComponent();
            imgView = (ImageView)main.ImageView_forPublicRef();
        }

        // for crossthread
        private delegate void RefreshTemperatureGrid();
        public void RefreshGrid()
        {
            if (dataGridView1.InvokeRequired)
            {
                RefreshTemperatureGrid rtg = new RefreshTemperatureGrid(RefreshGrid);
                dataGridView1.Invoke(rtg);
            }
            else
            {
                ShowTemperatureOnGrid();
            }
        }

        public void ShowTemperatureOnGrid()
        {
            if (imgView.CAM1_POICount == 0)
            {
                dataGridView1.Columns.Clear();
                return;
            }
            dataGridView1.Columns.Clear();
            //dataGridView1.ColumnCount = imgView.CAM1_POICount + 1;
            dataGridView1.ColumnCount = imgView.CAM1_POICount;

            for (int i = 0; i < imgView.CAM1_POICount; i++)
            {
                string temp = (i + 1).ToString();
                dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns[i].Name = "#" + temp;
                dataGridView1[(i), 0].Value = imgView.CAM1_TemperatureArr[i].ToString("N1") + "℃";
            }
        }

    }
}
