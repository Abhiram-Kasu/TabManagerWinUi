using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabManagerWinUi.Extensions;
using TabManagerWinUi.Models;
using Windows.Storage;

namespace TabManagerWinUi.Services
{
    internal class SerializeListService : ISerializeListService
    {
        private static readonly StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        private const string _fileName = "TabGroupsSerialized.json";
        private StorageFile _file;
        public async Task<bool> DoesExistingPrefsExist()
        {
            _file = await storageFolder.CreateFileAsync(_fileName,
        Windows.Storage.CreationCollisionOption.OpenIfExists);
            using var stream = await _file.OpenStreamForReadAsync();
            
            return stream.Length > 0;
            
           
        }

        public async Task<IEnumerable<TabGroup>> GetTabGroups()
        {
            var bytes = await File.ReadAllBytesAsync(_file.Path);
            var s = Encoding.ASCII.GetString(bytes);
            
            return JsonConvert.DeserializeObject<IEnumerable<TabGroup>>(s);
        }

        public async Task UpdateTabGroups(IEnumerable<TabGroup> tabGroups)
        {
            
            var bytes = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(tabGroups));
            try
            {
                await File.WriteAllBytesAsync(_file.Path, bytes);
            }
            catch (Exception) { }
            
            
        }

        public void ClearData()
        {
            File.Delete(_file.Path);
        }

        public string GetPathToData() => _file.Path;
       
    }
}
