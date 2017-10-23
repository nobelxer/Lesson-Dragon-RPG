﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//consider re-wire
using RPG.Core;

namespace RPG.Weapons
{
    public class Projectile : MonoBehaviour
    {

        [SerializeField] float projectileSpeed;
        [SerializeField] GameObject shooter; // so can inspect when paused

        const float DESTROY_DELAY = 0.01f;
        float damageCaused;

        public void SetShooter(GameObject shooter)
        {
            this.shooter = shooter;
        }

        public void SetDamage(float damage)
        {
            damageCaused = damage;
        }

        public float GetDefaultLaunchSpeed()
        {
            return projectileSpeed;
        }

        void OnCollisionEnter(Collision collision)
        {
            var layerCollideWith = collision.gameObject.layer;
            if (shooter && layerCollideWith != shooter.layer)
            {
                DamageIfDamageable(collision);
            }

        }

        private void DamageIfDamageable(Collision collision)
        {
            Component damageableComponent = collision.gameObject.GetComponent(typeof(IDamageable));
            print("damageableComponent = " + damageableComponent);

            if (damageableComponent)
            {
                (damageableComponent as IDamageable).TakeDamage(damageCaused);
            }
            Destroy(gameObject, DESTROY_DELAY);
        }
    }
}