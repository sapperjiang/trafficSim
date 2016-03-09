namespace GISTranSim
{
    partial class ODInOutMgr
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
        	this.DGV_ODData = new System.Windows.Forms.DataGridView();
        	this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
        	this.BT_LoadOD = new System.Windows.Forms.Button();
        	this.BT_Concel = new System.Windows.Forms.Button();
        	this.BT_SaveOD = new System.Windows.Forms.Button();
        	((System.ComponentModel.ISupportInitialize)(this.DGV_ODData)).BeginInit();
        	this.flowLayoutPanel1.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// DGV_ODData
        	// 
        	this.DGV_ODData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        	this.DGV_ODData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        	this.DGV_ODData.Location = new System.Drawing.Point(3, 3);
        	this.DGV_ODData.Name = "DGV_ODData";
        	this.DGV_ODData.RowTemplate.Height = 23;
        	this.DGV_ODData.Size = new System.Drawing.Size(547, 346);
        	this.DGV_ODData.TabIndex = 0;
        	// 
        	// flowLayoutPanel1
        	// 
        	this.flowLayoutPanel1.Controls.Add(this.DGV_ODData);
        	this.flowLayoutPanel1.Controls.Add(this.BT_LoadOD);
        	this.flowLayoutPanel1.Controls.Add(this.BT_Concel);
        	this.flowLayoutPanel1.Controls.Add(this.BT_SaveOD);
        	this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
        	this.flowLayoutPanel1.Name = "flowLayoutPanel1";
        	this.flowLayoutPanel1.Size = new System.Drawing.Size(550, 391);
        	this.flowLayoutPanel1.TabIndex = 1;
        	// 
        	// BT_LoadOD
        	// 
        	this.BT_LoadOD.Location = new System.Drawing.Point(3, 355);
        	this.BT_LoadOD.Name = "BT_LoadOD";
        	this.BT_LoadOD.Size = new System.Drawing.Size(75, 23);
        	this.BT_LoadOD.TabIndex = 1;
        	// 
        	// BT_Concel
        	// 
        	this.BT_Concel.Location = new System.Drawing.Point(84, 355);
        	this.BT_Concel.Name = "BT_Concel";
        	this.BT_Concel.Size = new System.Drawing.Size(75, 23);
        	this.BT_Concel.TabIndex = 1;
        	this.BT_Concel.Text = "(x)取消";
        	this.BT_Concel.UseVisualStyleBackColor = true;
        	this.BT_Concel.Click += new System.EventHandler(this.BT_Concel_Click);
        	// 
        	// BT_SaveOD
        	// 
        	this.BT_SaveOD.Location = new System.Drawing.Point(165, 355);
        	this.BT_SaveOD.Name = "BT_SaveOD";
        	this.BT_SaveOD.Size = new System.Drawing.Size(75, 23);
        	this.BT_SaveOD.TabIndex = 2;
        	this.BT_SaveOD.Text = "(s)保存";
        	this.BT_SaveOD.UseVisualStyleBackColor = true;
        	this.BT_SaveOD.Click += new System.EventHandler(this.BT_SaveOD_Click);
        	// 
        	// ODInOutMgr
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(550, 391);
        	this.Controls.Add(this.flowLayoutPanel1);
        	this.Name = "ODInOutMgr";
        	this.Text = "仿真数据输出";
        	((System.ComponentModel.ISupportInitialize)(this.DGV_ODData)).EndInit();
        	this.flowLayoutPanel1.ResumeLayout(false);
        	this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView DGV_ODData;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button BT_LoadOD;
        private System.Windows.Forms.Button BT_Concel;
        private System.Windows.Forms.Button BT_SaveOD;
    }
}