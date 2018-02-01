using System;
using System.Collections.Generic;
using System.Drawing;

using SubSys_MathUtility;
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_Graphics
{
    //单线程版本
    internal class XNodePainter : AbstractPainter
    {
        internal XNodePainter(System.Windows.Forms.Control canvas)
        {
            this.Canvas = canvas;
            this._graphic = canvas.CreateGraphics();
        }
        ~XNodePainter()
        {
            //this.Canvas = canvas;
            this._graphic.Dispose();
        }
        /// <summary>
        ///画一个矩形表示交叉口
        /// </summary>
        private void DrawXnode(IEntity te)
        {
            XNode rN = te as XNode;

            //计算交叉口矩形
            int iPixels = GraphicsSetter.iPixels;

            //计算左上角的屏幕坐标
            int iOffset = rN.Length / 2;
            OxyzPointF pStart = new OxyzPointF(rN.SpatialGrid._X - iOffset, rN.SpatialGrid._Y - iOffset);

            int iHO = iOffset / 2;
            OxyzPointF pA = new OxyzPointF(pStart._X + iHO, pStart._Y);
            OxyzPointF pB = new OxyzPointF(pStart._X + iHO * 3, pStart._Y);

            OxyzPointF pC = new OxyzPointF(pStart._X, pStart._Y + iHO);
            OxyzPointF pD = new OxyzPointF(pStart._X, pStart._Y + iHO * 3);

            OxyzPointF pE = new OxyzPointF(pStart._X + iHO, pStart._Y + iHO * 4);
            OxyzPointF pF = new OxyzPointF(pStart._X + iHO * 3, pStart._Y + iHO * 4);

            OxyzPointF pG = new OxyzPointF(pStart._X + iHO * 4, pStart._Y + iHO);
            OxyzPointF pI = new OxyzPointF(pStart._X + iHO * 4, pStart._Y + iHO * 3);

            var pA1 = Coordinates.Project(pA, iPixels).ToPointF();
            var pB1 = Coordinates.Project(pB, iPixels).ToPointF();

            var pC1 = Coordinates.Project(pC, iPixels).ToPointF();
            var pD1 = Coordinates.Project(pD, iPixels).ToPointF();

            var pE1 = Coordinates.Project(pE, iPixels).ToPointF();
            var pF1 = Coordinates.Project(pF, iPixels).ToPointF();

            var pG1 = Coordinates.Project(pG, iPixels).ToPointF();
            var pI1 = Coordinates.Project(pI, iPixels).ToPointF();

            PointF[] pits = { pD1, pC1, pA1, pB1, pG1, pI1, pF1, pE1 };
            _graphic.FillPolygon(new SolidBrush(GraphicsSetter.roadColor), pits);

        }


        protected override void SubPerform(IEntity mobilesInn)
        {

            this.DrawXnode(mobilesInn);

            var node = mobilesInn as XNode;

            MobilePainter.Paint(node.Mobiles, _graphic);

        }

        protected override void SubRevoke(IEntity tVar)
        {
            this.Canvas.Invalidate();
        }
    }


}
