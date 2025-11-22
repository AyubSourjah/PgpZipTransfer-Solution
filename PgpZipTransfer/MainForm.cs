using PgpZipTransfer.Models;
using PgpZipTransfer.Persistence;
using PgpZipTransfer.Services;

namespace PgpZipTransfer;

public partial class MainForm : Form
{
    private readonly SettingsStore _settingsStore = new();
    private AppSettings _settings = new();
    private readonly ZipService _zipService = new();
    private readonly PgpEncryptionService _pgpService = new();

    public MainForm()
    {
        InitializeComponent();
        LoadSettings();
        ApplySettingsToUi();
    }

    private void LoadSettings() => _settings = _settingsStore.Load();
    private void ApplySettingsToUi() => txtSourceFolder.Text = _settings.SourceFolder ?? string.Empty;

    private void btnBrowseSource_Click(object sender, EventArgs e)
    {
        using var fbd = new FolderBrowserDialog();
        if (fbd.ShowDialog() == DialogResult.OK)
        {
            txtSourceFolder.Text = fbd.SelectedPath;
            _settings.SourceFolder = fbd.SelectedPath;
            if (string.IsNullOrWhiteSpace(_settings.OutputFolder))
            {
                _settings.OutputFolder = Path.Combine(_settings.SourceFolder!, "output");
            }
            _settingsStore.Save(_settings);
        }
    }

    private void btnConfigure_Click(object sender, EventArgs e)
    {
        using var cfg = new ConfigForm(_settings, _settingsStore);
        if (cfg.ShowDialog(this) == DialogResult.OK)
        {
            _settings = _settingsStore.Load();
        }
    }

    private async void btnStart_Click(object sender, EventArgs e)
    {
        var source = txtSourceFolder.Text;
        if (string.IsNullOrWhiteSpace(source) || !Directory.Exists(source))
        {
            MessageBox.Show("Select a valid source folder.");
            return;
        }
        if (string.IsNullOrWhiteSpace(_settings.PublicKeyPath) || !File.Exists(_settings.PublicKeyPath))
        {
            MessageBox.Show("Configure a valid public key first.");
            return;
        }
        string output = _settings.OutputFolder ?? Path.Combine(source, "output");
        Directory.CreateDirectory(output);

        progressBar.Value = 0;
        lblStatus.Text = "Zipping files...";
        ToggleUi(false);

        try
        {
            var allFiles = Directory.GetFiles(source, "*", SearchOption.AllDirectories)
                .Where(f => !f.StartsWith(output, StringComparison.OrdinalIgnoreCase)).ToList();
            if (!allFiles.Any())
            {
                MessageBox.Show("No files found to zip.");
                ToggleUi(true);
                return;
            }
            string zipPath = Path.Combine(output, $"archive_{DateTime.Now:yyyyMMdd_HHmmss}.zip");
            string pgpPath = zipPath + ".pgp";

            IProgress<int> overallProgress = new Progress<int>(p => progressBar.Value = p);
            var zipProgress = new Progress<int>(p => overallProgress.Report(Math.Min(80, p)));
            var encryptProgress = new Progress<int>(p => overallProgress.Report(80 + (p * 20 / 100)));

            await Task.Run(async () =>
            {
                await _zipService.ZipFolderAsync(source, zipPath, f => !f.StartsWith(output, StringComparison.OrdinalIgnoreCase), zipProgress);
                overallProgress.Report(80);
                lblStatus.Invoke(() => lblStatus.Text = "Encrypting...");
                await _pgpService.EncryptAndSignAsync(zipPath, pgpPath, _settings.PublicKeyPath!, _settings.PrivateKeyPath, _settings.Passphrase, encryptProgress);
            });

            progressBar.Value = 100;
            lblStatus.Text = "Completed";
            try { System.Diagnostics.Process.Start("explorer.exe", output); } catch { }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error: " + ex.Message);
        }
        finally
        {
            ToggleUi(true);
        }
    }

    private void ToggleUi(bool enabled)
    {
        btnBrowseSource.Enabled = enabled;
        btnConfigure.Enabled = enabled;
        btnStart.Enabled = enabled;
    }

    private void btnDecrypt_Click(object sender, EventArgs e)
    {
        using var f = new DecryptForm(_settings);
        f.ShowDialog(this);
    }
}
