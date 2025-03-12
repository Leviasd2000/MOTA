using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class playermovement : MonoBehaviour
{
    public float moveSpeed;

    private Rigidbody2D rb;

    Animator animator;



    // Initial setting
    private void Start() 
    {
        rb = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>();
    }

    // Frequency is not fixed
    private void Update()  
    {
       


    }
    private void FixedUpdate()
    {
        // get player input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        // print
        Debug.Log("moveX:" + moveX + "moveY:" + moveY);
        // 

        // calculate move direction
        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;
        animator.SetFloat("horizontal", moveX);
        animator.SetFloat("vertical", moveY);
        animator.SetFloat("speed", moveDirection.sqrMagnitude);
        
        // move application
        rb.linearVelocity = moveDirection * moveSpeed;
    }
}

