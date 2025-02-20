using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    float xRotate = 0f;
    float yRotate = 0f;

    float camSensitivity = 200f;

    public GameObject player;

    private void Start()
    {
        //hides cursor and locks it within the game window
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        xRotate += Input.GetAxis("Mouse X") * camSensitivity * Time.deltaTime;
        yRotate += -Input.GetAxis("Mouse Y") * camSensitivity * Time.deltaTime;

        // Move camera up/down (rotate around x axis) with mouse y
        // and rotate player left/right (rotate around y axis) with mouse x
        transform.localEulerAngles = new Vector3(yRotate, 0f, 0f);
        player.transform.localEulerAngles = new Vector3(0f, xRotate, 0f);
    }
}
