//using System;
//using System.Collections.Generic;
//using System.Drawing;

//using SubSys_SimDriving;
//using SubSys_SimDriving.TrafficModel;

//namespace SubSys_Graphics
//{
//    //单线程版本
//    internal class SignalPaintService : PaintService
//    {
//        internal SignalPaintService(System.Windows.Forms.Form fr)
//        {
//            this.Form = fr;
//            this.g = fr.CreateGraphics();
//        }
       
//        internal void Draw(Point pStart, int iLength, int iWidth)
//        {
//            Pen p = new Pen(Color.Red);
//            Rectangle rect = new Rectangle(pStart, new Size(iLength, iWidth));
//            this.Form.Invalidate(rect);//原来的无效
//            this.g.DrawRectangle(p, rect);//从新绘制
//        }
//        /// <summary>
//        /// 画一个矩形表示道路
//        /// </summary>
//        internal void Refresh(Point pStart, int iLength, int iWidth)
//        {
//            Pen p = new Pen(Color.Red);
//            this.g.DrawRectangle(p, pStart.X, pStart.Y, iLength, iWidth);
//        }

//        internal List<Rectangle> CreateCellSpaces(Point pStart, int iLength, int iCellWidth)
//        {
//            List<Rectangle> CellSpaces = new List<Rectangle>();
//            int iLoopCount = iLength;

//            for (int i = 0; i < iLoopCount; i++)
//            {
//                Point pS = new Point(pStart.X + i * iCellWidth, pStart.Y);
//                CellSpaces.Add(new Rectangle(pS, new Size(iCellWidth, iCellWidth)));//起点坐标
//            }
//            return CellSpaces;
//        }

//        internal Point GetStartPoint(int iLength)
//        {
//            System.Windows.Forms.Form fr = this.Form;
//            Point pWndCenter = new Point((int)(fr.Size.Width * 0.5), (int)(fr.Size.Height * 0.5));
//            int x = (int)(pWndCenter.X - iLength * 0.5);
//            return new Point(x, pWndCenter.Y);
//        }
//        protected override void SubPerform(ITrafficEntity tVar)
//        {
//            Graphics g = this.Form.CreateGraphics();

//            int iCellWidth = GUISettings.iGUI_CellPixels;

//            RoadLane rl = tVar as RoadLane;
//            if (rl == null)
//            {
//                throw new Exception("TrafficEntity类型为" +tVar.GetType().ToString()
//                    + ",使用了错误的Depicher");
//            }
//            Size szLane = new Size(rl.iLength * iCellWidth, rl.iWidth);
//            szLane.Height = iCellWidth;

//            Point pStart = GetStartPoint(szLane.Width);

//            //获取元胞生存空间
//            if (CellSpaces == null)
//            {
//                this.CreateCellSpaces(pStart, szLane.Width, iCellWidth);
//            }
//            this.Draw(pStart, szLane.Width, szLane.Height);

//            foreach (var cell in rl)
//            {
//                base.PaintCar(CellSpaces[(int)cell.rltPos.Y], cell.cmCarModel);
//            }
//            g.Dispose();
//        }
//        protected override void SubRevoke(ITrafficEntity tVar)
//        {
//            this.Form.Invalidate();
//        }
//    }


//}
