using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseDroneController : Drone
{
    void FixedUpdate() {
        timer += Time.deltaTime;

        //Checks if Drone is in range to fire
        if (Vector3.Distance(thisEnemyObject.transform.position, player.transform.position) > 8f) {
            inFireRange = false;
        } else {
            inFireRange = true;
        }

        //Checks if Drone is in range to follow
        if (Vector3.Distance(thisEnemyObject.transform.position, player.transform.position) > 12f) {
            inFollowRange = false;
        } else {
            inFollowRange = true;
        }


        facePlayer();


        
        if (inFollowRange && !inFireRange) {
            navMesh.SetDestination(player.transform.position);

        } else {
            navMesh.SetDestination(this.transform.position);
        }

        while (timer >= atkFrequency) {

            facePlayer();
            shootAttack();
            timer -= atkFrequency;
        }
    }
}
