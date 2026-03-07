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
        button1 = new Button();
        richTextBox2 = new RichTextBox();
        activityMonitor1 = new ActivityMonitor();
        richTextBox3 = new RichTextBox();
        btnOpenLogFolder = new Button();
        SuspendLayout();
        // 
        // button1
        // 
        button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        button1.Location = new Point(551, 564);
        button1.Name = "button1";
        button1.Size = new Size(129, 42);
        button1.TabIndex = 2;
        button1.Text = "Upload Now";
        button1.UseVisualStyleBackColor = true;
        // 
        // richTextBox2
        // 
        richTextBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        richTextBox2.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        richTextBox2.Location = new Point(12, 354);
        richTextBox2.Name = "richTextBox2";
        richTextBox2.Size = new Size(668, 204);
        richTextBox2.TabIndex = 3;
        richTextBox2.Text = "";
        // 
        // activityMonitor1
        // 
        activityMonitor1.Location = new Point(12, 12);
        activityMonitor1.Name = "activityMonitor1";
        activityMonitor1.Size = new Size(286, 126);
        activityMonitor1.TabIndex = 4;
        // 
        // richTextBox3
        // 
        richTextBox3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        richTextBox3.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        richTextBox3.Location = new Point(12, 144);
        richTextBox3.Name = "richTextBox3";
        richTextBox3.Size = new Size(668, 204);
        richTextBox3.TabIndex = 5;
        richTextBox3.Text = "";
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
        // Form1
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(692, 621);
        Controls.Add(btnOpenLogFolder);
        Controls.Add(richTextBox3);
        Controls.Add(activityMonitor1);
        Controls.Add(richTextBox2);
        Controls.Add(button1);
        Icon = (Icon)resources.GetObject("$this.Icon");
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Time Bin Tracker";
        Load += Form1_Load;
        ResumeLayout(false);
    }

    #endregion
    private Button button1;
    private RichTextBox richTextBox2;
    private ActivityMonitor activityMonitor1;
    private RichTextBox richTextBox3;
    private Button btnOpenLogFolder;
}
