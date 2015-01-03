using System;
using System.Collections;
using System.Collections.Generic;

using SubSys_MathUtility;
using SubSys_SimDriving.Agents;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.TrafficModel;

using System.Drawing;

namespace SubSys_SimDriving.TrafficModel
{
   
    /// <summary>
    /// ʹ�þ������͵Ľṹ��ζ�Ų�֧����·����.��·��֧���д����ۣ���·������֧�ֵ�
    /// </summary>
    public class RoadNode : RoadEntity
    {
        /// <summary>
        /// ·��ת��Ϊ���������,iahead  ��Ӧ��С����
        /// </summary>
        private Point MakeCenterXY(RoadLane rl, int iAhead)
        {
            return new Point(rl.Rank, iAhead - SimSettings.iMaxLanes);
        }


        #region ������������


        /// <summary>
        /// �ж�ָ������ǰ����Ahead��λ�ô��Ƿ���Ԫ��ռ��
        /// </summary>
        internal bool IsBlocked(RoadLane rl, int iAhead)
        {
            Point irltXY = this.MakeCenterXY(rl,iAhead);
            Point iRealXY = Coordinates.GetRealXY(irltXY,rl.ToVector());
            return cells.IsBlocked(iRealXY.X, iRealXY.Y);
        }
        /// <summary>
        /// �ж�ָ������ǰ����Ahead��λ�ô��Ƿ���Ԫ��ռ��
        /// </summary>
        internal bool IsBlocked(Point iRealXY)
        {
            return cells.IsBlocked(iRealXY.X, iRealXY.Y);
        }

     
      
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="rl"></param>
        internal void BlockLane(RoadLane rl)
        {
            if (rl == null)
            {
                throw new ArgumentNullException();
            }
            if (IsBlocked(rl,1)==false)//�յ������
            {
                this.AddCell(rl, 1);
            }
        }
        /// <summary>
        /// ��������ͨ
        /// </summary>
        /// <param name="rl"></param>
        internal void UnblockLane(RoadLane rl)
        {
            if (rl == null)
            {
                throw new ArgumentNullException();
            }
            if (IsBlocked(rl, 1)==true)//�ǿ���ɾ��
            {//��null ��λ�� rl.rank �͵�1-6��λ�õ�Ԫ��ռ��
                this.RemoveCell(rl, 1);//(id, rl.parEntity, null);
            }
        } /// <summary>
        /// �жϵ�x������ǰ���Ƿ���iAheadSpace������
        /// </summary>
        /// <returns></returns>
        internal bool IsLaneBlocked(RoadLane rl, int iAheadSpace)
        {
            bool isBlocked = false;
            for (int i = 1; i <= iAheadSpace; i++)
            {
                isBlocked = this.IsBlocked(rl, i);
                if (isBlocked == true)
                    break;
            }
            return isBlocked;
        }

        internal bool IsLaneBlocked(RoadLane rl)
        {
            return this.IsBlocked(rl, 1);
        }
         #endregion

        #region Ԫ����������
        /// <summary>
        /// Ϊ���̵����׼���ķ���������������Ԫ��
        /// </summary>
        private void AddCell(RoadLane rl, int iAheadSpace)
        {
            Point ipt = this.MakeCenterXY(rl, 1);
            ipt = Coordinates.GetRealXY(ipt, rl.ToVector());
            cells.Add(ipt.X, ipt.Y, null);//�������õ�Ԫ������Ϊnull
        }

        /// <summary>
        /// ��ָ���ĵ����һ��Ԫ����
        /// </summary>
        internal void AddCell(Cell ca)
        {
            ca.Container = this;

            cells.Add(ca.Track.pCurrPos.X,ca.Track.pCurrPos.Y, ca);
        }
        /// <summary>
        /// Ҫ�����������Ǿ�������
        /// </summary>
        /// <param name="iOldPoint"></param>
        /// <param name="iNewPoint"></param>
        /// <returns></returns>
        internal bool MoveCell(Point iOldPoint, Point iNewPoint)
        {
            return cells.Move(iOldPoint, iNewPoint);
        }


        internal bool RemoveCell(Cell ce)
        {
            //ce.Container = nu
            return this.cells.Remove(ce.Track.pCurrPos.X, ce.Track.pCurrPos.Y);
        }

        /// <summary>
        /// ����ָ����·�Σ�·��ǰ���ľ������ɾ��Ԫ��
        /// </summary>
        /// <param name="rl">��ת����ϵ��Ҫ�õ��ļ�����ת�Ƕȵ�����</param>
        /// <param name="iAheadSpace">ǰ�о�����</param>
        internal bool RemoveCell(RoadLane rl, int iAheadSpace)
        {
            Point ipt = this.MakeCenterXY(rl, 1);
            Point iRealIndex = Coordinates.GetRealXY(ipt, rl.ToVector());
            return cells.Remove(iRealIndex.X, iRealIndex.Y);
        }
        #endregion
       

