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
        Way Way
        {
            get;
            set;
        }
        /// <summary>
        /// 对向道路
        /// </summary>
        Way CtrWay
        {
            get;
            set;
        }
        XNode RoadNode
        {
            get;
            set;
        }    
        /// <summary>
        /// 对向节点
        /// </summary>
         XNode CtrRoadNode
        {
            get;
            set;
        }    
    }
    /// <summary>
    /// 包含两个way的道路。如果是单行路，只包含一个way
    /// </summary>
    public class Road:TrafficEntity,IRoad
    {
        private Way _roadEdge;
        private Way _ctrRoadEdge;
        private XNode _roadNode;
        private XNode _ctrRoadNode;

        public Road()
        {
            this.EntityType = EntityType.Road;
        }
        public Way Way
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

        public Way CtrWay
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

        public XNode RoadNode
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

        public XNode CtrRoadNode
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
