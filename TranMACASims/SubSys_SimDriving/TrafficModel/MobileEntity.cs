using SubSys_SimDriving;
using System.Drawing;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.RoutePlan;
using SubSys_MathUtility;
using System;

namespace SubSys_SimDriving
{
	public abstract partial class MobileEntity : TrafficEntity
	{
		/// <summary>
		/// ��ǰ�������ٶ�
		/// </summary>
		internal int iSpeed;
		
	}
	
	//--------------------2015��1��11���������ӵ�����-------------------
	/// <summary>
	/// ���лᶯ������Ļ�����
	/// </summary>
	public abstract partial class MobileEntity : TrafficEntity
	{
		private static int MobileID = 0;
		private bool IsCopyed = false;
		
		public DriveStrategy Strategy;
		
		//  public Shape ��״�Ѿ����ˣ����Ǽ̳�TrafficEntity��shape
		
		/// <summary>
		/// ���󿽱���ֵ����
		/// </summary>
		/// <returns></returns>
		public virtual MobileEntity Clone()
		{
			MobileEntity cm = this.MemberwiseClone() as MobileEntity;
			cm.IsCopyed = true;
			//this.EntityType = EntityType.Mobile;
			return cm;
		}
		
		public Color Color;
		
		public EdgeRoute EdgeRoute;
		public NodeRoute NodeRoute;

		internal DriveStrategy Driver = new DefaultDriveAgent();

		~MobileEntity()
		{
			if (this.IsCopyed != true)
			{
				base.UnRegiser();
			}
		}
		//internal SpeedLevel CurrSpeed;
		/// <summary>
		/// ��ǰ�����ļ��ٶ�
		/// </summary>
		internal int iAcc = 1;
	
		
		
		#region ���Բ���
		
		public override int iLength
		{
			get
			{//�����ĳ��Ⱦ���Ԫ���ĳ��ȣ���������״�����Ǽ���Ԫ������״
				return this.Shape.Count;
			}
		}
		
		#endregion
		
		
		
		protected MobileEntity() { }
		
		[System.Obsolete("��ʱ,��Ϊcar���Ѿ���ʱ")]
		public MobileEntity(Car cm)
		{
			//��trafficmodel �̳еı����ֶ�
			this._id = ++MobileEntity.MobileID;
			
			this.EntityType = EntityType.Mobile;
			this.Color = Color.Green;
			this.iSpeed = 0;
			base.Register();
			this.EdgeRoute = new EdgeRoute();
			this.NodeRoute = new NodeRoute();
		}
		/// <summary>
		/// ������¼̬��ϣ���¼������ʱ����Ϣ���Լ�����ȷ��ʲôʱ�����·��
		/// </summary>
		internal int iTimeStep;
		

		/// <summary>
		/// ��Χ�ڽ�����ڲ��Ľڵ�,�ڲ���ʹ�þ�������ϵ
		/// </summary>
		public Track Track = new Track();
		/// <summary>
		/// ʹ���ڽ������
		/// </summary>
		/// <param name="iStep"></param>
		internal void TrackMove(int iStep)
		{
			Point p = this.Grid;
			while (iStep-- > 0)
			{
				p = this.Track.NextPoint(p);
			}
			this.Grid = p;
		}

