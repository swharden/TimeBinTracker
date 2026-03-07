namespace TimeBinTracker.App;

partial class ActivityMonitor
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        groupBox1 = new GroupBox();
        label2 = new Label();
        label1 = new Label();
        label3 = new Label();
        tableLayoutPanel1 = new TableLayoutPanel();
        groupBox1.SuspendLayout();
        tableLayoutPanel1.SuspendLayout();
        SuspendLayout();
        // 
        // groupBox1
        // 
        groupBox1.Controls.Add(tableLayoutPanel1);
        groupBox1.Dock = DockStyle.Fill;
        groupBox1.Location = new Point(0, 0);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new Size(285, 126);
        groupBox1.TabIndex = 0;
        groupBox1.TabStop = false;
        groupBox1.Text = "Activity Monitor";
        // 
        // label2
        // 
        label2.Dock = DockStyle.Fill;
        label2.Location = new Point(3, 62);
        label2.Name = "label2";
        label2.Size = new Size(267, 32);
        label2.TabIndex = 1;
        label2.Text = "Status...";
        label2.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // label1
        // 
        label1.Dock = DockStyle.Fill;
        label1.Location = new Point(3, 31);
        label1.Name = "label1";
        label1.Size = new Size(267, 31);
        label1.TabIndex = 0;
        label1.Text = "Checking...";
        label1.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // label3
        // 
        label3.Dock = DockStyle.Fill;
        label3.Location = new Point(3, 0);
        label3.Name = "label3";
        label3.Size = new Size(267, 31);
        label3.TabIndex = 2;
        label3.Text = "Checking...";
        label3.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        tableLayoutPanel1.ColumnCount = 1;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanel1.Controls.Add(label3, 0, 0);
        tableLayoutPanel1.Controls.Add(label2, 0, 2);
        tableLayoutPanel1.Controls.Add(label1, 0, 1);
        tableLayoutPanel1.Location = new Point(6, 26);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 3;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel1.Size = new Size(273, 94);
        tableLayoutPanel1.TabIndex = 3;
        // 
        // ActivityMonitor
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(groupBox1);
        Name = "ActivityMonitor";
        Size = new Size(285, 126);
        groupBox1.ResumeLayout(false);
        tableLayoutPanel1.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private GroupBox groupBox1;
    private Label label2;
    private Label label1;
    private TableLayoutPanel tableLayoutPanel1;
    private Label label3;
}
