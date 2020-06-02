using System.Collections.Generic;

namespace OWML.Common
{
    public interface IModBehaviour
    {
        IModHelper ModHelper { get; }
        object Interface { get; }
        void Configure(IModConfig config);
        IList<IModBehaviour> GetDependants();
        IList<IModBehaviour> GetDependencies();
        void SetInterface(object inter);
    }
}
