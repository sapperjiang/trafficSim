namespace SubSys_SimDriving.TrafficModel
{
    public enum EntityType
    {
        RoadNet = 0,

        Road = 2,//��·

        Way = 4,//·��

        Lane = 6,//����

        XNode = 8,//�����

        SignalLight = 10,//�źŵ�

        SignalGroup = 12,//�������߶���źŵ�����źŵ���

        VMSEntity = 14,//��Ϣ��
        
        Cell = 18,

        /// <summary>
        /// �������͵ĳ��������˵ĳ����ʾ
        /// </summary>
        Mobile = 20,

        SmallCar = 21,

        MediumCar = 22,

        Bus = 23,///largePassangerCar

        SmallTruck = 24,

        MediumTruck = 28,

        LargeTruck = 30,

        Pedastrain = 32//����
 
    }

    //public static class EntityType
    //{
    //    //��̬ʵ��
    //    public static int RoadNetWork = 0;

    //    public static int Road = 2;//��·

    //    public static int RoadEdge = 4;//·��

    //    public static int RoadLane = 6;//����

    //    public static int RoadNode = 8;//�����

    //    public static int SignalLight = 10;//�źŵ�

    //    public static int SignalGroup = 12;//�������߶���źŵ�����źŵ���

    //    public static int VMSEntity = 14;//��Ϣ��

    //    public static class AbstractType
    //    {
    //        public static int Cell = 18;
    //        public static int CarModel = 20;
    //    }
    //    public static class MobileType
    //    {
    //        public static int SmallCar = 20;

    //        public static int MediumCar = 22;

    //        public static int Bus = 22;///largePassangerCar

    //        public static int SmallTruck = 24;

    //        public static int MediumTruck = 28;

    //        public static int LargeTruck = 30;

    //        public static int Pedastrain = 32;//����
    //    }
    //} 

}
 
