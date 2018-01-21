using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using SubSys_MathUtility;
using SubSys_SimDriving;
using SubSys_SimDriving.RoutePlan;
using SubSys_SimDriving;

namespace SubSys_SimDriving.TrafficModel
{
	/// <summary>
	///Road��һ������ߣ�һ����·�������� һ������Edge�ĳ�����Lane����һ�������ǻ��ν���ڣ��Լ��Ժ����չ����
	/// </summary>
	public partial class Way : StaticEntity
	{
        public override EntityShape Shape
        {
            get
            {
                return base.Shape;
            }
            set
            {
                base.Shape = value;
                foreach (var item in this.Lanes)
                {
                    item.CreateShape();
                }
            }
        }

        /// <summary>
        /// ��ǰ����·�ε�ͬ��ʱ��
        /// </summary>
        internal static int iTimeStep;

		internal static int iCount = 0;
		#region ���캯�� ���ֳ�Ա�ĳ�ʼ����RegiserService����
		//public Way():this(new RoadNode(),new RoadNode()){}
		/// <summary>
		/// ǿ���ȹ���ڵ�
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
  //      [System.Obsolete("outdated")]
		//public Way(XNode from, XNode to)
		//{
		//	if (from ==null && to == null)
		//	{
		//		throw new ArgumentNullException("�޷�ʹ�ÿյĽڵ㹹���");
		//	}
		//	this.From =from;
		//	this.To = to;

  //          this._lanes = new Lanes();

  //          this.EntityType = EntityType.Way;
		//	this._entityID = ++Way.iCount;

		//}

        public Way CtrWay
        {
            get
            {
                return this.Container == null ? null : (this.Container as Road).CtrWay;
            }
        }
//		/// <summary>
//		/// ����һ����from�㵽to��ĵ�·
//		/// </summary>
//		/// <param name="from"></param>
//		/// <param name="to"></param>
//		internal Way(Point from, Point to)
//		{
//			this.XNodeFrom =new XNode(from);;
//			this.XNodeTo =  new XNode(to);
//			this._lanes = new LaneChain();
//
//			this._entityID = ++Way.iRoadCount;
//
//		}
		
		//internal Way(XNode from, XNode to,TripCostAnalyzer tripCost):this(from,to)
		//{
		//	this._tripCostAnalyzer = tripCost;
		//}

		#endregion
      //      [System.Obsolete("this fun needs to be restruct!")]
		public override int Length
		{
			get {
                //int iRealLength = (int)Coordinates.Distance(this.Shape.Start, this.Shape.End)- 2* SimSettings.iMaxLanes;
                //if (iRealLength<SimSettings.iMaxLanes)
                //{
                //	ThrowHelper.ThrowArgumentException("�����ڵ�֮�����̫��");
                //}
                //return iRealLength;
                return this.Shape.Count;
			}
		}
		public override int Width
		{
			get { return this.Lanes.Count*SimSettings.iCarWidth; }
		}

		public XNode From;
		public XNode To;
		
