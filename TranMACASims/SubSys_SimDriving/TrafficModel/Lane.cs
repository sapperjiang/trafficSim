using System;
using SubSys_MathUtility;
using SubSys_SimDriving;
using System.Collections.Generic;
using SubSys_SimDriving.SysSimContext;
using System.Drawing;

namespace SubSys_SimDriving.TrafficModel
{
	/// <summary>
	/// ��ͨʵ������ĳ���,ԭ����lane���ݽṹ
	/// </summary>
	public partial class Lane : StaticEntity, IComparable<Lane>, IComparer<Lane>
	{
		/// <summary>
		/// ȫ�ֵĳ�����������������ʼ������ID
		/// </summary>
		private static int iLaneCount=0;
		///// <summary>
		///// ÿ�θ���CurrTimeStep��ʱ����Զ�����ȴ��б�
		///// </summary>
		public override int iLength
		{
			get
			{
				return this.Container.iLength;
			}
		}
		public override int iWidth
		{
			get
			{
				return SimSettings.iCarWidth;
			}
		}

		public override EntityShape Shape
		{
			get
			{
				EntityShape eShape = base.Shape;

				if (eShape.Count == 0)//shape û�г�ʼ��
				{
					CreateShape(eShape);
				}
				return eShape;
			}
		}

		/// <summary>
		/// �������µ�Ԫ���ռ������д�ú���
		/// </summary>
		/// <param name="eShape"></param>
		private  void CreateShape(EntityShape eShape)
		{

			EntityShape es = this.Container.Shape;

			OxyzPointF pNorm = VectorTools.GetNormalVector(this.Container.ToVector());
			OxyzPointF mpOffset = new OxyzPointF(pNorm._X*(this.Rank - 0.5f),pNorm._Y * (this.Rank - 0.5f));
			//ƽ������
			OxyzPointF pFirst = Coordinates.Offset(es[0], mpOffset);
			//�����յ�
			OxyzPointF pFEnd = Coordinates.Offset(es[es.Count - 1], mpOffset);
			
			OxyzPointF mp = new OxyzPointF(pFEnd._X-pFirst._X,pFEnd._Y-pFirst._Y);
			int iLoopCount = this.iLength;//Ԫ�����ȣ���ʼ���μ�registerservice

			float xSplit = mp._X / iLoopCount;//������������
			float ySplit = mp._Y / iLoopCount;//������������
			
			eShape.Add(pFirst);
			for (int i = 1; i < iLoopCount; i++)//x��
			{   //�е�
				eShape.Add(new OxyzPointF(pFirst._X + (i-0.5f) * xSplit, pFirst._Y + (i-0.5f) * ySplit));
			}
			eShape.Add(pFEnd);
		}
		
		/// <summary>
		/// û�е��ã���ʱ����д
		/// </summary>
		/// <param name="iAheadSpace"></param>
		/// <returns></returns>
		internal bool IsLaneBlocked(int iAheadSpace)
		{
			return this.iLastPos-1 <= iAheadSpace ;
		}
		/// <summary>
		/// �ѹ�ʱ�����������һ��Ԫ����λ�ã�Ӧ����Y����
		/// </summary>
		private int _ilastPos;
		/// <summary>
		/// �ѹ�ʱ����ȡ���������һ��Ԫ����λ�ã����û��Ԫ���򷵻س�������
		/// </summary>
		[System.Obsolete("�ѹ�ʱ")]
		internal int iLastPos
		{
			get
			{
				Cell ce = this._cells.PeekLast();
				if (ce== null)
				{
					this._ilastPos = this.iLength;
				}
				else //if (this._ilastPos > ce.RltPos.Y)
				{
					this._ilastPos = ce.Grid.Y;
				}
				return _ilastPos;
				
			}
		}

		
	
	
		/// <summary>
		///���������򣬴��ڲ೵����ʼ�ĵڼ�������,�����Գ�����������
		/// </summary>
		public int Rank;

		[System.Obsolete("�ѹ�ʱ����¼�����������λ�õ����飬�µ�Ԫ���ռ����۽�����ֹͣʹ�ø�����")]
		public int[] PrevCarPos;

		/// <summary>
		/// �ֳ������ź�
		/// </summary>
		internal SignalLight SignalLight;


		
		/// <summary>
		/// �źŵ����к���
		/// </summary>
		/// <param name="iCrtTimeStep"></param>
		internal void PlaySignal(int iCrtTimeStep)
		{
			XNode rN = ( this.Container as Way).xNodeTo;
			if (SignalLight == null)//���źŽ����
			{
				rN.UnblockLane(this);
				return;
			}
			if (this.SignalLight.IsGreen(iCrtTimeStep) == false)
			{//��ƻ����ǻƵ�������
				rN.BlockLane(this);
			}
			else//�̵�
			{
				rN.UnblockLane(this);
			}
		}

		public override OxyzPointF ToVector()
		{
			OxyzPoint pA = this.Shape[0];
			OxyzPoint pB = this.Shape[this.Shape.Count - 1];
			return new OxyzPointF(pB._X-pA._X,pB._Y-pA._Y);
		}
		
		#region ���캯��
		
		[System.Obsolete("��ֹʹ�õĹ�����")]
		private Lane()
		{
			this.laneType = LaneType.Straight;
		}

