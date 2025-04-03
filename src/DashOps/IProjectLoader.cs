namespace Mastersign.DashOps
{
    public interface IProjectLoader
    {
        string ProjectPath { get; }
        ProjectView ProjectView { get; }

        void ReloadProjectAndProjectView();
    }
}