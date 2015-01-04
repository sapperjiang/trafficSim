using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using SubSys_MathUtility;
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.SysSimContext.Service;

namespace SubSys_Graphics
{
    /// <summary>
    /// 调用RoadLane来绘制自身
    /// </summary>
    public class RoadEdgePaintService : PaintService
    {
        public RoadEdgePaintService(System.Windows.Forms.Control fr)
        {
            this.Canvas = fr;
            this.graphic = fr.CreateGraphics();
            this.IsRunning = true;
        }
 
        protected override void SubPerform(ITrafficEntity tVar)
        {
            IService rp = PaintServiceMgr.GetService(PaintServiceType.RoadLane, this.Canvas);
            RoadEdge re = tVar as RoadEdge;

            //先画一个车道
            foreach (var lane in re.Lanes)
            {
                rp.Perform(lane);
            }
            
           //画一个双黄线
            Point pa = Coordinates.Project(re.Shape.Start.ToPoint(), GraphicsConfiger.iCellPixels);            
            Point pB = Coordinates.Project(re.Shape.End.ToPoint(), GraphicsConfiger.iCellPixels);
            
            graphic.DrawLine(new Pen(new SolidBrush(Color.Yellow),2), pa, pB);

        }

        protected override void SubRevoke(ITrafficEntity tVar)
        {
            this.Canvas.Invalidate();
        }
    }


}
