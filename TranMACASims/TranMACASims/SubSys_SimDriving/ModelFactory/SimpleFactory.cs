using System;
using System.Collections.Generic;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.SysSimContext;
using System.Text;

namespace SubSys_SimDriving.ModelFactory
{
    ///// <summary>
    ///// 简单工厂模式，可以使用命令模式建造更复杂的功能集合类型
    ///// </summary>
    //internal abstract class AbstractModelFactory
    //{
    //    //internal static RoadEdge Build(AbstractModelFactory modelFactory)
    //    //{
    //    //    return modelFactory.BuildModel();
    //    //}

    //    internal abstract RoadEdge BuildModel();
    //}

    internal class RoadNodeFactory
    {
 
    }

    internal class RoadEdgeFacory //: AbstractModelFactory
    {
        internal static RoadEdge BuildOneWay(int iLeftCount, int iStraightCount, int iRightCount)
        {
            if (iLeftCount + iStraightCount + iRightCount >= SimContext.SimSettings.iMaxWidth)
            {
                throw new ArgumentOutOfRangeException("无法创建超过" + SimContext.SimSettings.iMaxWidth.ToString() + "个车道！");
            }

            RoadNode rnStart = new RoadNode();// new IRoad();
            RoadNode rnEnd = new RoadNode();
            RoadEdge re = new RoadEdge(rnStart, rnEnd);

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
        internal static Road BuildTwoWay(int iLeftCount,int iStraightCount,int iRightCount)
        {
            if(iLeftCount+iStraightCount+iRightCount >=SimContext.SimSettings.iMaxWidth)
            {
                throw new ArgumentOutOfRangeException("无法创建超过"+SimContext.SimSettings.iMaxWidth.ToString()+"个车道！");
            }

            Road road = new Road();
            road.roadNode = new RoadNode();
            road.ctrRoadNode = new RoadNode();

            road.roadEdge = new RoadEdge(road.roadNode,road.ctrRoadNode);
            road.ctrRoadEdge = new RoadEdge(road.ctrRoadNode, road.roadNode);//反向边

            for (int i = 0; i < iLeftCount; i++)//左。
			{
			    road.roadEdge.AddLane(LaneType.Left);
                road.ctrRoadEdge.AddLane(LaneType.Left);
			}
            for (int i = 0; i < iStraightCount; i++)//直行
			{
			    road.roadEdge.AddLane(LaneType.Straight);
                road.ctrRoadEdge.AddLane(LaneType.Straight);
			}
            for (int i = 0; i < iRightCount; i++)//右转
			{
			    road.roadEdge.AddLane(LaneType.Right);
                road.ctrRoadEdge.AddLane(LaneType.Right);
            }
            return road;
        }
    }
}
