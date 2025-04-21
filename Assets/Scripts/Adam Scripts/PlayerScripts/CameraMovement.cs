using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera _mainCam;

    private float _xRotate;
    private float _yRotate;
    [SerializeField] private float _camPivotMax = 50f;
    [SerializeField] private float _camSensitivity;

    // public get, protected set (ask about this?)
    public float CamSensitivity { get { return _camSensitivity; } protected set { } }

    private void Start()
    {
        _mainCam = GetComponentInChildren<Camera>();

        _xRotate = transform.localRotation.eulerAngles.y;

        //hides cursor and locks it within the game window
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        _yRotate += -Input.GetAxis("Mouse Y") * _camSensitivity * Time.deltaTime;
        _xRotate += Input.GetAxis("Mouse X") * _camSensitivity * Time.deltaTime;

        // Limit player vertical camera movement to +/- camPivotMax
        _yRotate = Mathf.Clamp(_yRotate, -_camPivotMax, _camPivotMax);

        // Move camera up/down (rotate around x axis) with mouse y
        // and rotate player left/right (rotate around y axis) with mouse x
        _mainCam.transform.localEulerAngles = new Vector3(_yRotate, 0f, 0f);
        transform.localEulerAngles = new Vector3(0f, _xRotate, 0f);
    }
}