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
            this._arguments = new string[0];
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
    }
    
    public partial class ProjectView : INotifyPropertyChanged
    {
        public ProjectView()
        {
            this._actionViews = new global::System.Collections.ObjectModel.ObservableCollection<ActionView>();
            this._perspectives = new global::System.Collections.ObjectModel.ObservableCollection<PerspectiveView>();
        }
        
        public ProjectView(global::System.Collections.ObjectModel.ObservableCollection<ActionView> actionViews, global::System.Collections.ObjectModel.ObservableCollection<PerspectiveView> perspectives)
        {
            this._actionViews = actionViews;
            this._perspectives = perspectives;
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
    }
    
    #endregion
}