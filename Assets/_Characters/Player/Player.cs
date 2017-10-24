using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

//TODO consider rewire...
using RPG.CameraUI; 
using RPG.Core;
using RPG.Weapons;

namespace RPG.Characters
{
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] int enemyLayer = 9;
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float damagePerHit = 5f;
        [SerializeField] Weapon weaponInUse;
        [SerializeField] AnimatorOverrideController animatorOverrideController;

        Animator animator;
        float currentHealthPoints;
        CameraRaycaster cameraRayCaster;
        float lastHitTime = 0f;

        public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

        void Start()
        {
            RegisterForMouseClick();
            SetCurrentMaxHealth();
            PutWeaponInHand();
            SetupRuntimeAnimator();
        }

        public void TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        }

        private void SetCurrentMaxHealth()
        {
            currentHealthPoints = maxHealthPoints;
        } 

        private void SetupRuntimeAnimator()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["DEFAULT ATTACK"] = weaponInUse.GetAttackAnimClip(); //TODO remove parameter
        }

        private void PutWeaponInHand()
        {
            var weaponPrefab = weaponInUse.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            var weapon = Instantiate(weaponPrefab, dominantHand.transform);
            weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
            weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;
        }

        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand foud on player, please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand scripts on player, please remove one");
            return dominantHands[0].gameObject;


        }

        private void RegisterForMouseClick()
        {
            cameraRayCaster = FindObjectOfType<CameraRaycaster>();
            cameraRayCaster.notifyMouseClickObservers += OnMouseClick;
        }

          void OnMouseClick(RaycastHit raycastHit, int layerHit)
        {
            if (layerHit == enemyLayer)
            {
                var enemy = raycastHit.collider.gameObject;

                if (IsTargetInRange(enemy))
                {
                    AttackTarget(enemy);
                }               
            }
        }


        private void AttackTarget(GameObject target)
        {
            var enemyComponenet = target.GetComponent<Enemy>();
            if (Time.time - lastHitTime > weaponInUse.GetMinTimeBetweenHits())
            {
                animator.SetTrigger("Attack"); //TODO make const
                enemyComponenet.TakeDamage(damagePerHit);
                lastHitTime = Time.time;
            }
        }

        private bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponInUse.GetMaxAttackRange();
        }      
    }
}