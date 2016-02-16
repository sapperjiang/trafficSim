namespace GISTranSim.Data
{
    partial class MeanSpeedCharter
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
        	this.CHART_SpaceTime = new System.Windows.Forms.DataVisualization.Charting.Chart();
        	((System.ComponentModel.ISupportInitialize)(this.CHART_SpaceTime)).BeginInit();
        	this.SuspendLayout();
        	// 
        	// CHART_SpaceTime
        	// 
        	this.CHART_SpaceTime.BorderlineColor = System.Drawing.Color.Blue;
        	this.CHART_SpaceTime.BorderSkin.BackColor = System.Drawing.Color.Blue;
        	this.CHART_SpaceTime.BorderSkin.SkinStyle = System.Windows.Forms.DataVisualization.Charting.BorderSkinStyle.FrameThin4;
        	this.CHART_SpaceTime.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.CHART_SpaceTime.Location = new System.Drawing.Point(0, 0);
        	this.CHART_SpaceTime.Name = "CHART_SpaceTime";
        	this.CHART_SpaceTime.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Excel;
        	this.CHART_SpaceTime.Size = new System.Drawing.Size(619, 440);
        	this.CHART_SpaceTime.TabIndex = 1;
        	this.CHART_SpaceTime.Text = "chart1";
        	// 
        	// MeanSpeedCharter
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(619, 440);
        	this.Controls.Add(this.CHART_SpaceTime);
        	this.Name = "MeanSpeedCharter";
        	this.Text = "DataCharter";
        	((System.ComponentModel.ISupportInitialize)(this.CHART_SpaceTime)).EndInit();
        	this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart CHART_SpaceTime;

    }
}