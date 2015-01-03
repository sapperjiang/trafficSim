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

            foreach (var lane in re.Lanes)
            {
                rp.Perform(lane);
            }

            Point pa = Coordinates.Project(re.roadNodeFrom.RelativePosition, GUISettings.iGUI_CellPixels);
            Point pB = Coordinates.Project(re.roadNodeTo.RelativePosition, GUISettings.iGUI_CellPixels);
            graphic.DrawLine(new Pen(new SolidBrush(Color.Yellow),2), pa, pB);

        }

        protected override void SubRevoke(ITrafficEntity tVar)
        {
            this.Canvas.Invalidate();
        }
    }


}
