﻿
using OWML.Common;
using OWML.Common.Menus;

namespace OWML.ModHelper
{
    public class ModHelper : IModHelper
    {
        public IModLogger Logger { get; }
        public IModConsole Console { get; }
        public IHarmonyHelper HarmonyHelper { get; }
        public IModEvents Events { get; }
        public IModAssets Assets { get; }
        public IModStorage Storage { get; }
        public IModMenus Menus { get; }
        public IModManifest Manifest { get; }
        public IModConfig Config { get; }
        public IOwmlConfig OwmlConfig { get; }
        public IModInteraction Interaction { get; }

        /// <summary>
        /// Create an instance.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="console"></param>
        /// <param name="harmonyHelper"></param>
        /// <param name="events"></param>
        /// <param name="assets"></param>
        /// <param name="storage"></param>
        /// <param name="menus"></param>
        /// <param name="manifest"></param>
        /// <param name="config"></param>
        /// <param name="owmlConfig"></param>
        /// <param name="interaction"></param>
        public ModHelper(IModLogger logger, IModConsole console, IHarmonyHelper harmonyHelper, IModEvents events, 
            IModAssets assets, IModStorage storage, IModMenus menus, IModManifest manifest, IModConfig config, IOwmlConfig owmlConfig, IModInteraction interaction)
        {
            Logger = logger;
            Console = console;
            HarmonyHelper = harmonyHelper;
            Events = events;
            Assets = assets;
            Storage = storage;
            Menus = menus;
            Manifest = manifest;
            Config = config;
            OwmlConfig = owmlConfig;
            Interaction = interaction;
        }

    }
}
