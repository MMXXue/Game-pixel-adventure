using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TransformTilemap : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject player;
    private UnityEngine.Rendering.Universal.Light2D playerLight;
    public Color dayColor = Color.white;
    public Color nightColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    public GameObject objectToActivate;
    private UnityEngine.Rendering.Universal.Light2D Light;
    public GameObject lightBar;

    void Start()
    {
        lightBar.SetActive(false);
        if (tilemap == null)
        {
            tilemap = GetComponent<Tilemap>();
        }
        if (player != null)
        {
            playerLight = player.GetComponentInChildren<UnityEngine.Rendering.Universal.Light2D>();
        }
        if (objectToActivate != null)
        {
            Light = objectToActivate.GetComponentInChildren<UnityEngine.Rendering.Universal.Light2D>();
        }

        tilemap.color = dayColor;
    }

    void Update()
    {
        Vector2 playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);

        if (playerPosition.x >= 68 && playerPosition.x <= 93 && playerPosition.y >= -2.4f && playerPosition.y <= 22)
        {
            // If the player is within the night zone, set the tilemap color to nightColor and enable the player light
            tilemap.color = nightColor;
            lightBar.SetActive(true);
            if (playerLight != null)
            {
                playerLight.enabled = true;
            }

            // Activate the GameObject if it exists
            if (Light != null)
            {
                Light.enabled = true;
            }
        }
        else
        {
            // If the player is outside the night zone, set the tilemap color to dayColor and disable the player light
            tilemap.color = dayColor;
            lightBar.SetActive(false);
            if (playerLight != null)
            {
                playerLight.enabled = false;
            }

            // Deactivate the GameObject if it exists
            if (Light != null)
            {
                Light.enabled = false;
            }
        }
    }
}