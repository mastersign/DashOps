using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Mastersign.DashOps.Model
{
    #region Scaleton Model Designer generated code
    
    // Scaleton Version: 0.2.5
    
    public enum ActionMatchMode
    {
        Description,
        Command,
        Facette,
        Tag,
    }
    
    public partial class CommandActionBase
    {
        public CommandActionBase()
        {
            this._arguments = new string[0];
            this._tags = new string[0];
        }
        
        #region String Representation
        
        public override string ToString()
        {
            return this.ToString(global::System.Globalization.CultureInfo.CurrentUICulture);
        }
        
        public virtual string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (Environment.NewLine + @"    Description = " + (!ReferenceEquals(_description, null) ? _description.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Reassure = " + _reassure.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Logs = " + (!ReferenceEquals(_logs, null) ? _logs.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Arguments = " + (!ReferenceEquals(_arguments, null) ? _arguments.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    WorkingDirectory = " + (!ReferenceEquals(_workingDirectory, null) ? _workingDirectory.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Tags = " + (!ReferenceEquals(_tags, null) ? _tags.ToString() : @"null").Replace("\n", "\n    "))));
        }
        
        #endregion
        
        #region Property Description
        
        private string _description;
        
        public virtual string Description
        {
            get { return _description; }
            set
            {
                if (string.Equals(value, _description))
                {
                    return;
                }
                _description = value;
            }
        }
        
        #endregion
        
        #region Property Reassure
        
        private bool _reassure;
        
        public virtual bool Reassure
        {
            get { return _reassure; }
            set
            {
                if ((value == _reassure))
                {
                    return;
                }
                _reassure = value;
            }
        }
        
        #endregion
        
        #region Property Logs
        
        private string _logs;
        
        public virtual string Logs
        {
            get { return _logs; }
            set
            {
                if (string.Equals(value, _logs))
                {
                    return;
                }
                _logs = value;
            }
        }
        
        #endregion
        
        #region Property Arguments
        
        private string[] _arguments;
        
        public virtual string[] Arguments
        {
            get { return _arguments; }
            set
            {
                if ((value == _arguments))
                {
                    return;
                }
                _arguments = value;
            }
        }
        
        #endregion
        
        #region Property WorkingDirectory
        
        private string _workingDirectory;
        
        public virtual string WorkingDirectory
        {
            get { return _workingDirectory; }
            set
            {
                if (string.Equals(value, _workingDirectory))
                {
                    return;
                }
                _workingDirectory = value;
            }
        }
        
        #endregion
        
        #region Property Tags
        
        private string[] _tags;
        
        public virtual string[] Tags
        {
            get { return _tags; }
            set
            {
                if ((value == _tags))
                {
                    return;
                }
                _tags = value;
            }
        }
        
        #endregion
    }
    
    public partial class CommandAction : CommandActionBase
    {
        public CommandAction()
        {
        }
        
        #region String Representation
        
        public override string ToString()
        {
            return this.ToString(global::System.Globalization.CultureInfo.CurrentUICulture);
        }
        
        public override string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (Environment.NewLine + @"    Description = " + (!ReferenceEquals(Description, null) ? Description.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Reassure = " + Reassure.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Logs = " + (!ReferenceEquals(Logs, null) ? Logs.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Arguments = " + (!ReferenceEquals(Arguments, null) ? Arguments.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    WorkingDirectory = " + (!ReferenceEquals(WorkingDirectory, null) ? WorkingDirectory.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Tags = " + (!ReferenceEquals(Tags, null) ? Tags.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Command = " + (!ReferenceEquals(_command, null) ? _command.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Verb = " + (!ReferenceEquals(_verb, null) ? _verb.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Service = " + (!ReferenceEquals(_service, null) ? _service.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Host = " + (!ReferenceEquals(_host, null) ? _host.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Facettes = " + (!ReferenceEquals(_facettes, null) ? _facettes.ToString() : @"null").Replace("\n", "\n    "))));
        }
        
        #endregion
        
        #region Property Command
        
        private string _command;
        
        public virtual string Command
        {
            get { return _command; }
            set
            {
                if (string.Equals(value, _command))
                {
                    return;
                }
                _command = value;
            }
        }
        
        #endregion
        
        #region Property Verb
        
        private string _verb;
        
        public virtual string Verb
        {
            get { return _verb; }
            set
            {
                if (string.Equals(value, _verb))
                {
                    return;
                }
                _verb = value;
            }
        }
        
        #endregion
        
        #region Property Service
        
        private string _service;
        
        public virtual string Service
        {
            get { return _service; }
            set
            {
                if (string.Equals(value, _service))
                {
                    return;
                }
                _service = value;
            }
        }
        
        #endregion
        
        #region Property Host
        
        private string _host;
        
        public virtual string Host
        {
            get { return _host; }
            set
            {
                if (string.Equals(value, _host))
                {
                    return;
                }
                _host = value;
            }
        }
        
        #endregion
        
        #region Property Facettes
        
        private Dictionary<string, string> _facettes;
        
        public virtual Dictionary<string, string> Facettes
        {
            get { return _facettes; }
            set
            {
                if ((value == _facettes))
                {
                    return;
                }
                _facettes = value;
            }
        }
        
        #endregion
    }
    
    public partial class CommandActionDiscovery : CommandActionBase
    {
        public CommandActionDiscovery()
        {
        }
        
        #region String Representation
        
        public override string ToString()
        {
            return this.ToString(global::System.Globalization.CultureInfo.CurrentUICulture);
        }
        
        public override string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (Environment.NewLine + @"    Description = " + (!ReferenceEquals(Description, null) ? Description.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Reassure = " + Reassure.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Logs = " + (!ReferenceEquals(Logs, null) ? Logs.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Arguments = " + (!ReferenceEquals(Arguments, null) ? Arguments.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    WorkingDirectory = " + (!ReferenceEquals(WorkingDirectory, null) ? WorkingDirectory.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Tags = " + (!ReferenceEquals(Tags, null) ? Tags.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    BasePath = " + (!ReferenceEquals(_basePath, null) ? _basePath.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    PathPattern = " + (!ReferenceEquals(_pathPattern, null) ? _pathPattern.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Verb = " + (!ReferenceEquals(_verb, null) ? _verb.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Service = " + (!ReferenceEquals(_service, null) ? _service.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Host = " + (!ReferenceEquals(_host, null) ? _host.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Facettes = " + (!ReferenceEquals(_facettes, null) ? _facettes.ToString() : @"null").Replace("\n", "\n    "))));
        }
        
        #endregion
        
        #region Property BasePath
        
        private string _basePath;
        
        public virtual string BasePath
        {
            get { return _basePath; }
            set
            {
                if (string.Equals(value, _basePath))
                {
                    return;
                }
                _basePath = value;
            }
        }
        
        #endregion
        
        #region Property PathPattern
        
        private string _pathPattern;
        
        public virtual string PathPattern
        {
            get { return _pathPattern; }
            set
            {
                if (string.Equals(value, _pathPattern))
                {
                    return;
                }
                _pathPattern = value;
            }
        }
        
        #endregion
        
        #region Property Verb
        
        private string _verb;
        
        public virtual string Verb
        {
            get { return _verb; }
            set
            {
                if (string.Equals(value, _verb))
                {
                    return;
                }
                _verb = value;
            }
        }
        
        #endregion
        
        #region Property Service
        
        private string _service;
        
        public virtual string Service
        {
            get { return _service; }
            set
            {
                if (string.Equals(value, _service))
                {
                    return;
                }
                _service = value;
            }
        }
        
        #endregion
        
        #region Property Host
        
        private string _host;
        
        public virtual string Host
        {
            get { return _host; }
            set
            {
                if (string.Equals(value, _host))
                {
                    return;
                }
                _host = value;
            }
        }
        
        #endregion
        
        #region Property Facettes
        
        private Dictionary<string, string> _facettes;
        
        public virtual Dictionary<string, string> Facettes
        {
            get { return _facettes; }
            set
            {
                if ((value == _facettes))
                {
                    return;
                }
                _facettes = value;
            }
        }
        
        #endregion
    }
    
    public partial class CommandActionPattern : CommandActionBase
    {
        public CommandActionPattern()
        {
            this._verb = new string[0];
            this._service = new string[0];
            this._host = new string[0];
        }
        
        #region String Representation
        
        public override string ToString()
        {
            return this.ToString(global::System.Globalization.CultureInfo.CurrentUICulture);
        }
        
        public override string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (Environment.NewLine + @"    Description = " + (!ReferenceEquals(Description, null) ? Description.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Reassure = " + Reassure.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Logs = " + (!ReferenceEquals(Logs, null) ? Logs.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Arguments = " + (!ReferenceEquals(Arguments, null) ? Arguments.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    WorkingDirectory = " + (!ReferenceEquals(WorkingDirectory, null) ? WorkingDirectory.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Tags = " + (!ReferenceEquals(Tags, null) ? Tags.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Command = " + (!ReferenceEquals(_command, null) ? _command.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Verb = " + (!ReferenceEquals(_verb, null) ? _verb.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Service = " + (!ReferenceEquals(_service, null) ? _service.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Host = " + (!ReferenceEquals(_host, null) ? _host.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Facettes = " + (!ReferenceEquals(_facettes, null) ? _facettes.ToString() : @"null").Replace("\n", "\n    "))));
        }
        
        #endregion
        
        #region Property Command
        
        private string _command;
        
        public virtual string Command
        {
            get { return _command; }
            set
            {
                if (string.Equals(value, _command))
                {
                    return;
                }
                _command = value;
            }
        }
        
        #endregion
        
        #region Property Verb
        
        private string[] _verb;
        
        public virtual string[] Verb
        {
            get { return _verb; }
            set
            {
                if ((value == _verb))
                {
                    return;
                }
                _verb = value;
            }
        }
        
        #endregion
        
        #region Property Service
        
        private string[] _service;
        
        public virtual string[] Service
        {
            get { return _service; }
            set
            {
                if ((value == _service))
                {
                    return;
                }
                _service = value;
            }
        }
        
        #endregion
        
        #region Property Host
        
        private string[] _host;
        
        public virtual string[] Host
        {
            get { return _host; }
            set
            {
                if ((value == _host))
                {
                    return;
                }
                _host = value;
            }
        }
        
        #endregion
        
        #region Property Facettes
        
        private Dictionary<string, String[]> _facettes;
        
        public virtual Dictionary<string, String[]> Facettes
        {
            get { return _facettes; }
            set
            {
                if ((value == _facettes))
                {
                    return;
                }
                _facettes = value;
            }
        }
        
        #endregion
    }
    
    public partial class ActionMatcher
    {
        public ActionMatcher()
        {
            this._mode = DEF_MODE;
        }
        
        #region String Representation
        
        public override string ToString()
        {
            return this.ToString(global::System.Globalization.CultureInfo.CurrentUICulture);
        }
        
        public virtual string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (Environment.NewLine + @"    Mode = " + _mode.ToString().Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Facette = " + (!ReferenceEquals(_facette, null) ? _facette.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Pattern = " + (!ReferenceEquals(_pattern, null) ? _pattern.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Value = " + (!ReferenceEquals(_value, null) ? _value.ToString(formatProvider) : @"null").Replace("\n", "\n    "))));
        }
        
        #endregion
        
        #region Property Mode
        
        private ActionMatchMode _mode;
        
        private const ActionMatchMode DEF_MODE = ActionMatchMode.Facette;
        
        [DefaultValue(DEF_MODE)]
        public virtual ActionMatchMode Mode
        {
            get { return _mode; }
            set
            {
                if ((value == _mode))
                {
                    return;
                }
                _mode = value;
            }
        }
        
        #endregion
        
        #region Property Facette
        
        private string _facette;
        
        public virtual string Facette
        {
            get { return _facette; }
            set
            {
                if (string.Equals(value, _facette))
                {
                    return;
                }
                _facette = value;
            }
        }
        
        #endregion
        
        #region Property Pattern
        
        private string _pattern;
        
        public virtual string Pattern
        {
            get { return _pattern; }
            set
            {
                if (string.Equals(value, _pattern))
                {
                    return;
                }
                _pattern = value;
            }
        }
        
        #endregion
        
        #region Property Value
        
        private string _value;
        
        public virtual string Value
        {
            get { return _value; }
            set
            {
                if (string.Equals(value, _value))
                {
                    return;
                }
                _value = value;
            }
        }
        
        #endregion
    }
    
    public partial class AutoAnnotation
    {
        public AutoAnnotation()
        {
            this._include = new List<ActionMatcher>();
            this._exclude = new List<ActionMatcher>();
            this._tags = new string[0];
        }
        
        #region String Representation
        
        public override string ToString()
        {
            return this.ToString(global::System.Globalization.CultureInfo.CurrentUICulture);
        }
        
        public virtual string ToString(IFormatProvider formatProvider)
        {
            global::System.Text.StringBuilder __collection_Include = new global::System.Text.StringBuilder();
            if ((!ReferenceEquals(_include, null) && !(_include.Count == 0)))
            {
                for (int __index = 0; __index < Math.Min(_include.Count, 10); __index++)
                {
                    ActionMatcher __item = _include[__index];
                    __collection_Include.AppendLine();
                    __collection_Include.Append((@"- " + __index.ToString() + @": " + (!ReferenceEquals(__item, null) ? __item.ToString() : @"null")));
                }
            }
            global::System.Text.StringBuilder __collection_Exclude = new global::System.Text.StringBuilder();
            if ((!ReferenceEquals(_exclude, null) && !(_exclude.Count == 0)))
            {
                for (int __index = 0; __index < Math.Min(_exclude.Count, 10); __index++)
                {
                    ActionMatcher __item = _exclude[__index];
                    __collection_Exclude.AppendLine();
                    __collection_Exclude.Append((@"- " + __index.ToString() + @": " + (!ReferenceEquals(__item, null) ? __item.ToString() : @"null")));
                }
            }
            return (this.GetType().FullName + @": " + (
                (Environment.NewLine + @"    Include = " + (!ReferenceEquals(_include, null) ? (_include.Count.ToString() + @" items" + __collection_Include.ToString()) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Exclude = " + (!ReferenceEquals(_exclude, null) ? (_exclude.Count.ToString() + @" items" + __collection_Exclude.ToString()) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Facettes = " + (!ReferenceEquals(_facettes, null) ? _facettes.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Tags = " + (!ReferenceEquals(_tags, null) ? _tags.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Reassure = " + _reassure.ToString(formatProvider).Replace("\n", "\n    "))));
        }
        
        #endregion
        
        #region Property Include
        
        private List<ActionMatcher> _include;
        
        public virtual List<ActionMatcher> Include
        {
            get { return _include; }
            set
            {
                if ((value == _include))
                {
                    return;
                }
                _include = value;
            }
        }
        
        #endregion
        
        #region Property Exclude
        
        private List<ActionMatcher> _exclude;
        
        public virtual List<ActionMatcher> Exclude
        {
            get { return _exclude; }
            set
            {
                if ((value == _exclude))
                {
                    return;
                }
                _exclude = value;
            }
        }
        
        #endregion
        
        #region Property Facettes
        
        private Dictionary<string, string> _facettes;
        
        public virtual Dictionary<string, string> Facettes
        {
            get { return _facettes; }
            set
            {
                if ((value == _facettes))
                {
                    return;
                }
                _facettes = value;
            }
        }
        
        #endregion
        
        #region Property Tags
        
        private string[] _tags;
        
        public virtual string[] Tags
        {
            get { return _tags; }
            set
            {
                if ((value == _tags))
                {
                    return;
                }
                _tags = value;
            }
        }
        
        #endregion
        
        #region Property Reassure
        
        private bool _reassure;
        
        public virtual bool Reassure
        {
            get { return _reassure; }
            set
            {
                if ((value == _reassure))
                {
                    return;
                }
                _reassure = value;
            }
        }
        
        #endregion
    }
    
    public partial class MonitorBase
    {
        public MonitorBase()
        {
            this._interval = DEF_INTERVAL;
            this._requiredPatterns = new string[0];
            this._forbiddenPatterns = new string[0];
        }
        
        #region String Representation
        
        public override string ToString()
        {
            return this.ToString(global::System.Globalization.CultureInfo.CurrentUICulture);
        }
        
        public virtual string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (Environment.NewLine + @"    Title = " + (!ReferenceEquals(_title, null) ? _title.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Logs = " + (!ReferenceEquals(_logs, null) ? _logs.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Interval = " + _interval.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    RequiredPatterns = " + (!ReferenceEquals(_requiredPatterns, null) ? _requiredPatterns.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    ForbiddenPatterns = " + (!ReferenceEquals(_forbiddenPatterns, null) ? _forbiddenPatterns.ToString() : @"null").Replace("\n", "\n    "))));
        }
        
        #endregion
        
        #region Property Title
        
        private string _title;
        
        public virtual string Title
        {
            get { return _title; }
            set
            {
                if (string.Equals(value, _title))
                {
                    return;
                }
                _title = value;
            }
        }
        
        #endregion
        
        #region Property Logs
        
        private string _logs;
        
        public virtual string Logs
        {
            get { return _logs; }
            set
            {
                if (string.Equals(value, _logs))
                {
                    return;
                }
                _logs = value;
            }
        }
        
        #endregion
        
        #region Property Interval
        
        private int _interval;
        
        private const int DEF_INTERVAL = -1;
        
        [DefaultValue(DEF_INTERVAL)]
        public virtual int Interval
        {
            get { return _interval; }
            set
            {
                if ((value == _interval))
                {
                    return;
                }
                _interval = value;
            }
        }
        
        #endregion
        
        #region Property RequiredPatterns
        
        private string[] _requiredPatterns;
        
        public virtual string[] RequiredPatterns
        {
            get { return _requiredPatterns; }
            set
            {
                if ((value == _requiredPatterns))
                {
                    return;
                }
                _requiredPatterns = value;
            }
        }
        
        #endregion
        
        #region Property ForbiddenPatterns
        
        private string[] _forbiddenPatterns;
        
        public virtual string[] ForbiddenPatterns
        {
            get { return _forbiddenPatterns; }
            set
            {
                if ((value == _forbiddenPatterns))
                {
                    return;
                }
                _forbiddenPatterns = value;
            }
        }
        
        #endregion
    }
    
    public partial class CommandMonitorBase : MonitorBase
    {
        public CommandMonitorBase()
        {
            this._arguments = new string[0];
        }
        
        #region String Representation
        
        public override string ToString()
        {
            return this.ToString(global::System.Globalization.CultureInfo.CurrentUICulture);
        }
        
        public override string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (Environment.NewLine + @"    Title = " + (!ReferenceEquals(Title, null) ? Title.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Logs = " + (!ReferenceEquals(Logs, null) ? Logs.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Interval = " + Interval.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    RequiredPatterns = " + (!ReferenceEquals(RequiredPatterns, null) ? RequiredPatterns.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    ForbiddenPatterns = " + (!ReferenceEquals(ForbiddenPatterns, null) ? ForbiddenPatterns.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Arguments = " + (!ReferenceEquals(_arguments, null) ? _arguments.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    WorkingDirectory = " + (!ReferenceEquals(_workingDirectory, null) ? _workingDirectory.ToString(formatProvider) : @"null").Replace("\n", "\n    "))));
        }
        
        #endregion
        
        #region Property Arguments
        
        private string[] _arguments;
        
        public virtual string[] Arguments
        {
            get { return _arguments; }
            set
            {
                if ((value == _arguments))
                {
                    return;
                }
                _arguments = value;
            }
        }
        
        #endregion
        
        #region Property WorkingDirectory
        
        private string _workingDirectory;
        
        public virtual string WorkingDirectory
        {
            get { return _workingDirectory; }
            set
            {
                if (string.Equals(value, _workingDirectory))
                {
                    return;
                }
                _workingDirectory = value;
            }
        }
        
        #endregion
    }
    
    public partial class CommandMonitor : CommandMonitorBase
    {
        public CommandMonitor()
        {
        }
        
        #region String Representation
        
        public override string ToString()
        {
            return this.ToString(global::System.Globalization.CultureInfo.CurrentUICulture);
        }
        
        public override string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (Environment.NewLine + @"    Title = " + (!ReferenceEquals(Title, null) ? Title.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Logs = " + (!ReferenceEquals(Logs, null) ? Logs.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Interval = " + Interval.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    RequiredPatterns = " + (!ReferenceEquals(RequiredPatterns, null) ? RequiredPatterns.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    ForbiddenPatterns = " + (!ReferenceEquals(ForbiddenPatterns, null) ? ForbiddenPatterns.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Arguments = " + (!ReferenceEquals(Arguments, null) ? Arguments.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    WorkingDirectory = " + (!ReferenceEquals(WorkingDirectory, null) ? WorkingDirectory.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Command = " + (!ReferenceEquals(_command, null) ? _command.ToString(formatProvider) : @"null").Replace("\n", "\n    "))));
        }
        
        #endregion
        
        #region Property Command
        
        private string _command;
        
        public virtual string Command
        {
            get { return _command; }
            set
            {
                if (string.Equals(value, _command))
                {
                    return;
                }
                _command = value;
            }
        }
        
        #endregion
    }
    
    public partial class CommandMonitorDiscovery : CommandMonitorBase
    {
        public CommandMonitorDiscovery()
        {
        }
        
        #region String Representation
        
        public override string ToString()
        {
            return this.ToString(global::System.Globalization.CultureInfo.CurrentUICulture);
        }
        
        public override string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (Environment.NewLine + @"    Title = " + (!ReferenceEquals(Title, null) ? Title.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Logs = " + (!ReferenceEquals(Logs, null) ? Logs.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Interval = " + Interval.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    RequiredPatterns = " + (!ReferenceEquals(RequiredPatterns, null) ? RequiredPatterns.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    ForbiddenPatterns = " + (!ReferenceEquals(ForbiddenPatterns, null) ? ForbiddenPatterns.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Arguments = " + (!ReferenceEquals(Arguments, null) ? Arguments.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    WorkingDirectory = " + (!ReferenceEquals(WorkingDirectory, null) ? WorkingDirectory.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    BasePath = " + (!ReferenceEquals(_basePath, null) ? _basePath.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    PathPattern = " + (!ReferenceEquals(_pathPattern, null) ? _pathPattern.ToString(formatProvider) : @"null").Replace("\n", "\n    "))));
        }
        
        #endregion
        
        #region Property BasePath
        
        private string _basePath;
        
        public virtual string BasePath
        {
            get { return _basePath; }
            set
            {
                if (string.Equals(value, _basePath))
                {
                    return;
                }
                _basePath = value;
            }
        }
        
        #endregion
        
        #region Property PathPattern
        
        private string _pathPattern;
        
        public virtual string PathPattern
        {
            get { return _pathPattern; }
            set
            {
                if (string.Equals(value, _pathPattern))
                {
                    return;
                }
                _pathPattern = value;
            }
        }
        
        #endregion
    }
    
    public partial class CommandMonitorPattern : CommandMonitor
    {
        public CommandMonitorPattern()
        {
        }
        
        #region String Representation
        
        public override string ToString()
        {
            return this.ToString(global::System.Globalization.CultureInfo.CurrentUICulture);
        }
        
        public override string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (Environment.NewLine + @"    Title = " + (!ReferenceEquals(Title, null) ? Title.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Logs = " + (!ReferenceEquals(Logs, null) ? Logs.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Interval = " + Interval.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    RequiredPatterns = " + (!ReferenceEquals(RequiredPatterns, null) ? RequiredPatterns.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    ForbiddenPatterns = " + (!ReferenceEquals(ForbiddenPatterns, null) ? ForbiddenPatterns.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Arguments = " + (!ReferenceEquals(Arguments, null) ? Arguments.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    WorkingDirectory = " + (!ReferenceEquals(WorkingDirectory, null) ? WorkingDirectory.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Command = " + (!ReferenceEquals(Command, null) ? Command.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Variables = " + (!ReferenceEquals(_variables, null) ? _variables.ToString() : @"null").Replace("\n", "\n    "))));
        }
        
        #endregion
        
        #region Property Variables
        
        private Dictionary<string, String[]> _variables;
        
        public virtual Dictionary<string, String[]> Variables
        {
            get { return _variables; }
            set
            {
                if ((value == _variables))
                {
                    return;
                }
                _variables = value;
            }
        }
        
        #endregion
    }
    
    public partial class WebMonitor : MonitorBase
    {
        public WebMonitor()
        {
            this._statusCodes = new int[0];
        }
        
        #region String Representation
        
        public override string ToString()
        {
            return this.ToString(global::System.Globalization.CultureInfo.CurrentUICulture);
        }
        
        public override string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (Environment.NewLine + @"    Title = " + (!ReferenceEquals(Title, null) ? Title.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Logs = " + (!ReferenceEquals(Logs, null) ? Logs.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Interval = " + Interval.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    RequiredPatterns = " + (!ReferenceEquals(RequiredPatterns, null) ? RequiredPatterns.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    ForbiddenPatterns = " + (!ReferenceEquals(ForbiddenPatterns, null) ? ForbiddenPatterns.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Url = " + (!ReferenceEquals(_url, null) ? _url.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Headers = " + (!ReferenceEquals(_headers, null) ? _headers.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    StatusCodes = " + (!ReferenceEquals(_statusCodes, null) ? _statusCodes.ToString() : @"null").Replace("\n", "\n    "))));
        }
        
        #endregion
        
        #region Property Url
        
        private string _url;
        
        public virtual string Url
        {
            get { return _url; }
            set
            {
                if (string.Equals(value, _url))
                {
                    return;
                }
                _url = value;
            }
        }
        
        #endregion
        
        #region Property Headers
        
        private Dictionary<string, string> _headers;
        
        public virtual Dictionary<string, string> Headers
        {
            get { return _headers; }
            set
            {
                if ((value == _headers))
                {
                    return;
                }
                _headers = value;
            }
        }
        
        #endregion
        
        #region Property StatusCodes
        
        private int[] _statusCodes;
        
        public virtual int[] StatusCodes
        {
            get { return _statusCodes; }
            set
            {
                if ((value == _statusCodes))
                {
                    return;
                }
                _statusCodes = value;
            }
        }
        
        #endregion
    }
    
    public partial class WebMonitorPattern : WebMonitor
    {
        public WebMonitorPattern()
        {
        }
        
        #region String Representation
        
        public override string ToString()
        {
            return this.ToString(global::System.Globalization.CultureInfo.CurrentUICulture);
        }
        
        public override string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (Environment.NewLine + @"    Title = " + (!ReferenceEquals(Title, null) ? Title.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Logs = " + (!ReferenceEquals(Logs, null) ? Logs.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Interval = " + Interval.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    RequiredPatterns = " + (!ReferenceEquals(RequiredPatterns, null) ? RequiredPatterns.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    ForbiddenPatterns = " + (!ReferenceEquals(ForbiddenPatterns, null) ? ForbiddenPatterns.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Url = " + (!ReferenceEquals(Url, null) ? Url.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Headers = " + (!ReferenceEquals(Headers, null) ? Headers.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    StatusCodes = " + (!ReferenceEquals(StatusCodes, null) ? StatusCodes.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Variables = " + (!ReferenceEquals(_variables, null) ? _variables.ToString() : @"null").Replace("\n", "\n    "))));
        }
        
        #endregion
        
        #region Property Variables
        
        private Dictionary<string, String[]> _variables;
        
        public virtual Dictionary<string, String[]> Variables
        {
            get { return _variables; }
            set
            {
                if ((value == _variables))
                {
                    return;
                }
                _variables = value;
            }
        }
        
        #endregion
    }
    
    public partial class Project
    {
        public Project()
        {
            this._actions = new List<CommandAction>();
            this._actionDiscovery = new List<CommandActionDiscovery>();
            this._actionPatterns = new List<CommandActionPattern>();
            this._perspectives = new List<string>();
            this._auto = new List<AutoAnnotation>();
            this._monitors = new List<CommandMonitor>();
            this._monitorDiscovery = new List<CommandMonitorDiscovery>();
            this._monitorPatterns = new List<CommandMonitorPattern>();
            this._webMonitors = new List<WebMonitor>();
            this._webMonitorPatterns = new List<WebMonitorPattern>();
            this._defaultMonitorInterval = DEF_DEFAULTMONITORINTERVAL;
        }
        
        #region String Representation
        
        public override string ToString()
        {
            return this.ToString(global::System.Globalization.CultureInfo.CurrentUICulture);
        }
        
        public virtual string ToString(IFormatProvider formatProvider)
        {
            global::System.Text.StringBuilder __collection_Actions = new global::System.Text.StringBuilder();
            if ((!ReferenceEquals(_actions, null) && !(_actions.Count == 0)))
            {
                for (int __index = 0; __index < Math.Min(_actions.Count, 10); __index++)
                {
                    CommandAction __item = _actions[__index];
                    __collection_Actions.AppendLine();
                    __collection_Actions.Append((@"- " + __index.ToString() + @": " + (!ReferenceEquals(__item, null) ? __item.ToString() : @"null")));
                }
            }
            global::System.Text.StringBuilder __collection_ActionDiscovery = new global::System.Text.StringBuilder();
            if ((!ReferenceEquals(_actionDiscovery, null) && !(_actionDiscovery.Count == 0)))
            {
                for (int __index = 0; __index < Math.Min(_actionDiscovery.Count, 10); __index++)
                {
                    CommandActionDiscovery __item = _actionDiscovery[__index];
                    __collection_ActionDiscovery.AppendLine();
                    __collection_ActionDiscovery.Append((@"- " + __index.ToString() + @": " + (!ReferenceEquals(__item, null) ? __item.ToString() : @"null")));
                }
            }
            global::System.Text.StringBuilder __collection_ActionPatterns = new global::System.Text.StringBuilder();
            if ((!ReferenceEquals(_actionPatterns, null) && !(_actionPatterns.Count == 0)))
            {
                for (int __index = 0; __index < Math.Min(_actionPatterns.Count, 10); __index++)
                {
                    CommandActionPattern __item = _actionPatterns[__index];
                    __collection_ActionPatterns.AppendLine();
                    __collection_ActionPatterns.Append((@"- " + __index.ToString() + @": " + (!ReferenceEquals(__item, null) ? __item.ToString() : @"null")));
                }
            }
            global::System.Text.StringBuilder __collection_Perspectives = new global::System.Text.StringBuilder();
            if ((!ReferenceEquals(_perspectives, null) && !(_perspectives.Count == 0)))
            {
                for (int __index = 0; __index < Math.Min(_perspectives.Count, 10); __index++)
                {
                    string __item = _perspectives[__index];
                    __collection_Perspectives.AppendLine();
                    __collection_Perspectives.Append((@"- " + __index.ToString() + @": " + (!ReferenceEquals(__item, null) ? __item.ToString(formatProvider) : @"null")));
                }
            }
            global::System.Text.StringBuilder __collection_Auto = new global::System.Text.StringBuilder();
            if ((!ReferenceEquals(_auto, null) && !(_auto.Count == 0)))
            {
                for (int __index = 0; __index < Math.Min(_auto.Count, 10); __index++)
                {
                    AutoAnnotation __item = _auto[__index];
                    __collection_Auto.AppendLine();
                    __collection_Auto.Append((@"- " + __index.ToString() + @": " + (!ReferenceEquals(__item, null) ? __item.ToString() : @"null")));
                }
            }
            global::System.Text.StringBuilder __collection_Monitors = new global::System.Text.StringBuilder();
            if ((!ReferenceEquals(_monitors, null) && !(_monitors.Count == 0)))
            {
                for (int __index = 0; __index < Math.Min(_monitors.Count, 10); __index++)
                {
                    CommandMonitor __item = _monitors[__index];
                    __collection_Monitors.AppendLine();
                    __collection_Monitors.Append((@"- " + __index.ToString() + @": " + (!ReferenceEquals(__item, null) ? __item.ToString() : @"null")));
                }
            }
            global::System.Text.StringBuilder __collection_MonitorDiscovery = new global::System.Text.StringBuilder();
            if ((!ReferenceEquals(_monitorDiscovery, null) && !(_monitorDiscovery.Count == 0)))
            {
                for (int __index = 0; __index < Math.Min(_monitorDiscovery.Count, 10); __index++)
                {
                    CommandMonitorDiscovery __item = _monitorDiscovery[__index];
                    __collection_MonitorDiscovery.AppendLine();
                    __collection_MonitorDiscovery.Append((@"- " + __index.ToString() + @": " + (!ReferenceEquals(__item, null) ? __item.ToString() : @"null")));
                }
            }
            global::System.Text.StringBuilder __collection_MonitorPatterns = new global::System.Text.StringBuilder();
            if ((!ReferenceEquals(_monitorPatterns, null) && !(_monitorPatterns.Count == 0)))
            {
                for (int __index = 0; __index < Math.Min(_monitorPatterns.Count, 10); __index++)
                {
                    CommandMonitorPattern __item = _monitorPatterns[__index];
                    __collection_MonitorPatterns.AppendLine();
                    __collection_MonitorPatterns.Append((@"- " + __index.ToString() + @": " + (!ReferenceEquals(__item, null) ? __item.ToString() : @"null")));
                }
            }
            global::System.Text.StringBuilder __collection_WebMonitors = new global::System.Text.StringBuilder();
            if ((!ReferenceEquals(_webMonitors, null) && !(_webMonitors.Count == 0)))
            {
                for (int __index = 0; __index < Math.Min(_webMonitors.Count, 10); __index++)
                {
                    WebMonitor __item = _webMonitors[__index];
                    __collection_WebMonitors.AppendLine();
                    __collection_WebMonitors.Append((@"- " + __index.ToString() + @": " + (!ReferenceEquals(__item, null) ? __item.ToString() : @"null")));
                }
            }
            global::System.Text.StringBuilder __collection_WebMonitorPatterns = new global::System.Text.StringBuilder();
            if ((!ReferenceEquals(_webMonitorPatterns, null) && !(_webMonitorPatterns.Count == 0)))
            {
                for (int __index = 0; __index < Math.Min(_webMonitorPatterns.Count, 10); __index++)
                {
                    WebMonitorPattern __item = _webMonitorPatterns[__index];
                    __collection_WebMonitorPatterns.AppendLine();
                    __collection_WebMonitorPatterns.Append((@"- " + __index.ToString() + @": " + (!ReferenceEquals(__item, null) ? __item.ToString() : @"null")));
                }
            }
            return (this.GetType().FullName + @": " + (
                (Environment.NewLine + @"    Version = " + (!ReferenceEquals(_version, null) ? _version.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Title = " + (!ReferenceEquals(_title, null) ? _title.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Logs = " + (!ReferenceEquals(_logs, null) ? _logs.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Actions = " + (!ReferenceEquals(_actions, null) ? (_actions.Count.ToString() + @" items" + __collection_Actions.ToString()) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    ActionDiscovery = " + (!ReferenceEquals(_actionDiscovery, null) ? (_actionDiscovery.Count.ToString() + @" items" + __collection_ActionDiscovery.ToString()) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    ActionPatterns = " + (!ReferenceEquals(_actionPatterns, null) ? (_actionPatterns.Count.ToString() + @" items" + __collection_ActionPatterns.ToString()) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Perspectives = " + (!ReferenceEquals(_perspectives, null) ? (_perspectives.Count.ToString() + @" items" + __collection_Perspectives.ToString()) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Auto = " + (!ReferenceEquals(_auto, null) ? (_auto.Count.ToString() + @" items" + __collection_Auto.ToString()) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Monitors = " + (!ReferenceEquals(_monitors, null) ? (_monitors.Count.ToString() + @" items" + __collection_Monitors.ToString()) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    MonitorDiscovery = " + (!ReferenceEquals(_monitorDiscovery, null) ? (_monitorDiscovery.Count.ToString() + @" items" + __collection_MonitorDiscovery.ToString()) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    MonitorPatterns = " + (!ReferenceEquals(_monitorPatterns, null) ? (_monitorPatterns.Count.ToString() + @" items" + __collection_MonitorPatterns.ToString()) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    WebMonitors = " + (!ReferenceEquals(_webMonitors, null) ? (_webMonitors.Count.ToString() + @" items" + __collection_WebMonitors.ToString()) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    WebMonitorPatterns = " + (!ReferenceEquals(_webMonitorPatterns, null) ? (_webMonitorPatterns.Count.ToString() + @" items" + __collection_WebMonitorPatterns.ToString()) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    DefaultMonitorInterval = " + _defaultMonitorInterval.ToString(formatProvider).Replace("\n", "\n    "))));
        }
        
        #endregion
        
        #region Property Version
        
        private string _version;
        
        public virtual string Version
        {
            get { return _version; }
            set
            {
                if (string.Equals(value, _version))
                {
                    return;
                }
                _version = value;
            }
        }
        
        #endregion
        
        #region Property Title
        
        private string _title;
        
        public virtual string Title
        {
            get { return _title; }
            set
            {
                if (string.Equals(value, _title))
                {
                    return;
                }
                _title = value;
            }
        }
        
        #endregion
        
        #region Property Logs
        
        private string _logs;
        
        public virtual string Logs
        {
            get { return _logs; }
            set
            {
                if (string.Equals(value, _logs))
                {
                    return;
                }
                _logs = value;
            }
        }
        
        #endregion
        
        #region Property Actions
        
        private List<CommandAction> _actions;
        
        public virtual List<CommandAction> Actions
        {
            get { return _actions; }
            set
            {
                if ((value == _actions))
                {
                    return;
                }
                _actions = value;
            }
        }
        
        #endregion
        
        #region Property ActionDiscovery
        
        private List<CommandActionDiscovery> _actionDiscovery;
        
        public virtual List<CommandActionDiscovery> ActionDiscovery
        {
            get { return _actionDiscovery; }
            set
            {
                if ((value == _actionDiscovery))
                {
                    return;
                }
                _actionDiscovery = value;
            }
        }
        
        #endregion
        
        #region Property ActionPatterns
        
        private List<CommandActionPattern> _actionPatterns;
        
        public virtual List<CommandActionPattern> ActionPatterns
        {
            get { return _actionPatterns; }
            set
            {
                if ((value == _actionPatterns))
                {
                    return;
                }
                _actionPatterns = value;
            }
        }
        
        #endregion
        
        #region Property Perspectives
        
        private List<string> _perspectives;
        
        public virtual List<string> Perspectives
        {
            get { return _perspectives; }
            set
            {
                if ((value == _perspectives))
                {
                    return;
                }
                _perspectives = value;
            }
        }
        
        #endregion
        
        #region Property Auto
        
        private List<AutoAnnotation> _auto;
        
        public virtual List<AutoAnnotation> Auto
        {
            get { return _auto; }
            set
            {
                if ((value == _auto))
                {
                    return;
                }
                _auto = value;
            }
        }
        
        #endregion
        
        #region Property Monitors
        
        private List<CommandMonitor> _monitors;
        
        public virtual List<CommandMonitor> Monitors
        {
            get { return _monitors; }
            set
            {
                if ((value == _monitors))
                {
                    return;
                }
                _monitors = value;
            }
        }
        
        #endregion
        
        #region Property MonitorDiscovery
        
        private List<CommandMonitorDiscovery> _monitorDiscovery;
        
        public virtual List<CommandMonitorDiscovery> MonitorDiscovery
        {
            get { return _monitorDiscovery; }
            set
            {
                if ((value == _monitorDiscovery))
                {
                    return;
                }
                _monitorDiscovery = value;
            }
        }
        
        #endregion
        
        #region Property MonitorPatterns
        
        private List<CommandMonitorPattern> _monitorPatterns;
        
        public virtual List<CommandMonitorPattern> MonitorPatterns
        {
            get { return _monitorPatterns; }
            set
            {
                if ((value == _monitorPatterns))
                {
                    return;
                }
                _monitorPatterns = value;
            }
        }
        
        #endregion
        
        #region Property WebMonitors
        
        private List<WebMonitor> _webMonitors;
        
        public virtual List<WebMonitor> WebMonitors
        {
            get { return _webMonitors; }
            set
            {
                if ((value == _webMonitors))
                {
                    return;
                }
                _webMonitors = value;
            }
        }
        
        #endregion
        
        #region Property WebMonitorPatterns
        
        private List<WebMonitorPattern> _webMonitorPatterns;
        
        public virtual List<WebMonitorPattern> WebMonitorPatterns
        {
            get { return _webMonitorPatterns; }
            set
            {
                if ((value == _webMonitorPatterns))
                {
                    return;
                }
                _webMonitorPatterns = value;
            }
        }
        
        #endregion
        
        #region Property DefaultMonitorInterval
        
        private int _defaultMonitorInterval;
        
        private const int DEF_DEFAULTMONITORINTERVAL = 60;
        
        [DefaultValue(DEF_DEFAULTMONITORINTERVAL)]
        public virtual int DefaultMonitorInterval
        {
            get { return _defaultMonitorInterval; }
            set
            {
                if ((value == _defaultMonitorInterval))
                {
                    return;
                }
                _defaultMonitorInterval = value;
            }
        }
        
        #endregion
    }
    
    #endregion
}