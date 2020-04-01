using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using UnityEngine;

namespace RPG.Comb
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        
        [SerializeField] private GameObject equipedWeaponPrefab = null;
        [SerializeField] private AnimatorOverrideController animatorOverride = null;
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private float damage = 30;
        [SerializeField] private bool isRightHanded = true;
        [SerializeField] private Projectile projectile = null;
        
        public void Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator)
        {
            if (equipedWeaponPrefab != null)
            {
                Transform handTransform = GetTransform(rightHandTransform, leftHandTransform);
                Instantiate(equipedWeaponPrefab, handTransform);
            }

            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }

        private Transform GetTransform(Transform rightHandTransform, Transform leftHandTransform)
        {
            Transform handTransform;

            if (isRightHanded)
            {
                handTransform = rightHandTransform;
            }
            else
            {
                handTransform = leftHandTransform;
            }

            return handTransform;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }
        
        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile projectileInstance = Instantiate(projectile, 
                                                         GetTransform(rightHand, leftHand).position,
                                                         Quaternion.identity);

            projectileInstance.SetTaget(target, damage);
        }

        public float GetRange()
        {
            return weaponRange;
        } 
        
        public float GetDamage()
        {
            return damage;
        }

        public float GetTimeBetweenAttacks()
        {
            return timeBetweenAttacks;
        }
    }
}


