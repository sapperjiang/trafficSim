using System;
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.MathSupport
{

    internal sealed class SinCos
    {
        internal int iSin = 0;
        internal int iCos = 0;
        internal SinCos(int iSinine, int iCosine)
        {
            iSin = iSinine;
            iCos = iCosine;
        }

        public override bool Equals(object obj)
        {
            SinCos item = (SinCos)obj;
            if (item.iSin == this.iSin
                && item.iCos == this.iCos)
            {
                return true;
            } return false;
        }
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
        internal static MyPoint getAgainstVector(MyPoint opVector)
        {
            return new MyPoint(-opVector.X, -opVector.Y);
        }
        /// <summary>
        //�ߵ��յ������ȥ�������������
        /// </summary>
        /// <param name="re"></param>
        /// <returns></returns>
        public static MyPoint getVector(RoadEdge re)
        {
            return new MyPoint(re.To.rltPos.X - re.From.rltPos.X, re.To.rltPos.Y - re.From.rltPos.Y);
        }
     
        static VectorTools() { }
        /// <summary>
        /// �Ե�mpA�͵�mpBΪ�յ㽨����ֱ�߷��̣�Ȼ���жϵ�mpNew��ֱ�ߵ��Ϸ������·�
        /// </summary>
        /// <param name="mpA">ֱ���������</param>
        /// <param name="mpB">ֱ���յ�����</param>
        /// <param name="mpNew">Ҫ����ĵ�����</param>
        /// <returns>����0��ʾ��mpNewλ��ֱ���ϣ�����1��ʾ��mpNewλ�������Ϸ�������-1��ʾ��mpNewλ��ֱ���·�</returns>
        internal static int getPointPos(MyPoint mpA,MyPoint mpB, MyPoint mpNew)
        {
            if ( mpB.X==mpA.X&& mpB.Y==mpA.Y)
	        {
                throw new ArgumentException("ֱ�ߵ������˵㲻����ͬ");
	        }
            float fResult =(mpNew.Y-mpA.Y)*(mpB.X-mpA.X)-(mpNew.X-mpA.X)*(mpB.Y-mpA.Y);
            if(Math.Abs(fResult)<0.9f)//����ֵ��1֮�ڣ���Ϊ������ϵ�������ɵ� 
            {
                return 0;//����0 ��ʾ������ֱ����
            }
            return fResult >= 0.9f ? 1 : -1;
        }

        /// <summary>
        /// ����ǵѿ�������ϵ�µĽ��
        ///��������ʽ�������������̣���������������̼��������������̵���ϵ,������
        ///���������أ���һ������Ĭ��Ϊ��,����-1��ʾλ�ڻ������·��������ҷ�������1��ʾλ�ڻ������Ϸ���������
        /// </summary>
        /// <param name="vBase">���������յ����꣬�������Ϊ0</param>
        /// <param name="vNew">Ҫ����ĵ�����������������Ӧ�������������յ�����</param>
        /// <returns>����-1��ʾλ�ڻ������·��������ҷ�������1��ʾλ�ڻ������Ϸ���������</returns>
        internal static int getVectorPos(MyPoint vBase, MyPoint vNew)
        {
            if (vBase.X==0&&vBase.Y==0
                ||vNew.X==0&&vNew.Y==0)
            {
                throw new ArgumentException("����������Ҫ�ж�����������Ϊ0����");
            }
            return VectorTools.getPointPos(new MyPoint(0.0f, 0.0f), vBase, vNew);
        }
        /// <summary>
        /// ��ȡ���������ļнǵ�����ֵ����ֵ��������-1��1������,��������������������0����
        /// </summary>
        internal static double getCosine(MyPoint vBase, MyPoint vNew)
        {
            //������������
            double fNumerator = vBase.X*vNew.X+vBase.Y*vBase.Y;
            //��һ��������������������
            double dBaseM = vBase.X *vBase.X + vBase.Y *vBase.Y;
            double dBase = Math.Sqrt(dBaseM);
            //�ڶ���������ģ
            double dNewM = vNew.X * vNew.X + vNew.Y * vNew.Y;
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
        /// �ж��ǶȲ�������Ƕȵ����Һ�����ֵ��315-45 Ϊ0�� 45-135�ȱ�Ϊ90��
        /// 135-225 Ϊ180�� 225-315�ȱ�Ϊ270�ȣ�
        /// </summary>
        internal static SinCos getSinCos(MyPoint mpBaseVector, MyPoint mpVector)
        {
            //����0.707С�ڸ���2��һ������45�ȱ�Ϊ90��
            double dCosineValue = VectorTools.getCosine(mpBaseVector, mpVector);
            //-45 180 +45�ȵ����ұձ�����
            if (-1.001<=dCosineValue&& dCosineValue<-0.708)//-����2������1.414 ��һ�� ��0.707
            {
                return new SinCos(0,-1);//cosine 180 ��-1��
            }

            int irtlPos = VectorTools.getVectorPos(mpBaseVector, mpVector);
            
            if (-0.708 <= dCosineValue && dCosineValue < 0.708)
            {///�ж�yλ�ڻ��������Ϸ������·�
                if (irtlPos < 0)//-45 270 +45��
                {
                    return new SinCos(-1, 0); //270
                }
                if (irtlPos > 0)// -45 90 +45��
                {
                    return new SinCos(1, 0);//90
                }
                else//iPos==0 ��ǰ�Ƕ��� ���ǲ����ܳ��ֵ�
                {
                    throw new Exception("�����ܳ��ֵ�ֵ");
                }
            }
            ///��֮����п�������ȣ��п�����360��
            if (0.708 <= dCosineValue && dCosineValue <= 1.001)
            {
                //315�ȵ�44��֮��ȫ������0�ȴ���
                return new SinCos(0, 1);
            }
            throw new Exception("������û�в������еĽǶ�ֵ");
        }

        /// <summary>
        /// ��ȡһ�������ķ��������÷�����λ�������Ҳ�,��������Ϊ������
        /// </summary>
        /// <param name="vtr"></param>
        public static MyPoint getNormalVector(MyPoint vtr)
        {
            double iDX = vtr.X*vtr.X;
            double iDY = vtr.Y*vtr.Y;
            double dDistance = Math.Sqrt(iDX + iDY);
            double dx = 0d;
            double dy = 0d;

            MyPoint p1 = null;
            MyPoint p2 = null;

            if (vtr.Y != 0)
            {
                dx = vtr.Y / dDistance;
                dy = dx * vtr.X / vtr.Y;
                p1 = new MyPoint((float)dx, (float)dy);
                p2 = new MyPoint((float)-dx, (float)-dy);
            }
            else if(vtr.X!=0)
            {
                dy = vtr.X / dDistance;
                dx = dy * vtr.Y / vtr.X;
                p1 = new MyPoint((float)dx, (float)dy);
                p2 = new MyPoint((float)-dx, (float)-dy);
            }
            if (VectorTools.getVectorPos(p1,new MyPoint(vtr.X,vtr.Y)) ==-1)
            {
                return new MyPoint(p1.X,-p1.Y);
            }
            return new MyPoint(p2.X,-p2.Y);
        }
    }
}
 
