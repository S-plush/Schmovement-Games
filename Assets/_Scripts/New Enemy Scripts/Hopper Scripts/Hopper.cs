using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hopper : Enemy 
{

    public void jumpAttack(int forwardVel, int upwardVel) {
        if (isFacingRight) {
            //Debug.Log("jumped");

            thisRigidBody.velocity = new Vector3(forwardVel, upwardVel, 0);

        } else if (isFacingRight) {
            //Debug.Log("jumped");

            thisRigidBody.velocity = new Vector3(-forwardVel, upwardVel, 0);

        }
    }

    public void shootAttack() {

    }


}
