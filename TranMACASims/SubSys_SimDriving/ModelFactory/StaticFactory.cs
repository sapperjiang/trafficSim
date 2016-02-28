using System;
using System.Collections.Generic;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving;
using SubSys_SimDriving.Agents;
using SubSys_SimDriving;
using SubSys_MathUtility;

namespace SubSys_SimDriving
{
	public partial class StaticFactory:IFactory
	{
		/// <summary>
		/// create xnode way road and lane.
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <param name="et"></param>
		/// <returns></returns>
		public StaticEntity Build(OxyzPointF start,OxyzPointF end, EntityType et)
		{
				var net  =  RoadNet.GetInstance();
			switch (et)
			{

				case EntityType.Road:
					var road =  new Road(start,end);

					road.Way=this.Build(start,end,EntityType.Way) as Way;
					road.CtrWay=this.Build(end,start,EntityType.Way) as Way;					
					road.Container= net;
					net._Roads.Add(road.GetHashCode(),road);//register
			
					return road;
					
				///avoid to create a xnode 	
				case EntityType.XNode:
					var node = new XNode(start);

					node.Container = net;
					net._XNodes.Add(node.GetHashCode(),node);//register
					
					break;

				case EntityType.Way:
					var way =  new Way(start,end);
					
					way.Container = net;
					
					net._Ways.Add(way.GetHashCode(),way);//register

					break;
				case EntityType.Lane:

					throw new Exception("lane should be builded by way");
					break;

//				case EntityType.RoadNet:
//					return net;

//				case EntityType.SignalLight:
//					var silight =  new SignalLight();
//					net.s
//					//break;
//				case EntityType.VMSEntity:
//					return new VMSEntity();
					//break;
				default:
					break;
			}
			throw new  ArgumentException("无法创建参数指定的构造型");
		}
	}

	public partial class MobileFactory//:IFactory
	{
		public AbstractAgent Build(BuildCommand bc, AgentType et)
		{
			throw new NotImplementedException();
		}

//		[System.Obsolete("注意修改函数")]
//		public MobileEntity Build(BuildCommand bc, EntityType et)
//		{
//			switch (et)
//			{
//				case EntityType.RoadNet:
//					return RoadNet.GetInstance();
//					//break;
//				case EntityType.SignalLight:
//					return new SignalLight();
//					//break;
//				case EntityType.VMSEntity:
//					return new VMSEntity();
//					//break;
//				case EntityType.Road:
//					return new Road();
//					
//				case EntityType.XNode:
//					///  XNodeBuildCmd rn = bc as XNodeBuildCmd;
//					return null;//new XNode(rn.rltPos);
//
//				case EntityType.Way:
//					//return new RoadEdge();
//					throw new NotImplementedException("无法创建参数指定的构造型");
//
//				case EntityType.Lane:
//					LaneBuildCmd rlbc= bc as LaneBuildCmd;
//					if (rlbc == null)
//					{
//						return new Lane(LaneType.StraightRight);
//					}
//					else
//					{
//						return new Lane(rlbc.laneType);
//					}
//				default:
//					break;
//			}
//			throw new  ArgumentException("无法创建参数指定的构造型");
//		}
	}
//
//	public class WayFactory//: AbstractModelFactory
//	{
//
//		/// <summary>
//		/// 创建单方向道路
//		/// </summary>
//		/// <param name="roadNodeFrom"></param>
//		/// <param name="roadNodeTo"></param>
//		/// <param name="iLeftCount">左转车道数目</param>
//		/// <param name="iStraightCount">直行车道数</param>
//		/// <param name="iRightCount">右转车道数</param>
//		/// <returns></returns>
//		public static Way BuildOneWay(OxyzPointF start,OxyzPointF end,int iLeftCount, int iStraightCount, int iRightCount)
//		{
//			Way  re=new Way(start,end);// eModelFactory.b
//
//			for (int i = 0; i < iLeftCount; i++)//左。
//			{
//				re.AddLane(LaneType.Left);
//			}
//			for (int i = 0; i < iStraightCount; i++)//直行
//			{
//				re.AddLane(LaneType.Straight);
//			}
//			for (int i = 0; i < iRightCount; i++)//右转
//			{
//				re.AddLane(LaneType.Right);
//			}
//			return re;
//		}
//
//
//		/// <summary>
//		/// 创建对称的两个RoadEdge，里面的车道数量由参数指定
//		/// </summary>
//		/// <param name="iLeftCount"></param>
//		/// <param name="iStraightCount"></param>
//		/// <param name="iRightCount"></param>
//		/// <returns></returns>
//		public static void BuildTwoWay(Way re,int iLeftCount,int iStraightCount,int iRightCount)
//		{
//			if(iLeftCount+iStraightCount+iRightCount >=SimSettings.iMaxLanes)
//			{
//				throw new ArgumentOutOfRangeException("无法创建超过"+SimSettings.iMaxLanes.ToString()+"个车道！");
//			}
//
//			for (int i = 0; i < iLeftCount; i++)//左。
//			{
//				re.AddLane(LaneType.Left);
//				//re.GetReverse().AddLane(LaneType.Left);
//			}
//			for (int i = 0; i < iStraightCount; i++)//直行
//			{
//				re.AddLane(LaneType.Straight);
//				//re.GetReverse().AddLane(LaneType.Straight);
//			}
//			for (int i = 0; i < iRightCount; i++)//右转
//			{
//				re.AddLane(LaneType.Right);
//				//re.GetReverse().AddLane(LaneType.Right);
//			}
//
//		}
//	}

