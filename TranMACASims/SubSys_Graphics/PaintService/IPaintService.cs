using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using SubSys_SimDriving;
using SubSys_SimDriving.SysSimContext.Service;

namespace SubSys_Graphics
{
    public interface IPaintService:IService
    {
        Control Canvas
        {
            get;
            set;
        }

        void PaintCar(Rectangle rec,ITrafficEntity car);
        
    }
    
}
