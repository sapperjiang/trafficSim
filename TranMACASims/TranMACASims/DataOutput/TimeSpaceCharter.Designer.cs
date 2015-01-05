namespace GISTranSim.Data
{
    partial class TimeSpaceCharter
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
            this._spaceTimeChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this._spaceTimeChart)).BeginInit();
            this.SuspendLayout();
            // 
            // CHART_SpaceTime
            // 
            this._spaceTimeChart.BorderlineColor = System.Drawing.Color.Blue;
            this._spaceTimeChart.BorderSkin.BackColor = System.Drawing.Color.Blue;
            this._spaceTimeChart.BorderSkin.SkinStyle = System.Windows.Forms.DataVisualization.Charting.BorderSkinStyle.FrameThin4;
            this._spaceTimeChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this._spaceTimeChart.Location = new System.Drawing.Point(0, 0);
            this._spaceTimeChart.Name = "CHART_SpaceTime";
            this._spaceTimeChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Excel;
            this._spaceTimeChart.Size = new System.Drawing.Size(619, 440);
            this._spaceTimeChart.TabIndex = 1;
            this._spaceTimeChart.Text = "chart1";
            // 
            // DataCharter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 440);
            this.Controls.Add(this._spaceTimeChart);
            this.Name = "DataCharter";
            this.Text = "DataCharter";
            ((System.ComponentModel.ISupportInitialize)(this._spaceTimeChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart _spaceTimeChart;

    }
}