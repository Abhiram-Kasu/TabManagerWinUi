using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TabManagerWinUi.Services;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TabManagerWinUi.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsView : Page
    {
        ISerializeListService _serializeListService;
        public SettingsView()
        {
            _serializeListService = new SerializeListService();
            _ = _serializeListService.DoesExistingPrefsExist();
            this.InitializeComponent();
        }

        private async void ClearSerializedData_Click(object sender, RoutedEventArgs e)
        {
            var d = new ContentDialog();
            d.XamlRoot = this.Content.XamlRoot;
            d.Title = "Are you sure you want to delete this data, it is irreversible!";
            
            d.PrimaryButtonText = "Yes";
            d.SecondaryButtonText = "No";
            d.PrimaryButtonStyle = this.Resources["WarningButton"] as Style;
            
            if(await d.ShowAsync() == ContentDialogResult.Primary)
            {
                _serializeListService.ClearData();
                var dd = new ContentDialog
                {
                    Title = "Done",
                    PrimaryButtonText = "Ok"

                };
                dd.XamlRoot = this.Content.XamlRoot;
                await dd.ShowAsync();
            }
            
        }

        private void CopyPathToSerializedData_Click(object sender, RoutedEventArgs e)
        {
            var dp = new DataPackage();
            dp.SetText(_serializeListService.GetPathToData());
            Clipboard.SetContent(dp);
           
            
        }
    }
}
