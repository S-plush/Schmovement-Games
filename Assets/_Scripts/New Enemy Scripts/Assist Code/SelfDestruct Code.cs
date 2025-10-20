using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SelfDestructCode : MonoBehaviour
{

    private void Start() {
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay() {
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other) {

        if (other.tag == "Player" && this.gameObject != null) {
            other.GetComponent<Alpha>().TakeDamage(2);
        }
        
    }



}
