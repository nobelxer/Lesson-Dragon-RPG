using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : AbilityBehaviour
    {     
        public override void Use(GameObject target)
        {
            DealDamage(target);    
            PlayParticleEffect();
            PlayAbilitySound();
            PlayAbilityAnimation();
        }
  
        private void DealDamage(GameObject target)
        {
            float damgeToDeal = (config as PowerAttackConfig).GetExtraDamage();
            target.GetComponent<HealthSystem>().TakeDamage(damgeToDeal);         
        }
    }
}
