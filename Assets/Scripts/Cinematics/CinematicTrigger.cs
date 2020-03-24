using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RGP.Cinematics
{
    
    
    public class CinematicTrigger
        : MonoBehaviour
    {
        private bool triggeredOnce = false;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!triggeredOnce && other.gameObject.tag == "Player")
            {
                GetComponent<PlayableDirector>().Play();
                triggeredOnce = true;
            }
        }
    }
}
