using System.Drawing;
using SubSys_SimDriving.TrafficModel;
using SubSys_MathUtility;

namespace SubSys_SimDriving
{
    /// <summary>
    /// 保存当前位置，起始位置，要去的位置的坐标信息，不同车辆（大车、中型车、小车）类型的Track可能不一样，其性质也不一样。
    /// </summary>
    public  partial class Track
    {
        internal Lane fromLane;
        internal Lane toLane;
        internal Point pTempPos;

        public Point pCurrPos;

        public Point pFromPos;
        public Point pToPos;
       
        internal Point NextPoint(Point iCurrPoint)
        {
            Point iNew = iCurrPoint;
            //算法保证每一个时间步长内都向目标终点接近，就是为了让其到终点的距离变小
            int iX = iCurrPoint.X - this.pToPos.X;//当前位置减去目的位置
            int iY = iCurrPoint.Y - this.pToPos.Y;
            if (iX != 0)//等于0的情况什么也不做
            {
                iNew.X = iX > 0 ? --iNew.X : ++iNew.X;
            }
            if (iY != 0)//等于0的情况什么也不做
            {
                iNew.Y = iY > 0 ? --iNew.Y : ++iNew.Y;
            }
            if (iX==0&&iY==0)///已经到达了目标地点，两个点的坐标差值为0
            {
                iNew = new Point(0, 0);
            }
            return iNew;
        }
        
    }
    
    //-------------------2015年1月11日-------------------------
    public  partial class Track
    {
    	public OxyzPoint opCurrPos;
    	public OxyzPoint opNextPos;
    	
    }

}
 
