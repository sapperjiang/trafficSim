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
using System.Collections;
using System.Collections.Generic;

namespace SubSys_SimDriving.TrafficModel
{
    internal class IntIDManager:EntityIDManager<int>
    {

        internal  IntIDManager() 
        {
            this.templateMaxID = 0;
        }
        
        internal override int GetUniqueRoadNodeID()
        { 
            this.templateMaxID ++;
            this.listIDContainer.Add(this.templateMaxID);
            return this.templateMaxID;
        }
    }
}