		/// <summary>
		/// ������������������
		/// </summary>
		/// <param name="re"></param>
		internal Lane(Way re):this(re,LaneType.Straight){
		}
		internal Lane(LaneType lt):this(null,lt)
		{
		}
		/// <summary>
		/// û�н����ڲ�ע�ᣬӦ����������ߵ���registere����ע��
		/// </summary>
		/// <param name="re"></param>
		/// <param name="lt"></param>
		internal Lane(Way re,LaneType lt)
		{
			this.PrevCarPos = new int[512];
			this.PrevCarPos[0] = -1;

			Container = re;
			this.laneType = lt;
			this._id = Lane.iLaneCount++;
			
			//��ʼ��cellspace
			this._Grids = new CellSpace(this);

		}
		#endregion
		
		/// <summary>
		/// ��ʱ�ģ����������Ҫ�޸�
		/// </summary>
		/// <param name="ce"></param>
		public void AddCell(Cell ce)
		{
			//��������ֵ��
			ce.Container = this;
			//�޸�����
			ce.Grid = new Point(this.Rank, ce.Grid.Y);

			System.Diagnostics.Debug.Assert(ce.Grid.Y < this.iLastPos);

			this._cells.Enqueue(ce);
		}
		public Cell RemoveCell()
		{
			return this._cells.Dequeue();
		}
		internal LaneType laneType;
		
		private CellQueue _cells = new CellQueue();

		#region ���복�����������һ������cellspace���
		/// <summary>
		/// �ȴ�����ó����ĵȴ�����
		/// </summary>
		private Queue<Cell> _waitedQueue = new Queue<Cell>();

		/// <summary>
		/// ע���������°潫��cellspace���
		/// </summary>
		/// <param name="ce"></param>
		public void EnterWaitedQueue(Cell ce)
		{
			//��������ֵ��
			ce.Container = this;
			this._waitedQueue.Enqueue(ce);
		}
		
		/// <summary>
		/// ���ȴ������е�Ԫ����ӵ�����Ԫ���У��°���cellspace��ʵ�ָù���
		/// </summary>
		private void DisposeWaitedQueue()
		{
			while (this._waitedQueue.Count > 0)
			{
				if (this.iLastPos == 0)//��������Ѿ����˾Ͳ��ܴ��������
				{
					break;
				}
				this.AddCell(this._waitedQueue.Dequeue());
			}
		}
		#endregion
		

		/// <summary>
		/// û�е���visitorģʽ���������и��ӵķ��񡢴���ȴ�����
		/// </summary>
		[System.Obsolete("�������и��ӵķ���ʹ��� �ȴ�����")]
		public override void UpdateStatus()
		{
			base.UpdateStatus();
		}
		protected override void OnStatusChanged()
		{
			this.DisposeWaitedQueue();//����ȴ���Ԫ��

			//���û������־����
			this.InvokeService(this);//������־��¼roadLane����
		}


		#region �������໥�Ƚϵ��㷨�ͺ���
		public int CompareTo(Lane other)
		{
			if (this.laneType == other.laneType)
			{
				return 0;
			}
			return this.laneType > other.laneType ? 1 : -1;
		}
		public int Compare(Lane x, Lane y)
		{
			return x.CompareTo(y);
		}
		/// <summary>
		/// ��̬����
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public static int CompareTo(Lane from, Lane to)
		{
			return from.CompareTo(to);
		}
		#endregion
		public Cell this[int index]
		{
			get
			{
				return this._cells[index];
			}
		}
		public int CellCount
		{
			get
			{
				return this._cells.Count;
			}
		}

		public IEnumerator<Cell> GetEnumerator()
		{
			return this._cells.GetEnumerator();
		}
	}
	
	
//-----------------2015��1��19��-----------------------------------------	
	/// <summary>
	/// 2015��1��19�գ���ԭ����lane���������������ֺ�ԭ�в��ִ���ļ���
	/// </summary>
	public partial class Lane
	{
		/// <summary>
		/// cellspace ���͵�Ԫ������ռ�,��������lane������Ԫ��
		/// </summary>
		private CellSpace _Grids;
		
		/// <summary>
		/// ���ȴ������е�Ԫ����ӵ�����Ԫ���У��°���cellspace��ʵ�ָù���
		/// </summary>
		internal override void ServeMobiles()
		{
//			while (this._mobilesInn.Count > 0)
//			{
//				if (this.SpaceCount == 0)//��������Ѿ����˾Ͳ��ܴ��������
//				{
//					break;
//				}else{
//					this.AddCell(this._mobilesInn.Dequeue());
//				}
//			}
		}

		
		/// <summary>
		/// �жϴӵ�·��㴦���г��ĵط���Ԫ���������
		/// </summary>
		[System.Obsolete("��������")]
		public int SpaceCount
		{
			get
			{
				//���������û��Ԫ��
				if (this.Mobiles.Count==0)
				{//���س������������
					return this._Grids.Count;
				}
				//ʵ����״�����һ����,����ʵ���г���
				MobileEntity me = this.Mobiles.Last.Value;
				//ʵ����״�����һ����,����ʵ���г���
				OxyzPoint op =me.Shape.End;

				CellGrid cg = new CellGrid();
			
				if ( this._Grids.TryGetValue(op.GetHashCode(),out cg)==true) {
					return	cg.iGridIndex;
				}
				return -1;
			}
		}
		
	}
}

