﻿using Mastersign.DashOps.ViewModel;
using static Mastersign.DashOps.Model_v2.Helper;

namespace Mastersign.DashOps.Model_v2;

partial class CommandMonitorPattern
{
    public MatchableMonitor CreateMatchable(IReadOnlyDictionary<string, string> instanceVariables)
    {
        var variables = CoalesceValues([instanceVariables]);
        return new MatchableMonitor
        {
            Title = ExpandTemplate(Title, variables),
            Tags = Tags ?? [],
            Variables = variables,
            Command = ExpandEnv(ExpandTemplate(Command, variables)),
        };
    }

    public CommandMonitorView CreateView(
        IReadOnlyList<AutoMonitorSettings> autoSettings,
        DefaultMonitorSettings monitorDefaults,
        DefaultSettings commonDefaults,
        IReadOnlyDictionary<string, string> instanceVariables)
    {
        var variables = instanceVariables;
        var view = new CommandMonitorView
        {
            Title = ExpandTemplate(Title, variables),
            Tags = Unite([Tags]),

            Command = ExpandEnv(ExpandTemplate(Command, variables)),
            Arguments = FormatArguments(
                Coalesce([
                    Arguments,
                    ..autoSettings.Select(s => s.Arguments),
                    monitorDefaults?.Arguments,
                    commonDefaults.Arguments,
                ])?
                    .Select(a => ExpandTemplate(a, variables))
                    .Select(ExpandEnv)),
        };
        view.UpdateWith(this, autoSettings, monitorDefaults, commonDefaults, variables);
        view.UpdateStatusFromLogFile();

        return view;
    }
}
