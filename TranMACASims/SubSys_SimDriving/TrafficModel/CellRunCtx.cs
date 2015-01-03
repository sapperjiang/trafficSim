using System;
using System.Collections.Generic;
using System.Text;
using SubSys_SimDriving.TrafficModel;
using System.Drawing;

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// 元胞运行的上下文信息，如前后车头时距，左右车头时距，
    /// 车道编号、左右车道，运行的地点（路段或者交叉口）
    /// </summary>
    internal class CellRunCtx
    {
        //运行的交通实体
        RoadEntity Container;

        //空间参数
        public int iFrontGap;

        public int iEntityGap;
        public int iToEntityGap;

        public int iRearGap;
        public int iLeftFrontGap;
        public int iLeftRearGap;
        public int iRightFrontGap;
        public int iRightRearGap;

        public CellOutParam Out;
        //
        //速度参数
        public int iSpeedLimit;//路段限速
        //public int iSpeed;//当前车速
        public int iFrontSpeed;
        public int iRearSpeed;
        public int iLeftFrontSpeed;
        public int iLeftRearSpeed;
        public int iRightFrontSpeed;
        public int iRightRearSpeed;
        //加速度参数
        public int iAcceleration;

        /// <summary>
        /// 安全车头时距
        /// </summary>
        public int iSafetyGap = SysSimContext.SimSettings.iSafeHeadWay;

        public double dModerationRatio = TrafficModel.ModelSetting.dRate;//随机漫化概率Probability
        public double dRandom
        {
            get
            {
                Random rd = new Random();
                return rd.NextDouble();
            }
        }

        public CarTrack CarTrack;

        /// <summary>
        /// 仅仅初始化不做赋值
        /// </summary>
        /// <param name="roadE"></param>
        /// <param name="ce"></param>
        public CellRunCtx(RoadEntity roadE,Cell ce)
        {
            Out = new CellOutParam();
            this.Container = roadE;
        }
    }

    internal class CellOutParam 
    {
        /// <summary>
        /// Y坐标表示前进的距离，x坐标表示前进的车道
        /// </summary>
        public int iMoveStepY;
        public int iMoveStepX;//运行的车道
        public int iSpeed;
        public int iAcceleration;
    }
}
