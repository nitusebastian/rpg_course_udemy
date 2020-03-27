using System;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter: MonoBehaviour, IAction
    {
        private Health target;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private GameObject weaponPrefab = null;
        [SerializeField] private Transform handTransform = null;
        [SerializeField] private AnimatorOverrideController weaponOverride = null;
        
        private float timeSinceLastAttack = Mathf.Infinity;
        [SerializeField] private float damage = 30;

        public void Start()
        {
            SpawnWeapon();
        }

        public void Update()
        {
            timeSinceLastAttack += Time.deltaTime; 
            
            if(target == null) return;
            if (target.IsDead()) return;
            
            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }
        
        public void SpawnWeapon()
        {
            Instantiate(weaponPrefab, handTransform);
            Animator animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = weaponOverride;
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform.position);
            
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                // This will trigger the Hit() event (eventually)
                TriggerAttack();
                timeSinceLastAttack = 0f;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        public bool CanAttack(GameObject target)
        {
            if (target == null) return false;
            
            Health targetToTest = target.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        //Animation Event
        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Hit()
        {
            if (target == null) return;
            
            target.TakeDamage(damage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(target.transform.position , transform.position) <= weaponRange;
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel(); 
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

       
    }
}