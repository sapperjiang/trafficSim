using System.Threading;
using SubSys_SimDriving.TrafficModel;
using System.Collections.Generic;
using SubSys_SimDriving;
using SubSys_SimDriving;

namespace SubSys_SimDriving
{
    public class SimSettings
    {
        /// <summary>
        /// 交叉口的最大元胞数，等于Roadedge内部可以容纳的最大车道数量
        /// </summary>
        public static int iMaxLanes = 6;
 
        public static int iMaxNodeWidth = 12;//交叉口最大的宽度
        /// <summary>
        /// 元胞车辆的宽度是2米这个与GIS坐标进行转化的时候使用
        /// </summary>
        public static int iCellWidth = 2;
        /// <summary>
        /// 安全车头时距
        /// </summary>
        public static int iSafeHeadWay = 1;
        /// <summary>
        /// 展宽渐变段的长度为10个元胞，60米
        /// </summary>
        public static int iExtendLength = (int)(60/iCellWidth);

        /// <summary>
        /// 表示标准小汽车宽度的元胞个数
        /// </summary>
        public static int iCarWidth = 1;

        /// <summary>
        /// 表示标准小汽车长度的元胞个数
        /// </summary>
        public static int iCarLength = 1;
    }

}
 
