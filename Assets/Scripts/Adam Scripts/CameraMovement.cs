using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public PlayerMovement _playerMovement;

    float xRotate = 0f;
    float yRotate = 0f;
    public float camPivotLock = 60f;
    public float camAllowance = 1f;

    // moved to playerMovement for easy adjusting in inspector
    //public float camSensitivity = 200f;

    public GameObject player;

    private void Start()
    {
        //hides cursor and locks it within the game window
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        /*if (transform.localEulerAngles.x - camPivotLock > camAllowance)
        {
            yRotate = -camPivotLock;
        }
        else if (transform.localEulerAngles.x - camPivotLock > camAllowance)
        {
            yRotate = camPivotLock;
        }*/

        xRotate += Input.GetAxis("Mouse X") * _playerMovement.camSensitivity * Time.deltaTime;
        yRotate += -Input.GetAxis("Mouse Y") * _playerMovement.camSensitivity * Time.deltaTime;

        // Move camera up/down (rotate around x axis) with mouse y
        // and rotate player left/right (rotate around y axis) with mouse x
        Debug.Log(transform.localEulerAngles.x);
        transform.localEulerAngles = new Vector3(yRotate, 0f, 0f);
        player.transform.localEulerAngles = new Vector3(0f, xRotate, 0f);
    }
}
