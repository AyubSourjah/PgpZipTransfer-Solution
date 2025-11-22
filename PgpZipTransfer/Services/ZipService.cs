namespace PgpZipTransfer.Services;

using System.IO.Compression;

public class ZipService
{
    public async Task ZipFolderAsync(string sourceFolder, string zipPath, Func<string,bool>? includePredicate, IProgress<int>? progress, CancellationToken token, LoggingService logger)
    {
        var files = Directory.GetFiles(sourceFolder, "*", SearchOption.AllDirectories)
            .Where(f => includePredicate?.Invoke(f) ?? true).ToList();
        int total = files.Count;
        logger.LogInfo($"Zipping {total} file(s) from {sourceFolder} to {zipPath}");
        if (File.Exists(zipPath)) File.Delete(zipPath);
        using var fs = new FileStream(zipPath, FileMode.Create, FileAccess.Write);
        using var archive = new ZipArchive(fs, ZipArchiveMode.Create);
        int processed = 0;
        foreach (var file in files)
        {
            token.ThrowIfCancellationRequested();
            var entryName = Path.GetRelativePath(sourceFolder, file);
            var entry = archive.CreateEntry(entryName, CompressionLevel.Optimal);
            using (var entryStream = entry.Open())
            using (var inFile = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                await inFile.CopyToAsync(entryStream, token);
            }
            processed++;
            if (processed % 10 == 0) logger.LogInfo($"Zipped {processed}/{total} files...");
            progress?.Report(total == 0 ? 0 : processed * 100 / Math.Max(1,total));
        }
        logger.LogInfo("Zipping complete");
    }
}
