using System;
using System.Diagnostics;
using System.Drawing;

namespace SubSys_MathUtility
{


    /// <summary>
    /// 计算向量和向量夹角，以及计算向量位置关系的静态数据类,可能需要重载以支持重构
    /// this tools use cartesian coordinates .so .used with cooridnates needs to convert.
    /// </summary>
    public static class LineTools
    {
       
        /// <summary>
        /// 判断两条线是否相交
        /// </summary>
        /// <param name="a">线段1起点坐标</param>
        /// <param name="b">线段1终点坐标</param>
        /// <param name="c">线段2起点坐标</param>
        /// <param name="d">线段2终点坐标</param>
        /// <param name="intersection">相交点坐标</param>
        /// <returns>是否相交 0:两线平行  -1:不平行且未相交  1:两线相交</returns>
        public static int GetIntersection(Point a, Point b, Point c, Point d, ref Point intersection)
        {
            //判断异常
            if (Math.Abs(b.X - a.Y) + Math.Abs(b.X - a.X) + Math.Abs(d.Y - c.Y) + Math.Abs(d.X - c.X) == 0)
            {
                if (c.X - a.X == 0)
                {
                    Debug.Print("ABCD是同一个点！");
                }
                else
                {
                    Debug.Print("AB是一个点，CD是一个点，且AC不同！");
                }
                return 0;
            }

            if (Math.Abs(b.Y - a.Y) + Math.Abs(b.X - a.X) == 0)
            {
                if ((a.X - d.X) * (c.Y - d.Y) - (a.Y - d.Y) * (c.X - d.X) == 0)
                {
                    Debug.Print("A、B是一个点，且在CD线段上！");
                }
                else
                {
                    Debug.Print("A、B是一个点，且不在CD线段上！");
                }
                return 0;
            }
            if (Math.Abs(d.Y - c.Y) + Math.Abs(d.X - c.X) == 0)
            {
                if ((d.X - b.X) * (a.Y - b.Y) - (d.Y - b.Y) * (a.X - b.X) == 0)
                {
                    Debug.Print("C、D是一个点，且在AB线段上！");
                }
                else
                {
                    Debug.Print("C、D是一个点，且不在AB线段上！");
                }
            }

            if ((b.Y - a.Y) * (c.X - d.X) - (b.X - a.X) * (c.Y - d.Y) == 0)
            {
                Debug.Print("线段平行，无交点！");
                return 0;
            }

            intersection.X = ((b.X - a.X) * (c.X - d.X) * (c.Y - a.Y) - c.X * (b.X - a.X) * (c.Y - d.Y) + a.X * (b.Y - a.Y) * (c.X - d.X)) / ((b.Y - a.Y) * (c.X - d.X) - (b.X - a.X) * (c.Y - d.Y));
            intersection.Y = ((b.Y - a.Y) * (c.Y - d.Y) * (c.X - a.X) - c.Y * (b.Y - a.Y) * (c.X - d.X) + a.Y * (b.X - a.X) * (c.Y - d.Y)) / ((b.X - a.X) * (c.Y - d.Y) - (b.Y - a.Y) * (c.X - d.X));

            if ((intersection.X - a.X) * (intersection.X - b.X) <= 0 && (intersection.X - c.X) * (intersection.X - d.X) <= 0 && (intersection.Y - a.Y) * (intersection.Y - b.Y) <= 0 && (intersection.Y - c.Y) * (intersection.Y - d.Y) <= 0)
            {
                Debug.Print("线段相交于点(" + intersection.X + "," + intersection.Y + ")！");
                return 1; //'相交
            }
            else
            {
                Debug.Print("线段相交于虚交点(" + intersection.X + "," + intersection.Y + ")！");
                return -1; //'相交但不在线段上
            }
        }

