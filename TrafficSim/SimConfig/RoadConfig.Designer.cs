namespace TrafficSim
{
    partial class RoadConfig
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
        	this.TB_RoadName = new System.Windows.Forms.TextBox();
        	this.LB_CarCount = new System.Windows.Forms.Label();
        	this.LB_RoadLength = new System.Windows.Forms.Label();
        	this.TB_RoadLength = new System.Windows.Forms.TextBox();
        	this.BT_Concel = new System.Windows.Forms.Button();
        	this.label1 = new System.Windows.Forms.Label();
        	this.TB_LeftLaneCount = new System.Windows.Forms.TextBox();
        	this.label2 = new System.Windows.Forms.Label();
        	this.TB_StraightLaneCount = new System.Windows.Forms.TextBox();
        	this.TB_RightLaneCount = new System.Windows.Forms.TextBox();
        	this.label3 = new System.Windows.Forms.Label();
        	this.SuspendLayout();
        	// 
        	// BT_ConFirm
        	// 
        	this.BT_ConFirm.Location = new System.Drawing.Point(32, 221);
        	this.BT_ConFirm.Name = "BT_ConFirm";
        	this.BT_ConFirm.Size = new System.Drawing.Size(75, 23);
        	this.BT_ConFirm.TabIndex = 0;
        	this.BT_ConFirm.Text = "确定";
        	this.BT_ConFirm.UseVisualStyleBackColor = true;
        	this.BT_ConFirm.Click += new System.EventHandler(this.BT_ConFirm_Click);
        	// 
        	// TB_RoadName
        	// 
        	this.TB_RoadName.Location = new System.Drawing.Point(129, 21);
        	this.TB_RoadName.Name = "TB_RoadName";
        	this.TB_RoadName.Size = new System.Drawing.Size(100, 21);
        	this.TB_RoadName.TabIndex = 1;
        	this.TB_RoadName.Text = "北京路";
        	// 
        	// LB_CarCount
        	// 
        	this.LB_CarCount.AutoSize = true;
        	this.LB_CarCount.Location = new System.Drawing.Point(32, 24);
        	this.LB_CarCount.Name = "LB_CarCount";
        	this.LB_CarCount.Size = new System.Drawing.Size(65, 12);
        	this.LB_CarCount.TabIndex = 2;
        	this.LB_CarCount.Text = "道路名称：";
        	// 
        	// LB_RoadLength
        	// 
        	this.LB_RoadLength.AutoSize = true;
        	this.LB_RoadLength.Location = new System.Drawing.Point(32, 69);
        	this.LB_RoadLength.Name = "LB_RoadLength";
        	this.LB_RoadLength.Size = new System.Drawing.Size(95, 12);
        	this.LB_RoadLength.TabIndex = 4;
        	this.LB_RoadLength.Text = "路段长度（m）：";
        	// 
        	// TB_RoadLength
        	// 
        	this.TB_RoadLength.Location = new System.Drawing.Point(129, 60);
        	this.TB_RoadLength.Name = "TB_RoadLength";
        	this.TB_RoadLength.Size = new System.Drawing.Size(100, 21);
        	this.TB_RoadLength.TabIndex = 3;
        	this.TB_RoadLength.Text = "125";
        	// 
        	// BT_Concel
        	// 
        	this.BT_Concel.Location = new System.Drawing.Point(154, 221);
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
        	this.label1.Size = new System.Drawing.Size(77, 12);
        	this.label1.TabIndex = 7;
        	this.label1.Text = "左转车道数：";
        	// 
        	// TB_LeftLaneCount
        	// 
        	this.TB_LeftLaneCount.Location = new System.Drawing.Point(129, 97);
        	this.TB_LeftLaneCount.Name = "TB_LeftLaneCount";
        	this.TB_LeftLaneCount.Size = new System.Drawing.Size(100, 21);
        	this.TB_LeftLaneCount.TabIndex = 6;
        	this.TB_LeftLaneCount.Text = "1";
        	// 
        	// label2
        	// 
        	this.label2.AutoSize = true;
        	this.label2.Location = new System.Drawing.Point(32, 147);
        	this.label2.Name = "label2";
        	this.label2.Size = new System.Drawing.Size(77, 12);
        	this.label2.TabIndex = 9;
        	this.label2.Text = "直行车道数：";
        	// 
        	// TB_StraightLaneCount
        	// 
        	this.TB_StraightLaneCount.Location = new System.Drawing.Point(129, 138);
        	this.TB_StraightLaneCount.Name = "TB_StraightLaneCount";
        	this.TB_StraightLaneCount.Size = new System.Drawing.Size(100, 21);
        	this.TB_StraightLaneCount.TabIndex = 8;
        	this.TB_StraightLaneCount.Text = "1";
        	// 
        	// TB_RightLaneCount
        	// 
        	this.TB_RightLaneCount.Location = new System.Drawing.Point(129, 176);
        	this.TB_RightLaneCount.Name = "TB_RightLaneCount";
        	this.TB_RightLaneCount.Size = new System.Drawing.Size(100, 21);
        	this.TB_RightLaneCount.TabIndex = 10;
        	this.TB_RightLaneCount.Text = "1";
        	// 
        	// label3
        	// 
        	this.label3.AutoSize = true;
        	this.label3.Location = new System.Drawing.Point(32, 182);
        	this.label3.Name = "label3";
        	this.label3.Size = new System.Drawing.Size(77, 12);
        	this.label3.TabIndex = 11;
        	this.label3.Text = "右转车道数：";
        	// 
        	// RoadConfig
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(274, 256);
        	this.Controls.Add(this.label3);
        	this.Controls.Add(this.TB_RightLaneCount);
        	this.Controls.Add(this.label2);
        	this.Controls.Add(this.TB_StraightLaneCount);
        	this.Controls.Add(this.label1);
        	this.Controls.Add(this.TB_LeftLaneCount);
        	this.Controls.Add(this.BT_Concel);
        	this.Controls.Add(this.LB_RoadLength);
        	this.Controls.Add(this.TB_RoadLength);
        	this.Controls.Add(this.LB_CarCount);
        	this.Controls.Add(this.TB_RoadName);
        	this.Controls.Add(this.BT_ConFirm);
        	this.Name = "RoadConfig";
        	this.Text = "道路设置（单向）";
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }
        private System.Windows.Forms.TextBox TB_RoadName;
        private System.Windows.Forms.TextBox TB_RightLaneCount;
        private System.Windows.Forms.Label label3;

        #endregion

        private System.Windows.Forms.Button BT_ConFirm;
        private System.Windows.Forms.Label LB_CarCount;
        private System.Windows.Forms.Label LB_RoadLength;
        private System.Windows.Forms.TextBox TB_RoadLength;
        private System.Windows.Forms.Button BT_Concel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TB_LeftLaneCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TB_StraightLaneCount;

    }
}