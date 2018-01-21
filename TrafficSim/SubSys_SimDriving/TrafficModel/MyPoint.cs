using SubSys_SimDriving.TrafficModel;
namespace SubSys_SimDriving
{
	public class MyPoint
	{
		internal float X;
		internal float Y;
        internal MyPoint(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
	}

    /// <summary>
    /// 保存当前位置，起始位置，要去的位置的坐标信息
    /// </summary>
    internal class MovingTrack
    {
        internal RoadLane fromLane;
        internal RoadLane toLane;
        internal Index iCurrPos;
        internal Index iFromPos;
        internal Index iToPos;

        internal Index getApproachingPoint(Index iCurrPoint)
        {
            Index iNew = iCurrPoint.Copy();
            //算法保证每一个时间步长内都向目标终点接近，就是为了让其到终点的距离变小
            int iX = iCurrPoint.X - this.iToPos.X;
            int iY = iCurrPoint.Y - this.iToPos.Y;
            if (iX != 0)//等于0的情况什么也不做
            {
                iNew.X = iX > 0 ? --iNew.X : ++iNew.X;
            }
            if (iY != 0)//等于0的情况什么也不做
            {
                iNew.Y = iY > 0 ? --iNew.Y : ++iNew.Y;
            }
            if (iX==0&&iY==0)///已经到达了目标地点
            {
                iNew = new Index(0, 0);
            }
            return iNew;
        }
    }
    internal class Index
    {
        internal int X;
        internal int Y;
        internal Index(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        internal Index Copy()
        {
            return this.MemberwiseClone() as Index;
        }
    }
	 
}
 
