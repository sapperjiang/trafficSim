using SubSys_SimDriving;

using System.Collections.Generic;
using System;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving
{
	/// <summary>
	/// 表示交叉口XNode，路段way，车道Lane,道路Road的基类型
	///  智能体的更新使用了访问者模式
	/// </summary>
	public abstract partial class StaticOBJ:TrafficOBJ
	{
		
		//_______________2015年1月新增的内容，原有的成员和方法将被部分废弃________________
		/// <summary>
		/// _2015年1月新增的内容，原有的成员和方法将被部分废弃
		/// </summary>


		#region 处理等待的mobileentity的数据结构和函数
		private Queue<MobileOBJ> _mobilesInn = new Queue<MobileOBJ>();

		/// <summary>
		/// 等待进入实体的道的等待mobileEntity，实际上是等待队列，交叉口和车道都有。
		/// 真正存储车辆位置的容器，因为车道与交叉口不同，在各自内部用不用的数据结构实现
		/// </summary>
		public Queue<MobileOBJ> MobilesInn {
			get {
				return _mobilesInn;
			}
		}

		/// <summary>
		/// 用于处理暂时无法进入车道的车辆的临时队列，每个交叉口更新周期，须进入车道的元胞的缓存，在下一个道路更新周期，将缓存纳入车道。基类的方法，应当由子类。lane和Xnode重写；
		/// </summary>
		/// <param name="me"></param>
		public virtual void EnterInn(MobileOBJ me)
		{
			//给mobile容器赋值；
			me.Container = this;		
			me.Shape.Add(this.Shape.Start);
			
			this._mobilesInn.Enqueue(me);
		}
		
		/// <summary>
		/// 抽象方法、需要lane和xnode实现，将等待队列中的mobile添加到update mobile
		/// </summary>
		internal virtual void ServeMobiles(){}
		
		#endregion

		
	}
	
	
}

