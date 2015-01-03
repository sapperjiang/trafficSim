using System;
using SubSys_SimDriving;
using SubSys_SimDriving.SysSimContext;
using System.Collections.Generic;
using SubSys_SimDriving.TrafficModel;
using SubSys_MathUtility;

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// Ӧ��ʵ��Ϊ����ģʽ,RoadNetWork ��simContext��һ����
    /// RoadNetWork Ӧ���ге�·���ڵ㹤��������
    /// </summary>
    public class RoadNetWork:TrafficEntity,IRoadNetWork
	{
        public static int iRoadNetWorkCount = 0;
        /// <summary>
        ///����ģʽ ��ֱֹ�ӵ��ýӿ����ɸ���,·���ı�ʹ����simContext
        ///·���Ľڵ��ʹ����simContext
        /// </summary>
        private RoadNetWork()
        {
            iRoadNetWorkCount += 1;
            ///�ڽӾ���ʹ�õĽڵ�ʹ���ⲿRoadNodeList��Ϊ�洢����
            ADNetWork = new AdjacencyTable<int>(this.RoadNodeList);
        }
        /// <summary>
        /// ��̬����˽�����ã�ֻ��ͨ��getInstance�������ʵ��
        /// </summary>
        private static RoadNetWork _roadNetWork;
        public static RoadNetWork GetInstance()
        {
            if (_roadNetWork == null)
            {
                System.Threading.Mutex mutext = new System.Threading.Mutex();
                mutext.WaitOne();
                _roadNetWork = new RoadNetWork();
                _roadNetWork.EntityType = EntityType.RoadNetWork;
                mutext.Close();
                mutext = null;
            }
            return RoadNetWork._roadNetWork;
        }
        /// <summary>
        /// ���ֵ�ʹ�÷���������
        /// </summary>
        internal RoadEdgeHTable RoadEdgeList = new RoadEdgeHTable();

        internal RoadNodeHTable RoadNodeList = new RoadNodeHTable();

        /// <summary>
        /// ��ȡ���еĳ����Ƿ��б�Ҫ����Ϊ�ò����Ѿ�������RoadEdge����
        /// </summary>
        internal RoadLaneHTable RoadLanes = new RoadLaneHTable();


        /// <summary>
        /// �������ڽӱ�����Ľڵ��ֵ�ʹ�÷��������ģ��߲�ʹ�ýڵ��ڲ������ֵ�
        /// </summary>
        private AdjacencyTable<int> ADNetWork;
        
        EntityIDManager<int> roadIDManager = new IntIDManager();

        private MyPoint _netWorkPos;

        private EntityType _entityType;
 
        #region INetWork ��Ա
        public ICollection<RoadEdge> RoadEdges 
        {
            get {
                return this.RoadEdgeList.Values;
            }
        }
        public ICollection<RoadNode> RoadNodes
        {
            get{
                return this.RoadNodeList.Values;
            }
        }
         
        public  void AddRoadNode(RoadNode value)
        {
            if (value!=null)
            {
               
                ADNetWork.AddRoadNode(value.GetHashCode(), value);
                value.Register();////ע�ᵽ·�����������캯��
            }
        }
         public void RemoveRoadNode(RoadNode value)
        {
            if (value != null)
            {
                ADNetWork.RemoveRoadNode(value.GetHashCode());//�Ѿ�ɾ���˽ڵ�
                value.UnRegiser();//�ظ�ɾ��
            }
            else 
            {
                throw new ArgumentNullException();
            }
        }
          public RoadNode FindRoadNode(RoadNode roadNode)
         {
             int i = this.RoadNodes.Count;
             //bool b = object.ReferenceEquals(this, SimCtx.NetWork);
             //bool c = object.ReferenceEquals(this.ADNetWork, SimCtx.NetWork.ADNetWork);

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
 
        ///// <summary>
        ///// ����ڹ��캯������ע�ᣬ�ͻ����·����ʹ�õ�RoadNode��ע���roadNode������ƥ�������
        ///// </summary>
        ///// <param name="fromRoadNode"></param>
        ///// <param name="ToRoadNode"></param>
        // void IRoadNetWork.AddRoadEdge(RoadNode fromRoadNode, RoadNode ToRoadNode)
        //{
        //    if (fromRoadNode != null && ToRoadNode != null)
        //    {
        //        if ((this as IRoadNetWork).FindRoadNode(fromRoadNode) != null && (this as IRoadNetWork).FindRoadNode(ToRoadNode) != null)
        //        {
        //            //������
        //            RoadEdge re = new RoadEdge(fromRoadNode, ToRoadNode);
        //            re.Register();//��������ĳ�ʼ������
        //            //��RoadEdge��ӵ�����������han�ֵ�
        //            this.RoadEdgeList.Add(re.GetHashCode(), re);

        //            //������ӵ�����ڽӾ���������
        //            ADNetWork.AddDirectedEdge(fromRoadNode.GetHashCode(), re);
        //        }
        //        else
        //        {
        //            throw new ArgumentException("û������������Ӵ�����·�ߵĽڵ�");
        //        }
        //    }
        //    else
        //    {
        //        throw new ArgumentNullException("�޷��ÿսڵ���ӱ�");
        //    }
        //}
        public void AddRoadEdge(RoadEdge re)
        {
            if (this.FindRoadNode(re.roadNodeFrom) != null && this.FindRoadNode(re.roadNodeFrom) != null)
            {
                re.Register();//����·��ע��
                //������ӵ�����ڽӾ���������
                ADNetWork.AddDirectedEdge(re.roadNodeFrom.GetHashCode(), re);
            }
            else
            {
                ThrowHelper.ThrowArgumentException("û������������Ӵ�����·�ߵĽڵ㣬�ڵ�û��ע��");
            }
        }
        public RoadEdge AddRoadEdge(RoadNode from, RoadNode To)
        {
            RoadEdge re = new RoadEdge(from, To);
            this.AddRoadEdge(re);
            return re;
        }
        [System.Obsolete("���ɾ����������������")]
        public void RemoveRoadEdge(RoadNode from, RoadNode to)
        {
            if (from != null && to != null)
            {
                RoadEdge re = this.FindRoadEdge(from,to);
                //�ڽӾ�����ɾ����
                ADNetWork.RemoveDirectedEdge(from.GetHashCode(), re);
                re.UnRegiser();//���ע��
            }
        }
         
        public RoadEdge FindRoadEdge(RoadNode from, RoadNode to)
        {
            if (from != null && to != null)
            {
                //�ҵ��ڲ��Ĺ�ϣ���Ӧ�ĸýڵ�
                RoadNode fromRN = this.FindRoadNode(from);
                if (fromRN != null)
                {//��ѯ�û�������
                    //System.Diagnostics.Debug.Assert(fromRN.FindRoadEdge(to) != this.RoadEdgeList[RoadEdge.GetHashCode(from, to)]);
                    return fromRN.FindRoadEdge(to);
                }
                return null;
            }
            throw new ArgumentNullException("��������Ϊ��");
        }
   
         #endregion

        public event UpdateHandler UpdateCompleted;


        private int _iCurrTimeStep;
        public int iCurrTimeStep
        {
            get
            {
                //throw new NotImplementedException();
                return this._iCurrTimeStep;
            }
            set
            {
                this._iCurrTimeStep = value;

                this.OnUpdateCompleted();//����ί��
            }
        }
        private void OnUpdateCompleted()
        {
            foreach (var handler in handlerList)
            {
                handler();//����ί�еķ���
            }
        }

        private List<UpdateHandler> handlerList;

        event UpdateHandler IRoadNetWork.UpdateCompleted
        {
            add 
            {
                if (handlerList == null)
                {
                    handlerList = new List<UpdateHandler>();
                }
                handlerList.Add(value); 
            }
            remove 
            {
                handlerList.Remove(value); 
                //throw new NotImplementedException(); 
            }
        }


        ICollection<RoadLane> IRoadNetWork.RoadLanes
        {
            get { return this.RoadLanes.Values; }
        }


        public RoadEdge FindRoadEdge(int reKey)
        {
            RoadEdge re ;
            this.RoadEdgeList.TryGetValue(reKey, out re);
            return re;
        }
    }
	 
}
 
