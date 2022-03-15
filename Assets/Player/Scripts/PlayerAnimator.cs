using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

    private PlayerController playerController;
    private Animator animator;
    private Rigidbody rb;

    void Start() {
        playerController = GetComponent<PlayerController>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }


    void Update() {

        if (playerController.doJump) {
            animator.SetTrigger("Jump");
        }

        animator.SetFloat("Acceleration", playerController.acceleration);
        animator.SetBool("OnGround", playerController.onGround);
    }
}
