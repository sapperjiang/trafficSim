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
	public class XNode : StaticOBJ
	{
        
		/// <summary>
		/// �µ�roadnode�Ĺ�ϣɢ��ֵ��������Position�Ĺ�ϣֵ����ID����
		/// </summary>
		/// <returns></returns>
		private HashMatrix _mobiles; 
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

		

		/// <summary>
		/// �������ڵ����г��ߵĹ�ϣ����ֵ�Ǵ���ߵ�Way��ϣ��ֵ�Ǵ���Way
		/// </summary>
		private Dictionary<int, Way> _dicEdges = new Dictionary<int,Way>();

		/// <summary>
		/// �ṩ�Թ�ϣ�����ڲ�Ԫ�صı���
		/// </summary>
		/// <returns></returns>
		public IEnumerator<MobileOBJ> GetEnumerator()
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
		/// ע���ڳ��߱��У�����Way��from�ֶ���this�ڵ㣬�������׳��쳣
		/// </summary>
		/// <param name="Way"></param>
		internal void AddWay(Way way)
		{
          //  System.Diagnostics.Debug.Assert(way != null);
            
			if (!Contains(way.GetHashCode()))
			{
                //�����ж��Ƿ��ǵ�ǰ��ĳ��ߵ���Ϣ��ֹ����
                System.Diagnostics.Debug.Assert(way.From == this);
                //{
                //                throw new Exception("����˲����ڸö���ı�");
                //}
                var iLaneCount = way.Width;//.Lanes.Count;
                base.Length = iLaneCount > base.Length ? iLaneCount : base.Length;

				_dicEdges.Add(way.GetHashCode(), way);
			}
			else
			{
				throw new ArgumentException("������ظ��ıߣ�");
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
		/// ����XNodeID������
		/// </summary>
		private static int iXNodeID;
		[System.Obsolete("ʹ���в����Ĺ��캯��")]
		private XNode()
		{
			this._entityID = ++iXNodeID;
            this.EntityType = EntityType.XNode;

			Random rd = new Random();
            

			this.GISGrid = new OxyzPointF(rd.Next(65535), rd.Next(65535));
			// ֱ��ʹ�������ĵ����ݽṹ,bug��Ӧ��ʹ�������Ľṹ
			if (this.GISGrid._X == 0.0f && this.GISGrid._Y == 0.0f)
			{
				ThrowHelper.ThrowArgumentNullException("RoadNode�����������꣡");
			}
		}

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
				mobile.Run(this as StaticOBJ);
				mobileNode = mobileNode.Next;
			}
			this.ServeMobiles();
			base.UpdateStatus();//���������OnStatusChanged ���л�ͼ����
		}

		protected override void OnStatusChanged()
		{
			//call its base's method to run services registered on this entity
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
            this._entityID = ++iXNodeID;
            this.EntityType = EntityType.XNode;
			this.SpatialGrid = pointCenter;


            Random rd = new Random();
            this.GISGrid = new OxyzPointF(rd.Next(65535), rd.Next(65535));
            // ֱ��ʹ�������ĵ����ݽṹ,bug��Ӧ��ʹ�������Ľṹ
            if (this.GISGrid._X == 0.0f && this.GISGrid._Y == 0.0f)
            {
                ThrowHelper.ThrowArgumentNullException("RoadNode�����������꣡");
            }

        }


     
        /// <summary>
        ///    /////[System.Obsolete("should be restructed")]
        /// </summary>
        public override int Length
        {
            get
            {
               return base.Length;
             //  return this.Ways.Count;
              //  return this.Shape.Count;
            }
        }
        public override int Width
        {
            get
            {
                return SimSettings.iCarWidth;
            }
        }

    }
}

