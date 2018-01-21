namespace TrafficSim
{
    partial class SimConfig
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
        	this.BT_ConFirm = new System.Windows.Forms.Button();
        	this.TB_CarCount = new System.Windows.Forms.TextBox();
        	this.LB_CarCount = new System.Windows.Forms.Label();
        	this.LB_RoadLength = new System.Windows.Forms.Label();
        	this.TB_RoadLength = new System.Windows.Forms.TextBox();
        	this.BT_Concel = new System.Windows.Forms.Button();
        	this.label1 = new System.Windows.Forms.Label();
        	this.TB_SimInterval = new System.Windows.Forms.TextBox();
        	this.label2 = new System.Windows.Forms.Label();
        	this.TB_Ratio = new System.Windows.Forms.TextBox();
        	this.SuspendLayout();
        	// 
        	// BT_ConFirm
        	// 
        	this.BT_ConFirm.Location = new System.Drawing.Point(22, 190);
        	this.BT_ConFirm.Name = "BT_ConFirm";
        	this.BT_ConFirm.Size = new System.Drawing.Size(75, 23);
        	this.BT_ConFirm.TabIndex = 0;
        	this.BT_ConFirm.Text = "确定";
        	this.BT_ConFirm.UseVisualStyleBackColor = true;
        	this.BT_ConFirm.Click += new System.EventHandler(this.BT_ConFirm_Click);
        	// 
        	// TB_CarCount
        	// 
        	this.TB_CarCount.Location = new System.Drawing.Point(101, 27);
        	this.TB_CarCount.Name = "TB_CarCount";
        	this.TB_CarCount.Size = new System.Drawing.Size(100, 21);
        	this.TB_CarCount.TabIndex = 1;
        	this.TB_CarCount.Text = "40";
        	// 
        	// LB_CarCount
        	// 
        	this.LB_CarCount.AutoSize = true;
        	this.LB_CarCount.Location = new System.Drawing.Point(32, 30);
        	this.LB_CarCount.Name = "LB_CarCount";
        	this.LB_CarCount.Size = new System.Drawing.Size(53, 12);
        	this.LB_CarCount.TabIndex = 2;
        	this.LB_CarCount.Text = "车辆数：";
        	// 
        	// LB_RoadLength
        	// 
        	this.LB_RoadLength.AutoSize = true;
        	this.LB_RoadLength.Location = new System.Drawing.Point(32, 69);
        	this.LB_RoadLength.Name = "LB_RoadLength";
        	this.LB_RoadLength.Size = new System.Drawing.Size(65, 12);
        	this.LB_RoadLength.TabIndex = 4;
        	this.LB_RoadLength.Text = "路段长度：";
        	// 
        	// TB_RoadLength
        	// 
        	this.TB_RoadLength.Location = new System.Drawing.Point(103, 66);
        	this.TB_RoadLength.Name = "TB_RoadLength";
        	this.TB_RoadLength.Size = new System.Drawing.Size(100, 21);
        	this.TB_RoadLength.TabIndex = 3;
        	this.TB_RoadLength.Text = "125";
        	// 
        	// BT_Concel
        	// 
        	this.BT_Concel.Location = new System.Drawing.Point(126, 190);
        	this.BT_Concel.Name = "BT_Concel";
        	this.BT_Concel.Size = new System.Drawing.Size(75, 23);
        	this.BT_Concel.TabIndex = 5;
        	this.BT_Concel.Text = "取消";
        	this.BT_Concel.UseVisualStyleBackColor = true;
        	this.BT_Concel.Click += new System.EventHandler(this.BT_ConcelClick);
        	// 
        	// label1
        	// 
        	this.label1.AutoSize = true;
        	this.label1.Location = new System.Drawing.Point(32, 106);
        	this.label1.Name = "label1";
        	this.label1.Size = new System.Drawing.Size(65, 12);
        	this.label1.TabIndex = 7;
        	this.label1.Text = "仿真速度：";
        	// 
        	// TB_SimInterval
        	// 
        	this.TB_SimInterval.Location = new System.Drawing.Point(100, 102);
        	this.TB_SimInterval.Name = "TB_SimInterval";
        	this.TB_SimInterval.Size = new System.Drawing.Size(100, 21);
        	this.TB_SimInterval.TabIndex = 6;
        	this.TB_SimInterval.Text = "100";
        	// 
        	// label2
        	// 
        	this.label2.AutoSize = true;
        	this.label2.Location = new System.Drawing.Point(32, 147);
        	this.label2.Name = "label2";
        	this.label2.Size = new System.Drawing.Size(65, 12);
        	this.label2.TabIndex = 9;
        	this.label2.Text = "漫化概率：";
        	// 
        	// TB_Ratio
        	// 
        	this.TB_Ratio.Location = new System.Drawing.Point(100, 143);
        	this.TB_Ratio.Name = "TB_Ratio";
        	this.TB_Ratio.Size = new System.Drawing.Size(100, 21);
        	this.TB_Ratio.TabIndex = 8;
        	this.TB_Ratio.Text = "0.85";
        	// 
        	// CarSetUp
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(274, 256);
        	this.Controls.Add(this.label2);
        	this.Controls.Add(this.TB_Ratio);
        	this.Controls.Add(this.label1);
        	this.Controls.Add(this.TB_SimInterval);
        	this.Controls.Add(this.BT_Concel);
        	this.Controls.Add(this.LB_RoadLength);
        	this.Controls.Add(this.TB_RoadLength);
        	this.Controls.Add(this.LB_CarCount);
        	this.Controls.Add(this.TB_CarCount);
        	this.Controls.Add(this.BT_ConFirm);
        	this.Name = "CarSetUp";
        	this.Text = "仿真参数设置";
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button BT_ConFirm;
        private System.Windows.Forms.TextBox TB_CarCount;
        private System.Windows.Forms.Label LB_CarCount;
        private System.Windows.Forms.Label LB_RoadLength;
        private System.Windows.Forms.TextBox TB_RoadLength;
        private System.Windows.Forms.Button BT_Concel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TB_SimInterval;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TB_Ratio;

    }
}