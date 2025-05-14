using Failsafe.Scripts.ImplantsSystem.Implementation.Implants;

namespace Failsafe.Scripts.ImplantsSystem.Implementation.Processors
{
    public class SimpleImplantProvider :  IImplantProcessor<SimpleImplant>
    {
        public void Add(SimpleImplant implant)
        {
            //do something with player data model or communicate with other systems
        }

        public void Remove(SimpleImplant implant)
        {
            //opposite of "add"
        }
    }
}