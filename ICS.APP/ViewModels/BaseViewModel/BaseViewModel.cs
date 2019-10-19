using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TeamsManager.APP.Annotations;

namespace TeamsManager.APP.ViewModels.BaseViewModel
{
    public abstract class BaseViewModel : INotifyPropertyChanged, IViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Load()
        {
        }
    }
}
