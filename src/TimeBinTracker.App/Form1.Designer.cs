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
        rtbPayload = new RichTextBox();
        activityMonitor1 = new ActivityMonitor();
        rtbChart = new RichTextBox();
        btnOpenLogFolder = new Button();
        btnUpdateChart = new Button();
        btnUpload = new Button();
        SuspendLayout();
        // 
        // rtbPayload
        // 
        rtbPayload.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        rtbPayload.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        rtbPayload.Location = new Point(12, 354);
        rtbPayload.Name = "rtbPayload";
        rtbPayload.Size = new Size(683, 216);
        rtbPayload.TabIndex = 3;
        rtbPayload.Text = "";
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
        rtbChart.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        rtbChart.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        rtbChart.Location = new Point(12, 144);
        rtbChart.Name = "rtbChart";
        rtbChart.Size = new Size(683, 204);
        rtbChart.TabIndex = 5;
        rtbChart.Text = "";
        // 
        // btnOpenLogFolder
        // 
        btnOpenLogFolder.Location = new Point(304, 21);
        btnOpenLogFolder.Name = "btnOpenLogFolder";
        btnOpenLogFolder.Size = new Size(150, 29);
        btnOpenLogFolder.TabIndex = 6;
        btnOpenLogFolder.Text = "Open Log Folder";
        btnOpenLogFolder.UseVisualStyleBackColor = true;
        // 
        // btnUpdateChart
        // 
        btnUpdateChart.Location = new Point(304, 56);
        btnUpdateChart.Name = "btnUpdateChart";
        btnUpdateChart.Size = new Size(150, 29);
        btnUpdateChart.TabIndex = 7;
        btnUpdateChart.Text = "Update Chart";
        btnUpdateChart.UseVisualStyleBackColor = true;
        // 
        // btnUpload
        // 
        btnUpload.Location = new Point(304, 91);
        btnUpload.Name = "btnUpload";
        btnUpload.Size = new Size(150, 29);
        btnUpload.TabIndex = 8;
        btnUpload.Text = "Upload Now";
        btnUpload.UseVisualStyleBackColor = true;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(707, 582);
        Controls.Add(btnUpload);
        Controls.Add(btnUpdateChart);
        Controls.Add(btnOpenLogFolder);
        Controls.Add(rtbChart);
        Controls.Add(activityMonitor1);
        Controls.Add(rtbPayload);
        Icon = (Icon)resources.GetObject("$this.Icon");
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Time Bin Tracker";
        ResumeLayout(false);
    }

    #endregion
    private RichTextBox rtbPayload;
    private ActivityMonitor activityMonitor1;
    private RichTextBox rtbChart;
    private Button btnOpenLogFolder;
    private Button btnUpdateChart;
    private Button btnUpload;
}
