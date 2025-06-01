using System;
using System.Collections.Generic;

namespace Failsafe.Scripts.ImplantsSystem.Implementation
{
    public class ImplantsService : IImplantsService
    {
        public event Action<IImplant> ImplantAdded = delegate { };
        public event Action<IImplant> ImplantRemoved = delegate { };
        
        private readonly Dictionary<Type, IImplantProcessor> _implantProcessorsMap = new ();
        private readonly HashSet<IImplant> _implants = new ();

        public IReadOnlyCollection<IImplant> InstalledImplants => _implants;

        public void Add(IImplant implant)
        {
            ProcessAddImplant(implant);
        }

        public void Remove(IImplant implant)
        {
            ProcessRemoveImplant(implant);
        }

        public bool CanAdd(IImplant implant)
        {
            var placeholder = GetProcessor(implant);

            return placeholder.CanAdd(implant) && !_implants.Contains(implant);
        }
        
        public void RegisterProcessor(IImplantProcessor implantProcessor)
        {
            _implantProcessorsMap.TryAdd(implantProcessor.Type,  implantProcessor);
        }

        public void UnregisterProcessor(IImplantProcessor implantProcessor)
        {
            _implantProcessorsMap.Remove(implantProcessor.Type);
        }

        private void ProcessAddImplant(IImplant implant)
        {
            var implantProcessor = GetProcessor(implant);
            
            implantProcessor.Process(implant);

            _implants.Add(implant);
            
            ImplantAdded.Invoke(implant);
        }
        
        private void ProcessRemoveImplant(IImplant implant)
        {
            var implantProcessor = GetProcessor(implant);
            
            implantProcessor.Remove(implant);
            
            _implants.Remove(implant);
            
            ImplantRemoved.Invoke(implant);
        }
        
        private IImplantProcessor GetProcessor(IImplant implant)
        {
            if (!_implantProcessorsMap.TryGetValue(implant.GetType(), out var implantProcessor))
            {
                throw new Exception($"There is no processor with id {implant.GetType()}");
            }
            
            return implantProcessor;
        }
    }
}