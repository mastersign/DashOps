namespace Mastersign.DashOps;

internal class DesignTimeActionView : ActionView
{
    public DesignTimeActionView()
    {
        Title = "Demo action with a rather long title";
        Tags = ["demo", "Some Longer Tag", "important"];
        Facets = new Dictionary<string, string>
        {
            { "host", "localhost" },
            { "verb", "demonstrating" },
            { "service", "Design Time" },
        };
        FacetViews.Add(new FacetView("host", "Hosts", "localhost"));
        FacetViews.Add(new FacetView("verb", "Verbs", "demonstrating"));
        FacetViews.Add(new FacetView("service", "Services", "localhost"));
        Visible = true;

        Command = @"C:\Program Files (x86)\Some Vendor\My Application\bin\MyApplication.exe";
        Arguments = @"--verbose --input-file ""D:\Massive Data Storage\Project XYZ\resources\my-file.bin"" --option-a A --option-b B";
        WorkingDirectory = @"D:\Massive Data Storage\Project XYZ\assembly";
        Environment = new Dictionary<string, string>
        {
            { "SOME_IMPORTANT_VARIABLE", "And a rather lengthy value for demonstartive purposes" },
            { "EDITOR", "notepad.exe" },
        };
      // <!--<Property name="ExecPaths" type="string[]" />-->
      // <!--<Property name="Terminal" />-->
        ExitCodes = [0, 4, 8, 12, 999_999_999];

        Reassure = true;
        KeepOpen = true;
        AlwaysClose = false;

        Logs = @"D:\Massive Data Storage\Project XYZ\logs";
        NoLogs = false;
        CurrentLogFile = 
            @"D:\Massive Data Storage\Project XYZ\logs\" 
            + DateTime.Now.ToString("yyyyMMdd_HHmmss")
            + "_" + CommandId 
            + "_OK_0.log";
    }
}
