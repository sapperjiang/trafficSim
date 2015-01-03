using System;
using System.Collections.Generic;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.Agents;
using System.Drawing;

namespace SubSys_SimDriving.ModelFactory
{
    public class TrafficEntityFactory:IAbstractFactory
    {
        public Agent BuildAgent(BuildCommand bc, AgentType et)
        {
            throw new NotImplementedException();
        }

        [System.Obsolete("注意修改函数")]
        public TrafficEntity BuildEntity(BuildCommand bc, EntityType et)
        {
            switch (et)
            {
                case EntityType.RoadNetWork:
                    return RoadNetWork.GetInstance();
                    //break;
                case EntityType.SignalLight:
                    return new SignalLight();
                    //break;
                case EntityType.VMSEntity:
                    return new VMSEntity();
                    //break;
                case EntityType.Road:
                    return new Road();
                
                case EntityType.RoadNode:
                    RoadNodeBuildCommand rn = bc as RoadNodeBuildCommand;
                    return new RoadNode(rn.rltPos);

                case EntityType.RoadEdge:
                    //return new RoadEdge();
                    throw new NotImplementedException("无法创建参数指定的构造型");

                //RoadEdgeBuildCommand rbc = bc as RoadEdgeBuildCommand; 
                    //for (int i = 0; i < rbc.iLeftCount; i++)//左。
                    //    {
                    //        rd.RoadEdge.AddLane(LaneType.Left);
                    //    }
                    //    for (int i = 0; i < rbc.iStraightCount; i++)//直行
                    //    {
                    //        rd.RoadEdge.AddLane(LaneType.Straight);
                    //    }
                    //    for (int i = 0; i < rbc.iRightCount; i++)//右转
                    //    {
                    //        rd.RoadEdge.AddLane(LaneType.Right);
                    //    }
                    //break;

                case EntityType.RoadLane:
                    RoadLaneBuildCommand rlbc= bc as RoadLaneBuildCommand;
                    if (rlbc == null)
                    {
                        return new RoadLane(LaneType.StraightRight);
                    }
                    else
                    {
                        return new RoadLane(rlbc.laneType);
                    }
                default:
                    break;
            }
            throw new  ArgumentException("无法创建参数指定的构造型");
        }
    }

    public class RoadEdgeFacory //: AbstractModelFactory
    {
 
 /// <summary>
 /// 创建单方向道路
 /// </summary>
 /// <param name="roadNodeFrom"></param>
 /// <param name="roadNodeTo"></param>
 /// <param name="iLeftCount">左转车道数目</param>
 /// <param name="iStraightCount">直行车道数</param>
 /// <param name="iRightCount">右转车道数</param>
 /// <returns></returns>
        public static RoadEdge BuildOneWay(Point start,Point end,int iLeftCount, int iStraightCount, int iRightCount)
        {

        	RoadEdge  re=new RoadEdge(start,end);// eModelFactory.b

            for (int i = 0; i < iLeftCount; i++)//左。
            {
                re.AddLane(LaneType.Left);
            }
            for (int i = 0; i < iStraightCount; i++)//直行
            {
                re.AddLane(LaneType.Straight);
            }
            for (int i = 0; i < iRightCount; i++)//右转
            {
                re.AddLane(LaneType.Right);
            }
            return re;

        }
        /// <summary>
        /// 创建对称的两个RoadEdge，里面的车道数量由参数指定
        /// </summary>
        /// <param name="iLeftCount"></param>
        /// <param name="iStraightCount"></param>
        /// <param name="iRightCount"></param>
        /// <returns></returns>
        public static void BuildTwoWay(RoadEdge re,int iLeftCount,int iStraightCount,int iRightCount)
        {
            if(iLeftCount+iStraightCount+iRightCount >=SimSettings.iMaxLanes)
            {
                throw new ArgumentOutOfRangeException("无法创建超过"+SimSettings.iMaxLanes.ToString()+"个车道！");
            }
         
            for (int i = 0; i < iLeftCount; i++)//左。
			{
                re.AddLane(LaneType.Left);
                //re.GetReverse().AddLane(LaneType.Left);
			}
            for (int i = 0; i < iStraightCount; i++)//直行
			{
                re.AddLane(LaneType.Straight);
                //re.GetReverse().AddLane(LaneType.Straight);
			}
            for (int i = 0; i < iRightCount; i++)//右转
			{
			    re.AddLane(LaneType.Right);
                //re.GetReverse().AddLane(LaneType.Right);
            }

        }
    }

    /// <summary>
    /// 抽象创建命令的类
    /// </summary>
    public abstract class BuildCommand
    {
    }
    public class AgentBuildCommand : BuildCommand
    {
    }

    public class RoadBuildCommand:BuildCommand
    {
        public RoadEdgeBuildCommand RoadEdgeCmd;
        public RoadEdgeBuildCommand CtrRoadEdgeCmd;
        public RoadBuildCommand(RoadEdgeBuildCommand re,RoadEdgeBuildCommand ctrRe)
        {
            this.RoadEdgeCmd = re;
            this.CtrRoadEdgeCmd = ctrRe;
        }
        public RoadBuildCommand()
        {
            //this.RoadEdgeCmd = re;
            //this.CtrRoadEdgeCmd = ctrRe;
        }
    }

    public class RoadLaneBuildCommand : BuildCommand
    {
        public LaneType laneType;
        public RoadLaneBuildCommand(LaneType ltLaneType)
        {
            this.laneType = ltLaneType;
        }
        public RoadLaneBuildCommand()
        {
            this.laneType = LaneType.Straight;
        }
    }
    public class RoadNodeBuildCommand : BuildCommand
    {
        public Point rltPos;
        public RoadNodeBuildCommand(Point p)
        {
            this.rltPos = p;
        }
     
    }
    public class RoadEdgeBuildCommand : BuildCommand
    {
        public List<RoadLaneBuildCommand> CmdList = new List<RoadLaneBuildCommand>();

        public RoadNode from;
        public RoadNode to;

        public RoadEdgeBuildCommand() { }

        public RoadEdgeBuildCommand(RoadNode from, RoadNode to)
        {
            if (from == null || to == null)
            {
                throw new ArgumentNullException();
            }
            this.from = from;
            this.to = to;
        }

    }

    public class CommandBuilder 
    {
       public static RoadEdgeBuildCommand BuildStanderdRoadEdge()
       {
            RoadEdgeBuildCommand reb = new RoadEdgeBuildCommand();
            reb.CmdList.Add(new RoadLaneBuildCommand(LaneType.Left));
            reb.CmdList.Add(new RoadLaneBuildCommand(LaneType.StraightRight));
            reb.CmdList.Add(new RoadLaneBuildCommand(LaneType.Right));
            return reb;
       }
    }

}
