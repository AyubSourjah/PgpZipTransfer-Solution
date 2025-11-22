# PGP Zip Transfer - Quick Use Guide

## Start
1. Ensure .NET 8 runtime is installed.
2. Have your PGP public key file ready (and optional private key + passphrase if you want signing).
3. Launch the application (from Visual Studio F5, or run the built EXE in `bin/Debug/net8.0-windows`).

## Configure
1. Click "Browse" and select the folder whose files you want to package.
2. Click "Config".
3. In the dialog:
   - Public Key: Browse to your public key file.
   - (Optional) Private Key + Passphrase: add if you want the output signed.
   - Output: Pick a folder (or accept the default `<SourceFolder>\output`).
   - Click "Save".

## Run
1. Click "Run" on the main window.
2. Progress bar shows zipping then encryption/signing.
3. When finished, Explorer opens the output folder.
4. You will see: `archive_YYYYMMDD_HHMMSS.zip.pgp` (and the original zip).

## Repeat Usage
- Source folder and key paths are remembered.
- Just adjust files in the source folder and click "Run" again.

## Common Messages
- "Select a valid source folder": Choose a real folder before running.
- "Configure a valid public key": Open Config and set the public key file.

## Completion
- Provide the `.zip.pgp` file to the receiving party.

That’s all—select folder, configure keys once, click Run.