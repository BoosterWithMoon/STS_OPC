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
    public partial class CAM1_ImageView : Form
    {

        MainForm main;
        ImageView imgView;
        //Point[] CAM1_POIArr = new Point[10];
        CAM1_ChartView c1_chart;
        CAM1_DataGridView c1_grid;
        ResultView result;
        STS.Core.Calculation cal = new STS.Core.Calculation();

        public Point clickedPoint;
        public Point clickedAfterUp;
        public bool CAM1_isMouseButtonDown = false;
        public bool CAM1_PointMoveFlag = false;
        public bool CAM1_clicked = false;
        public bool CAM1_POIClicked = false;
        public int CAM1_pointIdx = 0;
        //bool c1focused = false;

        public CAM1_ImageView(MainForm _main)
        {
            this.main = _main;
            InitializeComponent();
            imgView = (ImageView)main.ImageView_forPublicRef();
            c1_chart = (CAM1_ChartView)main.CAM1_ChartView_forPublicRef();
            c1_grid = (CAM1_DataGridView)main.CAM1_GridView_forPublicRef();
            result = (ResultView)main.ResultView_forPublicRef();
        }

        Point temp;
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            float DataPoint = 0.0f;
            imgView = (ImageView)main.ImageView_forPublicRef();

            if (imgView.m_bmp_zoom == 0) return;

            temp = pictureBox1.PointToClient(new Point(MousePosition.X, MousePosition.Y));
            float currentZoom = imgView.m_bmp_zoom;

            // MousePosition에 대해 CalculatePoint
            float[] calculatePos = cal.MousePosZoom(main.pIRDX_Array[0], temp, imgView.m_bmp_ofs_x, imgView.m_bmp_ofs_y, currentZoom);

            if ((calculatePos[0] >= 0.0f) && (calculatePos[1] >= 0.0f))
            {
                ushort ux = Convert.ToUInt16((ushort)calculatePos[0] + 1);
                ushort uy = Convert.ToUInt16((ushort)calculatePos[1] + 1);

                ushort sizex = 0, sizey = 0;
                DIASDAQ.DDAQ_IRDX_PIXEL_GET_SIZE(main.pIRDX_Array[0], ref sizex, ref sizey);

                if ((ux <= sizex) && (uy <= sizey))
                {
                    DIASDAQ.DDAQ_IRDX_PIXEL_GET_DATA_POINT(main.pIRDX_Array[0], ux, uy, ref DataPoint);

                    if (DataPoint < 0) DataPoint = 0;
                    else
                    {
                        imgView.pointTemperatureData = DataPoint.ToString("N1");
                    }

                    if (CAM1_PointMoveFlag)
                    {
                        clickedPoint = cal.MovePoint(imgView.CAM1_ClickedPosition, clickedPoint, CAM1_pointIdx, ux, uy, imgView.m_bmp_isize_x, imgView.m_bmp_isize_y);
                    }
                }
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            // 현재 Logging이 동작 중이라면 POI 그리기/이동의 작업을 하지 않음
            if (main.isLoggingRunning) return;

            // 좌클릭 이외에 다른 버튼을 클릭 했을 때에는 아무것도 안함
            if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Middle) { return; }

            clickedPoint = pictureBox1.PointToClient(new Point(MousePosition.X, MousePosition.Y));
            cal.CalculatePoint(main.pIRDX_Array[0], clickedPoint, imgView.m_bmp_ofs_x, imgView.m_bmp_ofs_y, imgView.m_bmp_zoom, ref imgView.ux, ref imgView.uy);

            if (main.Activate_DrawPOI == true && imgView.CAM1_POICount < 10 &&
                clickedPoint.X > imgView.m_bmp_ofs_x && clickedPoint.Y > imgView.m_bmp_ofs_y &&
                clickedPoint.X < (imgView.m_bmp_ofs_x + imgView.m_bmp_size_x) && clickedPoint.Y < (imgView.m_bmp_ofs_y + imgView.m_bmp_size_y))
            {
                Point tempPoint = new Point();
                tempPoint.X = imgView.ux;
                tempPoint.Y = imgView.uy;

                CAM1_pointIdx = imgView.CAM1_POICount;

                clickedPoint.X = imgView.ux;
                clickedPoint.Y = imgView.uy;

                imgView.CAM1_ClickedPosition[imgView.CAM1_POICount].X = tempPoint.X;
                imgView.CAM1_ClickedPosition[imgView.CAM1_POICount].Y = tempPoint.Y;

                imgView.CAM1_POICount++;

                imgView.CAM1_POICheckFlag = true;

                CAM1_isMouseButtonDown = true;
                CAM1_PointMoveFlag = true;
                CAM1_POIClicked = true;
            }
            else if (imgView.CAM1_POICount > 0)
            {
                bool hit = false;

                for (int i = 0; i < imgView.CAM1_POICount; i++)
                {
                    if (imgView.CAM1_ClickedPosition[i].X - 4 < imgView.ux - 1 && imgView.CAM1_ClickedPosition[i].X + 4 > imgView.ux - 1 &&
                        imgView.CAM1_ClickedPosition[i].Y - 4 < imgView.uy - 1 && imgView.CAM1_ClickedPosition[i].Y + 4 > imgView.uy - 1)
                    {
                        CAM1_PointMoveFlag = true;
                        CAM1_POIClicked = true;
                        hit = true;

                        CAM1_pointIdx = i;

                        clickedPoint.X = imgView.ux;
                        clickedPoint.Y = imgView.uy;
                    }
                }
                if (!hit)
                    CAM1_POIClicked = false;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            // 사각형 ROI 관련
            //if (main.rectROIDraw && clickedAfterUp == Point.Empty)
            //{
            //    clickedAfterUp = pictureBox1.PointToClient(new Point(MousePosition.X, MousePosition.Y));
            //}
            imgView.isCAM1Focused = true;
            imgView.isCAM2Focused = false;

            main.split_CAM1Info.Panel2.BackColor = Color.Black;
            main.split_CAM1Info.Panel2.Padding = new Padding { All = 5 };
            main.split_CAM2Info.Panel2.BackColor = Color.Transparent;

            if (e.X > imgView.m_bmp_ofs_x && e.Y > imgView.m_bmp_ofs_y &&
                e.X < (imgView.m_bmp_ofs_x + imgView.m_bmp_size_x) && e.Y < (imgView.m_bmp_ofs_y + imgView.m_bmp_size_x))
            {
                CAM1_isMouseButtonDown = false;
                CAM1_POIClicked = false;
            }
            if (CAM1_PointMoveFlag)
            {
                CAM1_PointMoveFlag = false;
                clickedPoint.X = 0;
                clickedPoint.Y = 0;
            }
        }
    }
}
