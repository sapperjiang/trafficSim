namespace SubSys_SimDriving.TrafficModel
{
	/**
	 * 车道类型枚举,按照右行规则数值由小到大
	 */
	public enum LaneType
	{
		Left = 0,//左
        
        LeftStraight = 1,//直左
		
        Straight = 2,//直行
		 
		StraightRight = 3,//直右
        
		Right = 4,//右转
 
		Extend = 5,//展宽车道，用于在车道展宽处增加车道
		 
	}
	 
}
 
