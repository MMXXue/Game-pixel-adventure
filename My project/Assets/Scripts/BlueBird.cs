using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlueBird : MonoBehaviour, IDamageable
{
    public float speed = 10.0f;
    public float trackingRange = 10.0f;
    private Transform player;
    private Rigidbody2D rb;

    public GameObject trophy; // Added trophy references
    public Slider healthBar;
    public Vector3 healthBarOffset = new Vector3(0, 2, 0);

    // Define scope of activity
    private float minX = 40.8f;
    private float maxX = 81.21f;
    private float minY = -49.14f;
    private float maxY = -35.52f;

    // Define the minimum distance from the player
    private float minDistanceX = 2.0f;
    private float minDistanceY = 2.0f;

    public float HP;

    void Start()
    {
        healthBar.gameObject.SetActive(true);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        healthBar.value = HP;
    }

    void Update()
    {
        //float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        //if (distanceToPlayer <= trackingRange)
        //{
        //    Vector2 newPosition = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        //    // Maintain a minimum distance from the player
        //    if (Mathf.Abs(newPosition.x - player.position.x) < minDistanceX)
        //    {
        //        newPosition.x = transform.position.x;
        //    }

        //    if (Mathf.Abs(newPosition.y - player.position.y) < minDistanceY)
        //    {
        //        newPosition.y = transform.position.y;
        //    }

        //    // Limit the enemy's range of movement
        //    newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        //    newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        //    transform.position = newPosition;

        //    // Update the orientation of the bird so that it faces the player, but only flip on the x-axis
        //    float scaleX = transform.localScale.x;
        //    float scaleY = transform.localScale.y;
        //    float scaleZ = transform.localScale.z;

        //    if (player.position.x > transform.position.x && scaleX > 0)
        //    {
        //        transform.localScale = new Vector3(-Mathf.Abs(scaleX), scaleY, scaleZ);
        //    }
        //    else if (player.position.x < transform.position.x && scaleX < 0)
        //    {
        //        transform.localScale = new Vector3(Mathf.Abs(scaleX), scaleY, scaleZ);
        //    }
        //}
        if (IsPlayerInTrackingRange() && IsPlayerInActivityScope())
        {
            Vector2 newPosition = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            // Maintain a minimum distance from the player
            if (Mathf.Abs(newPosition.x - player.position.x) < minDistanceX)
            {
                newPosition.x = transform.position.x;
            }

            if (Mathf.Abs(newPosition.y - player.position.y) < minDistanceY)
            {
                newPosition.y = transform.position.y;
            }

            // Limit the enemy's range of movement
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

            transform.position = newPosition;

            UpdateOrientation();
        }
        if (HP <= 0)
        {
            Destroy(healthBar.gameObject);
            Destroy(gameObject);
            trophy.SetActive(true); // Activate trophy
        }
        UpdateHealthBarPosition();
    }
    
    void UpdateHealthBarPosition()
    {
        if (healthBar)
        {
            healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + healthBarOffset);
        }
    }

    bool IsPlayerInTrackingRange()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        return distanceToPlayer <= trackingRange;
    }

    bool IsPlayerInActivityScope()
    {
        return player.position.x >= minX && player.position.x <= maxX
            && player.position.y >= minY && player.position.y <= maxY;
    }

    void UpdateOrientation()
    {
        float scaleX = transform.localScale.x;
        float scaleY = transform.localScale.y;
        float scaleZ = transform.localScale.z;

        if (player.position.x > transform.position.x && scaleX > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(scaleX), scaleY, scaleZ);
        }
        else if (player.position.x < transform.position.x && scaleX < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(scaleX), scaleY, scaleZ);
        }
    }

    public float GetHP()
    {
        return HP;
    }

    public void RemoveHP(float damage)
    {
        HP -= damage;
        if (healthBar)
        {
            healthBar.value = HP;
        }
        Debug.Log(HP);
    }
}
