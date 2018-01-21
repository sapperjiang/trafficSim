using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_NetworkBuilder
{
    /// <summary>
  //  /// observer:观察drawpolygon和command的情况。
    /// </summary>
    public class NetworkBuilder
    {
        //--------------------------20160131--------------------------------------	
        public static void BulidNetWork(Mementos ways)
        {
            Mementos temp = new Mementos();
            foreach (var drawItem in ways.DrawObjects)
            {
                if (drawItem.IsStateChanged == true)
                {
                    if (drawItem.WayParam.CBCreateReverseWay.Checked == true)
                    {
                        DrawObject ctrWay = drawItem.Clone();// DrawPolygon();

                        ctrWay.Shape.Reverse();

                        ctrWay.Shape.Offset(1);
                        ctrWay.BulidWay();

                        temp.Add(ctrWay);
                    }
                    drawItem.BulidWay();
                    drawItem.IsStateChanged = false;
                }
            }
            ways.AddRange(temp);
        }

    }

}