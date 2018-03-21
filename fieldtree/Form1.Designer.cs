namespace fieldtree
{
    partial class Form1
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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tbWidth = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSetExtent = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbImportance = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnINNNext = new System.Windows.Forms.Button();
            this.rdbActionIncrNN = new System.Windows.Forms.RadioButton();
            this.rdbActionMove = new System.Windows.Forms.RadioButton();
            this.rdbActionWindowQuery = new System.Windows.Forms.RadioButton();
            this.rdbRangeQuery = new System.Windows.Forms.RadioButton();
            this.rdbActionDelete = new System.Windows.Forms.RadioButton();
            this.rdbActionFind = new System.Windows.Forms.RadioButton();
            this.rdbActionsAdd = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbCreationMode = new System.Windows.Forms.ComboBox();
            this.chbOverFlow = new System.Windows.Forms.CheckBox();
            this.tb_pValue = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbCapacity = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rdbCover = new System.Windows.Forms.RadioButton();
            this.rdbPartition = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.tbStopwatch = new System.Windows.Forms.TextBox();
            this.tbNumRects = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.tbRandSeed = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.dbCanvas = new fieldtree.CanvasControl();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbWidth
            // 
            this.tbWidth.Location = new System.Drawing.Point(42, 89);
            this.tbWidth.Name = "tbWidth";
            this.tbWidth.Size = new System.Drawing.Size(58, 20);
            this.tbWidth.TabIndex = 1;
            this.tbWidth.Text = "512";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Size";
            // 
            // btnSetExtent
            // 
            this.btnSetExtent.Location = new System.Drawing.Point(12, 239);
            this.btnSetExtent.Name = "btnSetExtent";
            this.btnSetExtent.Size = new System.Drawing.Size(75, 23);
            this.btnSetExtent.TabIndex = 8;
            this.btnSetExtent.Text = "Set";
            this.btnSetExtent.UseVisualStyleBackColor = true;
            this.btnSetExtent.Click += new System.EventHandler(this.btnSetExtent_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 305);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Importance (Min Layer)";
            // 
            // tbImportance
            // 
            this.tbImportance.Location = new System.Drawing.Point(132, 302);
            this.tbImportance.Name = "tbImportance";
            this.tbImportance.Size = new System.Drawing.Size(65, 20);
            this.tbImportance.TabIndex = 21;
            this.tbImportance.Text = "100";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.btnINNNext);
            this.groupBox2.Controls.Add(this.rdbActionIncrNN);
            this.groupBox2.Controls.Add(this.rdbActionMove);
            this.groupBox2.Controls.Add(this.rdbActionWindowQuery);
            this.groupBox2.Controls.Add(this.rdbRangeQuery);
            this.groupBox2.Controls.Add(this.rdbActionDelete);
            this.groupBox2.Controls.Add(this.rdbActionFind);
            this.groupBox2.Controls.Add(this.rdbActionsAdd);
            this.groupBox2.Location = new System.Drawing.Point(11, 345);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(333, 228);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Actions";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(39, 191);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(246, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Left click to select the source, then click Find Next";
            // 
            // btnINNNext
            // 
            this.btnINNNext.Enabled = false;
            this.btnINNNext.Location = new System.Drawing.Point(121, 165);
            this.btnINNNext.Name = "btnINNNext";
            this.btnINNNext.Size = new System.Drawing.Size(75, 23);
            this.btnINNNext.TabIndex = 7;
            this.btnINNNext.Text = "Find Next";
            this.btnINNNext.UseVisualStyleBackColor = true;
            this.btnINNNext.Click += new System.EventHandler(this.btnINNNext_Click);
            // 
            // rdbActionIncrNN
            // 
            this.rdbActionIncrNN.AutoSize = true;
            this.rdbActionIncrNN.Location = new System.Drawing.Point(14, 168);
            this.rdbActionIncrNN.Name = "rdbActionIncrNN";
            this.rdbActionIncrNN.Size = new System.Drawing.Size(99, 17);
            this.rdbActionIncrNN.TabIndex = 6;
            this.rdbActionIncrNN.Text = "Incremental NN";
            this.rdbActionIncrNN.UseVisualStyleBackColor = true;
            this.rdbActionIncrNN.CheckedChanged += new System.EventHandler(this.rdbActionIncrNN_CheckedChanged);
            // 
            // rdbActionMove
            // 
            this.rdbActionMove.AutoSize = true;
            this.rdbActionMove.Location = new System.Drawing.Point(14, 145);
            this.rdbActionMove.Name = "rdbActionMove";
            this.rdbActionMove.Size = new System.Drawing.Size(52, 17);
            this.rdbActionMove.TabIndex = 5;
            this.rdbActionMove.Text = "Move";
            this.rdbActionMove.UseVisualStyleBackColor = true;
            this.rdbActionMove.CheckedChanged += new System.EventHandler(this.rdbActionMove_CheckedChanged);
            // 
            // rdbActionWindowQuery
            // 
            this.rdbActionWindowQuery.AutoSize = true;
            this.rdbActionWindowQuery.Location = new System.Drawing.Point(14, 122);
            this.rdbActionWindowQuery.Name = "rdbActionWindowQuery";
            this.rdbActionWindowQuery.Size = new System.Drawing.Size(194, 17);
            this.rdbActionWindowQuery.TabIndex = 4;
            this.rdbActionWindowQuery.Text = "Window Query (Find Contained Obj)";
            this.rdbActionWindowQuery.UseVisualStyleBackColor = true;
            this.rdbActionWindowQuery.CheckedChanged += new System.EventHandler(this.rdbActionWindowQuery_CheckedChanged);
            // 
            // rdbRangeQuery
            // 
            this.rdbRangeQuery.AutoSize = true;
            this.rdbRangeQuery.Location = new System.Drawing.Point(14, 99);
            this.rdbRangeQuery.Name = "rdbRangeQuery";
            this.rdbRangeQuery.Size = new System.Drawing.Size(187, 17);
            this.rdbRangeQuery.TabIndex = 3;
            this.rdbRangeQuery.Text = "Range Query (Find Contained Obj)";
            this.rdbRangeQuery.UseVisualStyleBackColor = true;
            this.rdbRangeQuery.CheckedChanged += new System.EventHandler(this.rdbRangeQuery_CheckedChanged);
            // 
            // rdbActionDelete
            // 
            this.rdbActionDelete.AutoSize = true;
            this.rdbActionDelete.Location = new System.Drawing.Point(14, 53);
            this.rdbActionDelete.Name = "rdbActionDelete";
            this.rdbActionDelete.Size = new System.Drawing.Size(108, 17);
            this.rdbActionDelete.TabIndex = 2;
            this.rdbActionDelete.Text = "Delete Rectangle";
            this.rdbActionDelete.UseVisualStyleBackColor = true;
            this.rdbActionDelete.CheckedChanged += new System.EventHandler(this.rdbActionDelete_CheckedChanged);
            // 
            // rdbActionFind
            // 
            this.rdbActionFind.AutoSize = true;
            this.rdbActionFind.Location = new System.Drawing.Point(14, 76);
            this.rdbActionFind.Name = "rdbActionFind";
            this.rdbActionFind.Size = new System.Drawing.Size(147, 17);
            this.rdbActionFind.TabIndex = 1;
            this.rdbActionFind.Text = "Point Query (Find nearest)";
            this.rdbActionFind.UseVisualStyleBackColor = true;
            this.rdbActionFind.CheckedChanged += new System.EventHandler(this.rdbActionFind_CheckedChanged);
            // 
            // rdbActionsAdd
            // 
            this.rdbActionsAdd.AutoSize = true;
            this.rdbActionsAdd.Checked = true;
            this.rdbActionsAdd.Location = new System.Drawing.Point(14, 30);
            this.rdbActionsAdd.Name = "rdbActionsAdd";
            this.rdbActionsAdd.Size = new System.Drawing.Size(96, 17);
            this.rdbActionsAdd.TabIndex = 0;
            this.rdbActionsAdd.TabStop = true;
            this.rdbActionsAdd.Text = "Add Rectangle";
            this.rdbActionsAdd.UseVisualStyleBackColor = true;
            this.rdbActionsAdd.CheckedChanged += new System.EventHandler(this.rdbActionsAdd_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cmbCreationMode);
            this.groupBox1.Controls.Add(this.chbOverFlow);
            this.groupBox1.Controls.Add(this.tb_pValue);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbCapacity);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.rdbCover);
            this.groupBox1.Controls.Add(this.rdbPartition);
            this.groupBox1.Controls.Add(this.tbWidth);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnSetExtent);
            this.groupBox1.Location = new System.Drawing.Point(11, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(355, 277);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 196);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Children Creation";
            // 
            // cmbCreationMode
            // 
            this.cmbCreationMode.FormattingEnabled = true;
            this.cmbCreationMode.Items.AddRange(new object[] {
            "Lazy",
            "All"});
            this.cmbCreationMode.Location = new System.Drawing.Point(106, 193);
            this.cmbCreationMode.Name = "cmbCreationMode";
            this.cmbCreationMode.Size = new System.Drawing.Size(121, 21);
            this.cmbCreationMode.TabIndex = 16;
            // 
            // chbOverFlow
            // 
            this.chbOverFlow.AutoSize = true;
            this.chbOverFlow.Location = new System.Drawing.Point(12, 160);
            this.chbOverFlow.Name = "chbOverFlow";
            this.chbOverFlow.Size = new System.Drawing.Size(96, 17);
            this.chbOverFlow.TabIndex = 15;
            this.chbOverFlow.Text = "Allow Overflow";
            this.chbOverFlow.UseVisualStyleBackColor = true;
            // 
            // tb_pValue
            // 
            this.tb_pValue.Location = new System.Drawing.Point(193, 55);
            this.tb_pValue.Name = "tb_pValue";
            this.tb_pValue.Size = new System.Drawing.Size(58, 20);
            this.tb_pValue.TabIndex = 14;
            this.tb_pValue.Text = "0.3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(144, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "p Value";
            // 
            // tbCapacity
            // 
            this.tbCapacity.Location = new System.Drawing.Point(88, 124);
            this.tbCapacity.Name = "tbCapacity";
            this.tbCapacity.Size = new System.Drawing.Size(58, 20);
            this.tbCapacity.TabIndex = 12;
            this.tbCapacity.Text = "2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Field Capacity";
            // 
            // rdbCover
            // 
            this.rdbCover.AutoSize = true;
            this.rdbCover.Location = new System.Drawing.Point(12, 54);
            this.rdbCover.Name = "rdbCover";
            this.rdbCover.Size = new System.Drawing.Size(103, 17);
            this.rdbCover.TabIndex = 10;
            this.rdbCover.Text = "Cover Field Tree";
            this.rdbCover.UseVisualStyleBackColor = true;
            // 
            // rdbPartition
            // 
            this.rdbPartition.AutoSize = true;
            this.rdbPartition.Checked = true;
            this.rdbPartition.Location = new System.Drawing.Point(12, 31);
            this.rdbPartition.Name = "rdbPartition";
            this.rdbPartition.Size = new System.Drawing.Size(113, 17);
            this.rdbPartition.TabIndex = 9;
            this.rdbPartition.TabStop = true;
            this.rdbPartition.Text = "Partition Field Tree";
            this.rdbPartition.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 582);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(102, 13);
            this.label6.TabIndex = 26;
            this.label6.Text = "Execution Time (ms)";
            // 
            // tbStopwatch
            // 
            this.tbStopwatch.Location = new System.Drawing.Point(121, 579);
            this.tbStopwatch.Name = "tbStopwatch";
            this.tbStopwatch.ReadOnly = true;
            this.tbStopwatch.Size = new System.Drawing.Size(244, 20);
            this.tbStopwatch.TabIndex = 25;
            // 
            // tbNumRects
            // 
            this.tbNumRects.Location = new System.Drawing.Point(15, 622);
            this.tbNumRects.Name = "tbNumRects";
            this.tbNumRects.Size = new System.Drawing.Size(97, 20);
            this.tbNumRects.TabIndex = 28;
            this.tbNumRects.Text = "1000";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 606);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 13);
            this.label7.TabIndex = 27;
            this.label7.Text = "Number Rectangles";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 648);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(173, 23);
            this.button1.TabIndex = 29;
            this.button1.Text = "Add Many Random Rectangles";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbRandSeed
            // 
            this.tbRandSeed.Location = new System.Drawing.Point(131, 622);
            this.tbRandSeed.Name = "tbRandSeed";
            this.tbRandSeed.Size = new System.Drawing.Size(97, 20);
            this.tbRandSeed.TabIndex = 31;
            this.tbRandSeed.Text = "1000";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(128, 606);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 13);
            this.label8.TabIndex = 30;
            this.label8.Text = "Random Seed";
            // 
            // dbCanvas
            // 
            this.dbCanvas.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.dbCanvas.Location = new System.Drawing.Point(376, 13);
            this.dbCanvas.Name = "dbCanvas";
            this.dbCanvas.Size = new System.Drawing.Size(800, 800);
            this.dbCanvas.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 821);
            this.Controls.Add(this.tbRandSeed);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbNumRects);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbStopwatch);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbImportance);
            this.Controls.Add(this.dbCanvas);
            this.Name = "Form1";
            this.Text = "Form1";
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private CanvasControl dbCanvas;
        private System.Windows.Forms.TextBox tbWidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSetExtent;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbImportance;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdbActionDelete;
        private System.Windows.Forms.RadioButton rdbActionFind;
        private System.Windows.Forms.RadioButton rdbActionsAdd;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdbCover;
        private System.Windows.Forms.RadioButton rdbPartition;
        private System.Windows.Forms.TextBox tb_pValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbCapacity;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chbOverFlow;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbCreationMode;
        private System.Windows.Forms.RadioButton rdbRangeQuery;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbStopwatch;
        private System.Windows.Forms.TextBox tbNumRects;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tbRandSeed;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RadioButton rdbActionIncrNN;
        private System.Windows.Forms.RadioButton rdbActionMove;
        private System.Windows.Forms.RadioButton rdbActionWindowQuery;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnINNNext;
    }
}

