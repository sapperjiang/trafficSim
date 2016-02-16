using System;
using SubSys_SimDriving;
using SubSys_SimDriving;
using System.Collections.Generic;
using SubSys_SimDriving.TrafficModel;
using SubSys_MathUtility;

namespace SubSys_SimDriving.TrafficModel
{
	/// <summary>
	/// Ӧ��ʵ��Ϊ����ģʽ,RoadNetWork ��simContext��һ����
	/// RoadNetWork Ӧ���ге�·���ڵ㹤��������
	/// </summary>
	public class RoadNet:TrafficEntity,IRoadNet
	{
		public static int iRoadNetCount = 0;
		/// <summary>
		///����ģʽ ��ֱֹ�ӵ��ýӿ����ɸ���,·���ı�ʹ����simContext
		///·���Ľڵ��ʹ����simContext
		/// </summary>
		private RoadNet()
		{
			iRoadNetCount += 1;
			///�ڽӾ���ʹ�õĽڵ�ʹ���ⲿRoadNodeList��Ϊ�洢����
			_atRoadNet = new AdjacencyTable<int>(this.htXNodes);
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
		internal WayHTable htWays = new WayHTable();

		internal RoadNodeHTable htXNodes = new RoadNodeHTable();

		/// <summary>
		/// ��ȡ���еĳ����Ƿ��б�Ҫ����Ϊ�ò����Ѿ�������RoadEdge����
		/// </summary>
		internal LaneHTable htLanes = new LaneHTable();


		/// <summary>
		/// �������ڽӱ�����Ľڵ��ֵ�ʹ�÷��������ģ��߲�ʹ�ýڵ��ڲ������ֵ�
		/// </summary>
		private AdjacencyTable<int> _atRoadNet;
		
		//EntityIDManager<int> _roadIDManager = new IntIDManager();

		//  private MyPoint _netWorkPos;

		private EntityType _entityType;
		
		#region INetWork ��Ա
		public ICollection<Way> Ways
		{
			get {
				return this.htWays.Values;
			}
		}
		public ICollection<XNode> XNodes
		{
			get{
				return this.htXNodes.Values;
			}
		}
		
		public  void AddXNode(XNode value)
		{
			if (value!=null)
			{			
				_atRoadNet.AddRoadNode(value.GetHashCode(), value);
				//ע�ᵽ·�����������캯��
				value.Register();
			}
		}
		public void RemoveXNode(XNode value)
		{
			if (value != null)
			{
				_atRoadNet.RemoveRoadNode(value.GetHashCode());//�Ѿ�ɾ���˽ڵ�
				value.UnRegiser();//�ظ�ɾ��
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
			if (_atRoadNet.Contains(roadNode.GetHashCode()))
			{
				return _atRoadNet.Find(roadNode.GetHashCode());
			}
			return null;
		}
		

		public void AddWay(Way re)
		{
			if (this.FindXNode(re.XNodeFrom) != null && this.FindXNode(re.XNodeFrom) != null)
			{
				re.Register();//����·��ע��
				//������ӵ�����ڽӾ���������
				_atRoadNet.AddDirectedEdge(re.XNodeFrom.GetHashCode(), re);
			}
			else
			{
				ThrowHelper.ThrowArgumentException("û������������Ӵ�����·�ߵĽڵ㣬�ڵ�û��ע��");
			}
		}
		public Way AddWay(XNode from, XNode To)
		{
			Way re = new Way(from, To);
			this.AddWay(re);
			return re;
		}
		[System.Obsolete("���ɾ����������������")]
		public void RemoveWay(XNode from, XNode to)
		{
			if (from != null && to != null)
			{
				Way re = this.FindWay(from,to);
				//�ڽӾ�����ɾ����
				_atRoadNet.RemoveDirectedEdge(from.GetHashCode(), re);
				re.UnRegiser();//���ע��
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
		public int iCurrTimeStep
		{
			get
			{
				return this._iCurrTimeStep;
			}
			set
			{
				this._iCurrTimeStep = value;
				//����ί��
				this.OnUpdateCompleted();
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
			get { return this.htLanes.Values; }
		}


		public Way FindWay(int reKey)
		{
			Way re ;
			this.htWays.TryGetValue(reKey, out re);
			return re;
		}
	}
	
}

