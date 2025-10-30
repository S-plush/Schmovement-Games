using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossController : FinalBoss 
{
    bool rightClose;
    bool leftClose;

    void FixedUpdate() {
        timer += Time.deltaTime;
        facePlayer();




        while (timer >= 4) {

            facePlayer();
            initiateAttack();
        }
    }


    void initiateAttack() {
        int atkType = Random.Range(0, 4);
        if(Vector3.Distance(thisEnemyObject.transform.position, player.transform.position) < 2.5f) {
            leap();
            timer = 2;
            return;
        }


        Debug.Log(atkType);

        switch (atkType) {
            case 0:
                summonHighGround();
                Invoke("fireAttack", 1.1f);
                timer = 1; break;
            case 1:
                leap();
                Invoke("summonHoppers", 1f);
                timer = 0; break;
            case 2:
                leap();
                timer = 0; break;
            case 3:

                Invoke("fireAttack", 0.1f);
                Invoke("fireAttack", 0.3f);
                Invoke("fireAttack", 0.5f);
                timer = 2; break;
        }

    }


}
