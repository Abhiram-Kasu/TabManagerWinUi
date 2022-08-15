using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TabManagerWinUi.Models;
using TabManagerWinUi.Services;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TabManagerWinUi.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomeView : Page
    {
        

        private readonly ObservableCollection<TabGroup> _tabGroups = new();

        public HomeView()
        {
            this.InitializeComponent();
            
            _tabGroups.CollectionChanged += (sender, e) => TabGroupsListView.ItemsSource = _tabGroups;
            _tabGroups.Add(new TabGroup()
            {
                Name = "TabGroup",
                Tabs = new List<Tab>()
                {
                    new Tab()
                    {
                        Name = "Home Page",
                        Link = @"https://stackoverflow.com/questions/40026593/uwp-listview-how-to-expand-an-item-when-select-it"

                    }
                }
            });
            _tabGroups.Add(new TabGroup()
            {
                Name = "Second Tab Group",
                Tabs = new List<Tab>()
                {
                    new Tab()
                    {
                        Name = "Second Page",
                        Link = @"https://www.google.com/search?q=how+to+comment+code+visual+studio+2022+windows+11&rlz=1C1CHBF_enUS905US905&oq=how+to+comment+code+visual+studio+2022+windows+11&aqs=chrome..69i57.10658j0j4&sourceid=chrome&ie=UTF-8"

                    },
                     new Tab()
                    {
                        Name = "Secondary Second Page",
                        Link = @"https://apps.microsoft.com/store/detail/fluent-xaml-theme-editor/9N2XD3Q8X57C?hl=en-us&gl=US"

                    }
                }
            });
        }

        private async void AddTabGroupButton_Click(object sender, RoutedEventArgs e)
        {
            
            string name = await ShowAddDialogAsync("Tab Group Name");
            if (string.IsNullOrEmpty(name))
            {
                var d = new ContentDialog
                {
                    PrimaryButtonText = "Ok",
                    Title = "You did not enter a valid name!",
                    XamlRoot = this.Content.XamlRoot
                };
                await d.ShowAsync();
                return;
                
            }
            _tabGroups.Add(new TabGroup()
            {
                Name = name
            });

        }

        private async Task<string> ShowAddDialogAsync(string title)
        {
            var inputTextBox = new TextBox { AcceptsReturn = false };
            (inputTextBox as FrameworkElement).VerticalAlignment = VerticalAlignment.Bottom;
            var dialog = new ContentDialog
            {
                Content = inputTextBox,
                Title = title,
                IsSecondaryButtonEnabled = true,
                PrimaryButtonText = "Ok",
                SecondaryButtonText = "Cancel",
                XamlRoot = this.Content.XamlRoot
            };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
                return inputTextBox.Text;
            else
                return "";
        }

        private async void LinkTextBlock_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri((sender as TextBlock).Tag.ToString()));
        }

        private void TabsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Assign DataTemplate for selected items
            foreach (var item in e.AddedItems)
            {
                ListViewItem lvi = (sender as ListView).ContainerFromItem(item) as ListViewItem;
                lvi.ContentTemplate = (DataTemplate)this.Resources["Detail"];
                
                
            }
            //Remove DataTemplate for unselected items
            foreach (var item in e.RemovedItems)
            {
                ListViewItem lvi = (sender as ListView).ContainerFromItem(item) as ListViewItem;
                lvi.ContentTemplate = (DataTemplate)this.Resources["Normal"];
            }
        }

        private void TabsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var listViewItem = (sender as ListView).ContainerFromItem(e.ClickedItem) as ListViewItem;
            if(listViewItem.ContentTemplate == (DataTemplate)this.Resources["Detail"])
            {
                listViewItem.ContentTemplate = (DataTemplate)this.Resources["Normal"];
            }
            else
            {
                listViewItem.ContentTemplate = (DataTemplate)this.Resources["Detail"];
            }

        }

        private void RemoveTabGroup_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            _tabGroups.Remove(_tabGroups.Where(x => x.Id.ToString() == button.CommandParameter.ToString()).First());
        }
    }
}
