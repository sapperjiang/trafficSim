using System;
using System.Collections.Generic;
using System.Text;
using SubSys_SimDriving.TrafficModel;
using System.Drawing;

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// 路段接口，包括两个或者一个RoadEdge。两个RoadNode,
    /// 他们之间的关系由工厂创建时候负责维护
    /// </summary>
    public  interface IRoad
    {
        RoadEdge RoadEdge
        {
            get;
            set;
        }
        /// <summary>
        /// 对向道路
        /// </summary>
        RoadEdge CtrRoadEdge
        {
            get;
            set;
        }
        RoadNode RoadNode
        {
            get;
            set;
        }    
    /// <summary>
        /// 对向节点
        /// </summary>
         RoadNode CtrRoadNode
        {
            get;
            set;
        }    
    }
    public class Road:TrafficEntity,IRoad
    {
        private RoadEdge _roadEdge;
        private RoadEdge _ctrRoadEdge;
        private RoadNode _roadNode;
        private RoadNode _ctrRoadNode;

        public Road()
        {
            this.EntityType = EntityType.Road;
        }
        public RoadEdge RoadEdge
        {
            get
            {
                return this._roadEdge;
            }
            set
            {
                this._roadEdge = value;
            }
        }

        public RoadEdge CtrRoadEdge
        {
            get
            {
                return this._ctrRoadEdge;
            }
            set
            {
                this._ctrRoadEdge = value;
            }
        }

        public RoadNode RoadNode
        {
            get
            {
                return this._roadNode;
            }
            set
            {
                this._roadNode = value;
            }
        }

        public RoadNode CtrRoadNode
        {
            get
            {
                return this._ctrRoadNode;
            }
            set
            {
                this._ctrRoadNode = value;
            }
        }
    }
}
