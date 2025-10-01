using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Drone : Enemy
{
    protected bool inFollowRange;
    protected bool inFireRange;
    [SerializeField] protected NavMeshAgent navMesh;


    public void shootAttack() {

        if (inFireRange) {
            Debug.Log("Drone Fired");
            fireArea.transform.LookAt(player.transform.position + new Vector3(0, 1, 0));
            Instantiate(bullet, fireArea.transform.position, fireArea.transform.rotation);
        };

    }
}
