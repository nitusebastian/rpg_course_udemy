using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        
        private GameObject player;
        private Fighter fighter;
        private Health health;
        private Mover mover;
        
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 5f;

        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [Range(0,1)]
        [SerializeField] private float patrolSpeedFraction = 0.2f;
        
        private Vector3 guardPosition;
        private float timeSinceLastSawPlayer = Mathf.Infinity;

        private int currentWaypointIndex = 0;
        
        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();

            guardPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if(health.IsDead()) return;
            
            
            if (InAttackRangeOfPlayer()  && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
            
            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextWaypoint = guardPosition;

            if (patrolPath)
            {
                if (AtWaypoint())
                {
                    CycleWaypoint();
                }

                nextWaypoint = GetCurrentWaypoint();
            }
            
            mover.StartMoveAction(nextWaypoint, patrolSpeedFraction);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex =  patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
           return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            fighter.Attack(player);
        }

        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer < chaseDistance;
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }

}