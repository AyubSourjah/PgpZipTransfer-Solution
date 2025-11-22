using PgpZipTransfer.Models;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace PgpZipTransfer;

public partial class DecryptForm : Form
{
    private readonly AppSettings _settings;

    public DecryptForm(AppSettings settings)
    {
        _settings = settings;
        InitializeComponent();
    }

    private void btnBrowseEncrypted_Click(object sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog();
        ofd.Filter = "PGP Files (*.pgp)|*.pgp|All Files (*.*)|*.*";
        if (ofd.ShowDialog() == DialogResult.OK)
        {
            txtEncryptedFile.Text = ofd.FileName;
        }
    }

    private async void btnDecrypt_Click(object sender, EventArgs e)
    {
        var encPath = txtEncryptedFile.Text;
        if (!File.Exists(encPath))
        {
            MessageBox.Show("Select a valid encrypted file.");
            return;
        }
        if (string.IsNullOrWhiteSpace(_settings.PrivateKeyPath) || !File.Exists(_settings.PrivateKeyPath) || string.IsNullOrWhiteSpace(_settings.Passphrase))
        {
            MessageBox.Show("Private key and passphrase required for decryption.");
            return;
        }

        btnDecrypt.Enabled = false;
        progressBar.Value = 0;
        lblStatus.Text = "Decrypting...";

        try
        {
            var outDir = Path.Combine(Path.GetDirectoryName(encPath)!, "decrypted");
            Directory.CreateDirectory(outDir);
            var outFile = Path.Combine(outDir, Path.GetFileNameWithoutExtension(encPath));
            await Task.Run(() => Decrypt(encPath, outFile, _settings.PrivateKeyPath!, _settings.Passphrase!));
            progressBar.Value = 100;
            lblStatus.Text = "Done";
            try { System.Diagnostics.Process.Start("explorer.exe", outDir); } catch { }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error: " + ex.Message);
        }
        finally
        {
            btnDecrypt.Enabled = true;
        }
    }

    private void Decrypt(string inputFile, string outputFile, string secretKeyPath, string passphrase)
    {
        using var fs = File.OpenRead(inputFile);
        using var keyIn = File.OpenRead(secretKeyPath);
        var decoderStream = PgpUtilities.GetDecoderStream(fs);
        var pgpObjFactory = new PgpObjectFactory(decoderStream);
        PgpObject? obj = pgpObjFactory.NextPgpObject();

        if (obj is PgpEncryptedDataList encList)
        {
            PgpSecretKeyRingBundle secretKeyRingBundle = new(PgpUtilities.GetDecoderStream(keyIn));
            PgpPrivateKey? privKey = null;
            PgpPublicKeyEncryptedData? encData = null;
            foreach (PgpPublicKeyEncryptedData pked in encList.GetEncryptedDataObjects())
            {
                var secretKey = secretKeyRingBundle.GetSecretKey(pked.KeyId);
                if (secretKey != null)
                {
                    privKey = secretKey.ExtractPrivateKey(passphrase.ToCharArray());
                    encData = pked;
                    break;
                }
            }
            if (privKey == null || encData == null)
                throw new Exception("Matching private key not found for decryption.");

            using var clear = encData.GetDataStream(privKey);
            var plainFact = new PgpObjectFactory(clear);
            PgpObject? message = plainFact.NextPgpObject();
            if (message is PgpCompressedData comp)
            {
                var compStream = comp.GetDataStream();
                plainFact = new PgpObjectFactory(compStream);
                message = plainFact.NextPgpObject();
            }
            if (message is PgpLiteralData lit)
            {
                using var outFile = File.Create(outputFile);
                using var inLit = lit.GetInputStream();
                inLit.CopyTo(outFile);
            }
            else
            {
                throw new Exception("Unexpected PGP message type.");
            }

            if (encData.IsIntegrityProtected() && !encData.Verify())
                throw new Exception("Integrity check failed.");
        }
        else
        {
            throw new Exception("No encrypted data found.");
        }
    }
}
