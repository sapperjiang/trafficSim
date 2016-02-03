using System;
using SubSys_MathUtility;
using SubSys_SimDriving;
using System.Collections;
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

		
		public override int iLength
		{
			get
			{
				return this.Shape.Count;
			}
		}
		public override int iWidth
		{
			get
			{
				return SimSettings.iCarWidth;
			}
		}

		/// <summary>
		/// ���û������ͼ�йص�һϵ�е�ռ���ɡ�
		/// ������ʼ�ĵص㣬�ǽ���ڵ�ǰ������һ���㣬��������С�����������ĵص㣬�ǽ���ڵĺ󷽣�һ����������ĩһ������������
		/// �����Ĵ�С�복���ķ����෴��
		/// </summary>
		public override EntityShape Shape
		{
			get
			{
				var eShape = base.Shape;

				if (eShape.Count == 0)//shape û�г�ʼ��
				{
					CreateShape(ref eShape);
				}
				return eShape;
			}
		}

		int iTest = 0;
		
		/// <summary>
		/// ��ͼ�ν�������ϵ��ת��ΪԪ������ϵ,create a shape of a lane 
		/// </summary>
		/// <param name="eShape"></param>
		//[System.Obsolete("replace with new item,this is for use for old cellsimulation model ")]
		private  void CreateShape(ref EntityShape eShape)
		{
			EntityShape es = this.Container.Shape;

			OxyzPointF pNorm = VectorTools.GetNormal(this.Container.ToVector());
			OxyzPointF mpOffset = new OxyzPointF(pNorm._X*(this.Rank - 0.5f),pNorm._Y * (this.Rank - 0.5f));
			//the first point //ƽ������
			OxyzPointF pFirst = Coordinates.Offset(es[0], mpOffset);
			//the end point //�����յ�
			OxyzPointF pFEnd = Coordinates.Offset(es[es.Count - 1], mpOffset);
			
			//for each point its x and y is int,not float or double
			OxyzPointF mp = new OxyzPointF(pFEnd._X-pFirst._X,pFEnd._Y-pFirst._Y);
			
			double dDistance = Coordinates.Distance(pFirst,pFEnd) ;
			//int loopCount = Convert.ToInt32( dDistance);//����Ϊ100������Ϊ100�ȷ�

			//each split  x is 1 int; each split y is 1 int
			int iLoopCount =Convert.ToInt32(dDistance);
			
			double xSplit = mp._X / dDistance;//������������
			double ySplit = mp._Y / dDistance;//������������
			
			eShape.Add(pFirst);
			OxyzPointF opf;
			
			int iBase=0;
			for (int i = 1; i < iLoopCount; i++)//x��
			{
				double dX = xSplit*(i-iBase);
				double dY = ySplit*(i-iBase);
				
				int iX = Convert.ToInt32( Math.Round((decimal)dX,0,MidpointRounding.AwayFromZero));
				int iY = Convert.ToInt32(Math.Round((decimal)dY,0,MidpointRounding.AwayFromZero));
				
				if ( iX==1||iY==1) {
					opf = new OxyzPointF(pFirst._X + i*iX, pFirst._Y+i*iY);
					eShape.Add(opf);
					iBase = i;
				}
			}

			eShape.Add(pFEnd);
			
			System.Diagnostics.Debug.Assert(pFEnd._X >=0f);
//			if (pFEnd._X==-1) {
//				throw new Exception();
//			}
		}
		//	private bool bDebug=true;
		
		
		
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
		///���������򣬴��ڲ೵����ʼ�ĵڼ�������,�����Գ����������򣬵�һ��������Rank ��1
		/// </summary>
		public int Rank;

		[System.Obsolete("�ѹ�ʱ����¼�����������λ�õ����飬�µ�Ԫ���ռ����۽�����ֹͣʹ�ø�����")]
		public int[] PrevCarPos;

		/// <summary>
		/// �ֳ������ź�
		/// </summary>
		internal SignalLight SignalLight;


		
		/// <summary>
		/// �źŵ����к���,modified on date 2016/1/27
		/// </summary>
		/// <param name="iCrtTimeStep">���ʱ��</param>
		internal void PlaySignal(int iCrtTimeStep)
		{
			
			if (SignalLight == null)//���źŽ����
			{
				this.bLaneBlocked =false;
				return;
			}
			if (this.SignalLight.IsGreen(iCrtTimeStep) == false)
			{//��ƻ����ǻƵ�������
				this.bLaneBlocked =true;
			}
			else//�̵�
			{
				this.bLaneBlocked =false;
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
			this._EntityID = Lane.iLaneCount++;
			
			//��ʼ��cellspace
			this._Grids = new CellSpace(this);

		}
		#endregion
		
		/// <summary>
		/// ��ʱ�ģ����������Ҫ�޸�
		/// </summary>
		/// <param name="ce"></param>
		[System.Obsolete("��ʱ�ģ����������Ҫ�޸�")]
		public void AddCell(Cell ce)
		{
			//��������ֵ��
			ce.Container = this;
			//�޸�����
			ce.Grid = new Point(this.Rank, ce.Grid.Y);

			System.Diagnostics.Debug.Assert(ce.Grid.Y < this.iLastPos);

			this._cells.Enqueue(ce);
		}
		
		[System.Obsolete("��ʱ�ģ����������Ҫ�޸�")]
		public Cell RemoveCell()
		{
			return this._cells.Dequeue();
		}
		internal LaneType laneType;
		
		[System.Obsolete("��ʱ�ģ��������������Ҫ")]
		private CellQueue _cells = new CellQueue();

		#region ���복�����������һ������cellspace���
		/// <summary>
		/// �ȴ�����ó����ĵȴ����У���ʱ����mobileInn����
		/// </summary>
		private Queue<Cell> _waitedQueue = new Queue<Cell>();

		/// <summary>
		///��ʱ��
		/// </summary>
		/// <param name="ce"></param>
		[System.Obsolete("��ʱ�ģ��°��ɸ����MobilesInn����")]
		public void EnterWaitedQueue(Cell ce)
		{
			//��������ֵ��
			ce.Container = this;
			this._waitedQueue.Enqueue(ce);
			
		}
		
		/// <summary>
		/// ���ȴ������е�Ԫ����ӵ�����Ԫ���У��°���cellspace��ʵ�ָù���
		/// </summary>
		[System.Obsolete("��ʱ�ģ��°��ɸ����MobilesInn����")]
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
			//this.DisposeWaitedQueue();//����ȴ���Ԫ��

			this.ServeMobiles();
			//���û������־����
			this.InvokeService(this);//������־�����¼roadLane����
			
			
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
		
		/// <summary>
		/// this index operator need to be modified
		/// </summary>
		[System.Obsolete("��ʱ�ģ��°�����Cell")]
		public Cell this[int index]
		{
			get
			{
				return this._cells[index];
			}
		}
		
		/// <summary>
		/// obselete
		/// </summary>
		[System.Obsolete("obsolete,replace with mobiles")]
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
		/// ���ȴ������е�Ԫ����ӵ�����Ԫ����
		/// </summary>
		internal override void ServeMobiles()
		{
			//as long as theres space for mobile to enter ,serve this mobile
			if (this.MobilesInn.Count>0) {
				var mobile = this.MobilesInn.Peek();
				while(this.LaneSpace>mobile.iLength)
				{
					this.Mobiles.AddLast(this.MobilesInn.Dequeue());
				}
			}		
		}

		
		/// <summary>
		/// �жϴӵ�·��㴦���г�ռ�ݵĵط���Ԫ���������
		/// </summary>
		[System.Obsolete("��������")]
		public int LaneSpace
		{
			get
			{
				//���������û��Ԫ��
				if (this.Mobiles.Count==0)
				{//���س������������
					return this.iLength;//Shape.Count;
				}
				//ʵ����״�����һ����,����ʵ���г���
				MobileEntity me = this.Mobiles.Last.Value;
				//ʵ����״�����һ����,����ʵ����
				return this.Shape.GetIndex(me.Shape.End);
			}
		}
		
		
		/// <summary>
		///The Left lane of current lane
		/// </summary>
		public Lane LeftLane
		{
			get {
				
				Way way = this.Container as Way;
				
				//the first lane in way is current lane,it has no left lane
				if (this.Rank==1) {
					return null;
				}
				//���ǵ�һ�����������ǵڶ������ϵĳ������������Rank ҪСһ�Ų�����೵��
				return way.Lanes[this.Rank-2];
			}
		}
		
		/// <summary>
		/// the right lane of current lane
		/// </summary>
		public Lane RightLane
		{
			get {
				
				Way way = this.Container as Way;
				
				//the last lane in way is current lane,it has no right lane
				if (this.Rank==way.Lanes.Count) {
					return null;
				}
				//���ǵ�һ�����������ǵڶ������ϵĳ������������Rank ҪСһ�Ų�����೵��
				return way.Lanes[this.Rank];
			}
		}
		
		private bool bLaneBlocked=false;

		public  bool IsBlocked
		{
			get{
				return this.bLaneBlocked;
			}
		}
		
	}

	//_______________2016��1�����������ݣ�ԭ�еĳ�Ա�ͷ����������ַ���
	public class MobilesShelter
	{
		internal StaticEntity _Container;
		/// <summary>
		/// cellspace ���͵�Ԫ������ռ�,��������lane������Ԫ��
		/// </summary>
		private CellSpace _cellSpace;
		
		internal MobilesShelter(StaticEntity container)
		{
			this._Container = container;
			this._cellSpace= new CellSpace(container);
		}
		/// <summary>
		/// ��ֹ�����޲������캯��
		/// </summary>
		private MobilesShelter()
		{
		}
		
		
		private LinkedList<MobileEntity> _mobiles = new LinkedList<MobileEntity>();
		
		/// <summary>
		/// ��һ���������ǵ�һ��
		/// </summary>
		public LinkedList<MobileEntity> Mobiles
		{
			get{return this._mobiles;}
		}
		
		protected void Enter(MobileEntity me)
		{
			int iShapeCount =this._mobiles.Count;
			
			
			//���������û�г����������������롣
			//�������Ԫ���ռ��ĩβ�п��೵λ���ҿ���ռ���ڳ������ȣ�������������
			if (this._cellSpace.iSpace>me.Shape.Count) {
				
				//����Ҫ����������1���޸�Ԫ���ռ��״̬����Ԫ���ռ䱻ռ���ˡ�2���޸ĳ�����״�����ꡣ
				this._mobiles.AddLast(me);
				
				//�޸ĳ�����Ԫ������ϵ
				for (int i = 0; i < me.Shape.Count; i++) {
					me.Shape[i]=this._Container.Shape[i];//container ��lane
					CellGrid cg = new CellGrid(me.Shape[i],true);
					//�޸�Ԫ���ռ������
					this._cellSpace.Add(cg);
				}
			}
			
			//�ѳ�������
			this._mobiles.AddLast(me);
			
		}
		/// <summary>
		/// �����˳����������������ֶ�������Ҫ����͸��¡�
		/// </summary>
		/// <param name="me"></param>
		protected void Exit(MobileEntity me)
		{
			this._mobiles.RemoveFirst();
			
			for (int i = 0; i < me.Shape.Count; i++)
			{
				this._cellSpace.Remove(me.Shape[i].GetHashCode());
			}
		}
		
		protected bool Move(int iStepForward)
		{
			return false;
		}
		
		protected bool IsEmpty()
		{
			return false;
		}
		

	}
}

