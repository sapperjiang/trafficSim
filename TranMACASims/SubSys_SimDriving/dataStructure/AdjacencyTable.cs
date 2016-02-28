/************************************************************************/
/* AdjacencyList<T>类使用泛型实现了图的邻接表存储结构。它包含两个内部类，
 * Vertex<Tvalue>类（109～118行代码）用于表示一个表头结点，
 * Node类（99～107）则用于表示表结点，其中存放着邻接点信息，用来表示表头结点的某条边。
 * 多个Node用next指针相连形成一个单链表，表头指针为Vertex类的firstEdge成员，
 * 表头结点所代表的顶点的所有边的信息均包含在链表内，其结构如图8.12所示。所不同之处在于：
 * Vertex类中包含了一个visited成员，它的作用是在图遍历时标识当前节点是否被访问过，
 * 这一点在稍后会讲到。邻接点指针域adjvex直接指向某个表头结点，而不是表头结点在数组中的索引。
 * AdjacencyList<T>类中使用了一个泛型List代替数组来保存表头结点信息（第5行代码），
 * 从而不再考虑数组存储空间不够的情况发生，简化了操作。
 * 由于一条无向边的信息需要在边的两个顶点分别存储信息，即添加两个有向边
 * 所以58～78行代码的私有方法AddDirectedEdge()方法用于添加一个有向边。
 * 新的邻接点信息即可以添加到链表的头部也可以添加到尾部，添加到链表头部可以简化操作，
 * 但考虑到要检查是否添加了重复边，需要遍历整个链表，所以最终把邻接点信息添加到链表尾部。                                                                     */
/************************************************************************/


using System;
using System.Collections.Generic;

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// 程序的GUI可能要求使用对象的坐标来查询路段顶点的位置，采用对象的position用作
    /// 哈希值可以快速检索对象，也比使用list泛型高效,负责保存节点到仿真上下文中，
    /// 不负责保存边到上下文中，边由network保存
    /// </summary>
    internal class AdjacencyTable<T>
    {
        Dictionary<T,XNode> dicRoadNode; //图的顶点集合
        /// <summary>
        /// 使用SimDrivingContext 仿真上下文初始化保存路段节点的字典
        /// </summary>
        internal AdjacencyTable(Dictionary<T,XNode> dic)
        {
            dicRoadNode = dic;
        } //构造方法
        private AdjacencyTable() //私有构造防止外部初始化指定容量的构造方法
        { }

        internal void AddRoadNode(T key,XNode value) /*添加?个顶点 */
        {   //不允许插入重复值
            if (Contains(key))//哈希值一致就认为顶点一致
            {
                throw new ArgumentException("插入了重复顶点！");
            }
            dicRoadNode.Add(key,value);
        }
        internal void RemoveRoadNode(T key)
        {
            dicRoadNode.Remove(key);
        }

        internal int RoadNodeCount
        {
            get { return dicRoadNode.Count; }
        }

        internal int RoadEdgeCount
        {
            get
            {
                int iCount = 0;
                foreach (XNode item in dicRoadNode.Values)
                {
                    iCount += item.Ways.Count;
                }
                return iCount;
            }
        }
   
        internal bool Contains(T key) //查找图中是否包含某RoadNode,key是根据对象位置进行的哈希散列值
        {
            if (key != null)
            {
                return dicRoadNode.ContainsKey(key);
            }
            return false; 
        }
        internal XNode Find(T key) //查找指定项并返回
        {
            if (key != null)
            {
                if(!dicRoadNode.ContainsKey(key)){
                throw new Exception("无法找到没有添加的RoadNode节点");
                }
                return dicRoadNode[key] as XNode;
            }
            return null;
        }
       
        /// <summary>
        /// 添加有向边
        /// </summary>
        /// <param name="fromRoadNodeHash">要将将边添加到RoadNode哈希表中的RoadNode</param>
        /// <param name="Edge">要添加的边</param>
        internal void AddDirectedEdge(T fromXNodeHash,Way way)
        {
            XNode rn= this.Find(fromXNodeHash);
            if(rn!=null)
            {
                rn.AddWay(way);
            }
        }
        internal void RemoveDirectedEdge(T roadNodeHash, Way edge)
        {
            XNode rn = this.Find(roadNodeHash);
            if (rn != null)
            {
                rn.RemoveWay(edge);
            }
        }
        //internal override string ToString() //仅用于测试
        //{   //打印每个节点和它的邻接点
        //    string s = string.Empty;
        //    foreach (RoadNode<T> v in items)
        //    {
        //        s += v.data.ToString() + ":";
        //        if (v.firstEdge != null)
        //        {
        //            Node tmp = v.firstEdge;
        //            while (tmp != null)
        //            {
        //                s += tmp.adjvex.data.ToString();
        //                tmp = tmp.next;
        //            }
        //        }
        //        s += "\r\n";
        //    }
        //    return s;
        //}
        //嵌套类，表示链表中的表结点
      
      //internal RoadNode<T> RoadNode
    }

   
}