using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossController : FinalBoss 
{

    void FixedUpdate() {
        timer += Time.deltaTime;
        facePlayer();


        while (timer >= 4) {

            facePlayer();
            initiateAttack();
        }
    }


    void initiateAttack() {
        int atkType = Random.Range(0, 5);

        Debug.Log("Cornered = " + isCornered());
        if (isCornered()) {
            if (isFacingLeft) {
                escape(-1);
            } else if (isFacingRight) {
                escape(1);
            }

            timer = 2;
            return;
        } 



        if (Vector3.Distance(thisEnemyObject.transform.position, player.transform.position) < 2.5f) {

            if (isFacingLeft) {
                escape(1);
            } else if (isFacingRight) {
                escape(-1);
            }
            
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
                Invoke("fireAttack", 0.1f);
                Invoke("fireAttack", 0.3f);
                Invoke("fireAttack", 0.5f);
                timer = 2; break;
            case 3:

                Invoke("fireAttack", 0.1f);
                Invoke("fireAttack", 0.3f);
                Invoke("fireAttack", 0.5f);
                timer = 2; break;
            case 4:
                leap();
                Invoke("fireAttack", 0.1f);
                Invoke("fireAttack", 0.3f);
                Invoke("fireAttack", 0.5f);
                timer = 1;  break;
            case 5:
                Invoke("fireAttack", 0.1f);
                Invoke("fireAttack", 0.3f);
                Invoke("fireAttack", 0.5f);
                Invoke("fireAttack", 0.7f);
                Invoke("fireAttack", 0.9f);

                timer = 0; break;


        }

    }


}
