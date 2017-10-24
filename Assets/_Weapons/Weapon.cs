using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Weapons
{
    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class Weapon : ScriptableObject
    {

        public Transform gripTransform;

        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip atackAnimation;
        [SerializeField] float minTimeBetweenHits = 0.5f;
        [SerializeField] float maxAttackRange = 2f;

        public float GetMinTimeBetweenHits()
        {
            //TODO consider whether we take animation time into account
            return minTimeBetweenHits;
        }

        public float GetMaxAttackRange()
        {
            //TODO consider whether we take animation time into account
            return maxAttackRange;
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

        // So that asset packs cannot cause crashes
        private void RemoveAnimaitonEvents()
        {
            atackAnimation.events = new AnimationEvent[0];
        }
    }
}
