using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.SysSimContext.Service;

namespace SubSys_Graphics
{
    public enum PaintServiceType
	{
	    RoadEdge,
        RoadNode,
        RoadLane,
	}
    public class PainterManager 
    {
        private static Dictionary<int, IPainter> servicePool = new Dictionary<int, IPainter>();
        public static IPainter GetService(PaintServiceType pst,Control canvas)
        {
            IPainter ips;
             if (servicePool.ContainsKey((int)pst)==true)
	        {
		        servicePool.TryGetValue((int)pst,out ips);
                ips.Canvas = canvas; 
                return ips;
	        }

            switch (pst)
            {
                case PaintServiceType.RoadEdge:
                        ips = new RoadEdgePainter(canvas);
                        servicePool.Add((int)pst,ips);
                        return ips;
                case PaintServiceType.RoadNode:
                        ips = new RoadNodePainter(canvas);
                        servicePool.Add((int)pst,ips);
                        return ips;
                case PaintServiceType.RoadLane:
                        ips = new RoadLanePainter(canvas);
                        servicePool.Add((int)pst, ips);
                        return ips;
                default:
                        throw new NotImplementedException("没有请求类型的画图服务");
            }
            
        }

    }
   
}
