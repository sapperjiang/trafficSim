using System.Windows.Forms;

namespace TrafficSim
{
    partial class DebugShower
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LB_LaneNuber = new System.Windows.Forms.ListBox();
            this.LB_LaneShape = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // LB_LaneNuber
            // 
            this.LB_LaneNuber.FormattingEnabled = true;
            this.LB_LaneNuber.ItemHeight = 12;
            this.LB_LaneNuber.Location = new System.Drawing.Point(12, 12);
            this.LB_LaneNuber.Name = "LB_LaneNuber";
            this.LB_LaneNuber.Size = new System.Drawing.Size(139, 268);
            this.LB_LaneNuber.TabIndex = 1;
            // 
            // LB_LaneShape
            // 
            this.LB_LaneShape.FormattingEnabled = true;
            this.LB_LaneShape.ItemHeight = 12;
            this.LB_LaneShape.Location = new System.Drawing.Point(157, 12);
            this.LB_LaneShape.Name = "LB_LaneShape";
            this.LB_LaneShape.Size = new System.Drawing.Size(213, 268);
            this.LB_LaneShape.TabIndex = 2;
            // 
            // DebugShower
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 293);
            this.Controls.Add(this.LB_LaneShape);
            this.Controls.Add(this.LB_LaneNuber);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DebugShower";
            this.ShowInTaskbar = false;
            this.Text = "DebugShower";
            this.ResumeLayout(false);

        }

        #endregion
        public ListBox LB_LaneNuber;
        public ListBox LB_LaneShape;

    }
}