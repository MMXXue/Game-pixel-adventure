using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    private string[] dialogue = {
        "Welcome to Pixel Wold!(Press E to interact)",
        "Use 'A', 'D' to move",
        "and 'space' to jump,",
        "press 'K' to shoot,",
        "press 'ESC' to adjust difficulty",
        "  NOTE: Hard mode is very challenging",
        "The health bar is in your upper right corner",
        " Pick up utilities for yourself and beat the final boss!",
        "Good luck, have fun!"
    };
    private int index;
    private bool playerInRange;
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    
    // Start is called before the first frame update
    void Start()
    {
        // Hide at start
        dialogueBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if in interaction range
        if (playerInRange)
        {
            // Press e to interact
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Check active or not
                if (dialogueBox.activeInHierarchy)
                {
                    if (index < dialogue.Length - 1)
                    {
                        // Next dialogue
                        index++;
                        dialogueText.text = dialogue[index];
                    }
                    else
                    {
                        dialogueBox.SetActive(false);
                    }
                }
                else
                {
                    dialogueBox.SetActive(true);
                    index = 0;
                    dialogueText.text = dialogue[index];
                }
            }
        }
        else if (dialogueBox.activeInHierarchy)
        {
            dialogueBox.SetActive(false);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            dialogueBox.SetActive(true);
            index = 0;
            dialogueText.text = dialogue[index];
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            dialogueBox.SetActive(false);
        }
    }
}
