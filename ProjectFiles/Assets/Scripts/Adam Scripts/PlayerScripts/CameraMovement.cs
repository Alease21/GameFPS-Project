using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public PlayerMovement _playerMovement;
    public GameObject player;

    float xRotate = 0f;
    float yRotate = 0f;
    public float camPivotMax = 50f;

    private void Start()
    {
        //hides cursor and locks it within the game window
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        yRotate += -Input.GetAxis("Mouse Y") * _playerMovement.camSensitivity * Time.deltaTime;
        xRotate += Input.GetAxis("Mouse X") * _playerMovement.camSensitivity * Time.deltaTime;

        // Limit player vertical camera movement to +/- camPivotMax
        yRotate = Mathf.Clamp(yRotate, -camPivotMax, camPivotMax);

        // Move camera up/down (rotate around x axis) with mouse y
        // and rotate player left/right (rotate around y axis) with mouse x
        transform.localEulerAngles = new Vector3(yRotate, 0f, 0f);
        player.transform.localEulerAngles = new Vector3(0f, xRotate, 0f);
    }
}