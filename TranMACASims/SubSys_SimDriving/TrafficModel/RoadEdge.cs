using System;
using System.Collections;
using System.Collections.Generic;
using SubSys_SimDriving;
using SubSys_MathUtility;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.RoutePlan;
using SubSys_SimDriving.Agents;
using System.Drawing;

namespace SubSys_SimDriving.TrafficModel
{
	/// <summary>
	/// һ������RoadEdge�ĳ�����RoadLane����һ�������ǻ��ν���ڣ��Լ��Ժ����չ����
	/// </summary>
	public class RoadEdge : RoadEntity
	{
		/// <summary>
		/// ��ǰ����·�ε�ͬ��ʱ��
		/// </summary>
		internal static int iTimeStep;

		internal static int iRoadEdgeCount = 0;
		#region ���캯�� ���ֳ�Ա�ĳ�ʼ����RegiserService����
		//public RoadEdge():this(new RoadNode(),new RoadNode()){}
		/// <summary>
		/// ǿ���ȹ���ڵ�
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		internal RoadEdge(RoadNode from, RoadNode to)
		{
			if (from ==null && to == null)
			{
				throw new ArgumentNullException("�޷�ʹ�ÿյĽڵ㹹���");
			}
			this.roadNodeFrom =from;
			this.roadNodeTo = to;
			this._lanes = new RoadLaneChain();

			this._id = RoadEdge.iRoadEdgeCount++;

		}
		
		internal RoadEdge(Point from, Point to)
		{
			
	
			this.roadNodeFrom =new RoadNode(from);;
			this.roadNodeTo =  new RoadNode(to);
			this._lanes = new RoadLaneChain();

			this._id = RoadEdge.iRoadEdgeCount++;

		}
		
		internal RoadEdge(RoadNode from, RoadNode to,TripCostAnalyzer tripCost):this(from,to)
		{
			this._tripCostAnalyzer = tripCost;
		}
		#endregion

		public override int iLength
		{
			get {
				int preNodeDistance = Coordinates.Distance(this.roadNodeFrom.RelativePosition, this.roadNodeTo.RelativePosition);

				int iRealLength = preNodeDistance- 2* SimSettings.iMaxLanes;
				if (iRealLength<10)
				{
					ThrowHelper.ThrowArgumentException("�����ڵ�֮�����̫��");
				}
				return iRealLength;
			}
		}
		public override int iWidth
		{
			get { return this.Lanes.Count*SimSettings.iCarWidth; }
		}

		public RoadNode roadNodeFrom;
		public RoadNode roadNodeTo;
		
		#region ·���ڲ��ĳ�����ص����ݽṹ�Ͳ�������
		/// <summary>
		/// �ɸ�����ӵ�����з���������ͬ��,�ڲ�������RoadLaneע��
		/// </summary>
		/// <param name="rl"></param>
		internal void AddLane(RoadLane rl)
		{
			if (rl != null)
			{
				//��ֹ����˽϶�ĳ���
				if (this.Lanes.Count ==SimSettings.iMaxLanes)
				{
					throw new ArgumentOutOfRangeException("�޷���ӳ���" + SimSettings.iMaxLanes + "������");
				}
				rl.Container = this;//���д���һ����Ҫ�߷���

				rl.Register();// //ͬ�����������ĵ����ݼ�¼

				
				//����laneRanking ��laneType���򣬲��뵽���ʵ�λ�ò��Ҹ���ǡ����
				//laneRanking���ڽ�����������
				int i = this._lanes.Count ;
				if (i == 0)//��һ��Ҫ��ӵĳ���
				{
					this._lanes.Add(rl);
					rl.Rank = 1;
				}
				while (i-->=1)//��������һ���������в������
				{
					RoadLane rLane = this._lanes[i];//i�Ѿ���С��һ����
					if (rLane.laneType > rl.laneType)
					{
						//���������laneRanking��ֵ��1
						rLane.Rank += 1;
						if (i==0)
						{
							this.Lanes.Insert(0, rl);//�������ұߵĳ���
							rl.Rank = 1;
						}
					}//rank����һ����ͬ����
					if (rLane.laneType <= rl.laneType)
					{   //�����µ�lane����ǰ������i��Ҫ����֮������Ӧ����i+1
						this._lanes.Insert(i+1, rl);
						//rl.Rank = i+2;//rank ��������1
						rl.Rank = i + 2;// this.Lanes.Count;
						break;
					}
				}
				//this.ilength =�˵�ĳ���//�˵�����֮��ľ���
			}
			else
			{
				throw new ArgumentNullException();
			}
		}
		internal void AddLane(LaneType lt)
		{
			RoadLane rl = new RoadLane(this, lt);
			this.AddLane(rl);
		}

