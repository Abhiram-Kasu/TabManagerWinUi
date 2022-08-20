using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TabManagerWinUi
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {

        private NavigationViewItem _lastItem;
        public MainWindow()
        {
            this.InitializeComponent();
            
            

            NavigateToView("Home");
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer is not NavigationViewItem item || item == _lastItem)
            {
                return;
            }
            var clickedItem = item.Tag.ToString() ?? "SettingsView";
            if (!NavigateToView(clickedItem)) return;
            _lastItem = item;
        }   

        private bool NavigateToView(string clickedView)
        {
            var view = Assembly.GetExecutingAssembly().GetType($"TabManagerWinUi.Views.{clickedView + "View"}");
            if (string.IsNullOrEmpty(clickedView))
            {
                return false;
            }

            ContentFrame.Navigate(view, null, new EntranceNavigationTransitionInfo());
            return true;
        }

       
    }
}
