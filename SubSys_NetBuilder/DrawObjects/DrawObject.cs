using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Collections.Generic;
using SubSys_NetBuilder;
using SubSys_MathUtility;
using SubSys_SimDriving.TrafficModel;
using SubSys_Graphics;

namespace SubSys_NetWorkBuilder
{
	/// <summary>
	/// Base class for all draw objects
	/// </summary>
public	abstract  partial class DrawObject
	{
        #region Members

        // Object properties
        private bool selected;
        private Color color;
        private int penWidth = GraphicsSetter.iPixels;

        // Allows to write Undo - Redo functions and don't care about
        // objects order in the list.
        int id;   

        // Last used property values (may be kept in the Registry)
        private static Color lastUsedColor = Color.Black;
        private static int lastUsedPenWidth ;

        // Entry names for serialization
        private const string entryColor = "Color";
        private const string entryPenWidth = "PenWidth";

        #endregion

        public DrawObject()
        {
            this.shape = new DrawShape();
            id = this.GetHashCode();
        }

        #region Properties

        /// <summary>
        /// Selection flag
        /// </summary>
        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
            }
        }

        /// <summary>
        /// Color
        /// </summary>
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        /// <summary>
        /// Pen width
        /// </summary>
        public int PenWidth
        {
            get
            {
                return penWidth;
            }
            set
            {
                penWidth = value;
            }
        }

        //public int PenWidth
        //{
        //    get
        //    {
        //        return penWidth;
        //    }
        //    set
        //    {
        //        penWidth = value;
        //    }
        //}

        /// <summary>
        /// Number of handles
        /// </summary>
        public virtual int HandleCount
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Object ID
        /// </summary>
        public int ID
        {
            get { return id; }
            set { id = value; }
        }


        /// <summary>
        /// Last used color
        /// </summary>
        public static Color LastUsedColor
        {
            get
            {
                return lastUsedColor;
            }
            set
            {
                lastUsedColor = value;
            }
        }

        /// <summary>
        /// Last used pen width
        /// </summary>
        public static int LastUsedPenWidth
        {
            get
            {
                return lastUsedPenWidth;
            }
            set
            {
                lastUsedPenWidth = value;
            }
        }

        #endregion

        #region Virtual Functions

        /// <summary>
        /// Clone this instance.
        /// </summary>
        public abstract DrawObject Clone();

        /// <summary>
        /// Draw object
        /// </summary>
        /// <param name="g"></param>
        public virtual void Draw(Graphics g)
        {
        }

        /// <summary>
        /// Get handle point by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public virtual Point GetHandle(int handleNumber)
        {
            return new Point(0, 0);
        }

        /// <summary>
        /// Get handle rectangle by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public virtual Rectangle GetHandleRectangle(int handleNumber)
        {
            Point point = GetHandle(handleNumber);

            return new Rectangle(point.X - 3, point.Y - 3, 7, 7);
        }

        /// <summary>
        /// Draw tracker for selected object
        /// </summary>
        /// <param name="g"></param>
        public virtual void DrawTracker(Graphics g)
        {
            if ( ! Selected )
                return;

            SolidBrush brush = new SolidBrush(Color.Black);

            for ( int i = 1; i <= HandleCount; i++ )
            {
                g.FillRectangle(brush, GetHandleRectangle(i));
            }

            brush.Dispose();
        }

        /// <summary>
        /// Hit test.
        /// Return value: -1 - no hit
        ///                0 - hit anywhere
        ///                > 1 - handle number
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual int HitTest(Point point)
        {
            return -1;
        }


        /// <summary>
        /// Test whether point is inside of the object
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        protected virtual bool PointInObject(Point point)
        {
            return false;
        }
        

        /// <summary>
        /// Get cursor for the handle
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public virtual Cursor GetHandleCursor(int handleNumber)
        {
            return System.Windows.Forms.Cursors.Default;
        }

        /// <summary>
        /// Test whether object intersects with rectangle
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public virtual bool IntersectsWith(Rectangle rectangle)
        {
            return false;
        }

        /// <summary>
        /// Move object
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        public virtual void Move(int deltaX, int deltaY)
        {
        }

        /// <summary>
        /// Move handle to the point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="handleNumber"></param>
        public virtual void MoveHandleTo(Point point, int handleNumber)
        {
        }

        /// <summary>
        /// Dump (for debugging)
        /// </summary>
        public virtual void Dump()
        {
            Trace.WriteLine(this.GetType().Name);
            Trace.WriteLine("Selected = " + 
                selected.ToString(CultureInfo.InvariantCulture)
                + " ID = " + id.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Normalize object.
        /// Call this function in the end of object resizing.
        /// </summary>
        public virtual void Normalize()
        {
        }


        /// <summary>
        /// Save object to serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public virtual void SaveToStream(SerializationInfo info, int orderNumber)
        {
            info.AddValue(
                String.Format(CultureInfo.InvariantCulture,
                    "{0}{1}",
                    entryColor, orderNumber),
                Color.ToArgb());

            info.AddValue(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryPenWidth, orderNumber),
                PenWidth);
        }

        /// <summary>
        /// Load object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public virtual void LoadFromStream(SerializationInfo info, int orderNumber)
        {
            int n = info.GetInt32(
                String.Format(CultureInfo.InvariantCulture,
                    "{0}{1}",
                    entryColor, orderNumber));

            Color = Color.FromArgb(n);

            PenWidth = info.GetInt32(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryPenWidth, orderNumber));

            id = this.GetHashCode();
        }

        #endregion

        #region Other functions

        /// <summary>
        /// Initialization
        /// </summary>
        protected void Initialize()
        {
            color = lastUsedColor;
            penWidth = LastUsedPenWidth;
        }

        /// <summary>
        /// Copy fields from this instance to cloned instance drawObject.
        /// Called from Clone functions of derived classes.
        /// </summary>
        protected void FillDrawObjectFields(DrawObject drawObject)
        {
            drawObject.selected = this.selected;
            drawObject.color = this.color;
            drawObject.penWidth = this.penWidth;
            drawObject.ID = this.ID;
        }

        #endregion


        //private Point start;
        //private Point End;
        ////---------------------added by sapperjiang------------------------------
        /// <summary>
        /// the start point of a segment
        /// </summary>
        public Point Start
        {
            get { return this.shape.Start; }
            set
            {
                this.shape[0] = value;
            }
        }

        /// <summary>
        /// the end point of a segment
        /// </summary>
        public Point End
        {
            get { return this.shape[this.shape.Count - 1]; }
            set { this.shape[this.shape.Count - 1] = value; }
        }

        //added by sapperjiang 20161119
        protected DrawShape shape;         // list of points

        public bool IsStateChanged = false;
        public WaySetter WaySetter;


        public DrawShape Shape
        {
            get
            {
                return this.shape;
            }
        }

        public Way Way
        {
            get
            {
                return way;
            }
            set
            {
                way = value;
            }
        }

        private Way way;
        public void BulidWay(WaySetter ws)
        {
            IRoadNet inet = RoadNet.GetInstance();
            Way = inet.BuildWay(this.Shape.Start, this.Shape.End);
            Way.Name = ws.TBWayName.Text + Way.ID.ToString();
            Way.TrueLength = (double)ws.TBLength.Value;
            ///先创建一个车道做测试
            int iLaneCount = System.Convert.ToInt32(ws.TBLaneCount.Value);
            Way.AddLane(LaneType.All);
            Way.Shape = EntityShape.CreateShape(this.shape);
        }

        public DrawObject BuildCtrWay()
        {
            DrawObject ctrWay = this.Clone();
            ctrWay.Shape.Reverse();
            ctrWay.Shape.Offset(1 );//右手坐标系，右手坐标系跟驾驶习惯有关系
            ctrWay.BulidWay(this.WaySetter);
            ctrWay.Shape.Offset( penWidth-1);//右手坐标系，右手坐标系跟驾驶习惯有关系

            Way.WaysBind(this.Way, ctrWay.Way);
            return ctrWay;
        }

        internal virtual bool ShowParamSetting(Point drawEndPoint)
        {
            if (this.WaySetter == null)
            {
                this.WaySetter = new WaySetter();
            }
            this.WaySetter.Location = drawEndPoint;

            if (this.WaySetter.ShowDialog() == DialogResult.OK)
            {
                this.IsStateChanged = true;
                return true;
            }
            this.IsStateChanged = false;
            return false;
        }

        public class DrawShape : List<Point>
        {
            public Point Start
            { get { return this[0]; } }
            public Point End
            {
                get
                {
                    if (this.Count > 1)
                    {
                        return this[this.Count - 1];
                    }
                    return this[0];
                }
            }

            public int Length
            {
                get { return this.Count; }
            }
            /// <summary>
            /// move shape by  "scaler" steps with its normal vector
            /// </summary>
            /// <param name="iScaler"></param>
            public void Offset(int iScaler)
            {
                Point vector = VectorTool.GetNormal(this.Start, this.End);
                double dWidth = iScaler;
                Point temp = new Point();
                for (int i = 0; i < this.Count; i++)
                {
                    temp = this[i];//value copy
                    Coordinates.Offset(ref temp, iScaler, vector);//ref copy
                    this[i] = temp;
                }
            }

            //public void Offset(int iScaler, Point vector)
            //{
            //    double dWidth = iScaler;
            //    Point temp = new Point();
            //    //List<Point> shape = new List<Point>();
            //    for (int i = 0; i < this.Count; i++)
            //    {
            //        temp = this[i];
            //        Coordinates.Offset(ref temp, vector);
            //    }
            //}
        }

    }
}
