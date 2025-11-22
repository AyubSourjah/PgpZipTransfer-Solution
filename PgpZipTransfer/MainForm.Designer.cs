namespace PgpZipTransfer;

public partial class MainForm : Form
{
    private TextBox txtSourceFolder;
    private Button btnBrowseSource;
    private Button btnConfigure;
    private Button btnStart;
    private ProgressBar progressBar;
    private Label lblStatus;
    private Button btnDecrypt;
    private Button btnCancel;

    private void InitializeComponent()
    {
        txtSourceFolder = new TextBox();
        btnBrowseSource = new Button();
        btnConfigure = new Button();
        btnStart = new Button();
        progressBar = new ProgressBar();
        btnDecrypt = new Button();
        btnCancel = new Button();
        lblStatus = new Label();
        SuspendLayout();
        // 
        // txtSourceFolder
        // 
        txtSourceFolder.Location = new Point(12, 15);
        txtSourceFolder.Name = "txtSourceFolder";
        txtSourceFolder.Size = new Size(520, 23);
        txtSourceFolder.TabIndex = 5;
        // 
        // btnBrowseSource
        // 
        btnBrowseSource.Location = new Point(538, 14);
        btnBrowseSource.Name = "btnBrowseSource";
        btnBrowseSource.Size = new Size(75, 25);
        btnBrowseSource.TabIndex = 4;
        btnBrowseSource.Text = "Browse";
        btnBrowseSource.Click += btnBrowseSource_Click;
        // 
        // btnConfigure
        // 
        btnConfigure.Location = new Point(619, 14);
        btnConfigure.Name = "btnConfigure";
        btnConfigure.Size = new Size(80, 25);
        btnConfigure.TabIndex = 3;
        btnConfigure.Text = "Config";
        btnConfigure.Click += btnConfigure_Click;
        // 
        // btnStart
        // 
        btnStart.Location = new Point(705, 14);
        btnStart.Name = "btnStart";
        btnStart.Size = new Size(75, 25);
        btnStart.TabIndex = 2;
        btnStart.Text = "Run";
        btnStart.Click += btnStart_Click;
        // 
        // progressBar
        // 
        progressBar.Location = new Point(12, 60);
        progressBar.Name = "progressBar";
        progressBar.Size = new Size(768, 23);
        progressBar.TabIndex = 1;
        // 
        // btnDecrypt
        // 
        btnDecrypt.Location = new Point(12, 100);
        btnDecrypt.Name = "btnDecrypt";
        btnDecrypt.Size = new Size(90, 25);
        btnDecrypt.TabIndex = 6;
        btnDecrypt.Text = "Decrypt";
        btnDecrypt.Click += btnDecrypt_Click;
        // 
        // btnCancel
        // 
        btnCancel.Location = new Point(108, 100);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(90, 25);
        btnCancel.TabIndex = 7;
        btnCancel.Text = "Cancel";
        btnCancel.Click += btnCancel_Click;
        // 
        // lblStatus
        // 
        lblStatus.Location = new Point(204, 100);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(768, 23);
        lblStatus.TabIndex = 0;
        lblStatus.Text = "Idle";
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(792, 150);
        Controls.Add(lblStatus);
        Controls.Add(btnDecrypt);
        Controls.Add(btnCancel);
        Controls.Add(progressBar);
        Controls.Add(btnStart);
        Controls.Add(btnConfigure);
        Controls.Add(btnBrowseSource);
        Controls.Add(txtSourceFolder);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "PeoplesHR PGP Zip Transfer - Internal Use ONLY!";
        ResumeLayout(false);
        PerformLayout();
    }
}