		internal RoadLane GetLane(RoadLane rlCurr, string strLorR)
		{
			RoadLane rl = null;
			switch (strLorR)
			{
				case "L":
					if (rlCurr.Rank>1)//����1������ߵĳ���
					{
						rl = this._lanes[rl.Rank - 2];//��������Ϊrank-2��
					}
					break;
				case "R":
					if (rlCurr.Rank < this.Lanes.Count)//����1������ߵĳ���
					{
						rl = this._lanes[rl.Rank];//�Ҳ������Ϊrank
					}
					break;
				default:
					ThrowHelper.ThrowArgumentException("����Ķ����2");
					break;
			}
			return rl;

		}
		/// <summary>
		/// �ɸ���ɾ��������з���������ͬ��
		/// </summary>
		/// <param name="rl"></param>
		//[System.Obsolete("Ӧ������ʵ�ʵ����ȷ��ɾ��������Ҫ�ĺ�������")]
		internal void RemoveLane(RoadLane rl)
		{
			if (rl != null)
			{
				for (int i = rl.Rank; i < this.Lanes.Count; i++)
				{
					this.Lanes[i].Rank -= 1;
				}
				this._lanes.Remove(rl);//��rank�������ǵ�rank-1������
				//ͬ�����������ĵ����ݼ�¼
				rl.UnRegiser();//���з�ע��
			}else
			{
				throw new ArgumentNullException();
			}
		}
		/// <summary>
		/// �洢���ڲ��ĳ���roadlane�������simContext ��ͬ
		/// </summary>
		private RoadLaneChain _lanes;
		public RoadLaneChain Lanes
		{
			get
			{
				return this._lanes;
			}
		}
		#endregion

		#region ���з���

		internal TripCostAnalyzer _tripCostAnalyzer;

		private int _tripCost;
		/// <summary>
		/// ·�εĽ�ͨ����/�ɱ�
		/// </summary>
		internal int TripCost
		{
			get { return _tripCost; }
		}

		/// <summary>
		/// ����·�εĽ�ͨ�ɱ�
		/// </summary>
		[System.Obsolete("������ʹ�ø��³ɹ�")]
		internal void UpdateTripCost()
		{
			if (this._tripCostAnalyzer != null)
			{
				//this._tripCost = _tripCostAnalyzer.GetTripCost(this);
			}
			else
			{
				throw new System.MissingFieldException("û�к��ʵĳ��з��ü����࣡");
			}
		}
		#endregion

		#region ��ϣ����
		/// <summary>
		/// ������ʼ�ڵ�ͽ����ڵ����ߵĹ�ϣֵ
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			//return RoadEdge.iRoadEdgeCount;
			return string.Concat(roadNodeFrom.GetHashCode().ToString(), roadNodeTo.GetHashCode().ToString()).GetHashCode();
		}
		/// <summary>
		/// ��̬�Ĺ�ϣ��������������ĳ���ߵĹ�ϣֵ
		/// </summary>
		internal static int GetHashCode(RoadNode rnFrom,RoadNode rnTo)
		{
			return string.Concat(rnFrom.GetHashCode().ToString(), rnTo.GetHashCode().ToString()).GetHashCode();
		}
		#endregion

