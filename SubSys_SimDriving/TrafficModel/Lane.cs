using System;
using SubSys_MathUtility;
using SubSys_SimDriving;
using System.Collections;
using System.Collections.Generic;
using SubSys_SimDriving;
using System.Drawing;

namespace SubSys_SimDriving.TrafficModel
{
	/// <summary>
	/// ��ͨʵ������ĳ���,ԭ����lane���ݽṹ
	/// </summary>
	public partial class Lane : StaticOBJ, IComparable<Lane>, IComparer<Lane>
	{
		/// <summary>
		/// ȫ�ֵĳ�����������������ʼ������ID
		/// </summary>
		private static int iCount=0;
        
		public override int Length
		{
			get
			{
				return this.Shape.Count;
			}
		}
		public override int Width
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
				return  base.Shape;

				//if (eShape.Count == 0)//shape û�г�ʼ��
				//{
				//	CreateShape(ref eShape);
				//}
				//return eShape;
			}
		}

		/// <summary>
		/// ��ͼ�ν�������ϵ��ת��ΪԪ������ϵ,create a shape of a lane
		/// </summary>
		/// <param name="eShape"></param>
		internal  void CreateShape()
		{
			EntityShape prant = this.Container.Shape;
			var pNorm = VectorTool.GetNormal(this.Container.ToVector());
            foreach (var point in prant)
            {
                int iScaler = this.Rank;
                var newPoint = Coordinates.Offset(point, OxyzPointF.Muilt(pNorm,iScaler));
                this.Shape.Add(newPoint);
          }
        }
        //private void CreateShape(ref EntityShape eShape)
        //{
        //    EntityShape esContainer = this.Container.Shape;

        //    var pNorm = VectorTools.GetNormal(this.Container.ToVector());

        //    //the first point
        //    var pFirst = esContainer.Start;
        //    //the end point
        //    var pFEnd = esContainer.End;

        //    double dDistance = Coordinates.Distance(pFirst, pFEnd);
        //    //int loopCount = Convert.ToInt32( dDistance);//����Ϊ100������Ϊ100�ȷ�

        //    //each split  x is 1 int; each split y is 1 int
        //    int iLoop = Convert.ToInt32(dDistance);

        //    double xSplit = (pFEnd._X - pFirst._X) / dDistance;//������������
        //    double ySplit = (pFEnd._Y - pFirst._Y) / dDistance;//������������

        //    eShape.Add(pFirst);

        //    var opCurr = new OxyzPointF(0, 0, 0);
        //    //	var opPrev = new OxyzPointF(0,0,0);

        //    for (int i = 1; i < iLoop; i++)//x�� iLoop to make sure each simulate step move equal distance
        //    {
        //        double dX = xSplit * i;
        //        double dY = ySplit * i;
        //        //�������� would miss accuracy
        //        //				int iX = Convert.ToInt32(Math.Round((decimal)dX,0,MidpointRounding.AwayFromZero));
        //        //				int iY = Convert.ToInt32(Math.Round((decimal)dY,0,MidpointRounding.AwayFromZero));
        //        opCurr = new OxyzPointF(pFirst._X + dX, pFirst._Y + dY);
        //    }

        //    eShape.Add(pFEnd);
        //}

        /// <summary>
        /// û�е��ã���ʱ����д
        /// </summary>
        /// <param name="iAheadSpace"></param>
        /// <returns></returns>
        internal bool IsLaneBlocked(int iAheadSpace)
		{
			//return this.iLastPos-1 <= iAheadSpace ;
			return false;
		}
		/// <summary>
		/// �ѹ�ʱ�����������һ��Ԫ����λ�ã�Ӧ����Y����
		/// </summary>
//		private int _ilastPos;
//		/// <summary>
//		/// �ѹ�ʱ����ȡ���������һ��Ԫ����λ�ã����û��Ԫ���򷵻س�������
//		/// </summary>
//		[System.Obsolete("�ѹ�ʱ")]
//		internal int iLastPos
//		{
//			get
//			{
//				Cell ce = this._cells.PeekLast();
//				if (ce== null)
//				{
//					this._ilastPos = this.iLength;
//				}
//				else //if (this._ilastPos > ce.RltPos.Y)
//				{
//					this._ilastPos = ce.Grid.Y;
//				}
//				return _ilastPos;
//
//			}
//		}

		/// <summary>
		///���������򣬴��ڲ೵����ʼ�ĵڼ�������,�����Գ����������򣬵�һ��������Rank ��1
		/// </summary>
		public int Rank;

