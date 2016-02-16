using System;
using System.Collections.Generic;
using System.Text;

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// 路段接口，包括两个或者一个RoadEdge。两个RoadNode,
    /// 他们之间的关系由工厂创建时候负责维护
    /// </summary>
    internal  class Road
    {
        internal  RoadEdge roadEdge;
        /// <summary>
        /// 对向道路
        /// </summary>
        internal  RoadEdge ctrRoadEdge;

        internal  RoadNode roadNode ;
        /// <summary>
        /// 对向节点
        /// </summary>
        internal  RoadNode ctrRoadNode;
    }
}
