namespace SolarApi.IO.Extensions;

public static class FileInfoExtensions
{
    public static byte[] ReadAllBytes(this FileInfo file)
        => File.ReadAllBytes(file.FullName);

    public static string ReadAllText(this FileInfo file)
        => File.ReadAllText(file.FullName);
}
