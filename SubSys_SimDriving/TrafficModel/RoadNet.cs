using System;
using SubSys_SimDriving;
using SubSys_SimDriving;
using System.Collections.Generic;
using SubSys_MathUtility;
using System.Drawing;

namespace SubSys_SimDriving.TrafficModel
{
	/// <summary>
	/// Ӧ��ʵ��Ϊ����ģʽ,RoadNetWork ��simContext��һ����
	/// RoadNetWork Ӧ���ге�·���ڵ㹤��������
	/// </summary>
	public class RoadNet:TrafficOBJ,IRoadNet
	{
		public static int iRoadNetCount = 0;
		/// <summary>
		///����ģʽ ��ֱֹ�ӵ��ýӿ����ɸ���,·���ı�ʹ����simContext
		///·���Ľڵ��ʹ����simContext
		/// </summary>
		private RoadNet()
		{
			iRoadNetCount += 1;
			///�ڽӾ���ʹ�õĽڵ�ʹ���ⲿXNodeList��Ϊ�洢����
			adTab = new AdjacencyList<int>(this.xnodes);
		}
		/// <summary>
		/// ��̬����˽�����ã�ֻ��ͨ��getInstance�������ʵ��
		/// </summary>
		private static RoadNet _roadNet;
		public static RoadNet GetInstance()
		{
			if (_roadNet == null)
			{
				//��ֹ���̴߳����˶��ʵ��
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
		/// ���ֵ�ʹ�÷���������
		/// </summary>
		internal WayDics ways = new WayDics();

		internal XNodeDics xnodes = new XNodeDics();
		
		internal RoadDics roads = new RoadDics();
		

		/// <summary>
		/// ��ȡ���еĳ����Ƿ��б�Ҫ����Ϊ�ò����Ѿ�������RoadEdge����
		/// </summary>
		internal LaneDics lanes = new LaneDics();


		/// <summary>
		/// �������ڽӱ�����Ľڵ��ֵ�ʹ�÷���������
		/// </summary>
		private AdjacencyList<int> adTab;
		
		//private EntityType _entityType;
		
		#region INetWork ��Ա
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
				adTab.RemoveXNode(value.GetHashCode());//�Ѿ�ɾ���˽ڵ�
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
				throw new ArgumentNullException("��������ΪNull");
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
				//������ӵ�����ڽӾ���������
				adTab.AddDirectedEdge(way.From.GetHashCode(), way);
			}
			else
			{
				ThrowHelper.ThrowArgumentException("û������������Ӵ�����·�ߵĽڵ㣬�ڵ�û��ע��");
			}
		}
	
		[System.Obsolete("���ɾ����������������")]
		public void RemoveWay(XNode from, XNode to)
		{
			if (from != null && to != null)
			{
				Way re = this.FindWay(from,to);
				//�ڽӾ�����ɾ����
				adTab.RemoveDirectedEdge(from.GetHashCode(), re);
		//		re.UnRegiser();//���ע��
			}
		}
		
		public Way FindWay(XNode from, XNode to)
		{
			if (from != null && to != null)
			{
				//�ҵ��ڲ��Ĺ�ϣ���Ӧ�ĸýڵ�
				XNode fromRN = this.FindXNode(from);
				if (fromRN != null)
				{//��ѯ�û�������
					//System.Diagnostics.Debug.Assert(fromRN.FindRoadEdge(to) != this.RoadEdgeList[RoadEdge.GetHashCode(from, to)]);
					return fromRN.FindWay(to);
				}
				ThrowHelper.ThrowArgumentNullException("��������Ϊ��");
				
			}
			throw new ArgumentNullException("��������Ϊ��");
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
				//����ί��
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
			{//����ί�еķ���
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
        /// ����һ��˫��way ��Road��
        /// </summary>
        /// <param name="start">����way����㣬�����·���յ�</param>
        /// <param name="end">����way����㣬�����·���յ�</param>
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

