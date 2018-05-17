using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Electric_Furnance_Monitoring_OPC_Included_
{
    class ImageView
    {
        MainForm main;
        CAM1_ImageView c1_imgView;
        CAM2_ImageView c2_imgView;
        STS.Core.Calculation cal = new STS.Core.Calculation();
        STS.Core.Drawing draw = new STS.Core.Drawing();

        #region Variables
        public Bitmap bmp;
        public Bitmap Stretched_bmp;
        public Bitmap CAM2_bmp;
        public Bitmap CAM2_Stretched_bmp;
        public string pointTemperatureData = "";
        public string CAM2_pointTemperatureData = "";

        public Graphics g;
        public Graphics g_backbuffer;
        public Graphics CAM2_g;
        public Graphics CAM2_g_backbuffer;
        
        public ushort m_bmp_isize_x = 512;    // real bmp image x
        public ushort m_bmp_isize_y = 384;    // real bmp image y
        public ushort c2_m_bmp_isize_x = 320;
        public ushort c2_m_bmp_isize_y = 240;

        public int m_bmp_size_x = 0;        // stretched bmp image x
        public int m_bmp_size_y = 0;        // stretched bmp image y
        public int c2_m_bmp_size_x = 0;
        public int c2_m_bmp_size_y = 0;

        // offsets
        public int m_bmp_ofs_x = 0;
        public int m_bmp_ofs_y = 0;
        public int c2_m_bmp_ofs_x = 0;
        public int c2_m_bmp_ofs_y = 0;

        // zoom
        public float m_bmp_zoom = 0.0f;
        public float c2_m_bmp_zoom = 0.0f;

        public int CAM1_POICount = 0;
        public int CAM1_compPOICount = 0;
        public bool CAM1_POICheckFlag = false;
        public Point[] CAM1_ClickedPosition = new Point[10];
        public float[] CAM1_TemperatureArr = new float[10];

        public int CAM2_POICount = 0;
        public int CAM2_compPOICount = 0;
        public bool CAM2_POICheckFlag = false;
        public Point[] CAM2_ClickedPosition = new Point[10];
        public float[] CAM2_TemperatureArr = new float[10];

        public bool isCAM1Focused = false;
        public bool isCAM2Focused = false;

        public bool isCAM1RectDone = false;
        public bool isCAM2RectDone = false;

        public ushort ux = 0, uy = 0;
        public ushort c2_ux = 0, c2_uy = 0;

        private static int POI_XLimit = 280;
        private static int POI_YLimit = 225;

        private static int POI_TemperatureBox_X = 85;
        private static int POI_TemperatureBox_Y = 19;
        #endregion

        public ImageView(MainForm _main)
        {
            this.main = _main;
            c1_imgView = (CAM1_ImageView)main.CAM1_ImageView_forPublicRef();
            c2_imgView = (CAM2_ImageView)main.CAM2_ImageView_forPublicRef();
        }

        /*public void CalculatePoint(IntPtr irdxHandle, Point p)
        {
            if (irdxHandle == main.pIRDX_Array[0])
            {
                float fx = (float)p.X - (float)m_bmp_ofs_x;
                float fy = (float)p.Y - (float)m_bmp_ofs_y;

                fx /= m_bmp_zoom;
                fy /= m_bmp_zoom;

                DIASDAQ.DDAQ_ZMODE zoomMode = (ushort)DIASDAQ.DDAQ_ZMODE.DIRECT;
                float zoom = 1.0f;

                DIASDAQ.DDAQ_IRDX_IMAGE_GET_ZOOM(irdxHandle, ref zoomMode, ref zoom);
                if (zoomMode > DIASDAQ.DDAQ_ZMODE.DIRECT)
                {
                    fx /= zoom;
                    fy /= zoom;
                }
                ux = Convert.ToUInt16((ushort)fx + 1);
                uy = Convert.ToUInt16((ushort)fy + 1);
            }

            else if (irdxHandle == main.pIRDX_Array[1])
            {
                float fx = (float)p.X - (float)c2_m_bmp_ofs_x;
                float fy = (float)p.Y - (float)c2_m_bmp_ofs_y;

                fx /= c2_m_bmp_zoom;
                fy /= c2_m_bmp_zoom;

                DIASDAQ.DDAQ_ZMODE zoomMode = (ushort)DIASDAQ.DDAQ_ZMODE.DIRECT;
                float zoom = 1.0f;

                DIASDAQ.DDAQ_IRDX_IMAGE_GET_ZOOM(irdxHandle, ref zoomMode, ref zoom);
                if (zoomMode > DIASDAQ.DDAQ_ZMODE.DIRECT)
                {
                    fx /= zoom;
                    fy /= zoom;
                }
                c2_ux = Convert.ToUInt16((ushort)fx + 1);
                c2_uy = Convert.ToUInt16((ushort)fy + 1);
            }
        }*/

        // 실제 이미지를 뿌려주는 함수
        public void DrawImage(IntPtr hIRDX, PictureBox pb)
        {
            if (hIRDX == IntPtr.Zero) return;
            else
            {
                #region Calculate ImageZoom
                float[] bmpInfo = new float[5];
                bmpInfo = cal.GetStretchedImg(hIRDX, pb);
                m_bmp_zoom = bmpInfo[0];
                m_bmp_size_x = Convert.ToInt32(bmpInfo[1]);
                m_bmp_size_y = Convert.ToInt32(bmpInfo[2]);
                m_bmp_ofs_x = Convert.ToInt32(bmpInfo[3]);
                m_bmp_ofs_y = Convert.ToInt32(bmpInfo[4]);
                #endregion

                bmp = draw.GetBitmap(hIRDX);
                g = pb.CreateGraphics();

                Stretched_bmp = new Bitmap(bmp, new Size(m_bmp_size_x, m_bmp_size_y));

                g_backbuffer = Graphics.FromImage(Stretched_bmp);
                DrawPOI(hIRDX, pb, CAM1_ClickedPosition, CAM1_POICount, ref g_backbuffer);

                Point MousePosTemp = c1_imgView.pictureBox1.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));

                draw.DrawMouseString(bmpInfo, m_bmp_isize_x, m_bmp_isize_y, MousePosTemp, POI_XLimit, POI_YLimit, pointTemperatureData, g_backbuffer);

                g.DrawImage((Image)Stretched_bmp, m_bmp_ofs_x, m_bmp_ofs_y, m_bmp_size_x, m_bmp_size_y);

                if (bmp != null) bmp.Dispose();
                if (Stretched_bmp != null) Stretched_bmp.Dispose();

                g_backbuffer.Dispose();
                g.Dispose();
            }
        }

        // POI 그리는 함수
        public void DrawPOI(IntPtr irdxHandle, PictureBox pb, Point[] position, int poiCount, ref Graphics g)
        {
            c1_imgView = (CAM1_ImageView)main.CAM1_ImageView_forPublicRef();
            cal.CalculateCurrentTemp(irdxHandle, CAM1_POICount, CAM1_ClickedPosition, CAM1_TemperatureArr);
            draw.DrawPoint(position, m_bmp_zoom, poiCount, g);
            draw.DrawPointString(position, m_bmp_zoom, poiCount, g, POI_XLimit, POI_YLimit, POI_TemperatureBox_X, POI_TemperatureBox_Y, CAM1_TemperatureArr);
        }

        public void CAM2_DrawImage(IntPtr irdxHandle, PictureBox pb)
        {
            if (irdxHandle == IntPtr.Zero) return;
            else
            {
                #region CAM2_Calculate ImageZoom
                /// CalculateImageZoom
                float[] c2_bmpInfo = new float[5];
                c2_bmpInfo = cal.GetStretchedImg(irdxHandle, pb);
                c2_m_bmp_zoom = c2_bmpInfo[0];
                c2_m_bmp_size_x = Convert.ToInt32(c2_bmpInfo[1]);
                c2_m_bmp_size_y = Convert.ToInt32(c2_bmpInfo[2]);
                c2_m_bmp_ofs_x = Convert.ToInt32(c2_bmpInfo[3]);
                c2_m_bmp_ofs_y = Convert.ToInt32(c2_bmpInfo[4]);
                #endregion

                CAM2_bmp = draw.GetBitmap(irdxHandle);
                CAM2_g = pb.CreateGraphics();

                CAM2_Stretched_bmp = new Bitmap(CAM2_bmp, new Size(c2_m_bmp_size_x, c2_m_bmp_size_y));
                CAM2_g_backbuffer = Graphics.FromImage(CAM2_Stretched_bmp);

                CAM2_DrawPOI(irdxHandle, pb, CAM2_ClickedPosition, CAM2_POICount, ref CAM2_g_backbuffer);

                Point MousePosTemp = c2_imgView.pictureBox1.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                draw.DrawMouseString(c2_bmpInfo, c2_m_bmp_isize_x, c2_m_bmp_isize_y, MousePosTemp, POI_XLimit, POI_YLimit, CAM2_pointTemperatureData, CAM2_g_backbuffer);

                CAM2_g.DrawImage((Image)CAM2_Stretched_bmp, c2_m_bmp_ofs_x, c2_m_bmp_ofs_y, c2_m_bmp_size_x, c2_m_bmp_size_y);

                if (CAM2_bmp!= null) CAM2_bmp.Dispose();
                if (CAM2_Stretched_bmp != null) CAM2_Stretched_bmp.Dispose();

                CAM2_g.Dispose();
                CAM2_g_backbuffer.Dispose();
            }
        }

        public void CAM2_DrawPOI(IntPtr irdxHandle, PictureBox pb, Point[] position, /*float[] temp, */int poiCount, ref Graphics g)
        {
            c2_imgView = (CAM2_ImageView)main.CAM2_ImageView_forPublicRef();
            cal.CalculateCurrentTemp(irdxHandle, CAM2_POICount, CAM2_ClickedPosition, CAM2_TemperatureArr);
            draw.DrawPoint(position, c2_m_bmp_zoom, poiCount, g);
            draw.DrawPointString(position, c2_m_bmp_zoom, poiCount, g, POI_XLimit, POI_YLimit, POI_TemperatureBox_X, POI_TemperatureBox_Y, CAM2_TemperatureArr);
        }

       /* private void DrawRectROI(IntPtr irdxHandle, Graphics g, PictureBox pb)
        {
            if (bmp == null) return;
            Pen rectDrawPen = new Pen(Brushes.White);
            g = Graphics.FromImage(bmp);

            if (c1_imgView.clickedAfterUp != Point.Empty)
            {
                Point tempClickedPoint = c1_imgView.clickedPoint;
                Point tempClickedAfterUp = c1_imgView.clickedAfterUp;

                int temp_lx = Convert.ToInt32(tempClickedPoint.X / m_bmp_zoom - m_bmp_ofs_x);
                int temp_ly = Convert.ToInt32(tempClickedPoint.Y / m_bmp_zoom - m_bmp_ofs_y);
                int temp_rx = Convert.ToInt32(tempClickedAfterUp.X / m_bmp_zoom - m_bmp_ofs_x);
                int temp_ry = Convert.ToInt32(tempClickedAfterUp.Y / m_bmp_zoom - m_bmp_ofs_y);
                g.DrawRectangle(rectDrawPen, new Rectangle(temp_lx, temp_ly, temp_rx - temp_lx, temp_ry - temp_ly));
            }
        }*/

        //public float MaxTempInRect = 0.0f;
        /*public void CalculateRectROI(IntPtr irdxHandle)
        {
            if (bmp == null) return;
            Point tempClickedPoint = c1_imgView.clickedPoint;
            Point tempClickedAfterUp = c1_imgView.clickedAfterUp;
            if (tempClickedPoint == Point.Empty || tempClickedAfterUp == Point.Empty) return;
            else
            {
                ushort temp_x1 = Convert.ToUInt16(tempClickedPoint.X / m_bmp_zoom);
                ushort temp_y1 = Convert.ToUInt16(tempClickedPoint.Y / m_bmp_zoom);
                ushort temp_x2 = Convert.ToUInt16(tempClickedAfterUp.X / m_bmp_zoom);
                ushort temp_y2 = Convert.ToUInt16(tempClickedAfterUp.Y / m_bmp_zoom);

                // RectROI 사이즈에 맞는 정적 2차원배열 생성
                float[,] RectROIData = new float[temp_x2 - temp_x1, temp_y2 - temp_y1];
                int elementX = 0, elementY = 0;
                for (ushort i = temp_x1; i < temp_x2; i++)
                {
                    for (ushort k = temp_y1; k < temp_y2; k++)
                    {
                        DIASDAQ.DDAQ_IRDX_PIXEL_GET_DATA_POINT(irdxHandle, i, k, ref RectROIData[elementX, elementY]);
                        elementY++;
                    }
                    elementX++; elementY = 0;
                }

                // RectROI 내 최고온도값 계산
                for (ushort i = 0; i < temp_x2 - temp_x1; i++)
                {
                    for (ushort j = 0; j < temp_y2 - temp_y1; j++)
                    {
                        if (RectROIData[i, j] >= MaxTempInRect) MaxTempInRect = RectROIData[i, j];
                        else if (RectROIData[i, j] < MaxTempInRect) continue;
                    }
                }
            }
        }*/

        // 현재 온도값을 계산
        /*public void CalculateCurrentTemp(IntPtr irdxHandle, int POICount, Point[] clickedPos, float[] tempArray)
        {
            for (int i = 0; i < POICount; i++)
            {
                float temp = 0.0f;
                ushort tempX = (ushort)clickedPos[i].X;
                ushort tempY = (ushort)clickedPos[i].Y;
                DIASDAQ.DDAQ_IRDX_PIXEL_GET_DATA_POINT(irdxHandle, tempX, tempY, ref temp);
                tempArray[i] = temp;
            }
        }*/

        public void DeletePOI_InArray()
        {
            if (isCAM1Focused)
            {
                if (CAM1_POICount == 0) return;
                CAM1_POICount--;
                CAM1_ClickedPosition[CAM1_POICount] = Point.Empty;
                CAM1_TemperatureArr[CAM1_POICount] = 0;
            }
            else if (isCAM2Focused)
            {
                if (CAM2_POICount == 0) return;
                CAM2_POICount--;
                CAM2_ClickedPosition[CAM2_POICount] = Point.Empty;
                CAM2_TemperatureArr[CAM2_POICount] = 0;
            }
        }

        public void DrawScaleBar(IntPtr irdxHandle, PictureBox pb)
        {
            Bitmap scaleBMP = draw.GetScale(irdxHandle, main.pictureBox_ScaleBar);
            Graphics g = pb.CreateGraphics();

            g.DrawImage(scaleBMP, 0, 0, main.pictureBox_ScaleBar.Width, main.pictureBox_ScaleBar.Height);
        }
    }
}
