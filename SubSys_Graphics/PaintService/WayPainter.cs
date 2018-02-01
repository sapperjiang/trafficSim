using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using SubSys_MathUtility;
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.Service;

namespace SubSys_Graphics
{
    /// <summary>
    /// 道路的绘制调用Lane来绘制自身
    /// </summary>
    public class WayPainter : AbstractPainter
    {
        public WayPainter(System.Windows.Forms.Control fr)
        {
            this.Canvas = fr;
            this._graphic = fr.CreateGraphics();
            this.IsRunning = true;
        }
 
        protected override void SubPerform(IEntity tVar)
        {
            IService rp = PainterManager.GetService(PaintServiceType.Lane, this.Canvas);
            Way way = tVar as Way;

            //先画一个车道
            foreach (var lane in way.Lanes)
            {
                rp.Perform(lane);
            }
            
           //画一个双黄线
//           var pa = Coordinates.Project(way.Shape.Start, GraphicsCfger.iPixels);
//            var pB = Coordinates.Project(way.Shape.End, GraphicsCfger.iPixels);
//            
//            _graphic.DrawLine(new Pen(new SolidBrush(Color.Yellow),2), pa, pB);

        }

        protected override void SubRevoke(IEntity tVar)
        {
            this.Canvas.Invalidate();
        }
    }


}
