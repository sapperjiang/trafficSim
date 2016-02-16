
using System;
using System.Collections.Generic;

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// 程序的GUI可能要求使用对象的坐标来查询路段顶点的位置，采用对象的position用作
    /// </summary>
    /// <typeparam name="T">int</typeparam>
    public class CACell
    {
        /// <summary>
        /// 使用链式结构支持快速下游访问
        /// </summary>
        public CACell nextCACell;
        /// <summary>
        /// 代表的车辆模型
        /// </summary>
        public CarModel cmCarModel;
        public int iCarModelID;
        public RoadLane Container;
       
        /// <summary>
        /// 相对于道路起点的偏移，记录车辆的空间信息，运行态哈希表和记录态哈希表
        /// </summary>
        public int iPos;

        /// <summary>
        /// 用作记录态哈希表记录车辆的时间信息，以及用来确定什么时候进入路段
        /// </summary>
        public int iTimeStep;

        public SpeedLevel CurrSpeed;

        /// <summary>
        /// 当前车辆的加速度
        /// </summary>
        public int iAcceleration = 2;

        /// <summary>
        /// 当前车辆的速度
        /// </summary>
        public int iSpeed = 5;

    }
   
}