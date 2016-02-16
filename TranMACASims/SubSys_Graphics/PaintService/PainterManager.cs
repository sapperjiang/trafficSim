using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving;
using SubSys_SimDriving.Service;

namespace SubSys_Graphics
{
    public enum PaintServiceType
	{
	    Way,
        XNode,
        Lane,
	}
    public static class PainterManager 
    {
        private static Dictionary<int, IPainter> _servicePool = new Dictionary<int, IPainter>();
        public static IPainter GetService(PaintServiceType pst,Control canvas)
        {
            IPainter ips;
             if (_servicePool.ContainsKey((int)pst)==true)
	        {
		        _servicePool.TryGetValue((int)pst,out ips);
                ips.Canvas = canvas; 
                return ips;
	        }

            switch (pst)
            {
                case PaintServiceType.Way:
                        ips = new WayPainter(canvas);
                        _servicePool.Add((int)pst,ips);
                        return ips;
                case PaintServiceType.XNode:
                        ips = new XNodePainter(canvas);
                        _servicePool.Add((int)pst,ips);
                        return ips;
                case PaintServiceType.Lane:
                        ips = new LanePainter(canvas);
                        _servicePool.Add((int)pst, ips);
                        return ips;
                default:
                        throw new NotImplementedException("没有请求类型的画图服务");
            }
            
        }

    }
   
}
