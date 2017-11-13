using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class WeaponConfig : ScriptableObject
    {

        public Transform gripTransform;

        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip atackAnimation;
        [SerializeField] float timeBetweenAnimationCycles = 0.5f;
        [SerializeField] float maxAttackRange = 2f;
        [SerializeField] float additionalDamage = 10f;
        [SerializeField] float damageDelay = 0.5f;

        public float GetTimeBetweenAnimationCycles()
        {           
            return timeBetweenAnimationCycles;
        }

        public float GetMaxAttackRange()
        {            
            return maxAttackRange;
        }

        public float GetDemageDelay()
        {
            return damageDelay;
        }

        public GameObject GetWeaponPrefab()
        {
            return weaponPrefab;
        }

        public AnimationClip GetAttackAnimClip()
        {
            RemoveAnimaitonEvents();
            return atackAnimation;
        }

        public float GetAdditionalDamage()
        {
            return additionalDamage;
        }

        // So that asset packs cannot cause crashes
        private void RemoveAnimaitonEvents()
        {
            atackAnimation.events = new AnimationEvent[0];
        }
    }
}
