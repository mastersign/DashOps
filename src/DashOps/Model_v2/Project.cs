namespace Mastersign.DashOps.Model_v2;

partial class Project
{
    protected void Initialize()
    {
        Defaults ??= new();
        Defaults.ForMonitors ??= new();
        Defaults.ForActions ??= new();

        AutoSettings ??= new();
    }
}
