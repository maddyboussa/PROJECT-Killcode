using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Killcode.Events;
using Killcode.Core;
using Killcode.Weapons;
using System.Linq;

public class PlayerShoot : MonoBehaviour
{
    #region FIELDS
    [SerializeField] private Stats playerStats;
    private Vector2 cursorPosition;
    private bool basicAttacking;

    [Header("Weapons")]
    [SerializeField] private GameEvent onWeaponBasic;
    [SerializeField] private GameEvent onWeaponSpecial;
    [SerializeField] private List<Weapon> availableWeapons = new List<Weapon>();
    [SerializeField] private Weapon currentWeapon;
    #endregion

    private void Start()
    {
        availableWeapons = gameObject.GetComponentsInChildren<Weapon>().ToList();
        currentWeapon = availableWeapons[Random.Range(0, availableWeapons.Count)];
        SetWeapon(this, currentWeapon);
        Debug.Log(currentWeapon);
    }

    private void Update()
    {
        // Check if the player is basic attacking
        if (basicAttacking)
        {
            // Basic Attack with the weapon
            onWeaponBasic.Raise(this, cursorPosition);
        }
    }

    /// <summary>
    /// Update the cursor position according to events
    /// </summary>
    /// <param name="sender">The sender of the event</param>
    /// <param name="data">The data being sent</param>
    public void UpdateCursorPos(Component sender, object data)
    {
        // Check if the incoming data is a Vector2
        if(data is Vector2)
        {
            // Set the cursorPosition to the incoming data
            cursorPosition = (Vector2)data;
        }
    }

    /// <summary>
    /// Fire a bullet
    /// </summary>
    /// <param name="context">The callback context of the input action</param>
    public void OnBasic(InputAction.CallbackContext context)
    {
        // Only trigger once
        if(context.started)
        {
            basicAttacking = true;
        } else if(context.canceled)
        {
            basicAttacking = false;
        }
    }

    public void OnSpecial(InputAction.CallbackContext context)
    {
        // Only trigger once
        if (context.started)
        {
            // Special attack with the weapon
            onWeaponSpecial.Raise(this, cursorPosition);
        }
    }

    public void SetWeapon(Weapon weaponToSet)
    {
        if (availableWeapons.Contains(weaponToSet))
        {
            foreach (Weapon weapon in availableWeapons)
            {
                if (weapon != weaponToSet)
                {
                    weapon.gameObject.SetActive(false);
                }
                else
                {
                    weapon.gameObject.SetActive(true);
                }
            }
        }
    }

    public void SetWeapon(Component sender, object data)
    {
        // Check if the data is the correct type
        if(data is Weapon)
        {
            if(availableWeapons.Contains((Weapon)data))
            {
                foreach(Weapon weapon in availableWeapons)
                {
                    if(weapon != (Weapon)data)
                    {
                        weapon.gameObject.SetActive(false);
                    } else
                    {
                        weapon.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
