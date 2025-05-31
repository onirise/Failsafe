using Failsafe.Scripts.ImplantsSystem.Implementation.Implants;

namespace Failsafe.Scripts.ImplantsSystem.Implementation.Processors
{
    public class SimpleImplantProcessor : IImplantProcessor<SimpleImplant>
    {
        public void Process(SimpleImplant implant)
        {
            //do something with player data model or communicate with other systems
        }

        public void Remove(SimpleImplant implant)
        {
            //opposite of "add"
        }

        public bool CanProcess(SimpleImplant implant)
        {
            //check if can add implant
            
            return true;
        }
    }
}