namespace Mastersign.DashOps.Exceptions;

internal class UnsupportedProjectFormatException : Exception
{
    public string FormatVersion { get; private set; }

    public UnsupportedProjectFormatException()
    {
    }

    public UnsupportedProjectFormatException(string version)
        : base($"Unsupported project format version: {version}")
    {
        FormatVersion = version;
    }
}
