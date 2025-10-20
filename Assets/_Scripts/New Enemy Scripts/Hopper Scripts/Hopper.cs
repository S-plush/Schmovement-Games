using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hopper : Enemy 
{
    protected bool isGrounded = true;
    protected bool inRange;

    public void jumpAttack(int forwardVel, int upwardVel) {
        if (inRange && isGrounded) { 
            if (isFacingRight) {
                thisRigidBody.velocity = new Vector3(forwardVel, upwardVel, 0);
                isGrounded = false;

            } else if (isFacingLeft) {
                thisRigidBody.velocity = new Vector3(-forwardVel, upwardVel, 0);
                isGrounded = false;

            }
        }
    }

    public void shootAttack() {
        if (inRange && isGrounded) {

            fireArea.transform.LookAt(player.transform.position + new Vector3(0, 1, 0));
            Instantiate(bullet, fireArea.transform.position, fireArea.transform.rotation);
        }
    }


    public void sdAttack() {
        if (inRange && isGrounded) {

        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Ground") {
            isGrounded = true;
        }
    }

}
