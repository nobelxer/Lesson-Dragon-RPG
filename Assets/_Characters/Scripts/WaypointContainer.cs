using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Characters
{
    public class WaypointContainer : MonoBehaviour
    {
        float gizmoRadius = .2f;
                
        void OnDrawGizmos()
        {
            Vector3 firstPosition = transform.GetChild(0).position;
            Vector3 previousPosition = firstPosition;

            foreach (Transform waypoint in transform) // for each tranform XXX in transform - each child
            {
                Gizmos.DrawSphere(waypoint.position, gizmoRadius);
                Gizmos.DrawLine(previousPosition, waypoint.position);
                previousPosition = waypoint.position;
            }
            Gizmos.DrawLine(previousPosition, firstPosition);
        }
       
    }
}
