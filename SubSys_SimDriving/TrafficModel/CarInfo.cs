using System.Collections.Generic;
using System.Drawing;

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// 程序的GUI可能要求使用对象的坐标来查询路段顶点的位置，采用对象的position用作
    /// </summary>
    /// <typeparam name="T">int</typeparam>
    public class CarInfo
    {
        public Point Position;//车辆的在当前时间下得时空位置
        public int iSpeed;//车辆的在当前时间下的速度
        public int iTimeStep;//仿真的时间步长记录
        /// <summary>
        /// 车辆的哈希表
        /// </summary>
        public int iCarHashCode;//便于根据记录查询并且找到实体类
        public int iCarNum;
        public int iContainerHashCode;//便于根据记录查询并且找到实体类
        public int iAcc;//当前时间步长下的车辆加速度
        public int iLaneRank; //车道的类型
        public int iPos; //车辆在车道中的位置
    }

   
   
}