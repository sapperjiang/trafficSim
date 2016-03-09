using System;
using System.Windows.Forms;
using System.Drawing;

namespace SubSys_NetworkBuilder
{
	/// <summary>
	/// Line tool
	/// </summary>
	class ToolLine : ToolObject
	{
        public ToolLine()
        {
            Cursor = new Cursor("Line.cur");
        }

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            Point pointscroll = GetEventPointInArea(drawArea, e);
            AddNewObject(drawArea, new DrawLine(pointscroll.X, pointscroll.Y, pointscroll.X + 1, pointscroll.Y + 1));
        }

        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
            Point pointscroll = GetEventPointInArea(drawArea, e);
            drawArea.Cursor = Cursor;

            if ( e.Button == MouseButtons.Left )
            {
                drawArea.GraphicsList[0].MoveHandleTo(pointscroll, 2);
                drawArea.Refresh();
                drawArea.GraphicsList.Dirty = true;
            }
        }
    }
}
