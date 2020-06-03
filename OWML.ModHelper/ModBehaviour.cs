using OWML.Common;
using System.Collections.Generic;
using UnityEngine;

namespace OWML.ModHelper
{
    public class ModBehaviour : MonoBehaviour, IModBehaviour
    {
        public IModHelper ModHelper { get; private set; }

        public object Interface { get; private set; }

        public void Init(IModHelper modHelper)
        {
            ModHelper = modHelper;
            Configure(modHelper.Config);
            DontDestroyOnLoad(gameObject);
            Interface = GetApi();
            if (Interface != null) ModHelper.Console.WriteLine("Interface is not null!");
        }

        public virtual void Configure(IModConfig config)
        {
        }

        /// <summary>Returns list of mods that depend on the current mod.</summary>
        public IList<IModBehaviour> GetDependants()
        {
            return ModHelper.Interaction.GetDependants(ModHelper.Manifest.UniqueName);
        }

        /// <summary>Returns dependencies of current mod.</summary>
        public IList<IModBehaviour> GetDependencies()
        {
            return ModHelper.Interaction.GetDependencies(ModHelper.Manifest.UniqueName);
        }

        public virtual object GetApi() => null;
    }
}
