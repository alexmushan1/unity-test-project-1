using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var player = GameObject.Find("Player");
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }
}
