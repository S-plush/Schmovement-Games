using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt : Enemy
{

    protected bool isGrounded = true;
    protected bool inFollowRange;
    protected bool inFireRange;

    public LedgeChecker ledgeChecker;



    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Ground") {
            isGrounded = true;
        }
    }
    public void shootAttack() {
        if (inFireRange && isGrounded) {

            fireArea.transform.LookAt(player.transform.position + new Vector3(0, 1, 0));
            Instantiate(bullet, fireArea.transform.position, fireArea.transform.rotation);
        }
    }
}
