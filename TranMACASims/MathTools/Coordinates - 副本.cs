using System;
using System.Drawing;

namespace SubSys_MathUtility
{
    public class MyPoint
    {
        public float _X;
        public float _Y;

        public MyPoint(float x, float y)
        {
            this._X = x;
            this._Y = y;
        }

        public PointF ToPointF()
        {
            return new PointF(this._X, this._Y);
        }
    }
    /// <summary>
    /// ÄÚ²¿Â·¶ÎµÄÏà¶Ô×ø±êÏµÍ³×ª»¯¾ø¶ÔÔª°û×ø±êÏµÏµÍ³
    /// </summary>
    public static class Coordinates
    {

        /// <summary>
        /// Ôª°û×ø±êÏµºÍÍ¼Ïñ×ø±êÏµÍ³Ö®¼äµÄÆ«ÒÆÁ¿£¬
        /// ³õÊ¼Á½¸ö×ø±êÏµÍ³µÄÔ­µã¶¼ÔÚÍ¼Ïñ×ø±êÏµÔ­µã´¦,ÏòÓÒÆ½ÒÆXÎªÕý
        /// ÏòÏÂÆ½ÒÆYÎªÕý
        /// </summary>
        public static Point GUI_Offset = new Point(0, 0);
        ///// <summary>
        ///// ½«Ôª°û×ø±êÏµ×ª»»ÎªÆÁÄ»×ø±êÏµ£¬ÄÚ²¿Ê¹ÓÃÁËGUIsettigÖÐµÄGUI_CellPixels;
        ///// </summary>
        ///// <param name="rltPos"></param>
        ///// <param name="offset"></param>
        public static Point Project(Point rltPos, int iCellPixels)
        {
            return Coordinates.Project(new MyPoint(rltPos.X, rltPos.Y), iCellPixels);
        }
        public static Point Project(MyPoint mp, int iScaleFactor)
        {
            Point scrnPoint = new Point();
            scrnPoint.X = (int)Math.Round(iScaleFactor * mp._X);
            scrnPoint.Y = (int)Math.Round(iScaleFactor * mp._Y);
            //¼ÆËãÆ½ÒÆ(Æ«ÒÆ)
            scrnPoint.X -= Coordinates.GUI_Offset.X;
            scrnPoint.Y -= Coordinates.GUI_Offset.Y;
            return scrnPoint;//Õâ¸öÊÇ¸ö½á¹¹²ÎÊý¸´ÖÆ£¬È»ºó·µ»ØÐÂµÄ½á¹û
        }
        /// <summary>
        /// offsetÖÐxºÍyµÄÖµ×óÉÏ¶¼Îª¸ºÖµ
        /// </summary>
        /// <param name="scrnPoint">Ô­×ø±êÏµ</param>
        /// <param name="offset">Æ«ÒÆ×ø±ê</param>
        /// <returns></returns>
        public static Point Offset(Point scrnPoint, Point offset)
        {
            //¼ÆËãÆ½ÒÆ(Æ«ÒÆ)
            scrnPoint.X -= offset.X;
            scrnPoint.Y -= offset.Y;
            return scrnPoint;//Õâ¸öÊÇ¸ö½á¹¹²ÎÊý¸´ÖÆ£¬È»ºó·µ»ØÐÂµÄ½á¹û
        }

        public static MyPoint Offset(MyPoint scrnPoint, MyPoint offset)
        {
            MyPoint mp = new MyPoint(scrnPoint._X, scrnPoint._Y);
            //¼ÆËãÆ½ÒÆ(Æ«ÒÆ)
            mp._X -= offset._X;
            mp._Y -= offset._Y;
            return mp;
        }

        public static MyPoint Offset(Point scrnPoint, MyPoint offset)
        {
            MyPoint mp = new MyPoint(scrnPoint.X - offset._X, scrnPoint.Y - offset._Y);
            //¼ÆËãÆ½ÒÆ(Æ«ÒÆ)
            //scrnPoint.X -= offset.X;
            //scrnPoint.Y -= offset.Y;
            return mp;//Õâ¸öÊÇ¸ö½á¹¹²ÎÊý¸´ÖÆ£¬È»ºó·µ»ØÐÂµÄ½á¹û
        }



        /// <summary>
        /// ÊäÈëÐÂ×ø±êÏµµÄµã£¬·µ»Ø¾É×ø±êÏµµÄµã
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="a"></param>
        public static Point Rotate(Point mpNew,SinCos a)
        {
            Point mp=new Point(0,0);
            mp.X = mpNew.X * a.iCos - mpNew.Y * a.iSin;
            mp.Y = mpNew.X * a.iSin + mpNew.Y * a.iCos;
            return mp;
        }

        /// <summary>
        /// ×ø±êÏµÆ½ÒÆ
        /// </summary>
        /// <param name="old"></param>
        /// <param name="panVector">Æ½ÒÆÏòÁ¿</param>
        public static void Offset(ref Point old,Point panVector)
        {
            old.Y += panVector.Y;
            old.X += panVector.X;
        }
        public static void OffsetYAxis(ref Point old, int iYSpan)
        {
            Point panVector = new Point(0, -iYSpan);
            Coordinates.Offset(ref old,panVector);
        }

        /// <summary>
        /// Á½¸öµãµÄÅ·Ê½¾àÀë
        /// </summary>
        public static int Distance(Point p1, Point p2)
        {
            int iX = Math.Abs(p1.X - p2.X);
            int iY = Math.Abs(p1.Y - p2.Y);
            return (int)Math.Round(Math.Sqrt(iX * iX + iY * iY));
        }

        public static MyPoint mpBaseVector = new MyPoint(0, 1.0f);//Ê¹ÓÃxÖá×ö»ùÏòÁ¿
        /// <summary>
        /// °´ÕÕ²ÎÊý2Ö¸¶¨µÄÏòÁ¿Óë»ùÏòÁ¿µÄ¼Ð½Ç¼ÆËã²ÎÊý1µãµÄ¾ø¶ÔÔª°û×ø±ê
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Point GetRealXY(Point point, MyPoint newVector)
        {
            if (newVector == null)
            {
                throw new ArgumentException("ÊäÈëµÄ²ÎÊý²»ÄÜÎªÁã");
            }
            //»ñÈ¡ÕýÏÒºÍÓàÏÒÖµ²¢ÇÒ½øÐÐÐý×ª±ä»»
            SinCos sc = VectorTools.getSinCos(mpBaseVector, newVector);
            return Coordinates.Rotate(point, sc);
        }
    }
}
 
