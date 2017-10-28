﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Speical Ability/Power Attack"))]
    public class PowerAttackConfig : SpecialAbilityConfig
    {
        [Header("Power Attack Specific")]
        [SerializeField]
        float extraDamage = 10f;

        public override ISpacialAbility AddCopmonent(GameObject gameObjectToAttachTo)
        {
            var behaviourComponent = gameObjectToAttachTo.AddComponent<PowerAttackBehaviour>();
            behaviourComponent.SetConfig(this);
            return behaviourComponent;
        }
    }
}
