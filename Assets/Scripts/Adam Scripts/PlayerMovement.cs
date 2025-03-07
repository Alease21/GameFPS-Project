using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CameraMovement _cameraMovement;

    Rigidbody rb;
    Animator _animator;

    public float playerSpeed;
    public float playerJumpFactor;
    public float playerDashFactor;
    public float camSensitivity;
    public float dashCoolDown = 2f;
    private float dashVerticalHold = .25f;

    public bool isJumping = false;
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

        // Movement based on the direction the player is facing &
        // update animation controller to match movement direction
        Vector3 offset = vertical * transform.forward + horizontal * transform.right;       
        transform.position += offset * Time.deltaTime * playerSpeed;

        _animator.SetFloat("Horizontal", horizontal);
        _animator.SetFloat("Vertical", vertical);
        
        // Jump input, no double jump
        if (Input.GetKeyDown("space") && !isJumping)
        {
            rb.AddForce(Vector3.up * playerJumpFactor);
        }

        // Dash in the direction player is currently moving or
        // if no direction keys are pressed, then dash forward
        if (Input.GetKeyDown(KeyCode.LeftShift) && !hasDashed)
        {
            hasDashed = true;
            StartCoroutine(DashCoolDown());

            if ((!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.UpArrow)) &&
                (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.LeftArrow)) &&
                (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.DownArrow)) &&
                (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.RightArrow)))
            {
                rb.AddForce(transform.forward.normalized * playerDashFactor);
            }
            else
            {
                rb.AddForce(offset.normalized * playerDashFactor);
            }
        }
    }

    // Jump bool changes based on ground collision
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            isJumping = false;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            isJumping = true;
        }
    }

    // Coroutine to freeze Y position of player for short time after dash,
    // and then reenable dash after a cooldown
    public IEnumerator DashCoolDown()
    {
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

        yield return new WaitForSecondsRealtime(dashVerticalHold);
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        yield return new WaitForSecondsRealtime(dashCoolDown - dashVerticalHold);
        hasDashed = false;
        Debug.Log("dash can be used again");
    }
}
