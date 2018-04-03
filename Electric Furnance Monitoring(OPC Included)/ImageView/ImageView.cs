﻿using System;
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
        // picturebox 위에서 마우스 온도값을 보여줄 위치 설정
        public enum TemperatureShowLocation : ushort
        {
            DEFAULT = 0,
            QUADRANT_ONE = 1,
            QUADRANT_TWO = 2,
            QUADRANT_THREE = 3,
            QUADRANT_FOUR = 4
        }

        MainForm main;
        CAM1_ImageView c1_imgView;
        CAM2_ImageView c2_imgView;
        //public Image img;
        public Bitmap bmp;
        public Bitmap Stretched_bmp;
        public Bitmap CAM2_bmp;
        public Bitmap CAM2_Stretched_bmp;
        public string pointTemperatureData = "";
        public string CAM2_pointTemperatureData = "";
        public TemperatureShowLocation TextLocation = TemperatureShowLocation.DEFAULT;

        //public Bitmap backbuffer;

        //private Rectangle oldRect, currentRect;

        public Graphics g;
        public Graphics g_backbuffer;
        public Graphics CAM2_g;
        public Graphics CAM2_g_backbuffer;

        public ImageView(MainForm _main)
        {
            this.main = _main;
            c1_imgView = (CAM1_ImageView)main.CAM1_ImageView_forPublicRef();
        }

        #region Variables

        public ushort m_bmp_isize_x = 0;    // real bmp image x
        public ushort m_bmp_isize_y = 0;    // real bmp image y
        public ushort c2_m_bmp_isize_x = 0;
        public ushort c2_m_bmp_isize_y = 0;

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
        #endregion

        public ushort ux = 0, uy = 0;
        public ushort c2_ux = 0, c2_uy = 0;
        public void CalculatePoint(IntPtr irdxHandle, Point p)
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
        }

        // 실제 이미지를 뿌려주는 함수
        public void DrawImage(IntPtr hIRDX, PictureBox pb)
        {
            if (hIRDX == IntPtr.Zero) return;
            else
            {
                #region Calculate ImageZoom
                // CalculateImageZoom
                int wnd_sizex = pb.Width;
                int wnd_sizey = pb.Height;

                IntPtr ppbits = new IntPtr();
                IntPtr ppbitmapinfo = new IntPtr();

                DIASDAQ.DDAQ_IRDX_IMAGE_GET_BITMAP(hIRDX, ref m_bmp_isize_x, ref m_bmp_isize_y, out ppbits, out ppbitmapinfo);

                float zoomx = (float)wnd_sizex / (float)m_bmp_isize_x;
                float zoomy = (float)wnd_sizey / (float)m_bmp_isize_y;

                m_bmp_zoom = (((zoomx) < (zoomy)) ? (zoomx) : (zoomy));
                if (m_bmp_zoom < 0.1f) m_bmp_zoom = 0.1f;

                m_bmp_size_x = (int)(m_bmp_zoom * m_bmp_isize_x);
                m_bmp_size_y = (int)(m_bmp_zoom * m_bmp_isize_y);

                m_bmp_ofs_x = (int)((wnd_sizex - m_bmp_size_x) / 2.0);
                m_bmp_ofs_y = (int)((wnd_sizey - m_bmp_size_y) / 2.0);
                #endregion

                bmp = GET_BITMAP(hIRDX);
                //if (bmp == null) return;
                g = pb.CreateGraphics();

                Stretched_bmp = new Bitmap(bmp, new Size(m_bmp_size_x, m_bmp_size_y));

                g_backbuffer = Graphics.FromImage(Stretched_bmp);
                DrawPOI(hIRDX, pb, CAM1_ClickedPosition, CAM1_POICount, ref g_backbuffer);

                Point MousePosTemp = c1_imgView.pictureBox1.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                g_backbuffer.DrawString(pointTemperatureData + "℃", new Font("맑은 고딕", 12, FontStyle.Bold), Brushes.Black, new Point((int)((MousePosTemp.X/* / m_bmp_zoom*/) - m_bmp_ofs_x + 5), (int)((MousePosTemp.Y /*/ m_bmp_zoom*/) - m_bmp_ofs_y - 20)));

                g.DrawImage((Image)Stretched_bmp, m_bmp_ofs_x, m_bmp_ofs_y, m_bmp_size_x, m_bmp_size_y);

                if (bmp != null) bmp.Dispose();
                if (Stretched_bmp != null) Stretched_bmp.Dispose();

                g_backbuffer.Dispose();
                g.Dispose();
            }
        }

        // POI 그리는 함수
        public void DrawPOI(IntPtr irdxHandle, PictureBox pb, Point[] position, /*float[] temp, */int poiCount, ref Graphics g)
        {
            c1_imgView = (CAM1_ImageView)main.CAM1_ImageView_forPublicRef();

            if (position[0].X <= 0 || position[0].Y <= 0) return;

            int fShort = 1, fLong = 4;
            //int fShort = 3, fLong = 3;
            //float fShort = 1.5f, fLong = 6.0f;

            for (int i = 0; i < poiCount; i++)
            {
                if (position[i].X == 0 && position[i].Y == 0) continue;

                float x1 = (position[i].X * m_bmp_zoom) - (fLong + 2);
                float y1 = (position[i].Y * m_bmp_zoom) - (fShort + 2);
                float x2 = (position[i].X * m_bmp_zoom) + (fLong + 2);
                float y2 = (position[i].Y * m_bmp_zoom) + (fShort + 2);
                g.FillRectangle(Brushes.White, x1, y1, (x2 - x1), (y2 - y1));

                x1 = (position[i].X * m_bmp_zoom) - (fShort + 2);
                y1 = (position[i].Y * m_bmp_zoom) - (fLong + 2);
                x2 = (position[i].X * m_bmp_zoom) + (fShort + 2);
                y2 = (position[i].Y * m_bmp_zoom) + (fLong + 2);
                g.FillRectangle(Brushes.White, x1, y1, (x2 - x1), (y2 - y1));

                x1 = position[i].X * m_bmp_zoom - (fShort + 1);
                y1 = position[i].Y * m_bmp_zoom - (fLong + 1);
                x2 = position[i].X * m_bmp_zoom + (fShort + 1);
                y2 = position[i].Y * m_bmp_zoom + (fLong + 1);
                g.FillRectangle(Brushes.LightGreen, x1, y1, (x2 - x1), (y2 - y1));

                x1 = position[i].X * m_bmp_zoom - (fLong + 1);
                y1 = position[i].Y * m_bmp_zoom - (fShort + 1);
                x2 = position[i].X * m_bmp_zoom + (fLong + 1);
                y2 = position[i].Y * m_bmp_zoom + (fShort + 1);
                g.FillRectangle(Brushes.LightGreen, x1, y1, (x2 - x1), (y2 - y1));
            }

            Font f = new Font("맑은 고딕", 12, FontStyle.Bold);
            // QUADRANT별 처리
            for (int i = 0; i < poiCount; i++)
            {
                if (position[i].X > 280 && position[i].Y < 225)
                {
                    g.FillRectangle(Brushes.White, (position[i].X * m_bmp_zoom) + 7-90, (position[i].Y * m_bmp_zoom + 2), 75, 19);
                    g.DrawString("[" + (i + 1) + "] " + CAM1_TemperatureArr[i].ToString("N1") + "℃", f, Brushes.Black, (position[i].X * m_bmp_zoom) + 5-90, position[i].Y * m_bmp_zoom);
                }
                else if(position[i].X>280 && position[i].Y > 225)
                {
                    g.FillRectangle(Brushes.White, (position[i].X * m_bmp_zoom) + 7 - 90, (position[i].Y * m_bmp_zoom + 2-30), 75, 19);
                    g.DrawString("[" + (i + 1) + "] " + CAM1_TemperatureArr[i].ToString("N1") + "℃", f, Brushes.Black, (position[i].X * m_bmp_zoom) + 5 - 90, position[i].Y * m_bmp_zoom-30);
                }
                else if(position[i].X>0 && position[i].Y>225)
                {
                    g.FillRectangle(Brushes.White, (position[i].X * m_bmp_zoom) + 7, (position[i].Y * m_bmp_zoom + 2 - 30), 75, 19);
                    g.DrawString("[" + (i + 1) + "] " + CAM1_TemperatureArr[i].ToString("N1") + "℃", f, Brushes.Black, (position[i].X * m_bmp_zoom) + 5, position[i].Y * m_bmp_zoom - 30);
                }
                else  // DEFAULT
                {
                    g.FillRectangle(Brushes.White, (position[i].X * m_bmp_zoom) + 7, (position[i].Y * m_bmp_zoom + 2), 75, 19);
                    g.DrawString("[" + (i + 1) + "] " + CAM1_TemperatureArr[i].ToString("N1") + "℃", f, Brushes.Black, (position[i].X * m_bmp_zoom) + 5, position[i].Y * m_bmp_zoom);
                }
            }
            
        }

        public void CAM2_DrawImage(IntPtr irdxHandle, PictureBox pb, Point[] clickedPos, int poiCnt)
        {
            if (irdxHandle == IntPtr.Zero) return;
            else
            {
                #region CAM2_Calculate ImageZoom
                // CalculateImageZoom
                int wnd_sizex = pb.Width;
                int wnd_sizey = pb.Height;

                IntPtr ppbits = new IntPtr();
                IntPtr ppbitmapinfo = new IntPtr();

                DIASDAQ.DDAQ_IRDX_IMAGE_GET_BITMAP(irdxHandle, ref c2_m_bmp_isize_x, ref c2_m_bmp_isize_y, out ppbits, out ppbitmapinfo);

                float zoomx = (float)wnd_sizex / (float)c2_m_bmp_isize_x;
                float zoomy = (float)wnd_sizey / (float)c2_m_bmp_isize_y;

                c2_m_bmp_zoom = (((zoomx) < (zoomy)) ? (zoomx) : (zoomy));
                if (c2_m_bmp_zoom < 0.1f) c2_m_bmp_zoom = 0.1f;

                c2_m_bmp_size_x = (int)(c2_m_bmp_zoom * c2_m_bmp_isize_x);
                c2_m_bmp_size_y = (int)(c2_m_bmp_zoom * c2_m_bmp_isize_y);

                c2_m_bmp_ofs_x = (int)((wnd_sizex - c2_m_bmp_size_x) / 2.0);
                c2_m_bmp_ofs_y = (int)((wnd_sizey - c2_m_bmp_size_y) / 2.0);
                #endregion

                //if (CAM2_bmp != null) CAM2_bmp.Dispose();
                CAM2_bmp = GET_BITMAP(irdxHandle);
                CAM2_g = pb.CreateGraphics();

                CAM2_Stretched_bmp = new Bitmap(CAM2_bmp, new Size(c2_m_bmp_size_x, c2_m_bmp_size_y));

                CAM2_g_backbuffer = Graphics.FromImage(CAM2_Stretched_bmp);
                CAM2_DrawPOI(irdxHandle, pb, CAM2_ClickedPosition, CAM2_POICount, ref CAM2_g_backbuffer);

                Point MousePosTemp = c2_imgView.pictureBox1.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                CAM2_g_backbuffer.DrawString(CAM2_pointTemperatureData + "℃", new Font("맑은 고딕", 12, FontStyle.Bold), Brushes.Black, new Point((int)((MousePosTemp.X) - m_bmp_ofs_x + 5), (int)((MousePosTemp.Y) - m_bmp_ofs_y - 20)));

                CAM2_g.DrawImage((Image)CAM2_Stretched_bmp, c2_m_bmp_ofs_x, c2_m_bmp_ofs_y, c2_m_bmp_size_x, c2_m_bmp_size_y);

                CAM2_bmp.Dispose();
                CAM2_Stretched_bmp.Dispose();

                CAM2_g.Dispose();
                CAM2_g_backbuffer.Dispose();
            }
        }

        public void CAM2_DrawPOI(IntPtr irdxHandle, PictureBox pb, Point[] position, /*float[] temp, */int poiCount, ref Graphics g)
        {
            c2_imgView = (CAM2_ImageView)main.CAM2_ImageView_forPublicRef();
            if (position[0].X <= 0 || position[0].Y <= 0) return;

            float fShort = 1.0f, fLong = 4.0f;

            for (int i = 0; i < poiCount; i++)
            {
                if (position[i].X == 0 && position[i].Y == 0) continue;

                float x1 = (position[i].X * c2_m_bmp_zoom) - (fLong + 2);
                float y1 = (position[i].Y * c2_m_bmp_zoom) - (fShort + 2);
                float x2 = (position[i].X * c2_m_bmp_zoom) + (fLong + 2);
                float y2 = (position[i].Y * c2_m_bmp_zoom) + (fShort + 2);
                g.FillRectangle(Brushes.White, x1, y1, (x2 - x1), (y2 - y1));

                x1 = (position[i].X * c2_m_bmp_zoom) - (fShort + 2);
                y1 = (position[i].Y * c2_m_bmp_zoom) - (fLong + 2);
                x2 = (position[i].X * c2_m_bmp_zoom) + (fShort + 2);
                y2 = (position[i].Y * c2_m_bmp_zoom) + (fLong + 2);
                g.FillRectangle(Brushes.White, x1, y1, (x2 - x1), (y2 - y1));

                x1 = (position[i].X * c2_m_bmp_zoom) - (fShort + 1);
                y1 = (position[i].Y * c2_m_bmp_zoom) - (fLong + 1);
                x2 = (position[i].X * c2_m_bmp_zoom) + (fShort + 1);
                y2 = (position[i].Y * c2_m_bmp_zoom) + (fLong + 1);
                g.FillRectangle(Brushes.LightGreen, x1, y1, (x2 - x1), (y2 - y1));

                x1 = (position[i].X * c2_m_bmp_zoom) - (fLong + 1);
                y1 = (position[i].Y * c2_m_bmp_zoom) - (fShort + 1);
                x2 = (position[i].X * c2_m_bmp_zoom) + (fLong + 1);
                y2 = (position[i].Y * c2_m_bmp_zoom) + (fShort + 1);
                g.FillRectangle(Brushes.LightGreen, x1, y1, (x2 - x1), (y2 - y1));
            }
            Font f = new Font("맑은 고딕", 12, FontStyle.Bold);
            for (int i = 0; i < poiCount; i++)
            {
                g.DrawString("#" + (i + 1) + " " + CAM2_TemperatureArr[i].ToString("N1") + "℃", f, Brushes.Black, (position[i].X * c2_m_bmp_zoom)+5, position[i].Y *c2_m_bmp_zoom);
            }
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

        public float MaxTempInRect = 0.0f;
        public void CalculateRectROI(IntPtr irdxHandle)
        {
            //if (bmp == null) return;
            //Point tempClickedPoint = c1_imgView.clickedPoint;
            //Point tempClickedAfterUp = c1_imgView.clickedAfterUp;
            //if (tempClickedPoint == Point.Empty || tempClickedAfterUp == Point.Empty) return;
            //else
            //{
            //    ushort temp_x1 = Convert.ToUInt16(tempClickedPoint.X / m_bmp_zoom);
            //    ushort temp_y1 = Convert.ToUInt16(tempClickedPoint.Y / m_bmp_zoom);
            //    ushort temp_x2 = Convert.ToUInt16(tempClickedAfterUp.X / m_bmp_zoom);
            //    ushort temp_y2 = Convert.ToUInt16(tempClickedAfterUp.Y / m_bmp_zoom);

            //    // RectROI 사이즈에 맞는 정적 2차원배열 생성
            //    float[,] RectROIData = new float[temp_x2 - temp_x1, temp_y2 - temp_y1];
            //    int elementX = 0, elementY = 0;
            //    for (ushort i = temp_x1; i < temp_x2; i++)
            //    {
            //        for (ushort k = temp_y1; k < temp_y2; k++)
            //        {
            //            DIASDAQ.DDAQ_IRDX_PIXEL_GET_DATA_POINT(irdxHandle, i, k, ref RectROIData[elementX, elementY]);
            //            elementY++;
            //        }
            //        elementX++; elementY = 0;
            //    }

            //    // RectROI 내 최고온도값 계산
            //    for (ushort i = 0; i < temp_x2 - temp_x1; i++)
            //    {
            //        for (ushort j = 0; j < temp_y2 - temp_y1; j++)
            //        {
            //            if (RectROIData[i, j] >= MaxTempInRect) MaxTempInRect = RectROIData[i, j];
            //            else if (RectROIData[i, j] < MaxTempInRect) continue;
            //        }
            //    }
            //}
        }

        // 현재 온도값을 계산(스레드에 들어가있는데 빠져도 무방할 듯)
        public void CalculateCurrentTemp(IntPtr irdxHandle, int POICount, Point[] clickedPos, float[] tempArray)
        {
            for (int i = 0; i < POICount; i++)
            {
                float temp = 0.0f;
                ushort tempX = (ushort)clickedPos[i].X;
                ushort tempY = (ushort)clickedPos[i].Y;
                DIASDAQ.DDAQ_IRDX_PIXEL_GET_DATA_POINT(irdxHandle, tempX, tempY, ref temp);
                tempArray[i] = temp;
            }
        }

        // POI 삭제 버튼을 눌렀을 때
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

        // 깜빡임 현상 제거를 위해 테스트 중인 함수(MainForm의 DllImport (MainForm #129) 참조)
        /*public void InvalidateTextRect(PictureBox pb, Point p, int x1, int y1, int x2, int y2)
        {
            //DLLIMPORT.InvalidateRect(ref oldRect, 0, true);
            //IntPtr pbHandle = Control.FromHandle(pb.Handle);
            IntPtr pbHandle = pb.Handle;

            Rectangle invalidateArea = new Rectangle(x1, y1, x2, y2);

            main.PaintWindow(pbHandle);
        }
        */

        // (bitmap을 카메라로부터 얻어옴)
        private static Bitmap GET_BITMAP(IntPtr hIRDX)
        {
            IntPtr pbitsImage = new IntPtr();
            IntPtr bmiImage = new IntPtr();
            ushort width = 0, height = 0;
            if (DIASDAQ.DDAQ_IRDX_IMAGE_GET_BITMAP(hIRDX, ref width, ref height, out pbitsImage, out bmiImage) != DIASDAQ.DDAQ_ERROR.NO_ERROR)
            {
                return null; // failure
            }

            MethodInfo mi = typeof(Bitmap).GetMethod("FromGDIplus", BindingFlags.Static | BindingFlags.NonPublic);

            if (mi == null)
            {
                return null; // permission problem 
            }

            IntPtr pBmp = IntPtr.Zero;
            int status = DIASDAQ.GDIPLUS_GdipCreateBitmapFromGdiDib(bmiImage, pbitsImage, out pBmp);

            if ((status == 0) && (pBmp != IntPtr.Zero))
            {
                return (Bitmap)mi.Invoke(null, new object[] { pBmp }); // success 
            }
            else
            {
                return null; // failure
            }
        }
    }
}
