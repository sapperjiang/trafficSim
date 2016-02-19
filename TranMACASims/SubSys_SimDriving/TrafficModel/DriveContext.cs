using System;
using System.Collections.Generic;
using System.Text;
using SubSys_SimDriving.TrafficModel;
using System.Drawing;

namespace SubSys_SimDriving.TrafficModel
{

	internal class MobileParam
	{
		/// <summary>
		/// Y,distances to move forword ,Y lane to move to 
		/// Y坐标表示前进的距离，x坐标表示前进的车道
		/// </summary>
		public int iMoveY;
		/// <summary>
		/// index of a destination lane to move on
		/// Y,distances to move forword ,Y lane to move to 
		/// Y坐标表示前进的距离，x坐标表示前进的车道
		/// </summary>
		public int iMoveX;
		/// <summary>
		/// mobile's next  speed 
		/// </summary>
		public int iSpeed;
		
		/// <summary>
		/// mobile's next acceleration
		/// </summary>
		public int iAcceleration;
		
	}
	
	/// <summary>
	/// Envirnment of a mobile ,如前后车头时距，左右车头时距，
	/// 车道编号、左右车道，运行的地点（路段或者交叉口）
	/// </summary>
	internal partial class DriveCtx
	{
		//运行的交通实体
		TrafficEntity Container;

		//空间参数
		public int iFrontHeadWay;
		public int iRearHeadWay;
		
		//
		public int iLaneGap;
		public int iXNodeGap;

		public int iLeftFrontHeadWay;
		public int iLeftRearHeadWay;
		public int iRightFrontHeadWay;
		public int iRightRearHeadWay;


		/// <summary>
		/// to carry mobile driving behavior message
		/// </summary>
		public MobileParam Params;
		//
		//速度参数
		public int iSpeedLimit=0;//路段限速
		
		public int iSpeed;//当前车速
		
		public int iFrontSpeed=0;
		public int iRearSpeed=0;
		public int iLeftFrontSpeed=0;
		public int iLeftRearSpeed=0;
		public int iRightFrontSpeed=0;
		public int iRightRearSpeed=0;
		//加速度参数
		public int iAcceleration;

		/// <summary>
		/// 安全车头时距
		/// </summary>
		public int iSafeHeadWay = SimSettings.iSafeHeadWay;

		public double dModerationRatio = TrafficModel.ModelSetting.dRate;//随机漫化概率Probability
		public double dRandom
		{
			get
			{
				Random rd = new Random();
				return rd.NextDouble();
			}
		}

		public Track MobileTrack;

		/// <summary>
		/// 仅仅初始化不做赋值
		/// </summary>
		/// <param name="roadE"></param>
		/// <param name="ce"></param>
//		[System.Obsolete("abandoned")]
//		public DriveContext(TrafficEntity te,Cell ce)
//		{
//			DriveParam = new MobileParam();
//			this.Container = te;
//		}
		
	}
	
	//2016/1/21
	internal partial class DriveCtx
	{
		public bool IsReachEnd =false;
		
		public DriveCtx(StaticEntity te)
		{
			Params = new MobileParam();
			this.Container = te;
		}
		
//		
//		public DrivingContext GetDriveCtx(TrafficEntity way)
//		{
//			return null;
//		}
	}
	
}
