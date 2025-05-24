using System;

namespace Failsafe.Scripts.ImplantsSystem
{
    public interface IImplantProcessor
    {
        Type Type { get; }

        void Process(IImplant implant);
        void Remove(IImplant implant);
        bool CanAdd(IImplant implant);
    }

    public interface IImplantProcessor<in T> : IImplantProcessor where T : IImplant
    {
        Type IImplantProcessor.Type => typeof(T);

        void IImplantProcessor.Process(IImplant implant)
        {
            if (implant is not T tImplant)
            {
                throw new ArgumentException($"Implant must be of type {typeof(T).FullName}", nameof(implant));
            }
            
            Process(tImplant);
        }

        void IImplantProcessor.Remove(IImplant implant)
        {
            if (implant is not T tImplant)
            {
                throw new ArgumentException($"Implant must be of type {typeof(T).FullName}", nameof(implant));
            }
            
            Remove(tImplant);
        }
        
        bool IImplantProcessor.CanAdd(IImplant implant)
        {
            if (implant is not T tImplant)
            {
                throw new ArgumentException($"Implant must be of type {typeof(T).FullName}", nameof(implant));
            }
            
            return CanProcess(tImplant);
        }
        
        void Process(T implant);
        void Remove(T implant);
        bool CanProcess(T implant);
    } 
}