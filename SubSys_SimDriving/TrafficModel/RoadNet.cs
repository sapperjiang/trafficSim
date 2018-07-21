using System;
using SubSys_SimDriving;
using SubSys_SimDriving;
using System.Collections.Generic;
using SubSys_MathUtility;
using System.Drawing;

namespace SubSys_SimDriving.TrafficModel
{
	/// <summary>
	/// 应当实现为单例模式,RoadNetWork 是simContext的一部分
	/// RoadNetWork 应当承承担路网节点工厂的责任
	/// </summary>
	public class RoadNet:TrafficOBJ,IRoadNet
	{
		public static int iRoadNetCount = 0;
		/// <summary>
		///单例模式 防止直接调用接口生成该类,路网的边使用了simContext
		///路网的节点表使用了simContext
		/// </summary>
		private RoadNet()
		{
			iRoadNetCount += 1;
			///邻接矩阵使用的节点使用外部XNodeList作为存储介质
			adTab = new AdjacencyList<int>(this.xnodes);
		}
		/// <summary>
		/// 静态引用私有引用，只能通过getInstance创建类的实例
		/// </summary>
		private static RoadNet _roadNet;
		public static RoadNet GetInstance()
		{
			if (_roadNet == null)
			{
				//防止多线程创建了多个实例
				System.Threading.Mutex mutext = new System.Threading.Mutex();
				mutext.WaitOne();
				_roadNet = new RoadNet();
				_roadNet.EntityType = EntityType.RoadNet;
				mutext.Close();
				mutext = null;
			}
			return RoadNet._roadNet;
		}
		/// <summary>
		/// 边字典使用仿真上下文
		/// </summary>
		internal WayDics ways = new WayDics();

		internal XNodeDics xnodes = new XNodeDics();
		
		internal RoadDics roads = new RoadDics();
		

		/// <summary>
		/// 获取所有的车道是否有必要，因为该部分已经存在了RoadEdge中了
		/// </summary>
		internal LaneDics lanes = new LaneDics();


		/// <summary>
		/// 仅仅是邻接表里面的节点字典使用仿真上下文
		/// </summary>
		private AdjacencyList<int> adTab;
		
		//private EntityType _entityType;
		
		#region INetWork 成员
		public ICollection<Way> Ways
		{
			get {
				return this.ways.Values;
			}
		}

        public ICollection<XNode> XNodes
		{
			get{
				return this.xnodes.Values;
			}
		}
		
		public  void AddXNode(XNode value)
		{
			if (value!=null)
			{			
				adTab.AddXNode(value.GetHashCode(), value);
			}
		}
		public void RemoveXNode(XNode value)
		{
			if (value != null)
			{
				adTab.RemoveXNode(value.GetHashCode());//已经删除了节点
			}
			else
			{
				throw new ArgumentNullException();
			}
		}
		public XNode FindXNode(XNode roadNode)
		{
			int i = this.XNodes.Count;
			//bool b = object.ReferenceEquals(this, SimCtx.NetWork);
			//bool c = object.ReferenceEquals(this.ADNetWork, SimCtx.NetWork.ADNetWork);

			if (roadNode == null)
			{
				throw new ArgumentNullException("参数不能为Null");
			}
			if (adTab.Contains(roadNode.GetHashCode()))
			{
				return adTab.Find(roadNode.GetHashCode());
			}
			return null;
		}
		

		public void AddWay(Way way)
		{
			if (this.FindXNode(way.From) != null && this.FindXNode(way.From) != null)
			{
				//将边添加到添加邻接矩阵网络中
				adTab.AddDirectedEdge(way.From.GetHashCode(), way);
			}
			else
			{
				ThrowHelper.ThrowArgumentException("没有在网络中添加创建道路边的节点，节点没有注册");
			}
		}
	
		[System.Obsolete("这个删除函数可能有问题")]
		public void RemoveWay(XNode from, XNode to)
		{
			if (from != null && to != null)
			{
				Way re = this.FindWay(from,to);
				//邻接矩阵中删除边
				adTab.RemoveDirectedEdge(from.GetHashCode(), re);
		//		re.UnRegiser();//解除注册
			}
		}
		
		public Way FindWay(XNode from, XNode to)
		{
			if (from != null && to != null)
			{
				//找到内部的哈希表对应的该节点
				XNode fromRN = this.FindXNode(from);
				if (fromRN != null)
				{//查询用户的请求
					//System.Diagnostics.Debug.Assert(fromRN.FindRoadEdge(to) != this.RoadEdgeList[RoadEdge.GetHashCode(from, to)]);
					return fromRN.FindWay(to);
				}
				ThrowHelper.ThrowArgumentNullException("参数不能为零");
				
			}
			throw new ArgumentNullException("参数不能为零");
		}
		
		#endregion

		public event UpdateHandler Updated;


		private int _iCurrTimeStep;
		public int iTimePulse
		{
			get
			{
				return this._iCurrTimeStep;
			}
			set
			{
				this._iCurrTimeStep = value;
				//调用委托
				//this.OnUpdateCompleted();
			}
		}
		private void OnUpdateCompleted()
		{
			if (_lsHandlers == null)
			{
				_lsHandlers = new List<UpdateHandler>();
			}
			foreach (var handler in _lsHandlers)
			{//调用委托的方法
				handler();
			}
		}

		private List<UpdateHandler> _lsHandlers;

		event UpdateHandler IRoadNet.Updated
		{
			add
			{
				if (_lsHandlers == null)
				{
					_lsHandlers = new List<UpdateHandler>();
				}
				_lsHandlers.Add(value);
			}
			remove
			{
				_lsHandlers.Remove(value);
			}
		}


		ICollection<Lane> IRoadNet.Lanes
		{
			get { return this.lanes.Values; }
		}


		public Way FindWay(int reKey)
		{
			Way re ;
			this.ways.TryGetValue(reKey, out re);
			return re;
		}

        public bool ModifyXNode(XNode old, XNode New)
        {
            throw new NotImplementedException();
        }

         public Way BulidWay(OxyzPointF start, OxyzPointF end)
        {
            return RoadNet.GetInstance().BulidEntity(start,end,EntityType.Way) as Way;
        }


        public Way BuildWay(Point start, Point end)
        {
            return this.BulidWay(new OxyzPointF(start), new OxyzPointF(end));
        }

        /// <summary>
        /// 创建一个双向way 的Road，
        /// </summary>
        /// <param name="start">正向way的起点，反向道路的终点</param>
        /// <param name="end">反向way的起点，反向道路的终点</param>
        /// <returns></returns>
        public Road BulidRoad(Point start, Point end)
        {
            return this.BulidEntity(new OxyzPointF(start), new OxyzPointF(end),EntityType.Road) as Road;
        }

        public StaticOBJ BulidEntity(OxyzPointF start, OxyzPointF end, EntityType et)
        {
            IStaticFactory IFacotry = new StaticFactory();
            return IFacotry.Build(start, end, et);
        }
    }
	
}

