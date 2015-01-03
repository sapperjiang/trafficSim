using System;
using System.Collections.Generic;
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.TrafficModel
{
    internal class LovelyCoodinates
    {
        public double CalcAngle()
        {
            return 0.0;
        }

        /// <summary>
        /// �Ե�mpA�͵�mpBΪ�յ㽨����ֱ�߷��̣�Ȼ���жϵ�mpNew��ֱ�ߵ��Ϸ������·�
        /// </summary>
        /// <param name="mpA"></param>
        /// <param name="mpB"></param>
        /// <param name="mpNew">Ҫ����ĵ�����</param>
        /// <returns></returns>
        public static int getPostionByBaseEquation(MyPoint mpA,MyPoint mpB, MyPoint mpNew)
        {
            return (mpNew.Y-mpA.Y)*(mpB.X-mpA.X)-(mpNew.X-mpA.X)*(mpB.Y-mpA.Y)>0 ?1:-1;
        }

        /// <summary>
        ///��������ʽ�������������̣���������������̼��������������̵���ϵ,������
        ///���������أ���һ������Ĭ��Ϊ��,(y-ya)(xb-xa)-(x-xa)(yb-ya) = 0
        /// </summary>
        /// <param name="mpBaseVectorEnd">���������յ����꣬�������Ϊ0</param>
        /// <param name="mpNew">Ҫ����ĵ�����������������Ӧ�������������յ�����</param>
        /// <returns>����-1��ʾλ�ڻ������·�������1��ʾλ�ڻ������Ϸ�</returns>
        public static int getPosByBaseEquation(MyPoint mpBaseVectorEnd, MyPoint mpNew)
        {
            return LovelyCoodinates.getPostionByBaseEquation(new MyPoint(0.0f, 0.0f), mpBaseVectorEnd, mpNew);
        }
        /// <summary>
        /// ��ȡ���������ļнǵ�����ֵ����ֵ��������-1��1������,��������������������0����
        /// </summary>
        public static double getVectorCosine(MyPoint mpBaseVector, MyPoint mpVector)
        {
            //������������
            double fNumerator = mpBaseVector.X*mpVector.X+mpBaseVector.Y*mpBaseVector.Y;
            //��һ��������������������
            double fMouldBase = mpBaseVector.X *mpBaseVector.X + mpBaseVector.Y *mpBaseVector.Y;
            double dBase = Math.Sqrt(fMouldBase);
            //�ڶ���������ģ
            double fMouldNew = mpBaseVector.X * mpVector.X + mpVector.Y * mpVector.Y;
            double dNew = Math.Sqrt((double)fMouldNew);
            //����������ģ�ĳ˻�
            double dDenominator = dBase * dNew;
            if (dDenominator == 0.0)
            {
                throw new DivideByZeroException("������ģΪ0�ǲ������");
            }
           ///��������ֵ
            return fNumerator/dDenominator;

        }
        /// <summary>
        /// �ж��ǶȲ�������Ƕȵ����Һ�����ֵ
        /// </summary>
        public static SinCos getSinCos(MyPoint mpBaseVector, MyPoint mpVector)
        {
            double dCosineValue = LovelyCoodinates.getVectorCosine(mpBaseVector, mpVector);
            //180�ȵ����ұձ�����
            if (-1<=dCosineValue&& dCosineValue<-1.707)//-����2������1.414 ��һ�� ��0.707
            {
                return new SinCos(0,-1);//cosine 180 ��-1��
            }
            ///90�Ȼ���270��
            if (-0.707 <= dCosineValue && dCosineValue < -0.707)
            {///�ж�yλ�ڻ��������Ϸ������·�
                int y= LovelyCoodinates.getPosByBaseEquation(mpBaseVector,mpVector);
                if (y<0)//270��
                {
                    return new SinCos(-1, 0); 
                }
                if (y > 0)//90��
                {
                    return new SinCos(1, 0);
                }
                else//y==0 
                {
                    throw new Exception("�����ܳ��ֵ�ֵ");
                }
            }
            return null ;
        }

        sealed class SinCos 
        {
            public int iSin = 0;
            public int iCos = 0;
            public SinCos(int iSinine, int iCosine)
            {
                iSin = iSinine;
                iCos = iCosine;
            }
        } 
    }

    public class HashMatrix<T>
    {
        /// <summary>
        /// �����������������Զ����RoadNode��positon
        /// </summary>
        public static int iMaxWidth = 6;

        int iMaxRow;//����������е����꣬���о�����б���ʹ��
        int iMaxColumn;//����������е����꣬�����о���ı���

        public class CellHashKey
        {
            //���ã�x,y�������´洢�ṹ�Ĺ�ϣֵ����֧������x.y���ٷ��ʾ���Ԫ��
            public static int GetHashCode(int ix, int iy)
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
        public bool IsCellBlocked(int x, int y)
        {
            return hashMat.ContainsKey(CellHashKey.GetHashCode(x,y));
        }
        /// <summary>
        /// ��Ԫ����o���ƶ���d��
        /// </summary>
        public void MoveCell(int xO, int yO, int xD, int yD)
        {
            int iHashkey = CellHashKey.GetHashCode(xO, yO);
            T cac;
            if (hashMat.TryGetValue(iHashkey, out cac) == true)
            {
                hashMat.Remove(CellHashKey.GetHashCode(xO, yO));
                hashMat.Add(CellHashKey.GetHashCode(xD, yD), cac);
            }
        }

        public void AddCell(int x,int y,T cell)
        {
            //�����к��е��������
            this.iMaxColumn = x > this.iMaxColumn ? x : this.iMaxColumn;
            this.iMaxRow = y > this.iMaxRow ? y : this.iMaxRow;

            int iHKey = CellHashKey.GetHashCode(x,y);
            if (!hashMat.ContainsKey(iHKey))
	        {
                hashMat.Add(iHKey,cell);
	        }
        }
        public void RemoveCell(int x, int y)
        {
            hashMat.Remove(CellHashKey.GetHashCode(x, y));
        }
        
        /// <summary>
        /// ö��ĳһ�е�����Ԫ��
        /// </summary>
        /// <param name="iRow">Ҫ��������</param>
        /// <returns></returns>
        public List<T> EnumerateRow(int iRow)//������ѧŷ������ϵ�У���Ӧy�ᣬ
        {
            List<T> listT = new List<T>();
            for (int x = 0; x < this.iMaxColumn; x++)
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
        public List<T> EnumerateColumn(int iRow)//������ѧŷ������ϵ�У���Ӧy�ᣬ
        {
            List<T> listT = new List<T>();
            for (int x = 0; x < this.iMaxColumn; x++)
            {
                T cac;
                if (hashMat.TryGetValue(CellHashKey.GetHashCode(x, iRow), out cac) == true)
                {
                    listT.Add(cac);
                }
            }
            return listT;
        }
    }

    /// <summary>
    /// ʹ�þ������͵Ľṹ��ζ�Ų�֧����·����.��·��֧���д����ۣ���·������֧�ֵ�
    /// </summary>
    public class RoadNode : RoadEntity
    {
        /// <summary>
        /// ����RoadNodeID������
        /// </summary>
        private static int iRoadNodeID;
        public RoadNode() 
        {
            this.ID = ++iRoadNodeID;
            /// ֱ��ʹ�������ĵ����ݽṹ,bug��Ӧ��ʹ�������Ľṹ
        }
        /// <summary>
        /// �ù�ϣ������ѯ�����٣���ֵ�Ǵ���ߵ�RoadSegment��ֵ�Ǵ���
        /// </summary>
        private Dictionary<int, RoadEdge> dicEdge = new Dictionary<int,RoadEdge>();
        //public bool visited; //���ʱ�־,����ʱʹ��
        /// <summary>
        /// �µ�roadnode�Ĺ�ϣɢ��ֵ��������Position�Ĺ�ϣֵ����ID����
        /// </summary>
        /// <returns></returns>
        public HashMatrix<CACell> cellMatix;
    
        /// <summary>
        /// �洢��Ϊʱ�䲻��һ�����еĽ���ڵ�ĳ�����ֻӦ���ڳ�ʼ���ĵط�����,
        /// ��������ȫ����··��ַ֮�󣬸ô����ṹӦ��Ϊ��
        /// </summary>
        Queue<CACell> queWaitedCACell = new Queue<CACell>();

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
                    if (roadEdge.rnFrom !=this)
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
            dicEdge.Remove(RoadEdge.GetHashCode(this,re.rnTo));
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
        public override int GetHashCode()
        {
            int iHash = this.Postion.GetHashCode() +this.ID.GetHashCode();
            return iHash.GetHashCode();
        }
        public override void UpdateStatus()
        {
            //�����첽��Ϣ
            for (int i = 0; i < this.asynRuleChain.Count; i++)
            {
                UpdateRule.UpdateRule visitorRule = this.asynRuleChain[i];
                visitorRule.VisitUpdate(this);//.VisitUpdate();
            }
            ////����ͬ����Ϣ
            //foreach (UpdateRule.UpdateRule item in this.synRuleChain)
            //{
            //    if (item != null)
            //    {
            //        item.Update();//visitor.visit һ���������һ�������ߣ��ܶ�ķ�����
            //    }
            //}
        }
    }
}
 
