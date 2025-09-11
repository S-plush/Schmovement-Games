using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HopperController : Hopper
{

    [SerializeField] private int forwardVelocity;
    [SerializeField] private int upwardVelocity;



    void FixedUpdate() {

        facePlayer();

        if (Vector3.Distance(thisEnemyObject.transform.position, player.transform.position) > 8f) {
            inRange = false;
            attacking = false;

        } else {
            inRange = true;
            attacking = true;
        }

        timer += Time.deltaTime;

        while (timer >= atkFrequency) {
            jumpAttack(forwardVelocity, upwardVelocity);
            timer -= atkFrequency;
        }

        animator.SetBool("Grounded", isGrounded);

    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Ground") {
            isGrounded = true;
        }
    }

}
