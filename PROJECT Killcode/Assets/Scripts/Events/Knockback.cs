using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private float strength = 10;
    [SerializeField]
    private float delay = 1.0f;

    public void PlayKnockback(Component sender, object data)
    {
        //Debug.Log(data);

        // check if sender is damaging this specific entity
        if (data.Equals(gameObject))
        {
            Debug.Log("kb");
            // ensure reset doesnt happen too soon
            StopAllCoroutines();

            // set direction relative to what damaged entity
            Vector2 direction = (transform.position - sender.transform.position).normalized;

            // add knockback force and start cooldown
            rb.AddForce(direction * strength, ForceMode2D.Impulse);
            StartCoroutine(Reset());
        }
        
    }

    // resets knocked back entity after cooldown
    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector3.zero;
    }
    
}
