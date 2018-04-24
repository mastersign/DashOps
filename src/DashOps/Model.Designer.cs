using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Mastersign.DashOps.Model
{
    #region Scaleton Model Designer generated code
    
    // Scaleton Version: 0.2.5
    
    public partial class CommandAction
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
    
    public partial class CommandActionDiscovery
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
                (Environment.NewLine + @"    Description = " + (!ReferenceEquals(_description, null) ? _description.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
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
        
        #region Property PathRegex
        
        private string _pathRegex;
        
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
    
    public partial class Project
    {
        public Project()
        {
            this._actions = new List<CommandAction>();
            this._actionDiscovery = new List<CommandActionDiscovery>();
            this._perspectives = new List<string>();
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
    }
    
    #endregion
}