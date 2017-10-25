using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;
using RPG.CameraUI; //TODO consider rewiring

namespace RPG.Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AICharacterControl))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class PlayerMovement : MonoBehaviour
    {

        ThirdPersonCharacter thirPersonCharacter = null;   // A reference to the ThirdPersonCharacter on the object
        CameraRaycaster cameraRaycaster = null;
        Vector3 clickPoint;
        AICharacterControl aiCharacterControl = null;
        GameObject walkTarget = null;

        // TODO solve fight between serialize and const
        [SerializeField] const int walkableLayerNumber = 8;
        [SerializeField] const int enemyLayerNumber = 9;

        private void Start()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            thirPersonCharacter = GetComponent<ThirdPersonCharacter>();
            aiCharacterControl = GetComponent<AICharacterControl>();
            walkTarget = new GameObject("walkTarget");

            cameraRaycaster.notifyMouseClickObservers += ProcessMouseClick;
        }

        void ProcessMouseClick(RaycastHit raycastHit, int layerHit)
        {
            switch (layerHit)
            {
                case enemyLayerNumber:
                    //navigate to enemy
                    GameObject enemy = raycastHit.collider.gameObject;
                    aiCharacterControl.SetTarget(enemy.transform);
                    break;
                case walkableLayerNumber:
                    // navigate to point on the ground
                    walkTarget.transform.position = raycastHit.point;
                    aiCharacterControl.SetTarget(walkTarget.transform);
                    break;
                default:
                    Debug.LogWarning("Dont know how to handle mouse click for player movement");
                    return;
            }
        }

        //TODO make this called again
        private void ProcessDirectMovement()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 movement = v * cameraForward + h * Camera.main.transform.right;

            thirPersonCharacter.Move(movement, false, false);

        }

    }
}

