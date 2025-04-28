using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    private Animator _animator;
    private CapsuleCollider _capsuleCollider;

    public float playerSpeed;
    public float playerJumpFactor;
    public float playerDashFactor;
    [SerializeField] private float _dashCoolDown = 2f;
    [SerializeField] private float _dashVerticalHold = .25f;

    // public gets, protected sets (ask about this?)
    public float DashCoolDown { get { return _dashCoolDown; } protected set { } }
    public float DashVerticalHold { get { return _dashVerticalHold; } protected set { } }

    private bool isJumping = false;
    private bool hasDashed = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
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

        /*

        RaycastHit hit;

        if (Physics.SphereCast(transform.position, _capsuleCollider.radius, -transform.up, out hit, _capsuleCollider.height / 2 - _capsuleCollider.radius))
        {
            isJumping = false;
        }
        else
        {
            isJumping = true;
        }
        */

        // Jump input, no double jump
        if (Input.GetKeyDown("space") && !isJumping)
        {
            _rb.AddForce(Vector3.up * playerJumpFactor);
        }

        // Dash in the direction player is currently moving or
        // if no direction keys are pressed, then dash forward
        if (Input.GetKeyDown(KeyCode.LeftShift) && !hasDashed)
        {
            hasDashed = true;
            StartCoroutine(InventoryController.instance.OnDash());//UI coro for dash CD visual
            StartCoroutine(DashCoolDownCoro());

            if ((!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.UpArrow)) &&
                (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.LeftArrow)) &&
                (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.DownArrow)) &&
                (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.RightArrow)))
            {
                _rb.AddForce(transform.forward.normalized * playerDashFactor);
            }
            else
            {
                _rb.AddForce(offset.normalized * playerDashFactor);
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
        // fix falling on slopes causing isJumping to flip true a lot and not allowing jumps
        if (collision.transform.tag == "Ground")
        {
            isJumping = true;
        }
    }

    // Coroutine to freeze Y position of player for short time after dash,
    // and then reenable dash after a cooldown
    public IEnumerator DashCoolDownCoro()
    {
        _rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

        yield return new WaitForSecondsRealtime(_dashVerticalHold);
        _rb.constraints = RigidbodyConstraints.FreezeRotation;

        yield return new WaitForSecondsRealtime(_dashCoolDown - _dashVerticalHold);
        hasDashed = false;
        Debug.Log("dash can be used again");
    }
}