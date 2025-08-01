using System;
using UnityEngine;

namespace Failsafe.Granate
{
    public class GranateView : MonoBehaviour
    {
        [SerializeField] private Granate _granate;
        [SerializeField] private ParticleSystem _explodeParticle;
        //[SerializeField] private AudioClip _explodeSound;

        private void Start()
        {
            _granate.OnExplode += OnExplodeVisual;
        }

        private void OnExplodeVisual()
        {
            _explodeParticle.Play();
        }

        private void OnDestroy()
        {
            _granate.OnExplode -= OnExplodeVisual;
        }
    }
}