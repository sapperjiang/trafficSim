#region Using directives

using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Globalization;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
#endregion

namespace SubSys_NetworkBuilder
{

    using PointEnumerator = IEnumerator<Point>;

    /// <summary>
    /// Polygon graphic object
    /// </summary>
    public class DrawPolygon:DrawObject //: DrawLine
    {
        private static Cursor handleCursor = new Cursor("PolyHandle.cur");

        private const string entryLength = "Length";
        private const string entryPoint = "Point";


        public DrawPolygon() : base()
        {
            shape = new DrawShape();

            Initialize();
        }

        public DrawPolygon(int x1, int y1, int x2, int y2) : base()
        {
            shape = new DrawShape();
            shape.Add(new Point(x1, y1));
            shape.Add(new Point(x2, y2));

            Initialize();
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawPolygon drawPolygon = new DrawPolygon();

            foreach(Point p in this.shape)
            {
                drawPolygon.shape.Add(p);
            }

            FillDrawObjectFields(drawPolygon);
            return drawPolygon;
        }


        public override void Draw(Graphics g)
        {
            int x1 = 0, y1 = 0;     // previous pointscroll
            int x2, y2;             // current pointscroll

            g.SmoothingMode = SmoothingMode.AntiAlias;

            Pen pen = new Pen(Color, PenWidth);

            PointEnumerator enumerator = shape.GetEnumerator();

            if (enumerator.MoveNext())
            {
                x1 = enumerator.Current.X;
                y1 = enumerator.Current.Y;
            }

            while (enumerator.MoveNext())
            {
                x2 = enumerator.Current.X;
                y2 = enumerator.Current.Y;

                g.DrawLine(pen, x1, y1, x2, y2);

                x1 = x2;
                y1 = y2;
            }

            pen.Dispose();
        }

        public void AddPoint(Point point)
        {
            shape.Add(point);
        }

        public override int HandleCount
        {
            get
            {
                return shape.Count;
            }
        }

        /// <summary>
        /// Get handle pointscroll by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public override Point GetHandle(int handleNumber)
        {
            if (handleNumber < 1)
                handleNumber = 1;

            if (handleNumber > shape.Count)
                handleNumber = shape.Count;

            return ((Point)shape[handleNumber - 1]);
        }

        public override Cursor GetHandleCursor(int handleNumber)
        {
            return handleCursor;
        }

        public override void MoveHandleTo(Point point, int handleNumber)
        {
            if (handleNumber < 1)
                handleNumber = 1;

            if (handleNumber > shape.Count)
                handleNumber = shape.Count;

            shape[handleNumber - 1] = point;

            Invalidate();
        }

        public override void Move(int deltaX, int deltaY)
        {
            int n = shape.Count;
            Point point;

            for (int i = 0; i < n; i++)
            {
                point = new Point(((Point)shape[i]).X + deltaX, ((Point)shape[i]).Y + deltaY);

                shape[i] = point;
            }

            Invalidate();
        }

        public override void SaveToStream(System.Runtime.Serialization.SerializationInfo info, int orderNumber)
        {
            info.AddValue(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryLength, orderNumber),
                shape.Count);

            int i = 0;
            foreach (Point p in shape)
            {
                info.AddValue(
                    String.Format(CultureInfo.InvariantCulture,
                    "{0}{1}-{2}",
                    entryPoint, orderNumber, i++),
                    p);

            }

            base.SaveToStream(info, orderNumber);  // ??
        }

        public override void LoadFromStream(System.Runtime.Serialization.SerializationInfo info, int orderNumber)
        {
            Point point;
            int n = info.GetInt32(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryLength, orderNumber));

            for (int i = 0; i < n; i++)
            {
                point = (Point)info.GetValue(
                    String.Format(CultureInfo.InvariantCulture,
                    "{0}{1}-{2}",
                    entryPoint, orderNumber, i),
                    typeof(Point));

                shape.Add(point);
            }

            base.LoadFromStream(info, orderNumber);
        }

        /// <summary>
        /// Create graphic object used for hit test
        /// </summary>
        protected override void CreateObjects()
        {
            if (AreaPath != null)
                return;

            // Create closed path which contains all polygon vertexes
            AreaPath = new GraphicsPath();

            int x1 = 0, y1 = 0;     // previous pointscroll
            int x2, y2;             // current pointscroll

            PointEnumerator enumerator = shape.GetEnumerator();

            if (enumerator.MoveNext())
            {
                x1 = enumerator.Current.X;
                y1 = enumerator.Current.Y;
            }

            while (enumerator.MoveNext())
            {
                x2 = enumerator.Current.X;
                y2 = enumerator.Current.Y;

                AreaPath.AddLine(x1, y1, x2, y2);

                x1 = x2;
                y1 = y2;
            }

            AreaPath.CloseFigure();

            AreaRegion = new Region(AreaPath);
           // AreaRegion = new Region()
        }

        protected override bool PointInObject(Point testPoint)
        {
            var shapes = this.shape.GetEnumerator();
            while (shapes.MoveNext())
            {
                if (InSmallRectangle(shapes.Current, testPoint) == true) return true;
             }
            return false;

        }

        private bool InSmallRectangle(Point old,Point inPoint)
        {
            //一个边长为4的正方形状
            var rect = new Rectangle(old.X - 4, old.Y - 4, 8, 8);
            AreaRegion = new Region(rect);

            return AreaRegion.IsVisible(inPoint);

        }

        //public override Rectangle GetBoundingBox()
        //{
        //    throw new NotImplementedException();
        //}


       

     
    }
}

