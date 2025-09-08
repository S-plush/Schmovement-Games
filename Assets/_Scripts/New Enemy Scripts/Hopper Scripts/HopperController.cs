using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HopperController : Hopper
{
    private bool isGrounded;

    [SerializeField] private float forwardVelocity;
    [SerializeField] private float upwardVelocity;


    private void Start() {
        facePlayer();
        isGrounded = true;
    }
    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Ground") {
            isGrounded = true;
        }
    }

}