//		[System.Obsolete("�ѹ�ʱ����¼�����������λ�õ����飬�µ�Ԫ���ռ����۽�����ֹͣʹ�ø�����")]
//		public int[] PrevCarPos;

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
				this.bBlocked =false;
				return;
			}
			if (this.SignalLight.IsGreen(iCrtTimeStep) == false)
			{//��ƻ����ǻƵ�������
				this.bBlocked =true;
			}
			else//�̵�
			{
				this.bBlocked =false;
			}
		}

		/// <summary>
		/// math coordinates
		/// </summary>
		/// <returns></returns>
        [System.Obsolete("to avoid warning")]
		public override OxyzPointF ToVector()
		{
			return this.Shape.End-this.Shape.Start;
		}
		
		#region ���캯��
		
		//[System.Obsolete("��ֹʹ�õĹ�����")]
		private Lane()
		{
			this.laneType = LaneType.Straight;
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
//			this.PrevCarPos = new int[512];
//			this.PrevCarPos[0] = -1;

			Container = re;
            this.EntityType = EntityType.Lane;

			this.laneType = lt;
			this._entityID = ++Lane.iCount;
			//id property is setted by base.
			//this.ID


		}
		#endregion
		
		/// <summary>
		/// ��ʱ�ģ����������Ҫ�޸�
		/// </summary>
		/// <param name="ce"></param>
//		[System.Obsolete("��ʱ�ģ����������Ҫ�޸�")]
//		public void AddCell(Cell ce)
//		{
//			//��������ֵ��
//			ce.Container = this;
//			//�޸�����
//			ce.Grid = new Point(this.Rank, ce.Grid.Y);
//
//			System.Diagnostics.Debug.Assert(ce.Grid.Y < this.iLastPos);
//
//			this._cells.Enqueue(ce);
//		}
		
//		[System.Obsolete("��ʱ�ģ����������Ҫ�޸�")]
//		public Cell RemoveCell()
//		{
//			return this._cells.Dequeue();
//		}
		internal LaneType laneType;
		
		//	[System.Obsolete("��ʱ�ģ��������������Ҫ")]
		//	private CellQueue _cells = new CellQueue();

		#region ���복�����������һ������cellspace���
		/// <summary>
		/// �ȴ�����ó����ĵȴ����У���ʱ����mobileInn����
		/// </summary>
		//	private Queue<Cell> _waitedQueue = new Queue<Cell>();

		/// <summary>
		///��ʱ��
		/// </summary>
		/// <param name="ce"></param>
//		[System.Obsolete("��ʱ�ģ��°��ɸ����MobilesInn����")]
//		public void EnterWaitedQueue(Cell ce)
//		{
//			//��������ֵ��
//			ce.Container = this;
//			this._waitedQueue.Enqueue(ce);
//
//		}
		
		/// <summary>
		/// ���ȴ������е�Ԫ����ӵ�����Ԫ���У��°���cellspace��ʵ�ָù���
		/// </summary>
