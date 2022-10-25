using PropertyChanged;
using System.ComponentModel;

namespace GenAssess
{
    /// <summary>
    ///  BaseViewModel: Is the class that will be used as the base view model that fires Property Changed events.
    /// </summary>

    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        ///  public event PropertyChangedEventHandler PropertyChanged: Event that is fired when any child property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// public void OnPropertyChanged: Fire a <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The changed string property name.</param>
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