	public class AgentFactory //: IFactory
	{
		public AbstractAgent Build(BuildCommand bc, AgentType et)
		{
			switch (et)
			{
				case AgentType.DecelerateAgent:
					throw new NotImplementedException();
					//break;
				case AgentType.CollisionAvoidingAgent:
					throw new NotImplementedException();
					//break;
				case AgentType.LaneShiftAgent:
					throw new NotImplementedException();
					//break;
					//case AgentType.SpeedUpDownAgent:
					//    return new SpeedUpDownAgent();
					//break;
				case AgentType.SignalLightAgent:
					return new SignalLightAgent();
					//break;
				default:
					throw new NotImplementedException();
					//break;
			}
		}
		
		public TrafficEntity Build(BuildCommand bc, EntityType et)
		{
			throw new NotImplementedException();
		}
	}
	

//		public static new bool IsServiceUp = true;//服务运行的开关变量,系统关键服务不应当停止
//		
//				SignalLight sg = tVar as SignalLight;
//				if (sg != null)
//				{
//					sg.EntityType = EntityType.SignalLight;
//					//sg.Grid = new Point(0, 0);
//					ISimCtx.SignalLights.Add(sg.GetHashCode(), sg);
//					return;
//				}
//				VMSEntity ve = tVar as VMSEntity;
//				if (ve != null)
//				{
//					ve.EntityType = EntityType.VMSEntity;
//					//	ve.Grid = new Point(0, 0);
//					ISimCtx.VMSEntities.Add(sg.GetHashCode(), ve);
//					return;
//				}
//
//				
//				SmallCar cm = tVar as SmallCar;
//				if (cm != null)
//				{
//					cm.EntityType = EntityType.SmallCar;
//					//	cm.Grid = new Point(0,0);
//
//					ISimCtx.CarModels.Add(cm.GetHashCode(), cm);
//					return;
//				}
//
//				ThrowHelper.ThrowArgumentException("无法识别的类型，没有注册");
//
//				#endregion
//			}
//			
//		}
//
//		protected override void SubRevoke(ITrafficEntity tVar)
//		{
//			if (RegisterService.IsServiceUp ==true)
//			{
//				#region
//
//				ISimContext isc = SimContext.GetInstance();
//
//				Way re = tVar as Way;//反注册
//				if (re != null)
//				{
//					///两个端点已经注册的情况下才允许反注册
//					XNode from = isc.RoadNet.FindXNode(re.XNodeFrom);
//					XNode to = isc.RoadNet.FindXNode(re.XNodeTo);
//					if ( from!=null&& to != null)
//					{
//						foreach (var lane in re.Lanes)//反注册掉内部所有lanes
//						{
//							lane.UnRegiser();
//						}//然后反注册掉自己
//						isc.RoadNet.Ways.Remove(re.GetHashCode());
//					}
//					return;
//				}
//				XNode rn = tVar as XNode;//内部使用邻接矩阵反注册
//				if (rn != null)
//				{
//					isc.RoadNet.XNodes.Remove(rn.GetHashCode()); return;
//				}
//				
//				Lane rl = tVar as Lane;
//				if (rl != null)
//				{
//					isc.RoadNet._Lanes.Remove(rl.GetHashCode());
//					return;
//				}
//
//				SmallCar cm = tVar as SmallCar;
//				if (cm != null)
//				{
//					isc.CarModels.Remove(cm.GetHashCode());
//					return;
//				}
//				SignalLight sg = tVar as SignalLight;
//				if (sg != null)
//				{
//					isc.SignalLights.Remove(sg.GetHashCode());
//					return;
//				}
//				VMSEntity ve = tVar as VMSEntity;
//				if (ve != null)
//				{
//					isc.VMSEntities.Remove(sg.GetHashCode());
//					return;
//				}
//				throw new System.Exception("无法识别的类型，没有注册");
//				#endregion
//			}
	
	
	

}
