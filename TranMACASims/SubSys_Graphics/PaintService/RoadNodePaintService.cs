using System;
using System.Collections.Generic;
using System.Drawing;

using SubSys_MathUtility;
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_Graphics
{
    //单线程版本
    internal class RoadNodePaintService : PaintService
    {
        internal RoadNodePaintService(System.Windows.Forms.Control canvas)
        {
            this.Canvas = canvas;
            this.graphic = canvas.CreateGraphics();
        }
       
         /// <summary>
         ///画一个矩形表示交叉口 
         /// </summary>
        private Rectangle DrawEntity(ITrafficEntity te)
        {
            RoadNode rN = te as RoadNode;
           
            //计算交叉口矩形
            int iPixels = GUISettings.iGUI_CellPixels; 
         
            //计算左上角的屏幕坐标
            int iOffset = rN.iLength / 2;
            
            Point pStart = Coordinates.Project(new Point(rN.RelativePosition.X - iOffset, rN.RelativePosition.Y - iOffset), iPixels);
      
            //重绘尺寸
            Size size =   new Size(rN.iLength * iPixels, rN.iWidth * iPixels);
            //擦除尺寸
            Size sizeInvalid = new Size(size.Height-1,size.Width-1);
            
            //确定交叉口无效区域
            Rectangle rect = new Rectangle(pStart,sizeInvalid);
            this.Canvas.Invalidate(rect);//原来的无效
            
                  //向下偏移一个像素
            pStart =Coordinates.Offset(pStart,new Point(1,1));
            //矩形向内偏移一个像素
            rect=  new Rectangle(pStart, size);
            
            this.graphic.DrawRectangle(new Pen(Color.Black), rect);//从新绘制
            return rect;

        }
        private void CreateCellSpaces(ITrafficEntity tVar)
        {
            int iLoopCount = tVar.iLength;//元胞长度，初始化参见registerservice
            int iLanes = iLoopCount / 2-1;
               
            Point pCenter = tVar.RelativePosition;

            this.CellSpaces.Clear();
            for (int i = -iLanes; i <= (iLanes+1); i++)//x行
            {
                for (int j = -iLanes; j <= (iLanes + 1); j++)//列
                {
                   MyPoint p= new MyPoint(pCenter.X + (i - 0.5f),pCenter.Y + (j - 0.5f));
                    CellSpaces.Add(i*iLoopCount+j,p);//起点坐标    
                }
            }
        }
//        绘制在交叉口的车辆
        private void PaintCar(ITrafficEntity tVar)
        {
            RoadNode rn = tVar as RoadNode;
            int iLength = rn.iLength;
            Point itp;
            foreach (var item in rn)
            {
                itp = item.Track.pCurrPos;
                MyPoint mp = CellSpaces[itp.X * iLength  - itp.Y+1];
               this.PaintCar(item.Car.Color,mp);
            }  

        }
        private void PaintCar(Color cell, MyPoint p)
        {

            int iWidth = GUISettings.iGUI_CellPixels;
            Point pDraw = Coordinates.Project(p, iWidth);

            graphic.FillEllipse(new SolidBrush(cell), pDraw.X - iWidth / 2, pDraw.Y - iWidth / 2, iWidth, iWidth);
        }


        protected override void SubPerform(ITrafficEntity tVar)
        {
            Graphics g = this.Canvas.CreateGraphics();
            
            Rectangle rect = this.DrawEntity(tVar);

            //获取元胞生存空间
            if (CellSpaces == null)
            {
                this.CellSpaces = new Dictionary<int, MyPoint>();
            } 
            
            this.CreateCellSpaces(tVar);
            
            PaintCar(tVar);

            g.Dispose();
        }

        protected override void SubRevoke(ITrafficEntity tVar)
        {
            this.Canvas.Invalidate();
        }
    }


}
