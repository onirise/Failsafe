using System;

namespace Failsafe.Scripts.ImplantsSystem
{
    public interface IImplantProcessor
    {
        Type Type { get; }

        void Add(IImplant implant);
        void Remove(IImplant implant);
    }

    public interface IImplantProcessor<in T> : IImplantProcessor where T : IImplant
    {
        Type IImplantProcessor.Type => typeof(T);

        void IImplantProcessor.Add(IImplant implant)
        {
            if (implant is not T tImplant)
            {
                throw new ArgumentException($"Implant must be of type {typeof(T).FullName}", nameof(implant));
            }
            
            Add(tImplant);
        }

        void IImplantProcessor.Remove(IImplant implant)
        {
            if (implant is not T tImplant)
            {
                throw new ArgumentException($"Implant must be of type {typeof(T).FullName}", nameof(implant));
            }
            
            Remove(tImplant);
        }
        
        void Add(T implant);
        void Remove(T implant);
    } 
}