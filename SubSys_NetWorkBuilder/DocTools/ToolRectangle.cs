using System;
using System.Resources;
using System.Windows.Forms;
using System.Drawing;


namespace SubSys_NetworkBuilder
{
	/// <summary>
	/// Rectangle tool
	/// </summary>
	class ToolRectangle : ToolObject
	{

		public ToolRectangle()
		{
			Cursor = new Cursor("Rectangle.cur");
			
//			var  buffer = ResourceManager.GetObject("Rectangle.cur") as byte[];
//
//			using (var m = new MemoryStream(buffer))
//			{
//				Cursor =  new Cursor(m);
//			}
			
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
				drawArea.Graphics[0].MoveHandleTo(pointscroll, 5);
				drawArea.Refresh();
				drawArea.Graphics.Dirty = true;
			}
		}
	}
}
