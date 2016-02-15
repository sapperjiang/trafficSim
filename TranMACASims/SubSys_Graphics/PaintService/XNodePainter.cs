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
		
		/// <summary>
		///画一个矩形表示交叉口
		/// </summary>
		private void DrawXnode(ITrafficEntity te)
		{
			XNode rN = te as XNode;
			
			//计算交叉口矩形
			int iPixels = GraphicsConfiger.iCellPixels;
			
			//计算左上角的屏幕坐标
			int iOffset = rN.iLength / 2;
			Point pStart = new Point(rN.Grid.X - iOffset, rN.Grid.Y - iOffset);

			int iHO = iOffset/2;
			Point  pA =new Point(pStart.X+iHO,pStart.Y);
			Point  pB =new Point(pStart.X+iHO*3,pStart.Y);
			
			Point  pC = new Point(pStart.X,pStart.Y+iHO);
			Point  pD =new Point(pStart.X,pStart.Y+iHO*3);
			
			Point  pE = new Point(pStart.X+iHO,pStart.Y+iHO*4);
			Point  pF= new Point(pStart.X+iHO*3,pStart.Y+iHO*4);
			
			Point  pG = new Point(pStart.X+iHO*4,pStart.Y+iHO);
			Point  pI = new Point(pStart.X+iHO*4,pStart.Y+iHO*3);
			
			pA =Coordinates.Project(pA , iPixels);
			pB = Coordinates.Project(pB, iPixels);
			
			pC = Coordinates.Project(pC, iPixels);
			pD = Coordinates.Project(pD , iPixels);
			
			pE = Coordinates.Project(pE, iPixels);//);
			pF= Coordinates.Project( pF , iPixels);
			
			pG = Coordinates.Project(pG , iPixels);
			pI = Coordinates.Project(pI , iPixels);

			Point[] pits = { pD, pC, pA, pB,pG,pI,pF,pE };
			_graphic.FillPolygon(new SolidBrush(GraphicsConfiger.roadColor), pits);
			
		}

		//        绘制在交叉口的车辆
		private void PaintCar(ITrafficEntity mobileContainer)
		{
			XNode rn = mobileContainer as XNode;
			int iLength = rn.iLength;
			Point itp;
			foreach (var item in rn)
			{
				itp = item.Track.pCurrPos;

				int iWidth = GraphicsConfiger.iCellPixels;
			Point pDraw = Coordinates.Project(p, iWidth);

			Brush b = new SolidBrush(Color.Red);
//			b.
			_graphic.FillEllipse(new SolidBrush(cell), pDraw.X - iWidth / 2, pDraw.Y - iWidth / 2, iWidth, iWidth);
			}

		}


		protected override void SubPerform(ITrafficEntity tVar)
		{
			Graphics g = this.Canvas.CreateGraphics();
			
			//   Rectangle rect = this.DrawEntity(tVar);
			this.DrawXnode(tVar);
			
			PaintCar(tVar);

			g.Dispose();
		}

		protected override void SubRevoke(ITrafficEntity tVar)
		{
			this.Canvas.Invalidate();
		}
	}


}
