namespace SubSys_SimDriving.TrafficModel
{
    public enum EntityType
    {
        RoadNet = 0,

        Road = 2,//道路

        Way = 4,//路段

        Lane = 6,//车道

        XNode = 8,//交叉口

        SignalLight = 10,//信号灯

        SignalGroup = 12,//两个或者多个信号灯组成信号灯组

        VMSEntity = 14,//信息板
        
        Cell = 18,

        /// <summary>
        /// 所有类型的车辆、行人的抽象表示
        /// </summary>
        Mobile = 20,

        SmallCar = 21,

        MediumCar = 22,

        Bus = 23,///largePassangerCar

        SmallTruck = 24,

        MediumTruck = 28,

        LargeTruck = 30,

        Pedastrain = 32//行人
 
    }

    //public static class EntityType
    //{
    //    //静态实体
    //    public static int RoadNetWork = 0;

    //    public static int Road = 2;//道路

    //    public static int RoadEdge = 4;//路段

    //    public static int RoadLane = 6;//车道

    //    public static int RoadNode = 8;//交叉口

    //    public static int SignalLight = 10;//信号灯

    //    public static int SignalGroup = 12;//两个或者多个信号灯组成信号灯组

    //    public static int VMSEntity = 14;//信息板

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

    //        public static int Pedastrain = 32;//行人
    //    }
    //} 

}
 
