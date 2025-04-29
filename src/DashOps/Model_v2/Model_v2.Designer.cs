using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Mastersign.DashOps.Model_v2
{
    #region Scaleton Model Designer generated code
    
    // Scaleton Version: 0.3.2
    
    public enum ActionMatchMode
    {
        Description,
        Command,
        Facet,
        Tag,
    }
    
    public partial class CommandActionBase
    {
        public CommandActionBase()
        {
        }
        
        #region String Representation
        
        public override string ToString()
        {
            return this.ToString(CultureInfo.CurrentUICulture);
        }
        
        public virtual string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (global::System.Environment.NewLine + @"    Description = " + (!ReferenceEquals(_description, null) ? _description.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Reassure = " + (!ReferenceEquals(_reassure, null) ? _reassure.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Background = " + (!ReferenceEquals(_background, null) ? _background.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Logs = " + (!ReferenceEquals(_logs, null) ? _logs.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoLogs = " + (!ReferenceEquals(_noLogs, null) ? _noLogs.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoExecutionInfo = " + (!ReferenceEquals(_noExecutionInfo, null) ? _noExecutionInfo.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    KeepOpen = " + (!ReferenceEquals(_keepOpen, null) ? _keepOpen.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    AlwaysClose = " + (!ReferenceEquals(_alwaysClose, null) ? _alwaysClose.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    UsePowerShellCore = " + (!ReferenceEquals(_usePowerShellCore, null) ? _usePowerShellCore.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    PowerShellExe = " + (!ReferenceEquals(_powerShellExe, null) ? _powerShellExe.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    UsePowerShellProfile = " + (!ReferenceEquals(_usePowerShellProfile, null) ? _usePowerShellProfile.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    PowerShellExecutionPolicy = " + (!ReferenceEquals(_powerShellExecutionPolicy, null) ? _powerShellExecutionPolicy.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Arguments = " + (!ReferenceEquals(_arguments, null) ? _arguments.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    WorkingDirectory = " + (!ReferenceEquals(_workingDirectory, null) ? _workingDirectory.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Environment = " + (!ReferenceEquals(_environment, null) ? _environment.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ExePaths = " + (!ReferenceEquals(_exePaths, null) ? _exePaths.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    UseWindowsTerminal = " + (!ReferenceEquals(_useWindowsTerminal, null) ? _useWindowsTerminal.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    WindowsTerminalArgs = " + (!ReferenceEquals(_windowsTerminalArgs, null) ? _windowsTerminalArgs.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ExitCodes = " + (!ReferenceEquals(_exitCodes, null) ? _exitCodes.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Tags = " + (!ReferenceEquals(_tags, null) ? _tags.ToString() : @"null").Replace("\n", "\n    "))));
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
        
        private bool? _reassure;
        
        public virtual bool? Reassure
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
        
        #region Property Background
        
        private bool? _background;
        
        public virtual bool? Background
        {
            get { return _background; }
            set
            {
                if ((value == _background))
                {
                    return;
                }
                _background = value;
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
        
        #region Property NoLogs
        
        private bool? _noLogs;
        
        public virtual bool? NoLogs
        {
            get { return _noLogs; }
            set
            {
                if ((value == _noLogs))
                {
                    return;
                }
                _noLogs = value;
            }
        }
        
        #endregion
        
        #region Property NoExecutionInfo
        
        private bool? _noExecutionInfo;
        
        public virtual bool? NoExecutionInfo
        {
            get { return _noExecutionInfo; }
            set
            {
                if ((value == _noExecutionInfo))
                {
                    return;
                }
                _noExecutionInfo = value;
            }
        }
        
        #endregion
        
        #region Property KeepOpen
        
        private bool? _keepOpen;
        
        public virtual bool? KeepOpen
        {
            get { return _keepOpen; }
            set
            {
                if ((value == _keepOpen))
                {
                    return;
                }
                _keepOpen = value;
            }
        }
        
        #endregion
        
        #region Property AlwaysClose
        
        private bool? _alwaysClose;
        
        public virtual bool? AlwaysClose
        {
            get { return _alwaysClose; }
            set
            {
                if ((value == _alwaysClose))
                {
                    return;
                }
                _alwaysClose = value;
            }
        }
        
        #endregion
        
        #region Property UsePowerShellCore
        
        private bool? _usePowerShellCore;
        
        public virtual bool? UsePowerShellCore
        {
            get { return _usePowerShellCore; }
            set
            {
                if ((value == _usePowerShellCore))
                {
                    return;
                }
                _usePowerShellCore = value;
            }
        }
        
        #endregion
        
        #region Property PowerShellExe
        
        private string _powerShellExe;
        
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
            }
        }
        
        #endregion
        
        #region Property UsePowerShellProfile
        
        private bool? _usePowerShellProfile;
        
        public virtual bool? UsePowerShellProfile
        {
            get { return _usePowerShellProfile; }
            set
            {
                if ((value == _usePowerShellProfile))
                {
                    return;
                }
                _usePowerShellProfile = value;
            }
        }
        
        #endregion
        
        #region Property PowerShellExecutionPolicy
        
        private string _powerShellExecutionPolicy;
        
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
        
        #region Property Environment
        
        private Dictionary<string, string> _environment;
        
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
            }
        }
        
        #endregion
        
        #region Property ExePaths
        
        private string[] _exePaths;
        
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
            }
        }
        
        #endregion
        
        #region Property UseWindowsTerminal
        
        private bool? _useWindowsTerminal;
        
        public virtual bool? UseWindowsTerminal
        {
            get { return _useWindowsTerminal; }
            set
            {
                if ((value == _useWindowsTerminal))
                {
                    return;
                }
                _useWindowsTerminal = value;
            }
        }
        
        #endregion
        
        #region Property WindowsTerminalArgs
        
        private string[] _windowsTerminalArgs;
        
        public virtual string[] WindowsTerminalArgs
        {
            get { return _windowsTerminalArgs; }
            set
            {
                if ((value == _windowsTerminalArgs))
                {
                    return;
                }
                _windowsTerminalArgs = value;
            }
        }
        
        #endregion
        
        #region Property ExitCodes
        
        private int[] _exitCodes;
        
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
            return this.ToString(CultureInfo.CurrentUICulture);
        }
        
        public override string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (global::System.Environment.NewLine + @"    Description = " + (!ReferenceEquals(Description, null) ? Description.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Reassure = " + (!ReferenceEquals(Reassure, null) ? Reassure.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Background = " + (!ReferenceEquals(Background, null) ? Background.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Logs = " + (!ReferenceEquals(Logs, null) ? Logs.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoLogs = " + (!ReferenceEquals(NoLogs, null) ? NoLogs.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoExecutionInfo = " + (!ReferenceEquals(NoExecutionInfo, null) ? NoExecutionInfo.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    KeepOpen = " + (!ReferenceEquals(KeepOpen, null) ? KeepOpen.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    AlwaysClose = " + (!ReferenceEquals(AlwaysClose, null) ? AlwaysClose.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    UsePowerShellCore = " + (!ReferenceEquals(UsePowerShellCore, null) ? UsePowerShellCore.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    PowerShellExe = " + (!ReferenceEquals(PowerShellExe, null) ? PowerShellExe.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    UsePowerShellProfile = " + (!ReferenceEquals(UsePowerShellProfile, null) ? UsePowerShellProfile.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    PowerShellExecutionPolicy = " + (!ReferenceEquals(PowerShellExecutionPolicy, null) ? PowerShellExecutionPolicy.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Arguments = " + (!ReferenceEquals(Arguments, null) ? Arguments.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    WorkingDirectory = " + (!ReferenceEquals(WorkingDirectory, null) ? WorkingDirectory.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Environment = " + (!ReferenceEquals(Environment, null) ? Environment.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ExePaths = " + (!ReferenceEquals(ExePaths, null) ? ExePaths.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    UseWindowsTerminal = " + (!ReferenceEquals(UseWindowsTerminal, null) ? UseWindowsTerminal.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    WindowsTerminalArgs = " + (!ReferenceEquals(WindowsTerminalArgs, null) ? WindowsTerminalArgs.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ExitCodes = " + (!ReferenceEquals(ExitCodes, null) ? ExitCodes.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Tags = " + (!ReferenceEquals(Tags, null) ? Tags.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Command = " + (!ReferenceEquals(_command, null) ? _command.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Facets = " + (!ReferenceEquals(_facets, null) ? _facets.ToString() : @"null").Replace("\n", "\n    "))));
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
        
        #region Property Facets
        
        private Dictionary<string, string> _facets;
        
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
            return this.ToString(CultureInfo.CurrentUICulture);
        }
        
        public override string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (global::System.Environment.NewLine + @"    Description = " + (!ReferenceEquals(Description, null) ? Description.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Reassure = " + (!ReferenceEquals(Reassure, null) ? Reassure.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Background = " + (!ReferenceEquals(Background, null) ? Background.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Logs = " + (!ReferenceEquals(Logs, null) ? Logs.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoLogs = " + (!ReferenceEquals(NoLogs, null) ? NoLogs.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoExecutionInfo = " + (!ReferenceEquals(NoExecutionInfo, null) ? NoExecutionInfo.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    KeepOpen = " + (!ReferenceEquals(KeepOpen, null) ? KeepOpen.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    AlwaysClose = " + (!ReferenceEquals(AlwaysClose, null) ? AlwaysClose.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    UsePowerShellCore = " + (!ReferenceEquals(UsePowerShellCore, null) ? UsePowerShellCore.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    PowerShellExe = " + (!ReferenceEquals(PowerShellExe, null) ? PowerShellExe.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    UsePowerShellProfile = " + (!ReferenceEquals(UsePowerShellProfile, null) ? UsePowerShellProfile.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    PowerShellExecutionPolicy = " + (!ReferenceEquals(PowerShellExecutionPolicy, null) ? PowerShellExecutionPolicy.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Arguments = " + (!ReferenceEquals(Arguments, null) ? Arguments.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    WorkingDirectory = " + (!ReferenceEquals(WorkingDirectory, null) ? WorkingDirectory.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Environment = " + (!ReferenceEquals(Environment, null) ? Environment.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ExePaths = " + (!ReferenceEquals(ExePaths, null) ? ExePaths.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    UseWindowsTerminal = " + (!ReferenceEquals(UseWindowsTerminal, null) ? UseWindowsTerminal.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    WindowsTerminalArgs = " + (!ReferenceEquals(WindowsTerminalArgs, null) ? WindowsTerminalArgs.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ExitCodes = " + (!ReferenceEquals(ExitCodes, null) ? ExitCodes.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Tags = " + (!ReferenceEquals(Tags, null) ? Tags.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    BasePath = " + (!ReferenceEquals(_basePath, null) ? _basePath.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    PathPattern = " + (!ReferenceEquals(_pathPattern, null) ? _pathPattern.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Interpreter = " + (!ReferenceEquals(_interpreter, null) ? _interpreter.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Facets = " + (!ReferenceEquals(_facets, null) ? _facets.ToString() : @"null").Replace("\n", "\n    "))));
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
        
        #region Property Interpreter
        
        private string _interpreter;
        
        public virtual string Interpreter
        {
            get { return _interpreter; }
            set
            {
                if (string.Equals(value, _interpreter))
                {
                    return;
                }
                _interpreter = value;
            }
        }
        
        #endregion
        
        #region Property Facets
        
        private Dictionary<string, string> _facets;
        
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
            }
        }
        
        #endregion
    }
    
    public partial class CommandActionPattern : CommandActionBase
    {
        public CommandActionPattern()
        {
        }
        
        #region String Representation
        
        public override string ToString()
        {
            return this.ToString(CultureInfo.CurrentUICulture);
        }
        
        public override string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (global::System.Environment.NewLine + @"    Description = " + (!ReferenceEquals(Description, null) ? Description.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Reassure = " + (!ReferenceEquals(Reassure, null) ? Reassure.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Background = " + (!ReferenceEquals(Background, null) ? Background.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Logs = " + (!ReferenceEquals(Logs, null) ? Logs.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoLogs = " + (!ReferenceEquals(NoLogs, null) ? NoLogs.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoExecutionInfo = " + (!ReferenceEquals(NoExecutionInfo, null) ? NoExecutionInfo.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    KeepOpen = " + (!ReferenceEquals(KeepOpen, null) ? KeepOpen.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    AlwaysClose = " + (!ReferenceEquals(AlwaysClose, null) ? AlwaysClose.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    UsePowerShellCore = " + (!ReferenceEquals(UsePowerShellCore, null) ? UsePowerShellCore.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    PowerShellExe = " + (!ReferenceEquals(PowerShellExe, null) ? PowerShellExe.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    UsePowerShellProfile = " + (!ReferenceEquals(UsePowerShellProfile, null) ? UsePowerShellProfile.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    PowerShellExecutionPolicy = " + (!ReferenceEquals(PowerShellExecutionPolicy, null) ? PowerShellExecutionPolicy.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Arguments = " + (!ReferenceEquals(Arguments, null) ? Arguments.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    WorkingDirectory = " + (!ReferenceEquals(WorkingDirectory, null) ? WorkingDirectory.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Environment = " + (!ReferenceEquals(Environment, null) ? Environment.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ExePaths = " + (!ReferenceEquals(ExePaths, null) ? ExePaths.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    UseWindowsTerminal = " + (!ReferenceEquals(UseWindowsTerminal, null) ? UseWindowsTerminal.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    WindowsTerminalArgs = " + (!ReferenceEquals(WindowsTerminalArgs, null) ? WindowsTerminalArgs.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ExitCodes = " + (!ReferenceEquals(ExitCodes, null) ? ExitCodes.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Tags = " + (!ReferenceEquals(Tags, null) ? Tags.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Command = " + (!ReferenceEquals(_command, null) ? _command.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Facets = " + (!ReferenceEquals(_facets, null) ? _facets.ToString() : @"null").Replace("\n", "\n    "))));
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
        
        #region Property Facets
        
        private Dictionary<string, string[]> _facets;
        
        public virtual Dictionary<string, string[]> Facets
        {
            get { return _facets; }
            set
            {
                if ((value == _facets))
                {
                    return;
                }
                _facets = value;
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
            return this.ToString(CultureInfo.CurrentUICulture);
        }
        
        public virtual string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (global::System.Environment.NewLine + @"    Mode = " + _mode.ToString().Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Facet = " + (!ReferenceEquals(_facet, null) ? _facet.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Pattern = " + (!ReferenceEquals(_pattern, null) ? _pattern.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Value = " + (!ReferenceEquals(_value, null) ? _value.ToString(formatProvider) : @"null").Replace("\n", "\n    "))));
        }
        
        #endregion
        
        #region Property Mode
        
        private ActionMatchMode _mode;
        
        private const ActionMatchMode DEF_MODE = ActionMatchMode.Description;
        
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
        
        #region Property Facet
        
        private string _facet;
        
        public virtual string Facet
        {
            get { return _facet; }
            set
            {
                if (string.Equals(value, _facet))
                {
                    return;
                }
                _facet = value;
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
    
    public partial class AutoActionSettings
    {
        public AutoActionSettings()
        {
            this._include = new List<ActionMatcher>();
            this._exclude = new List<ActionMatcher>();
        }
        
        #region String Representation
        
        public override string ToString()
        {
            return this.ToString(CultureInfo.CurrentUICulture);
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
                (global::System.Environment.NewLine + @"    Include = " + (!ReferenceEquals(_include, null) ? (_include.Count.ToString() + @" items" + __collection_Include.ToString()) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Exclude = " + (!ReferenceEquals(_exclude, null) ? (_exclude.Count.ToString() + @" items" + __collection_Exclude.ToString()) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Facets = " + (!ReferenceEquals(_facets, null) ? _facets.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Tags = " + (!ReferenceEquals(_tags, null) ? _tags.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Logs = " + (!ReferenceEquals(_logs, null) ? _logs.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoLogs = " + (!ReferenceEquals(_noLogs, null) ? _noLogs.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoExecutionInfo = " + (!ReferenceEquals(_noExecutionInfo, null) ? _noExecutionInfo.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Reassure = " + (!ReferenceEquals(_reassure, null) ? _reassure.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Background = " + (!ReferenceEquals(_background, null) ? _background.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    KeepOpen = " + (!ReferenceEquals(_keepOpen, null) ? _keepOpen.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    AlwaysClose = " + (!ReferenceEquals(_alwaysClose, null) ? _alwaysClose.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Environment = " + (!ReferenceEquals(_environment, null) ? _environment.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ExePaths = " + (!ReferenceEquals(_exePaths, null) ? _exePaths.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ExitCodes = " + (!ReferenceEquals(_exitCodes, null) ? _exitCodes.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    UsePowerShellCore = " + (!ReferenceEquals(_usePowerShellCore, null) ? _usePowerShellCore.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    PowerShellExe = " + (!ReferenceEquals(_powerShellExe, null) ? _powerShellExe.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    UsePowerShellProfile = " + (!ReferenceEquals(_usePowerShellProfile, null) ? _usePowerShellProfile.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    PowerShellExecutionPolicy = " + (!ReferenceEquals(_powerShellExecutionPolicy, null) ? _powerShellExecutionPolicy.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    UseWindowsTerminal = " + (!ReferenceEquals(_useWindowsTerminal, null) ? _useWindowsTerminal.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    WindowsTerminalArgs = " + (!ReferenceEquals(_windowsTerminalArgs, null) ? _windowsTerminalArgs.ToString() : @"null").Replace("\n", "\n    "))));
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
        
        #region Property Facets
        
        private Dictionary<string, string> _facets;
        
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
        
        #region Property NoLogs
        
        private bool? _noLogs;
        
        public virtual bool? NoLogs
        {
            get { return _noLogs; }
            set
            {
                if ((value == _noLogs))
                {
                    return;
                }
                _noLogs = value;
            }
        }
        
        #endregion
        
        #region Property NoExecutionInfo
        
        private bool? _noExecutionInfo;
        
        public virtual bool? NoExecutionInfo
        {
            get { return _noExecutionInfo; }
            set
            {
                if ((value == _noExecutionInfo))
                {
                    return;
                }
                _noExecutionInfo = value;
            }
        }
        
        #endregion
        
        #region Property Reassure
        
        private bool? _reassure;
        
        public virtual bool? Reassure
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
        
        #region Property Background
        
        private bool? _background;
        
        public virtual bool? Background
        {
            get { return _background; }
            set
            {
                if ((value == _background))
                {
                    return;
                }
                _background = value;
            }
        }
        
        #endregion
        
        #region Property KeepOpen
        
        private bool? _keepOpen;
        
        public virtual bool? KeepOpen
        {
            get { return _keepOpen; }
            set
            {
                if ((value == _keepOpen))
                {
                    return;
                }
                _keepOpen = value;
            }
        }
        
        #endregion
        
        #region Property AlwaysClose
        
        private bool? _alwaysClose;
        
        public virtual bool? AlwaysClose
        {
            get { return _alwaysClose; }
            set
            {
                if ((value == _alwaysClose))
                {
                    return;
                }
                _alwaysClose = value;
            }
        }
        
        #endregion
        
        #region Property Environment
        
        private Dictionary<string, string> _environment;
        
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
            }
        }
        
        #endregion
        
        #region Property ExePaths
        
        private string[] _exePaths;
        
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
            }
        }
        
        #endregion
        
        #region Property ExitCodes
        
        private int[] _exitCodes;
        
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
            }
        }
        
        #endregion
        
        #region Property UsePowerShellCore
        
        private bool? _usePowerShellCore;
        
        public virtual bool? UsePowerShellCore
        {
            get { return _usePowerShellCore; }
            set
            {
                if ((value == _usePowerShellCore))
                {
                    return;
                }
                _usePowerShellCore = value;
            }
        }
        
        #endregion
        
        #region Property PowerShellExe
        
        private string _powerShellExe;
        
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
            }
        }
        
        #endregion
        
        #region Property UsePowerShellProfile
        
        private bool? _usePowerShellProfile;
        
        public virtual bool? UsePowerShellProfile
        {
            get { return _usePowerShellProfile; }
            set
            {
                if ((value == _usePowerShellProfile))
                {
                    return;
                }
                _usePowerShellProfile = value;
            }
        }
        
        #endregion
        
        #region Property PowerShellExecutionPolicy
        
        private string _powerShellExecutionPolicy;
        
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
            }
        }
        
        #endregion
        
        #region Property UseWindowsTerminal
        
        private bool? _useWindowsTerminal;
        
        public virtual bool? UseWindowsTerminal
        {
            get { return _useWindowsTerminal; }
            set
            {
                if ((value == _useWindowsTerminal))
                {
                    return;
                }
                _useWindowsTerminal = value;
            }
        }
        
        #endregion
        
        #region Property WindowsTerminalArgs
        
        private string[] _windowsTerminalArgs;
        
        public virtual string[] WindowsTerminalArgs
        {
            get { return _windowsTerminalArgs; }
            set
            {
                if ((value == _windowsTerminalArgs))
                {
                    return;
                }
                _windowsTerminalArgs = value;
            }
        }
        
        #endregion
    }
    
    public partial class AutoSettings
    {
        public AutoSettings()
        {
            this._forActions = new List<AutoActionSettings>();
        }
        
        #region String Representation
        
        public override string ToString()
        {
            return this.ToString(CultureInfo.CurrentUICulture);
        }
        
        public virtual string ToString(IFormatProvider formatProvider)
        {
            global::System.Text.StringBuilder __collection_ForActions = new global::System.Text.StringBuilder();
            if ((!ReferenceEquals(_forActions, null) && !(_forActions.Count == 0)))
            {
                for (int __index = 0; __index < Math.Min(_forActions.Count, 10); __index++)
                {
                    AutoActionSettings __item = _forActions[__index];
                    __collection_ForActions.AppendLine();
                    __collection_ForActions.Append((@"- " + __index.ToString() + @": " + (!ReferenceEquals(__item, null) ? __item.ToString() : @"null")));
                }
            }
            return (this.GetType().FullName + @": " + 
                (global::System.Environment.NewLine + @"    ForActions = " + (!ReferenceEquals(_forActions, null) ? (_forActions.Count.ToString() + @" items" + __collection_ForActions.ToString()) : @"null").Replace("\n", "\n    ")));
        }
        
        #endregion
        
        #region Property ForActions
        
        private List<AutoActionSettings> _forActions;
        
        public virtual List<AutoActionSettings> ForActions
        {
            get { return _forActions; }
            set
            {
                if ((value == _forActions))
                {
                    return;
                }
                _forActions = value;
            }
        }
        
        #endregion
    }
    
    public partial class MonitorBase
    {
        public MonitorBase()
        {
            this._requiredPatterns = new string[0];
            this._forbiddenPatterns = new string[0];
        }
        
        #region String Representation
        
        public override string ToString()
        {
            return this.ToString(CultureInfo.CurrentUICulture);
        }
        
        public virtual string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (global::System.Environment.NewLine + @"    Title = " + (!ReferenceEquals(_title, null) ? _title.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Deactivated = " + (!ReferenceEquals(_deactivated, null) ? _deactivated.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Interval = " + (!ReferenceEquals(_interval, null) ? _interval.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Logs = " + (!ReferenceEquals(_logs, null) ? _logs.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoLogs = " + (!ReferenceEquals(_noLogs, null) ? _noLogs.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoExecutionInfo = " + (!ReferenceEquals(_noExecutionInfo, null) ? _noExecutionInfo.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    RequiredPatterns = " + (!ReferenceEquals(_requiredPatterns, null) ? _requiredPatterns.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ForbiddenPatterns = " + (!ReferenceEquals(_forbiddenPatterns, null) ? _forbiddenPatterns.ToString() : @"null").Replace("\n", "\n    "))));
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
        
        #region Property Deactivated
        
        private bool? _deactivated;
        
        public virtual bool? Deactivated
        {
            get { return _deactivated; }
            set
            {
                if ((value == _deactivated))
                {
                    return;
                }
                _deactivated = value;
            }
        }
        
        #endregion
        
        #region Property Interval
        
        private int? _interval;
        
        public virtual int? Interval
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
        
        #region Property NoLogs
        
        private bool? _noLogs;
        
        public virtual bool? NoLogs
        {
            get { return _noLogs; }
            set
            {
                if ((value == _noLogs))
                {
                    return;
                }
                _noLogs = value;
            }
        }
        
        #endregion
        
        #region Property NoExecutionInfo
        
        private bool? _noExecutionInfo;
        
        public virtual bool? NoExecutionInfo
        {
            get { return _noExecutionInfo; }
            set
            {
                if ((value == _noExecutionInfo))
                {
                    return;
                }
                _noExecutionInfo = value;
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
        }
        
        #region String Representation
        
        public override string ToString()
        {
            return this.ToString(CultureInfo.CurrentUICulture);
        }
        
        public override string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (global::System.Environment.NewLine + @"    Title = " + (!ReferenceEquals(Title, null) ? Title.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Deactivated = " + (!ReferenceEquals(Deactivated, null) ? Deactivated.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Interval = " + (!ReferenceEquals(Interval, null) ? Interval.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Logs = " + (!ReferenceEquals(Logs, null) ? Logs.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoLogs = " + (!ReferenceEquals(NoLogs, null) ? NoLogs.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoExecutionInfo = " + (!ReferenceEquals(NoExecutionInfo, null) ? NoExecutionInfo.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    RequiredPatterns = " + (!ReferenceEquals(RequiredPatterns, null) ? RequiredPatterns.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ForbiddenPatterns = " + (!ReferenceEquals(ForbiddenPatterns, null) ? ForbiddenPatterns.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    UsePowerShellCore = " + (!ReferenceEquals(_usePowerShellCore, null) ? _usePowerShellCore.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    PowerShellExe = " + (!ReferenceEquals(_powerShellExe, null) ? _powerShellExe.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    UsePowerShellProfile = " + (!ReferenceEquals(_usePowerShellProfile, null) ? _usePowerShellProfile.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    PowerShellExecutionPolicy = " + (!ReferenceEquals(_powerShellExecutionPolicy, null) ? _powerShellExecutionPolicy.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Arguments = " + (!ReferenceEquals(_arguments, null) ? _arguments.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    WorkingDirectory = " + (!ReferenceEquals(_workingDirectory, null) ? _workingDirectory.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Environment = " + (!ReferenceEquals(_environment, null) ? _environment.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ExePaths = " + (!ReferenceEquals(_exePaths, null) ? _exePaths.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ExitCodes = " + (!ReferenceEquals(_exitCodes, null) ? _exitCodes.ToString() : @"null").Replace("\n", "\n    "))));
        }
        
        #endregion
        
        #region Property UsePowerShellCore
        
        private bool? _usePowerShellCore;
        
        public virtual bool? UsePowerShellCore
        {
            get { return _usePowerShellCore; }
            set
            {
                if ((value == _usePowerShellCore))
                {
                    return;
                }
                _usePowerShellCore = value;
            }
        }
        
        #endregion
        
        #region Property PowerShellExe
        
        private string _powerShellExe;
        
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
            }
        }
        
        #endregion
        
        #region Property UsePowerShellProfile
        
        private bool? _usePowerShellProfile;
        
        public virtual bool? UsePowerShellProfile
        {
            get { return _usePowerShellProfile; }
            set
            {
                if ((value == _usePowerShellProfile))
                {
                    return;
                }
                _usePowerShellProfile = value;
            }
        }
        
        #endregion
        
        #region Property PowerShellExecutionPolicy
        
        private string _powerShellExecutionPolicy;
        
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
        
        #region Property Environment
        
        private Dictionary<string, string> _environment;
        
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
            }
        }
        
        #endregion
        
        #region Property ExePaths
        
        private string[] _exePaths;
        
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
            }
        }
        
        #endregion
        
        #region Property ExitCodes
        
        private int[] _exitCodes;
        
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
            return this.ToString(CultureInfo.CurrentUICulture);
        }
        
        public override string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (global::System.Environment.NewLine + @"    Title = " + (!ReferenceEquals(Title, null) ? Title.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Deactivated = " + (!ReferenceEquals(Deactivated, null) ? Deactivated.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Interval = " + (!ReferenceEquals(Interval, null) ? Interval.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Logs = " + (!ReferenceEquals(Logs, null) ? Logs.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoLogs = " + (!ReferenceEquals(NoLogs, null) ? NoLogs.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoExecutionInfo = " + (!ReferenceEquals(NoExecutionInfo, null) ? NoExecutionInfo.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    RequiredPatterns = " + (!ReferenceEquals(RequiredPatterns, null) ? RequiredPatterns.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ForbiddenPatterns = " + (!ReferenceEquals(ForbiddenPatterns, null) ? ForbiddenPatterns.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    UsePowerShellCore = " + (!ReferenceEquals(UsePowerShellCore, null) ? UsePowerShellCore.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    PowerShellExe = " + (!ReferenceEquals(PowerShellExe, null) ? PowerShellExe.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    UsePowerShellProfile = " + (!ReferenceEquals(UsePowerShellProfile, null) ? UsePowerShellProfile.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    PowerShellExecutionPolicy = " + (!ReferenceEquals(PowerShellExecutionPolicy, null) ? PowerShellExecutionPolicy.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Arguments = " + (!ReferenceEquals(Arguments, null) ? Arguments.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    WorkingDirectory = " + (!ReferenceEquals(WorkingDirectory, null) ? WorkingDirectory.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Environment = " + (!ReferenceEquals(Environment, null) ? Environment.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ExePaths = " + (!ReferenceEquals(ExePaths, null) ? ExePaths.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ExitCodes = " + (!ReferenceEquals(ExitCodes, null) ? ExitCodes.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Command = " + (!ReferenceEquals(_command, null) ? _command.ToString(formatProvider) : @"null").Replace("\n", "\n    "))));
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
            return this.ToString(CultureInfo.CurrentUICulture);
        }
        
        public override string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (global::System.Environment.NewLine + @"    Title = " + (!ReferenceEquals(Title, null) ? Title.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Deactivated = " + (!ReferenceEquals(Deactivated, null) ? Deactivated.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Interval = " + (!ReferenceEquals(Interval, null) ? Interval.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Logs = " + (!ReferenceEquals(Logs, null) ? Logs.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoLogs = " + (!ReferenceEquals(NoLogs, null) ? NoLogs.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoExecutionInfo = " + (!ReferenceEquals(NoExecutionInfo, null) ? NoExecutionInfo.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    RequiredPatterns = " + (!ReferenceEquals(RequiredPatterns, null) ? RequiredPatterns.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ForbiddenPatterns = " + (!ReferenceEquals(ForbiddenPatterns, null) ? ForbiddenPatterns.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    UsePowerShellCore = " + (!ReferenceEquals(UsePowerShellCore, null) ? UsePowerShellCore.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    PowerShellExe = " + (!ReferenceEquals(PowerShellExe, null) ? PowerShellExe.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    UsePowerShellProfile = " + (!ReferenceEquals(UsePowerShellProfile, null) ? UsePowerShellProfile.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    PowerShellExecutionPolicy = " + (!ReferenceEquals(PowerShellExecutionPolicy, null) ? PowerShellExecutionPolicy.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Arguments = " + (!ReferenceEquals(Arguments, null) ? Arguments.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    WorkingDirectory = " + (!ReferenceEquals(WorkingDirectory, null) ? WorkingDirectory.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Environment = " + (!ReferenceEquals(Environment, null) ? Environment.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ExePaths = " + (!ReferenceEquals(ExePaths, null) ? ExePaths.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ExitCodes = " + (!ReferenceEquals(ExitCodes, null) ? ExitCodes.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    BasePath = " + (!ReferenceEquals(_basePath, null) ? _basePath.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    PathPattern = " + (!ReferenceEquals(_pathPattern, null) ? _pathPattern.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Interpreter = " + (!ReferenceEquals(_interpreter, null) ? _interpreter.ToString(formatProvider) : @"null").Replace("\n", "\n    "))));
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
        
        #region Property Interpreter
        
        private string _interpreter;
        
        public virtual string Interpreter
        {
            get { return _interpreter; }
            set
            {
                if (string.Equals(value, _interpreter))
                {
                    return;
                }
                _interpreter = value;
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
            return this.ToString(CultureInfo.CurrentUICulture);
        }
        
        public override string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (global::System.Environment.NewLine + @"    Title = " + (!ReferenceEquals(Title, null) ? Title.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Deactivated = " + (!ReferenceEquals(Deactivated, null) ? Deactivated.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Interval = " + (!ReferenceEquals(Interval, null) ? Interval.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Logs = " + (!ReferenceEquals(Logs, null) ? Logs.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoLogs = " + (!ReferenceEquals(NoLogs, null) ? NoLogs.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoExecutionInfo = " + (!ReferenceEquals(NoExecutionInfo, null) ? NoExecutionInfo.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    RequiredPatterns = " + (!ReferenceEquals(RequiredPatterns, null) ? RequiredPatterns.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ForbiddenPatterns = " + (!ReferenceEquals(ForbiddenPatterns, null) ? ForbiddenPatterns.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    UsePowerShellCore = " + (!ReferenceEquals(UsePowerShellCore, null) ? UsePowerShellCore.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    PowerShellExe = " + (!ReferenceEquals(PowerShellExe, null) ? PowerShellExe.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    UsePowerShellProfile = " + (!ReferenceEquals(UsePowerShellProfile, null) ? UsePowerShellProfile.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    PowerShellExecutionPolicy = " + (!ReferenceEquals(PowerShellExecutionPolicy, null) ? PowerShellExecutionPolicy.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Arguments = " + (!ReferenceEquals(Arguments, null) ? Arguments.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    WorkingDirectory = " + (!ReferenceEquals(WorkingDirectory, null) ? WorkingDirectory.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Environment = " + (!ReferenceEquals(Environment, null) ? Environment.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ExePaths = " + (!ReferenceEquals(ExePaths, null) ? ExePaths.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ExitCodes = " + (!ReferenceEquals(ExitCodes, null) ? ExitCodes.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Command = " + (!ReferenceEquals(Command, null) ? Command.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Variables = " + (!ReferenceEquals(_variables, null) ? _variables.ToString() : @"null").Replace("\n", "\n    "))));
        }
        
        #endregion
        
        #region Property Variables
        
        private Dictionary<string, string[]> _variables;
        
        public virtual Dictionary<string, string[]> Variables
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
        }
        
        #region String Representation
        
        public override string ToString()
        {
            return this.ToString(CultureInfo.CurrentUICulture);
        }
        
        public override string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (global::System.Environment.NewLine + @"    Title = " + (!ReferenceEquals(Title, null) ? Title.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Deactivated = " + (!ReferenceEquals(Deactivated, null) ? Deactivated.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Interval = " + (!ReferenceEquals(Interval, null) ? Interval.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Logs = " + (!ReferenceEquals(Logs, null) ? Logs.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoLogs = " + (!ReferenceEquals(NoLogs, null) ? NoLogs.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoExecutionInfo = " + (!ReferenceEquals(NoExecutionInfo, null) ? NoExecutionInfo.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    RequiredPatterns = " + (!ReferenceEquals(RequiredPatterns, null) ? RequiredPatterns.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ForbiddenPatterns = " + (!ReferenceEquals(ForbiddenPatterns, null) ? ForbiddenPatterns.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Url = " + (!ReferenceEquals(_url, null) ? _url.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Headers = " + (!ReferenceEquals(_headers, null) ? _headers.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    HttpTimeout = " + (!ReferenceEquals(_httpTimeout, null) ? _httpTimeout.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ServerCertificateHash = " + (!ReferenceEquals(_serverCertificateHash, null) ? _serverCertificateHash.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoTlsVerify = " + (!ReferenceEquals(_noTlsVerify, null) ? _noTlsVerify.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    StatusCodes = " + (!ReferenceEquals(_statusCodes, null) ? _statusCodes.ToString() : @"null").Replace("\n", "\n    "))));
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
        
        #region Property HttpTimeout
        
        private int? _httpTimeout;
        
        public virtual int? HttpTimeout
        {
            get { return _httpTimeout; }
            set
            {
                if ((value == _httpTimeout))
                {
                    return;
                }
                _httpTimeout = value;
            }
        }
        
        #endregion
        
        #region Property ServerCertificateHash
        
        private string _serverCertificateHash;
        
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
            }
        }
        
        #endregion
        
        #region Property NoTlsVerify
        
        private bool? _noTlsVerify;
        
        public virtual bool? NoTlsVerify
        {
            get { return _noTlsVerify; }
            set
            {
                if ((value == _noTlsVerify))
                {
                    return;
                }
                _noTlsVerify = value;
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
            return this.ToString(CultureInfo.CurrentUICulture);
        }
        
        public override string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (global::System.Environment.NewLine + @"    Title = " + (!ReferenceEquals(Title, null) ? Title.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Deactivated = " + (!ReferenceEquals(Deactivated, null) ? Deactivated.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Interval = " + (!ReferenceEquals(Interval, null) ? Interval.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Logs = " + (!ReferenceEquals(Logs, null) ? Logs.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoLogs = " + (!ReferenceEquals(NoLogs, null) ? NoLogs.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoExecutionInfo = " + (!ReferenceEquals(NoExecutionInfo, null) ? NoExecutionInfo.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    RequiredPatterns = " + (!ReferenceEquals(RequiredPatterns, null) ? RequiredPatterns.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ForbiddenPatterns = " + (!ReferenceEquals(ForbiddenPatterns, null) ? ForbiddenPatterns.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Url = " + (!ReferenceEquals(Url, null) ? Url.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Headers = " + (!ReferenceEquals(Headers, null) ? Headers.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    HttpTimeout = " + (!ReferenceEquals(HttpTimeout, null) ? HttpTimeout.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ServerCertificateHash = " + (!ReferenceEquals(ServerCertificateHash, null) ? ServerCertificateHash.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoTlsVerify = " + (!ReferenceEquals(NoTlsVerify, null) ? NoTlsVerify.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    StatusCodes = " + (!ReferenceEquals(StatusCodes, null) ? StatusCodes.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Variables = " + (!ReferenceEquals(_variables, null) ? _variables.ToString() : @"null").Replace("\n", "\n    "))));
        }
        
        #endregion
        
        #region Property Variables
        
        private Dictionary<string, string[]> _variables;
        
        public virtual Dictionary<string, string[]> Variables
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
    
    public partial class FacetPerspective
    {
        public FacetPerspective()
        {
        }
        
        #region String Representation
        
        public override string ToString()
        {
            return this.ToString(CultureInfo.CurrentUICulture);
        }
        
        public virtual string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (global::System.Environment.NewLine + @"    Caption = " + (!ReferenceEquals(_caption, null) ? _caption.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Facet = " + (!ReferenceEquals(_facet, null) ? _facet.ToString(formatProvider) : @"null").Replace("\n", "\n    "))));
        }
        
        #endregion
        
        #region Property Caption
        
        private string _caption;
        
        public virtual string Caption
        {
            get { return _caption; }
            set
            {
                if (string.Equals(value, _caption))
                {
                    return;
                }
                _caption = value;
            }
        }
        
        #endregion
        
        #region Property Facet
        
        private string _facet;
        
        public virtual string Facet
        {
            get { return _facet; }
            set
            {
                if (string.Equals(value, _facet))
                {
                    return;
                }
                _facet = value;
            }
        }
        
        #endregion
    }
    
    public partial class DefaultActionSettings
    {
        public DefaultActionSettings()
        {
            this._exePaths = new string[0];
            this._powerShellExecutionPolicy = DEF_POWERSHELLEXECUTIONPOLICY;
            this._windowsTerminalArgs = new string[0];
        }
        
        #region String Representation
        
        public override string ToString()
        {
            return this.ToString(CultureInfo.CurrentUICulture);
        }
        
        public virtual string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (global::System.Environment.NewLine + @"    Reassure = " + _reassure.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Background = " + _background.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    KeepOpen = " + _keepOpen.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    AlwaysClose = " + _alwaysClose.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Logs = " + (!ReferenceEquals(_logs, null) ? _logs.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoLogs = " + _noLogs.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoExecutionInfo = " + _noExecutionInfo.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    WorkingDirectory = " + (!ReferenceEquals(_workingDirectory, null) ? _workingDirectory.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Environment = " + (!ReferenceEquals(_environment, null) ? _environment.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ExePaths = " + (!ReferenceEquals(_exePaths, null) ? _exePaths.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ExitCodes = " + (!ReferenceEquals(_exitCodes, null) ? _exitCodes.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    UsePowerShellCore = " + _usePowerShellCore.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    PowerShellExe = " + (!ReferenceEquals(_powerShellExe, null) ? _powerShellExe.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    UsePowerShellProfile = " + _usePowerShellProfile.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    PowerShellExecutionPolicy = " + (!ReferenceEquals(_powerShellExecutionPolicy, null) ? _powerShellExecutionPolicy.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    UseWindowsTerminal = " + _useWindowsTerminal.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    WindowsTerminalArgs = " + (!ReferenceEquals(_windowsTerminalArgs, null) ? _windowsTerminalArgs.ToString() : @"null").Replace("\n", "\n    "))));
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
        
        #region Property Background
        
        private bool _background;
        
        public virtual bool Background
        {
            get { return _background; }
            set
            {
                if ((value == _background))
                {
                    return;
                }
                _background = value;
            }
        }
        
        #endregion
        
        #region Property KeepOpen
        
        private bool _keepOpen;
        
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
            }
        }
        
        #endregion
        
        #region Property AlwaysClose
        
        private bool _alwaysClose;
        
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
        
        #region Property NoLogs
        
        private bool _noLogs;
        
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
            }
        }
        
        #endregion
        
        #region Property NoExecutionInfo
        
        private bool _noExecutionInfo;
        
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
        
        #region Property Environment
        
        private Dictionary<string, string> _environment;
        
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
            }
        }
        
        #endregion
        
        #region Property ExePaths
        
        private string[] _exePaths;
        
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
            }
        }
        
        #endregion
        
        #region Property ExitCodes
        
        private int[] _exitCodes;
        
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
            }
        }
        
        #endregion
        
        #region Property UsePowerShellCore
        
        private bool _usePowerShellCore;
        
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
            }
        }
        
        #endregion
        
        #region Property PowerShellExe
        
        private string _powerShellExe;
        
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
            }
        }
        
        #endregion
        
        #region Property UsePowerShellProfile
        
        private bool _usePowerShellProfile;
        
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
            }
        }
        
        #endregion
        
        #region Property PowerShellExecutionPolicy
        
        private string _powerShellExecutionPolicy;
        
        private const string DEF_POWERSHELLEXECUTIONPOLICY = @"RemoteSigned";
        
        [DefaultValue(DEF_POWERSHELLEXECUTIONPOLICY)]
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
            }
        }
        
        #endregion
        
        #region Property UseWindowsTerminal
        
        private bool _useWindowsTerminal;
        
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
            }
        }
        
        #endregion
        
        #region Property WindowsTerminalArgs
        
        private string[] _windowsTerminalArgs;
        
        public virtual string[] WindowsTerminalArgs
        {
            get { return _windowsTerminalArgs; }
            set
            {
                if ((value == _windowsTerminalArgs))
                {
                    return;
                }
                _windowsTerminalArgs = value;
            }
        }
        
        #endregion
    }
    
    public partial class DefaultMonitorSettings
    {
        public DefaultMonitorSettings()
        {
            this._interval = DEF_INTERVAL;
            this._httpTimeout = DEF_HTTPTIMEOUT;
            this._requiredPatterns = new string[0];
            this._forbiddenPatterns = new string[0];
            this._exePaths = new string[0];
            this._powerShellExecutionPolicy = DEF_POWERSHELLEXECUTIONPOLICY;
        }
        
        #region String Representation
        
        public override string ToString()
        {
            return this.ToString(CultureInfo.CurrentUICulture);
        }
        
        public virtual string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (global::System.Environment.NewLine + @"    Deactivated = " + _deactivated.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Interval = " + _interval.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    HttpTimeout = " + _httpTimeout.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoTlsVerify = " + _noTlsVerify.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    StatusCodes = " + (!ReferenceEquals(_statusCodes, null) ? _statusCodes.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    RequiredPatterns = " + (!ReferenceEquals(_requiredPatterns, null) ? _requiredPatterns.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ForbiddenPatterns = " + (!ReferenceEquals(_forbiddenPatterns, null) ? _forbiddenPatterns.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Logs = " + (!ReferenceEquals(_logs, null) ? _logs.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoLogs = " + _noLogs.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    NoExecutionInfo = " + _noExecutionInfo.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    WorkingDirectory = " + (!ReferenceEquals(_workingDirectory, null) ? _workingDirectory.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Environment = " + (!ReferenceEquals(_environment, null) ? _environment.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ExePaths = " + (!ReferenceEquals(_exePaths, null) ? _exePaths.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ExitCodes = " + (!ReferenceEquals(_exitCodes, null) ? _exitCodes.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    UsePowerShellCore = " + _usePowerShellCore.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    PowerShellExe = " + (!ReferenceEquals(_powerShellExe, null) ? _powerShellExe.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    UsePowerShellProfile = " + _usePowerShellProfile.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    PowerShellExecutionPolicy = " + (!ReferenceEquals(_powerShellExecutionPolicy, null) ? _powerShellExecutionPolicy.ToString(formatProvider) : @"null").Replace("\n", "\n    "))));
        }
        
        #endregion
        
        #region Property Deactivated
        
        private bool _deactivated;
        
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
            }
        }
        
        #endregion
        
        #region Property Interval
        
        private int _interval;
        
        private const int DEF_INTERVAL = 60;
        
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
        
        #region Property HttpTimeout
        
        private int _httpTimeout;
        
        private const int DEF_HTTPTIMEOUT = 20;
        
        [DefaultValue(DEF_HTTPTIMEOUT)]
        public virtual int HttpTimeout
        {
            get { return _httpTimeout; }
            set
            {
                if ((value == _httpTimeout))
                {
                    return;
                }
                _httpTimeout = value;
            }
        }
        
        #endregion
        
        #region Property NoTlsVerify
        
        private bool _noTlsVerify;
        
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
        
        #region Property NoLogs
        
        private bool _noLogs;
        
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
            }
        }
        
        #endregion
        
        #region Property NoExecutionInfo
        
        private bool _noExecutionInfo;
        
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
        
        #region Property Environment
        
        private Dictionary<string, string> _environment;
        
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
            }
        }
        
        #endregion
        
        #region Property ExePaths
        
        private string[] _exePaths;
        
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
            }
        }
        
        #endregion
        
        #region Property ExitCodes
        
        private int[] _exitCodes;
        
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
            }
        }
        
        #endregion
        
        #region Property UsePowerShellCore
        
        private bool _usePowerShellCore;
        
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
            }
        }
        
        #endregion
        
        #region Property PowerShellExe
        
        private string _powerShellExe;
        
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
            }
        }
        
        #endregion
        
        #region Property UsePowerShellProfile
        
        private bool _usePowerShellProfile;
        
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
            }
        }
        
        #endregion
        
        #region Property PowerShellExecutionPolicy
        
        private string _powerShellExecutionPolicy;
        
        private const string DEF_POWERSHELLEXECUTIONPOLICY = @"RemoteSigned";
        
        [DefaultValue(DEF_POWERSHELLEXECUTIONPOLICY)]
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
            }
        }
        
        #endregion
    }
    
    public partial class DefaultSettings
    {
        public DefaultSettings()
        {
        }
        
        #region String Representation
        
        public override string ToString()
        {
            return this.ToString(CultureInfo.CurrentUICulture);
        }
        
        public virtual string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (global::System.Environment.NewLine + @"    ForActions = " + (!ReferenceEquals(_forActions, null) ? _forActions.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ForMonitors = " + (!ReferenceEquals(_forMonitors, null) ? _forMonitors.ToString() : @"null").Replace("\n", "\n    "))));
        }
        
        #endregion
        
        #region Property ForActions
        
        private DefaultActionSettings _forActions;
        
        public virtual DefaultActionSettings ForActions
        {
            get { return _forActions; }
            set
            {
                if ((value == _forActions))
                {
                    return;
                }
                _forActions = value;
            }
        }
        
        #endregion
        
        #region Property ForMonitors
        
        private DefaultMonitorSettings _forMonitors;
        
        public virtual DefaultMonitorSettings ForMonitors
        {
            get { return _forMonitors; }
            set
            {
                if ((value == _forMonitors))
                {
                    return;
                }
                _forMonitors = value;
            }
        }
        
        #endregion
    }
    
    public partial class Project
    {
        public Project()
        {
            this._perspectives = new List<FacetPerspective>();
            this._actions = new List<CommandAction>();
            this._actionDiscovery = new List<CommandActionDiscovery>();
            this._actionPatterns = new List<CommandActionPattern>();
            this._monitors = new List<CommandMonitor>();
            this._monitorDiscovery = new List<CommandMonitorDiscovery>();
            this._monitorPatterns = new List<CommandMonitorPattern>();
            this._webMonitors = new List<WebMonitor>();
            this._webMonitorPatterns = new List<WebMonitorPattern>();
            this.Initialize();
        }
        
        #region String Representation
        
        public override string ToString()
        {
            return this.ToString(CultureInfo.CurrentUICulture);
        }
        
        public virtual string ToString(IFormatProvider formatProvider)
        {
            global::System.Text.StringBuilder __collection_Perspectives = new global::System.Text.StringBuilder();
            if ((!ReferenceEquals(_perspectives, null) && !(_perspectives.Count == 0)))
            {
                for (int __index = 0; __index < Math.Min(_perspectives.Count, 10); __index++)
                {
                    FacetPerspective __item = _perspectives[__index];
                    __collection_Perspectives.AppendLine();
                    __collection_Perspectives.Append((@"- " + __index.ToString() + @": " + (!ReferenceEquals(__item, null) ? __item.ToString() : @"null")));
                }
            }
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
                (global::System.Environment.NewLine + @"    Version = " + (!ReferenceEquals(_version, null) ? _version.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Title = " + (!ReferenceEquals(_title, null) ? _title.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Perspectives = " + (!ReferenceEquals(_perspectives, null) ? (_perspectives.Count.ToString() + @" items" + __collection_Perspectives.ToString()) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    StartupPerspective = " + (!ReferenceEquals(_startupPerspective, null) ? _startupPerspective.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    StartupSelection = " + (!ReferenceEquals(_startupSelection, null) ? _startupSelection.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    PauseMonitoring = " + _pauseMonitoring.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Defaults = " + (!ReferenceEquals(_defaults, null) ? _defaults.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    AutoSettings = " + (!ReferenceEquals(_autoSettings, null) ? _autoSettings.ToString() : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Actions = " + (!ReferenceEquals(_actions, null) ? (_actions.Count.ToString() + @" items" + __collection_Actions.ToString()) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ActionDiscovery = " + (!ReferenceEquals(_actionDiscovery, null) ? (_actionDiscovery.Count.ToString() + @" items" + __collection_ActionDiscovery.ToString()) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    ActionPatterns = " + (!ReferenceEquals(_actionPatterns, null) ? (_actionPatterns.Count.ToString() + @" items" + __collection_ActionPatterns.ToString()) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    Monitors = " + (!ReferenceEquals(_monitors, null) ? (_monitors.Count.ToString() + @" items" + __collection_Monitors.ToString()) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    MonitorDiscovery = " + (!ReferenceEquals(_monitorDiscovery, null) ? (_monitorDiscovery.Count.ToString() + @" items" + __collection_MonitorDiscovery.ToString()) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    MonitorPatterns = " + (!ReferenceEquals(_monitorPatterns, null) ? (_monitorPatterns.Count.ToString() + @" items" + __collection_MonitorPatterns.ToString()) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    WebMonitors = " + (!ReferenceEquals(_webMonitors, null) ? (_webMonitors.Count.ToString() + @" items" + __collection_WebMonitors.ToString()) : @"null").Replace("\n", "\n    ")) + 
                (global::System.Environment.NewLine + @"    WebMonitorPatterns = " + (!ReferenceEquals(_webMonitorPatterns, null) ? (_webMonitorPatterns.Count.ToString() + @" items" + __collection_WebMonitorPatterns.ToString()) : @"null").Replace("\n", "\n    "))));
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
        
        #region Property Perspectives
        
        private List<FacetPerspective> _perspectives;
        
        public virtual List<FacetPerspective> Perspectives
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
        
        #region Property StartupPerspective
        
        private string _startupPerspective;
        
        public virtual string StartupPerspective
        {
            get { return _startupPerspective; }
            set
            {
                if (string.Equals(value, _startupPerspective))
                {
                    return;
                }
                _startupPerspective = value;
            }
        }
        
        #endregion
        
        #region Property StartupSelection
        
        private string _startupSelection;
        
        public virtual string StartupSelection
        {
            get { return _startupSelection; }
            set
            {
                if (string.Equals(value, _startupSelection))
                {
                    return;
                }
                _startupSelection = value;
            }
        }
        
        #endregion
        
        #region Property PauseMonitoring
        
        private bool _pauseMonitoring;
        
        public virtual bool PauseMonitoring
        {
            get { return _pauseMonitoring; }
            set
            {
                if ((value == _pauseMonitoring))
                {
                    return;
                }
                _pauseMonitoring = value;
            }
        }
        
        #endregion
        
        #region Property Defaults
        
        private DefaultSettings _defaults;
        
        public virtual DefaultSettings Defaults
        {
            get { return _defaults; }
            set
            {
                if ((value == _defaults))
                {
                    return;
                }
                _defaults = value;
            }
        }
        
        #endregion
        
        #region Property AutoSettings
        
        private AutoSettings _autoSettings;
        
        public virtual AutoSettings AutoSettings
        {
            get { return _autoSettings; }
            set
            {
                if ((value == _autoSettings))
                {
                    return;
                }
                _autoSettings = value;
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
    }
    
    #endregion
}