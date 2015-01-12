using System;
using System.Collections.Generic;
using System.Text;
using SubSys_SimDriving.TrafficModel;
using System.Drawing;

namespace SubSys_SimDriving.TrafficModel
{  

    internal class RunCtxParam 
    {
        /// <summary>
        /// Y坐标表示前进的距离，x坐标表示前进的车道
        /// </summary>
        public int iMoveStepY;
        public int iMoveStepX;//运行的车道
        public int iSpeed;
        public int iAcceleration;
        
  
    }
    
    /// <summary>
    /// 元胞运行的上下文信息，如前后车头时距，左右车头时距，
    /// 车道编号、左右车道，运行的地点（路段或者交叉口）
    /// </summary>
    internal partial class RunCtx
    {
        //运行的交通实体
        TrafficEntity Container;

        //空间参数
        public int iFrontGap;

        public int iEntityGap;
        public int iToEntityGap;

        public int iRearGap;
        public int iLeftFrontGap;
        public int iLeftRearGap;
        public int iRightFrontGap;
        public int iRightRearGap;

        public void GetRunCtx(TrafficEntity way)
        {
        	
        }
        public RunCtxParam Out;
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

        public Track CarTrack;

        /// <summary>
        /// 仅仅初始化不做赋值
        /// </summary>
        /// <param name="roadE"></param>
        /// <param name="ce"></param>
        public RunCtx(TrafficEntity te,Cell ce)
        {
            Out = new RunCtxParam();
            this.Container = te;
        }
        
    }
    
    
    internal partial class RunCtx
    {
    	   
             public RunCtx(StaticEntity te,Cell ce)
        {
            Out = new RunCtxParam();
            this.Container = te;
        }
    }
    
}
