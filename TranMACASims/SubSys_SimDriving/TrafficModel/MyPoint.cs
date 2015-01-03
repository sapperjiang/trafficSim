using System.Drawing;
using SubSys_SimDriving.TrafficModel;
namespace SubSys_SimDriving
{


    /// <summary>
    /// 保存当前位置，起始位置，要去的位置的坐标信息
    /// </summary>
    public class CarTrack
    {
        internal RoadLane fromLane;
        internal RoadLane toLane;
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
    //public class Point
    //{
    //    public int X;
    //    public int Y;
    //    public Point(int x, int y)
    //    {
    //        this.X = x;
    //        this.Y = y;
    //    }
    //    public Point Copy()
    //    {
    //        return new Point(this.X,this.Y);
    //    }
    //    public static Point MinValue()
    //    {
    //        return new Point(0,0);
    //    }
    //}
	 
}
 
