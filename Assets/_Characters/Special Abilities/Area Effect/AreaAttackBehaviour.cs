using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;
using RPG.Core;
using System;

public class AreaAttackBehaviour : AbilityBehaviour
{   

    public override void Use(GameObject target)
    {
        DealRadialDamage();    
        PlayParticleEffect();
        PlayAbilitySound();
    }

    private void DealRadialDamage()
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
            var damageable = hit.collider.gameObject.GetComponent<HealthSystem>();
            bool hitPlayer = hit.collider.gameObject.GetComponent<PlayerMovement>();
            if (damageable != null && !hitPlayer)
            {
                float damageToDeal = (config as AreaEffectConfig).GetDamageToEachTarget(); //TODO ok rick?
                damageable.TakeDamage(damageToDeal);
            }
        }
    }
}
