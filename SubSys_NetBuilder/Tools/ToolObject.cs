using System;
using System.Windows.Forms;
using System.Drawing;


namespace SubSys_NetWorkBuilder
{
	/// <summary>
	/// Base class for all tools which create new graphic object
	/// </summary>
	abstract class ToolObject : global::SubSys_NetWorkBuilder.Tool
	{
       protected string str = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

        private Cursor cursor;

        /// <summary>
        /// Tool cursor.
        /// </summary>
        protected Cursor Cursor
        {
            get
            {
                return cursor;
            }
            set
            {
                cursor = value;
            }
        }


        /// <summary>
        /// Left mouse is released.
        /// New object is created and resized.
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
        {

            if (drawArea.Graphics.Count > 0)
            {
                if (drawArea.Graphics.First.Shape.Length>=2)//有一定长度才画图
                {
                    DrawObject draw = drawArea.Graphics.First;
                    draw.ShowParamSetting(e.Location);
                    draw.BulidWay(draw.WaySetter);

                    if (draw.WaySetter.CBCreateReverseWay.Checked == true)
                    {
                        DrawObject ctrWay = draw.BuildCtrWay();

                        drawArea.Graphics.AddFirst(ctrWay);
                        drawArea.SetDirty();
                        drawArea.AddCommandToHistory(new CommandAdd(ctrWay));
                    }

                    drawArea.AddCommandToHistory(new CommandAdd(drawArea.Graphics.First));
                    drawArea.ActiveTool = DrawArea.DrawToolType.Pointer;

                    drawArea.Capture = false;
                    drawArea.Refresh();
                } 
         }

            //drawArea.GraphicsList[0].Normalize();
            //drawArea.AddCommandToHistory(new CommandAdd(drawArea.GraphicsList[0]));
            //drawArea.ActiveTool = DrawArea.DrawToolType.Pointer;

            //drawArea.Capture = false;
            //drawArea.Refresh();
        }

        /// <summary>
        /// Add new object to draw area.
        /// Function is called when user left-clicks draw area,
        /// and one of ToolObject-derived tools is active.
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="o"></param>
        protected void AddNewObject(DrawArea drawArea, DrawObject o)
        {
            drawArea.Graphics.UnselectAll();

            o.Selected = true;
            drawArea.Graphics.Add(o);

            drawArea.Capture = true;
            drawArea.Refresh();

            drawArea.SetDirty();
        }
	}
}
