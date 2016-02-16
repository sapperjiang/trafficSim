 
namespace SubSys_SimDriving.MathSupport
{
    internal class Coordinates
    {
        /// <summary>
        /// 输入新坐标系的点，返回旧坐标系的点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="a"></param>
        internal static Index Rotate(Index mpNew,SinCos a)
        {
            Index mp=new Index(0,0);
            mp.X = mpNew.X * a.iCos - mpNew.Y * a.iSin;
            mp.Y = mpNew.X * a.iSin + mpNew.Y * a.iCos;
            return mp;
        }

        /// <summary>
        /// 坐标系平移
        /// </summary>
        /// <param name="old"></param>
        /// <param name="panVector">平移向量</param>
        internal static void Translate(ref Index old,Index panVector)
        {
            old.Y += panVector.Y;
            old.X += panVector.X;
        }
        internal static void TranslateYAxis(ref Index old, int iYSpan)
        {
            Index panVector = new Index(0, -iYSpan);
            Coordinates.Translate(ref old,panVector);
        }

        /// <summary>
        /// 将路段端点坐标系转化为交叉口中央坐标系
        /// </summary>
        /// <param name="old"></param>
        internal static void ToCenterXY(ref Index old)
        {
            Coordinates.Translate(ref old,new Index(0, -SysSimContext.SimContext.SimSettings.iMaxWidth));
        }

       
    }
}
 
