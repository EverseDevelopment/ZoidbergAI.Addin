using Common.Application;
using System.Windows;
using System.Windows.Input;

namespace Common.UI.Base
{
    public abstract class WindowBase : Window
    {
        public WindowBase()
        {
            OnInitialized();
        }

        protected virtual void OnInitialized()
        {
            this.Title = ApplicationContext.Instance.AppFormsTitle;
        }
    }
}
