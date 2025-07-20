using Failsafe.Scripts.EffectSystem;
using Failsafe.Scripts.Health;
using UnityEngine;
using VContainer;

namespace Failsafe.Player.Scripts
{
    
    public class FlammableComponent : MonoBehaviour
    {
        [Inject] private IEffectManager effectManager;
        [Inject] private IHealth _health;

        public void SetOnFire()
        {
            effectManager.ApplyEffect(new FireEffect(_health, 5));
            
        }


    
    


    }
    
    
    
    public class FireEffect : Effect
    { 
   
        private IHealth _health;

        public FireEffect(IHealth health, float duration)
        {
            _health = health;
            _duration = duration;
        }
        public override void ApplyEffect()
        {
            
        }

        public override void ClearEffect()
        {

        }

        public override void Update()
        {
            _health.AddHealth(-0.5f);
        }
    }
}


