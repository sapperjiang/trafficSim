using System.Drawing;
using SubSys_SimDriving.TrafficModel;
namespace SubSys_SimDriving
{


    /// <summary>
    /// ���浱ǰλ�ã���ʼλ�ã�Ҫȥ��λ�õ�������Ϣ
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
            //�㷨��֤ÿһ��ʱ�䲽���ڶ���Ŀ���յ�ӽ�������Ϊ�����䵽�յ�ľ����С
            int iX = iCurrPoint.X - this.pToPos.X;//��ǰλ�ü�ȥĿ��λ��
            int iY = iCurrPoint.Y - this.pToPos.Y;
            if (iX != 0)//����0�����ʲôҲ����
            {
                iNew.X = iX > 0 ? --iNew.X : ++iNew.X;
            }
            if (iY != 0)//����0�����ʲôҲ����
            {
                iNew.Y = iY > 0 ? --iNew.Y : ++iNew.Y;
            }
            if (iX==0&&iY==0)///�Ѿ�������Ŀ��ص㣬������������ֵΪ0
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
 
