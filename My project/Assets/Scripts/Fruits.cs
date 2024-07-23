using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Fruits : MonoBehaviour
{
    private int bananas = 0;

    private AudioSource audios;
    [SerializeField] AudioClip collect;

    public TextMeshProUGUI bananaText;

    // Start is called before the first frame update
    void Start()
    {
        audios = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fruits"))
        {
            audios.clip = collect;
            audios.Play();
            // record the number of fruits && destory fruits
            Destroy(collision.gameObject);
            bananas++;
            Debug.Log("Bananas: " + bananas);
            bananaText.text = "Bananas: " + bananas;
        }

    }
}
