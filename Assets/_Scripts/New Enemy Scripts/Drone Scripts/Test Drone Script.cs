using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestDroneScript : Enemy
{

    public NavMeshAgent navMesh;
    protected bool inFollowRange;
    protected bool inFireRange;

    void FixedUpdate()
    {
        timer += Time.deltaTime;

        facePlayer();

        navMesh.SetDestination(player.transform.position);







        while (timer >= atkFrequency) {

            facePlayer();
            shootAttack();
            timer -= atkFrequency;
        }
    }

    public void shootAttack() {

            fireArea.transform.LookAt(player.transform.position + new Vector3(0, 1, 0));
            Instantiate(bullet, fireArea.transform.position, fireArea.transform.rotation);
        
    }
}
