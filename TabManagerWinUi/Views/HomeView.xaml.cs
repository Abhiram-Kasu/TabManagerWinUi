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
        
        private readonly ISerializeListService _serializeListService;
        private readonly ObservableCollection<TabGroup> _tabGroups = new();

        public HomeView()
        {
            this.InitializeComponent();

            _serializeListService = new SerializeListService();

            

            _tabGroups.CollectionChanged += async (sender, e) => await UpdateList();
            
        }

        private async Task UpdateList()
        {
            TabGroupsListView.ItemsSource = null;
            TabGroupsListView.ItemsSource = _tabGroups;
            await _serializeListService.UpdateTabGroups(_tabGroups);
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
                Name = name,
                Tabs = new List<Tab>()
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

        private async void AddTab_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var name = await ShowAddDialogAsync("Link Name:");
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
            var link = await ShowAddDialogAsync("Link Address:");
            if(!string.IsNullOrEmpty(link) && Uri.TryCreate(link, UriKind.Absolute, out var uri))
            {
                int index = 0;
                foreach(var item in _tabGroups)
                {
                    
                    if (item.Id.ToString() == button.CommandParameter.ToString())
                    {
                        break;
                    }
                    index++;

                }
                var copy = _tabGroups[index];
                copy.Tabs = copy.Tabs.Append(new Tab
                {
                    Name = name,
                    Link = link
                });
                _tabGroups[index] = copy;
                
                
                await UpdateList();

            }
            else
            {
                var d = new ContentDialog
                {
                    PrimaryButtonText = "Ok",
                    Title = "You did not enter a valid link address!",
                    XamlRoot = this.Content.XamlRoot
                };
                await d.ShowAsync();
                return;
            }
        }

        private void TabGroupsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var listViewItem = (sender as ListView).ContainerFromItem(e.ClickedItem) as ListViewItem;
            if (listViewItem.ContentTemplate == (DataTemplate)this.Resources["ShowTabs"])
            {
                listViewItem.ContentTemplate = (DataTemplate)this.Resources["HideTabs"];
            }
            else
            {
                listViewItem.ContentTemplate = (DataTemplate)this.Resources["ShowTabs"];
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (await _serializeListService.DoesExistingPrefsExist())
            {
                
                var list = await _serializeListService.GetTabGroups();
                foreach(var item in list)
                {
                    _tabGroups.Add(item);
                }
            }
                

        }

        private async void RemoveTabButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            
            foreach(var tabGroup in _tabGroups)
            {
                tabGroup.Tabs = tabGroup.Tabs.Where(x => x.ID.ToString() != button.Tag.ToString()).ToArray();



            }
            await UpdateList();
        }

        private async void OpenTabs_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            
            
            foreach (var item in _tabGroups.Where(x => x.Id.ToString() == button.CommandParameter.ToString()).First().Tabs)
            {
                await Launcher.LaunchUriAsync(new Uri(item.Link));
            }
            
        }
    }
}
