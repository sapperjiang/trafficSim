using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubSys_Graphics
{
    public class GUISettings
    {
        public static int iGUI_CellPixels = CellPixels.iMedium;
        //internal static int iGUI_CellShapeMargin = 1;
//        internal static int iG_BentchWidth = 1024;
        internal static int iG_BentchWidth = 1440;
        internal static int iG_BentchHeight = 900;
        
//        internal static int iG_BentchHeight = 768;
        /// <summary>
        /// 这个代表像素宽度例如，一个元胞对应多少像素
        /// </summary>
        public struct CellPixels
        {
            internal static int iMin = 4;//4像素
            internal static int iMedium = 8;//8像素
            internal static int iMax = 12;//12像素级放大
        }
    }
}
