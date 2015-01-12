
using System;
using System.Collections;
using System.Collections.Generic;

namespace SubSys_SimDriving.TrafficModel
{
	/// <summary>
	/// 用来表示道路一条边的数据结构
	/// </summary>
       internal class Edge
    {
        internal string StartNodeID ;
        internal string EndNodeID   ;
        internal double Weight      ; //权值，代价        
    }
   
    /// <summary>
    ///节点则抽象成Node类，一个节点上挂着以此节点作为起点的“出边”表。
    /// </summary>
    internal class GridNode
    {
        private string iD ;
        private ArrayList _lsEdges ;//Edge的集合－－出边表
        internal GridNode(string id )
     {
            this.iD = id ;
            this._lsEdges = new ArrayList() ;
        }
        
        #region property
        internal string ID
     {
            get
          {
                return this.iD ;
            }
        }
        internal ArrayList EdgeList
      {
            get
          {
                return this._lsEdges ;
            }
        }
        #endregion
    }
 

    //在计算的过程中，我们需要记录到达每一个节点权值最小的路径，这个抽象可以用PassedPath类来表示：
    /// <summary>
    /// PassedPath 用于缓存计算过程中的到达某个节点的权值最小的路径
    /// </summary>
    internal class PassedPath
    {
        private string     _currNodeID ;
        private bool     _bProcessed ;   //是否已被处理
        private double     _dWeight ;        //累积的权值
        private ArrayList _lsPassedIDs ; //路径

        internal PassedPath(string ID)
        {
            this._currNodeID = ID ;
            this._dWeight    = double.MaxValue ;
            this._lsPassedIDs = new ArrayList() ;
            this._bProcessed = false ;
        }

        #region property
        internal bool IsProcessed
        {
            get
            {
                return this._bProcessed ;
            }
            set
            {
                this._bProcessed = value ;
            }
        }

        internal string CurNodeID
        {
            get
            {
                return this._currNodeID ;
            }
        }

        internal double Weight 
        {
            get
            {
                return this._dWeight ;
            }
            set
            {
                this._dWeight = value ;
            }
        }

        internal ArrayList PassedIDList
        {
            get
            {
                return this._lsPassedIDs ;
            }
        }
        #endregion
    }

    //另外，还需要一个表PlanCourse来记录规划的中间结果，即它管理了每一个节点的PassedPath。
 

    /// <summary>
    /// PlanCourse 缓存从源节点到其它任一节点的最小权值路径＝》路径表
    /// </summary>
    internal class PlanCourse
    {
        private Hashtable _htPassedPath ;    

        #region ctor
        internal PlanCourse(ArrayList nodeList ,string originID)
        {
            this._htPassedPath = new Hashtable() ;

            GridNode originNode = null ;
            foreach(GridNode node in nodeList)
            {
                if(node.ID == originID)
                {
                    originNode = node ;
                }
                else
                {
                    PassedPath pPath = new PassedPath(node.ID) ;
                    this._htPassedPath.Add(node.ID ,pPath) ;
                }
            }

            if(originNode == null) 
            {
                throw new Exception("The origin node is not exist !") ;
            }        
    
            this.InitializeWeight(originNode) ;
        }

        private void InitializeWeight(GridNode originNode)
        {
            if((originNode.EdgeList == null) ||(originNode.EdgeList.Count == 0))
            {
                return ;
            }

            foreach(Edge edge in originNode.EdgeList)
            {
                PassedPath pPath = this[edge.EndNodeID] ;
                if(pPath == null)
                {
                    continue ;
                }

                pPath.PassedIDList.Add(originNode.ID) ;
                pPath.Weight = edge.Weight ;
            }
        }
        #endregion

