namespace TimeBinTracker.App;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
        activityMonitor1 = new ActivityMonitor();
        rtbChart = new RichTextBox();
        btnOpenLogFolder = new Button();
        btnUpload = new Button();
        lblUploadResult = new Label();
        cbStartWithWindows = new CheckBox();
        groupBox1 = new GroupBox();
        groupBox1.SuspendLayout();
        SuspendLayout();
        // 
        // activityMonitor1
        // 
        activityMonitor1.Location = new Point(12, 12);
        activityMonitor1.Name = "activityMonitor1";
        activityMonitor1.Size = new Size(286, 126);
        activityMonitor1.TabIndex = 4;
        // 
        // rtbChart
        // 
        rtbChart.BackColor = SystemColors.Control;
        rtbChart.BorderStyle = BorderStyle.None;
        rtbChart.Dock = DockStyle.Fill;
        rtbChart.Font = new Font("Consolas", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
        rtbChart.Location = new Point(10, 30);
        rtbChart.Name = "rtbChart";
        rtbChart.Size = new Size(663, 122);
        rtbChart.TabIndex = 5;
        rtbChart.Text = "";
        // 
        // btnOpenLogFolder
        // 
        btnOpenLogFolder.Location = new Point(304, 64);
        btnOpenLogFolder.Name = "btnOpenLogFolder";
        btnOpenLogFolder.Size = new Size(150, 29);
        btnOpenLogFolder.TabIndex = 6;
        btnOpenLogFolder.Text = "Open Log Folder";
        btnOpenLogFolder.UseVisualStyleBackColor = true;
        // 
        // btnUpload
        // 
        btnUpload.Location = new Point(304, 109);
        btnUpload.Name = "btnUpload";
        btnUpload.Size = new Size(150, 29);
        btnUpload.TabIndex = 8;
        btnUpload.Text = "Upload Now";
        btnUpload.UseVisualStyleBackColor = true;
        // 
        // lblUploadResult
        // 
        lblUploadResult.AutoSize = true;
        lblUploadResult.Location = new Point(460, 113);
        lblUploadResult.Name = "lblUploadResult";
        lblUploadResult.Size = new Size(67, 20);
        lblUploadResult.TabIndex = 9;
        lblUploadResult.Text = "waiting...";
        // 
        // cbStartWithWindows
        // 
        cbStartWithWindows.AutoSize = true;
        cbStartWithWindows.Location = new Point(307, 23);
        cbStartWithWindows.Name = "cbStartWithWindows";
        cbStartWithWindows.Size = new Size(159, 24);
        cbStartWithWindows.TabIndex = 10;
        cbStartWithWindows.Text = "Start with Windows";
        cbStartWithWindows.UseVisualStyleBackColor = true;
        // 
        // groupBox1
        // 
        groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        groupBox1.Controls.Add(rtbChart);
        groupBox1.Location = new Point(12, 144);
        groupBox1.Name = "groupBox1";
        groupBox1.Padding = new Padding(10);
        groupBox1.Size = new Size(683, 162);
        groupBox1.TabIndex = 11;
        groupBox1.TabStop = false;
        groupBox1.Text = "Today's Activity";
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(707, 318);
        Controls.Add(groupBox1);
        Controls.Add(cbStartWithWindows);
        Controls.Add(lblUploadResult);
        Controls.Add(btnUpload);
        Controls.Add(btnOpenLogFolder);
        Controls.Add(activityMonitor1);
        Icon = (Icon)resources.GetObject("$this.Icon");
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Time Bin Tracker";
        groupBox1.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private RichTextBox rtbPayload;
    private ActivityMonitor activityMonitor1;
    private RichTextBox rtbChart;
    private Button btnOpenLogFolder;
    private Button btnUpload;
    private Label lblUploadResult;
    private CheckBox cbStartWithWindows;
    private GroupBox groupBox1;
}
