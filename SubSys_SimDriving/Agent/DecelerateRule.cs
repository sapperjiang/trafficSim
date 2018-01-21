using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.UpdateRule
{
	public class DecelerateRule : UpdateRule
	{
        public override void VisitUpdate(RoadEdge roadEdge)
        {
            if (roadEdge == null)
            {
                throw new System.ArgumentException("������ģʽ���ʶ�����Ϊ�գ�RoadEntityû�и�ֵ��");
            }
            //����ÿ������
            foreach (RoadLane rl in roadEdge.laneChain)
            {
                this.VisitUpdate(rl);
            }
        }

        public override void VisitUpdate(RoadLane roadLane)
        {

            System.Windows.Forms.MessageBox.Show("DecelerateRule Updated");
        }
        public override void VisitUpdate(RoadNode roadLane)
        {
            System.Windows.Forms.MessageBox.Show("DecelerateRule Updated");
        }

	}
	 
}
 
