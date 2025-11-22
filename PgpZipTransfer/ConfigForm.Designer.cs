namespace PgpZipTransfer;

public partial class ConfigForm : Form
{
    private TextBox txtPublicKey;
    private Button btnBrowsePublic;
    private TextBox txtPrivateKey;
    private Button btnBrowsePrivate;
    private TextBox txtPassphrase;
    private TextBox txtOutputFolder;
    private Button btnBrowseOutput;
    private Button btnSave;
    private Button btnCancel;
    private Label lblPublic;
    private Label lblPrivate;
    private Label lblPass;
    private Label lblOutput;

    private void InitializeComponent()
    {
        txtPublicKey = new TextBox();
        btnBrowsePublic = new Button();
        txtPrivateKey = new TextBox();
        btnBrowsePrivate = new Button();
        txtPassphrase = new TextBox();
        txtOutputFolder = new TextBox();
        btnBrowseOutput = new Button();
        btnSave = new Button();
        btnCancel = new Button();
        lblPublic = new Label();
        lblPrivate = new Label();
        lblPass = new Label();
        lblOutput = new Label();
        SuspendLayout();
        // Public Key
        lblPublic.Text = "Public Key";
        lblPublic.Location = new Point(12, 15);
        txtPublicKey.Location = new Point(100, 12);
        txtPublicKey.Size = new Size(360, 23);
        btnBrowsePublic.Text = "Browse";
        btnBrowsePublic.Location = new Point(466, 11);
        btnBrowsePublic.Size = new Size(75, 25);
        btnBrowsePublic.Click += btnBrowsePublic_Click;
        // Private Key
        lblPrivate.Text = "Private Key";
        lblPrivate.Location = new Point(12, 50);
        txtPrivateKey.Location = new Point(100, 47);
        txtPrivateKey.Size = new Size(360, 23);
        btnBrowsePrivate.Text = "Browse";
        btnBrowsePrivate.Location = new Point(466, 46);
        btnBrowsePrivate.Size = new Size(75, 25);
        btnBrowsePrivate.Click += btnBrowsePrivate_Click;
        // Passphrase
        lblPass.Text = "Passphrase";
        lblPass.Location = new Point(12, 85);
        txtPassphrase.Location = new Point(100, 82);
        txtPassphrase.Size = new Size(360, 23);
        txtPassphrase.PasswordChar = '*';
        // Output Folder
        lblOutput.Text = "Output";
        lblOutput.Location = new Point(12, 120);
        txtOutputFolder.Location = new Point(100, 117);
        txtOutputFolder.Size = new Size(360, 23);
        btnBrowseOutput.Text = "Browse";
        btnBrowseOutput.Location = new Point(466, 116);
        btnBrowseOutput.Size = new Size(75, 25);
        btnBrowseOutput.Click += btnBrowseOutput_Click;
        // Save / Cancel
        btnSave.Text = "Save";
        btnSave.Location = new Point(385, 160);
        btnSave.Size = new Size(75, 27);
        btnSave.Click += btnSave_Click;
        btnCancel.Text = "Cancel";
        btnCancel.Location = new Point(466, 160);
        btnCancel.Size = new Size(75, 27);
        btnCancel.Click += btnCancel_Click;
        // Form
        ClientSize = new Size(560, 210);
        Controls.AddRange(new Control[]{lblPublic, txtPublicKey, btnBrowsePublic, lblPrivate, txtPrivateKey, btnBrowsePrivate, lblPass, txtPassphrase, lblOutput, txtOutputFolder, btnBrowseOutput, btnSave, btnCancel});
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Text = "PGP Configuration";
        StartPosition = FormStartPosition.CenterParent;
        ResumeLayout(false);
        PerformLayout();
    }
}
