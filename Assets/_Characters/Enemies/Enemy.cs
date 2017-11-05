using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO consider re-wire
using RPG.Core;
using RPG.Weapons;

namespace RPG.Characters
{
    public class Enemy : MonoBehaviour, IDamageable
    {

        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float chaseRadius = 4f;

        [SerializeField] float attackRadius = 2f;
        [SerializeField] float damagePerShot = 9f;
        [SerializeField] float firingPeriodInS = 0.5f;
        [SerializeField] float firingPeriodVariation = 0.2f;
        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projectileSocket;
        [SerializeField] Vector3 aimOffset = new Vector3(0, 1.5f, 0);

        bool isAttacking = false;
        float currentHealthPoints;
        AICharacterControl aiCharacterControl = null;
        Player player = null;

        public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

        public void TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            if (currentHealthPoints <= 0) { Destroy(gameObject); }
        }

        void Start()
        {
            player = FindObjectOfType<Player>();
            aiCharacterControl = GetComponent<AICharacterControl>();
            currentHealthPoints = maxHealthPoints;
        }

        void Update()
        {
            if(player.healthAsPercentage <= Mathf.Epsilon)
            {
                StopAllCoroutines();
                Destroy(this);
            }

            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceToPlayer <= attackRadius && !isAttacking)
            {
                isAttacking = true;
                float randomiseDelay = Random.Range(firingPeriodInS - firingPeriodVariation, firingPeriodInS + firingPeriodVariation); 
                InvokeRepeating("FireProjectile", randomiseDelay, firingPeriodInS); // TODO switch to coroutines
            }
            if (distanceToPlayer > attackRadius)
            {
                isAttacking = false;
                CancelInvoke();
            }

            if (distanceToPlayer <= chaseRadius)
            {
                aiCharacterControl.SetTarget(player.transform);
            }
            else
            {
                aiCharacterControl.SetTarget(transform);
            }
        }

        //TODO separate out Character firing logic
        void FireProjectile()
        {
            GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity); // instantiate projectile
            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
            projectileComponent.SetDamage(damagePerShot); // set the damage
            projectileComponent.SetShooter(gameObject);

            Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
            float projectileSpeed = projectileComponent.GetDefaultLaunchSpeed();
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = new Color(255f, 0f, 0f, .5f);
            Gizmos.DrawWireSphere(transform.position, attackRadius);

            Gizmos.color = new Color(0.5f, 0f, 255f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}
