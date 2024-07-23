using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Transform player;
    public Transform rain; // Rain particle system's Transform component
    private float distanceAboveCamera = 10f;
    public float smoothing;

    // Start is called before the first frame update
    void Start()
    {
        rain = GameObject.Find("Rain").transform; // 根据Rain的名称查找Rain对象
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            // camera position != player position
            if (transform.position != player.position)
            {
                Vector3 playerPos = player.position;
                transform.position = Vector3.Lerp(transform.position, playerPos, smoothing);
            }
            // make the rain follow the camera (camera follows player)
            rain.position = new Vector3(transform.position.x, transform.position.y + distanceAboveCamera, transform.position.z);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
