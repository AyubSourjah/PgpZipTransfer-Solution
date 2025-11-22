# PGP Zip Transfer

## Overview
`PGP Zip Transfer` is a .NET 8 Windows Forms utility that:
1. Zips all files in a selected source folder.
2. Optionally signs (if private key + passphrase provided) and encrypts the zip with a PGP public key.
3. Writes the resulting `.zip.pgp` file to an output folder.
4. Opens Windows File Explorer to the output folder on completion.
5. Persists user configuration between runs.

Assembly is strong-name signed.

## Prerequisites
- Windows with .NET 8 runtime (or SDK if you want to build). 
- PGP public key file (ASCII armored or binary) containing an encryption key.
- (Optional) PGP private/secret key + passphrase for signing.

## Build
```bash
# From solution root
dotnet build
```
Run either via Visual Studio (F5) or:
```bash
dotnet run --project PgpZipTransfer
```

## First-Time Use
1. Start the application.
2. Click `Browse` and select the source folder whose contents you want to package.
3. Click `Config` to open the configuration dialog.
4. In the configuration dialog:
   - Set `Public Key` (Browse to your `.asc` / `.pgp` public key file).
   - (Optional) Set `Private Key` and `Passphrase` if signing is required.
   - (Optional) Choose `Output` folder (defaults to `<SourceFolder>\output`).
   - Click `Save`.
5. Back on the main window click `Run`.

## What Happens During Run
- All files under the selected source folder (recursively) except anything inside the chosen output folder are added to a zip archive.
- A progress bar shows up to 80% during zipping.
- At 80% it switches status to encrypt/sign and progresses to 100%.
- Resulting files:
  - `archive_YYYYMMDD_HHMMSS.zip`
  - `archive_YYYYMMDD_HHMMSS.zip.pgp` (encrypted, optionally signed)
- Windows Explorer opens pointing at the output folder.

## Configuration Persistence
Settings are stored as JSON in:
```
%AppData%\PgpZipTransfer\settings.json
```
Fields:
- `SourceFolder`
- `OutputFolder`
- `PublicKeyPath`
- `PrivateKeyPath` (optional)
- `Passphrase` (optional)

## Signing Behavior
If both `PrivateKeyPath` and `Passphrase` are supplied and the secret key contains a signing key, the encrypted file is signed (one-pass signature + final signature packet). If omitted, only encryption is performed.

## Key File Formats
- Public/secret key files may be ASCII armored (`-----BEGIN PGP PUBLIC KEY BLOCK-----`) or binary exported keyring files. The app scans the keyring for the first suitable key.

## Exclusions
- Files inside the output folder are ignored during zipping.
- No explicit file-type exclusions; add a dedicated staging folder if you need filtering.

## Error Handling
- Missing source folder / keys results in a message box and abort.
- General exceptions display a message box with the error message.

## Troubleshooting
| Issue | Cause | Resolution |
|-------|-------|------------|
| "Configure a valid public key" | Public key not set or path invalid | Open `Config`, set correct file path |
| Output folder contains previous results | Normal behavior | Clear manually if desired |
| Signature missing | Private key or passphrase not provided, or key lacks signing capability | Provide both; verify key supports signing |
| Encryption fails | Key format or unsupported key type | Ensure key is valid, try re-exporting ASCII armored |

## Operational Notes
- Progress percentages are approximate (80% zipping / 20% encrypt & sign).
- Large files: encryption chunk size is 64KB; performance depends on disk and CPU.
- Strong-name signing of assembly does not imply Authenticode / code-signing certificate.

## Extensibility Suggestions
- Add file filters (extensions, size limits).
- Add logging (e.g., to `%AppData%/PgpZipTransfer/log.txt`).
- Add decryption/verification feature.
- Add cancellation token to stop long operations.

## Security Considerations
- Passphrase is stored in plain JSON today (not secure). For production encrypt or protect it (DPAPI, Windows Credential Manager, etc.).
- Output folder permissions should be restricted if sensitive data is processed.

## Uninstall / Cleanup
Delete:
- Application deployment folder (if self-contained build published).
- `%AppData%\PgpZipTransfer` directory for settings.

## Support Checklist for Implementation Engineer
1. Confirm .NET 8 installed.
2. Obtain correct PGP key material (encryption + optional signing key).
3. Validate key paths accessible by running user.
4. Verify output folder write permissions.
5. Run test with small source set; validate resulting `.zip.pgp` can be decrypted externally.
6. Move to production usage.
