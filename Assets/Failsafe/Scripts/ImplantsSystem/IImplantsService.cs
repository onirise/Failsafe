using System;
using System.Collections.Generic;

namespace Failsafe.Scripts.ImplantsSystem
{
    public interface IImplantsService
    {
        event Action<IImplant> ImplantAdded; 
        event Action<IImplant> ImplantRemoved;
        
        IReadOnlyCollection<IImplant> InstalledImplants { get; }
        
        void Add(IImplant implant);
        void Remove(IImplant implant);
        bool CanAdd(IImplant implant);
        
        void RegisterProcessor(IImplantProcessor implantProcessor);
        void UnregisterProcessor(IImplantProcessor implantProcessor);
    }
}