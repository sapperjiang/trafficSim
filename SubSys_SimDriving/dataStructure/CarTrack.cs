using System.Collections.Generic;
using System.Drawing;

namespace SubSys_SimDriving.TrafficModel
{
    public class CarTrack : Queue<CarInfo>
    {
        CarInfo[] ciArray;
        public CarInfo this[int index]
        {
            get
            {
                if (ciArray ==null)
                {
                    ciArray = base.ToArray();
                }
                if (index<ciArray.Length)
                {
                    return ciArray[index];
                }
                return null;
            }
            
        }
    
    }
   
}