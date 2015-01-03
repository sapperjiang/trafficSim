using System;
using System.Collections.Generic;
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.TrafficModel
{
    internal class LovelyCoodinates
    {
        public double CalcAngle()
        {
            return 0.0;
        }

        /// <summary>
        /// 以点mpA和点mpB为终点建立的直线方程，然后判断点mpNew在直线的上方还是下方
        /// </summary>
        /// <param name="mpA"></param>
        /// <param name="mpB"></param>
        /// <param name="mpNew">要检验的点坐标</param>
        /// <returns></returns>
        public static int getPostionByBaseEquation(MyPoint mpA,MyPoint mpB, MyPoint mpNew)
        {
            return (mpNew.Y-mpA.Y)*(mpB.X-mpA.X)-(mpNew.X-mpA.X)*(mpB.Y-mpA.Y)>0 ?1:-1;
        }

        /// <summary>
        ///利用两点式，建立基础方程，并且利用这个方程计算输入点与基方程的联系,调用三
        ///参数的重载，第一个参数默认为零,(y-ya)(xb-xa)-(x-xa)(yb-ya) = 0
        /// </summary>
        /// <param name="mpBaseVectorEnd">基向量的终点坐标，起点坐标为0</param>
        /// <param name="mpNew">要检验的点的坐标如果是向量，应当输入向量的终点坐标</param>
        /// <returns>返回-1表示位于基向量下方，返回1表示位于基向量上方</returns>
        public static int getPosByBaseEquation(MyPoint mpBaseVectorEnd, MyPoint mpNew)
        {
            return LovelyCoodinates.getPostionByBaseEquation(new MyPoint(0.0f, 0.0f), mpBaseVectorEnd, mpNew);
        }
        /// <summary>
        /// 获取两个向量的夹角的余弦值，该值的区间是-1到1闭区间,两个参数向量都不能是0向量
        /// </summary>
        public static double getVectorCosine(MyPoint mpBaseVector, MyPoint mpVector)
        {
            //向量的数量积
            double fNumerator = mpBaseVector.X*mpVector.X+mpBaseVector.Y*mpBaseVector.Y;
            //第一个向量（基向量）的摸
            double fMouldBase = mpBaseVector.X *mpBaseVector.X + mpBaseVector.Y *mpBaseVector.Y;
            double dBase = Math.Sqrt(fMouldBase);
            //第二个向量的模
            double fMouldNew = mpBaseVector.X * mpVector.X + mpVector.Y * mpVector.Y;
            double dNew = Math.Sqrt((double)fMouldNew);
            //两个向量的模的乘积
            double dDenominator = dBase * dNew;
            if (dDenominator == 0.0)
            {
                throw new DivideByZeroException("向量的模为0是不允许的");
            }
           ///返回余弦值
            return fNumerator/dDenominator;

        }
        /// <summary>
        /// 判定角度并且输出角度的正弦和余弦值
        /// </summary>
        public static SinCos getSinCos(MyPoint mpBaseVector, MyPoint mpVector)
        {
            double dCosineValue = LovelyCoodinates.getVectorCosine(mpBaseVector, mpVector);
            //180度的左开右闭闭区间
            if (-1<=dCosineValue&& dCosineValue<-1.707)//-根号2的是是1.414 其一半 是0.707
            {
                return new SinCos(0,-1);//cosine 180 是-1；
            }
            ///90度或者270度
            if (-0.707 <= dCosineValue && dCosineValue < -0.707)
            {///判断y位于基向量的上方还是下方
                int y= LovelyCoodinates.getPosByBaseEquation(mpBaseVector,mpVector);
                if (y<0)//270度
                {
                    return new SinCos(-1, 0); 
                }
                if (y > 0)//90度
                {
                    return new SinCos(1, 0);
                }
                else//y==0 
                {
                    throw new Exception("不可能出现的值");
                }
            }
            return null ;
        }

        sealed class SinCos 
        {
            public int iSin = 0;
            public int iCos = 0;
            public SinCos(int iSinine, int iCosine)
            {
                iSin = iSinine;
                iCos = iCosine;
            }
        } 
    }

    public class HashMatrix<T>
    {
        /// <summary>
        /// 最大六个车道，坐标远点是RoadNode的positon
        /// </summary>
        public static int iMaxWidth = 6;

        int iMaxRow;//矩阵中最大行的坐标，进行矩阵的行遍历使用
        int iMaxColumn;//矩阵中最大列的坐标，进行列矩阵的遍历

        public class CellHashKey
        {
            //利用（x,y）计算新存储结构的哈希值，以支持利用x.y快速访问矩阵元素
            public static int GetHashCode(int ix, int iy)
            {
                return (ix.GetHashCode() + iy.GetHashCode()).GetHashCode();
            }
        }

        private Dictionary<int,T> hashMat = new Dictionary<int,T>();
        /// <summary>
        /// 判断元胞是否被占用了
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsCellBlocked(int x, int y)
        {
            return hashMat.ContainsKey(CellHashKey.GetHashCode(x,y));
        }
        /// <summary>
        /// 把元宝从o点移动到d点
        /// </summary>
        public void MoveCell(int xO, int yO, int xD, int yD)
        {
            int iHashkey = CellHashKey.GetHashCode(xO, yO);
            T cac;
            if (hashMat.TryGetValue(iHashkey, out cac) == true)
            {
                hashMat.Remove(CellHashKey.GetHashCode(xO, yO));
                hashMat.Add(CellHashKey.GetHashCode(xD, yD), cac);
            }
        }

        public void AddCell(int x,int y,T cell)
        {
            //更新行和列的最大索引
            this.iMaxColumn = x > this.iMaxColumn ? x : this.iMaxColumn;
            this.iMaxRow = y > this.iMaxRow ? y : this.iMaxRow;

            int iHKey = CellHashKey.GetHashCode(x,y);
            if (!hashMat.ContainsKey(iHKey))
	        {
                hashMat.Add(iHKey,cell);
	        }
        }
        public void RemoveCell(int x, int y)
        {
            hashMat.Remove(CellHashKey.GetHashCode(x, y));
        }
        
        /// <summary>
        /// 枚举某一行的所有元素
        /// </summary>
        /// <param name="iRow">要遍历的行</param>
        /// <returns></returns>
        public List<T> EnumerateRow(int iRow)//行在数学欧氏坐标系中，对应y轴，
        {
            List<T> listT = new List<T>();
            for (int x = 0; x < this.iMaxColumn; x++)
            {
                T cac;
                if (hashMat.TryGetValue(CellHashKey.GetHashCode(x, iRow), out cac) == true)
                {
                    listT.Add(cac);
                }
            }
            return listT;
        }
        /// <summary>
        /// 枚举某一行的所有元素
        /// </summary>
        /// <param name="iRow">要遍历的行</param>
        /// <returns></returns>
        public List<T> EnumerateColumn(int iRow)//行在数学欧氏坐标系中，对应y轴，
        {
            List<T> listT = new List<T>();
            for (int x = 0; x < this.iMaxColumn; x++)
            {
                T cac;
                if (hashMat.TryGetValue(CellHashKey.GetHashCode(x, iRow), out cac) == true)
                {
                    listT.Add(cac);
                }
            }
            return listT;
        }
    }

    /// <summary>
    /// 使用矩阵类型的结构意味着不支持五路交叉.环路的支持有待讨论，三路交叉是支持的
    /// </summary>
    public class RoadNode : RoadEntity
    {
        /// <summary>
        /// 控制RoadNodeID的数量
        /// </summary>
        private static int iRoadNodeID;
        public RoadNode() 
        {
            this.ID = ++iRoadNodeID;
            /// 直接使用上下文的数据结构,bug不应当使用上下文结构
        }
        /// <summary>
        /// 用哈希表来查询更快速，键值是代表边的RoadSegment，值是代表
        /// </summary>
        private Dictionary<int, RoadEdge> dicEdge = new Dictionary<int,RoadEdge>();
        //public bool visited; //访问标志,遍历时使用
        /// <summary>
        /// 新的roadnode的哈希散列值由其中心Position的哈希值和其ID构成
        /// </summary>
        /// <returns></returns>
        public HashMatrix<CACell> cellMatix;
    
        /// <summary>
        /// 存储因为时间不在一个序列的进入节点的车辆，只应该在初始化的地方出现,
        /// 当车辆完全进入路路网址之后，该存贮结构应当为空
        /// </summary>
        Queue<CACell> queWaitedCACell = new Queue<CACell>();

        #region 用来保存邻接矩阵中节点出边的边成员,不应当使用RoadNetwork之外的类访问这些成员

        /// <summary>
        /// 注意在出边表中，保持roadedge的from字段是this节点，否则函数抛出异常
        /// </summary>
        /// <param name="roadEdge"></param>
        internal void AddRoadEdge(RoadEdge roadEdge)
        {
            if (roadEdge != null)
            {
                if (!Contains(roadEdge.GetHashCode()))
                {
                    //加入判断是否是当前点的出边的信息防止出错
                    if (roadEdge.rnFrom !=this)
                    {
                        throw new Exception("添加了不属于该顶点的边");
                    }
                    dicEdge.Add(roadEdge.GetHashCode(), roadEdge);
                }
                else
                {
                    throw new ArgumentException("添加了重复的边！");
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        /// <summary>
        /// 找到边 从this到toNode节点的边，出边表
        /// </summary>
        /// <param name="fromRN"></param>
        internal void RemoveEdge(RoadEdge re)
        {
            if (re == null )
            {
                throw new ArgumentNullException();
            }
            dicEdge.Remove(RoadEdge.GetHashCode(this,re.rnTo));
        }
        internal void RemoveEdge(RoadNode toRN)
        {
            if (toRN == null)
            {
                throw new ArgumentNullException();
            }
            dicEdge.Remove(RoadEdge.GetHashCode(this,toRN));
        }
        internal void RemoveEdge(int iEdgeHashKey)
        {
            dicEdge.Remove(iEdgeHashKey);
        }

        /// <summary>
        /// 查找方法，新的结构采用出边表
        /// </summary>
        /// <param name="toRoadNode">出节点</param>
        /// <returns></returns>
        internal RoadEdge FindRoadEdge(RoadNode toRoadNode)
        {
            int iHashkey = RoadEdge.GetHashCode(this,toRoadNode);
            if (dicEdge.ContainsKey(iHashkey))
            {
                return dicEdge[iHashkey];
            }
            return null;
        }
        internal int RoadEdgeCount
        {
            get { return dicEdge.Count; }
        }
        internal bool Contains(int EdgeKey)
        {
            return dicEdge.ContainsKey(EdgeKey);
        }

        #endregion
        public override int GetHashCode()
        {
            int iHash = this.Postion.GetHashCode() +this.ID.GetHashCode();
            return iHash.GetHashCode();
        }
        public override void UpdateStatus()
        {
            //更新异步消息
            for (int i = 0; i < this.asynRuleChain.Count; i++)
            {
                UpdateRule.UpdateRule visitorRule = this.asynRuleChain[i];
                visitorRule.VisitUpdate(this);//.VisitUpdate();
            }
            ////更新同步消息
            //foreach (UpdateRule.UpdateRule item in this.synRuleChain)
            //{
            //    if (item != null)
            //    {
            //        item.Update();//visitor.visit 一个规则就是一个访问者，很多的访问者
            //    }
            //}
        }
    }
}
 
