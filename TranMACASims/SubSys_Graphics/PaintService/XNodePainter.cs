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
		private void DrawXnode(ITrafficEntity te)
		{
			XNode rN = te as XNode;
			
			//计算交叉口矩形
			int iPixels = GraphicsCfger.iPixels;
			
			//计算左上角的屏幕坐标
			int iOffset = rN.Length / 2;
			PointF pStart = new PointF(rN.Grid.X - iOffset, rN.Grid.Y - iOffset);

			int iHO = iOffset/2;
			PointF  pA =new PointF(pStart.X+iHO,pStart.Y);
			PointF  pB =new PointF(pStart.X+iHO*3,pStart.Y);
			
			PointF  pC = new PointF(pStart.X,pStart.Y+iHO);
			PointF  pD =new PointF(pStart.X,pStart.Y+iHO*3);
			
			PointF  pE = new PointF(pStart.X+iHO,pStart.Y+iHO*4);
			PointF  pF= new PointF(pStart.X+iHO*3,pStart.Y+iHO*4);
			
			PointF  pG = new PointF(pStart.X+iHO*4,pStart.Y+iHO);
			PointF  pI = new PointF(pStart.X+iHO*4,pStart.Y+iHO*3);
			
			pA =Coordinates.Project(pA , iPixels);
			pB = Coordinates.Project(pB, iPixels);
			
			pC = Coordinates.Project(pC, iPixels);
			pD = Coordinates.Project(pD , iPixels);
			
			pE = Coordinates.Project(pE, iPixels);
			pF= Coordinates.Project( pF , iPixels);
			
			pG = Coordinates.Project(pG , iPixels);
			pI = Coordinates.Project(pI , iPixels);

			PointF[] pits = { pD, pC, pA, pB,pG,pI,pF,pE };
			_graphic.FillPolygon(new SolidBrush(GraphicsCfger.roadColor), pits);
			
		}


		protected override void SubPerform(ITrafficEntity mobilesInn)
		{

			//this.DrawXnode(mobilesInn);
			
			var node = mobilesInn as XNode;
			
			MobilePainter.Paint(node.Mobiles,_graphic);

		}

		protected override void SubRevoke(ITrafficEntity tVar)
		{
			this.Canvas.Invalidate();
		}
	}


}
