namespace Dmnk.Blazor.InteractiveTests.Tests;

internal sealed class TemporaryDirectory : IDisposable
{
    public DirectoryInfo Directory { get; }

    public TemporaryDirectory()
    {
        Directory = new DirectoryInfo(Path.Combine(
            Path.GetTempPath(),
            "Dmnk.Blazor.InteractiveTests.Tests",
            Guid.NewGuid().ToString("N")));

        Directory.Create();
    }

    public FileInfo CreateFile(string relativePath, string contents = "")
    {
        var file = new FileInfo(Path.Combine(Directory.FullName, relativePath));
        file.Directory?.Create();
        File.WriteAllText(file.FullName, contents);
        return file;
    }

    public DirectoryInfo CreateDirectory(string relativePath)
    {
        var directory = new DirectoryInfo(Path.Combine(Directory.FullName, relativePath));
        directory.Create();
        return directory;
    }

    public void Dispose()
    {
        if (Directory.Exists)
            Directory.Delete(recursive: true);
    }
}
