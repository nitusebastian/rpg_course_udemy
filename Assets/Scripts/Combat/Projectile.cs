using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;

        private Health target = null;
        private float damage = 0;
        
        public void SetTaget(Health target, float damage)
        {
            this.target = target;
            this.damage = damage;
        }

        private void Update()
        {
            if (target == null) return;
            
            transform.LookAt(GetAimPosition());
            transform.Translate(transform.TransformDirection(Vector3.forward) * speed * Time.deltaTime, Space.World);
        }

        private Vector3 GetAimPosition()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return (target.transform.position + Vector3.up * targetCapsule.height / 2);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}