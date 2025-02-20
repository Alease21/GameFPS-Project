using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    Animator _animator;

    public float playerSpeed;
    public float playerJumpFactor;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Root motion player movement through animator, allows forward movement
        // based on facing direction
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        _animator.SetFloat("Horizontal", horizontal);
        _animator.SetFloat("Vertical", vertical);

        // Jump input
        if (Input.GetKeyDown("space"))
        {
            // figure this out.
            //    -ground collision?
            //    -falling velocity?

            rb.AddForce(Vector3.up * playerJumpFactor);
        }
    }
}
