using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VZ17PackCode : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player Spell") {
            Destroy(this.gameObject);
        }
    }
}
