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
    /// ���������������нǣ��Լ���������λ�ù�ϵ�ľ�̬������,������Ҫ������֧���ع�
    /// </summary>
    public static class VectorTools
    {
        /// <summary>
        /// ��ȡ������������
        /// </summary>
        /// <param name="opVector"></param>
        /// <returns></returns>
        public static OxyzPointF GetInverse(OxyzPointF opVector)
        {
            return new OxyzPointF(-opVector._X, -opVector._Y);
        }
     
        static VectorTools() { }
        /// <summary>
        /// �Ե�mpA�͵�mpBΪ�յ㽨����ֱ�߷��̣�Ȼ���жϵ�mpNew��ֱ�ߵ��Ϸ������·�
        /// </summary>
        /// <param name="mpA">ֱ���������</param>
        /// <param name="mpB">ֱ���յ�����</param>
        /// <param name="mpNew">Ҫ����ĵ�����</param>
        /// <returns>����0��ʾ��mpNewλ��ֱ���ϣ�����1��ʾ��mpNewλ�������Ϸ�������-1��ʾ��mpNewλ��ֱ���·�</returns>
        public static int GetPointPosition(OxyzPointF mpA,OxyzPointF mpB, OxyzPointF mpNew)
        {
            if ( mpB._X==mpA._X&& mpB._Y==mpA._Y)
	        {
                throw new ArgumentException("ֱ�ߵ������˵㲻����ͬ");
	        }
            double fResult =(mpNew._Y-mpA._Y)*(mpB._X-mpA._X)-(mpNew._X-mpA._X)*(mpB._Y-mpA._Y);
            if(Math.Abs(fResult)<0.9f)//����ֵ��1֮�ڣ���Ϊ������ϵ�������ɵ� 
            {
                return 0;//����0 ��ʾ������ֱ����
            }
            return fResult >= 0.9f ? 1 : -1;
        }

        /// <summary>
        /// ����ǵѿ�������ϵ�µĽ��
        ///��������ʽ�������������̣���������������̼��������������̵���ϵ,���������������أ���һ������Ĭ��Ϊ��,����-1��ʾλ�ڻ������·��������ҷ�������1��ʾλ�ڻ������Ϸ���������
        /// </summary>
        /// <param name="vBase">���������յ����꣬�������Ϊ0</param>
        /// <param name="vNew">Ҫ����ĵ�����������������Ӧ�������������յ�����</param>
        /// <returns>����-1��ʾλ�ڻ������·��������ҷ�������1��ʾλ�ڻ������Ϸ���������</returns>
        public static int GetVectorPos(OxyzPointF vBase, OxyzPointF vNew)
        {
            if (vBase._X==0&&vBase._Y==0
                ||vNew._X==0&&vNew._Y==0)
            {
                throw new ArgumentException("����������Ҫ�ж�����������Ϊ0����");
            }
            int iReturn = VectorTools.GetPointPosition(new OxyzPointF(0.0f, 0.0f), vBase, vNew);
            if (iReturn == 0&&vNew._X < vBase._X)
            {
                 iReturn = 2;
            }
            return iReturn;

        }
        /// <summary>
        /// ��ȡ���������ļнǵ�����ֵ����ֵ��������-1��1������,��������������������0����
        /// </summary>
        public static double GetCosine(OxyzPointF vBase, OxyzPointF vNew)
        {
            //������������
            double fNumerator = vBase._X*vNew._X+vBase._Y*vBase._Y;
            //��һ��������������������
            double dBaseM = vBase._X *vBase._X + vBase._Y *vBase._Y;
            double dBase = Math.Sqrt(dBaseM);
            //�ڶ���������ģ
            double dNewM = vNew._X * vNew._X + vNew._Y * vNew._Y;
            double dNew = Math.Sqrt(dNewM);
            //����������ģ�ĳ˻�
            double dDenominator = dBase * dNew;
            if (dDenominator == 0.0)
            {
                throw new DivideByZeroException("������ģΪ0�ǲ�����ģ��޷�����0�����ĽǶ�");
            }
           ///��������ֵ
            return fNumerator/dDenominator;
        }
        /// <summary>
        /// �ж��ǶȲ�������Ƕȵ����Һ�����ֵ��315-45 Ϊ0�� 45-135�ȱ�Ϊ90��,135-225 Ϊ180�� 225-315�ȱ�Ϊ270�ȣ�
        /// </summary>
        public static SinCos GetSinCos(OxyzPointF mpBaseVector, OxyzPointF mpVector)
        {
            //����0.707С�ڸ���2��һ������45�ȱ�Ϊ90��
            double dCosineValue = VectorTools.GetCosine(mpBaseVector, mpVector);
            //-45 180 +45�ȵ����ұձ�����
            if (-1.001<=dCosineValue&& dCosineValue<-0.708)//-����2������1.414 ��һ�� ��0.707
            {
                return new SinCos(0,-1);//cosine 180 ��-1��
            }

            int irtlPos = VectorTools.GetVectorPos(mpBaseVector, mpVector);
            
            if (-0.708 <= dCosineValue && dCosineValue < 0.708)
            { //�ж�yλ�ڻ��������Ϸ������·�
                if (irtlPos < -0.1)//-45 270 +45��
                {
                    return new SinCos(-1, 0); //270
                }
                if (irtlPos > 0.1)// -45 90 +45��
                {
                    return new SinCos(1, 0);//90
                }
                if (-0.1<irtlPos&&irtlPos<0.1)
                {
                    return new SinCos(1, 0);
                }
                else//iPos==0 ��ǰ�Ƕ��� ���ǲ����ܳ��ֵ�
                {
                    throw new Exception("�����ܳ��ֵ�ֵ");
                }
            }
            //��֮����п�������ȣ��п�����360��
            if (0.708 <= dCosineValue && dCosineValue <= 1.001)
            {
                //315�ȵ�44��֮��ȫ������0�ȴ���
                return new SinCos(0, 1);
            }
            throw new Exception("������û�в������еĽǶ�ֵ");
        }

        /// <summary>
        /// ��ȡһ��������unit ���������÷�����λ�������Ҳ�,��������Ϊ������
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
 
