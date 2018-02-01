using System;
using System.Windows.Forms;
using System.Drawing;


namespace SubSys_NetWorkBuilder
{
	/// <summary>
	/// Rectangle tool
	/// </summary>
	class ToolRectangle : global::SubSys_NetWorkBuilder.ToolObject
	{

		public ToolRectangle()
		{
            Cursor = new Cursor("Rectangle.cur");
		}

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new DrawRectangle(e.X, e.Y, 1, 1));
        }

        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
            drawArea.Cursor = Cursor;

            if ( e.Button == MouseButtons.Left )
            {
                Point point = new Point(e.X, e.Y);
                drawArea.GraphicsList[0].MoveHandleTo(point, 5);
                drawArea.Refresh();
            }
        }
	}
}
