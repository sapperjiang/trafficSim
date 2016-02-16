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
    /// ���浱ǰλ�ã���ʼλ�ã�Ҫȥ��λ�õ�������Ϣ
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
            //�㷨��֤ÿһ��ʱ�䲽���ڶ���Ŀ���յ�ӽ�������Ϊ�����䵽�յ�ľ����С
            int iX = iCurrPoint.X - this.iToPos.X;
            int iY = iCurrPoint.Y - this.iToPos.Y;
            if (iX != 0)//����0�����ʲôҲ����
            {
                iNew.X = iX > 0 ? --iNew.X : ++iNew.X;
            }
            if (iY != 0)//����0�����ʲôҲ����
            {
                iNew.Y = iY > 0 ? --iNew.Y : ++iNew.Y;
            }
            if (iX==0&&iY==0)///�Ѿ�������Ŀ��ص�
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
 
