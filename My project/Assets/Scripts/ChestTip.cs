using TMPro;
using UnityEngine;

public class ChestTip : MonoBehaviour
{
    public string[] tip = {
        "Congrats",
        "Use 'W', 'A' to move"
    };
    private int index;
    public GameObject chestTip;
    public TextMeshProUGUI tipText;
    
    // Start is called before the first frame update
    void Start()
    {
        // Hide at start
        chestTip.SetActive(false);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            chestTip.SetActive(true);
            index = 0;
            tipText.text = tip[index];
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            chestTip.SetActive(false);
        }
    }
}
