using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Movement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    // public Animator animator;

    Vector2 movement;

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal2");
        movement.y = Input.GetAxisRaw("Vertical2");

        // animator.SetFloat("Horizontal", movement.x);
        // animator.SetFloat("Vertical", movement.y);
        // animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
