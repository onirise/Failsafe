using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Failsafe.Granate
{
    public class GrenadeView : MonoBehaviour
    {
        [SerializeField] private Grenade grenade;
        [SerializeField] private ParticleSystem _explodeParticle;
        //[SerializeField] private AudioClip _explodeSound;

        private void Start()
        {
            grenade.OnExplode += OnExplodeVisual;
        }

        private void OnExplodeVisual()
        {
            _explodeParticle.Play();
        }

        private void OnDestroy()
        {
            grenade.OnExplode -= OnExplodeVisual;
        }
    }
}