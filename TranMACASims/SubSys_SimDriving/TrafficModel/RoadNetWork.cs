using System;
using SubSys_SimDriving;
using SubSys_SimDriving.SysSimContext;
using System.Collections.Generic;
using SubSys_SimDriving.TrafficModel;
using SubSys_MathUtility;

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// 应当实现为单例模式,RoadNetWork 是simContext的一部分
    /// RoadNetWork 应当承承担路网节点工厂的责任
    /// </summary>
    public class RoadNetWork:TrafficEntity,IRoadNetWork
	{
        public static int iRoadNetWorkCount = 0;
        /// <summary>
        ///单例模式 防止直接调用接口生成该类,路网的边使用了simContext
        ///路网的节点表使用了simContext
        /// </summary>
        private RoadNetWork()
        {
            iRoadNetWorkCount += 1;
            ///邻接矩阵使用的节点使用外部RoadNodeList作为存储介质
            ADNetWork = new AdjacencyTable<int>(this.RoadNodeList);
        }
        /// <summary>
        /// 静态引用私有引用，只能通过getInstance创建类的实例
        /// </summary>
        private static RoadNetWork _roadNetWork;
        public static RoadNetWork GetInstance()
        {
            if (_roadNetWork == null)
            {
                System.Threading.Mutex mutext = new System.Threading.Mutex();
                mutext.WaitOne();
                _roadNetWork = new RoadNetWork();
                _roadNetWork.EntityType = EntityType.RoadNetWork;
                mutext.Close();
                mutext = null;
            }
            return RoadNetWork._roadNetWork;
        }
        /// <summary>
        /// 边字典使用仿真上下文
        /// </summary>
        internal RoadEdgeHTable RoadEdgeList = new RoadEdgeHTable();

        internal RoadNodeHTable RoadNodeList = new RoadNodeHTable();

        /// <summary>
        /// 获取所有的车道是否有必要，因为该部分已经存在了RoadEdge中了
        /// </summary>
        internal RoadLaneHTable RoadLanes = new RoadLaneHTable();


        /// <summary>
        /// 仅仅是邻接表里面的节点字典使用仿真上下文，边不使用节点内部的新字典
        /// </summary>
        private AdjacencyTable<int> ADNetWork;
        
        EntityIDManager<int> roadIDManager = new IntIDManager();

        private MyPoint _netWorkPos;

        private EntityType _entityType;
 
        #region INetWork 成员
        public ICollection<RoadEdge> RoadEdges 
        {
            get {
                return this.RoadEdgeList.Values;
            }
        }
        public ICollection<RoadNode> RoadNodes
        {
            get{
                return this.RoadNodeList.Values;
            }
        }
         
        public  void AddRoadNode(RoadNode value)
        {
            if (value!=null)
            {
               
                ADNetWork.AddRoadNode(value.GetHashCode(), value);
                value.Register();////注册到路网中其他构造函数
            }
        }
         public void RemoveRoadNode(RoadNode value)
        {
            if (value != null)
            {
                ADNetWork.RemoveRoadNode(value.GetHashCode());//已经删除了节点
                value.UnRegiser();//重复删除
            }
            else 
            {
                throw new ArgumentNullException();
            }
        }
          public RoadNode FindRoadNode(RoadNode roadNode)
         {
             int i = this.RoadNodes.Count;
             //bool b = object.ReferenceEquals(this, SimCtx.NetWork);
             //bool c = object.ReferenceEquals(this.ADNetWork, SimCtx.NetWork.ADNetWork);

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
 
        ///// <summary>
        ///// 如果在构造函数里面注册，就会造成路网中使用的RoadNode和注册的roadNode数量不匹配的问题
        ///// </summary>
        ///// <param name="fromRoadNode"></param>
        ///// <param name="ToRoadNode"></param>
        // void IRoadNetWork.AddRoadEdge(RoadNode fromRoadNode, RoadNode ToRoadNode)
        //{
        //    if (fromRoadNode != null && ToRoadNode != null)
        //    {
        //        if ((this as IRoadNetWork).FindRoadNode(fromRoadNode) != null && (this as IRoadNetWork).FindRoadNode(ToRoadNode) != null)
        //        {
        //            //创建边
        //            RoadEdge re = new RoadEdge(fromRoadNode, ToRoadNode);
        //            re.Register();//添加其他的初始化代码
        //            //将RoadEdge添加到仿真上下文han字典
        //            this.RoadEdgeList.Add(re.GetHashCode(), re);

        //            //将边添加到添加邻接矩阵网络中
        //            ADNetWork.AddDirectedEdge(fromRoadNode.GetHashCode(), re);
        //        }
        //        else
        //        {
        //            throw new ArgumentException("没有在网络中添加创建道路边的节点");
        //        }
        //    }
        //    else
        //    {
        //        throw new ArgumentNullException("无法用空节点添加边");
        //    }
        //}
        public void AddRoadEdge(RoadEdge re)
        {
            if (this.FindRoadNode(re.roadNodeFrom) != null && this.FindRoadNode(re.roadNodeFrom) != null)
            {
                re.Register();//将道路边注册
                //将边添加到添加邻接矩阵网络中
                ADNetWork.AddDirectedEdge(re.roadNodeFrom.GetHashCode(), re);
            }
            else
            {
                ThrowHelper.ThrowArgumentException("没有在网络中添加创建道路边的节点，节点没有注册");
            }
        }
        public RoadEdge AddRoadEdge(RoadNode from, RoadNode To)
        {
            RoadEdge re = new RoadEdge(from, To);
            this.AddRoadEdge(re);
            return re;
        }
        [System.Obsolete("这个删除函数可能有问题")]
        public void RemoveRoadEdge(RoadNode from, RoadNode to)
        {
            if (from != null && to != null)
            {
                RoadEdge re = this.FindRoadEdge(from,to);
                //邻接矩阵中删除边
                ADNetWork.RemoveDirectedEdge(from.GetHashCode(), re);
                re.UnRegiser();//解除注册
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
                    //System.Diagnostics.Debug.Assert(fromRN.FindRoadEdge(to) != this.RoadEdgeList[RoadEdge.GetHashCode(from, to)]);
                    return fromRN.FindRoadEdge(to);
                }
                return null;
            }
            throw new ArgumentNullException("参数不能为零");
        }
   
         #endregion

        public event UpdateHandler UpdateCompleted;


        private int _iCurrTimeStep;
        public int iCurrTimeStep
        {
            get
            {
                //throw new NotImplementedException();
                return this._iCurrTimeStep;
            }
            set
            {
                this._iCurrTimeStep = value;

                this.OnUpdateCompleted();//调用委托
            }
        }
        private void OnUpdateCompleted()
        {
            foreach (var handler in handlerList)
            {
                handler();//调用委托的方法
            }
        }

        private List<UpdateHandler> handlerList;

        event UpdateHandler IRoadNetWork.UpdateCompleted
        {
            add 
            {
                if (handlerList == null)
                {
                    handlerList = new List<UpdateHandler>();
                }
                handlerList.Add(value); 
            }
            remove 
            {
                handlerList.Remove(value); 
                //throw new NotImplementedException(); 
            }
        }


        ICollection<RoadLane> IRoadNetWork.RoadLanes
        {
            get { return this.RoadLanes.Values; }
        }


        public RoadEdge FindRoadEdge(int reKey)
        {
            RoadEdge re ;
            this.RoadEdgeList.TryGetValue(reKey, out re);
            return re;
        }
    }
	 
}
 
