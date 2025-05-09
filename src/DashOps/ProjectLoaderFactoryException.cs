namespace Mastersign.DashOps;

internal class ProjectLoaderFactoryException : Exception
{
    public ProjectLoaderFactoryException()
    {
    }

    public ProjectLoaderFactoryException(string message) : base(message)
    {
    }

    public ProjectLoaderFactoryException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
