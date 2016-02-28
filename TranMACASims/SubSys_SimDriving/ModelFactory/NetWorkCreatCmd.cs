using System;
using System.Collections.Generic;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving;
using SubSys_SimDriving.Agents;
using SubSys_SimDriving;
using SubSys_MathUtility;

namespace SubSys_SimDriving

{	/// <summary>
	/// 抽象创建命令的类
	/// </summary>
	public abstract class BuildCommand
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