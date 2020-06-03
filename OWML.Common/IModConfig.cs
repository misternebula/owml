using System;
using System.Collections.Generic;

namespace OWML.Common
{
    /// <summary>
    /// The config file for each mod.
    /// </summary>
    public interface IModConfig
    {
        /// <summary>Is the mod enabled?</summary>
        bool Enabled { get; set; }

        /// <summary>Does the mod use VR?</summary>
        bool RequireVR { get; set; }

        Dictionary<string, object> Settings { get; set; }

        /// <summary>Get a value from the config file.</summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The name of the setting.</param>
        T GetSettingsValue<T>(string key);

        /// <summary>Set a value in the config file.</summary>
        /// <param name="key">The name of the setting.</param>
        /// <param name="value">What value to set it to.</param>
        void SetSettingsValue(string key, object value);

        [Obsolete("Use GetSettingsValue instead")]
        T GetSetting<T>(string key);
    }
}
