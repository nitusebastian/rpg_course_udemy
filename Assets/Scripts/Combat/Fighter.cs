using System;
using RPG.Comb;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter: MonoBehaviour, IAction
    {
        private Health target;
      
        
        [SerializeField] private Transform handTransform = null;
        [SerializeField] private Weapon weapon;
        
        private float timeSinceLastAttack = Mathf.Infinity;
       

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
            if(weapon == null) return;
            
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(handTransform, animator);
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform.position);
            
            if (timeSinceLastAttack >= weapon.GetTimeBetweenAttacks())
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
            
            target.TakeDamage(weapon.GetDamage());
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(target.transform.position , transform.position) <= weapon.GetRange();
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