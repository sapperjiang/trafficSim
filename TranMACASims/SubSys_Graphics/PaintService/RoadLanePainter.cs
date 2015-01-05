using System;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;

using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;
using SubSys_MathUtility;

namespace SubSys_Graphics
{
     /// <summary>
    /// 到此开始启用装饰者模式,首先实现一个最基本的RoadLanePainter
    /// 然后启用装饰着来实现各种不同的车道类型；
    /// 还要实现一个工厂来根据参数创建不同的车道类型
    /// </summary>
    internal class RoadLanePainter : AbstractPainter
    {
        internal RoadLanePainter(System.Windows.Forms.Control canvas)
        {
            this.Canvas = canvas;
            this.graphic = canvas.CreateGraphics();
            this.IsRunning = true;
            //this.CellSpaces = new Dictionary<int, Rectangle>();
        }
        //如果是斜线可能就不行了，要用fillPolygon
        private void PaintCar(RoadLane roadLane)
        {
            if (roadLane.CellCount==0)
            {
                return;
            }
            if (roadLane.PrevCarPos[0] != -1)
            {
                int i=0;
                while (roadLane.PrevCarPos[i] != -1)
                {
                    MyPoint p = roadLane.Shape[roadLane.PrevCarPos[i] + 1];

                    //
                    PaintCar(GraphicsConfiger.roadColor, p);
                    //把有车的地方用道路颜色覆盖
                    i++;
                }
            }

            int j = 0;
            foreach (var cell in roadLane)
            {
                //画上车辆的新位置
                int iY = cell.RelativePosition.Y;
                int iPos = iY>1?iY-1:0;
                MyPoint p = roadLane.Shape[iPos+1];
                PaintCar(cell.Car.Color, p);
                roadLane.PrevCarPos[j++] = iPos;//保存车辆的位置便于下次使用
            }
            roadLane.PrevCarPos[j] = -1;
        }

        private void PaintCar(Color cell, MyPoint p)
        {
//Color.bl
            int iWidth = GraphicsConfiger.iCellPixels;
            Point pDraw = Coordinates.Project(p, iWidth);

            graphic.FillEllipse(new SolidBrush(cell), pDraw.X - iWidth / 2, pDraw.Y - iWidth / 2, iWidth, iWidth);
        }

        /// <summary>
        /// 画一个车道
        /// </summary>
        /// <param name="tVar"></param>
        protected override void SubPerform(ITrafficEntity tVar)
        {
            Graphics g = this.Canvas.CreateGraphics();
            RoadLane rl = tVar as RoadLane;
            if (rl == null)
            {
                throw new Exception(string.Format("TrafficEntity类型为{0},使用了错误的绘图服务",tVar.GetType().ToString()));
            }

            MyPoint spStart = rl.Container.Shape.Start;//[0];
            MyPoint spEnd = rl.Container.Shape.End;//[rl.Container.EntityShape.Count - 1];


            //获取单位平移向量(法向量)
            MyPoint mp1 = VectorTools.GetNormalVector((rl.Container as RoadEdge).ToVector());
            //获取平移向量
            MyPoint mpOffset = new MyPoint(mp1._X, mp1._Y);
            MyPoint mpMulti = new MyPoint(mpOffset._X * (rl.Rank - 1), mpOffset._Y * (rl.Rank - 1));
            spStart = Coordinates.Offset(spStart, mpMulti);
            spEnd = Coordinates.Offset(spEnd, mpMulti);

            //平移点
            MyPoint pOffsetFirst = Coordinates.Offset(spStart, mpOffset);
            MyPoint pOffsetEnd = Coordinates.Offset(spEnd, mpOffset);

            Point pA = Coordinates.Project(spStart, GraphicsConfiger.iCellPixels);
            Point pB = Coordinates.Project(spEnd, GraphicsConfiger.iCellPixels);
            Point pC = Coordinates.Project(pOffsetFirst, GraphicsConfiger.iCellPixels);
            Point pD = Coordinates.Project(pOffsetEnd, GraphicsConfiger.iCellPixels);


            Point[] pits = { pA, pB, pD, pC };
            g.FillPolygon(new SolidBrush(GraphicsConfiger.roadColor), pits);

            //画车道线
            Pen p = new Pen(new SolidBrush(Color.White), 1);
            p.DashStyle = DashStyle.Solid;//选用虚线画车道前半段
            //Point pMid = Point.Round(new PointF((pA.X + pB.X) * 0.5f, (pA.Y + pB.Y) * 0.5f));
            //Point pMidTwo = Point.Round(new PointF((pC.X + pC.X) * 0.5f, (pD.Y + pD.Y) * 0.5f));

            g.DrawLine(p, pA, pB);
            g.DrawLine(p, pC, pD);
            //g.DrawLine(p, pA, pMid);
            //g.DrawLine(p, pC, pMidTwo);

            //p.DashStyle = DashStyle.Solid;//使用实现画车道的后半段
            //g.DrawLine(p, pMid, pB);
            //g.DrawLine(p, pMidTwo, pD);

            //}

            PaintCar(rl);//画车
           
            g.Dispose();
        }

        protected override void SubRevoke(ITrafficEntity tVar)
        {
            throw new NotImplementedException();
        }
    }


}
