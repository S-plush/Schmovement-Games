using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeChecker : MonoBehaviour
{
    public bool groundDetected;

    private void OnTriggerEnter(Collider other) {
        groundDetected = false;

        Debug.Log("Ground Detected");
    }


    private void OnTriggerExit(Collider other) {
        groundDetected = true;
        Debug.Log("Ground Not Detected");
    }

    public bool isGroundDetected() {
        return groundDetected;
    }

}
