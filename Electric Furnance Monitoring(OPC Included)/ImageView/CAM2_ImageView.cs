﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Electric_Furnance_Monitoring_OPC_Included_
{
    public partial class CAM2_ImageView : Form
    {
        MainForm main;
        ImageView imgView;
        STS.Core.Calculation cal;

        public Point CAM2_clickedPoint;
        public bool CAM2_isMouseButtonDown;
        public bool CAM2_PointMoveFlag;
        public bool CAM2_clicked;
        public bool CAM2_POIClicked;
        public int CAM2_pointIdx;

        public bool CAM2_isImageInPoint = false;

        public CAM2_ImageView(MainForm _main)
        {
            this.main = _main;
            InitializeComponent();
            imgView = (ImageView)main.ImageView_forPublicRef();
            cal = new STS.Core.Calculation();

            CAM2_clickedPoint = new Point();
            CAM2_isMouseButtonDown = false;
            CAM2_PointMoveFlag = false;
            CAM2_clicked = false;
            CAM2_POIClicked = false;
            CAM2_pointIdx = 0;
        }

        Point temp;
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            float DataPoint = 0.0f;
            imgView = (ImageView)main.ImageView_forPublicRef();

            if (imgView.c2_m_bmp_zoom == 0) return;

            temp = pictureBox1.PointToClient(new Point(MousePosition.X, MousePosition.Y));
            float currentZoom = imgView.c2_m_bmp_zoom;

            float[] calculatePos = cal.MousePosZoom(main.pIRDX_Array[1], temp, imgView.c2_m_bmp_ofs_x, imgView.c2_m_bmp_ofs_y, currentZoom);

            if ((calculatePos[0] >= 0.0f) && (calculatePos[1] >= 0.0f))
            {
                ushort ux = Convert.ToUInt16((ushort)calculatePos[0] + 1);
                ushort uy = Convert.ToUInt16((ushort)calculatePos[1] + 1);

                ushort sizex = 0, sizey = 0;
                DIASDAQ.DDAQ_IRDX_PIXEL_GET_SIZE(main.pIRDX_Array[1], ref sizex, ref sizey);

                if ((ux <= sizex) && (uy <= sizey))
                {
                    DIASDAQ.DDAQ_IRDX_PIXEL_GET_DATA_POINT(main.pIRDX_Array[1], ux, uy, ref DataPoint);

                    if (DataPoint < 0) DataPoint = 0;
                    else
                    {
                        imgView.CAM2_pointTemperatureData = DataPoint.ToString("N1");
                    }

                    if (CAM2_PointMoveFlag)
                    {
                        CAM2_clickedPoint = cal.MovePoint(imgView.CAM2_ClickedPosition, CAM2_clickedPoint, CAM2_pointIdx, ux, uy, imgView.c2_m_bmp_isize_x, imgView.c2_m_bmp_isize_y);
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

            CAM2_clickedPoint = pictureBox1.PointToClient(new Point(MousePosition.X, MousePosition.Y));
            cal.CalculatePoint(main.pIRDX_Array[1], CAM2_clickedPoint, imgView.c2_m_bmp_ofs_x, imgView.c2_m_bmp_ofs_y, imgView.c2_m_bmp_zoom, ref imgView.c2_ux, ref imgView.c2_uy);

            if (main.Activate_DrawPOI == true && imgView.CAM2_POICount < 10 &&
                (CAM2_clickedPoint.X > imgView.c2_m_bmp_ofs_x && CAM2_clickedPoint.Y > imgView.c2_m_bmp_ofs_y &&
                (CAM2_clickedPoint.X < (imgView.c2_m_bmp_ofs_x + imgView.c2_m_bmp_size_x)) && (CAM2_clickedPoint.Y < (imgView.c2_m_bmp_ofs_y + imgView.c2_m_bmp_size_y))))
            {
                Point tempPoint = new Point();
                tempPoint.X = imgView.c2_ux;
                tempPoint.Y = imgView.c2_uy;

                CAM2_pointIdx = imgView.CAM2_POICount;

                CAM2_clickedPoint.X = imgView.c2_ux;
                CAM2_clickedPoint.Y = imgView.c2_uy;

                imgView.CAM2_ClickedPosition[imgView.CAM2_POICount].X = tempPoint.X;
                imgView.CAM2_ClickedPosition[imgView.CAM2_POICount].Y = tempPoint.Y;

                imgView.CAM2_POICount++;

                imgView.CAM2_POICheckFlag = true;

                CAM2_isMouseButtonDown = true;
                CAM2_PointMoveFlag = true;
                CAM2_POIClicked = true;
            }
            else if (imgView.CAM2_POICount > 0)
            {
                bool hit = false;

                for (int i = 0; i < imgView.CAM2_POICount; i++)
                {
                    if (imgView.CAM2_ClickedPosition[i].X - 4 < imgView.c2_ux - 1 && imgView.CAM2_ClickedPosition[i].X + 4 > imgView.c2_ux - 1 &&
                        imgView.CAM2_ClickedPosition[i].Y - 4 < imgView.c2_uy - 1 && imgView.CAM2_ClickedPosition[i].Y + 4 > imgView.c2_uy - 1)
                    {
                        CAM2_PointMoveFlag = true;
                        CAM2_POIClicked = true;
                        hit = true;

                        CAM2_pointIdx = i;

                        CAM2_clickedPoint.X = imgView.c2_ux;
                        CAM2_clickedPoint.Y = imgView.c2_uy;
                    }
                }
                if (!hit)
                    CAM2_POIClicked = false;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            imgView.isCAM1Focused = false;
            imgView.isCAM2Focused = true;

            main.split_CAM2Info.Panel2.BackColor = Color.Black;
            main.split_CAM2Info.Panel2.Padding = new Padding { All = 5 };
            main.split_CAM1Info.Panel2.BackColor = Color.Transparent;

            if (e.X > imgView.c2_m_bmp_ofs_x && e.Y > imgView.c2_m_bmp_ofs_y &&
                e.X < (imgView.c2_m_bmp_ofs_x + imgView.c2_m_bmp_size_x) && e.Y < (imgView.c2_m_bmp_ofs_y + imgView.c2_m_bmp_size_x))
            {
                CAM2_isMouseButtonDown = false;
                CAM2_POIClicked = false;
            }

            if (CAM2_PointMoveFlag)
            {
                CAM2_PointMoveFlag = false;
                CAM2_clickedPoint.X = 0;
                CAM2_clickedPoint.Y = 0;
            }
        }
    }
}
