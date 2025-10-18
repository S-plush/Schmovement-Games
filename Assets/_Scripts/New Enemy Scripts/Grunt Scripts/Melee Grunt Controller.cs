using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeGruntController : Grunt
{
    void FixedUpdate() {

        timer += Time.deltaTime;

        //Checks if Grunt is in range to fire
        if (Vector3.Distance(thisEnemyObject.transform.position, player.transform.position) > 2.5f) {
            inFireRange = false;
        } else {
            inFireRange = true;
        }

        //Checks if Grunt is in range to follow
        if (Vector3.Distance(thisEnemyObject.transform.position, player.transform.position) > 12f) {
            inFollowRange = false;
        } else {
            inFollowRange = true;
        }


        facePlayer();
        if (ledgeChecker.isGroundDetected()) {

            if (inFollowRange && inFireRange == false) {
                if (isFacingLeft) {
                    this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(player.transform.position.x, this.transform.position.y, this.transform.position.z), 0.05f);
                } else if (isFacingRight) {
                    this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(player.transform.position.x, this.transform.position.y, this.transform.position.z), 0.05f);
                }


            } else {
                this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position, 0.1f);
            }
        }


        while (timer >= atkFrequency) {

            facePlayer();
            meleeAttack();
            timer = 0;
        }
    }
}
