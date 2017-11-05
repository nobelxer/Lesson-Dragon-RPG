using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : AbilityBehaviour
    {
        PowerAttackConfig config;
        AudioSource audioSource = null;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void SetConfig(PowerAttackConfig configToSet)
        {
            this.config = configToSet;
        }

        public override void Use(AbilityUseParams useParams)
        {
            DealDamage(useParams);
            audioSource.clip = config.GetAudioClip();
            PlayParticleEffect();
            audioSource.Play();
        }

        private void PlayParticleEffect()
        {
            var particlePrefab = config.GetParticlePrefab();
            var prefab = Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation);
            //TODO decide on positioning
            ParticleSystem myParticleSystem = prefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(prefab, myParticleSystem.main.duration);
        }
        private void DealDamage(AbilityUseParams useParams)
        {
            float damgeToDeal = useParams.baseDamage + config.GetExtraDamage();
            useParams.target.TakeDamage(damgeToDeal);
        }
    }
}
