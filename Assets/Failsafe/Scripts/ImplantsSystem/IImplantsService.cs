using System;
using System.Collections.Generic;

namespace Failsafe.Scripts.ImplantsSystem
{
    public interface IImplantsService
    {
        event Action<IImplant> ImplantAdded; 
        event Action<IImplant> ImplantRemoved;
        
        event Action<IImplantsPlaceholder> PlaceholderAdded;
        event Action<IImplantsPlaceholder> PlaceholderRemoved;
        
        IEnumerable<IImplantsPlaceholder> Placeholders { get; }
        
        void AddImplant(IImplant implant);
        void RemoveImplant(IImplant implant);
        bool CanAddImplant(IImplant implant);
        
        void RegisterPlaceholder(IImplantsPlaceholder placeholder);
        void UnregisterPlaceholder(IImplantsPlaceholder placeholder);
        void RegisterProcessor(IImplantProcessor implantProcessor);
        void UnregisterProcessor(IImplantProcessor implantProcessor);
    }
}