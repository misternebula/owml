using System;
using System.IO;
using Newtonsoft.Json;
using OWML.Common;

namespace OWML.ModHelper
{
    public class ModStorage : IModStorage
    {
        private readonly IModConsole _console;
        private readonly IModManifest _manifest;

        public ModStorage(IModConsole console, IModManifest manifest)
        {
            _console = console;
            _manifest = manifest;
        }

        /// <summary>Deserialize JSON file to given type.</summary>
        /// <typeparam name="T">The type to deserialize the file to.</typeparam>
        /// <param name="filename">The name of the file. The folder that the mod is in is automatically added to the beginning.</param>
        public T Load<T>(string filename)
        {
            var path = _manifest.ModFolderPath + filename;
            if (!File.Exists(path))
            {
                return default;
            }
            try
            {
                var json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                _console.WriteLine($"Error while loading {path}: {ex}");
                return default;
            }
        }

        /// <summary>Serialize object to JSON file.</summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="filename">The name of the output file. The folder that the mod is in is automatically added to the beginning.</param>
        public void Save<T>(T obj, string filename)
        {
            var path = _manifest.ModFolderPath + filename;
            try
            {
                var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                _console.WriteLine($"Error while saving {path}: {ex}");
            }
        }

    }
}
