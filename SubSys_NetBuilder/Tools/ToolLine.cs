using System;
using System.Windows.Forms;
using System.Drawing;

namespace SubSys_NetWorkBuilder
{
	/// <summary>
	/// Line tool
	/// </summary>
	class ToolLine : global::SubSys_NetWorkBuilder.ToolObject
	{
        public ToolLine()
        {
            Cursor = new Cursor(str+ "Line.cur");
        }

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new DrawLine(e.X, e.Y, e.X + 1, e.Y + 1));
        }

        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
            drawArea.Cursor = Cursor;

            if ( e.Button == MouseButtons.Left )
            {
                Point point = new Point(e.X, e.Y);
                drawArea.Graphics.First.Shape.Add(point);
                drawArea.Graphics[0].MoveHandleTo(point, 2);
                drawArea.Refresh();
            }
        }
    }
}
