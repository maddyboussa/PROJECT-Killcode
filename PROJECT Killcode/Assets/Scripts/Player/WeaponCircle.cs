using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponCircle : MonoBehaviour
{
    [SerializeField] GameObject weaponSpritePrefab;
    private GameObject activeWeaponSprite;

    // physics variables
    [SerializeField] float circleRadius;
    private Vector2 direction;
    private Vector2 posOnCirlce;

    private Vector2 cursorPosition;
    private Camera mainCam;


    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        cursorPosition = transform.position;

        // instantiate weapon sprite prefab
        activeWeaponSprite = Instantiate(weaponSpritePrefab);
        activeWeaponSprite.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Track mouse posiiton
        cursorPosition = mainCam.ScreenToWorldPoint(new Vector2(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y));

        // calculate direction from player center to mouse position
        direction = (cursorPosition - (Vector2)transform.position).normalized;

        // calculate sprite position on circle
        posOnCirlce = direction * circleRadius;

        // update weapon sprite's location accordingly
        activeWeaponSprite.GetComponent<Transform>().position = (Vector2)transform.position + posOnCirlce;        
    }

    /// <summary>
    /// Will respond to a weapon changed event and update active sprite accordingly
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="data"></param>
    public void ChangeWeaponSprite(Component sender, object data)
    {
        //activeWeaponSprite = ;

        // each weapon item will have a field to pass in its connected sprite object
        // then, wherever the active weapon is changed, it will raise on weapon changed event and pass in its corresponding sprite object
        // this method will recieve sprite object and will update it here
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, circleRadius);

    //    // draw direction
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawRay(transform.position, direction);

    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawRay(transform.position, direction * circleRadius);
    //}
}
