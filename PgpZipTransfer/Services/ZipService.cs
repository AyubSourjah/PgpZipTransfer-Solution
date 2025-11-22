namespace PgpZipTransfer.Services;

using System.IO.Compression;

public class ZipService
{
    public async Task ZipFolderAsync(string sourceFolder, string zipPath, Func<string,bool>? includePredicate, IProgress<int>? progress)
    {
        var files = Directory.GetFiles(sourceFolder, "*", SearchOption.AllDirectories)
            .Where(f => includePredicate?.Invoke(f) ?? true).ToList();
        int total = files.Count;
        if (File.Exists(zipPath)) File.Delete(zipPath);
        using var fs = new FileStream(zipPath, FileMode.Create, FileAccess.Write);
        using var archive = new ZipArchive(fs, ZipArchiveMode.Create);
        int processed = 0;
        foreach (var file in files)
        {
            var entryName = Path.GetRelativePath(sourceFolder, file);
            var entry = archive.CreateEntry(entryName, CompressionLevel.Optimal);
            using (var entryStream = entry.Open())
            using (var inFile = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                await inFile.CopyToAsync(entryStream);
            }
            processed++;
            progress?.Report(total == 0 ? 0 : processed * 100 / Math.Max(1,total));
        }
    }
}
