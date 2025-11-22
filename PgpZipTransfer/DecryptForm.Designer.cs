namespace PgpZipTransfer;

public partial class DecryptForm : Form
{
    private TextBox txtEncryptedFile;
    private Button btnBrowseEncrypted;
    private Button btnDecrypt;
    private ProgressBar progressBar;
    private Label lblStatus;

    private void InitializeComponent()
    {
        txtEncryptedFile = new TextBox();
        btnBrowseEncrypted = new Button();
        btnDecrypt = new Button();
        progressBar = new ProgressBar();
        lblStatus = new Label();
        SuspendLayout();
        // txtEncryptedFile
        txtEncryptedFile.Location = new Point(12, 15);
        txtEncryptedFile.Size = new Size(360, 23);
        // btnBrowseEncrypted
        btnBrowseEncrypted.Location = new Point(378, 14);
        btnBrowseEncrypted.Size = new Size(75, 25);
        btnBrowseEncrypted.Text = "Browse";
        btnBrowseEncrypted.Click += btnBrowseEncrypted_Click;
        // btnDecrypt
        btnDecrypt.Location = new Point(459, 14);
        btnDecrypt.Size = new Size(75, 25);
        btnDecrypt.Text = "Decrypt";
        btnDecrypt.Click += btnDecrypt_Click;
        // progressBar
        progressBar.Location = new Point(12, 55);
        progressBar.Size = new Size(522, 23);
        // lblStatus
        lblStatus.Location = new Point(12, 85);
        lblStatus.Size = new Size(522, 23);
        lblStatus.Text = "Idle";
        // Form
        ClientSize = new Size(550, 120);
        Controls.Add(lblStatus);
        Controls.Add(progressBar);
        Controls.Add(btnDecrypt);
        Controls.Add(btnBrowseEncrypted);
        Controls.Add(txtEncryptedFile);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Text = "Decrypt PGP File";
        StartPosition = FormStartPosition.CenterParent;
        ResumeLayout(false);
        PerformLayout();
    }
}