        /// <summary>
        /// �µ�roadnode�Ĺ�ϣɢ��ֵ��������Position�Ĺ�ϣֵ����ID����
        /// </summary>
        /// <returns></returns>
        private HashMatrix<Cell> cells = new HashMatrix<Cell>();
        /// <summary>
        /// �������ڵ����г��ߵĹ�ϣ����ֵ�Ǵ���ߵ�RoadEdge��ϣ��ֵ�Ǵ���RoadEdge
        /// </summary>
        private Dictionary<int, RoadEdge> dicEdge = new Dictionary<int,RoadEdge>();


        internal Cell this[int index]
        {
            get
            {
                return this.cells[index];
            }
        }
        internal ICollection<int> Keys
        {
            get
            {
                return this.cells.Keys;
            }
        }

        /// <summary>
        /// �ṩ�Թ�ϣ�����ڲ�Ԫ�صı���
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Cell> GetEnumerator()
        {
            return this.cells.GetEnumerator();
        }

        public ICollection RoadEdges
        {
            get
            {
                return this.dicEdge.Values;
            }
        }
         #region ���������ڽӾ����нڵ���ߵı߳�Ա,��Ӧ��ʹ��RoadNetwork֮����������Щ��Ա
        /// <summary>
        /// ע���ڳ��߱��У�����roadedge��from�ֶ���this�ڵ㣬�������׳��쳣
        /// </summary>
        /// <param name="roadEdge"></param>
        internal void AddRoadEdge(RoadEdge roadEdge)
        {
            if (roadEdge != null)
            {
                if (!Contains(roadEdge.GetHashCode()))
                {
                    //�����ж��Ƿ��ǵ�ǰ��ĳ��ߵ���Ϣ��ֹ����
                    if (roadEdge.roadNodeFrom !=this)
                    {
                        throw new Exception("����˲����ڸö���ı�");
                    }
                    dicEdge.Add(roadEdge.GetHashCode(), roadEdge);
                }
                else
                {
                    throw new ArgumentException("������ظ��ıߣ�");
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
        /// <summary>
        /// �ҵ��� ��this��toNode�ڵ�ıߣ����߱�
        /// </summary>
        /// <param name="fromRN"></param>
        internal void RemoveEdge(RoadEdge re)
        {
            if (re == null )
            {
                throw new ArgumentNullException();
            }
            dicEdge.Remove(re.GetHashCode());
        }
        internal void RemoveEdge(RoadNode toRN)
        {
            if (toRN == null)
            {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.obj);
            }
            dicEdge.Remove(RoadEdge.GetHashCode(this,toRN));
        }
  
        /// <summary>
        /// ���ҷ������µĽṹ���ó��߱�
        /// </summary>
        /// <param name="toRoadNode">���ڵ�</param>
        /// <returns></returns>
        public RoadEdge FindRoadEdge(RoadNode toRoadNode)
        {
            int iHashkey = RoadEdge.GetHashCode(this,toRoadNode);
            if (dicEdge.ContainsKey(iHashkey))
            {
                return dicEdge[iHashkey];
            }
            return null;
        }
    
        public bool Contains(int EdgeKey)
        {
            return dicEdge.ContainsKey(EdgeKey);
        }

        #endregion
        /// <summary>
        /// ����RoadNodeID������
        /// </summary>
        private static int iRoadNodeID;
        [System.Obsolete("ʹ���в����Ĺ��캯��")]
        internal RoadNode()
        {
            this._id = ++iRoadNodeID;
            Random rd = new Random();

            this.gisPos = new MyPoint(rd.Next(65535), rd.Next(65535));
            /// ֱ��ʹ�������ĵ����ݽṹ,bug��Ӧ��ʹ�������Ľṹ
            if (this.gisPos._X == 0.0f && this.gisPos._Y == 0.0f)
            {
                throw new Exception("RoadNode�����������꣡");
            }
        }
        internal RoadNode(Point rltPostion)
        {
//        	this.na
            this._id = ++iRoadNodeID;
            Random rd = new Random();
            this.RelativePosition = rltPostion;
            this.gisPos = new MyPoint(rd.Next(65535), rd.Next(65535));
        }
        
        public override int GetHashCode()
        {
            int iHash = this.gisPos.GetHashCode() +this.ID.GetHashCode();
            return iHash.GetHashCode();
        }
        /// <summary>
        /// ����agent������Ԫ������ʻ�������÷���
        /// </summary>
        public override void UpdateStatus()
        {
            //�����첽agent������еĻ�
            for (int i = 0; i < this.asynAgents.Count; i++)
            {
                Agent visitor = this.asynAgents[i];
                visitor.VisitUpdate(this);//.VisitUpdate();
            }
            
            //ͨ����������һ�����Ͻ���
            foreach (var item in new List<int>(this.Keys))
            {
                this[item].Drive(this);
            }

            //foreach (int i = 0; i < this.Keys.Count; i++)
            //{
            //    this[this.Keys[i]].Drive(this);
            //}
            base.UpdateStatus();//���������OnStatusChanged ���л�ͼ����
        }

        protected override void OnStatusChanged()
        {
            this.InvokeServices(this);
        }
    }
}
 
