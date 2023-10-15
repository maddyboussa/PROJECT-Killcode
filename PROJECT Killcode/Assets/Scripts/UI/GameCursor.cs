using Killcode.Events;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class GameCursor : MonoBehaviour
{
    #region FIELDS
    [SerializeField] private GameEvent onCursorMoved;
    private Camera mainCam;
    private Vector2 cursorDirection;
    private Vector2 cursorVelocity;
    [SerializeField] private Vector2 cursorPosition;
    [SerializeField] private Vector2 previousCursorPosition;
    private Rect currentBounds;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;

        mainCam = Camera.main;
        cursorPosition = transform.position;
        previousCursorPosition = cursorPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // Set the cursor position
        cursorPosition = transform.position;

        // Track mouse posiiton
        cursorPosition = mainCam.ScreenToWorldPoint(new Vector2(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y));

        // Contain the cursor
        ContainCursor();

        // Update the transform position
        transform.position = cursorPosition;

        // if the current cursorPosition does not equal the previous one, then the cursor has moved
        if (cursorPosition != previousCursorPosition)
        {
            // Invoke onCursorMoved
            onCursorMoved.Raise(this, cursorPosition);
        }

        // Update previous cursor position
        previousCursorPosition = cursorPosition;
    }

    /// <summary>
    /// Update the bounds of the cursor
    /// </summary>
    /// <param name="bounds">The bounds to update to</param>
    public void UpdateBounds(Component sender, object data)
    {
        if(data is Rect)
        {
            currentBounds = (Rect) data;
        }
    }

    /// <summary>
    /// Contain the player within the screen
    /// </summary>
    private void ContainCursor()
    {
        cursorPosition.x = Mathf.Clamp(cursorPosition.x, currentBounds.xMin, currentBounds.xMax);
        cursorPosition.y = Mathf.Clamp(cursorPosition.y, currentBounds.yMin, currentBounds.yMax);
    }
}
