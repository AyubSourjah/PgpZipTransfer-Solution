namespace PgpZipTransfer.Models;

public class AppSettings
{
    public string? SourceFolder { get; set; }
    public string? OutputFolder { get; set; }
    public string? PublicKeyPath { get; set; }
    public string? PrivateKeyPath { get; set; }
    public string? Passphrase { get; set; }
}
