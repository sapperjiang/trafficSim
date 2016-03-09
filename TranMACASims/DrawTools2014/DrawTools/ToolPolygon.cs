using System;
using System.Windows.Forms;
using System.Drawing;


namespace DrawTools
{
	/// <summary>
	/// Polygon tool
	/// </summary>
	class ToolPolygon : DrawTools.ToolObject
	{
		public ToolPolygon()
		{
            Cursor = new Cursor(GetType(), "Pencil.cur");
        }

        private int lastX;
        private int lastY;
        private DrawPolygon newPolygon;
        private const int minDistance = 15*15;

        /// <summary>
        /// Left nouse button is pressed
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            Point p = GetEventPointInArea(drawArea, e);
            // Create new polygon, add it to the list
            // and keep reference to it
            newPolygon = new DrawPolygon(p.X, p.Y, p.X + 1, p.Y + 1);
            AddNewObject(drawArea, newPolygon);
            lastX = p.X;
            lastY = p.Y;
        }

        /// <summary>
        /// Mouse move - resize new polygon
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
            Point p = GetEventPointInArea(drawArea, e);
            drawArea.Cursor = Cursor;

            if ( e.Button != MouseButtons.Left )
                return;

            if ( newPolygon == null )
                return;                 // precaution

            Point point = new Point(p.X, p.Y);
            int distance = (p.X - lastX)*(p.X - lastX) + (p.Y - lastY)*(p.Y - lastY);

            if ( distance < minDistance )
            {
                // Distance between last two points is less than minimum -
                // move last pointscroll
                newPolygon.MoveHandleTo(point, newPolygon.HandleCount);
                drawArea.GraphicsList.Dirty = true;
            }
            else
            {
                // Add new pointscroll
                newPolygon.AddPoint(point);
                lastX = p.X;
                lastY = p.Y;
                drawArea.GraphicsList.Dirty = true;
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
