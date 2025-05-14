namespace Mastersign.DashOps.Exceptions;

internal class ProjectLoadException : Exception
{
    public string FormatVersion { get; private set; }

    public ProjectLoadException()
    {
    }

    public ProjectLoadException(string formatVersion, string message) : base(message)
    {
        FormatVersion = formatVersion;
    }

    public ProjectLoadException(string formatVersion, string message, Exception innerException) : base(message, innerException)
    {
        FormatVersion = formatVersion;
    }
}
