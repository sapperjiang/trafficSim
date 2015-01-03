using System;
using SubSys_SimDriving;
using SubSys_SimDriving.SysSimDrivingContext;
using System.Collections.Generic;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.TrafficModel
{
    public class RoadNetWork:TrafficEntity,IRoadNetWork
	{
        /// <summary>
        /// ���ֵ�ʹ�÷���������
        /// </summary>
        private Dictionary<int, RoadEdge> dicRoadEdge;// = new Dictionary<int, RoadEdge>();

        /// <summary>
        /// �������ڽӱ�����Ľڵ��ֵ�ʹ�÷��������ģ��߲�ʹ�ýڵ��ڲ������ֵ�
        /// </summary>
        private AdjacencyTable<int> adlistNetWork;
        
        EntityIDManager<int> roadIDManager = new IntIDManager();

        public RoadNetWork() 
        {
            //ʹ�������Ĺ�ϣ��
            dicRoadEdge = this.SimDrivingContext._roadEdgeHashTable;
            adlistNetWork= new AdjacencyTable<int>(this.SimDrivingContext._roadNodeHashTable);
        }

        private MyPoint _netWorkPosition;

        private EntityType _entityType;
      
        //#region ITrafficEntity ��Ա
        //public EntityType EntityType
        //{
        //    get
        //    {
        //        return this._entityType;
        //    }
        //    set
        //    {
        //        this._entityType = value;
        //    }
        //}

    

        //public EntityStatus EntityStatus
        //{
        //    get
        //    {
        //        throw new System.NotImplementedException();
        //    }
        //    set
        //    {
        //        throw new System.NotImplementedException();
        //    }
        //}

        //public MyPoint Postion
        //{
        //    get
        //    {
        //        return this._netWorkPosition;
        //    }
        //    set
        //    {
        //        this._netWorkPosition = value;
        //    }
        //}

            //#endregion

        #region INetWork ��Ա
         public void AddRoadNode(RoadNode value)
        {
            if (value!=null)
            {
                adlistNetWork.AddRoadNode(value.GetHashCode(), value);
            }
        }
         public void RemoveRoadNode(RoadNode value)
        {
            if (value != null)
            {
                adlistNetWork.RemoveRoadNode(value.GetHashCode());
            }
            else 
            {
                throw new ArgumentNullException();
            }
        }
         public void AddRoadEdge(RoadNode fromRoadNode, RoadNode ToRoadNode)
        {
            if (fromRoadNode != null && ToRoadNode != null)
            {
                if (this.FindRoadNode(fromRoadNode) != null && this.FindRoadNode(ToRoadNode) != null)
                {
                    //������
                    RoadEdge re = new RoadEdge(fromRoadNode, ToRoadNode);
                    //��RoadEdge��ӵ�����������han�ֵ�
                    this.dicRoadEdge.Add(re.GetHashCode(), re);
                    //������ӵ�����ڽӾ���������
                    adlistNetWork.AddDirectedEdge(fromRoadNode.GetHashCode(), re);
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
         public void AddRoadEdge(RoadEdge re)
    {
        if (re.rnFrom == null || re.rnTo != null)
        {
            if (this.FindRoadNode(re.rnFrom) != null && this.FindRoadNode(re.rnFrom) != null)
            {
                //��RoadEdge��ӵ��ֵ�
                this.dicRoadEdge.Add(re.GetHashCode(), re);
                //������ӵ�����ڽӾ���������
                adlistNetWork.AddDirectedEdge(re.rnFrom.GetHashCode(), re);
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
         public void RemoveEdge(RoadNode fromRoadNode, RoadNode ToRoadNode)
        {
            if (fromRoadNode != null && ToRoadNode != null)
            {
                RoadEdge re = new RoadEdge(fromRoadNode, ToRoadNode);
                //�ڽӾ�����ɾ����
                adlistNetWork.RemoveDirectedEdge(fromRoadNode.GetHashCode(), new RoadEdge(fromRoadNode, ToRoadNode));
                //������·���ֵ���ɾ����
                this.dicRoadEdge.Remove(re.GetHashCode());
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
                return fromRN.FindRoadEdge(to);
            }
            return null;
        }
        else
        {
            throw new ArgumentNullException();
        }
    }
         public RoadNode FindRoadNode(RoadNode roadNode)
    {
        if (roadNode ==null)
        {
            throw new ArgumentNullException("��������ΪNull");
        }
        if(adlistNetWork.Contains(roadNode.GetHashCode()))
        {
            return adlistNetWork.Find(roadNode.GetHashCode());
        }
        return null;
    }
         public int RoadNodeCount
        {
            get { return adlistNetWork.RoadNodeCount; }
        }
         public int RoadEdgeCount
        {
            get { return dicRoadEdge.Count;}
        }

         #endregion

    }
	 
}
 
