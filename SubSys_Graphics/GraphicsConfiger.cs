using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using SubSys_MathUtility;

namespace SubSys_Graphics
{
	public static class GraphicsSetter
	{
		/// <summary>
		/// define a point means graphics pixels
		/// </summary>
		public static int iPixels = Pixels.iMin;
		//internal static int iGUI_CellShapeMargin = 1;
		//        internal static int iG_BentchWidth = 1024;
		internal static int iG_BentchWidth = 1440;
		internal static int iG_BentchHeight = 900;
		
		internal static  Color roadColor = Color.DarkGray;
        public static OxyzPointF baseOffset = new OxyzPointF(-50f, -50f, 0f);

        //        internal static int iG_BentchHeight = 768;
        /// <summary>
        /// 这个代表像素宽度例如，一个元胞对应多少像素
        /// </summary> 
        public struct Pixels
		{
			//        	   public static int
			public static int iMin = 8;//4像素
			public static int iMedium = 10;//8像素
			public static int iMax = 12;//12像素级放大
		}
		public static void ScaleByPixels(int i)
		{
			iPixels +=i;//增加像素
			if (iPixels<Pixels.iMin) {
				iPixels -=i;//超出像素最小值就恢复原值
//				throw new Exception();
			}
		}
        /// <summary>
        /// 一个像素单元代表的网格对应的物理长度的，例如，6m
        /// </summary>
        public static int iCellSize =6;
	}
}