		#region ·���ڲ��ĳ�����ص����ݽṹ�Ͳ�������
		/// <summary>
		/// �ɸ������ӵ�����з���������ͬ��,�ڲ�������RoadLaneע��
		/// </summary>
		/// <param name="rl"></param>
		internal void AddLane(Lane rl)
		{
			if (rl != null)
			{
				//��ֹ�����˽϶�ĳ���
				if (this.Lanes.Count ==SimSettings.iMaxLanes)
				{
					throw new ArgumentOutOfRangeException("�޷����ӳ���" + SimSettings.iMaxLanes + "������");
				}
				rl.Container = this;//���д���һ����Ҫ�߷���

				RoadNet.GetInstance().lanes.Add(rl.GetHashCode(),rl);
			//	rl.Register();// //ͬ�����������ĵ����ݼ�¼

				
				//����laneRanking ��laneType���򣬲��뵽���ʵ�λ�ò��Ҹ���ǡ����
				//laneRanking���ڽ�����������
				int i = this._lanes.Count ;
				if (i == 0)//��һ��Ҫ���ӵĳ���
				{
					this._lanes.Add(rl);
					rl.Rank = 1;
				}
				while (i-->=1)//��������һ���������в������
				{
					Lane rLane = this._lanes[i];//i�Ѿ���С��һ����
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
		public void AddLane(LaneType lt)
		{
			Lane rl = new Lane(this, lt);
            rl.Name = this.Name + lt.ToString();
			this.AddLane(rl);
		}

		internal Lane GetLane(Lane rlCurr, string strLorR)
		{
			Lane rl = null;
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
		internal void RemoveLane(Lane rl)
		{
			if (rl != null)
			{
				for (int i = rl.Rank; i < this.Lanes.Count; i++)
				{
					this.Lanes[i].Rank -= 1;
				}
				this._lanes.Remove(rl);//��rank�������ǵ�rank-1������
				//ͬ�����������ĵ����ݼ�¼
				RoadNet.GetInstance().lanes.Remove(rl.GetHashCode());//���з�ע��
				
			}else
			{
				throw new ArgumentNullException();
			}
		}
		
		/// <summary>
		/// �洢���ڲ��ĳ���roadlane�������simContext ��ͬ
		/// </summary>
		
		private Lanes _lanes;
		/// <summary>
		/// ��·���������г����ļ���
		/// </summary>
		public Lanes Lanes
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
			//return Way.iWayCount;
			//return string.Concat(XNodeFrom.GetHashCode().ToString(), XNodeTo.GetHashCode().ToString()).GetHashCode();
            return string.Concat(From.GetHashCode().ToString(), To.GetHashCode().ToString()).GetHashCode();

        }
        /// <summary>
        /// ��̬�Ĺ�ϣ��������������ĳ���ߵĹ�ϣֵ
        /// </summary>
        internal static int GetHashCode(XNode rnFrom,XNode rnTo)
		{
			return string.Concat(rnFrom.GetHashCode().ToString(), rnTo.GetHashCode().ToString()).GetHashCode();
		}
		#endregion

		/// <summary>
		/// ��ʱ�ģ��Ͼɵĺ�������visitor ģʽ��vmsagent�ȡ�Ȼ������Ԫ��ģ�ͣ�Ȼ��������з���
		/// </summary>
		//[System.Obsolete("obsolete")]
		public override void UpdateStatus()
		{
			////�����첽��Ϣ
			for (int i = 0; i < this.asynAgents.Count; i++)
			{
				Agents.AbstractAgent visitorAgent = this.asynAgents[i];
				visitorAgent.VisitUpdate(this);
			}
			
		
			Lane lane ;
			for (int i = 0; i < this.Lanes.Count; i++) {
				
				lane = this.Lanes[i];
				
				var mobileNode = lane.Mobiles.First;
				
				//update mobile on a lane one by one
				while(mobileNode!=null) {
					var mobile = mobileNode.Value;
					//mobile is possibaly be deleted
					mobile.Run(lane);
					mobileNode = mobileNode.Next;
				}
				lane.UpdateStatus();//����ע���ڳ����ϵķ���
			}
			base.UpdateStatus();//����ע����·���ϵķ�����WayPaintService
		}
		/// <summary>
		/// ·�ε�OnStatusChangedί�и�RoadLane����
		/// </summary>
		protected override void OnStatusChanged()
		{
			this.InvokeServices(this);
		}

		/// <summary>
		/// ���������ȥ�յ�����
		/// </summary>
		/// <returns></returns>
		[System.Obsolete("������������п��ܲ���������ȫһ����·�ζ˵����꣬�ú�������ͼ�����һ���⣬��ʽ����Ӧ��ʹ��")]
		public override OxyzPointF ToVector()
		{
			var p = this.Shape.End-this.Shape.Start;
			if (p._X == 0.0f && p._Y == 0.0f)
			{
				throw new Exception("��������������");
			}
			return p;
		}

		/// <summary>
		/// ��ȡ��һ��Road�ڲ�����Way���Ӧ�ķ���·��
		/// </summary>
		/// <returns></returns>
		public Way FindReverse()
		{
			return (ISimCtx.RoadNet as IRoadNet).FindWay(this.To, this.From);
		}

		/// <summary>
		/// �洢�ӽ����roadNode����·�εĳ�������Ϊʱ�䳬ǰһ��ʱ�䲽����
		/// ��Ҫ��������з�ֹһ��Ԫ���ȸ��µ�·�Σ�Ȼ����·�����ָ���һ�θ�������
		/// </summary>
		//private Queue<Cell> queWaitedCell = new Queue<Cell>();

		/// <summary>
		/// �޸��źŵ����
		/// </summary>
		/// <param name="sl">�µ��źŵ�</param>
		/// <param name="lt">Ҫ�޸ĵĳ�������</param>
		public void ModifySignalGroup(SignalLight sl, LaneType lt)
		{
			foreach (Lane rl in this.Lanes)
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
	public partial class Way : StaticEntity
	{
		public Way(OxyzPointF opStart,OxyzPointF opEnd)
		{
			this._entityID = ++Way.iCount;
            this.EntityType = EntityType.Way;
			
			this.Shape.Add(opStart);
			this.Shape.Add(opEnd);

            this.From = new XNode(opStart); ;
            this.To = new XNode(opEnd);
            this._lanes = new Lanes();

		}

		internal override void ServeMobiles()
		{
			throw  new Exception("this. function should not be called ,call lane serveMobiles instead!");
		}

       // internal Draw
	}
	
}
