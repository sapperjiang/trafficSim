using System;
using System.Diagnostics;
using System.Drawing;

namespace SubSys_MathUtility
{


    /// <summary>
    /// ���������������нǣ��Լ���������λ�ù�ϵ�ľ�̬������,������Ҫ������֧���ع�
    /// this tools use cartesian coordinates .so .used with cooridnates needs to convert.
    /// </summary>
    public static class LineTools
    {
       
        /// <summary>
        /// �ж��������Ƿ��ཻ
        /// </summary>
        /// <param name="a">�߶�1�������</param>
        /// <param name="b">�߶�1�յ�����</param>
        /// <param name="c">�߶�2�������</param>
        /// <param name="d">�߶�2�յ�����</param>
        /// <param name="intersection">�ཻ������</param>
        /// <returns>�Ƿ��ཻ 0:����ƽ��  -1:��ƽ����δ�ཻ  1:�����ཻ</returns>
        public static int GetIntersection(Point a, Point b, Point c, Point d, ref Point intersection)
        {
            //�ж��쳣
            if (Math.Abs(b.X - a.Y) + Math.Abs(b.X - a.X) + Math.Abs(d.Y - c.Y) + Math.Abs(d.X - c.X) == 0)
            {
                if (c.X - a.X == 0)
                {
                    Debug.Print("ABCD��ͬһ���㣡");
                }
                else
                {
                    Debug.Print("AB��һ���㣬CD��һ���㣬��AC��ͬ��");
                }
                return 0;
            }

            if (Math.Abs(b.Y - a.Y) + Math.Abs(b.X - a.X) == 0)
            {
                if ((a.X - d.X) * (c.Y - d.Y) - (a.Y - d.Y) * (c.X - d.X) == 0)
                {
                    Debug.Print("A��B��һ���㣬����CD�߶��ϣ�");
                }
                else
                {
                    Debug.Print("A��B��һ���㣬�Ҳ���CD�߶��ϣ�");
                }
                return 0;
            }
            if (Math.Abs(d.Y - c.Y) + Math.Abs(d.X - c.X) == 0)
            {
                if ((d.X - b.X) * (a.Y - b.Y) - (d.Y - b.Y) * (a.X - b.X) == 0)
                {
                    Debug.Print("C��D��һ���㣬����AB�߶��ϣ�");
                }
                else
                {
                    Debug.Print("C��D��һ���㣬�Ҳ���AB�߶��ϣ�");
                }
            }

            if ((b.Y - a.Y) * (c.X - d.X) - (b.X - a.X) * (c.Y - d.Y) == 0)
            {
                Debug.Print("�߶�ƽ�У��޽��㣡");
                return 0;
            }

            intersection.X = ((b.X - a.X) * (c.X - d.X) * (c.Y - a.Y) - c.X * (b.X - a.X) * (c.Y - d.Y) + a.X * (b.Y - a.Y) * (c.X - d.X)) / ((b.Y - a.Y) * (c.X - d.X) - (b.X - a.X) * (c.Y - d.Y));
            intersection.Y = ((b.Y - a.Y) * (c.Y - d.Y) * (c.X - a.X) - c.Y * (b.Y - a.Y) * (c.X - d.X) + a.Y * (b.X - a.X) * (c.Y - d.Y)) / ((b.X - a.X) * (c.Y - d.Y) - (b.Y - a.Y) * (c.X - d.X));

            if ((intersection.X - a.X) * (intersection.X - b.X) <= 0 && (intersection.X - c.X) * (intersection.X - d.X) <= 0 && (intersection.Y - a.Y) * (intersection.Y - b.Y) <= 0 && (intersection.Y - c.Y) * (intersection.Y - d.Y) <= 0)
            {
                Debug.Print("�߶��ཻ�ڵ�(" + intersection.X + "," + intersection.Y + ")��");
                return 1; //'�ཻ
            }
            else
            {
                Debug.Print("�߶��ཻ���齻��(" + intersection.X + "," + intersection.Y + ")��");
                return -1; //'�ཻ�������߶���
            }
        }

