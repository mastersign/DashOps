using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Mastersign.DashOps
{
    #region Scaleton Model Designer generated code
    
    // Scaleton Version: 0.3.2
    
    public enum ActionStatus
    {
        Unknown,
        StartError,
        Running,
        Success,
        SuccessWithoutLogFile,
        Failed,
        FailedWithoutLogFile,
    }
    
    public enum WindowMode
    {
        Default = 0,
        Fixed = 1,
        Auto = 2,
    }
    
    public partial class ActionView : INotifyPropertyChanged
    {
        public ActionView()
        {
            this._exePaths = new string[0];
            this._exitCodes = new int[0];
            this._tags = new string[0];
            this._facetViews = new global::System.Collections.ObjectModel.ObservableCollection<FacetView>();
            this._status = DEF_STATUS;
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
        
        #region Property UsePowerShellCore
        
        private bool _usePowerShellCore;
        
        public event EventHandler UsePowerShellCoreChanged;
        
        protected virtual void OnUsePowerShellCoreChanged()
        {
            EventHandler handler = UsePowerShellCoreChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"UsePowerShellCore");
        }
        
        public virtual bool UsePowerShellCore
        {
            get { return _usePowerShellCore; }
            set
            {
                if ((value == _usePowerShellCore))
                {
                    return;
                }
                _usePowerShellCore = value;
                this.OnUsePowerShellCoreChanged();
            }
        }
        
        #endregion
        
        #region Property PowerShellExe
        
        private string _powerShellExe;
        
        public event EventHandler PowerShellExeChanged;
        
        protected virtual void OnPowerShellExeChanged()
        {
            EventHandler handler = PowerShellExeChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"PowerShellExe");
        }
        
        public virtual string PowerShellExe
        {
            get { return _powerShellExe; }
            set
            {
                if (string.Equals(value, _powerShellExe))
                {
                    return;
                }
                _powerShellExe = value;
                this.OnPowerShellExeChanged();
            }
        }
        
        #endregion
        
        #region Property UsePowerShellProfile
        
        private bool _usePowerShellProfile;
        
        public event EventHandler UsePowerShellProfileChanged;
        
        protected virtual void OnUsePowerShellProfileChanged()
        {
            EventHandler handler = UsePowerShellProfileChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"UsePowerShellProfile");
        }
        
        public virtual bool UsePowerShellProfile
        {
            get { return _usePowerShellProfile; }
            set
            {
                if ((value == _usePowerShellProfile))
                {
                    return;
                }
                _usePowerShellProfile = value;
                this.OnUsePowerShellProfileChanged();
            }
        }
        
        #endregion
        
        #region Property PowerShellExecutionPolicy
        
        private string _powerShellExecutionPolicy;
        
        public event EventHandler PowerShellExecutionPolicyChanged;
        
        protected virtual void OnPowerShellExecutionPolicyChanged()
        {
            EventHandler handler = PowerShellExecutionPolicyChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"PowerShellExecutionPolicy");
        }
        
        public virtual string PowerShellExecutionPolicy
        {
            get { return _powerShellExecutionPolicy; }
            set
            {
                if (string.Equals(value, _powerShellExecutionPolicy))
                {
                    return;
                }
                _powerShellExecutionPolicy = value;
                this.OnPowerShellExecutionPolicyChanged();
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
        
        #region Property Environment
        
        private Dictionary<string, string> _environment;
        
        public event EventHandler EnvironmentChanged;
        
        protected virtual void OnEnvironmentChanged()
        {
            EventHandler handler = EnvironmentChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Environment");
        }
        
        public virtual Dictionary<string, string> Environment
        {
            get { return _environment; }
            set
            {
                if ((value == _environment))
                {
                    return;
                }
                _environment = value;
                this.OnEnvironmentChanged();
            }
        }
        
        #endregion
        
        #region Property ExePaths
        
        private string[] _exePaths;
        
        public event EventHandler ExePathsChanged;
        
        protected virtual void OnExePathsChanged()
        {
            EventHandler handler = ExePathsChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"ExePaths");
        }
        
        public virtual string[] ExePaths
        {
            get { return _exePaths; }
            set
            {
                if ((value == _exePaths))
                {
                    return;
                }
                _exePaths = value;
                this.OnExePathsChanged();
            }
        }
        
        #endregion
        
        #region Property UseWindowsTerminal
        
        private bool _useWindowsTerminal;
        
        public event EventHandler UseWindowsTerminalChanged;
        
        protected virtual void OnUseWindowsTerminalChanged()
        {
            EventHandler handler = UseWindowsTerminalChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"UseWindowsTerminal");
        }
        
        public virtual bool UseWindowsTerminal
        {
            get { return _useWindowsTerminal; }
            set
            {
                if ((value == _useWindowsTerminal))
                {
                    return;
                }
                _useWindowsTerminal = value;
                this.OnUseWindowsTerminalChanged();
            }
        }
        
        #endregion
        
        #region Property WindowsTerminalArguments
        
        private string _windowsTerminalArguments;
        
        public event EventHandler WindowsTerminalArgumentsChanged;
        
        protected virtual void OnWindowsTerminalArgumentsChanged()
        {
            EventHandler handler = WindowsTerminalArgumentsChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"WindowsTerminalArguments");
        }
        
        public virtual string WindowsTerminalArguments
        {
            get { return _windowsTerminalArguments; }
            set
            {
                if (string.Equals(value, _windowsTerminalArguments))
                {
                    return;
                }
                _windowsTerminalArguments = value;
                this.OnWindowsTerminalArgumentsChanged();
            }
        }
        
        #endregion
        
        #region Property ExitCodes
        
        private int[] _exitCodes;
        
        public event EventHandler ExitCodesChanged;
        
        protected virtual void OnExitCodesChanged()
        {
            EventHandler handler = ExitCodesChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"ExitCodes");
        }
        
        public virtual int[] ExitCodes
        {
            get { return _exitCodes; }
            set
            {
                if ((value == _exitCodes))
                {
                    return;
                }
                _exitCodes = value;
                this.OnExitCodesChanged();
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
        
        #region Property Facets
        
        private Dictionary<string, string> _facets;
        
        public event EventHandler FacetsChanged;
        
        protected virtual void OnFacetsChanged()
        {
            EventHandler handler = FacetsChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Facets");
        }
        
        public virtual Dictionary<string, string> Facets
        {
            get { return _facets; }
            set
            {
                if ((value == _facets))
                {
                    return;
                }
                _facets = value;
                this.OnFacetsChanged();
            }
        }
        
        #endregion
        
        #region Property FacetViews
        
        private global::System.Collections.ObjectModel.ObservableCollection<FacetView> _facetViews;
        
        public event EventHandler FacetViewsChanged;
        
        protected virtual void OnFacetViewsChanged()
        {
            EventHandler handler = FacetViewsChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"FacetViews");
        }
        
        public virtual global::System.Collections.ObjectModel.ObservableCollection<FacetView> FacetViews
        {
            get { return _facetViews; }
            set
            {
                if ((value == _facetViews))
                {
                    return;
                }
                _facetViews = value;
                this.OnFacetViewsChanged();
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
        
        #region Property NoExecutionInfo
        
        private bool _noExecutionInfo;
        
        public event EventHandler NoExecutionInfoChanged;
        
        protected virtual void OnNoExecutionInfoChanged()
        {
            EventHandler handler = NoExecutionInfoChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"NoExecutionInfo");
        }
        
        public virtual bool NoExecutionInfo
        {
            get { return _noExecutionInfo; }
            set
            {
                if ((value == _noExecutionInfo))
                {
                    return;
                }
                _noExecutionInfo = value;
                this.OnNoExecutionInfoChanged();
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
        
        #region Property Status
        
        private ActionStatus _status;
        
        public event EventHandler StatusChanged;
        
        protected virtual void OnStatusChanged()
        {
            EventHandler handler = StatusChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Status");
        }
        
        private const ActionStatus DEF_STATUS = ActionStatus.Unknown;
        
        [DefaultValue(DEF_STATUS)]
        public virtual ActionStatus Status
        {
            get { return _status; }
            set
            {
                if ((value == _status))
                {
                    return;
                }
                _status = value;
                this.OnStatusChanged();
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
        
        public PerspectiveView(string title, string facet, global::System.Collections.ObjectModel.ObservableCollection<ActionView> sourceActions, Func<ActionView, bool> filter, Func<ActionView, string[]> classifier)
        {
            this._title = title;
            this._facet = facet;
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
        
        #region Property Facet
        
        private string _facet;
        
        public virtual string Facet
        {
            get { return _facet; }
        }
        
        #endregion
        
        #region Property IsSelected
        
        private bool _isSelected;
        
        public event EventHandler IsSelectedChanged;
        
        protected virtual void OnIsSelectedChanged()
        {
            EventHandler handler = IsSelectedChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"IsSelected");
        }
        
        public virtual bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if ((value == _isSelected))
                {
                    return;
                }
                _isSelected = value;
                this.OnIsSelectedChanged();
            }
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
        
        private Func<ActionView, string[]> _classifier;
        
        public virtual Func<ActionView, string[]> Classifier
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
            this._tags = new string[0];
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
        
        #region Property NoExecutionInfo
        
        private bool _noExecutionInfo;
        
        public event EventHandler NoExecutionInfoChanged;
        
        protected virtual void OnNoExecutionInfoChanged()
        {
            EventHandler handler = NoExecutionInfoChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"NoExecutionInfo");
        }
        
        public virtual bool NoExecutionInfo
        {
            get { return _noExecutionInfo; }
            set
            {
                if ((value == _noExecutionInfo))
                {
                    return;
                }
                _noExecutionInfo = value;
                this.OnNoExecutionInfoChanged();
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
        
        #region Property Deactivated
        
        private bool _deactivated;
        
        public event EventHandler DeactivatedChanged;
        
        protected virtual void OnDeactivatedChanged()
        {
            EventHandler handler = DeactivatedChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Deactivated");
        }
        
        public virtual bool Deactivated
        {
            get { return _deactivated; }
            set
            {
                if ((value == _deactivated))
                {
                    return;
                }
                _deactivated = value;
                this.OnDeactivatedChanged();
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
            this._exePaths = new string[0];
            this._exitCodes = new int[0];
        }
        
        #region Change Tracking
        
        
        #endregion
        
        #region Property UsePowerShellCore
        
        private bool _usePowerShellCore;
        
        public event EventHandler UsePowerShellCoreChanged;
        
        protected virtual void OnUsePowerShellCoreChanged()
        {
            EventHandler handler = UsePowerShellCoreChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"UsePowerShellCore");
        }
        
        public virtual bool UsePowerShellCore
        {
            get { return _usePowerShellCore; }
            set
            {
                if ((value == _usePowerShellCore))
                {
                    return;
                }
                _usePowerShellCore = value;
                this.OnUsePowerShellCoreChanged();
            }
        }
        
        #endregion
        
        #region Property PowerShellExe
        
        private string _powerShellExe;
        
        public event EventHandler PowerShellExeChanged;
        
        protected virtual void OnPowerShellExeChanged()
        {
            EventHandler handler = PowerShellExeChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"PowerShellExe");
        }
        
        public virtual string PowerShellExe
        {
            get { return _powerShellExe; }
            set
            {
                if (string.Equals(value, _powerShellExe))
                {
                    return;
                }
                _powerShellExe = value;
                this.OnPowerShellExeChanged();
            }
        }
        
        #endregion
        
        #region Property UsePowerShellProfile
        
        private bool _usePowerShellProfile;
        
        public event EventHandler UsePowerShellProfileChanged;
        
        protected virtual void OnUsePowerShellProfileChanged()
        {
            EventHandler handler = UsePowerShellProfileChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"UsePowerShellProfile");
        }
        
        public virtual bool UsePowerShellProfile
        {
            get { return _usePowerShellProfile; }
            set
            {
                if ((value == _usePowerShellProfile))
                {
                    return;
                }
                _usePowerShellProfile = value;
                this.OnUsePowerShellProfileChanged();
            }
        }
        
        #endregion
        
        #region Property PowerShellExecutionPolicy
        
        private string _powerShellExecutionPolicy;
        
        public event EventHandler PowerShellExecutionPolicyChanged;
        
        protected virtual void OnPowerShellExecutionPolicyChanged()
        {
            EventHandler handler = PowerShellExecutionPolicyChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"PowerShellExecutionPolicy");
        }
        
        public virtual string PowerShellExecutionPolicy
        {
            get { return _powerShellExecutionPolicy; }
            set
            {
                if (string.Equals(value, _powerShellExecutionPolicy))
                {
                    return;
                }
                _powerShellExecutionPolicy = value;
                this.OnPowerShellExecutionPolicyChanged();
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
        
        #region Property Environment
        
        private Dictionary<string, string> _environment;
        
        public event EventHandler EnvironmentChanged;
        
        protected virtual void OnEnvironmentChanged()
        {
            EventHandler handler = EnvironmentChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Environment");
        }
        
        public virtual Dictionary<string, string> Environment
        {
            get { return _environment; }
            set
            {
                if ((value == _environment))
                {
                    return;
                }
                _environment = value;
                this.OnEnvironmentChanged();
            }
        }
        
        #endregion
        
        #region Property ExePaths
        
        private string[] _exePaths;
        
        public event EventHandler ExePathsChanged;
        
        protected virtual void OnExePathsChanged()
        {
            EventHandler handler = ExePathsChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"ExePaths");
        }
        
        public virtual string[] ExePaths
        {
            get { return _exePaths; }
            set
            {
                if ((value == _exePaths))
                {
                    return;
                }
                _exePaths = value;
                this.OnExePathsChanged();
            }
        }
        
        #endregion
        
        #region Property ExitCodes
        
        private int[] _exitCodes;
        
        public event EventHandler ExitCodesChanged;
        
        protected virtual void OnExitCodesChanged()
        {
            EventHandler handler = ExitCodesChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"ExitCodes");
        }
        
        public virtual int[] ExitCodes
        {
            get { return _exitCodes; }
            set
            {
                if ((value == _exitCodes))
                {
                    return;
                }
                _exitCodes = value;
                this.OnExitCodesChanged();
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
        
        #region Property Timeout
        
        private TimeSpan _timeout;
        
        public event EventHandler TimeoutChanged;
        
        protected virtual void OnTimeoutChanged()
        {
            EventHandler handler = TimeoutChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Timeout");
        }
        
        public virtual TimeSpan Timeout
        {
            get { return _timeout; }
            set
            {
                if ((value == _timeout))
                {
                    return;
                }
                _timeout = value;
                this.OnTimeoutChanged();
            }
        }
        
        #endregion
        
        #region Property ServerCertificateHash
        
        private string _serverCertificateHash;
        
        public event EventHandler ServerCertificateHashChanged;
        
        protected virtual void OnServerCertificateHashChanged()
        {
            EventHandler handler = ServerCertificateHashChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"ServerCertificateHash");
        }
        
        public virtual string ServerCertificateHash
        {
            get { return _serverCertificateHash; }
            set
            {
                if (string.Equals(value, _serverCertificateHash))
                {
                    return;
                }
                _serverCertificateHash = value;
                this.OnServerCertificateHashChanged();
            }
        }
        
        #endregion
        
        #region Property NoTlsVerify
        
        private bool _noTlsVerify;
        
        public event EventHandler NoTlsVerifyChanged;
        
        protected virtual void OnNoTlsVerifyChanged()
        {
            EventHandler handler = NoTlsVerifyChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"NoTlsVerify");
        }
        
        public virtual bool NoTlsVerify
        {
            get { return _noTlsVerify; }
            set
            {
                if ((value == _noTlsVerify))
                {
                    return;
                }
                _noTlsVerify = value;
                this.OnNoTlsVerifyChanged();
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
    
    public partial class FacetView : INotifyPropertyChanged
    {
        public FacetView()
        {
        }
        
        public FacetView(string facet, string title, string value)
        {
            this._facet = facet;
            this._title = title;
            this._value = value;
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
        
        #region Property Facet
        
        private string _facet;
        
        public virtual string Facet
        {
            get { return _facet; }
        }
        
        #endregion
        
        #region Property Title
        
        private string _title;
        
        public virtual string Title
        {
            get { return _title; }
        }
        
        #endregion
        
        #region Property Value
        
        private string _value;
        
        public virtual string Value
        {
            get { return _value; }
        }
        
        #endregion
    }
    
    public partial class WindowSettings : INotifyPropertyChanged
    {
        public WindowSettings()
        {
            this._mode = DEF_MODE;
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
        
        #region Property Mode
        
        private WindowMode _mode;
        
        public event EventHandler ModeChanged;
        
        protected virtual void OnModeChanged()
        {
            EventHandler handler = ModeChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Mode");
        }
        
        private const WindowMode DEF_MODE = WindowMode.Default;
        
        [DefaultValue(DEF_MODE)]
        public virtual WindowMode Mode
        {
            get { return _mode; }
            set
            {
                if ((value == _mode))
                {
                    return;
                }
                _mode = value;
                this.OnModeChanged();
            }
        }
        
        #endregion
        
        #region Property ScreenNo
        
        private int? _screenNo;
        
        public event EventHandler ScreenNoChanged;
        
        protected virtual void OnScreenNoChanged()
        {
            EventHandler handler = ScreenNoChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"ScreenNo");
        }
        
        public virtual int? ScreenNo
        {
            get { return _screenNo; }
            set
            {
                if ((value == _screenNo))
                {
                    return;
                }
                _screenNo = value;
                this.OnScreenNoChanged();
            }
        }
        
        #endregion
        
        #region Property Left
        
        private int? _left;
        
        public event EventHandler LeftChanged;
        
        protected virtual void OnLeftChanged()
        {
            EventHandler handler = LeftChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Left");
        }
        
        public virtual int? Left
        {
            get { return _left; }
            set
            {
                if ((value == _left))
                {
                    return;
                }
                _left = value;
                this.OnLeftChanged();
            }
        }
        
        #endregion
        
        #region Property Top
        
        private int? _top;
        
        public event EventHandler TopChanged;
        
        protected virtual void OnTopChanged()
        {
            EventHandler handler = TopChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Top");
        }
        
        public virtual int? Top
        {
            get { return _top; }
            set
            {
                if ((value == _top))
                {
                    return;
                }
                _top = value;
                this.OnTopChanged();
            }
        }
        
        #endregion
        
        #region Property Width
        
        private int? _width;
        
        public event EventHandler WidthChanged;
        
        protected virtual void OnWidthChanged()
        {
            EventHandler handler = WidthChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Width");
        }
        
        public virtual int? Width
        {
            get { return _width; }
            set
            {
                if ((value == _width))
                {
                    return;
                }
                _width = value;
                this.OnWidthChanged();
            }
        }
        
        #endregion
        
        #region Property Height
        
        private int? _height;
        
        public event EventHandler HeightChanged;
        
        protected virtual void OnHeightChanged()
        {
            EventHandler handler = HeightChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Height");
        }
        
        public virtual int? Height
        {
            get { return _height; }
            set
            {
                if ((value == _height))
                {
                    return;
                }
                _height = value;
                this.OnHeightChanged();
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
            this.Initialize();
        }
        
        public ProjectView(global::System.Collections.ObjectModel.ObservableCollection<ActionView> actionViews, global::System.Collections.ObjectModel.ObservableCollection<PerspectiveView> perspectives, global::System.Collections.ObjectModel.ObservableCollection<MonitorView> monitorViews)
        {
            this._actionViews = actionViews;
            this._perspectives = perspectives;
            this._monitorViews = monitorViews;
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
        
        #region Property FormatVersion
        
        private string _formatVersion;
        
        public event EventHandler FormatVersionChanged;
        
        protected virtual void OnFormatVersionChanged()
        {
            EventHandler handler = FormatVersionChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"FormatVersion");
        }
        
        public virtual string FormatVersion
        {
            get { return _formatVersion; }
            set
            {
                if (string.Equals(value, _formatVersion))
                {
                    return;
                }
                _formatVersion = value;
                this.OnFormatVersionChanged();
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
        
        #region Property MainWindow
        
        private WindowSettings _mainWindow;
        
        public event EventHandler MainWindowChanged;
        
        protected virtual void OnMainWindowChanged()
        {
            EventHandler handler = MainWindowChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"MainWindow");
        }
        
        public virtual WindowSettings MainWindow
        {
            get { return _mainWindow; }
            set
            {
                if ((value == _mainWindow))
                {
                    return;
                }
                _mainWindow = value;
                this.OnMainWindowChanged();
            }
        }
        
        #endregion
        
        #region Property EditorWindow
        
        private WindowSettings _editorWindow;
        
        public event EventHandler EditorWindowChanged;
        
        protected virtual void OnEditorWindowChanged()
        {
            EventHandler handler = EditorWindowChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"EditorWindow");
        }
        
        public virtual WindowSettings EditorWindow
        {
            get { return _editorWindow; }
            set
            {
                if ((value == _editorWindow))
                {
                    return;
                }
                _editorWindow = value;
                this.OnEditorWindowChanged();
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