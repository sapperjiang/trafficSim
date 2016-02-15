using System.Threading;
using SubSys_SimDriving.TrafficModel;
using System.Collections.Generic;
using SubSys_SimDriving;
using SubSys_SimDriving;

namespace SubSys_SimDriving
{
    public class SimSettings
    {
        /// <summary>
        /// ����ڵ����Ԫ����������Roadedge�ڲ��������ɵ���󳵵�����
        /// </summary>
        public static int iMaxLanes = 6;
 
        public static int iMaxNodeWidth = 12;//��������Ŀ��
        /// <summary>
        /// Ԫ�������Ŀ����2�������GIS�������ת����ʱ��ʹ��
        /// </summary>
        public static int iCellWidth = 2;
        /// <summary>
        /// ��ȫ��ͷʱ��
        /// </summary>
        public static int iSafeHeadWay = 1;
        /// <summary>
        /// չ����εĳ���Ϊ10��Ԫ����60��
        /// </summary>
        public static int iExtendLength = (int)(60/iCellWidth);

        /// <summary>
        /// ��ʾ��׼С������ȵ�Ԫ������
        /// </summary>
        public static int iCarWidth = 1;

        /// <summary>
        /// ��ʾ��׼С�������ȵ�Ԫ������
        /// </summary>
        public static int iCarLength = 1;
    }

}
 
