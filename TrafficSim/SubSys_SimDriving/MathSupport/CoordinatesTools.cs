 
namespace SubSys_SimDriving.MathSupport
{
    internal class Coordinates
    {
        /// <summary>
        /// ����������ϵ�ĵ㣬���ؾ�����ϵ�ĵ�
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
        /// ����ϵƽ��
        /// </summary>
        /// <param name="old"></param>
        /// <param name="panVector">ƽ������</param>
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
        /// ��·�ζ˵�����ϵת��Ϊ�������������ϵ
        /// </summary>
        /// <param name="old"></param>
        internal static void ToCenterXY(ref Index old)
        {
            Coordinates.Translate(ref old,new Index(0, -SysSimContext.SimContext.SimSettings.iMaxWidth));
        }

       
    }
}
 
