using Failsafe.Player.Scripts;
using Failsafe.Scripts.EffectSystem;
using Failsafe.Scripts.Health;
using UnityEngine;
using VContainer;

namespace Failsafe.Environment.FireArea
{
    public class EffectArea : MonoBehaviour
    {


        void OnTriggerEnter(Collider other)
        {
            
            if (other.TryGetComponent(out FlammableComponent flammable))
            {
                flammable.SetOnFire();
                
                
            }
        }
        

    }


    

}