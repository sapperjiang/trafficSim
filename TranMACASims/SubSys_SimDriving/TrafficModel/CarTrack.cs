using System.Drawing;
using SubSys_SimDriving.TrafficModel;
using SubSys_MathUtility;

namespace SubSys_SimDriving
{
    /// <summary>
    /// ���浱ǰλ�ã���ʼλ�ã�Ҫȥ��λ�õ�������Ϣ����ͬ�������󳵡����ͳ���С�������͵�Track���ܲ�һ����������Ҳ��һ����
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
    
    //-------------------2015��1��11��-------------------------
    public  partial class Track
    {
    	public OxyzPoint opCurrPos;
    	public OxyzPoint opNextPos;
    	
    }

}
 
