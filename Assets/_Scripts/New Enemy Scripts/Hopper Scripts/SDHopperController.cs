using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class SDHopperController : Hopper
{
    [SerializeField] private int forwardVelocity;
    [SerializeField] private int upwardVelocity;

    private bool activated = false;


    void FixedUpdate() {

        facePlayer();

        if (Vector3.Distance(thisEnemyObject.transform.position, player.transform.position) > 8f) {
            inRange = false;
            attacking = false;

        } else {
            inRange = true;
            attacking = true;
        }

        if (Vector3.Distance(thisEnemyObject.transform.position, player.transform.position) < 3f && activated == false)
        {
            Debug.Log("Self-Destruct Sequence Activated");
            activated = true;
            timer = 0;
        }

        timer += Time.deltaTime;

        while (timer >= atkFrequency && activated == false) {
            jumpAttack(forwardVelocity, upwardVelocity);
            timer = 0;
        } 

        if(activated == true && timer >= 3) {


            Debug.Log("Boom!");
            Instantiate(bullet, thisEnemyObject.transform.position + new Vector3(0, 1, 0), thisEnemyObject.transform.rotation);
            Destroy(this.gameObject);
        }



        animator.SetBool("Grounded", isGrounded);

    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Ground") {
            isGrounded = true;
        }
    }
}
