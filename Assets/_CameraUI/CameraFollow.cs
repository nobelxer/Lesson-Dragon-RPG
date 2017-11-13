using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.CameraUI
{
    public class CameraFollow : MonoBehaviour
    {

        GameObject player;
        Vector3 target;
        public float smoothTime = 0.3f;
        private Vector3 velocity = Vector3.zero;

        // Use this for initialization
        void Start()
        {            
            player = GameObject.FindGameObjectWithTag("Player");            
        }

        // Update is called once per frame
        void LateUpdate()
        {
            target = player.transform.position;
            transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
        }     
    }
}