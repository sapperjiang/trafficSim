using System;

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.SysSimContext.Service;
using SubSys_MathUtility;

namespace SubSys_Graphics
{
    //internal class MyGraphices:gra
    public abstract class PaintService:Service,IPaintService
    {
        protected Graphics graphic;
        private Control _form;//要画的控件

        //public static int iGUICellPixels = GUISettings.CellPixels.iMedium;
        //public static int iGUIMaxLanes = SimSettings.iMaxLanes;

        public Control Canvas
        {
            get { return _form; }
            set { _form = value; }
        }
        /// <summary>
        /// 哈希索引是统一的方法进行访问，如果是roadlane使用rltPos.Y gethashcode
        /// 如果是RoadNode使用roadlane的x+y进行索引
        /// </summary>
        private Dictionary<int, MyPoint> _cellSpaces;

        protected Dictionary<int,MyPoint> CellSpaces
        {
            get { return _cellSpaces; }
            set { _cellSpaces = value; }
        }

        /// <summary>
        /// 提供一个基本的描绘car的函数，画一个圆形代表car然后用car的颜色填充
        /// </summary>
        public virtual void PaintCar(Rectangle rec, ITrafficEntity car)
        {
            if (car.EntityType == EntityType.CarModel)
            {
                Car cm = car as Car;
                graphic.FillRectangle(new SolidBrush(cm.Color), rec);
            }
        }

        protected override void SubPerform(ITrafficEntity tVar)
        {
            throw new NotImplementedException();
        }

        protected override void SubRevoke(ITrafficEntity tVar)
        {
            throw new NotImplementedException();
        }
    }
   
}
