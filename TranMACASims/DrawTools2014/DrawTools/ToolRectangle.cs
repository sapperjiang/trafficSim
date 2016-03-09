using System;
using System.Windows.Forms;
using System.Drawing;


namespace DrawTools
{
	/// <summary>
	/// Rectangle tool
	/// </summary>
	class ToolRectangle : DrawTools.ToolObject
	{

		public ToolRectangle()
		{
            Cursor = new Cursor(GetType(), "Rectangle.cur");
		}

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            Point pointscroll = GetEventPointInArea(drawArea, e);

            AddNewObject(drawArea, new DrawRectangle(pointscroll.X, pointscroll.Y, 1, 1));
        }

        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
            Point pointscroll = GetEventPointInArea(drawArea, e);
            drawArea.Cursor = Cursor;

            if ( e.Button == MouseButtons.Left )
            {
                drawArea.GraphicsList[0].MoveHandleTo(pointscroll, 5);
                drawArea.Refresh();
                drawArea.GraphicsList.Dirty = true;
            }
        }
	}
}
