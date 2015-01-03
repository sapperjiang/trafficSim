using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.RoutePlan;

namespace SubSys_DataManage
{
    public class CarSimulator
    {

        static ISimContext isc = SimContext.GetInstance();

        internal static Color GetRandomColor()
        {
            Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
            //  对于C#的随机数，没什么好说的
            System.Threading.Thread.Sleep(RandomNum_First.Next(50));
            Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);
            //  为了在白色背景上显示，尽量生成深色
            int int_Red = RandomNum_First.Next(256);
            int int_Green = RandomNum_Sencond.Next(256);
            int int_Blue = (int_Red + int_Green > 400) ? 0 : 400 - int_Red - int_Green;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;
            return Color.FromArgb(int_Red, int_Green, int_Blue);
        }

        internal static Car Make(EdgeRoute er)
        {
            Car cm3 = new Car();
            cm3.Color = CarSimulator.GetRandomColor();
            cm3.EdgeRoute = er;
            return cm3;
        }
        
        public static Cell MakeCell(EdgeRoute er)
        {
            return new Cell(CarSimulator.Make(er));
        }
    }
}
