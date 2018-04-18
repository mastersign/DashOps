using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Mastersign.DashOps.Model
{
    #region Scaleton Model Designer generated code
    
    // Scaleton Version: 0.2.4
    
    public partial class CommandAction : INotifyPropertyChanged
    {
        public CommandAction()
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
                (Environment.NewLine + @"    Command = " + (!ReferenceEquals(_command, null) ? _command.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Arguments = " + (!ReferenceEquals(_arguments, null) ? _arguments.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Verb = " + (!ReferenceEquals(_verb, null) ? _verb.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Service = " + (!ReferenceEquals(_service, null) ? _service.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Host = " + (!ReferenceEquals(_host, null) ? _host.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Tags = " + (!ReferenceEquals(_tags, null) ? _tags.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Facettes = " + (!ReferenceEquals(_facettes, null) ? _facettes.ToString() : @"null").Replace("\n", "\n    "))));
        }
        
        #endregion
        
        #region Change Tracking
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        
        #endregion
        
        #region Property Description
        
        private string _description;
        
        public event EventHandler DescriptionChanged;
        
        protected virtual void OnDescriptionChanged()
        {
            EventHandler handler = DescriptionChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Description");
        }
        
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
                this.OnDescriptionChanged();
            }
        }
        
        #endregion
        
        #region Property Command
        
        private string _command;
        
        public event EventHandler CommandChanged;
        
        protected virtual void OnCommandChanged()
        {
            EventHandler handler = CommandChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Command");
        }
        
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
                this.OnCommandChanged();
            }
        }
        
        #endregion
        
        #region Property Arguments
        
        private string[] _arguments;
        
        public event EventHandler ArgumentsChanged;
        
        protected virtual void OnArgumentsChanged()
        {
            EventHandler handler = ArgumentsChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Arguments");
        }
        
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
                this.OnArgumentsChanged();
            }
        }
        
        #endregion
        
        #region Property Verb
        
        private string _verb;
        
        public event EventHandler VerbChanged;
        
        protected virtual void OnVerbChanged()
        {
            EventHandler handler = VerbChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Verb");
        }
        
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
                this.OnVerbChanged();
            }
        }
        
        #endregion
        
        #region Property Service
        
        private string _service;
        
        public event EventHandler ServiceChanged;
        
        protected virtual void OnServiceChanged()
        {
            EventHandler handler = ServiceChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Service");
        }
        
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
                this.OnServiceChanged();
            }
        }
        
        #endregion
        
        #region Property Host
        
        private string _host;
        
        public event EventHandler HostChanged;
        
        protected virtual void OnHostChanged()
        {
            EventHandler handler = HostChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Host");
        }
        
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
                this.OnHostChanged();
            }
        }
        
        #endregion
        
        #region Property Tags
        
        private string[] _tags;
        
        public event EventHandler TagsChanged;
        
        protected virtual void OnTagsChanged()
        {
            EventHandler handler = TagsChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Tags");
        }
        
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
                this.OnTagsChanged();
            }
        }
        
        #endregion
        
        #region Property Facettes
        
        private Dictionary<string, string> _facettes;
        
        public event EventHandler FacettesChanged;
        
        protected virtual void OnFacettesChanged()
        {
            EventHandler handler = FacettesChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Facettes");
        }
        
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
                this.OnFacettesChanged();
            }
        }
        
        #endregion
    }
    
    public partial class CommandActionDiscovery : INotifyPropertyChanged
    {
        public CommandActionDiscovery()
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
                (Environment.NewLine + @"    DescriptionTemplate = " + (!ReferenceEquals(_descriptionTemplate, null) ? _descriptionTemplate.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    BasePath = " + (!ReferenceEquals(_basePath, null) ? _basePath.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    PathRegex = " + (!ReferenceEquals(_pathRegex, null) ? _pathRegex.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Arguments = " + (!ReferenceEquals(_arguments, null) ? _arguments.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Verb = " + (!ReferenceEquals(_verb, null) ? _verb.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Service = " + (!ReferenceEquals(_service, null) ? _service.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Host = " + (!ReferenceEquals(_host, null) ? _host.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Tags = " + (!ReferenceEquals(_tags, null) ? _tags.ToString() : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Facettes = " + (!ReferenceEquals(_facettes, null) ? _facettes.ToString() : @"null").Replace("\n", "\n    "))));
        }
        
        #endregion
        
        #region Change Tracking
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        
        #endregion
        
        #region Property DescriptionTemplate
        
        private string _descriptionTemplate;
        
        public event EventHandler DescriptionTemplateChanged;
        
        protected virtual void OnDescriptionTemplateChanged()
        {
            EventHandler handler = DescriptionTemplateChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"DescriptionTemplate");
        }
        
        public virtual string DescriptionTemplate
        {
            get { return _descriptionTemplate; }
            set
            {
                if (string.Equals(value, _descriptionTemplate))
                {
                    return;
                }
                _descriptionTemplate = value;
                this.OnDescriptionTemplateChanged();
            }
        }
        
        #endregion
        
        #region Property BasePath
        
        private string _basePath;
        
        public event EventHandler BasePathChanged;
        
        protected virtual void OnBasePathChanged()
        {
            EventHandler handler = BasePathChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"BasePath");
        }
        
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
                this.OnBasePathChanged();
            }
        }
        
        #endregion
        
        #region Property PathRegex
        
        private string _pathRegex;
        
        public event EventHandler PathRegexChanged;
        
        protected virtual void OnPathRegexChanged()
        {
            EventHandler handler = PathRegexChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"PathRegex");
        }
        
        public virtual string PathRegex
        {
            get { return _pathRegex; }
            set
            {
                if (string.Equals(value, _pathRegex))
                {
                    return;
                }
                _pathRegex = value;
                this.OnPathRegexChanged();
            }
        }
        
        #endregion
        
        #region Property Arguments
        
        private string[] _arguments;
        
        public event EventHandler ArgumentsChanged;
        
        protected virtual void OnArgumentsChanged()
        {
            EventHandler handler = ArgumentsChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Arguments");
        }
        
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
                this.OnArgumentsChanged();
            }
        }
        
        #endregion
        
        #region Property Verb
        
        private string _verb;
        
        public event EventHandler VerbChanged;
        
        protected virtual void OnVerbChanged()
        {
            EventHandler handler = VerbChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Verb");
        }
        
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
                this.OnVerbChanged();
            }
        }
        
        #endregion
        
        #region Property Service
        
        private string _service;
        
        public event EventHandler ServiceChanged;
        
        protected virtual void OnServiceChanged()
        {
            EventHandler handler = ServiceChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Service");
        }
        
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
                this.OnServiceChanged();
            }
        }
        
        #endregion
        
        #region Property Host
        
        private string _host;
        
        public event EventHandler HostChanged;
        
        protected virtual void OnHostChanged()
        {
            EventHandler handler = HostChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Host");
        }
        
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
                this.OnHostChanged();
            }
        }
        
        #endregion
        
        #region Property Tags
        
        private string[] _tags;
        
        public event EventHandler TagsChanged;
        
        protected virtual void OnTagsChanged()
        {
            EventHandler handler = TagsChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Tags");
        }
        
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
                this.OnTagsChanged();
            }
        }
        
        #endregion
        
        #region Property Facettes
        
        private Dictionary<string, string> _facettes;
        
        public event EventHandler FacettesChanged;
        
        protected virtual void OnFacettesChanged()
        {
            EventHandler handler = FacettesChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Facettes");
        }
        
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
                this.OnFacettesChanged();
            }
        }
        
        #endregion
    }
    
    public partial class Project : INotifyPropertyChanged
    {
        public Project()
        {
            this._actions = new global::System.Collections.ObjectModel.ObservableCollection<CommandAction>();
            this._actionDiscovery = new global::System.Collections.ObjectModel.ObservableCollection<CommandActionDiscovery>();
            this._perspectives = new global::System.Collections.ObjectModel.ObservableCollection<string>();
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
            return (this.GetType().FullName + @": " + (
                (Environment.NewLine + @"    Title = " + (!ReferenceEquals(_title, null) ? _title.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Logs = " + (!ReferenceEquals(_logs, null) ? _logs.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Actions = " + (!ReferenceEquals(_actions, null) ? (_actions.Count.ToString() + @" items" + __collection_Actions.ToString()) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    ActionDiscovery = " + (!ReferenceEquals(_actionDiscovery, null) ? (_actionDiscovery.Count.ToString() + @" items" + __collection_ActionDiscovery.ToString()) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    Perspectives = " + (!ReferenceEquals(_perspectives, null) ? (_perspectives.Count.ToString() + @" items" + __collection_Perspectives.ToString()) : @"null").Replace("\n", "\n    "))));
        }
        
        #endregion
        
        #region Change Tracking
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        
        #endregion
        
        #region Property Title
        
        private string _title;
        
        public event EventHandler TitleChanged;
        
        protected virtual void OnTitleChanged()
        {
            EventHandler handler = TitleChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Title");
        }
        
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
                this.OnTitleChanged();
            }
        }
        
        #endregion
        
        #region Property Logs
        
        private string _logs;
        
        public event EventHandler LogsChanged;
        
        protected virtual void OnLogsChanged()
        {
            EventHandler handler = LogsChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Logs");
        }
        
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
                this.OnLogsChanged();
            }
        }
        
        #endregion
        
        #region Property Actions
        
        private global::System.Collections.ObjectModel.ObservableCollection<CommandAction> _actions;
        
        public event EventHandler ActionsChanged;
        
        protected virtual void OnActionsChanged()
        {
            EventHandler handler = ActionsChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Actions");
        }
        
        public virtual global::System.Collections.ObjectModel.ObservableCollection<CommandAction> Actions
        {
            get { return _actions; }
            set
            {
                if ((value == _actions))
                {
                    return;
                }
                _actions = value;
                this.OnActionsChanged();
            }
        }
        
        #endregion
        
        #region Property ActionDiscovery
        
        private global::System.Collections.ObjectModel.ObservableCollection<CommandActionDiscovery> _actionDiscovery;
        
        public event EventHandler ActionDiscoveryChanged;
        
        protected virtual void OnActionDiscoveryChanged()
        {
            EventHandler handler = ActionDiscoveryChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"ActionDiscovery");
        }
        
        public virtual global::System.Collections.ObjectModel.ObservableCollection<CommandActionDiscovery> ActionDiscovery
        {
            get { return _actionDiscovery; }
            set
            {
                if ((value == _actionDiscovery))
                {
                    return;
                }
                _actionDiscovery = value;
                this.OnActionDiscoveryChanged();
            }
        }
        
        #endregion
        
        #region Property Perspectives
        
        private global::System.Collections.ObjectModel.ObservableCollection<string> _perspectives;
        
        public event EventHandler PerspectivesChanged;
        
        protected virtual void OnPerspectivesChanged()
        {
            EventHandler handler = PerspectivesChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Perspectives");
        }
        
        public virtual global::System.Collections.ObjectModel.ObservableCollection<string> Perspectives
        {
            get { return _perspectives; }
            set
            {
                if ((value == _perspectives))
                {
                    return;
                }
                _perspectives = value;
                this.OnPerspectivesChanged();
            }
        }
        
        #endregion
    }
    
    #endregion
}