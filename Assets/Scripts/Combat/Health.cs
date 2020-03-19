using UnityEngine;

namespace RPG.Combat
{
    public class Health: MonoBehaviour
    {
        [SerializeField] private float health = 100f;
        private bool isDead;

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
            

            if (health == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (isDead) return;
            
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            Destroy(GetComponent<CapsuleCollider>());
        }
    }
}