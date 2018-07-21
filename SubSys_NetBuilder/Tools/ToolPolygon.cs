using System;
using System.Windows.Forms;
using System.Drawing;


namespace SubSys_NetWorkBuilder
{
	/// <summary>
	/// Polygon tool
	/// </summary>
	class ToolPolygon : global::SubSys_NetWorkBuilder.ToolObject
	{
		public ToolPolygon()
		{
            //string str = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            Cursor = new Cursor(str+"Pencil.cur");
        }

        private int lastX;
        private int lastY;
        private DrawPolygon newPolygon;
        private const int minDistance = 5*5;

        /// <summary>
        /// Left mouse button is pressed
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            // Create new polygon, add it to the list
            // and keep reference to it
            //newPolygon = new DrawPolygon(e.X, e.Y, e.X + 1, e.Y + 1);
            newPolygon = new DrawPolygon();
            AddNewObject(drawArea, newPolygon);
            newPolygon.Shape.Add(new Point(e.X, e.Y));//添加第一个点
            lastX = e.X;
            lastY = e.Y;
        }

        /// <summary>
        /// Mouse move - resize new polygon
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
            drawArea.Cursor = Cursor;

            if ( e.Button != MouseButtons.Left )
                return;

            if ( newPolygon == null )
                return;                 // precaution

            Point point = new Point(e.X, e.Y);
            int distance = (e.X - lastX)*(e.X - lastX) + (e.Y - lastY)*(e.Y - lastY);

            if ( distance < minDistance )
            {
                // Distance between last two points is less than minimum -
                // move last point
                newPolygon.MoveHandleTo(point, newPolygon.HandleCount);
            }
            else
            {
                // Add new point
                newPolygon.AddPoint(point);
                lastX = e.X;
                lastY = e.Y;
            }

            drawArea.Refresh();
        }

        public override void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
        {
            newPolygon = null;

            base.OnMouseUp (drawArea, e);
        }


	}
}
