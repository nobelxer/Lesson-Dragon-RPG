using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{

    [RequireComponent(typeof(WeaponSystem))]
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(HealthSystem))]

    public class EnemyAI : MonoBehaviour
    {       
        [SerializeField] float chaseRadius = 4f;
        [SerializeField] WaypointContainer patrolPath;
        [SerializeField] float wayPointTolerance = 2f;
        [SerializeField] float waypointWaitingTime = 1f;

        PlayerControl player;
        Character character;
        float currentWeaponRange;
        float distanceToPlayer;
        int nextWayPointIndex;


        enum State { idle, patrolling, attacking, chasing }
        State state = State.idle;

        void Start()
        {
            player = FindObjectOfType<PlayerControl>();
            character = GetComponent<Character>();
        }

        void Update()
        {
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();

            bool inAttackRange = distanceToPlayer <= currentWeaponRange;
            bool inChaseRange = distanceToPlayer > currentWeaponRange && distanceToPlayer <= chaseRadius;
            bool outsideChaseRange = distanceToPlayer > chaseRadius;

            if (outsideChaseRange)
            {
                StopAllCoroutines();
                weaponSystem.StopAttacking();
                StartCoroutine(Patrol());
            }
            if (inChaseRange)
            {
                StopAllCoroutines();
                weaponSystem.StopAttacking();
                StartCoroutine(ChasePlayer());
            }
            if (inAttackRange)
            {
                StopAllCoroutines();               
                weaponSystem.AttackTarget(player.gameObject);
            }           
        } 

        IEnumerator Patrol()
        {
            state = State.patrolling;
            while(true){
                Vector3 nextWaypointPosition = patrolPath.transform.GetChild(nextWayPointIndex).position;
                character.SetDestination(nextWaypointPosition);
                CycleWaypointWhenClose(nextWaypointPosition);
                yield return new WaitForSeconds(waypointWaitingTime);
            }           
        }

        private void CycleWaypointWhenClose(Vector3 nextWaypointPosition)
        {
           if(Vector3.Distance(transform.position, nextWaypointPosition) <= wayPointTolerance)
            {
                nextWayPointIndex = (nextWayPointIndex + 1) % patrolPath.transform.childCount;
                print(nextWayPointIndex);
            }
        }

        IEnumerator ChasePlayer()
        {
            state = State.chasing;
            while (distanceToPlayer >= currentWeaponRange)
            {
                character.SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = new Color(255f, 0f, 0f, .5f);
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);

            Gizmos.color = new Color(0.5f, 0f, 255f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}
