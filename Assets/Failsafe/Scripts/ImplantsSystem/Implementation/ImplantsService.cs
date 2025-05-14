using System;
using System.Collections.Generic;

namespace Failsafe.Scripts.ImplantsSystem.Implementation
{
    public class ImplantsService : IImplantsService
    {
        public event Action<IImplant> ImplantAdded = delegate { };
        public event Action<IImplant> ImplantRemoved = delegate { };
        public event Action<IImplantsPlaceholder> PlaceholderAdded = delegate { };
        public event Action<IImplantsPlaceholder> PlaceholderRemoved = delegate { };
        
        private readonly Dictionary<string, IImplantsPlaceholder> _implantsPlaceholders = new ();
        private readonly Dictionary<Type, IImplantProcessor> _implantProcessorsMap = new ();
        
        public IEnumerable<IImplantsPlaceholder> Placeholders => _implantsPlaceholders.Values;

        public ImplantsService() { }
        
        public void AddImplant(IImplant implant)
        {
            var placeholder = GetPlaceholder(implant.PlaceholderId);
            
            AddToPlaceholder(implant, placeholder);
            
            ProcessAddImplant(implant);
        }

        public void RemoveImplant(IImplant implant)
        {
            var placeholder = GetPlaceholder(implant.PlaceholderId);
            
            RemoveFromPlaceholder(implant, placeholder);
            
            ProcessRemoveImplant(implant);
        }

        public bool CanAddImplant(IImplant implant)
        {
            var placeholder = GetPlaceholder(implant.PlaceholderId);

            return placeholder.CanAddImplant;
        }

        public void RegisterPlaceholder(IImplantsPlaceholder placeholder)
        {
            _implantsPlaceholders.Add(placeholder.ID, placeholder);

            if (placeholder.InstalledImplantsCount > 0)
            {
                foreach (var implant in placeholder.InstalledImplants)
                {
                    ProcessAddImplant(implant);
                }
            }
            
            PlaceholderAdded.Invoke(placeholder);
        }

        public void UnregisterPlaceholder(IImplantsPlaceholder placeholder)
        {
            if (placeholder.InstalledImplantsCount > 0)
            {
                foreach (var implant in placeholder.InstalledImplants)
                {
                    ProcessRemoveImplant(implant);
                }
            }
            
            _implantsPlaceholders.Remove(placeholder.ID);
            
            PlaceholderRemoved.Invoke(placeholder);
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
            
            implantProcessor.Add(implant);
            
            ImplantAdded.Invoke(implant);
        }
        
        private void ProcessRemoveImplant(IImplant implant)
        {
            var implantProcessor = GetProcessor(implant);
            
            implantProcessor.Remove(implant);
            
            ImplantRemoved.Invoke(implant);
        }

        private IImplantsPlaceholder GetPlaceholder(string placeholderId)
        {
            if (!_implantsPlaceholders.TryGetValue(placeholderId, out var implantsPlaceholder))
            {
                throw new Exception($"There is no placeholder with id {placeholderId}");
            }

            return implantsPlaceholder;
        }

        private IImplantProcessor GetProcessor(IImplant implant)
        {
            if (!_implantProcessorsMap.TryGetValue(implant.GetType(), out var implantProcessor))
            {
                throw new Exception($"There is no processor with id {implant.GetType()}");
            }
            
            return implantProcessor;
        }

        private void AddToPlaceholder(IImplant implant, IImplantsPlaceholder placeholder)
        {
            if (placeholder.Contains(implant))
            {
                throw new Exception($"Implant already added {implant.ID}");
            }
            
            placeholder.Add(implant);
        }

        private void RemoveFromPlaceholder(IImplant implant, IImplantsPlaceholder placeholder)
        {
            if (!placeholder.Contains(implant))
            {
                throw new Exception($"There is no implant with id {implant.ID}");
            }
            
            placeholder.Remove(implant);
        }
    }
}