namespace KHJHLog
{
    partial class frmClassOrder
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grdClassOrder = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.colSchool = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClassName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRealNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEstNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLockComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdClassOrder)).BeginInit();
            this.SuspendLayout();
            // 
            // grdClassOrder
            // 
            this.grdClassOrder.BackgroundColor = System.Drawing.Color.White;
            this.grdClassOrder.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdClassOrder.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSchool,
            this.colClassName,
            this.colRealNumber,
            this.colEstNumber,
            this.colOrder,
            this.colLock,
            this.colLockComment});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdClassOrder.DefaultCellStyle = dataGridViewCellStyle1;
            this.grdClassOrder.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.grdClassOrder.Location = new System.Drawing.Point(12, 12);
            this.grdClassOrder.Name = "grdClassOrder";
            this.grdClassOrder.ReadOnly = true;
            this.grdClassOrder.RowHeadersVisible = false;
            this.grdClassOrder.RowTemplate.Height = 24;
            this.grdClassOrder.Size = new System.Drawing.Size(748, 420);
            this.grdClassOrder.TabIndex = 0;
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.BackColor = System.Drawing.Color.Transparent;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(685, 444);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(75, 23);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.TabIndex = 1;
            this.buttonX1.Text = "查詢";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // colSchool
            // 
            this.colSchool.HeaderText = "學校";
            this.colSchool.Name = "colSchool";
            this.colSchool.ReadOnly = true;
            // 
            // colClassName
            // 
            this.colClassName.HeaderText = "班級名稱";
            this.colClassName.Name = "colClassName";
            this.colClassName.ReadOnly = true;
            // 
            // colRealNumber
            // 
            this.colRealNumber.HeaderText = "實際人數";
            this.colRealNumber.Name = "colRealNumber";
            this.colRealNumber.ReadOnly = true;
            // 
            // colEstNumber
            // 
            this.colEstNumber.HeaderText = "編班人數";
            this.colEstNumber.Name = "colEstNumber";
            this.colEstNumber.ReadOnly = true;
            // 
            // colOrder
            // 
            this.colOrder.HeaderText = "編班順位";
            this.colOrder.Name = "colOrder";
            this.colOrder.ReadOnly = true;
            // 
            // colLock
            // 
            this.colLock.HeaderText = "編班鎖定";
            this.colLock.Name = "colLock";
            this.colLock.ReadOnly = true;
            // 
            // colLockComment
            // 
            this.colLockComment.HeaderText = "鎖定備註";
            this.colLockComment.Name = "colLockComment";
            this.colLockComment.ReadOnly = true;
            // 
            // frmClassOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 473);
            this.Controls.Add(this.buttonX1);
            this.Controls.Add(this.grdClassOrder);
            this.MaximumSize = new System.Drawing.Size(780, 500);
            this.MinimumSize = new System.Drawing.Size(780, 500);
            this.Name = "frmClassOrder";
            this.Text = "查詢編班";
            this.TitleText = "查詢編班";
            ((System.ComponentModel.ISupportInitialize)(this.grdClassOrder)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX grdClassOrder;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSchool;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClassName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRealNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLock;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLockComment;
    }
}