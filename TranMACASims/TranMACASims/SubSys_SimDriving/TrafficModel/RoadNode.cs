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
        /// �����������������Զ����RoadNode��positon
        /// </summary>
        internal readonly int iMaxWidth = SimContext.SimSettings.iMaxWidth;

        internal class CellHashKey
        {
            //���ã�x,y�������´洢�ṹ�Ĺ�ϣֵ����֧������x.y���ٷ��ʾ���Ԫ��
            internal static int GetHashCode(int ix, int iy)
            {
                return (ix.GetHashCode() + iy.GetHashCode()).GetHashCode();
            }
        }
       
        private Dictionary<int,T> hashMat = new Dictionary<int,T>();
        /// <summary>
        /// �ж�Ԫ���Ƿ�ռ����
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        internal bool IsCellBlocked(int x, int y)
        {
            return hashMat.ContainsKey(CellHashKey.GetHashCode(x,y));
        }
        /// <summary>
        /// ��Ԫ����o���ƶ���d��
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
            //�����к��е��������
            if (Math.Abs(x) >this.iMaxWidth||Math.Abs(y) > this.iMaxWidth)
            {
                throw new ArgumentOutOfRangeException("x����y ����������Ĭ�ϵ������ֵ");
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

        #region ö����

        /// <summary>
        /// ö��ĳһ�е�����Ԫ��
        /// </summary>
        /// <param name="iRow">Ҫ��������</param>
        /// <returns></returns>
        internal List<T> EnumerateRow(int iRow)//����ŷ������ϵ�У���Ӧy�ᣬ
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
        /// ö��ĳһ�е�����Ԫ��
        /// </summary>
        /// <param name="iRow">Ҫ��������</param>
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
        /// �ṩ�Դ洢Ԫ�صĸ�Ч����
        /// </summary>
        /// <returns></returns>
        internal IEnumerator<T> GetEnumerator()
        {
            return this.hashMat.Values.GetEnumerator();
        }
        #endregion

    }

    /// <summary>
    /// ʹ�þ������͵Ľṹ��ζ�Ų�֧����·����.��·��֧���д����ۣ���·������֧�ֵ�
    /// </summary>
    internal class RoadNode : RoadEntity
    {
         #region ����ϵ��ת�����߼�����
        /// <summary>
        /// ��������ϵ��ת�Ļ�׼����
        /// </summary>
        private MyPoint mpBaseVector;
        /// <summary>
        /// ���ظñߵ��������������������������ļнǵ����Һ����ҵ�ֵ
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        private Index getOriginIndex(Index point, MyPoint newVector)
        {
            if (point == null || newVector == null)
            {
                throw new ArgumentException("����Ĳ�������Ϊ��");
            }
            if(mpBaseVector==null)
            {
                mpBaseVector = newVector;
            }
            //��ȡ���Һ�����ֵ���ҽ�����ת�任
            SinCos sc = VectorTools.getSinCos(mpBaseVector, newVector);
            return Coordinates.Rotate(point, sc);
        }
        /// <summary>
        /// �ж�ָ���������Ƿ���Ԫ��ռ��
        /// </summary>
        /// <param name="point"></param>
        /// <param name="re"></param>
        /// <returns></returns>
        internal bool IsCellBlocked(RoadLane rl, int iAhead)
        {
            //��ת�任
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
        /// ����������
        /// </summary>
        /// <param name="rl"></param>
        internal void BlockRoadLane(RoadLane rl)
        {
            if (rl == null)
            {
                throw new ArgumentNullException();
            }
            if (!IsCellBlocked(rl,1))//�յ������
            {//��null ��λ�� rl.rank �͵�1-6��λ�õ�Ԫ��ռ��
                Index iOrgIndex = this.getOriginIndex(rl.ToCenterXY(1), rl.parEntity.ToVector());
                this.AddCell(iOrgIndex,null);//(id, rl.parEntity, null);
            }
        }
        /// <summary>
        /// ��������ͨ
        /// </summary>
        /// <param name="rl"></param>
        internal void UnblockRoadLane(RoadLane rl)
        {
            if (rl == null)
            {
                throw new ArgumentNullException();
            }
            if (IsCellBlocked(rl, 1))//�ǿ���ɾ��
            {//��null ��λ�� rl.rank �͵�1-6��λ�õ�Ԫ��ռ��
                this.RemoveCell(rl, 1);//(id, rl.parEntity, null);
            }
        }


        private void AddCell(Index point,RoadLane rl)
        {
            Index indexOld =this.getOriginIndex(point,rl.parEntity.ToVector());
            cellMatrix.AddCell(indexOld.X, indexOld.Y, null);//�������õ�Ԫ������Ϊnull
        }
        /// <summary>
        /// ��ָ���ĵ����һ��Ԫ����
        /// </summary>
        internal void AddCell(Cell ca)
        {
            //��Rank������Ϊx��0Ϊy
            //������ȫ��ת��ת��Ϊԭ������
            ca.track.iFromPos = this.getOriginIndex(ca.track.iFromPos, ca.track.fromLane.parEntity.ToVector());
            ca.track.iCurrPos = ca.track.iFromPos;
            ca.track.iToPos = this.getOriginIndex(ca.track.iToPos, ca.track.fromLane.parEntity.ToVector());
             
            cellMatrix.AddCell(ca.track.iCurrPos.X,ca.track.iCurrPos.Y, ca);
        }

        /// <summary>
        /// ����ָ����·�Σ�·��ǰ���ľ������ɾ��Ԫ��
        /// </summary>
        /// <param name="rl">��ת����ϵ��Ҫ�õ��ļ�����ת�Ƕȵ�����</param>
        /// <param name="iAheadSpace">ǰ�о�����</param>
        internal bool RemoveCell(RoadLane rl, int iAheadSpace)
        {
            return this.RemoveCell(rl.ToCenterXY(iAheadSpace), rl.parEntity);
        }
        
        /// <summary>
        /// ɾ��ָ���㴦��Ԫ��
        /// </summary>
        /// <param name="point">������</param>
        /// <param name="re">��ת����ϵ��Ҫ�õ��ļ�����ת�Ƕȵ�����</param>
        internal bool RemoveCell(Index point, RoadEdge re)
        {
            Index indexOrg = this.getOriginIndex(point, re.ToVector());
            return cellMatrix.RemoveCell(indexOrg.X, indexOrg.Y);
        }
     
        /// <summary>
        /// �жϵ�x������ǰ���Ƿ���iAheadSpace������
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
        /// �µ�roadnode�Ĺ�ϣɢ��ֵ��������Position�Ĺ�ϣֵ����ID����
        /// </summary>
        /// <returns></returns>
        private HashMatrix<Cell> cellMatrix = new HashMatrix<Cell>();
        /// <summary>
        /// �������ڵ����г��ߵĹ�ϣ����ֵ�Ǵ���ߵ�RoadEdge��ϣ��ֵ�Ǵ���RoadEdge
        /// </summary>
        private Dictionary<int, RoadEdge> dicEdge = new Dictionary<int,RoadEdge>();

        internal ICollection RoadEdges
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
                    if (roadEdge.from !=this)
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
        /// ���ҷ������µĽṹ���ó��߱�
        /// </summary>
        /// <param name="toRoadNode">���ڵ�</param>
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
        /// ����RoadNodeID������
        /// </summary>
        private static int iRoadNodeID;
        [System.Obsolete("ʹ�������ģ����Position���ԡ��Ժ�Ӧ�����ԸĽ�")]
        internal RoadNode() 
        {
            this.ID = ++iRoadNodeID;
            Random rd = new Random();
            this.Postion = new MyPoint(rd.Next(65535), rd.Next(65535));
            /// ֱ��ʹ�������ĵ����ݽṹ,bug��Ӧ��ʹ�������Ľṹ
            if (this.Postion.X == 0.0f && this.Postion.Y == 0.0f)
            {
                throw new Exception("RoadNode�����������꣡");
            }
        }
        
        public override int GetHashCode()
        {
            int iHash = this.Postion.GetHashCode() +this.ID.GetHashCode();
            return iHash.GetHashCode();
        }
        internal override void UpdateStatus()
        {
            //�����첽��Ϣ
            for (int i = 0; i < this.asynAgents.Count; i++)
            {
                Agents.Agent visitorAgent = this.asynAgents[i];
                visitorAgent.VisitUpdate(this);//.VisitUpdate();
            }
            ////����ͬ����Ϣ
            //foreach (UpdateAgent.UpdateAgent item in this.synAgentChain)
            //{
            //    if (item != null)
            //    {
            //        item.Update();//visitor.visit һ���������һ�������ߣ��ܶ�ķ�����
            //    }
            //}
        }
        
        /// <summary>
        /// �ṩ�Թ�ϣ�����ڲ�Ԫ�صı���
        /// </summary>
        /// <returns></returns>
        internal IEnumerator<Cell> GetEnumerator()
        {
            return this.cellMatrix.GetEnumerator();
        }
    }
}
 
