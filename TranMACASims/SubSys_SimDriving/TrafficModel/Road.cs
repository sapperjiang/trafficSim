using System;
using System.Collections.Generic;
using System.Text;
using SubSys_SimDriving.TrafficModel;
using System.Drawing;
using SubSys_MathUtility;

namespace SubSys_SimDriving.TrafficModel
{
	/// <summary>
	/// 路段接口，包括两个或者一个RoadEdge。两个RoadNode,
	/// 他们之间的关系由工厂创建时候负责维护
	/// </summary>
	public  interface IRoad
	{
		Way Way
		{
			get;
			set;
		}
		/// <summary>
		/// 对向道路
		/// </summary>
		Way CtrWay
		{
			get;
			set;
		}
		XNode XNode
		{
			get;
			set;
		}
		/// <summary>
		/// 对向节点
		/// </summary>
		XNode CtrXNode
		{
			get;
			set;
		}
	}
	/// <summary>
	/// 包含两个way的道路。如果是单行路，只包含一个way
	/// </summary>
	public class Road:StaticEntity,IRoad
	{
		private Way _way;
		private Way _ctrWay;
		private XNode _XNode;
		private XNode _ctrXNode;

		private Road()
		{
			this.EntityType = EntityType.Road;
		}

		internal Road(OxyzPointF start,OxyzPointF end)
		{
			this.EntityType = EntityType.Road;	
		}
		public Way Way
		{
			get
			{
				return this._way;
			}
			set
			{
				this._way = value;
			}
		}

		public Way CtrWay
		{
			get
			{
				return this._ctrWay;
			}
			set
			{
				this._ctrWay = value;
			}
		}

		public XNode XNode
		{
			get
			{
				return this._XNode;
			}
			set
			{
				this._XNode = value;
			}
		}

		public XNode CtrXNode
		{
			get
			{
				return this._ctrXNode;
			}
			set
			{
				this._ctrXNode = value;
			}
		}
	}
}
