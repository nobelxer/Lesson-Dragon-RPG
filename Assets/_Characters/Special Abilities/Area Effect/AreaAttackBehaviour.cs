﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;
using RPG.Core;
using System;

public class AreaAttackBehaviour : AbilityBehaviour
{   

    public override void Use(AbilityUseParams useParams)
    {
        DealRadialDamage(useParams);    
        PlayParticleEffect();
        PlayAbilitySound();
    }

    private void DealRadialDamage(AbilityUseParams useParams)
    {    
        RaycastHit[] hits = Physics.SphereCastAll

            (
            transform.position,
            (config as AreaEffectConfig).GetRadius(),
            Vector3.up,
            (config as AreaEffectConfig).GetRadius()
            );

        foreach (RaycastHit hit in hits)
        {
            var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
            bool hitPlayer = hit.collider.gameObject.GetComponent<Player>();
            if (damageable != null && !hitPlayer)
            {
                float damageToDeal = useParams.baseDamage + (config as AreaEffectConfig).GetDamageToEachTarget(); //TODO ok rick?
                damageable.TakeDamage(damageToDeal);
            }
        }
    }
}
