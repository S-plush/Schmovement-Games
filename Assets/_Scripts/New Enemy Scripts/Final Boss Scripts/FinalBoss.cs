using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class FinalBoss : Enemy
{
    protected bool isGrounded = true;
    protected bool inFollowRange;
    protected bool inFireRange;

    [SerializeField] private Rigidbody finalBossRigidBody;
    [SerializeField] private GameObject icePlatformGameObject;
    [SerializeField] private GameObject fireBlastGameObject;
    [SerializeField] private GameObject stoneAttackGameObject;
    [SerializeField] private GameObject windWallGameObject;
    [SerializeField] private GameObject sDHopper;


    public void leap() {

        float xDirec = Random.Range(0, 2);
        if (xDirec == 0) {
            xDirec = -1;
        }

        finalBossRigidBody.AddForce(new Vector3(400 * xDirec, 400f, 0));
    }
    
    public void fireAttack() {
        fireArea.transform.LookAt(player.transform.position + new Vector3(0, 1, 0));
        Instantiate(fireBlastGameObject, fireArea.transform.position, fireArea.transform.rotation);
    }



    public void summonHighGround() {


        float xDirec = Random.Range(-1, 2);

        finalBossRigidBody.AddForce(new Vector3(300 * xDirec, 500f, 0));
        Debug.Log("Jump!");
        Invoke("summonIcePlatform", 1f);
    }

    public void summonIcePlatform() {
        Instantiate(icePlatformGameObject, this.gameObject.transform.position + new Vector3(0, -0.5f, 0), this.gameObject.transform.rotation);
    }


    public void summonHoppers() {

        leap();

        int numHoppers;

        float hopperPos;


        numHoppers = Random.Range(2, 4);

        Debug.Log("numHopper: " + numHoppers);

        for (int i = 0; i < numHoppers; i++) { 
        
            hopperPos = Random.Range(-3, 3);

            Debug.Log("hopPos: " + hopperPos);

            Debug.Log(sDHopper != null);
            Debug.Log(thisEnemyObject != null);

            Instantiate(sDHopper, this.gameObject.transform.position + new Vector3(hopperPos, -1, 0), this.gameObject.transform.rotation);

        }

    }







}
