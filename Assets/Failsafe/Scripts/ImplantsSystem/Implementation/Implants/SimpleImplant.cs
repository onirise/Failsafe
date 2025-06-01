using System;

namespace Failsafe.Scripts.ImplantsSystem.Implementation.Implants
{
    [Serializable]
    public class SimpleImplant : ImplantAbstract
    {
        //can be filled with fields to modify player data model or impact to other systems
        
        public SimpleImplant(string id, string placeholderId) : base(id, placeholderId) { }
    }
}