        /// <summary>
        /// 计算两条直线的交点
        /// </summary>
        /// <param name="lineFirstStar">L1的点1坐标</param>
        /// <param name="lineFirstEnd">L1的点2坐标</param>
        /// <param name="lineSecondStar">L2的点1坐标</param>
        /// <param name="lineSecondEnd">L2的点2坐标</param>
        /// <returns></returns>
        public static PointF GetIntersection(PointF lineFirstStar, PointF lineFirstEnd, PointF lineSecondStar, PointF lineSecondEnd)
        {
            /*
             * L1，L2都存在斜率的情况：
             * 直线方程L1: ( y - y1 ) / ( y2 - y1 ) = ( x - x1 ) / ( x2 - x1 ) 
             * => y = [ ( y2 - y1 ) / ( x2 - x1 ) ]( x - x1 ) + y1
             * 令 a = ( y2 - y1 ) / ( x2 - x1 )
             * 有 y = a * x - a * x1 + y1   .........1
             * 直线方程L2: ( y - y3 ) / ( y4 - y3 ) = ( x - x3 ) / ( x4 - x3 )
             * 令 b = ( y4 - y3 ) / ( x4 - x3 )
             * 有 y = b * x - b * x3 + y3 ..........2
             * 
             * 如果 a = b，则两直线平等，否则， 联解方程 1,2，得:
             * x = ( a * x1 - b * x3 - y1 + y3 ) / ( a - b )
             * y = a * x - a * x1 + y1
             * 
             * L1存在斜率, L2平行Y轴的情况：
             * x = x3
             * y = a * x3 - a * x1 + y1
             * 
             * L1 平行Y轴，L2存在斜率的情况：
             * x = x1
             * y = b * x - b * x3 + y3
             * 
             * L1与L2都平行Y轴的情况：
             * 如果 x1 = x3，那么L1与L2重合，否则平等
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
                case 0: //L1与L2都平行Y轴
                    {
                        if (lineFirstStar.X == lineSecondStar.X)
                        {
                            //throw new Exception("两条直线互相重合，且平行于Y轴，无法计算交点。");
                            return new PointF(0, 0);
                        }
                        else
                        {
                            //throw new Exception("两条直线互相平行，且平行于Y轴，无法计算交点。");
                            return new PointF(0, 0);
                        }
                    }
                case 1: //L1存在斜率, L2平行Y轴
                    {
                        float x = lineSecondStar.X;
                        float y = (lineFirstStar.X - x) * (-a) + lineFirstStar.Y;
                        return new PointF(x, y);
                    }
                case 2: //L1 平行Y轴，L2存在斜率
                    {
                        float x = lineFirstStar.X;
                        //网上有相似代码的，这一处是错误的。你可以对比case 1 的逻辑 进行分析
                        //源code:lineSecondStar * x + lineSecondStar * lineSecondStar.X + p3.Y;
                        float y = (lineSecondStar.X - x) * (-b) + lineSecondStar.Y;
                        return new PointF(x, y);
                    }
                case 3: //L1，L2都存在斜率
                    {
                        if (a == b)
                        {
                            // throw new Exception("两条直线平行或重合，无法计算交点。");
                            return new PointF(0, 0);
                        }
                        float x = (a * lineFirstStar.X - b * lineSecondStar.X - lineFirstStar.Y + lineSecondStar.Y) / (a - b);
                        float y = a * x - a * lineFirstStar.X + lineFirstStar.Y;
                        return new PointF(x, y);
                    }
            }
            // throw new Exception("不可能发生的情况");
            return new PointF(0, 0);
        }

        /// <summary>
        //所谓算法三, 
        //判断每一条线段的两个端点是否都在另一条线段的两侧, 是则求出两条线段所在直线的交点, 否则不相交. 
        //其实只是对算法二的一个改良, 改良的地方主要就是 : 
        //不通过法线投影来判断点和线段的位置关系, 而是通过点和线段构成的三角形面积来判断.

        //先来复习下三角形面积公式: 已知三角形三点a(x, y) b(x, y) c(x, y), 三角形面积为: 

        //1.var triArea = ((a.x - c.x) * (b.y - c.y) - (a.y - c.y) * (b.x - c.x)) / 2;

        //因为 两向量叉乘==两向量构成的平行四边形(以两向量为邻边)的面积 , 所以上面的公式也不难理解.
        //而且由于向量是有方向的, 所以面积也是有方向的, 通常我们以逆时针为正, 顺时针为负数.

        //改良算法关键点就是: 
        //如果"线段ab和点c构成的三角形面积"与"线段ab和点d构成的三角形面积" 构成的三角形面积的正负符号相异, 
        //那么点c和点d位于线段ab两侧.如下图所示:

        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>

        public static PointF GetIntersection (Point a, Point b, Point c, Point d)
        {

            // 三角形abc 面积的2倍  
            var area_abc = (a.X - c.X) * (b.Y - c.Y) - (a.Y - c.Y) * (b.X - c.X);

            // 三角形abd 面积的2倍  
            var area_abd = (a.X - d.X) * (b.Y - d.Y) - (a.Y - d.Y) * (b.X - d.X);

            // 面积符号相同则两点在线段同侧,不相交 (对点在线段上的情况 下面的公式结果为0,本例当作相交处理);  
            if (area_abc * area_abd > 0)
            {
                return PointF.Empty;
            }

            // 三角形cda 面积的2倍  
            var area_cda = (c.X - a.X) * (d.Y - a.Y) - (c.Y - a.Y) * (d.X - a.X);
            // 三角形cdb 面积的2倍  
            // 注意: 这里有一个小优化.不需要再用公式计算面积,而是通过已知的三个面积加减得出.  
            var area_cdb = area_cda + area_abc - area_abd;
            if (area_cda * area_cdb >= 0)
            {
                return PointF.Empty;
            }

            //计算交点坐标  
            var t = area_cda / (area_abd - area_abc);


            var dx = t * (b.X - a.X);
            var dy = t * (b.Y - a.Y);
            return new PointF(a.X + dx ,a.Y + dy);

        }


    }
}
 
