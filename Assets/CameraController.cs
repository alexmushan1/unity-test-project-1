using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    GameObject player;
    Camera mainCamera;
    float zoomSpeed = 30.0f;

    float minZoom = 10.0f;

    float maxZoom = 100.0f;

    void Start()
    {
        player = GameObject.Find("Player");
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Zoom input from the mouse scroll wheel
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");

        // Adjust the camera's orthographic size or field of view based on the zoom input
        if (mainCamera.orthographic)
        {
            mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - zoomInput * zoomSpeed, minZoom, maxZoom);
        }
        else
        {
            mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView - zoomInput * zoomSpeed, minZoom, maxZoom);
        }
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }
}
