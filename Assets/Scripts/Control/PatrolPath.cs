using System;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            const float waypointGizmoRadius = 0.3f;
            
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(GetWaypoint(i), waypointGizmoRadius);

                
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(GetNextIndex(i)));
            }
        }

        public int GetNextIndex(int i)
        {
            if (i + 1 == transform.childCount)
            {
                return 0;
            }
            return i + 1;
        }

        public Vector3 GetWaypoint(int i)
        {
            return i < transform.childCount? transform.GetChild(i).position : transform.GetChild(0).position;
        }
    }
}