
using System;
using System.Collections.Generic;

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// 程序的GUI可能要求使用对象的坐标来查询路段顶点的位置，采用对象的position用作
    /// 哈希值可以快速检索对象，也比使用list泛型高效,负责保存节点到仿真上下文中，
    /// 不负责保存边到上下文中，边由network保存
    /// </summary>
    /// <typeparam name="T">int</typeparam>
    public class AdjacencyTable<T>
    {
        Dictionary<T,RoadNode> dicRoadNode; //图的顶点集合
        /// <summary>
        /// 使用SimDrivingContext 仿真上下文初始化保存路段节点的字典
        /// </summary>
        /// <param name="dic"></param>
        public AdjacencyTable(Dictionary<T,RoadNode> dic)
        {
            dicRoadNode = dic;
        } //构造方法
        private AdjacencyTable() //私有构造防止外部初始化指定容量的构造方法
        { }

        public void AddRoadNode(T key,RoadNode value) /*添加?个顶点 */
        {   //不允许插入重复值
            if (Contains(key))//哈希值一致就认为顶点一致
            {
                throw new ArgumentException("插入了重复顶点！");
            }
            dicRoadNode.Add(key,value);
        }
        public void RemoveRoadNode(T key)
        {
            dicRoadNode.Remove(key);
        }

        public int RoadNodeCount
        {
            get { return dicRoadNode.Count; }
        }

        public int RoadEdgeCount
        {
            get
            {
                int iCount = 0;
                foreach (RoadNode item in dicRoadNode.Values)
                {
                    iCount += item.RoadEdgeCount;
                }
                return iCount;
            }
        }
   
        public bool Contains(T key) //查找图中是否包含某RoadNode,key是根据对象位置进行的哈希散列值
        {
            if (key != null)
            {
                return dicRoadNode.ContainsKey(key);
            }
            return false; 
        }
        public RoadNode Find(T key) //查找指定项并返回
        {
            if (key != null)
            {
                if(!dicRoadNode.ContainsKey(key)){
                throw new Exception("无法找到没有添加的RoadNode节点");
                }
                return dicRoadNode[key] as RoadNode;
            }
            return null;
        }
       
        /// <summary>
        /// 添加有向边
        /// </summary>
        /// <param name="fromRoadNodeHash">要将将边添加到RoadNode哈希表中的RoadNode</param>
        /// <param name="Edge">要添加的边</param>
        public void AddDirectedEdge(T fromRoadNodeHash,RoadEdge Edge)
        {
            RoadNode rn= this.Find(fromRoadNodeHash);
            if(rn!=null)
            {
                rn.AddRoadEdge(Edge);
            }
        }
        public void RemoveDirectedEdge(T roadNodeHash, RoadEdge edge)
        {
            RoadNode rn = this.Find(roadNodeHash);
            if (rn != null)
            {
                rn.RemoveEdge(edge.GetHashCode());
            }
        }
        //public override string ToString() //仅用于测试
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
      
      //public RoadNode<T> RoadNode
    }

   
}