		/// <summary>
		/// ����visitor ģʽ��vmsagent�ȡ�Ȼ������Ԫ��ģ�ͣ�Ȼ��������з���
		/// </summary>
		public override void UpdateStatus()
		{
			////�����첽��Ϣ
			for (int i = 0; i < this.asynAgents.Count; i++)
			{
				Agents.Agent visitorAgent = this.asynAgents[i];
				visitorAgent.VisitUpdate(this);//.VisitUpdate();
			}
			//��roadedge����Ԫ����drive Ŀ�������ó������Ի���
			foreach (var lane in this.Lanes)
			{
				for (int i = 0; i < lane.CellCount; i++)
				{
					lane[i].Drive(this);//����һ��Ԫ���ķ���
				}
				lane.UpdateStatus();//����ע���ڳ����ϵķ���
			}
			base.UpdateStatus();//����ע����·���ϵķ�����RoadEdgePaintService
		}
		/// <summary>
		/// ·�ε�OnStatusChangedί�и�RoadLane����
		/// </summary>
		[System.Obsolete("�����˷���")]
		protected override void OnStatusChanged()
		{
			this.InvokeServices(this);
		}

		/// <summary>
		/// ���������ȥ�յ�����
		/// </summary>
		/// <returns></returns>
		[System.Obsolete("������������п��ܲ���������ȫһ����·�ζ˵����꣬�ú�������ͼ�����һ���⣬��ʽ����Ӧ��ʹ��")]
		public override MyPoint ToVector()
		{
			MyPoint p = new MyPoint(roadNodeTo.RelativePosition.X - roadNodeFrom.RelativePosition.X, roadNodeTo.RelativePosition.Y - roadNodeFrom.RelativePosition.Y);
			if (p._X == 0.0f && p._Y == 0.0f)
			{
				p._X = 12;
				p._Y = 12;
				//throw new Exception("RoadEdge��������������");
			}
			return p;
		}

		public override EntityShape Shape
		{
			get
			{
				EntityShape eShape = base.Shape;

				if (eShape.Count == 0)//shape û�г�ʼ��
				{
					int pX =this.roadNodeTo.RelativePosition.X - this.roadNodeFrom.RelativePosition.X;
					int pY =  this.roadNodeTo.RelativePosition.Y - this.roadNodeFrom.RelativePosition.Y;
					//�����ȷ�
					float dLq = this.iLength + 2 * SimSettings.iMaxLanes;//��ĸ
					float xSplit = pX / dLq;//������������
					float ySplit = pY / dLq;//������������
					//�������
					int iOffset = SimSettings.iMaxLanes;
					eShape.Add(new MyPoint(this.roadNodeFrom.RelativePosition.X + iOffset * xSplit, this.roadNodeFrom.RelativePosition.Y + iOffset * ySplit));
					//�����յ�
					eShape.Add(new MyPoint(this.roadNodeTo.RelativePosition.X - iOffset * xSplit, this.roadNodeTo.RelativePosition.Y - iOffset * ySplit));
				}
				return eShape;
			}
		}

		/// <summary>
		/// ��ȡ��һ��Road�ڲ�����RoadEdge���Ӧ�ķ���·��
		/// </summary>
		/// <returns></returns>
		public RoadEdge GetReverse()
		{
			return (ISimCtx.NetWork as IRoadNetWork).FindRoadEdge(this.roadNodeTo, this.roadNodeFrom);
		}

		/// <summary>
		/// �洢�ӽ����roadNode����·�εĳ�������Ϊʱ�䳬ǰһ��ʱ�䲽����
		/// ��Ҫ��������з�ֹһ��Ԫ���ȸ��µ�·�Σ�Ȼ����·�����ָ���һ�θ�������
		/// </summary>
		private Queue<Cell> queWaitedCell = new Queue<Cell>();

		/// <summary>
		/// �޸��źŵ����
		/// </summary>
		/// <param name="sl">�µ��źŵ�</param>
		/// <param name="lt">Ҫ�޸ĵĳ�������</param>
		public void ModifySignalGroup(SignalLight sl, LaneType lt)
		{
			foreach (RoadLane rl in this.Lanes)
			{
				if (rl.laneType == lt)
				{
					rl.SignalLight = sl;
				}
			}
		}
		/// <summary>
		/// ·������
		/// </summary>
		internal SpeedLevel iSpeedLimit;

		
		
	}
	
}

