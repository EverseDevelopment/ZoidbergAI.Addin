using System;
using System.ComponentModel;
using System.Runtime.Remoting.Channels;

namespace Common.UI.Base
{
    /// <summary>
    /// NotifiyBase for Mvvm ViewModels
    /// </summary>
    public class NotifyPropertyBase : INotifyPropertyChanged
    {
        public event EventHandler BringWindowToFront;

        public NotifyPropertyBase()
        {
        }

        /// <summary>
        /// Event that gets fired when any property changes on child classes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Refresh a single property to UI
        /// </summary>
        public void RefreshProperty(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Refresh all properties to UI
        /// </summary>
        public void RefreshAllProperties()
        {
            System.Reflection.PropertyInfo[] properties = this.GetType().GetProperties();
            foreach (var property in properties)
            {
                RefreshProperty(property.Name);
            }
        }

        protected void OnBringWindowToFront()
        {
            BringWindowToFront?.Invoke(this, EventArgs.Empty);
        }
    }
}