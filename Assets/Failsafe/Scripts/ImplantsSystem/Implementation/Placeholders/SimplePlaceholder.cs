using System;
using System.Collections.Generic;

namespace Failsafe.Scripts.ImplantsSystem.Implementation.Placeholders
{
    public class SimplePlaceholder : IImplantsPlaceholder
    {
        private readonly int _placeholdersCount;
        private readonly string _placeholderId;
        
        private readonly HashSet<IImplant> _implants = new ();

        public string ID => _placeholderId;
        public int InstalledImplantsCount => _implants.Count;
        public int MaxCapacity => _placeholdersCount;
        public bool CanAddImplant => MaxCapacity > InstalledImplantsCount;

        public IEnumerable<IImplant> InstalledImplants => _implants;

        public SimplePlaceholder(string placeholderId, int placeholdersCount)
        {
            _placeholderId = placeholderId;
            _placeholdersCount = placeholdersCount;
        }
        
        public void Add(IImplant implant)
        {
            if (!CanAddImplant)
            {
                throw new ArgumentOutOfRangeException(implant.GetType().Name);
            }
            
            _implants.Add(implant);
        }

        public void Remove(IImplant implant)
        {
            _implants.Remove(implant);
        }

        public bool Contains(IImplant implant)
        {
            return _implants.Contains(implant);
        }
    }
}