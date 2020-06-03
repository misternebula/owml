using System.Collections.Generic;

namespace OWML.Common
{
    public interface IModBehaviour
    {
        IModHelper ModHelper { get; }

        object Interface { get; }

        void Configure(IModConfig config);

        /// <summary>Returns list of mods that depend on the current mod.</summary>
        IList<IModBehaviour> GetDependants();

        /// <summary>Returns dependencies of current mod.</summary>
        IList<IModBehaviour> GetDependencies();

        object GetApi();
    }
}
