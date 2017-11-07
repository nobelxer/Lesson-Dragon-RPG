using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : AbilityBehaviour
    {     
        public override void Use(AbilityUseParams useParams)
        {
            DealDamage(useParams);     
            PlayParticleEffect();
            PlayAbilitySound();
        }
  
        private void DealDamage(AbilityUseParams useParams)
        {
            float damgeToDeal = useParams.baseDamage + (config as PowerAttackConfig).GetExtraDamage();
            useParams.target.TakeDamage(damgeToDeal);
        }
    }
}
