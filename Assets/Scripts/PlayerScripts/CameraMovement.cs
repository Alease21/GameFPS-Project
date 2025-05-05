using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //Singleton setup
    public static CameraMovement instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private Camera _mainCam;

    private float _xRotate;
    private float _yRotate;
    public float XRotate { get { return _xRotate; } private set { _xRotate = value; } }
    public float YRotate { get { return _yRotate; } private set { _yRotate = value; } }

    [SerializeField] private float _camPivotMax = 50f;
    [SerializeField] private float _camSensitivity;

    private float shotLerpTimer = .1f;
    bool isRecoiling = false;
    float angleToLerp = 10f; //grab from weaponSO?

    public float CamSensitivity { get { return _camSensitivity; } private set { _camSensitivity = value; } }

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

        if (!isRecoiling)
        {
            // Limit player vertical camera movement to +/- camPivotMax
            _yRotate = Mathf.Clamp(_yRotate, -_camPivotMax, _camPivotMax);
        }

        // Move camera up/down (rotate around x axis) with mouse y
        // and rotate player left/right (rotate around y axis) with mouse x
        _mainCam.transform.localEulerAngles = new Vector3(_yRotate, 0f, 0f);
        transform.localEulerAngles = new Vector3(0f, _xRotate, 0f);
    }
    public IEnumerator GunRecoilCoro(WeaponSO.WeaponType weaponType)
    {
        float ySnapSnot = _yRotate;

        isRecoiling = true;

        switch (weaponType)
        {
            case WeaponSO.WeaponType.HitScan:
            case WeaponSO.WeaponType.Projectile:

                WeaponController.instance.StartCoroutine(WeaponController.instance.WeaponRecoilAnimation(shotLerpTimer));

                for (float timer = 0f; timer < shotLerpTimer; timer += Time.deltaTime)
                {
                    float lerpRatio = timer / shotLerpTimer;

                    _yRotate = Mathf.Lerp(ySnapSnot, ySnapSnot - angleToLerp, lerpRatio);
                    yield return null;
                }
                break;
            case WeaponSO.WeaponType.Continuous:
                // figure out animation for this
                break;
        }
        isRecoiling = false;
    }
    public void OnLoadGameData(float x, float y)
    {
        XRotate = x;
        YRotate = y;
    }
}