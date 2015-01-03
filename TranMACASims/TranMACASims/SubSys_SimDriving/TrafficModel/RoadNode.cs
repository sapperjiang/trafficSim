using System;
using System.Collections;
using System.Collections.Generic;
using SubSys_SimDriving;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.MathSupport;

namespace SubSys_SimDriving.TrafficModel
{
    internal class HashMatrix<T>
    {
        /// <summary>
        /// 最大六个车道，坐标远点是RoadNode的positon
        /// </summary>
        internal readonly int iMaxWidth = SimContext.SimSettings.iMaxWidth;

        internal class CellHashKey
        {
            //利用（x,y）计算新存储结构的哈希值，以支持利用x.y快速访问矩阵元素
            internal static int GetHashCode(int ix, int iy)
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
        internal bool IsCellBlocked(int x, int y)
        {
            return hashMat.ContainsKey(CellHashKey.GetHashCode(x,y));
        }
        /// <summary>
        /// 把元宝从o点移动到d点
        /// </summary>
        internal bool MoveCell(Index inXY, Index inXY_D)
        {
            int iHashkey = CellHashKey.GetHashCode(inXY.X, inXY.Y);
            T cac;
            if (hashMat.TryGetValue(iHashkey, out cac) == true)
            {
                hashMat.Remove(iHashkey);
                hashMat.Add(CellHashKey.GetHashCode(inXY_D.X,inXY_D.Y), cac);
                return true;
            }
            return false;
        }
        internal void AddCell(int x,int y,T cell)
        {
            //更新行和列的最大索引
            if (Math.Abs(x) >this.iMaxWidth||Math.Abs(y) > this.iMaxWidth)
            {
                throw new ArgumentOutOfRangeException("x或者y 参数超出了默认的最大数值");
            }
            int iHKey = CellHashKey.GetHashCode(x,y);
            if (!hashMat.ContainsKey(iHKey))
	        {
                hashMat.Add(iHKey,cell);
	        }
        }
        internal bool RemoveCell(int x, int y)
        {
            return hashMat.Remove(CellHashKey.GetHashCode(x, y));
        }

        #region 枚举器

