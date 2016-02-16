using System;

namespace SubSys_MathUtility
{

    public sealed class SinCos
    {
        internal int iSin = 0;
        internal int iCos = 0;
        internal SinCos(int iSinine, int iCosine)
        {
            iSin = iSinine;
            iCos = iCosine;
        }

//        public override bool Equals(object obj)
//        {
//            SinCos item = (SinCos)obj;
//            if (item.iSin == this.iSin
//                && item.iCos == this.iCos)
//            {
//                return true;
//            } return false;
//        }
    } 

    /// <summary>
    /// 计算向量和向量夹角，以及计算向量位置关系的静态数据类,可能需要重载以支持重构
    /// </summary>
    public static class VectorTools
    {
        /// <summary>
        /// 获取向量的逆向量
        /// </summary>
        /// <param name="opVector"></param>
        /// <returns></returns>
        public static OxyzPointF GetInverse(OxyzPointF opVector)
        {
            return new OxyzPointF(-opVector._X, -opVector._Y);
        }
     
        static VectorTools() { }
        /// <summary>
        /// 以点mpA和点mpB为终点建立的直线方程，然后判断点mpNew在直线的上方还是下方
        /// </summary>
        /// <param name="mpA">直线起点坐标</param>
        /// <param name="mpB">直线终点坐标</param>
        /// <param name="mpNew">要检验的点坐标</param>
        /// <returns>返回0表示点mpNew位于直线上，返回1表示点mpNew位于向量上方，返回-1表示点mpNew位于直线下方</returns>
        public static int GetPointPosition(OxyzPointF mpA,OxyzPointF mpB, OxyzPointF mpNew)
        {
            if ( mpB._X==mpA._X&& mpB._Y==mpA._Y)
	        {
                throw new ArgumentException("直线的两个端点不能相同");
	        }
            double fResult =(mpNew._Y-mpA._Y)*(mpB._X-mpA._X)-(mpNew._X-mpA._X)*(mpB._Y-mpA._Y);
            if(Math.Abs(fResult)<0.9f)//绝对值在1之内，因为点坐标系有误差造成的 
            {
                return 0;//返回0 表示向量在直线上
            }
            return fResult >= 0.9f ? 1 : -1;
        }

