using System.Diagnostics;
using System.Collections.Generic;
using SubSys_SimDriving;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.Agents
{
    internal class SpeedUpDownAgent : Agent
    {
        internal SpeedUpDownAgent()
        {   
            this.strAgentName = AgentName.SpeedUpDownAgent;
            this.priority = AgentPriority.Medium;
            this.agentType = AgentType.Synchronization;
        }
        /// <summary>
        /// ����roadEdge����
        /// </summary>
        /// <param name="roadEdge"></param>
        internal override void VisitUpdate(RoadEdge roadEdge)
        {
            if (roadEdge == null)
            {
                throw new System.ArgumentException("������ģʽ���ʶ�����Ϊ�գ�RoadEntityû�и�ֵ��");
            }
            //����ÿ������
            foreach (RoadLane rl in roadEdge.Lanes)
            {
                this.VisitUpdate(rl);
            }
        }
        internal sealed override void VisitUpdate(RoadNode roadNode)
        {
            if (roadNode == null)
            {
                throw new System.ArgumentException("������ģʽ���ʶ�����Ϊ�գ�roadNodeû�и�ֵ��");
            }
            IEnumerator<Cell> ceEnum = roadNode.GetEnumerator();
            ceEnum.Reset();
            while (ceEnum.MoveNext())
            {
                Cell ce = ceEnum.Current;
                ce.Drive(roadNode);
            }

        }

        /// <summary>
        /// ����һ���������������Ԫ�����󣬶��ڻ�������˵������Ϊ��һ��˳��
        /// ǰ�м��ٵ������ٶȣ������ٶ���ʻ�����߻������ٵ������ٶȣ������ʻԱ��Ϊ��ͨģ��
        /// ���Ǻ��鷳��
        /// </summary>
        /// <param name="rL"></param>
        //internal override void VisitUpdate(RoadLane rL)
        //{
        //    int iHeadWay = 0;
        //    Cell ca = rL.cells.PeekLast();//����ĩβһ��Ԫ��
        //    int iOutCount = 0;//��¼Ҫɾ������Ԫ�� 
        //    while (ca != null)
        //    {
        //        //û����һ��Ԫ����ʹ��·�γ��ȣ�����ʹ����һ��Ԫ����λ��
        //        iHeadWay = ca.nextCell == null ? rL.iLength : ca.iPos;
        //        iHeadWay -= ca.iPos;//���㳵ͷʱ��
        //        //��������ʽ S=1/2*a*t^2+v*t;����һ��ʱ�䲽���ڲ������ƶ��ľ���
        //        int iMoveStep = (int)(0.5 * ca.cmCarModel.iAcc + ca.cmCarModel.iSpeed);
        //        if (iMoveStep <= iHeadWay)//��ͷʱ��ȼ�����ľ����
        //        {
        //            ca.cmCarModel.iSpeed += ca.cmCarModel.iAcc;//�����ٶ�
        //            ca.iPos += iMoveStep;//λ�ø���
        //        }else//iMoveStep �Ƚϴ������������·�ڡ���ôiHeadWay��0
        //            //iMoveStep ��˵����һ��ʱ�䲽���ھ�Ҫʻ�복�����뽻�����
        //        { 
        //            int iAheadSpace = iMoveStep-iHeadWay;

        //            Debug.Assert(rL.parEntity!=null);
        //            Debug.Assert(rL.parEntity.to != null);

        //            ///���ΪfalseԪ����Ҫ�ƶ���������Ӧ������ɾ��
        //            RoadNode toRoadNode = rL.parEntity.to;
        //            if (toRoadNode.IsAheadBlocked(rL,iAheadSpace)==false)
        //            {
        //                //ǰ���ľ���ȳ�ͷʱ��󡣽��뽻��ڣ����ҽ����û��Ԫ��
        //                                       //��ӵ������
        //                toRoadNode.AddCell(rL, iAheadSpace, ca);
        //                iOutCount++;//��¼Ҫ��ȥ�ĳ���Ԫ����
        //            }
        //        }
        //        ca = ca.nextCell;//ǰ������һ��caԪ�������и���
        //    }//while ����

        //    Debug.Assert(iOutCount<=rL.cells.Count);
        //    //ɾ���Ѿ�ʻ��·�εĳ���Ԫ�����Ѿ�ʻ����·��Ԫ��
        //    while (iOutCount-- > 0)
        //    {
        //        rL.cells.Dequeue();
        //    }

        //}
        /// <summary>
        /// �ɰ汾���ɶ���ĩβ�����ͷ�������ð汾���ɶ���ͷ�����β������
        /// �����������Ҫ�������ģʽ����Ϊÿ����ʻԱ��Ϊ����һ�����ݲ�����ʽ
        ///Ԫ��״̬�ĸ��¿���Ҫ����۲���ģʽ
        /// </summary>
        /// <param name="rL"></param>
        internal override void VisitUpdate(RoadLane rL)
        {
            IEnumerator<Cell> enumCell = rL.cells.GetEnumerator();
            enumCell.Reset();

            while (enumCell.MoveNext())
            {
                Cell ca = enumCell.Current;
                ca.Drive(rL);
            }//while ����
        }

    }
}