		/// <summary>
		/// ֻӦ����ת����ʱ�����һ�Σ�Ѱ�ҹ켣��һ������������ʼλ�ó�����ǰ��iAheadSpace�����ʱ��
		/// </summary>
		/// <param name="iAheadSpace"></param>
		internal virtual void CalcTrack(int iAheadSpace)
		{
			Lane rl = this.Container as Lane;
			if (rl == null)
			{
				ThrowHelper.ThrowArgumentNullException("�Բ���·���ϵ�Ԫ�����ô˴η����Ǵ����");
			}
			Track mt = this.Track;
			mt.fromLane = rl;
			//��������ϵ�ĳ����Ľ������ڵĵ�һ����
			mt.pFromPos = new Point(rl.Rank - 1, -SimSettings.iMaxLanes + 1);
			
			//��ȡת����Ϣ
			Way re = rl.Container as Way;
			int iTurn = this.EdgeRoute.GetSwerve(re);

			Way reNext = this.EdgeRoute.FindNext(re);
			if (iTurn == 0)//ֱ��ʹ����������ϵ
			{
				//����ֱ���Ӧ
				mt.pToPos = new Point(rl.Rank-1,SimSettings.iMaxLanes);
				if (reNext == null)//û��Ŀ�공�����Ѿ���ͷ��
				{
					mt.toLane = null;
				}
				else
				{
					if (reNext.Lanes.Count < rl.Rank)//��ֹ������ƥ��
					{   //Ŀ�공����С�ڱ�������,����Ŀ�공�����ڲ೵��
						mt.toLane = reNext.Lanes[0];
					}
					else
					{   //Ŀ�공���������ڱ�������
						mt.toLane = reNext.Lanes[rl.Rank-1];
					}
				}
			}
			if (iTurn == 1)//��ת
			{
				//����1��reNext ����������������������ѡ�񳵵�
				int iLaneIndex = - reNext.Lanes.Count + 1;
				mt.pToPos = new Point(  SimSettings.iMaxLanes,iLaneIndex);
				mt.toLane = reNext.Lanes[-iLaneIndex];
			}
			if (iTurn == -1)//��ת
			{
				//-4λ�õ�����Ӧ��Ϊ-3 ,�����Ҳ�����ѡ�񳵵�
				int iLaneIndex = new Random(1).Next(reNext.Lanes.Count)- 1;
				mt.pToPos = new Point(-SimSettings.iMaxLanes + 1,iLaneIndex);
				mt.toLane = reNext.Lanes[iLaneIndex];
			}
			if (iTurn ==2 )
			{
				int iLaneIndex = reNext.Lanes.Count- 1;
				mt.pToPos = new Point(-iLaneIndex,-SimSettings.iMaxLanes+1);
				mt.toLane = reNext.Lanes[iLaneIndex];
			}
			
			//������ȫ��ת��ת��Ϊԭ������
			mt.pFromPos = Coordinates.GetRealXY(mt.pFromPos, mt.fromLane.Container.ToVector());
			mt.pTempPos = new Point(rl.Rank-1,iAheadSpace-SimSettings.iMaxLanes);//������
			mt.pTempPos = Coordinates.GetRealXY(mt.pTempPos, mt.fromLane.ToVector());
			mt.pToPos = Coordinates.GetRealXY(mt.pToPos, mt.fromLane.Container.ToVector());
		}
		
		
		internal void Drive(StaticEntity DriveEnvirnment)
		{
//			this.Driver.dr(DriveEnvirnment);
			//�������Ҫ��д
//			this.DriveStg.Drive(rN,this);
		}

		[System.Obsolete("����ϵͳ�����⣬postion����ֵ roadhash������,iTimeStep������")]
		internal CarInfo GetCarInfo()
		{
			CarInfo ci = new CarInfo();//�ṹ��ֵ����
			ci.iSpeed = this.iSpeed;
			ci.iAcc = this.iAcc;
			ci.iCarHashCode = this.GetHashCode();
			ci.iCarNum = this.ID;
			
			ci.iTimeStep = ISimCtx.iCurrTimeStep;
			
			
			if (this.Container.EntityType == EntityType.Lane)
			{
				ci.iPos = this.Grid.Y+(int)this.Container.Shape[0]._X;
			}
			else if (this.Container.EntityType == EntityType.XNode)
			{
				ci.iPos = this.Container.Grid.X + this.Grid.X-1;
			}
			return ci;
		}

	
		/// <summary>
		/// ���������Ҫ����д�����㵱ǰԪ������ǰ���ĳ�ͷʱ��
		/// </summary>
		/// <param name="iEntityGap"></param>
		/// <param name="iToEntityGap"></param>
		public virtual void GetEntityGap(out int iEntityGap,out int iToEntityGap)
		{
			iEntityGap=0;
			iToEntityGap=0;
		}

		/// <summary>
		/// ����Ԫ���ڽ�����ڲ������߶��ٲ�
		/// </summary>
		/// <param name="rN"></param>
		/// <param name="ct"></param>
		/// <returns></returns>
		private bool GetTrackGap(XNode rN, Point pcc,out int Gap)
		{
			bool bReachEnd = false;
			int iCount = 0;
			Point p = this.Track.NextPoint(pcc);
			if (p.X == 0 && p.Y == 0)
			{
				bReachEnd = true;
			}
			while (rN.IsBlocked(p) == false)
			{
				p = this.Track.NextPoint(p);
				if (p.X == 0 && p.Y == 0)
				{
					bReachEnd =true;
					break;
				}
				iCount++;
			}
			Gap = iCount;
			return bReachEnd;
		}
		
		
	}
	
}

