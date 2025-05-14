using System.Collections.Generic;

namespace Failsafe.Scripts.ImplantsSystem
{
    public interface IImplantsPlaceholder
    {
        string ID { get; }
        
        int InstalledImplantsCount { get; }
        int MaxCapacity { get; }
        
        bool CanAddImplant { get; }
        
        IEnumerable<IImplant> InstalledImplants { get; }
        
        void Add(IImplant implant);
        void Remove(IImplant implant);
        bool Contains(IImplant implant);
    }
}