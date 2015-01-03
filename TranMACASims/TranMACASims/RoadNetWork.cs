using System;
using SubSys_SimDriving;
using SubSys_SimDriving.SysSimDrivingContext;
using System.Collections.Generic;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.TrafficModel
{
    public class RoadNetWork:TrafficEntity,IRoadNetWork
	{
        /// <summary>
        /// 边字典使用仿真上下文
        /// </summary>
        private Dictionary<int, RoadEdge> dicRoadEdge;// = new Dictionary<int, RoadEdge>();

        /// <summary>
        /// 仅仅是邻接表里面的节点字典使用仿真上下文，边不使用节点内部的新字典
        /// </summary>
        private AdjacencyTable<int> adlistNetWork;
        
        EntityIDManager<int> roadIDManager = new IntIDManager();

        public RoadNetWork() 
        {
            //使用上下文哈希表
            dicRoadEdge = this.SimDrivingContext._roadEdgeHashTable;
            adlistNetWork= new AdjacencyTable<int>(this.SimDrivingContext._roadNodeHashTable);
        }

        private MyPoint _netWorkPosition;

        private EntityType _entityType;
      
        //#region ITrafficEntity 成员
        //public EntityType EntityType
        //{
        //    get
        //    {
        //        return this._entityType;
        //    }
        //    set
        //    {
        //        this._entityType = value;
        //    }
        //}

    

        //public EntityStatus EntityStatus
        //{
        //    get
        //    {
        //        throw new System.NotImplementedException();
        //    }
        //    set
        //    {
        //        throw new System.NotImplementedException();
        //    }
        //}

        //public MyPoint Postion
        //{
        //    get
        //    {
        //        return this._netWorkPosition;
        //    }
        //    set
        //    {
        //        this._netWorkPosition = value;
        //    }
        //}

            //#endregion

        #region INetWork 成员
         public void AddRoadNode(RoadNode value)
        {
            if (value!=null)
            {
                adlistNetWork.AddRoadNode(value.GetHashCode(), value);
            }
        }
         public void RemoveRoadNode(RoadNode value)
        {
            if (value != null)
            {
                adlistNetWork.RemoveRoadNode(value.GetHashCode());
            }
            else 
            {
                throw new ArgumentNullException();
            }
        }
         public void AddRoadEdge(RoadNode fromRoadNode, RoadNode ToRoadNode)
        {
            if (fromRoadNode != null && ToRoadNode != null)
            {
                if (this.FindRoadNode(fromRoadNode) != null && this.FindRoadNode(ToRoadNode) != null)
                {
                    //创建边
                    RoadEdge re = new RoadEdge(fromRoadNode, ToRoadNode);
                    //将RoadEdge添加到仿真上下文han字典
                    this.dicRoadEdge.Add(re.GetHashCode(), re);
                    //将边添加到添加邻接矩阵网络中
                    adlistNetWork.AddDirectedEdge(fromRoadNode.GetHashCode(), re);
                }
                else
                {
                    throw new ArgumentException("没有在网络中添加创建道路边的节点");
                }
            }
            else
            {
                throw new ArgumentNullException("无法用空节点添加边");
            }
        }
         public void AddRoadEdge(RoadEdge re)
    {
        if (re.rnFrom == null || re.rnTo != null)
        {
            if (this.FindRoadNode(re.rnFrom) != null && this.FindRoadNode(re.rnFrom) != null)
            {
                //将RoadEdge添加到字典
                this.dicRoadEdge.Add(re.GetHashCode(), re);
                //将边添加到添加邻接矩阵网络中
                adlistNetWork.AddDirectedEdge(re.rnFrom.GetHashCode(), re);
            }
            else
            {
                throw new ArgumentException("没有在网络中添加创建道路边的节点");
            }

        }
        else
        {
            throw new ArgumentNullException("无法用空节点添加边");
        }
    }
         public void RemoveEdge(RoadNode fromRoadNode, RoadNode ToRoadNode)
        {
            if (fromRoadNode != null && ToRoadNode != null)
            {
                RoadEdge re = new RoadEdge(fromRoadNode, ToRoadNode);
                //邻接矩阵中删除边
                adlistNetWork.RemoveDirectedEdge(fromRoadNode.GetHashCode(), new RoadEdge(fromRoadNode, ToRoadNode));
                //上下文路段字典中删除边
                this.dicRoadEdge.Remove(re.GetHashCode());
            }
        }
         public RoadEdge FindRoadEdge(RoadNode from, RoadNode to)
    {
        if (from != null && to != null)
        {
            //找到内部的哈希表对应的该节点
            RoadNode fromRN = this.FindRoadNode(from);
            if (fromRN != null)
            {//查询用户的请求
                return fromRN.FindRoadEdge(to);
            }
            return null;
        }
        else
        {
            throw new ArgumentNullException();
        }
    }
         public RoadNode FindRoadNode(RoadNode roadNode)
    {
        if (roadNode ==null)
        {
            throw new ArgumentNullException("参数不能为Null");
        }
        if(adlistNetWork.Contains(roadNode.GetHashCode()))
        {
            return adlistNetWork.Find(roadNode.GetHashCode());
        }
        return null;
    }
         public int RoadNodeCount
        {
            get { return adlistNetWork.RoadNodeCount; }
        }
         public int RoadEdgeCount
        {
            get { return dicRoadEdge.Count;}
        }

         #endregion

    }
	 
}
 
