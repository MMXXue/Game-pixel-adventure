using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
// Reload Scene
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    AudioSource audioS;
    [SerializeField] AudioClip death;

    private Animator anim;
    private Rigidbody2D rb;

    public GameObject playerPS;

    private List<Vector3> positions = new List<Vector3>(); // to store positions
    private List<float> timestamps = new List<float>(); // to store timestamps

    public HealthBar healthBar;
    public LightBar lightBar;
    private bool isCharacterInZone = false;
    protected internal float damage;
    protected internal string difficulty;
    protected internal readonly Dictionary<string, float> difficultyDict = new()
    {
        { "Normal", 0.175f},
        { "Easy", 0.1f},
        { "Hard", 0.5f}
    };

    private List<Vector3> respawnPoints = new List<Vector3>();
    private Vector3 lastPassedRespawnPoint;

    // Start is called before the first frame update
    void Start()
    {

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioS = GetComponent<AudioSource>();

        difficulty = "Normal";
        damage = difficultyDict[difficulty];

        respawnPoints.Add(new Vector3(-45, -2, 0));
        respawnPoints.Add(new Vector3(-16, 0, 0));
        respawnPoints.Add(new Vector3(44, 1, 0));
        respawnPoints.Add(new Vector3(87, -14, 0));
        respawnPoints.Add(new Vector3(53, -11, 0));
        respawnPoints.Add(new Vector3(-1, -10, 0));
        respawnPoints.Add(new Vector3(-34, -11, 0));
        respawnPoints.Add(new Vector3(-16, -42, 0));
        respawnPoints.Add(new Vector3(24, -48, 0));
        respawnPoints.Add(new Vector3(38, -40, 0));

        lastPassedRespawnPoint = transform.position;

        StartCoroutine(SavePlayerPosition());

        // If the HealthBar component is referenced correctly, add an event listener to listen for events where blood bars return to 0
        if (healthBar != null)
        {
            healthBar.OnHealthDepleted += Death;
        }
        else
        {
            Debug.LogError("HealthBar component is not assigned!");
        }
    }

    // Coroutine to save player's position at regular intervals
    IEnumerator SavePlayerPosition()
    {
        while (true)
        {
            positions.Add(transform.position);
            timestamps.Add(Time.time);
            yield return new WaitForSeconds(2f);  // adjust the interval as needed
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isCharacterInZone)
        {
            lightBar.ReduceLight(-8f * Time.deltaTime);
        }

        foreach (var point in respawnPoints)
        {
            if (Vector3.Distance(transform.position, point) < 5f)  // minimum distance between the player and the save point
            {
                lastPassedRespawnPoint = point;
                break;  // If the most recent archive point is found, exit the loop
            }
        }
    }

    // touch dieline
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "dieLine":
                audioS.clip = death;
                audioS.Play();
                Death();
                break;
            case "LightSource":
                isCharacterInZone = true;
                break;
        }
    }
    
    // touch trap/enemy
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Trap" || collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "FinalBoss")
        {
            // play animation when player gets hurt or dies
            Animator animator = gameObject.GetComponent<Animator>();
    
            if (animator != null)
            {
                if (healthBar.GetHealth() <= 0)
                {
                    animator.Play("die", -1, 0);

                    //Restart(); // Call Restart directly if health is 0
                }
                else
                {
                    animator.Play("hurt", -1, 0);
                    healthBar.TakeDamage(healthBar.maxHealth * damage);
                }
            }
            //TakeDamage();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Add 10 HP / second when nearby the campfire
        if (other.gameObject.CompareTag("Campfire")) 
        {
            float campfireHealRate = 10f;
            float currentHP = healthBar.GetHealth();
            float HPToAdd = campfireHealRate * Time.deltaTime;
            float maxHP = healthBar.maxHealth;
            float newHP = currentHP + HPToAdd;
            
            // add HP nearby campfires
            if (newHP >= maxHP) 
            {
                healthBar.SetHealth(maxHP);
            } 
            else 
            {
                healthBar.SetHealth(currentHP + HPToAdd);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    { 
        if (other.gameObject.CompareTag("LightSource"))
        {
            isCharacterInZone = false;
        }
    }

    // call player's death animator
    internal void Death()
    {
        anim.SetTrigger("death");
        audioS.clip = death;
        audioS.Play();

        rb.bodyType = RigidbodyType2D.Static;

        if (playerPS != null)  // Check if playerPS is not null before accessing it
        {
            playerPS.gameObject.SetActive(false);  // Deactivate instead of destroying
        }

        //float fiveSecondsAgo = Time.time - 1f;
        float fiveSecondsAgo = Time.time;

        // Find the position from 1 second ago
        for (int i = timestamps.Count - 1; i >= 0; i--)
        {
            if (timestamps[i] <= fiveSecondsAgo)
            {
                //transform.position = positions[i];
                transform.position = lastPassedRespawnPoint;
                break;
            }
        }
    }

    // Reset player's state and position
    private void Restart()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;  // re-enable the Rigidbody2D
        anim.Rebind();  // reset the animator state

        if (playerPS != null)  // Check if playerPS is not null before accessing it
        {
            playerPS.gameObject.SetActive(true);  // Reactivate the playerPS
        }
        else
        {
            Debug.LogError("playerPS is still null upon restart");
        }

        StartCoroutine(SavePlayerPosition());  // restart saving position after player respawns

        // Take damage
        if (healthBar != null)
        {
            // When the player died, restart half health value
            healthBar.SetHealth(healthBar.maxHealth * 1f);
        }
        else
        {
            Debug.LogError("HealthBar component is not assigned!");
        }
    }

    private void OnDestroy()
    {
        // Remove event listeners when the GameObject is destroyed to avoid potential memory leaks
        if (healthBar != null)
        {
            healthBar.OnHealthDepleted -= Death;
        }
    }

}