        internal PassedPath this[string nodeID]
        {
            get
            {
                return (PassedPath)this._htPassedPath[nodeID] ;
            }
        }
    }

//    在所有的基础构建好后，路径规划算法就很容易实施了，该算法主要步骤如下：
//(1)用一张表(PlanCourse)记录源点到任何其它一节点的最小权值，初始化这张表时，如果源点能直通某节点，则权值设为对应的边的权，否则设为double.MaxValue。
//(2)选取没有被处理并且当前累积权值最小的节点TargetNode，用其边的可达性来更新到达其它节点的路径和权值（如果其它节点   经此节点后权值变小则更新，否则不更新），然后标记TargetNode为已处理。
//(3)重复(2)，直至所有的可达节点都被处理一遍。
//(4)从PlanCourse表中获取目的点的PassedPath，即为结果。
    
//    下面就来看上述步骤的实现，该实现被封装在RoutePlanner类中：
 
    /// <summary>
    /// RoutePlanner 提供图算法中常用的路径规划功能。
    /// 2005.09.06
    /// </summary>
    internal class RoutePlanner
    {
        internal RoutePlanner()
        {            
        }

        #region Paln
        //获取权值最小的路径
        //internal RoutePlanResult Paln(ArrayList nodeList ,string originID ,string destID)
        //{
        //    PlanCourse planCourse = new PlanCourse(nodeList ,originID) ;

        //    Node curNode = this.GetMinWeightRudeNode(planCourse ,nodeList ,originID) ;

        //    #region 计算过程
        //    while(curNode != null)
        //    {
        //        PassedPath curPath = planCourse[curNode.ID] ;
        //        foreach(Edge edge in curNode.EdgeList)
        //        {
        //            PassedPath targetPath = planCourse[edge.EndNodeID] ;
        //            double tempWeight = curPath.Weight + edge.Weight ;

        //            if(tempWeight < targetPath.Weight)
        //            {
        //                targetPath.Weight = tempWeight ;
        //                targetPath.PassedIDList.Clear() ;

        //                for(int i=0 ;i<curPath.PassedIDList.Count ;i++)
        //                {
        //                    targetPath.PassedIDList.Add(curPath.PassedIDList[i].ToString()) ;
        //                }

        //                targetPath.PassedIDList.Add(curNode.ID) ;
        //            }
        //        }

        //        //标志为已处理
        //        planCourse[curNode.ID].BeProcessed = true ;
        //        //获取下一个未处理节点
        //        curNode = this.GetMinWeightRudeNode(planCourse ,nodeList ,originID) ;
        //    }
        //    #endregion
            
        //    //表示规划结束
        //    return this.GetResult(planCourse ,destID) ;                
        //}
        #endregion

        #region GetResult
        //从PlanCourse表中取出目标节点的PassedPath，这个PassedPath即是规划结果
        private void GetResult(PlanCourse planCourse ,string destID)
        {
            PassedPath pPath = planCourse[destID]  ;            

            if(float.Equals( pPath.Weight,float.MaxValue))
            {
                //RoutePlanResult result1 = new RoutePlanResult(null ,int.MaxValue) ;
                //return result1 ;
            }
            
            string[] passedNodeIDs = new string[pPath.PassedIDList.Count] ;
            for(int i=0 ;i<passedNodeIDs.Length ;i++)
            {
                passedNodeIDs[i] = pPath.PassedIDList[i].ToString() ;
            }
            //RoutePlanResult result = new RoutePlanResult(passedNodeIDs ,pPath.Weight) ;

            //return result ;            
        }
        #endregion

        #region GetMinWeightRudeNode
        //从PlanCourse取出一个当前累积权值最小，并且没有被处理过的节点
        private GridNode GetMinWeightRudeNode(PlanCourse planCourse ,ArrayList nodeList ,string originID)
        {
            double weight = double.MaxValue ;
            GridNode destNode = null ;

            foreach(GridNode node in nodeList)
            {
                if(node.ID == originID)
                {
                    continue ;
                }

                PassedPath pPath = planCourse[node.ID] ;
                if(pPath.IsProcessed)
                {
                    continue ;
                }

                if(pPath.Weight < weight)
                {
                    weight = pPath.Weight ;
                    destNode = node ;
                }
            }

            return destNode ;
        }
        #endregion
    }
}