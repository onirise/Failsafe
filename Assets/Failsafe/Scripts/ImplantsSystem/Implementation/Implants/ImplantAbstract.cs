using System;
using UnityEngine;

namespace Failsafe.Scripts.ImplantsSystem.Implementation.Implants
{
    [Serializable]
    public abstract class ImplantAbstract : IImplant
    {
        [field: SerializeField]
        public string ID { get; private set; }
        [field: SerializeField]
        public string PlaceholderId { get; private set; }
        
        protected ImplantAbstract(string id, string placeholderId)
        {
            ID = id;
            PlaceholderId = placeholderId;
        }
    }
}