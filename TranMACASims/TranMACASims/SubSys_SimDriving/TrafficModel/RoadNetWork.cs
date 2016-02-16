using System;
using SubSys_SimDriving;
using SubSys_SimDriving.SysSimContext;
using System.Collections;
using System.Collections.Generic;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// Ӧ��ʵ��Ϊ����ģʽ,RoadNetWork ��simContext��һ����
    /// </summary>
    internal class RoadNetWork:TrafficEntity,IRoadNetWork
	{
        /// <summary>
        ///����ģʽ ��ֱֹ�ӵ��ýӿ����ɸ���,·���ı�ʹ����simContext
        ///·���Ľڵ��ʹ����simContext
        /// </summary>
        private RoadNetWork() 
        {
            ///�ڽӾ���ʹ�õĽڵ�ʹ���ⲿRoadNodeList��Ϊ�洢����
            ADNetWork = new AdjacencyTable<int>(this.RoadNodeList);
            //k = new AdjacencyTable<int>(this.SimDrivingContext.RoadNodeList);
        }
        /// <summary>
        /// ��̬����˽�����ã�ֻ��ͨ��getInstance�������ʵ��
        /// </summary>
        private static RoadNetWork _roadNetWork;
        internal static RoadNetWork GetInstance()
        {
            if (_roadNetWork == null)
            {
                System.Threading.Mutex mutext = new System.Threading.Mutex();
                mutext.WaitOne();
                _roadNetWork = new RoadNetWork();
                mutext.Close();
             //   mutext.Dispose();
                mutext = null;
            }
            return _roadNetWork;
        }
        /// <summary>
        /// ���ֵ�ʹ�÷���������
        /// </summary>
        private RoadEdgeHashTable RoadEdgeList = new RoadEdgeHashTable();

        private RoadNodeHashTable RoadNodeList = new RoadNodeHashTable();
        /// <summary>
        /// �������ڽӱ�����Ľڵ��ֵ�ʹ�÷��������ģ��߲�ʹ�ýڵ��ڲ������ֵ�
        /// </summary>
        private AdjacencyTable<int> ADNetWork;
        
        EntityIDManager<int> roadIDManager = new IntIDManager();

        private MyPoint _netWorkPosition;

        private EntityType _entityType;

        //RoadNode FindRoadNode(
        #region INetWork ��Ա
        ICollection IRoadNetWork.RoadEdges 
        {
            get {
                return this.RoadEdgeList.Values;
            }
        }
         ICollection IRoadNetWork.RoadNodes
        {
            get{
                return this.RoadNodeList.Values;
            }
        }
         
         void IRoadNetWork.AddRoadNode(RoadNode value)
        {
            if (value!=null)
            {
                ADNetWork.AddRoadNode(value.GetHashCode(), value);
            }
        }
         void IRoadNetWork.RemoveRoadNode(RoadNode value)
        {
            if (value != null)
            {
                ADNetWork.RemoveRoadNode(value.GetHashCode());
            }
            else 
            {
                throw new ArgumentNullException();
            }
        }
         RoadNode IRoadNetWork.FindRoadNode(RoadNode roadNode)
         {
             if (roadNode == null)
             {
                 throw new ArgumentNullException("��������ΪNull");
             }
             if (ADNetWork.Contains(roadNode.GetHashCode()))
             {
                 return ADNetWork.Find(roadNode.GetHashCode());
             }
             return null;
         }
 

         void IRoadNetWork.AddRoadEdge(RoadNode fromRoadNode, RoadNode ToRoadNode)
        {
            if (fromRoadNode != null && ToRoadNode != null)
            {
                if ((this as IRoadNetWork).FindRoadNode(fromRoadNode) != null && (this as IRoadNetWork).FindRoadNode(ToRoadNode) != null)
                {
                    //������
                    RoadEdge re = new RoadEdge(fromRoadNode, ToRoadNode);
                    //��RoadEdge��ӵ�����������han�ֵ�
                    this.RoadEdgeList.Add(re.GetHashCode(), re);
                    //������ӵ�����ڽӾ���������
                    ADNetWork.AddDirectedEdge(fromRoadNode.GetHashCode(), re);
                }
                else
                {
                    throw new ArgumentException("û������������Ӵ�����·�ߵĽڵ�");
                }
            }
            else
            {
                throw new ArgumentNullException("�޷��ÿսڵ���ӱ�");
            }
        }
         void IRoadNetWork.AddRoadEdge(RoadEdge re)
    {
        if (re.from == null || re.to != null)
        {
            if ((this as IRoadNetWork).FindRoadNode(re.from) != null && (this as IRoadNetWork).FindRoadNode(re.from) != null)
            {
                //��RoadEdge��ӵ��ֵ�
                this.RoadEdgeList.Add(re.GetHashCode(), re);
                //������ӵ�����ڽӾ���������
                ADNetWork.AddDirectedEdge(re.from.GetHashCode(), re);
            }
            else
            {
                throw new ArgumentException("û������������Ӵ�����·�ߵĽڵ�");
            }

        }
        else
        {
            throw new ArgumentNullException("�޷��ÿսڵ���ӱ�");
        }
    }
         void IRoadNetWork.RemoveRoadEdge(RoadNode fromRoadNode, RoadNode ToRoadNode)
        {
            if (fromRoadNode != null && ToRoadNode != null)
            {
                RoadEdge re = new RoadEdge(fromRoadNode, ToRoadNode);
                //�ڽӾ�����ɾ����
                ADNetWork.RemoveDirectedEdge(fromRoadNode.GetHashCode(), new RoadEdge(fromRoadNode, ToRoadNode));
                //������·���ֵ���ɾ����
                this.RoadEdgeList.Remove(re.GetHashCode());
            }
        }
         
         RoadEdge IRoadNetWork.FindRoadEdge(RoadNode from, RoadNode to)
    {
        if (from != null && to != null)
        {
            //�ҵ��ڲ��Ĺ�ϣ���Ӧ�ĸýڵ�
            RoadNode fromRN = (this as IRoadNetWork).FindRoadNode(from);
            if (fromRN != null)
            {//��ѯ�û�������
                return fromRN.FindRoadEdge(to);
            }
            return null;
        }
        else
        {
            throw new ArgumentNullException();
        }
    }
        
         
        int IRoadNetWork.RoadNodeCount
        {
            get { return ADNetWork.RoadNodeCount; }
        }
        int IRoadNetWork.RoadEdgeCount
        {
            get { return RoadEdgeList.Count; }
        }

         #endregion
    }
	 
}
 
