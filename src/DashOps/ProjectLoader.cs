using Mastersign.DashOps.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Mastersign.DashOps
{
    public class ProjectLoader
    {
        public string ProjectPath { get; private set; }

        public Project Project { get; private set; }

        public ProjectView ProjectView { get; private set; }

        public ProjectLoader(string projectPath)
        {
            if (!File.Exists(projectPath))
            {
                throw new FileNotFoundException("Project file not found.", projectPath);
            }
            this.ProjectPath = projectPath;
            this.ProjectView = new ProjectView();
            InitializeDeserialization();
            InitializePerspectives();
            LoadProject();
            UpdateProjectView();
        }

        private Deserializer _deserializer;

        private void InitializeDeserialization()
        {
            this._deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();
        }

        private void InitializePerspectives()
        {
            this.ProjectView.InitializeFacettePerspectives(
                nameof(CommandAction.Host),
                nameof(CommandAction.Service),
                nameof(CommandAction.Verb));
        }

        private void LoadProject()
        {
            using (var s = File.Open(this.ProjectPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var r = new StreamReader(s, Encoding.UTF8))
            {
                this.Project = this._deserializer.Deserialize<Project>(r);
            }
        }

        private ActionView ActionViewFromCommandAction(CommandAction action)
        {
            var actionView = new ActionView
            {
                Command = action.Command,
                Arguments = action.Arguments,
                Description = action.Description,
                Tags = action.Tags,
                Facettes = action.Facettes != null 
                    ? new Dictionary<string, string>(action.Facettes) 
                    : new Dictionary<string, string>(),
            };
            if (!string.IsNullOrWhiteSpace(action.Verb))
            {
                actionView.Facettes[nameof(CommandAction.Verb)] = action.Verb;
            }
            if (!string.IsNullOrWhiteSpace(action.Service))
            {
                actionView.Facettes[nameof(CommandAction.Service)] = action.Service;
            }
            if (!string.IsNullOrWhiteSpace(action.Host))
            {
                actionView.Facettes[nameof(CommandAction.Host)] = action.Host;
            }
            return actionView;
        }

        private void UpdateProjectView()
        {
            this.ProjectView.ActionViews.Clear();
            this.ProjectView.Title = this.Project?.Title ?? "Unknown";
            if (this.Project == null) return;

            foreach (var action in this.Project.Actions)
            {
                this.ProjectView.ActionViews.Add(ActionViewFromCommandAction(action));
            }
        }
    }
}
