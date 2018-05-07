using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Mastersign.DashOps
{
    #region Scaleton Model Designer generated code
    
    // Scaleton Version: 0.2.5
    
    public partial class ActionView : INotifyPropertyChanged
    {
        public ActionView()
        {
            this._tags = new string[0];
        }
        
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
        
        #region Property Reassure
        
        private bool _reassure;
        
        public event EventHandler ReassureChanged;
        
        protected virtual void OnReassureChanged()
        {
            EventHandler handler = ReassureChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Reassure");
        }
        
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
                this.OnReassureChanged();
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
        
        private string _arguments;
        
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
        
        public virtual string Arguments
        {
            get { return _arguments; }
            set
            {
                if (string.Equals(value, _arguments))
                {
                    return;
                }
                _arguments = value;
                this.OnArgumentsChanged();
            }
        }
        
        #endregion
        
        #region Property WorkingDirectory
        
        private string _workingDirectory;
        
        public event EventHandler WorkingDirectoryChanged;
        
        protected virtual void OnWorkingDirectoryChanged()
        {
            EventHandler handler = WorkingDirectoryChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"WorkingDirectory");
        }
        
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
                this.OnWorkingDirectoryChanged();
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
        
        #region Property NoLogs
        
        private bool _noLogs;
        
        public event EventHandler NoLogsChanged;
        
        protected virtual void OnNoLogsChanged()
        {
            EventHandler handler = NoLogsChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"NoLogs");
        }
        
        public virtual bool NoLogs
        {
            get { return _noLogs; }
            set
            {
                if ((value == _noLogs))
                {
                    return;
                }
                _noLogs = value;
                this.OnNoLogsChanged();
            }
        }
        
        #endregion
        
        #region Property KeepOpen
        
        private bool _keepOpen;
        
        public event EventHandler KeepOpenChanged;
        
        protected virtual void OnKeepOpenChanged()
        {
            EventHandler handler = KeepOpenChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"KeepOpen");
        }
        
        public virtual bool KeepOpen
        {
            get { return _keepOpen; }
            set
            {
                if ((value == _keepOpen))
                {
                    return;
                }
                _keepOpen = value;
                this.OnKeepOpenChanged();
            }
        }
        
        #endregion
        
        #region Property AlwaysClose
        
        private bool _alwaysClose;
        
        public event EventHandler AlwaysCloseChanged;
        
        protected virtual void OnAlwaysCloseChanged()
        {
            EventHandler handler = AlwaysCloseChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"AlwaysClose");
        }
        
        public virtual bool AlwaysClose
        {
            get { return _alwaysClose; }
            set
            {
                if ((value == _alwaysClose))
                {
                    return;
                }
                _alwaysClose = value;
                this.OnAlwaysCloseChanged();
            }
        }
        
        #endregion
        
        #region Property CurrentLogFile
        
        private string _currentLogFile;
        
        public event EventHandler CurrentLogFileChanged;
        
        protected virtual void OnCurrentLogFileChanged()
        {
            EventHandler handler = CurrentLogFileChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"CurrentLogFile");
        }
        
        public virtual string CurrentLogFile
        {
            get { return _currentLogFile; }
            set
            {
                if (string.Equals(value, _currentLogFile))
                {
                    return;
                }
                _currentLogFile = value;
                this.OnCurrentLogFileChanged();
            }
        }
        
        #endregion
        
        #region Property Visible
        
        private bool _visible;
        
        public event EventHandler VisibleChanged;
        
        protected virtual void OnVisibleChanged()
        {
            EventHandler handler = VisibleChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Visible");
        }
        
        public virtual bool Visible
        {
            get { return _visible; }
            set
            {
                if ((value == _visible))
                {
                    return;
                }
                _visible = value;
                this.OnVisibleChanged();
            }
        }
        
        #endregion
    }
    
    public partial class ActionSubset : INotifyPropertyChanged
    {
        public ActionSubset()
        {
            this._actions = new global::System.Collections.ObjectModel.ObservableCollection<ActionView>();
        }
        
        public ActionSubset(string title)
        {
            this._title = title;
            this._actions = new global::System.Collections.ObjectModel.ObservableCollection<ActionView>();
        }
        
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
        
        public virtual string Title
        {
            get { return _title; }
        }
        
        #endregion
        
        #region Property Actions
        
        private global::System.Collections.ObjectModel.ObservableCollection<ActionView> _actions;
        
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
        
        public virtual global::System.Collections.ObjectModel.ObservableCollection<ActionView> Actions
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
    }
    
    public partial class PerspectiveView : INotifyPropertyChanged
    {
        public PerspectiveView()
        {
            this._sourceActions = new global::System.Collections.ObjectModel.ObservableCollection<ActionView>();
            this._subsets = new global::System.Collections.ObjectModel.ObservableCollection<ActionSubset>();
            this.Initialize();
        }
        
        public PerspectiveView(string title, global::System.Collections.ObjectModel.ObservableCollection<ActionView> sourceActions, Func<ActionView, bool> filter, Func<ActionView, String[]> classifier)
        {
            this._title = title;
            this._sourceActions = sourceActions;
            this._filter = filter;
            this._classifier = classifier;
            this._subsets = new global::System.Collections.ObjectModel.ObservableCollection<ActionSubset>();
            this.Initialize();
        }
        
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
        
        public virtual string Title
        {
            get { return _title; }
        }
        
        #endregion
        
        #region Property SourceActions
        
        private global::System.Collections.ObjectModel.ObservableCollection<ActionView> _sourceActions;
        
        public virtual global::System.Collections.ObjectModel.ObservableCollection<ActionView> SourceActions
        {
            get { return _sourceActions; }
        }
        
        #endregion
        
        #region Property Filter
        
        private Func<ActionView, bool> _filter;
        
        public virtual Func<ActionView, bool> Filter
        {
            get { return _filter; }
        }
        
        #endregion
        
        #region Property Classifier
        
        private Func<ActionView, String[]> _classifier;
        
        public virtual Func<ActionView, String[]> Classifier
        {
            get { return _classifier; }
        }
        
        #endregion
        
        #region Property Subsets
        
        private global::System.Collections.ObjectModel.ObservableCollection<ActionSubset> _subsets;
        
        public event EventHandler SubsetsChanged;
        
        protected virtual void OnSubsetsChanged()
        {
            EventHandler handler = SubsetsChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Subsets");
        }
        
        public virtual global::System.Collections.ObjectModel.ObservableCollection<ActionSubset> Subsets
        {
            get { return _subsets; }
            set
            {
                if ((value == _subsets))
                {
                    return;
                }
                _subsets = value;
                this.OnSubsetsChanged();
            }
        }
        
        #endregion
        
        #region Property CurrentSubset
        
        private ActionSubset _currentSubset;
        
        public event EventHandler CurrentSubsetChanged;
        
        protected virtual void OnCurrentSubsetChanged()
        {
            EventHandler handler = CurrentSubsetChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"CurrentSubset");
        }
        
        public virtual ActionSubset CurrentSubset
        {
            get { return _currentSubset; }
            set
            {
                if ((value == _currentSubset))
                {
                    return;
                }
                _currentSubset = value;
                this.OnCurrentSubsetChanged();
            }
        }
        
        #endregion
    }
    
    public partial class MonitorView : INotifyPropertyChanged
    {
        public MonitorView()
        {
            this._requiredPatterns = new global::System.Text.RegularExpressions.Regex[0];
            this._forbiddenPatterns = new global::System.Text.RegularExpressions.Regex[0];
        }
        
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
        
        #region Property NoLogs
        
        private bool _noLogs;
        
        public event EventHandler NoLogsChanged;
        
        protected virtual void OnNoLogsChanged()
        {
            EventHandler handler = NoLogsChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"NoLogs");
        }
        
        public virtual bool NoLogs
        {
            get { return _noLogs; }
            set
            {
                if ((value == _noLogs))
                {
                    return;
                }
                _noLogs = value;
                this.OnNoLogsChanged();
            }
        }
        
        #endregion
        
        #region Property CurrentLogFile
        
        private string _currentLogFile;
        
        public event EventHandler CurrentLogFileChanged;
        
        protected virtual void OnCurrentLogFileChanged()
        {
            EventHandler handler = CurrentLogFileChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"CurrentLogFile");
        }
        
        public virtual string CurrentLogFile
        {
            get { return _currentLogFile; }
            set
            {
                if (string.Equals(value, _currentLogFile))
                {
                    return;
                }
                _currentLogFile = value;
                this.OnCurrentLogFileChanged();
            }
        }
        
        #endregion
        
        #region Property Interval
        
        private TimeSpan _interval;
        
        public event EventHandler IntervalChanged;
        
        protected virtual void OnIntervalChanged()
        {
            EventHandler handler = IntervalChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Interval");
        }
        
        public virtual TimeSpan Interval
        {
            get { return _interval; }
            set
            {
                if ((value == _interval))
                {
                    return;
                }
                _interval = value;
                this.OnIntervalChanged();
            }
        }
        
        #endregion
        
        #region Property RequiredPatterns
        
        private global::System.Text.RegularExpressions.Regex[] _requiredPatterns;
        
        public event EventHandler RequiredPatternsChanged;
        
        protected virtual void OnRequiredPatternsChanged()
        {
            EventHandler handler = RequiredPatternsChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"RequiredPatterns");
        }
        
        public virtual global::System.Text.RegularExpressions.Regex[] RequiredPatterns
        {
            get { return _requiredPatterns; }
            set
            {
                if ((value == _requiredPatterns))
                {
                    return;
                }
                _requiredPatterns = value;
                this.OnRequiredPatternsChanged();
            }
        }
        
        #endregion
        
        #region Property ForbiddenPatterns
        
        private global::System.Text.RegularExpressions.Regex[] _forbiddenPatterns;
        
        public event EventHandler ForbiddenPatternsChanged;
        
        protected virtual void OnForbiddenPatternsChanged()
        {
            EventHandler handler = ForbiddenPatternsChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"ForbiddenPatterns");
        }
        
        public virtual global::System.Text.RegularExpressions.Regex[] ForbiddenPatterns
        {
            get { return _forbiddenPatterns; }
            set
            {
                if ((value == _forbiddenPatterns))
                {
                    return;
                }
                _forbiddenPatterns = value;
                this.OnForbiddenPatternsChanged();
            }
        }
        
        #endregion
        
        #region Property IsRunning
        
        private bool _isRunning;
        
        public event EventHandler IsRunningChanged;
        
        protected virtual void OnIsRunningChanged()
        {
            EventHandler handler = IsRunningChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"IsRunning");
        }
        
        public virtual bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                if ((value == _isRunning))
                {
                    return;
                }
                _isRunning = value;
                this.OnIsRunningChanged();
            }
        }
        
        #endregion
        
        #region Property LastExecutionTime
        
        private DateTime _lastExecutionTime;
        
        public event EventHandler LastExecutionTimeChanged;
        
        protected virtual void OnLastExecutionTimeChanged()
        {
            EventHandler handler = LastExecutionTimeChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"LastExecutionTime");
        }
        
        public virtual DateTime LastExecutionTime
        {
            get { return _lastExecutionTime; }
            set
            {
                if ((value == _lastExecutionTime))
                {
                    return;
                }
                _lastExecutionTime = value;
                this.OnLastExecutionTimeChanged();
            }
        }
        
        #endregion
        
        #region Property HasLastExecutionResult
        
        private bool _hasLastExecutionResult;
        
        public event EventHandler HasLastExecutionResultChanged;
        
        protected virtual void OnHasLastExecutionResultChanged()
        {
            EventHandler handler = HasLastExecutionResultChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"HasLastExecutionResult");
        }
        
        public virtual bool HasLastExecutionResult
        {
            get { return _hasLastExecutionResult; }
            set
            {
                if ((value == _hasLastExecutionResult))
                {
                    return;
                }
                _hasLastExecutionResult = value;
                this.OnHasLastExecutionResultChanged();
            }
        }
        
        #endregion
        
        #region Property HasExecutionResultChanged
        
        private bool _hasExecutionResultChanged;
        
        public event EventHandler HasExecutionResultChangedChanged;
        
        protected virtual void OnHasExecutionResultChangedChanged()
        {
            EventHandler handler = HasExecutionResultChangedChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"HasExecutionResultChanged");
        }
        
        public virtual bool HasExecutionResultChanged
        {
            get { return _hasExecutionResultChanged; }
            set
            {
                if ((value == _hasExecutionResultChanged))
                {
                    return;
                }
                _hasExecutionResultChanged = value;
                this.OnHasExecutionResultChangedChanged();
            }
        }
        
        #endregion
        
        #region Property LastExecutionResult
        
        private bool _lastExecutionResult;
        
        public event EventHandler LastExecutionResultChanged;
        
        protected virtual void OnLastExecutionResultChanged()
        {
            EventHandler handler = LastExecutionResultChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"LastExecutionResult");
        }
        
        public virtual bool LastExecutionResult
        {
            get { return _lastExecutionResult; }
            set
            {
                if ((value == _lastExecutionResult))
                {
                    return;
                }
                _lastExecutionResult = value;
                this.OnLastExecutionResultChanged();
            }
        }
        
        #endregion
    }
    
    public partial class CommandMonitorView : MonitorView, INotifyPropertyChanged
    {
        public CommandMonitorView()
        {
        }
        
        #region Change Tracking
        
        
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
        
        private string _arguments;
        
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
        
        public virtual string Arguments
        {
            get { return _arguments; }
            set
            {
                if (string.Equals(value, _arguments))
                {
                    return;
                }
                _arguments = value;
                this.OnArgumentsChanged();
            }
        }
        
        #endregion
        
        #region Property WorkingDirectory
        
        private string _workingDirectory;
        
        public event EventHandler WorkingDirectoryChanged;
        
        protected virtual void OnWorkingDirectoryChanged()
        {
            EventHandler handler = WorkingDirectoryChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"WorkingDirectory");
        }
        
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
                this.OnWorkingDirectoryChanged();
            }
        }
        
        #endregion
    }
    
    public partial class WebMonitorView : MonitorView, INotifyPropertyChanged
    {
        public WebMonitorView()
        {
            this._statusCodes = new int[0];
        }
        
        #region Change Tracking
        
        
        #endregion
        
        #region Property Url
        
        private string _url;
        
        public event EventHandler UrlChanged;
        
        protected virtual void OnUrlChanged()
        {
            EventHandler handler = UrlChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Url");
        }
        
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
                this.OnUrlChanged();
            }
        }
        
        #endregion
        
        #region Property Headers
        
        private Dictionary<string, string> _headers;
        
        public event EventHandler HeadersChanged;
        
        protected virtual void OnHeadersChanged()
        {
            EventHandler handler = HeadersChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Headers");
        }
        
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
                this.OnHeadersChanged();
            }
        }
        
        #endregion
        
        #region Property StatusCodes
        
        private int[] _statusCodes;
        
        public event EventHandler StatusCodesChanged;
        
        protected virtual void OnStatusCodesChanged()
        {
            EventHandler handler = StatusCodesChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"StatusCodes");
        }
        
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
                this.OnStatusCodesChanged();
            }
        }
        
        #endregion
    }
    
    public partial class ProjectView : INotifyPropertyChanged
    {
        public ProjectView()
        {
            this._actionViews = new global::System.Collections.ObjectModel.ObservableCollection<ActionView>();
            this._perspectives = new global::System.Collections.ObjectModel.ObservableCollection<PerspectiveView>();
            this._monitorViews = new global::System.Collections.ObjectModel.ObservableCollection<MonitorView>();
        }
        
        public ProjectView(global::System.Collections.ObjectModel.ObservableCollection<ActionView> actionViews, global::System.Collections.ObjectModel.ObservableCollection<PerspectiveView> perspectives, global::System.Collections.ObjectModel.ObservableCollection<MonitorView> monitorViews)
        {
            this._actionViews = actionViews;
            this._perspectives = perspectives;
            this._monitorViews = monitorViews;
        }
        
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
        
        #region Property ActionViews
        
        private global::System.Collections.ObjectModel.ObservableCollection<ActionView> _actionViews;
        
        public virtual global::System.Collections.ObjectModel.ObservableCollection<ActionView> ActionViews
        {
            get { return _actionViews; }
        }
        
        #endregion
        
        #region Property Perspectives
        
        private global::System.Collections.ObjectModel.ObservableCollection<PerspectiveView> _perspectives;
        
        public virtual global::System.Collections.ObjectModel.ObservableCollection<PerspectiveView> Perspectives
        {
            get { return _perspectives; }
        }
        
        #endregion
        
        #region Property CurrentPerspective
        
        private PerspectiveView _currentPerspective;
        
        public event EventHandler CurrentPerspectiveChanged;
        
        protected virtual void OnCurrentPerspectiveChanged()
        {
            EventHandler handler = CurrentPerspectiveChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"CurrentPerspective");
        }
        
        public virtual PerspectiveView CurrentPerspective
        {
            get { return _currentPerspective; }
            set
            {
                if ((value == _currentPerspective))
                {
                    return;
                }
                _currentPerspective = value;
                this.OnCurrentPerspectiveChanged();
            }
        }
        
        #endregion
        
        #region Property MonitorViews
        
        private global::System.Collections.ObjectModel.ObservableCollection<MonitorView> _monitorViews;
        
        public virtual global::System.Collections.ObjectModel.ObservableCollection<MonitorView> MonitorViews
        {
            get { return _monitorViews; }
        }
        
        #endregion
        
        #region Property DefaultMonitorInterval
        
        private TimeSpan _defaultMonitorInterval;
        
        public event EventHandler DefaultMonitorIntervalChanged;
        
        protected virtual void OnDefaultMonitorIntervalChanged()
        {
            EventHandler handler = DefaultMonitorIntervalChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"DefaultMonitorInterval");
        }
        
        public virtual TimeSpan DefaultMonitorInterval
        {
            get { return _defaultMonitorInterval; }
            set
            {
                if ((value == _defaultMonitorInterval))
                {
                    return;
                }
                _defaultMonitorInterval = value;
                this.OnDefaultMonitorIntervalChanged();
            }
        }
        
        #endregion
        
        #region Property IsMonitoringPaused
        
        private bool _isMonitoringPaused;
        
        public event EventHandler IsMonitoringPausedChanged;
        
        protected virtual void OnIsMonitoringPausedChanged()
        {
            EventHandler handler = IsMonitoringPausedChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"IsMonitoringPaused");
        }
        
        public virtual bool IsMonitoringPaused
        {
            get { return _isMonitoringPaused; }
            set
            {
                if ((value == _isMonitoringPaused))
                {
                    return;
                }
                _isMonitoringPaused = value;
                this.OnIsMonitoringPausedChanged();
            }
        }
        
        #endregion
    }
    
    #endregion
}