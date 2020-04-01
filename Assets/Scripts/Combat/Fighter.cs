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
      
        
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private Weapon defaultWeapon = null;
        
        private Weapon currentWeapon = null;
        
        private float timeSinceLastAttack = Mathf.Infinity;
       

        public void Start()
        {
            EquipWeapon(defaultWeapon);
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
        
        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            currentWeapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform.position);
            
            if (timeSinceLastAttack >= currentWeapon.GetTimeBetweenAttacks())
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

        
        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        //Animation Event
        public void Hit()
        {
            if (target == null) return;

            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target);
            }
            else
            {
                target.TakeDamage(currentWeapon.GetDamage());
            }
        }

        public void Shoot()
        {
            Hit();
        }
        

        private bool GetIsInRange()
        {
            return Vector3.Distance(target.transform.position , transform.position) <= currentWeapon.GetRange();
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