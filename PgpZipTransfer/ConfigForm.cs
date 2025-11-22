using PgpZipTransfer.Models;
using PgpZipTransfer.Persistence;

namespace PgpZipTransfer;

public partial class ConfigForm : Form
{
    private readonly AppSettings _settings;
    private readonly SettingsStore _store;
    public ConfigForm(AppSettings settings, SettingsStore store)
    {
        _settings = settings;
        _store = store;
        InitializeComponent();
        Apply();
    }

    private void Apply()
    {
        txtPublicKey.Text = _settings.PublicKeyPath ?? string.Empty;
        txtPrivateKey.Text = _settings.PrivateKeyPath ?? string.Empty;
        txtPassphrase.Text = _settings.Passphrase ?? string.Empty;
        txtOutputFolder.Text = _settings.OutputFolder ?? string.Empty;
    }

    private void btnBrowsePublic_Click(object sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog();
        if (ofd.ShowDialog() == DialogResult.OK)
        {
            txtPublicKey.Text = ofd.FileName;
        }
    }

    private void btnBrowsePrivate_Click(object sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog();
        if (ofd.ShowDialog() == DialogResult.OK)
        {
            txtPrivateKey.Text = ofd.FileName;
        }
    }

    private void btnBrowseOutput_Click(object sender, EventArgs e)
    {
        using var fbd = new FolderBrowserDialog();
        if (fbd.ShowDialog() == DialogResult.OK)
        {
            txtOutputFolder.Text = fbd.SelectedPath;
        }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        _settings.PublicKeyPath = txtPublicKey.Text;
        _settings.PrivateKeyPath = string.IsNullOrWhiteSpace(txtPrivateKey.Text) ? null : txtPrivateKey.Text;
        _settings.Passphrase = string.IsNullOrWhiteSpace(txtPassphrase.Text) ? null : txtPassphrase.Text;
        _settings.OutputFolder = string.IsNullOrWhiteSpace(txtOutputFolder.Text) ? null : txtOutputFolder.Text;
        _store.Save(_settings);
        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
