using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public float bulletDamage = 10.0f;
    public float range = 200f; // Maximum range of bullets
    
    private Rigidbody2D rb;
    private Vector3 startPosition; // The starting position when the bullet was fired
    private Enemy _enemy;
    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        _enemy = GetComponent<Enemy>();

        //startPosition = transform.position; // Record the starting position of the bullet

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)  // Ensure that Rigidbody2D components are present
        {
            Debug.LogError("Rigidbody2D component missing on " + gameObject.name);
            return;
        }

        if (speed == 0)  // Make sure speed has a non-zero value
        {
            Debug.LogWarning("Speed is zero, the bullet won't move.");
        }

        startPosition = transform.position;
    }

    void Update()
    {
        //transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Check the bullet is out of range
        if (Vector3.Distance(startPosition, transform.position) > range)
        {
            Destroy(gameObject); // Destroy a bullet
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {        
        // Reduce health value
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.RemoveHP(bulletDamage);

            Animator animator = other.gameObject.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play("Hit", -1, 0);
            }
            Destroy(gameObject);
        }

        // If the bullet hits the Trap or Platform, destroy the bullet
        if (other.gameObject.CompareTag("Trap") || other.gameObject.CompareTag("Platform"))
        {
            Destroy(gameObject);
        }

        // Destroy the bullet after 2 seconds
        Destroy(gameObject, 2f);
    }
        

    public void SetDirection(bool facingRight)
    {
        // Check if rb is null
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null) return;  
        }

        if (facingRight)
        {
            rb.velocity = Vector2.right * speed;
        }
        else
        {
            rb.velocity = Vector2.left * speed;
        }
    }

}
