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
    public class PaintServiceMgr 
    {
        private static Dictionary<int, IPaintService> servicePool = new Dictionary<int, IPaintService>();
        public static IPaintService GetService(PaintServiceType pst,Control canvas)
        {
            IPaintService ips;
             if (servicePool.ContainsKey((int)pst)==true)
	        {
		        servicePool.TryGetValue((int)pst,out ips);
                ips.Canvas = canvas; 
                return ips;
	        }

            switch (pst)
            {
                case PaintServiceType.RoadEdge:
                        ips = new RoadEdgePaintService(canvas);
                        servicePool.Add((int)pst,ips);
                        return ips;
                case PaintServiceType.RoadNode:
                        ips = new RoadNodePaintService(canvas);
                        servicePool.Add((int)pst,ips);
                        return ips;
                case PaintServiceType.RoadLane:
                        ips = new RoadLanePaintService(canvas);
                        servicePool.Add((int)pst, ips);
                        return ips;
                default:
                        throw new NotImplementedException("没有请求类型的画图服务");
            }
            
        }

    }
   
}
