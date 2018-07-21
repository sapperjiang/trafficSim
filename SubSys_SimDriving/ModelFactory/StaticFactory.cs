using System;
using System.Collections.Generic;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving;
using SubSys_SimDriving.Agents;
using SubSys_SimDriving;
using SubSys_MathUtility;

namespace SubSys_SimDriving
{
	public partial class StaticFactory:IStaticFactory
	{
		/// <summary>
		/// create xnode way road and lane.
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <param name="et"></param>
		/// <returns></returns>
		public StaticOBJ Build(OxyzPointF start,OxyzPointF end, EntityType et)
		{
			var net  =  RoadNet.GetInstance();
			switch (et)
			{

				case EntityType.Road:
                    var road = new Road();

					road.Way=this.Build(start,end,EntityType.Way) as Way;
					road.CtrWay=this.Build(end,start,EntityType.Way) as Way;
					road.Container= net;

					//register
					net.roads.Add(road.GetHashCode(),road);//register
					return road;
					///avoid to create a xnode
				case EntityType.XNode:
					var node = new XNode(start);

					node.Container = net;
					net.xnodes.Add(node.GetHashCode(),node);//register
                    return node;

				case EntityType.Way:
					var way =  new Way(start,end);
					//way.AddLane(LaneType.Straight);
					//way.Container = net;
					net.ways.Add(way.GetHashCode(),way);//register

                    return way;
                    //break;
				case EntityType.Lane:

					throw new Exception("lane should be builded by way");
					break;

				default:
					break;
			}
			throw new  ArgumentException("无法创建参数指定的构造型");
		}
	}


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
		
		public TrafficOBJ Build(BuildCommand bc, EntityType et)
		{
			throw new NotImplementedException();
		}
	}
	
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
