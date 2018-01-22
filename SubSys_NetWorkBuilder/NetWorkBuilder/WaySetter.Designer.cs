namespace SubSys_NetworkBuilder
{
    partial class WaySetter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WaySetter));
            this.TBLaneCount = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TBWayName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TBLength = new System.Windows.Forms.NumericUpDown();
            this.TBWayNumber = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.BTConfirm = new System.Windows.Forms.Button();
            this.BTConcel = new System.Windows.Forms.Button();
            this.CBCreateReverseWay = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.TBLaneCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TBLength)).BeginInit();
            this.SuspendLayout();
            // 
            // TBLaneCount
            // 
            this.TBLaneCount.Location = new System.Drawing.Point(80, 121);
            this.TBLaneCount.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.TBLaneCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TBLaneCount.Name = "TBLaneCount";
            this.TBLaneCount.Size = new System.Drawing.Size(86, 21);
            this.TBLaneCount.TabIndex = 0;
            this.TBLaneCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 130);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "车道数：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "道路名称：";
            // 
            // TBWayName
            // 
            this.TBWayName.Location = new System.Drawing.Point(81, 68);
            this.TBWayName.Name = "TBWayName";
            this.TBWayName.Size = new System.Drawing.Size(86, 21);
            this.TBWayName.TabIndex = 3;
            this.TBWayName.Text = "兴旺路";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(183, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "长度(m):";
            // 
            // TBLength
            // 
            this.TBLength.Location = new System.Drawing.Point(242, 68);
            this.TBLength.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.TBLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TBLength.Name = "TBLength";
            this.TBLength.Size = new System.Drawing.Size(86, 21);
            this.TBLength.TabIndex = 5;
            this.TBLength.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // TBWayNumber
            // 
            this.TBWayNumber.Enabled = false;
            this.TBWayNumber.Location = new System.Drawing.Point(81, 39);
            this.TBWayNumber.Name = "TBWayNumber";
            this.TBWayNumber.Size = new System.Drawing.Size(86, 21);
            this.TBWayNumber.TabIndex = 7;
            this.TBWayNumber.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "道路编号：";
            // 
            // BTConfirm
            // 
            this.BTConfirm.Location = new System.Drawing.Point(253, 287);
            this.BTConfirm.Name = "BTConfirm";
            this.BTConfirm.Size = new System.Drawing.Size(75, 23);
            this.BTConfirm.TabIndex = 8;
            this.BTConfirm.Text = "确定";
            this.BTConfirm.UseVisualStyleBackColor = true;
            this.BTConfirm.Click += new System.EventHandler(this.BTConfirm_Click);
            // 
            // BTConcel
            // 
            this.BTConcel.Location = new System.Drawing.Point(21, 287);
            this.BTConcel.Name = "BTConcel";
            this.BTConcel.Size = new System.Drawing.Size(75, 23);
            this.BTConcel.TabIndex = 9;
            this.BTConcel.Text = "取消";
            this.BTConcel.UseVisualStyleBackColor = true;
            this.BTConcel.Click += new System.EventHandler(this.BTCancel_Click);
            // 
            // CBCreateReverseWay
            // 
            this.CBCreateReverseWay.AutoSize = true;
            this.CBCreateReverseWay.Checked = true;
            this.CBCreateReverseWay.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBCreateReverseWay.Location = new System.Drawing.Point(242, 125);
            this.CBCreateReverseWay.Name = "CBCreateReverseWay";
            this.CBCreateReverseWay.Size = new System.Drawing.Size(108, 16);
            this.CBCreateReverseWay.TabIndex = 10;
            this.CBCreateReverseWay.Text = "创建相反的道路";
            this.CBCreateReverseWay.UseVisualStyleBackColor = true;
            // 
            // WaySetter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 340);
            this.Controls.Add(this.CBCreateReverseWay);
            this.Controls.Add(this.BTConcel);
            this.Controls.Add(this.BTConfirm);
            this.Controls.Add(this.TBWayNumber);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TBLength);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TBWayName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TBLaneCount);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WaySetter";
            this.Text = "设置路段属性";
            this.Load += new System.EventHandler(this.WaySetter_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TBLaneCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TBLength)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.NumericUpDown TBLaneCount;
        internal System.Windows.Forms.TextBox TBWayName;
        internal System.Windows.Forms.NumericUpDown TBLength;
        internal System.Windows.Forms.TextBox TBWayNumber;
        internal System.Windows.Forms.CheckBox CBCreateReverseWay;

        private System.Windows.Forms.Button BTConfirm;
        private System.Windows.Forms.Button BTConcel;

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
      
    }
}