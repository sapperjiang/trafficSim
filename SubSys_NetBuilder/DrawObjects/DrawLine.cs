using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Globalization;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace SubSys_NetWorkBuilder
{
	/// <summary>
	/// Line graphic object
	/// </summary>
	class DrawLine : global::SubSys_NetWorkBuilder.DrawObject
	{
        //private Point Start;
        //private Point End;

        private const string entryStart = "Start";
        private const string entryEnd = "End";

        /// <summary>
        ///  Graphic objects for hit test
        /// </summary>
        private GraphicsPath areaPath = null;
        private Pen areaPen = null;
        private Region areaRegion = null;


		public DrawLine() //: this(0, 0, 1, 0)
		{
		}

        public DrawLine(int x1, int y1, int x2, int y2) : base()
        {
            this.shape.Add(new Point(x1, y1));
            this.shape.Add(new Point(x2, y2));
            //Start.X = x1;
            //Start.Y = y1;
            //End.X = x2;
            //End.Y = y2;

            Initialize();
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawLine drawLine = new DrawLine();
            drawLine.shape.Add(this.Start);
            drawLine.shape.Add( this.End);

            FillDrawObjectFields(drawLine);
            return drawLine;
        }


        public override void Draw(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Pen pen = new Pen(Color, PenWidth);

            g.DrawLine(pen, Start.X, Start.Y, End.X, End.Y);

            pen.Dispose();
        }

        public override int HandleCount
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        /// Get handle point by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public override Point GetHandle(int handleNumber)
        {
            if ( handleNumber == 1 )
                return Start;
            else
                return End;
        }

        /// <summary>
        /// Hit test.
        /// Return value: -1 - no hit
        ///                0 - hit anywhere
        ///                > 1 - handle number
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override int HitTest(Point point)
        {
            if ( Selected )
            {
                for ( int i = 1; i <= HandleCount; i++ )
                {
                    if ( GetHandleRectangle(i).Contains(point) )
                        return i;
                }
            }

            if ( PointInObject(point) )
                return 0;

            return -1;
        }

        protected override bool PointInObject(Point point)
        {
            CreateObjects();

            return AreaRegion.IsVisible(point);
        }

        public override bool IntersectsWith(Rectangle rectangle)
        {
            CreateObjects();

            return AreaRegion.IsVisible(rectangle);
        }

        public override Cursor GetHandleCursor(int handleNumber)
        {
            switch ( handleNumber )
            {
                case 1:
                case 2:
                    return Cursors.SizeAll;
                default:
                    return Cursors.Default;
            }
        }

        public override void MoveHandleTo(Point point, int handleNumber)
        {
            if ( handleNumber == 1 )
                Start = point;
            else
                End = point;

            Invalidate();
        }

        public override void Move(int deltaX, int deltaY)
        {
            Point start = this.Start;
            start.X += deltaX;
            start.Y += deltaY;
            Point end = this.End;

            end.X += deltaX;
            end.Y += deltaY;

            Invalidate();
        }

        public override void SaveToStream(System.Runtime.Serialization.SerializationInfo info, int orderNumber)
        {
            info.AddValue(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryStart, orderNumber),
                Start);

            info.AddValue(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryEnd, orderNumber),
                End);

            base.SaveToStream (info, orderNumber);
        }

        public override void LoadFromStream(SerializationInfo info, int orderNumber)
        {
            Start = (Point)info.GetValue(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryStart, orderNumber),
                typeof(Point));

            End = (Point)info.GetValue(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryEnd, orderNumber),
                typeof(Point));

            base.LoadFromStream (info, orderNumber);
        }


        /// <summary>
        /// Invalidate object.
        /// When object is invalidated, path used for hit test
        /// is released and should be created again.
        /// </summary>
        protected void Invalidate()
        {
            if ( AreaPath != null )
            {
                AreaPath.Dispose();
                AreaPath = null;
            }

            if ( AreaPen != null )
            {
                AreaPen.Dispose();
                AreaPen = null;
            }

            if ( AreaRegion != null )
            {
                AreaRegion.Dispose();
                AreaRegion = null;
            }
        }

        /// <summary>
        /// Create graphic objects used from hit test.
        /// </summary>
        protected virtual void CreateObjects()
        {
            if ( AreaPath != null )
                return;

            // Create path which contains wide line
            // for easy mouse selection
            AreaPath = new GraphicsPath();
            AreaPen = new Pen(Color.Black, 7);
            AreaPath.AddLine(Start.X, Start.Y, End.X, End.Y);
            AreaPath.Widen(AreaPen);

            // Create region from the path
            AreaRegion = new Region(AreaPath);
        }

        protected GraphicsPath AreaPath
        {
            get
            {
                return areaPath;
            }
            set
            {
                areaPath = value;
            }
        }

        protected Pen AreaPen
        {
            get
            {
                return areaPen;
            }
            set
            {
                areaPen = value;
            }
        }

        protected Region AreaRegion
        {
            get
            {
                return areaRegion;
            }
            set
            {
                areaRegion = value;
            }
        }

	}
}
