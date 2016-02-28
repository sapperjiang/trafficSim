using System;
using System.Collections;
using System.Collections.Generic;

using SubSys_MathUtility;
using SubSys_SimDriving.Agents;
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

using System.Drawing;

namespace SubSys_SimDriving.TrafficModel
{
	
	/// <summary>
	/// ʹ�þ�������data structure
	/// ��·����.��·��֧���д����ۣ���·����,crossings are supported
	/// ��ʾ��·����ڵ���
	/// </summary>
	public class XNode : StaticEntity
	{
//		/// <summary>
//		/// ·��ת��Ϊ���������,iahead  ��Ӧ��С����
//		/// </summary>
//		private Point MakeCenterXY(Lane rl, int iAhead)
//		{
//			return new Point(rl.Rank, iAhead - SimSettings.iMaxLanes);
//		}


		#region ������������


//		/// <summary>
//		/// �ж�ָ������ǰ����Ahead��λ�ô��Ƿ���Ԫ��ռ��
//		/// </summary>
//		internal bool IsBlocked(Lane rl, int iAhead)
//		{
//			Point irltXY = this.MakeCenterXY(rl,iAhead);
//			Point iRealXY = Coordinates.GetRealXY(irltXY,rl.ToVector());
//			return _mobiles.IsBlocked(iRealXY.X, iRealXY.Y);
//		}
//		/// <summary>
//		/// �ж�ָ������ǰ����Ahead��λ�ô��Ƿ���Ԫ��ռ��
//		/// </summary>
//		internal bool IsBlocked(Point iRealXY)
//		{
//			return _mobiles.IsBlocked(iRealXY.X, iRealXY.Y);
//		}
//

		/// <summary>
		/// ����������
		/// </summary>
		/// <param name="rl"></param>
		internal void BlockLane(Lane rl)
		{
//			if (rl == null)
//			{
//				throw new ArgumentNullException();
//			}
//			if (IsBlocked(rl,1)==false)//�յ������
//			{
//				//	this.AddCell(rl, 1);
//			}
		}
		/// <summary>
		/// ��������ͨ
		/// </summary>
		/// <param name="rl"></param>
		internal void UnblockLane(Lane rl)
		{
//			if (rl == null)
//			{
//				throw new ArgumentNullException();
//			}
//			if (IsBlocked(rl, 1)==true)//�ǿ���ɾ��
//			{//��null ��λ�� rl.rank �͵�1-6��λ�õ�Ԫ��ռ��
//				//			this.RemoveCell(rl, 1);//(id, rl.parEntity, null);
//			}
		} /// <summary>
		/// �жϵ�x������ǰ���Ƿ���iAheadSpace������
		/// </summary>
		/// <returns></returns>
//		internal bool IsLaneBlocked(Lane rl, int iAheadSpace)
//		{
//			bool isBlocked = false;
//			for (int i = 1; i <= iAheadSpace; i++)
//			{
//				isBlocked = this.IsBlocked(rl, i);
//				if (isBlocked == true)
//					break;
//			}
//			return isBlocked;
		//	}
//
//		internal bool IsLaneBlocked(Lane rl)
//		{
//			return this.IsBlocked(rl, 1);
//		}
		#endregion

