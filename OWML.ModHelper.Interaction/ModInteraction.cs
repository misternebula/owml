using OWML.Common;
using System.Collections.Generic;
using System.Linq;

namespace OWML.ModHelper.Interaction
{
    /// <summary>Per-mod handler for interaction between mods.</summary>
    public class ModInteraction : IModInteraction
    {
        private readonly IList<IModBehaviour> _modList;

        private readonly InterfaceProxyFactory _proxyFactory;

        private readonly IModManifest _modManifest;

        private Dictionary<string, List<IModBehaviour>> _dependantDict = new Dictionary<string, List<IModBehaviour>>();

        private Dictionary<string, List<IModBehaviour>> _dependencyDict = new Dictionary<string, List<IModBehaviour>>();

        /// <summary>Construct an instance.</summary>
        /// <param name="list">The list of loaded mods.</param>
        /// <param name="proxyFactory">The proxy factory instance.</param>
        /// <param name="manifest">The manifest of the mod this instance is attached to.</param>
        public ModInteraction(IList<IModBehaviour> list, InterfaceProxyFactory proxyFactory, IModManifest manifest)
        {
            _modList = list;
            _modManifest = manifest;
            _proxyFactory = proxyFactory;
            RegenerateDictionaries();
        }

        private void RegenerateDictionaries()
        {
            _dependantDict = new Dictionary<string, List<IModBehaviour>>();
            _dependencyDict = new Dictionary<string, List<IModBehaviour>>();
            foreach (var mod in _modList)
            {
                var dependants = new List<IModBehaviour>();
                var dependencies = new List<IModBehaviour>();
                foreach (var dependency in _modList)
                { 
                    if (dependency.ModHelper.Manifest.Dependencies.Contains(mod.ModHelper.Manifest.UniqueName))
                    {
                        dependants.Add(dependency);
                    }

                    if (mod.ModHelper.Manifest.Dependencies.Contains(dependency.ModHelper.Manifest.UniqueName))
                    {
                        dependencies.Add(dependency);
                    }
                }
                _dependantDict[mod.ModHelper.Manifest.UniqueName] = dependants;
                _dependencyDict[mod.ModHelper.Manifest.UniqueName] = dependencies;
            }
        }

        /// <summary>Returns list of mods that depend on the given mod.</summary>
        /// <param name="dependencyUniqueName">The unique name of the mod.</param>
        public IList<IModBehaviour> GetDependants(string dependencyUniqueName)
        {
            if (_dependantDict.Count != _modList.Count)
            {
                RegenerateDictionaries();
            }
            return _dependantDict[dependencyUniqueName];
        }

        /// <summary>Returns list of dependencies of the given mod.</summary>
        /// <param name="uniqueName">The unique name of the mod.</param>
        public IList<IModBehaviour> GetDependencies(string uniqueName)
        {
            if (_dependantDict.Count != _modList.Count)
            {
                RegenerateDictionaries();
            }
            return _dependencyDict[uniqueName];
        }

        /// <summary>Return the mod that maches the given name.</summary>
        /// <param name="uniqueName">The unique name of the mod.</param>
        public IModBehaviour GetMod(string uniqueName)
        {
            return _modList.First(m => m.ModHelper.Manifest.UniqueName == uniqueName);
        }

        private object GetApi(string uniqueName)
        {
            var mod = GetMod(uniqueName);
            return mod.Api;
        }

        /// <summary>Get the API of a given mod.</summary>
        /// <typeparam name="TInterface">The interface through which to access the API.</typeparam>
        /// <param name="uniqueName">The unique name of the mod providing the API.</param>
        public TInterface GetApi<TInterface>(string uniqueName) where TInterface : class
        {
            var inter = GetApi(uniqueName);
            if (inter == null)
            {
                return null;
            }

            if (inter is TInterface castInter)
            {
                return castInter;
            }

            return _proxyFactory.CreateProxy<TInterface>(inter, _modManifest.UniqueName, uniqueName);
        }

        /// <summary>Returns list of all loaded mods - disabled and enabled.</summary>
        public IList<IModBehaviour> GetMods()
        {
            return _modList;
        }

        /// <summary>Returns true if a given mod is loaded - *not* if the mod is enabled/disabled.</summary>
        /// <param name="uniqueName">The unique name of the mod.</param>
        public bool ModExists(string uniqueName)
        {
            return _modList.Any(m => m.ModHelper.Manifest.UniqueName == uniqueName);
        }
    }
}
