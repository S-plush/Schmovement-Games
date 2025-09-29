using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyRotateCode : MonoBehaviour
{

    public static int MoneyCount;


    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            MoneyCount++;
            Debug.Log(MoneyCount);
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate() {
        this.gameObject.transform.Rotate(0, 0, 5);
    }
}
