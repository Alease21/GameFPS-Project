using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CameraMovement _cameraMovement;

    Rigidbody rb;
    Animator _animator;

    // make sliders in inspector?
    public float playerSpeed;
    public float playerJumpFactor;
    public float playerDashFactor;
    public float camSensitivity;

    [SerializeField] private bool isJumping = false;
    private bool hasDashed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Movement based on the direction the player is facing
        Vector3 offset = vertical * transform.forward + horizontal * transform.right;       
        transform.position += offset * Time.deltaTime * playerSpeed;

        // Update animation controller to match movement direction
        _animator.SetFloat("Horizontal", horizontal);
        _animator.SetFloat("Vertical", vertical);
        
        // Jump input
        if (Input.GetKeyDown("space") && !isJumping)
        {
            rb.AddForce(Vector3.up * playerJumpFactor);
            isJumping = true;
        }

        // Dash in the direction player is currently moving
        if (Input.GetKeyDown(KeyCode.R) && !hasDashed)
        {
            hasDashed = true;
            StartCoroutine(DashCoolDown());

            // -add friction/drag?
            rb.AddForce(offset * playerDashFactor);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            isJumping = false;
        }
    }

    public IEnumerator DashCoolDown()
    {
        yield return new WaitForSecondsRealtime(3);
        hasDashed = false;
        Debug.Log("dash can be used again");
    }
}
