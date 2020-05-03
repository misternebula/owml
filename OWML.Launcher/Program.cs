﻿using System;
using System.IO;
using Newtonsoft.Json;
using OWML.Common;
using OWML.ModHelper;
using OWML.ModHelper.Logging;
using OWML.ModLoader;
using OWML.Patcher;

namespace OWML.Launcher
{
    public class Program
    {
        static void Main(string[] args)
        {
            var owmlConfig = GetOwmlConfig();
            var owmlManifest = GetOwmlManifest();
            var writer = GetWriter(owmlManifest);
            var modFinder = new ModFinder(owmlConfig, writer);
            var outputListener = new OutputListener(owmlConfig);
            var pathFinder = new PathFinder(owmlConfig, writer);
            var owPatcher = new OWPatcher(owmlConfig, writer);
            var vrPatcher = new VRPatcher(owmlConfig, writer);
            var app = new App(owmlConfig, owmlManifest, writer, modFinder, outputListener, pathFinder, owPatcher, vrPatcher);
            app.Run(args);
        }

        private static IModConsole GetWriter(IModManifest owmlManifest)
        {
            if (CommandLineArguments.HasArgument(Constants.ConsolePortArgument))
            {
                return new ModSocketOutput(null, owmlManifest);
            }
            else
            {
                return new OutputWriter();
            }
        }

        private static IOwmlConfig GetOwmlConfig()
        {
            var config = GetJsonObject<OwmlConfig>("OWML.Config.json");
            config.OWMLPath = AppDomain.CurrentDomain.BaseDirectory;
            return config;
        }

        private static IModManifest GetOwmlManifest()
        {
            return GetJsonObject<ModManifest>("OWML.Manifest.json");
        }

        private static T GetJsonObject<T>(string filename)
        {
            var json = File.ReadAllText(filename)
                .Replace("\\\\", "/")
                .Replace("\\", "/");
            return JsonConvert.DeserializeObject<T>(json);
        }

    }
}
