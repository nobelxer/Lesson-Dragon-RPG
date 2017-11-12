using UnityEngine;
using RPG.Characters;

public class AreaAttackBehaviour : AbilityBehaviour
{   
    public override void Use(GameObject target)
    {
        DealRadialDamage();    
        PlayParticleEffect();
        PlayAbilityAnimation();
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
            bool hitPlayer = hit.collider.gameObject.GetComponent<PlayerControl>();
            if (damageable != null && !hitPlayer)
            {
                float damageToDeal = (config as AreaEffectConfig).GetDamageToEachTarget();
                damageable.TakeDamage(damageToDeal);              
            }
        }
    }
}
