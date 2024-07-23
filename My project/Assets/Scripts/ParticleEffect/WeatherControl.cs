using UnityEngine;

public class WeatherControl : MonoBehaviour
{
    public ParticleSystem snowParticleSystem;
    public ParticleSystem rainParticleSystem;
    public Transform player;

    void Start()
    {
        // Getting the transform component of the player and the particle systems for rain and snow
        player = GameObject.FindWithTag("Player").transform;
        rainParticleSystem = GameObject.Find("Rain").GetComponent<ParticleSystem>();
        snowParticleSystem = GameObject.Find("Snow").GetComponent<ParticleSystem>();
    }

    void Update()
    {
        // Defining the areas where rain and snow effects should play
        Rect rainZone = new Rect(-70, -44, 60, 38); // parameters are x, y, width, height
        Rect snowZone = new Rect(-12.4f, 0, 55.4f, 12); // parameters are x, y, width, height

        Vector2 playerPosition = new Vector2(player.position.x, player.position.y);

        // Checking if the player is within the rain zone
        if (rainZone.Contains(playerPosition))
        {
            //Debug.Log("rainZone");
            if (!rainParticleSystem.isPlaying)
            {
                // If the player is within the rain zone and the rain particle system is not playing, start it
                rainParticleSystem.Play();
            }
        }
        else
        {
            //Debug.Log("Out of rainZone");
            if (rainParticleSystem.isPlaying)
            {
                // If the player is outside the rain zone and the rain particle system is playing, stop it
                rainParticleSystem.Stop();
            }
        }

        // Checking if the player is within the snow zone
        if (snowZone.Contains(playerPosition))
        {
            if (!snowParticleSystem.isPlaying)
            {
                // If the player is within the snow zone and the snow particle system is not playing, start it
                snowParticleSystem.Play();
            }
        }
        else
        {
            if (snowParticleSystem.isPlaying)
            {
                // If the player is outside the snow zone and the snow particle system is playing, stop it
                snowParticleSystem.Stop();
            }
        }
    }
}
