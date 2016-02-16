using System;
using SubSys_SimDriving;
using SubSys_SimDriving.SysSimContext;
using System.Collections;
using System.Collections.Generic;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// 应当实现为单例模式,RoadNetWork 是simContext的一部分
    /// </summary>
    internal class RoadNetWork:TrafficEntity,IRoadNetWork
	{
        /// <summary>
        ///单例模式 防止直接调用接口生成该类,路网的边使用了simContext
        ///路网的节点表使用了simContext
        /// </summary>
        private RoadNetWork() 
        {
            ///邻接矩阵使用的节点使用外部RoadNodeList作为存储介质
            ADNetWork = new AdjacencyTable<int>(this.RoadNodeList);
            //k = new AdjacencyTable<int>(this.SimDrivingContext.RoadNodeList);
        }
        /// <summary>
        /// 静态引用私有引用，只能通过getInstance创建类的实例
        /// </summary>
        private static RoadNetWork _roadNetWork;
        internal static RoadNetWork GetInstance()
        {
            if (_roadNetWork == null)
            {
                System.Threading.Mutex mutext = new System.Threading.Mutex();
                mutext.WaitOne();
                _roadNetWork = new RoadNetWork();
                mutext.Close();
             //   mutext.Dispose();
                mutext = null;
            }
            return _roadNetWork;
        }
        /// <summary>
        /// 边字典使用仿真上下文
        /// </summary>
        private RoadEdgeHashTable RoadEdgeList = new RoadEdgeHashTable();

        private RoadNodeHashTable RoadNodeList = new RoadNodeHashTable();
        /// <summary>
        /// 仅仅是邻接表里面的节点字典使用仿真上下文，边不使用节点内部的新字典
        /// </summary>
        private AdjacencyTable<int> ADNetWork;
        
        EntityIDManager<int> roadIDManager = new IntIDManager();

        private MyPoint _netWorkPosition;

        private EntityType _entityType;

        //RoadNode FindRoadNode(
        #region INetWork 成员
        ICollection IRoadNetWork.RoadEdges 
        {
            get {
                return this.RoadEdgeList.Values;
            }
        }
         ICollection IRoadNetWork.RoadNodes
        {
            get{
                return this.RoadNodeList.Values;
            }
        }
         
         void IRoadNetWork.AddRoadNode(RoadNode value)
        {
            if (value!=null)
            {
                ADNetWork.AddRoadNode(value.GetHashCode(), value);
            }
        }
         void IRoadNetWork.RemoveRoadNode(RoadNode value)
        {
            if (value != null)
            {
                ADNetWork.RemoveRoadNode(value.GetHashCode());
            }
            else 
            {
                throw new ArgumentNullException();
            }
        }
         RoadNode IRoadNetWork.FindRoadNode(RoadNode roadNode)
         {
             if (roadNode == null)
             {
                 throw new ArgumentNullException("参数不能为Null");
             }
             if (ADNetWork.Contains(roadNode.GetHashCode()))
             {
                 return ADNetWork.Find(roadNode.GetHashCode());
             }
             return null;
         }
 

         void IRoadNetWork.AddRoadEdge(RoadNode fromRoadNode, RoadNode ToRoadNode)
        {
            if (fromRoadNode != null && ToRoadNode != null)
            {
                if ((this as IRoadNetWork).FindRoadNode(fromRoadNode) != null && (this as IRoadNetWork).FindRoadNode(ToRoadNode) != null)
                {
                    //创建边
                    RoadEdge re = new RoadEdge(fromRoadNode, ToRoadNode);
                    //将RoadEdge添加到仿真上下文han字典
                    this.RoadEdgeList.Add(re.GetHashCode(), re);
                    //将边添加到添加邻接矩阵网络中
                    ADNetWork.AddDirectedEdge(fromRoadNode.GetHashCode(), re);
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
         void IRoadNetWork.AddRoadEdge(RoadEdge re)
    {
        if (re.from == null || re.to != null)
        {
            if ((this as IRoadNetWork).FindRoadNode(re.from) != null && (this as IRoadNetWork).FindRoadNode(re.from) != null)
            {
                //将RoadEdge添加到字典
                this.RoadEdgeList.Add(re.GetHashCode(), re);
                //将边添加到添加邻接矩阵网络中
                ADNetWork.AddDirectedEdge(re.from.GetHashCode(), re);
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
         void IRoadNetWork.RemoveRoadEdge(RoadNode fromRoadNode, RoadNode ToRoadNode)
        {
            if (fromRoadNode != null && ToRoadNode != null)
            {
                RoadEdge re = new RoadEdge(fromRoadNode, ToRoadNode);
                //邻接矩阵中删除边
                ADNetWork.RemoveDirectedEdge(fromRoadNode.GetHashCode(), new RoadEdge(fromRoadNode, ToRoadNode));
                //上下文路段字典中删除边
                this.RoadEdgeList.Remove(re.GetHashCode());
            }
        }
         
         RoadEdge IRoadNetWork.FindRoadEdge(RoadNode from, RoadNode to)
    {
        if (from != null && to != null)
        {
            //找到内部的哈希表对应的该节点
            RoadNode fromRN = (this as IRoadNetWork).FindRoadNode(from);
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
        
         
        int IRoadNetWork.RoadNodeCount
        {
            get { return ADNetWork.RoadNodeCount; }
        }
        int IRoadNetWork.RoadEdgeCount
        {
            get { return RoadEdgeList.Count; }
        }

         #endregion
    }
	 
}
 
