using System;
using System.Collections.Generic;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving;
using SubSys_SimDriving.Agents;
using System.Drawing;

namespace SubSys_SimDriving
{
    public partial class StaticFactory:IFactory
    {
        public AbstractAgent Build(BuildCommand bc, AgentType et)
        {
            throw new NotImplementedException();
        }

        [System.Obsolete("注意修改函数")]
        public TrafficEntity Build(BuildCommand bc, EntityType et)
        {
            switch (et)
            {
                case EntityType.RoadNet:
                    return RoadNet.GetInstance();
                    //break;
                case EntityType.SignalLight:
                    return new SignalLight();
                    //break;
                case EntityType.VMSEntity:
                    return new VMSEntity();
                    //break;
                case EntityType.Road:
                    return new Road();
                
                case EntityType.XNode:
                    XNodeBuildCmd rn = bc as XNodeBuildCmd;
                    return new XNode(rn.rltPos);

                case EntityType.Way:
                    //return new RoadEdge();
                    throw new NotImplementedException("无法创建参数指定的构造型");

                case EntityType.Lane:
                    LaneBuildCmd rlbc= bc as LaneBuildCmd;
                    if (rlbc == null)
                    {
                        return new Lane(LaneType.StraightRight);
                    }
                    else
                    {
                        return new Lane(rlbc.laneType);
                    }
                default:
                    break;
            }
            throw new  ArgumentException("无法创建参数指定的构造型");
        }
    }

    public class WayFactory//: AbstractModelFactory
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
        public static Way BuildOneWay(Point start,Point end,int iLeftCount, int iStraightCount, int iRightCount)
        {
        	Way  re=new Way(start,end);// eModelFactory.b

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
        
//         public static Way BuildOneWay(int iLeftCount, int iStraightCount, int iRightCount)
//        {
//
//        	Way  re=new Way(start,end);// eModelFactory.b
//
//            for (int i = 0; i < iLeftCount; i++)//左。
//            {
//                re.AddLane(LaneType.Left);
//            }
//            for (int i = 0; i < iStraightCount; i++)//直行
//            {
//                re.AddLane(LaneType.Straight);
//            }
//            for (int i = 0; i < iRightCount; i++)//右转
//            {
//                re.AddLane(LaneType.Right);
//            }
//            return re;
//
//        }
        
        
        /// <summary>
        /// 创建对称的两个RoadEdge，里面的车道数量由参数指定
        /// </summary>
        /// <param name="iLeftCount"></param>
        /// <param name="iStraightCount"></param>
        /// <param name="iRightCount"></param>
        /// <returns></returns>
        public static void BuildTwoWay(Way re,int iLeftCount,int iStraightCount,int iRightCount)
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
        public WayBuildCommand RoadEdgeCmd;
        public WayBuildCommand CtrRoadEdgeCmd;
        public RoadBuildCommand(WayBuildCommand re,WayBuildCommand ctrRe)
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

    public class LaneBuildCmd : BuildCommand
    {
        public LaneType laneType;
        public LaneBuildCmd(LaneType ltLaneType)
        {
            this.laneType = ltLaneType;
        }
        public LaneBuildCmd()
        {
            this.laneType = LaneType.Straight;
        }
    }
    public class XNodeBuildCmd : BuildCommand
    {
        public Point rltPos;
        public XNodeBuildCmd(Point p)
        {
            this.rltPos = p;
        }
     
    }
    public class WayBuildCommand : BuildCommand
    {
        public List<LaneBuildCmd> CmdList = new List<LaneBuildCmd>();

        public XNode from;
        public XNode to;

        public WayBuildCommand() { }

        public WayBuildCommand(XNode from, XNode to)
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
       public static WayBuildCommand BuildStanderdRoadEdge()
       {
            WayBuildCommand reb = new WayBuildCommand();
            reb.CmdList.Add(new LaneBuildCmd(LaneType.Left));
            reb.CmdList.Add(new LaneBuildCmd(LaneType.StraightRight));
            reb.CmdList.Add(new LaneBuildCmd(LaneType.Right));
            return reb;
       }
    }
    
    

}
