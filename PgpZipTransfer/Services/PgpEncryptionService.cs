using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Security;

namespace PgpZipTransfer.Services;

public class PgpEncryptionService
{
    public async Task EncryptAndSignAsync(string inputFile, string outputFile, string publicKeyPath, string? privateKeyPath, string? passphrase, IProgress<int>? progress)
    {
        progress?.Report(0);
        using var publicKeyStream = File.OpenRead(publicKeyPath);
        var pubKey = ReadPublicKey(publicKeyStream);

        PgpSecretKey? secretKey = null;
        PgpPrivateKey? privateKey = null;
        if (!string.IsNullOrWhiteSpace(privateKeyPath) && File.Exists(privateKeyPath) && !string.IsNullOrWhiteSpace(passphrase))
        {
            using var secretIn = File.OpenRead(privateKeyPath);
            secretKey = ReadSecretKey(secretIn);
            privateKey = secretKey.ExtractPrivateKey(passphrase.ToCharArray());
        }

        using var outStream = File.Create(outputFile);
        var encGen = new PgpEncryptedDataGenerator(Org.BouncyCastle.Bcpg.SymmetricKeyAlgorithmTag.Cast5, true, new SecureRandom());
        encGen.AddMethod(pubKey);
        using var cOut = encGen.Open(outStream, new byte[1 << 16]);

        PgpSignatureGenerator? sGen = null;
        if (privateKey != null && secretKey != null)
        {
            sGen = new PgpSignatureGenerator(secretKey.PublicKey.Algorithm, Org.BouncyCastle.Bcpg.HashAlgorithmTag.Sha256);
            sGen.InitSign(PgpSignature.BinaryDocument, privateKey);
            foreach (string userId in secretKey.PublicKey.GetUserIds())
            {
                var subPacket = new PgpSignatureSubpacketGenerator();
                subPacket.SetSignerUserId(false, userId);
                sGen.SetHashedSubpackets(subPacket.Generate());
                break;
            }
            sGen.GenerateOnePassVersion(false).Encode(cOut);
        }

        var compGen = new PgpCompressedDataGenerator(Org.BouncyCastle.Bcpg.CompressionAlgorithmTag.Zip);
        using var compOut = compGen.Open(cOut);

        using var fIn = File.OpenRead(inputFile);
        var litGen = new PgpLiteralDataGenerator();
        using var pOut = litGen.Open(compOut, PgpLiteralData.Binary, Path.GetFileName(inputFile), fIn.Length, DateTime.UtcNow);

        byte[] buf = new byte[1 << 16];
        int read;
        long total = fIn.Length;
        long done = 0;
        while ((read = await fIn.ReadAsync(buf.AsMemory(0, buf.Length))) > 0)
        {
            await pOut.WriteAsync(buf.AsMemory(0, read));
            done += read;
            progress?.Report(total == 0 ? 100 : (int)(done * 100 / total));
            if (sGen != null)
            {
                sGen.Update(buf, 0, read);
            }
        }
        if (sGen != null)
        {
            sGen.Generate().Encode(compOut);
        }
        progress?.Report(100);
    }

    private PgpPublicKey ReadPublicKey(Stream input)
    {
        var pgpPub = new PgpPublicKeyRingBundle(PgpUtilities.GetDecoderStream(input));
        foreach (PgpPublicKeyRing kRing in pgpPub.GetKeyRings())
        {
            foreach (PgpPublicKey k in kRing.GetPublicKeys())
            {
                if (k.IsEncryptionKey)
                    return k;
            }
        }
        throw new Exception("No encryption key found in public key ring.");
    }

    private PgpSecretKey ReadSecretKey(Stream input)
    {
        var pgpSec = new PgpSecretKeyRingBundle(PgpUtilities.GetDecoderStream(input));
        foreach (PgpSecretKeyRing kRing in pgpSec.GetKeyRings())
        {
            foreach (PgpSecretKey k in kRing.GetSecretKeys())
            {
                if (k.IsSigningKey)
                    return k;
            }
        }
        throw new Exception("No signing key found in secret key ring.");
    }
}
