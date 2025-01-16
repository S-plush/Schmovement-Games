using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AlphaMovement : MonoBehaviour
{
    public float alphaMovementSpd = 3.5f;
    public float jumpSpd = 5f;
    public float fallSpd = 2.5f;
    public float lowJumpSpd = 2f;

    private bool isGrounded;
    private Rigidbody alpha;
    private BoxCollider boxCollider;
    private Vector3 horizontalMovement;

    void Start()
    {
        alpha = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            this.gameObject.transform.Translate(new Vector3(alphaMovementSpd * Time.deltaTime, 0, 0));
        }
        else if (Input.GetKey(KeyCode.A))
        {
            this.gameObject.transform.Translate(new Vector3(-alphaMovementSpd * Time.deltaTime, 0, 0));
        }

        if (isGrounded)
        {
            fallSpd = -0.5f;

            if (Input.GetButtonDown("Jump"))
            {
                alpha.AddForce(Vector3.up * jumpSpd);

                if (alpha.velocity.y < 0)
                {
                    alpha.velocity += Vector3.up * Physics.gravity.y * (fallSpd - 1) * Time.deltaTime;
                }

                isGrounded = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
}
