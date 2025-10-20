using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackDisappear : MonoBehaviour
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
            other.GetComponent<Alpha>().TakeDamage(1);
        }

    }
}
