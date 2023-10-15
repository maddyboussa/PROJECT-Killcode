using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Killcode.Events;
using UnityEngine.Rendering;

namespace Killcode.Player
{
    public class CheckBounds : MonoBehaviour
    {
        #region FIELDS
        public GameEvent onBoundsChanged;
        private Camera mainCam;
        private float camHeight;
        private float camWidth;

        BoxCollider2D pBoxCollider;
        [SerializeField] Rect currentBounds;
        Vector2 playerPosition;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            mainCam = Camera.main;
            pBoxCollider = GetComponent<BoxCollider2D>();
            currentBounds = CalculateViewportRectangleMain();
            playerPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            // Set player position
            playerPosition = transform.position;

            // Contain the player position within the screen
            ContainWithinScreen();

            // Update the transform position
            transform.position = playerPosition;
        }

        /// <summary>
        /// Calculat the current viewport
        /// </summary>
        /// <returns>A rectangle that specifies the viewport bounds</returns>
        private Rect CalculateViewportRectangleMain()
        {
            float orthographicSize = mainCam.orthographicSize;
            float aspectRatio = mainCam.aspect;

            // Set camHeight and camWidth
            camHeight = orthographicSize * 2;
            camWidth = camHeight * aspectRatio;

            // Set rectangle
            Rect viewportRect = new Rect(
                mainCam.transform.position.x - camWidth / 2,
                mainCam.transform.position.y - camHeight / 2,
                camWidth,
                camHeight);

            // Let the game know the bounds have changed
            onBoundsChanged.Raise(this, viewportRect);

            return viewportRect;
        }

        /// <summary>
        /// Contain the player within the screen
        /// </summary>
        private void ContainWithinScreen()
        {
            playerPosition.x = Mathf.Clamp(playerPosition.x, currentBounds.xMin + (pBoxCollider.size.x / 2), currentBounds.xMax - (pBoxCollider.size.x / 2));
            playerPosition.y = Mathf.Clamp(playerPosition.y, currentBounds.yMin + (pBoxCollider.size.y / 2), currentBounds.yMax - (pBoxCollider.size.y / 2));
        }

        /// <summary>
        /// Update all bounds
        /// </summary>
        /// <param name="sender">The reference to the sender</param>
        /// <param name="data">The data to retrieve</param>
        public void UpdateBounds(Component sender, object data)
        {
            onBoundsChanged.Raise(this, currentBounds);
        }
    }
}
