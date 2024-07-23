using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuList : MonoBehaviour
{
    public GameObject menuList;
    public PlayerLife playerLife;
    public TextMeshProUGUI difficultyText;

    // want to close menu
    [SerializeField] private bool menuKeys = true;
    [SerializeField] private AudioSource bgmSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (menuKeys)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuList.SetActive(true);
                menuKeys = false;
                // stop time
                Time.timeScale = 0;
                // stop music
                bgmSound.Pause();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuList.SetActive(false);
            menuKeys = true;
            // restore time
            Time.timeScale = 1;
            // restore music
            bgmSound.Play();
        }
    }


    // Return game
    public void Return()
    {
        menuList.SetActive(false);
        menuKeys = true;
        // restore time
        Time.timeScale = (1);
        // restore music
        bgmSound.Play();
    }


    // Restart game
    public void Restart()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;

    }


    // Exit game
    public void Exit()
    {
        Application.Quit();
    }

    public void Difficulty()
    {
        playerLife.difficulty = playerLife.difficulty switch
        {
            "Normal" => "Easy",
            "Easy" => "Hard",
            "Hard" => "Normal",
            _ => playerLife.difficulty
        };
        playerLife.damage = playerLife.difficultyDict[playerLife.difficulty];
        difficultyText.SetText(playerLife.difficulty);

        float enemyHealth = playerLife.difficulty switch
        {
            "Easy" => 10f,
            "Hard" => 100f,
            _ => 30f
        };
        EnemyManager.Instance.UpdateEnemiesHealth(enemyHealth);
    }

}
