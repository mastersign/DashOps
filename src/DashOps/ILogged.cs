using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace Mastersign.DashOps
{
    public interface ILogged : INotifyPropertyChanged
    {
        string Logs { get; }
        string CommandId { get; }
    }
}