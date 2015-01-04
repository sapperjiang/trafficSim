using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;

namespace SubSys_Graphics
{
	public static class GraphicsConfiger
	{
		public static int iCellPixels = CellPixels.iMedium;
		//internal static int iGUI_CellShapeMargin = 1;
		//        internal static int iG_BentchWidth = 1024;
		internal static int iG_BentchWidth = 1440;
		internal static int iG_BentchHeight = 900;
		
		internal static  Color roadColor = Color.DarkGray;
		
		//        internal static int iG_BentchHeight = 768;
		/// <summary>
		/// 这个代表像素宽度例如，一个元胞对应多少像素
		/// </summary>
		public struct CellPixels
		{
			//        	   public static int
			public static int iMin = 2;//4像素
			public static int iMedium = 4;//8像素
			public static int iMax = 8;//12像素级放大
		}
		public static void ScaleCellPixels(int i)
		{
			iCellPixels +=i;//增加像素
			if (iCellPixels<CellPixels.iMin) {
				iCellPixels -=i;//超出像素最小值就恢复原值
//				throw new Exception();
			}
		}
		//        	public static int ScaleDown()
	}
}
