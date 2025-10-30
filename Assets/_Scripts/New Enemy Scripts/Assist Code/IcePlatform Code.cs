using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePlatformCode : MonoBehaviour
{

    private int health = 5;

    private void Start() {
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay() {
        yield return new WaitForSeconds(20);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player Spell") {
            health -= 1;
            Debug.Log(health);
            if (health == 0) {
                Destroy(this.gameObject);
            }
        }
        

    }




}