        /// <summary>
        /// ��������ֱ�ߵĽ���
        /// </summary>
        /// <param name="lineFirstStar">L1�ĵ�1����</param>
        /// <param name="lineFirstEnd">L1�ĵ�2����</param>
        /// <param name="lineSecondStar">L2�ĵ�1����</param>
        /// <param name="lineSecondEnd">L2�ĵ�2����</param>
        /// <returns></returns>
        public static PointF GetIntersection(PointF lineFirstStar, PointF lineFirstEnd, PointF lineSecondStar, PointF lineSecondEnd)
        {
            /*
             * L1��L2������б�ʵ������
             * ֱ�߷���L1: ( y - y1 ) / ( y2 - y1 ) = ( x - x1 ) / ( x2 - x1 ) 
             * => y = [ ( y2 - y1 ) / ( x2 - x1 ) ]( x - x1 ) + y1
             * �� a = ( y2 - y1 ) / ( x2 - x1 )
             * �� y = a * x - a * x1 + y1   .........1
             * ֱ�߷���L2: ( y - y3 ) / ( y4 - y3 ) = ( x - x3 ) / ( x4 - x3 )
             * �� b = ( y4 - y3 ) / ( x4 - x3 )
             * �� y = b * x - b * x3 + y3 ..........2
             * 
             * ��� a = b������ֱ��ƽ�ȣ����� ���ⷽ�� 1,2����:
             * x = ( a * x1 - b * x3 - y1 + y3 ) / ( a - b )
             * y = a * x - a * x1 + y1
             * 
             * L1����б��, L2ƽ��Y��������
             * x = x3
             * y = a * x3 - a * x1 + y1
             * 
             * L1 ƽ��Y�ᣬL2����б�ʵ������
             * x = x1
             * y = b * x - b * x3 + y3
             * 
             * L1��L2��ƽ��Y��������
             * ��� x1 = x3����ôL1��L2�غϣ�����ƽ��
             * 
            */
            float a = 0, b = 0;
            int state = 0;
            if (lineFirstStar.X != lineFirstEnd.X)
            {
                a = (lineFirstEnd.Y - lineFirstStar.Y) / (lineFirstEnd.X - lineFirstStar.X);
                state |= 1;
            }
            if (lineSecondStar.X != lineSecondEnd.X)
            {
                b = (lineSecondEnd.Y - lineSecondStar.Y) / (lineSecondEnd.X - lineSecondStar.X);
                state |= 2;
            }
            switch (state)
            {
                case 0: //L1��L2��ƽ��Y��
                    {
                        if (lineFirstStar.X == lineSecondStar.X)
                        {
                            //throw new Exception("����ֱ�߻����غϣ���ƽ����Y�ᣬ�޷����㽻�㡣");
                            return new PointF(0, 0);
                        }
                        else
                        {
                            //throw new Exception("����ֱ�߻���ƽ�У���ƽ����Y�ᣬ�޷����㽻�㡣");
                            return new PointF(0, 0);
                        }
                    }
                case 1: //L1����б��, L2ƽ��Y��
                    {
                        float x = lineSecondStar.X;
                        float y = (lineFirstStar.X - x) * (-a) + lineFirstStar.Y;
                        return new PointF(x, y);
                    }
                case 2: //L1 ƽ��Y�ᣬL2����б��
                    {
                        float x = lineFirstStar.X;
                        //���������ƴ���ģ���һ���Ǵ���ġ�����ԶԱ�case 1 ���߼� ���з���
                        //Դcode:lineSecondStar * x + lineSecondStar * lineSecondStar.X + p3.Y;
                        float y = (lineSecondStar.X - x) * (-b) + lineSecondStar.Y;
                        return new PointF(x, y);
                    }
                case 3: //L1��L2������б��
                    {
                        if (a == b)
                        {
                            // throw new Exception("����ֱ��ƽ�л��غϣ��޷����㽻�㡣");
                            return new PointF(0, 0);
                        }
                        float x = (a * lineFirstStar.X - b * lineSecondStar.X - lineFirstStar.Y + lineSecondStar.Y) / (a - b);
                        float y = a * x - a * lineFirstStar.X + lineFirstStar.Y;
                        return new PointF(x, y);
                    }
            }
            // throw new Exception("�����ܷ��������");
            return new PointF(0, 0);
        }

        /// <summary>
        //��ν�㷨��, 
        //�ж�ÿһ���߶ε������˵��Ƿ�����һ���߶ε�����, ������������߶�����ֱ�ߵĽ���, �����ཻ. 
        //��ʵֻ�Ƕ��㷨����һ������, �����ĵط���Ҫ���� : 
        //��ͨ������ͶӰ���жϵ���߶ε�λ�ù�ϵ, ����ͨ������߶ι��ɵ�������������ж�.

        //������ϰ�������������ʽ: ��֪����������a(x, y) b(x, y) c(x, y), ���������Ϊ: 

        //1.var triArea = ((a.x - c.x) * (b.y - c.y) - (a.y - c.y) * (b.x - c.x)) / 2;

        //��Ϊ ���������==���������ɵ�ƽ���ı���(��������Ϊ�ڱ�)����� , ��������Ĺ�ʽҲ�������.
        //���������������з����, �������Ҳ���з����, ͨ����������ʱ��Ϊ��, ˳ʱ��Ϊ����.

        //�����㷨�ؼ������: 
        //���"�߶�ab�͵�c���ɵ����������"��"�߶�ab�͵�d���ɵ����������" ���ɵ������������������������, 
        //��ô��c�͵�dλ���߶�ab����.����ͼ��ʾ:

        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>

        public static PointF GetIntersection (Point a, Point b, Point c, Point d)
        {

            // ������abc �����2��  
            var area_abc = (a.X - c.X) * (b.Y - c.Y) - (a.Y - c.Y) * (b.X - c.X);

            // ������abd �����2��  
            var area_abd = (a.X - d.X) * (b.Y - d.Y) - (a.Y - d.Y) * (b.X - d.X);

            // ���������ͬ���������߶�ͬ��,���ཻ (�Ե����߶��ϵ���� ����Ĺ�ʽ���Ϊ0,���������ཻ����);  
            if (area_abc * area_abd > 0)
            {
                return PointF.Empty;
            }

            // ������cda �����2��  
            var area_cda = (c.X - a.X) * (d.Y - a.Y) - (c.Y - a.Y) * (d.X - a.X);
            // ������cdb �����2��  
            // ע��: ������һ��С�Ż�.����Ҫ���ù�ʽ�������,����ͨ����֪����������Ӽ��ó�.  
            var area_cdb = area_cda + area_abc - area_abd;
            if (area_cda * area_cdb >= 0)
            {
                return PointF.Empty;
            }

            //���㽻������  
            var t = area_cda / (area_abd - area_abc);


            var dx = t * (b.X - a.X);
            var dy = t * (b.Y - a.Y);
            return new PointF(a.X + dx ,a.Y + dy);

        }


    }
}
 
