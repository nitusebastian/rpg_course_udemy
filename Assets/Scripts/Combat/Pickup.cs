using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Comb;
using UnityEngine;

namespace RPG.Combat
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                Destroy(gameObject);
            }
        }
    }
}
