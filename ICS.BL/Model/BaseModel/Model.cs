using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TeamsManager.BL.Model.BaseModel
{
    public abstract class Model : IModel, INotifyPropertyChanged
    {
        public int? Id { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
