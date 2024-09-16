using UnityEngine;
namespace Reggborn.Player
{
    [RequireComponent(typeof(ParticleSystem))]
    public sealed class AuraParticles : MonoBehaviour
    {
        [SerializeField] private AnimationCurve influenceOverLifetime;
        [SerializeField] private ParticleSystem pSystem;
        private ParticleSystem.Particle[] _particlesArray = new ParticleSystem.Particle[0];
        private int _particleCount;

        private Vector3 _lastPos;
        private Vector3 _dir;

        private void Update()
        {
            Initialize();

            _particleCount = pSystem.GetParticles(_particlesArray);
            _dir = transform.position - _lastPos;
            for (int i = 0; i < _particleCount; i++)
            {
                _particlesArray[i].position = Vector3.Lerp(
                    _particlesArray[i].position,
                    _dir + _particlesArray[i].position,
                    influenceOverLifetime.Evaluate(1 - (_particlesArray[i].remainingLifetime / _particlesArray[i].startLifetime)));
            }
            pSystem.SetParticles(_particlesArray);
            _lastPos = transform.position;
        }

        private void Initialize()
        {
            if (!pSystem)
                pSystem = GetComponent<ParticleSystem>();

            if (!pSystem || _particlesArray.Length < pSystem.main.maxParticles)
                _particlesArray = new ParticleSystem.Particle[pSystem.main.maxParticles];
        }

    }
}