		#region Ԫ����������
//		/// <summary>
//		/// Ϊ���̵����׼���ķ���������������Ԫ��
//		/// </summary>
//		private void AddCell(Lane rl, int iAheadSpace)
//		{
//			Point ipt = this.MakeCenterXY(rl, 1);
//			ipt = Coordinates.GetRealXY(ipt, rl.ToVector());
//			cells.Add(ipt.X, ipt.Y, null);//�������õ�Ԫ������Ϊnull
//		}
//
//		/// <summary>
//		/// ��ָ���ĵ����һ��Ԫ����
//		/// </summary>
//		internal void AddCell(Cell ca)
//		{
//			ca.Container = this;
//			cells.Add(ca.Track.pCurrPos.X,ca.Track.pCurrPos.Y, ca);
//		}
		/// <summary>
		/// Ҫ�����������Ǿ�������
		/// </summary>
		/// <param name="iOldPoint"></param>
		/// <param name="iNewPoint"></param>
		/// <returns></returns>
//		[System.Obsolete("cell is not used in new version of this software ,replace with mobile")]
//		internal bool MoveCell(Point iOldPoint, Point iNewPoint)
//		{
//			return cells.Move(iOldPoint, iNewPoint);
//		}
//
//		internal bool RemoveCell(Cell ce)
//		{
//			return this.cells.Remove(ce.Track.pCurrPos.X, ce.Track.pCurrPos.Y);
//		}
//
//		/// <summary>
//		/// ����ָ����·�Σ�·��ǰ���ľ������ɾ��Ԫ��
//		/// </summary>
//		/// <param name="rl">��ת����ϵ��Ҫ�õ��ļ�����ת�Ƕȵ�����</param>
//		/// <param name="iAheadSpace">ǰ�о�����</param>
//		internal bool RemoveCell(Lane rl, int iAheadSpace)
//		{
//			Point ipt = this.MakeCenterXY(rl, 1);
//			Point iRealIndex = Coordinates.GetRealXY(ipt, rl.ToVector());
//			return cells.Remove(iRealIndex.X, iRealIndex.Y);
//		}
		
		/// <summary>
		/// �µ�roadnode�Ĺ�ϣɢ��ֵ��������Position�Ĺ�ϣֵ����ID����
		/// </summary>
		/// <returns></returns>
		//private HashMatrix<Cell> cells = new HashMatrix<Cell>();
		private HashMatrix _mobiles; //= new HashMatrix<MobileEntity>();
		public HashMatrix Mobiles
		{
			get
			{
				if (this._mobiles == null) {
					this._mobiles=new HashMatrix();
				}
				return this._mobiles;
			}
		}

		
		#endregion
		
		

		/// <summary>
		/// �������ڵ����г��ߵĹ�ϣ����ֵ�Ǵ���ߵ�RoadEdge��ϣ��ֵ�Ǵ���RoadEdge
		/// </summary>
		private Dictionary<int, Way> _dicEdges = new Dictionary<int,Way>();

		/// <summary>
		/// �ṩ�Թ�ϣ�����ڲ�Ԫ�صı���
		/// </summary>
		/// <returns></returns>
		public IEnumerator<MobileEntity> GetEnumerator()
		{
			return this._mobiles.GetEnumerator();
		}

		public ICollection Ways
		{
			get
			{
				return this._dicEdges.Values;
			}
		}
		#region ���������ڽӾ����нڵ���ߵı߳�Ա,��Ӧ��ʹ��RoadNetwork֮����������Щ��Ա
		/// <summary>
		/// ע���ڳ��߱��У�����roadedge��from�ֶ���this�ڵ㣬�������׳��쳣
		/// </summary>
		/// <param name="roadEdge"></param>
		internal void AddWay(Way way)
		{
			if (way != null)
			{
				if (!Contains(way.GetHashCode()))
				{
					//�����ж��Ƿ��ǵ�ǰ��ĳ��ߵ���Ϣ��ֹ����
					if (way.XNodeFrom !=this)
					{
						throw new Exception("����˲����ڸö���ı�");
					}
					_dicEdges.Add(way.GetHashCode(), way);
				}
				else
				{
					throw new ArgumentException("������ظ��ıߣ�");
				}
			}
			else
			{
				throw new ArgumentNullException();
			}
		}
		/// <summary>
		/// �ҵ��� ��this��toNode�ڵ�ıߣ����߱�
		/// </summary>
		/// <param name="fromRN"></param>
		internal void RemoveWay(Way re)
		{
			if (re == null )
			{
				throw new ArgumentNullException();
			}
			_dicEdges.Remove(re.GetHashCode());
		}
		internal void RemoveWay(XNode toRN)
		{
			if (toRN == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.obj);
			}
			_dicEdges.Remove(Way.GetHashCode(this,toRN));
		}
		
