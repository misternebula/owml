using OWML.Common;
using System.Collections.Generic;
using UnityEngine;

namespace OWML.ModHelper
{
    public class ModBehaviour : MonoBehaviour, IModBehaviour
    {
        /// <summary>
        /// The attached ModHelper.
        /// </summary>
        public IModHelper ModHelper { get; private set; }

        /// <summary>
        /// The mod API.
        /// </summary>
        public object Api { get; private set; }

        /// <summary>
        /// Set up the mod behaviour.
        /// </summary>
        /// <param name="modHelper">ModHelper to set up with.</param>
        public void Init(IModHelper modHelper)
        {
            ModHelper = modHelper;
            Configure(modHelper.Config);
            DontDestroyOnLoad(gameObject);
            Api = GetApi();
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
