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
			
			pE = Coordinates.Project(pE, iPixels);//);
			pF= Coordinates.Project( pF , iPixels);
			
			pG = Coordinates.Project(pG , iPixels);
			pI = Coordinates.Project(pI , iPixels);

			PointF[] pits = { pD, pC, pA, pB,pG,pI,pF,pE };
//			_graphic.FillPolygon(new SolidBrush(GraphicsCfger.roadColor), pits);
			
		}

		/// <summary>
		/// paint a mobile within a XNode
		/// </summary>
		/// <param name="mobileContainer"></param>
		private void PaintMobile(ITrafficEntity mobileContainer)
		{
			var node = mobileContainer as XNode;
			int iLength = node.Length;
			//use GetEnumerator() method
			int iWidth =GraphicsCfger.iPixels;
			foreach (var mobile in node.Mobiles)
			{			
				//mobilePoint = mobile.Track.Current.Clone();//clone to avoid be modified;
				for (int i = 0; i < mobile.Shape.Count; i++) {
					var mobilePrev = mobile.PrevShape[i];
					
					var mobileShape = mobile.Shape[i];
					


					PointF pMobile = Coordinates.Project(mobileShape, iWidth);
					
					PointF pMobilePrev = Coordinates.Project(mobilePrev, iWidth);

					//获取单位平移向量(法向量)
					OxyzPointF mpOffset = VectorTools.GetNormal((mobile.Track.FromLane.Container as Way).ToVector());
					//	OxyzPointF mpMulti = new OxyzPointF(mpOffset._X * (rl.Rank - 1), mpOffset._Y * (rl.Rank - 1));
					//get offset vector
					OxyzPointF mpMulti = new OxyzPointF(mpOffset._X * (2 - 1), mpOffset._Y * (2 - 1));
					
					pMobile = Coordinates.Offset(pMobile, mpMulti.ToPoint());
					
					pMobilePrev= Coordinates.Offset(pMobilePrev, mpMulti.ToPoint());
					
									if (!mobilePrev.Equals(mobileShape)) {
					_graphic.FillEllipse(new SolidBrush(GraphicsCfger.roadColor), pMobilePrev.X, pMobilePrev.Y ,iWidth, iWidth);
			
					}
					//cover old track
					
//					graphic.FillEllipse(new SolidBrush(Color.Red), pDraw.X -iWidth / 2, pDraw.Y - iWidth / 2,iWidth, iWidth);
					//_graphic.FillEllipse(new SolidBrush(mobile.Color), pMobile.X, pMobile.Y ,iWidth, iWidth);
					_graphic.FillEllipse(new SolidBrush(Color.Red), pMobile.X, pMobile.Y ,iWidth, iWidth);

					//debug
					string strMsg = mobile.ID.ToString();//.Shape.Start.ToString()+mobile.Shape.End.ToString();
					
					PointF pF=  Coordinates.Project(mobileShape.ToOxyzPointF(), iWidth);
					_graphic.DrawString(strMsg,new Font("Arial",6),new SolidBrush(Color.Red),pMobile.X,pMobile.Y-30f);
					
//					 strMsg = pMobilePrev.X.ToString();//.Shape.Start.ToString()+mobile.Shape.End.ToString();
//					
//					_graphic.DrawString(strMsg,new Font("Arial",4),new SolidBrush(Color.Red),pMobile.X,pMobile.Y-10f);
//					
				}
				
			}
			

		}


		protected override void SubPerform(ITrafficEntity tVar)
		{
			Graphics g = this.Canvas.CreateGraphics();
			
			//   Rectangle rect = this.DrawEntity(tVar);
			this.DrawXnode(tVar);
			
			PaintMobile(tVar);

			g.Dispose();
		}

		protected override void SubRevoke(ITrafficEntity tVar)
		{
			this.Canvas.Invalidate();
		}
	}


}