//		[System.Obsolete("��ʱ�ģ��°��ɸ����MobilesInn����")]
//		private void DisposeWaitedQueue()
//		{
//			while (this._waitedQueue.Count > 0)
//			{
//				if (this.iLastPos == 0)//��������Ѿ����˾Ͳ��ܴ��������
//				{
//					break;
//				}
//				this.AddCell(this._waitedQueue.Dequeue());
//			}
//		}
		#endregion
		

		/// <summary>
		/// û�е���visitorģʽ���������и��ӵķ��񡢴���ȴ�����
		/// </summary>
		//[System.Obsolete("�������и��ӵķ���ʹ��� �ȴ�����")]
		public override void UpdateStatus()
		{
			base.UpdateStatus();
		}
		
		protected override void OnStatusChanged()
		{
			//this.DisposeWaitedQueue();//����ȴ���Ԫ��

			this.ServeMobiles();
			//���û������־����
			this.InvokeServices(this);//������־�����¼roadLane����

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
//		[System.Obsolete("��ʱ�ģ��°�����Cell")]
//		public MobileEntity this[int index]
//		{
//			get
//			{
//				return null;// this._cells[index];
//			}
//		}
		
//		/// <summary>
//		/// obselete
//		/// </summary>
//		[System.Obsolete("obsolete,replace with mobiles")]
//		public int CellCount
//		{
//			get
//			{
//				return this._cells.Count;
//			}
//		}

//		public IEnumerator<Cell> GetEnumerator()
//		{
//			return this._cells.GetEnumerator();
//		}
	}


	//-----------------2015��1��19��-----------------------------------------
	/// <summary>
	/// 2015��1��19�գ���ԭ����lane���������������ֺ�ԭ�в��ִ���ļ���
	/// </summary>
	public partial class Lane
	{
//		/// <summary>
//		/// cellspace ���͵�Ԫ������ռ�,��������lane������Ԫ��
//		/// </summary>
//		private CellSpace _Grids;

		private LinkedList<MobileOBJ> _mobiles ;
		/// <summary>
		/// ��������洢�ڽ���ںͳ������ڲ��ĳ�����
		/// </summary>
		public LinkedList<MobileOBJ> Mobiles {
			get {
				if (this._mobiles==null) {
					this._mobiles = new LinkedList<MobileOBJ>();
				}
				return this._mobiles;
			}
		}
		
		/// <summary>
		/// ���ȴ������е�Ԫ����ӵ�����Ԫ����
		/// </summary>
		internal override void ServeMobiles()
		{
			//as long as theres space for mobile to enter ,serve this mobile
			while(this.MobilesInn.Count>0)
			{
				var mobile = this.MobilesInn.Peek();
				//if (this.LaneSpace>mobile.iLength) {
				this.Mobiles.AddLast(this.MobilesInn.Dequeue());
				//}
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
					return this.Length;//Shape.Count;
				}
				//ʵ����״�����һ����,����ʵ���г���
				MobileOBJ me = this.Mobiles.Last.Value;
				//ʵ����״�����һ����,����ʵ����
				return this.Shape.GetIndex(me.Shape.End);
			}
		}
		
		
		/// <summary>
		///The Left lane of current lane
		/// </summary>
		public Lane Left
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
		public Lane Right
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
		
		private bool bBlocked=false;

		public  bool IsBlocked
		{
			get{
				return this.bBlocked;
			}
		}
		
		

		
	}

	//_______________2016��1�����������ݣ�ԭ�еĳ�Ա�ͷ����������ַ���
//	public class MobilesShelter
//	{
//		internal StaticEntity _Container;
//
//		internal MobilesShelter(StaticEntity container)
//		{
//			this._Container = container;
//		}
//		/// <summary>
//		/// ��ֹ�����޲������캯��
//		/// </summary>
//		private MobilesShelter()
//		{
//		}
//
//
//		private LinkedList<MobileEntity> _mobiles = new LinkedList<MobileEntity>();
//
//		/// <summary>
//		/// ��һ���������ǵ�һ��
//		/// </summary>
//		public LinkedList<MobileEntity> Mobiles
//		{
//			get{return this._mobiles;}
//		}
//
	////		protected void Enter(MobileEntity me)
	////		{
//		////			int iShapeCount =this._mobiles.Count;
//		////
//		////
//		////			//���������û�г����������������롣
//		////			//�������Ԫ���ռ��ĩβ�п��೵λ���ҿ���ռ���ڳ������ȣ�������������
//		////			if (this._cellSpace.iSpace>me.Shape.Count) {
//		////
//		////				//����Ҫ����������1���޸�Ԫ���ռ��״̬����Ԫ���ռ䱻ռ���ˡ�2���޸ĳ�����״�����ꡣ
//		////				this._mobiles.AddLast(me);
//		////
//		////				//�޸ĳ�����Ԫ������ϵ
//		////				for (int i = 0; i < me.Shape.Count; i++) {
//		////		//			me.Shape[i]=this._Container.Shape[i];//container ��lane
//		////		//			CellGrid cg = new CellGrid(me.Shape[i],true);
//		////					//�޸�Ԫ���ռ������
//		////					this._cellSpace.Add(cg);
//		////				}
	////		//	}
	////
	////			//�ѳ�������
	////		//	this._mobiles.AddLast(me);
	////
	////		}
	////		/// <summary>
	////		/// �����˳����������������ֶ�������Ҫ����͸��¡�
	////		/// </summary>
	////		/// <param name="me"></param>
	////		protected void Exit(MobileEntity me)
	////		{
	////			this._mobiles.RemoveFirst();
	////
	////			for (int i = 0; i < me.Shape.Count; i++)
	////			{
	////				//this._cellSpace.Remove(me.Shape[i].GetHashCode());
	////			}
	////		}
//
//		protected bool Move(int iStepForward)
//		{
//			return false;
//		}
//
//		protected bool IsEmpty()
//		{
//			return false;
//		}
//
//
//
//
//
//	}
}

