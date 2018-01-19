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
            this.DataView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.DataView)).BeginInit();
            this.SuspendLayout();
            // 
            // DebugDataView
            // 
            this.DataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataView.Location = new System.Drawing.Point(12, 12);
            this.DataView.Name = "DebugDataView";
            this.DataView.RowTemplate.Height = 23;
            this.DataView.Size = new System.Drawing.Size(450, 422);
            this.DataView.TabIndex = 0;
            // 
            // DebugShower
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 446);
            this.Controls.Add(this.DataView);
            this.Name = "DebugShower";
            this.Text = "DebugShower";
            ((System.ComponentModel.ISupportInitialize)(this.DataView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataView;

        public DataGridView DataView
        {
            get
            {
                return dataView;
            }

            set
            {
                dataView = value;
            }
        }
    }
}