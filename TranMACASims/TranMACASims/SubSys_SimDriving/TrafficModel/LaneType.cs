namespace SubSys_SimDriving.TrafficModel
{
	/**
	 * 车道类型枚举,按照右行规则数值由小到大，所有车道最多三条
	 */
	internal enum LaneType
	{
		Left = 0,//左.
        
        LeftStraight = 3,//直左
		
        Straight = 6,//直行
		 
		StraightRight = 9,//直右
        
		Right = 12,//右转
 
		Extend = 15,//展宽车道，用于在车道展宽处增加车道
		 
	}
	 
}
 
