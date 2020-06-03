using Newtonsoft.Json;
using OWML.Common;

namespace OWML.ModHelper
{
    /// <summary>Contains the config and path data for OWML.</summary>
    public class OwmlConfig : IOwmlConfig
    {
        /// <summary>The path of the game exe.</summary>
        [JsonProperty("gamePath")]
        public string GamePath { get; set; }

        /// <summary>Is OWML in verbose mode?.</summary>
        [JsonProperty("verbose")]
        public bool Verbose { get; private set; }

        /// <summary>The path of the game data.</summary>
        [JsonIgnore]
        public string DataPath => $"{GamePath}/OuterWilds_Data";

        /// <summary>The path of the managed game data..</summary>
        [JsonIgnore]
        public string ManagedPath => $"{DataPath}/Managed";

        /// <summary>The path of the game plugins.</summary>
        [JsonIgnore]
        public string PluginsPath => $"{DataPath}/Plugins";

        /// <summary>The path OWML is located in.</summary>
        [JsonProperty("owmlPath")]
        public string OWMLPath { get; set; }

        /// <summary>The path of OWML's log file.</summary>
        [JsonIgnore]
        public string LogFilePath => $"{OWMLPath}Logs/OWML.Log.txt";

        /// <summary>The path of OWML's output file.</summary>
        [JsonIgnore]
        public string OutputFilePath => $"{OWMLPath}Logs/OWML.Output.txt";

        /// <summary>The location of the mods.</summary>
        [JsonIgnore]
        public string ModsPath => $"{OWMLPath}Mods";
    }
}
