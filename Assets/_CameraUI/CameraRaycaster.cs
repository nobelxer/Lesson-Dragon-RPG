using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using RPG.Characters; //So we can detect by type

namespace RPG.CameraUI
{
    public class CameraRaycaster : MonoBehaviour
    {
        [SerializeField] Texture2D enemyCursor = null;
        [SerializeField] Texture2D walkCursor = null;
        [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

        const int POTENTIALLY_WALKABLE_LAYER = 8;
        float maxRaycastDepth = 1000f; // Hard coded value

        Rect screenRect;

        // When mouse is over enemy
        public delegate void OnMouseOverEnemy(EnemyAI enemy);
        public event OnMouseOverEnemy onMouseOverEnemy;

        // When mouse is over terrain
        public delegate void OnMouseOverTerrain(Vector3 destination);
        public event OnMouseOverTerrain onMouseOverPotentiallyWalkable;     

        void Update()
        {
            screenRect = new Rect(0, 0, Screen.width, Screen.height);
            // Check if pointer is over an interactable UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // implement UI interaction
            }
            else
            {
                PerformRaycasts();
            }            
        }

        void PerformRaycasts()
        {
            if (screenRect.Contains(Input.mousePosition))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //Specify layer priority
                if (RayCastForEnemy(ray)) { return; }
                if (RayCastForPotentiallyWalkable(ray)) { return; }
            }
        }

        private bool RayCastForEnemy(Ray ray)
        {
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo, maxRaycastDepth);
            var gemeObjectHit = hitInfo.collider.gameObject;
            var enemyHit = gemeObjectHit.GetComponent<EnemyAI>();
            if (enemyHit)
            {
                Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverEnemy(enemyHit);
                return true;
            }
            return false;
        }

        private bool RayCastForPotentiallyWalkable(Ray ray)
        {
            RaycastHit hitInfo;
            LayerMask potentiallyWalkableLayer = 1 << POTENTIALLY_WALKABLE_LAYER;
            bool potentiallyWalkableHit = Physics.Raycast(ray, out hitInfo, maxRaycastDepth, potentiallyWalkableLayer);
            if (potentiallyWalkableHit)
            {
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverPotentiallyWalkable(hitInfo.point);
                return true;
            }
            return false;
        }                  
    }
}