        /// <summary>
        /// 这个是笛卡尔坐标系下的结果
        ///利用两点式，建立基础方程，并且利用这个方程计算输入点与基方程的联系,调用三参数的重载，第一个参数默认为零,返回-1表示位于基向量下方或者是右方，返回1表示位于基向量上方或者是左方
        /// </summary>
        /// <param name="vBase">基向量的终点坐标，起点坐标为0</param>
        /// <param name="vNew">要检验的点的坐标如果是向量，应当输入向量的终点坐标</param>
        /// <returns>返回-1表示位于基向量下方或者是右方，返回1表示位于基向量上方或者是左方</returns>
        public static int GetVectorPos(OxyzPointF vBase, OxyzPointF vNew)
        {
            if (vBase._X==0&&vBase._Y==0
                ||vNew._X==0&&vNew._Y==0)
            {
                throw new ArgumentException("基向量或者要判定的向量不能为0向量");
            }
            int iReturn = VectorTools.GetPointPosition(new OxyzPointF(0.0f, 0.0f), vBase, vNew);
            if (iReturn == 0&&vNew._X < vBase._X)
            {
                 iReturn = 2;
            }
            return iReturn;

        }
        /// <summary>
        /// 获取两个向量的夹角的余弦值，该值的区间是-1到1闭区间,两个参数向量都不能是0向量
        /// </summary>
        public static double GetCosine(OxyzPointF vBase, OxyzPointF vNew)
        {
            //向量的数量积
            double fNumerator = vBase._X*vNew._X+vBase._Y*vBase._Y;
            //第一个向量（基向量）的摸
            double dBaseM = vBase._X *vBase._X + vBase._Y *vBase._Y;
            double dBase = Math.Sqrt(dBaseM);
            //第二个向量的模
            double dNewM = vNew._X * vNew._X + vNew._Y * vNew._Y;
            double dNew = Math.Sqrt(dNewM);
            //两个向量的模的乘积
            double dDenominator = dBase * dNew;
            if (dDenominator == 0.0)
            {
                throw new DivideByZeroException("向量的模为0是不允许的，无法计算0向量的角度");
            }
           ///返回余弦值
            return fNumerator/dDenominator;
        }
        /// <summary>
        /// 判定角度并且输出角度的正弦和余弦值，315-45 为0度 45-135度变为90度,135-225 为180度 225-315度变为270度，
        /// </summary>
        public static SinCos GetSinCos(OxyzPointF mpBaseVector, OxyzPointF mpVector)
        {
            //由于0.707小于根号2的一半所以45度变为90度
            double dCosineValue = VectorTools.GetCosine(mpBaseVector, mpVector);
            //-45 180 +45度的左开右闭闭区间
            if (-1.001<=dCosineValue&& dCosineValue<-0.708)//-根号2的是是1.414 其一半 是0.707
            {
                return new SinCos(0,-1);//cosine 180 是-1；
            }

            int irtlPos = VectorTools.GetVectorPos(mpBaseVector, mpVector);
            
            if (-0.708 <= dCosineValue && dCosineValue < 0.708)
            { //判断y位于基向量的上方还是下方
                if (irtlPos < -0.1)//-45 270 +45度
                {
                    return new SinCos(-1, 0); //270
                }
                if (irtlPos > 0.1)// -45 90 +45度
                {
                    return new SinCos(1, 0);//90
                }
                if (-0.1<irtlPos&&irtlPos<0.1)
                {
                    return new SinCos(1, 0);
                }
                else//iPos==0 当前角度下 这是不可能出现的
                {
                    throw new Exception("不可能出现的值");
                }
            }
            //这之间的有可能是零度，有可能是360度
            if (0.708 <= dCosineValue && dCosineValue <= 1.001)
            {
                //315度到44度之间全部当作0度处理
                return new SinCos(0, 1);
            }
            throw new Exception("不可能没有捕获所有的角度值");
        }

        /// <summary>
        /// 获取一个向量的unit 法向量，该法向量位于向量右侧,向量不能为零向量
        /// </summary>
        /// <param name="vtr"></param>
        public static OxyzPointF GetNormal(OxyzPointF vtr)
        {
            double iDX = vtr._X*vtr._X;
            double iDY = vtr._Y*vtr._Y;
            double dDistance = Math.Sqrt(iDX + iDY);
            double dx = 0d;
            double dy = 0d;

            OxyzPointF p1=new OxyzPointF(0f,0f,0f) ;
            OxyzPointF p2=new OxyzPointF(0f,0f,0f) ; 

            if (vtr._Y != 0)
            {
                dx = vtr._Y / dDistance;
                dy = dx * vtr._X / vtr._Y;
                p1 = new OxyzPointF((float)dx, (float)dy);
                p2 = new OxyzPointF((float)-dx, (float)-dy);
            }
            else if(vtr._X!=0)
            {
                dy = vtr._X / dDistance;
                dx = dy * vtr._Y / vtr._X;
                p1 = new OxyzPointF((float)dx, (float)dy);
                p2 = new OxyzPointF((float)-dx, (float)-dy);
            }
            if (VectorTools.GetVectorPos(p1,new OxyzPointF(vtr._X,vtr._Y)) ==-1)
            {
                return new OxyzPointF(p1._X,-p1._Y);
            }
            return new OxyzPointF(p2._X,-p2._Y);
        }
        
        public static OxyzPointF GetMultiNormal(OxyzPointF uniVector,int iMuti)
        {
			return  new OxyzPointF(uniVector._X * iMuti, uniVector._Y * iMuti,uniVector._Z*iMuti);
        }
    }
}
 
