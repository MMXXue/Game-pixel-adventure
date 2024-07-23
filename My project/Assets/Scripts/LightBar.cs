using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;

public class LightBar : MonoBehaviour
{
    public GameObject player; // Reference to the player GameObject
    private Light2D playerLight; // Reference to the Light2D component
    public float maxLight = 200;
    private float currentLight;
    public PlayerLife playerLife;
    public Slider lightBarSlider;
    public GameObject[] specialObjects; // Array of special GameObjects
    public float lightIncreaseRate = 10f; // Light increase rate
    public float proximityDistance = 3f; // Distance for player proximity

    void Start()
    {
        if (player != null)
        {
            playerLight = player.GetComponentInChildren<Light2D>();
        }
        currentLight = maxLight;
        UpdateLightBar();
    }

    void Update()
    {
        if (playerLight.enabled)
        {
            bool isPlayerNearSpecialObject = IsPlayerNearbySpecialObject();
            if (isPlayerNearSpecialObject)
            {
                IncreaseLight(lightIncreaseRate * Time.deltaTime);
            }
            else
            {
                ReduceLight(10f * Time.deltaTime);
            }
        }
        else
        {
            ReduceLight(-10f * Time.deltaTime);
        }
    }

    bool IsPlayerNearbySpecialObject()
    {
        foreach (GameObject specialObject in specialObjects)
        {
            if (specialObject != null)
            {
                float distance = Vector3.Distance(player.transform.position, specialObject.transform.position);
                if (distance <= proximityDistance)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void ReduceLight(float lightValue)
    {
        currentLight -= lightValue;
        currentLight = Mathf.Clamp(currentLight, 0, maxLight);
        UpdateLightBar();

        if (currentLight <= 0)
        {
            playerLife.Death();
            currentLight = maxLight;
        }
    }

    public void IncreaseLight(float lightValue)
    {
        currentLight += lightValue;
        currentLight = Mathf.Clamp(currentLight, 0, maxLight);
        UpdateLightBar();
    }

    private void UpdateLightBar()
    {
        float lightPercentage = currentLight / maxLight;
        lightBarSlider.value = lightPercentage;
    }

    // Adds a method to set light
    public void SetLight(float lightValue)
    {
        currentLight = Mathf.Clamp(lightValue, 0, maxLight);
        UpdateLightBar();
    }
}