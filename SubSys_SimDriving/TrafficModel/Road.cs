using SubSys_MathUtility;
using SubSys_SimDriving.TrafficModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SubSys_SimDriving.TrafficModel
{
	/// <summary>
	/// 路段接口，包括两个或者一个Way。两个RoadNode,
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
		XNode From
		{
			get;
			//set;
		}
		/// <summary>
		/// 对向节点
		/// </summary>
		XNode To
		{
			get;
			//set;
		}
	}
	/// <summary>
	/// 包含两个way的道路。如果是单行路，只包含一个way
	/// </summary>
	public class Road:StaticEntity,IRoad
	{
		private Way way;
		private Way ctrWay;

		internal Road()
		{
			this.EntityType = EntityType.Road;
		}
		public Way Way
		{
			get
			{
				return this.way;
			}
			set
			{
				this.way = value;
                this.way.Container = this;
			}
		}

		public Way CtrWay
		{
			get
			{
				return this.ctrWay;
			}
			set
			{
				this.ctrWay = value;
                this.ctrWay.Container = this;
            }
		}

		public XNode From
		{
			get
			{
                return this.Way.From;// _XNode;
			}
        }

		public XNode To
		{
			get
			{
                return this.way.To;
			}
		}
	}
}
