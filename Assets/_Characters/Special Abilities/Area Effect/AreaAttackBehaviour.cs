﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;
using RPG.Core;
using System;

public class AreaAttackBehaviour : MonoBehaviour, ISpacialAbility {

    AreaEffectConfig config;
    AudioSource audioSource = null;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SetConfig(AreaEffectConfig configToSet)
    {
        this.config = configToSet;
    }

    public void Use(AbilityUseParams useParams)
    {
        DealRadialDamage(useParams);
        audioSource.clip = config.GetAudioClip();
        PlayParticleEffect();
        audioSource.Play();
    }

    private void PlayParticleEffect()
    {
        var prefab = Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity);
        //TODO decide on positioning
        ParticleSystem myParticleSystem = prefab.GetComponent<ParticleSystem>();
        myParticleSystem.Play();
        Destroy(prefab, myParticleSystem.main.duration);
    }

    private void DealRadialDamage(AbilityUseParams useParams)
    {    
        RaycastHit[] hits = Physics.SphereCastAll

            (
            transform.position,
            config.GetRadius(),
            Vector3.up,
            config.GetRadius()
            );

        foreach (RaycastHit hit in hits)
        {
            var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
            bool hitPlayer = hit.collider.gameObject.GetComponent<Player>();
            if (damageable != null && !hitPlayer)
            {
                float damageToDeal = useParams.baseDamage + config.GetDamageToEachTarget(); //TODO ok rick?
                damageable.TakeDamage(damageToDeal);
            }
        }
    }
}
