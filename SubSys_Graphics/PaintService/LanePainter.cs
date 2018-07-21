using System;
using System.Drawing;

using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;
using SubSys_MathUtility;
using System.Collections.Generic;
using System.Drawing.Drawing2D;

namespace SubSys_Graphics
{
    /// <summary>
    /// 到此开始启用装饰者模式,首先实现一个最基本的RoadLanePainter
    /// 然后启用装饰着来实现各种不同的车道类型；
    /// 还要实现一个工厂来根据参数创建不同的车道类型
    /// </summary>
    internal class LanePainter : AbstractPainter
	{
		Brush laneBrush = new SolidBrush(GraphicsSetter.roadColor);
		internal LanePainter(System.Windows.Forms.Control canvas)
		{
			this.Canvas = canvas;
			this._graphic = canvas.CreateGraphics();
			this.IsRunning = true;
		}
		#region
		
		
		//protected override void SubPerform(ITrafficEntity tVar)
		//{
		//	Graphics g = this.Canvas.CreateGraphics();
		//	var lane = tVar as Lane;
		//	if (lane == null)
		//	{
		//		throw new Exception(string.Format("TrafficEntity类型为{0},使用了错误的绘图服务",tVar.GetType().ToString()));
		//	}

		//	OxyzPointF spStart = lane.Shape.Start;
		//	OxyzPointF spEnd = lane.Shape.End;

		//	//获取单位平移向量(法向量)
		//	var vtUnit = VectorTool.GetNormal(lane.ToVector());
		//	//获取平移向量
		//	var vtMulti =VectorTool.GetMultiNormal(vtUnit,lane.Rank-1);
			
		//	var liUpper = new List<PointF>(lane.Shape.Count*2);
		//	var stDown = new Stack<PointF>(lane.Shape.Count);
			
		//	int iWidth = GraphicsSetter.iPixels;
		//	var UpRightVector = new OxyzPointF(1f,-1f,0f);//offset
		//	foreach (var pShape in lane.Shape) {
				
		//		var pUpper = Coordinates.Offset(pShape,vtMulti);
		//		//move down for one unit
		//		var pDown = Coordinates.Offset(pUpper,vtUnit);
				
		//		var spUpper = Coordinates.Project(pUpper, iWidth);
		//		spUpper =Coordinates.Offset(spUpper,iWidth/2,UpRightVector);//cell offset
				
		//		var spDown = Coordinates.Project(pDown, iWidth);
		//		spDown =Coordinates.Offset(spDown,iWidth/2,UpRightVector);
				
		//		liUpper.Add(spUpper.ToPointF());
		//		stDown.Push(spDown.ToPointF());
		//	}
		//	while (stDown.Count>0) {
		//		liUpper.Add(stDown.Pop());
		//	}

		//	PointF[]  p= liUpper.ToArray();
		//	g.FillClosedCurve(laneBrush, p);
			
		//	MobilePainter.Paint(lane.Mobiles,_graphic);//画车
			
		//	g.Dispose();
		//}


        protected override void SubPerform(IEntity la)
        {
            var lane = la as Lane;
            int x1 = 0, y1 = 0;     // previous point
            int x2, y2;             // current point

            Graphics g = this.Canvas.CreateGraphics();
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int penWidth = GraphicsSetter.iPixels;
            Pen pen = new Pen(GraphicsSetter.roadColor, penWidth);

            IEnumerator<OxyzPointF> enumerator = lane.Shape.GetEnumerator();
            Point temp;
            //OxyzPointF basePoint;
         
            if (enumerator.MoveNext())
            {
                //basePoint = Coordinates.Offset( enumerator.Current, GraphicsSetter.baseOffset);
                //temp = Coordinates.Project(basePoint, penWidth).ToPoint();
                temp = Coordinates.Project(enumerator.Current, penWidth).ToPoint();
                x1 = temp.X;
                y1 = temp.Y;
            }

            while (enumerator.MoveNext())
            {
                //basePoint = Coordinates.Offset(enumerator.Current, GraphicsSetter.baseOffset);
                //temp = Coordinates.Project(basePoint, penWidth).ToPoint();
                temp = Coordinates.Project(enumerator.Current, penWidth).ToPoint();
                x2 = temp.X;
                y2 = temp.Y;

                g.DrawLine(pen, x1, y1, x2, y2);

                x1 = x2;
                y1 = y2;
            }

            MobilePainter.Paint(lane.Mobiles, _graphic);//画车
            pen.Dispose();
            g.Dispose();
        }




        protected override void SubRevoke(IEntity tVar)
		{
			throw new NotImplementedException();
		}
	}
	
	public class MobilePainter
	{
		internal static void Paint(LinkedList<MobileOBJ> mobiles,Graphics _graphic)
		{
			int iWidth = GraphicsSetter.iPixels;
			
			foreach (var mobile in mobiles)
			{
				
				for (int i = 0; i < mobile.Shape.Count; i++) {

					var mobilePrev = mobile.PrevShape[i];
					var mobileShape = mobile.Shape[i];
					
					OxyzPointF opCurr = Coordinates.Project(mobileShape, iWidth);
					OxyzPointF opPrev = Coordinates.Project(mobilePrev, iWidth);

                    //up left offset under screen coordinates
                    PointF pfPrev = Coordinates.Offset(opPrev, iWidth / 2).ToPointF();//move upleft for a step.
                    PointF pfCurr = Coordinates.Offset(opCurr, iWidth / 2).ToPointF();//move upleft for a step.

                    //PointF pfPrev = Coordinates.Offset(ofPrev, GraphicsSetter.baseOffset).ToPointF();//move upleft for a step.
                    //PointF pfCurr = Coordinates.Offset(ofCurr, GraphicsSetter.baseOffset).ToPointF();//move upleft for a step.

                    if (!mobilePrev.Equals(mobileShape)) {//cover old track
						_graphic.FillEllipse(new SolidBrush(GraphicsSetter.roadColor), pfPrev.X, pfPrev.Y ,iWidth, iWidth);
					}
                    //_graphic.draw
                    _graphic.FillEllipse(new SolidBrush(mobile.Color), pfCurr.X, pfCurr.Y ,iWidth, iWidth);
					//for debug
//					string strMsg="CurrX:"+opCurr._X.ToString()+":PrevX"+opPrev._X.ToString();
//					strMsg+="Mxy:"+mobile.Track.Current._X.ToString()+mobile.Track.Current._Y.ToString();
//					//	strMsg+="CtnerX:"+mobile.Container.Shape.Start._X.ToString();
//					PointF pF=  Coordinates.Project(mobileShape, iWidth);
//					_graphic.DrawString(strMsg,new Font("Arial",6),new SolidBrush(Color.Red),pfCurr.X+30f,pfCurr.Y);
				}
			}
		}
		
	}

	#endregion
}