        /// <summary>
        /// 枚举某一行的所有元素
        /// </summary>
        /// <param name="iRow">要遍历的行</param>
        /// <returns></returns>
        internal List<T> EnumerateRow(int iRow)//行在欧氏坐标系中，对应y轴，
        {
            List<T> listT = new List<T>();
            for (int x = 0; x < this.iMaxWidth; x++)
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
        internal List<T> EnumerateColumn(int iRow)
        {
            List<T> listT = new List<T>();
            for (int x = 0; x < this.iMaxWidth; x++)
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
        /// 提供对存储元素的高效遍历
        /// </summary>
        /// <returns></returns>
        internal IEnumerator<T> GetEnumerator()
        {
            return this.hashMat.Values.GetEnumerator();
        }
        #endregion

    }

    /// <summary>
    /// 使用矩阵类型的结构意味着不支持五路交叉.环路的支持有待讨论，三路交叉是支持的
    /// </summary>
    internal class RoadNode : RoadEntity
    {
         #region 坐标系旋转方法逻辑方法
        /// <summary>
        /// 计算坐标系旋转的基准向量
        /// </summary>
        private MyPoint mpBaseVector;
        /// <summary>
        /// 返回该边的逆向量（入边向量）与基向量的夹角的正弦和余弦的值
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        private Index getOriginIndex(Index point, MyPoint newVector)
        {
            if (point == null || newVector == null)
            {
                throw new ArgumentException("输入的参数不能为零");
            }
            if(mpBaseVector==null)
            {
                mpBaseVector = newVector;
            }
            //获取正弦和余弦值并且进行旋转变换
            SinCos sc = VectorTools.getSinCos(mpBaseVector, newVector);
            return Coordinates.Rotate(point, sc);
        }
        /// <summary>
        /// 判断指定的坐标是否有元胞占据
        /// </summary>
        /// <param name="point"></param>
        /// <param name="re"></param>
        /// <returns></returns>
        internal bool IsCellBlocked(RoadLane rl, int iAhead)
        {
            //旋转变换
            Index indexOld = this.getOriginIndex(rl.ToCenterXY(iAhead),rl.parEntity.ToVector());
            return cellMatrix.IsCellBlocked(indexOld.X, indexOld.Y);
        }
        internal bool IsCellBlocked(Index point)
        {
            return cellMatrix.IsCellBlocked(point.X, point.Y);
        }
        internal bool MoveCell(Index iOldPoint,Index iNewPoint)
        {
            return cellMatrix.MoveCell(iOldPoint, iNewPoint);
        }

        /// <summary>
        /// 将车道堵塞
        /// </summary>
        /// <param name="rl"></param>
        internal void BlockRoadLane(RoadLane rl)
        {
            if (rl == null)
            {
                throw new ArgumentNullException();
            }
            if (!IsCellBlocked(rl,1))//空的则添加
            {//用null 将位置 rl.rank 和第1-6个位置的元胞占据
                Index iOrgIndex = this.getOriginIndex(rl.ToCenterXY(1), rl.parEntity.ToVector());
                this.AddCell(iOrgIndex,null);//(id, rl.parEntity, null);
            }
        }
        /// <summary>
        /// 将车道疏通
        /// </summary>
        /// <param name="rl"></param>
        internal void UnblockRoadLane(RoadLane rl)
        {
            if (rl == null)
            {
                throw new ArgumentNullException();
            }
            if (IsCellBlocked(rl, 1))//非空则删除
            {//用null 将位置 rl.rank 和第1-6个位置的元胞占据
                this.RemoveCell(rl, 1);//(id, rl.parEntity, null);
            }
        }


        private void AddCell(Index point,RoadLane rl)
        {
            Index indexOld =this.getOriginIndex(point,rl.parEntity.ToVector());
            cellMatrix.AddCell(indexOld.X, indexOld.Y, null);//堵塞作用的元胞可以为null
        }
        /// <summary>
        /// 在指定的点添加一个元胞，
        /// </summary>
        internal void AddCell(Cell ca)
        {
            //第Rank个车道为x，0为y
            //三个点全部转化转换为原生坐标
            ca.track.iFromPos = this.getOriginIndex(ca.track.iFromPos, ca.track.fromLane.parEntity.ToVector());
            ca.track.iCurrPos = ca.track.iFromPos;
            ca.track.iToPos = this.getOriginIndex(ca.track.iToPos, ca.track.fromLane.parEntity.ToVector());
             
            cellMatrix.AddCell(ca.track.iCurrPos.X,ca.track.iCurrPos.Y, ca);
        }

        /// <summary>
        /// 按照指定的路段，路段前部的距离进行删除元胞
        /// </summary>
        /// <param name="rl">旋转坐标系所要用到的计算旋转角度的向量</param>
        /// <param name="iAheadSpace">前行距离数</param>
        internal bool RemoveCell(RoadLane rl, int iAheadSpace)
        {
            return this.RemoveCell(rl.ToCenterXY(iAheadSpace), rl.parEntity);
        }
        
        /// <summary>
        /// 删除指定点处的元胞
        /// </summary>
        /// <param name="point">点坐标</param>
        /// <param name="re">旋转坐标系所要用到的计算旋转角度的向量</param>
        internal bool RemoveCell(Index point, RoadEdge re)
        {
            Index indexOrg = this.getOriginIndex(point, re.ToVector());
            return cellMatrix.RemoveCell(indexOrg.X, indexOrg.Y);
        }
     
        /// <summary>
        /// 判断第x个车道前面是否有iAheadSpace个车辆
        /// </summary>
        /// <returns></returns>
        internal bool IsLaneBlocked(RoadLane rl,int iAheadSpace)
        {
            bool isBlocked = false;
            for (int i = 1; i <= iAheadSpace; i++)
            {
                isBlocked = this.IsCellBlocked(rl, i);
                if (isBlocked == true)
                    break;
            }
            return isBlocked;
        }
        internal bool IsLaneBlocked(RoadLane rl)
        {
            return this.IsCellBlocked(rl, 1);
        }
        #endregion

        /// <summary>
        /// 新的roadnode的哈希散列值由其中心Position的哈希值和其ID构成
        /// </summary>
        /// <returns></returns>
        private HashMatrix<Cell> cellMatrix = new HashMatrix<Cell>();
        /// <summary>
        /// 存贮本节点所有出边的哈希表，键值是代表边的RoadEdge哈希，值是代表RoadEdge
        /// </summary>
        private Dictionary<int, RoadEdge> dicEdge = new Dictionary<int,RoadEdge>();

        internal ICollection RoadEdges
        {
            get
            {
                return this.dicEdge.Values;
            }
        }
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
                    if (roadEdge.from !=this)
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
            dicEdge.Remove(RoadEdge.GetHashCode(this,re.to));
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
        /// <summary>
        /// 控制RoadNodeID的数量
        /// </summary>
        private static int iRoadNodeID;
        [System.Obsolete("使用随机数模拟了Position属性。以后应当加以改进")]
        internal RoadNode() 
        {
            this.ID = ++iRoadNodeID;
            Random rd = new Random();
            this.Postion = new MyPoint(rd.Next(65535), rd.Next(65535));
            /// 直接使用上下文的数据结构,bug不应当使用上下文结构
            if (this.Postion.X == 0.0f && this.Postion.Y == 0.0f)
            {
                throw new Exception("RoadNode产生了零坐标！");
            }
        }
        
        public override int GetHashCode()
        {
            int iHash = this.Postion.GetHashCode() +this.ID.GetHashCode();
            return iHash.GetHashCode();
        }
        internal override void UpdateStatus()
        {
            //更新异步消息
            for (int i = 0; i < this.asynAgents.Count; i++)
            {
                Agents.Agent visitorAgent = this.asynAgents[i];
                visitorAgent.VisitUpdate(this);//.VisitUpdate();
            }
            ////更新同步消息
            //foreach (UpdateAgent.UpdateAgent item in this.synAgentChain)
            //{
            //    if (item != null)
            //    {
            //        item.Update();//visitor.visit 一个规则就是一个访问者，很多的访问者
            //    }
            //}
        }
        
        /// <summary>
        /// 提供对哈希矩阵内部元素的遍历
        /// </summary>
        /// <returns></returns>
        internal IEnumerator<Cell> GetEnumerator()
        {
            return this.cellMatrix.GetEnumerator();
        }
    }
}
 
