using System;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using SharkSpirit.Editor.Core.Services;

namespace SharkSpirit.Editor.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Frame _frame;

        #region Public Methods

        public NavigationService(Frame frame)
        {
            _frame = frame;
        }

        public void GoForward()
        {
            _frame.GoForward();
        }

        public void GoBack()
        {
            _frame.GoBack();
        }

        public bool Navigate(string page)
        {
            var type = Assembly.GetExecutingAssembly().GetTypes().SingleOrDefault(a => a.Name.Equals(page));

            if (type == null) return false;

            var src = Activator.CreateInstance(type);
            return _frame.Navigate(src);
        }

        public bool Navigate<T>(object parameter = null)
        {
            var type = typeof(T);
            return Navigate(type, parameter);
        }

        public bool Navigate(Type source, object parameter = null)
        {
            var src = Activator.CreateInstance(source);
            return _frame.Navigate(src, parameter);
        }

        #endregion
    }
}