		/// <summary>
		/// ���ҷ������µĽṹ���ó��߱�
		/// </summary>
		/// <param name="toRoadNode">���ڵ�</param>
		/// <returns></returns>
		public Way FindWay(XNode toRoadNode)
		{
			int iHashkey = Way.GetHashCode(this,toRoadNode);
			if (_dicEdges.ContainsKey(iHashkey))
			{
				return _dicEdges[iHashkey];
			}
			return null;
		}
		
		public bool Contains(int EdgeKey)
		{
			return _dicEdges.ContainsKey(EdgeKey);
		}

		#endregion
		/// <summary>
		/// ����RoadNodeID������
		/// </summary>
		private static int iRoadNodeID;
		[System.Obsolete("ʹ���в����Ĺ��캯��")]
		internal XNode()
		{
			this._entityID = ++iRoadNodeID;
			Random rd = new Random();

			this.GISGrid = new OxyzPointF(rd.Next(65535), rd.Next(65535));
			// ֱ��ʹ�������ĵ����ݽṹ,bug��Ӧ��ʹ�������Ľṹ
			if (this.GISGrid._X == 0.0f && this.GISGrid._Y == 0.0f)
			{
				ThrowHelper.ThrowArgumentNullException("RoadNode�����������꣡");
			}
		}
//		internal XNode(Point rltPostion)
//		{
//			this._entityID = ++iRoadNodeID;
//			Random rd = new Random();
//		//	this.Grid = rltPostion;
//			this.GISGrid = new OxyzPointF(rd.Next(65535), rd.Next(65535));
//		}
//		
		public override int GetHashCode()
		{
			int iHash = this.GISGrid.GetHashCode() +this.ID.GetHashCode();
			return iHash.GetHashCode();
		}
		/// <summary>
		/// ����agent������Ԫ������ʻ�������÷���
		/// </summary>
		public override void UpdateStatus()
		{
			//�����첽agent������еĻ�
			for (int i = 0; i < this.asynAgents.Count; i++)
			{
				AbstractAgent visitor = this.asynAgents[i];
				visitor.VisitUpdate(this);//.VisitUpdate();
			}
		
			var mobileNode = this.Mobiles.First;
			//update mobile on a lane one by one
			while(mobileNode!=null) {
				var mobile = mobileNode.Value;
				//mobile is possibaly be deleted
				mobile.Run(this as StaticEntity);
				mobileNode = mobileNode.Next;
			}
			this.ServeMobiles();
			base.UpdateStatus();//���������OnStatusChanged ���л�ͼ����
		}

		protected override void OnStatusChanged()
		{
			//call its base's method to run services registered on this entity
			
			//this.ServeMobiles();
			
			this.InvokeServices(this);
		}
		
		
		
		
		/// <summary>
		/// ������ڵĵȴ���������,a mobile that can enter xnode was added to mobilesInn.
		/// see driverstratigy for details
		/// </summary>
		internal override void ServeMobiles()
		{
			while (this.MobilesInn.Count > 0)
			{
				this.Mobiles.Add(this.MobilesInn.Dequeue());
			}

		}

		
		
		/// <summary>
		/// �ж�ָ������ǰ����Ahead��λ�ô��Ƿ���Ԫ��ռ��
		/// </summary>
		public bool IsOccupied(OxyzPointF opPoint)
		{
			//if this point is not added to dictionary ,add it then
			//return this._occupiedPoints.;
			return this.Mobiles.IsOccupied(opPoint);
		}

		internal XNode(OxyzPointF pointCenter)
		{
			this._entityID = ++iRoadNodeID;
			//Random rd = new Random();
			this.SpatialGrid = pointCenter;
		}
		
		
	}
}

