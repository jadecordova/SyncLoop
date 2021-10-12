using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Notifies property changed.
    /// </summary>
    public class Notifier : INotifyPropertyChanged
    {
        /// <summary>
        /// Property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Property changed notify method.
        /// </summary>
        /// <param name="propertyName">Name of property changed.</param>
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
