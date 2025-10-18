using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeChecker : MonoBehaviour
{
    public bool groundDetected;

    public LayerMask Default;

    private void FixedUpdate() {

        if(Physics.Raycast(this.gameObject.transform.position, Vector3.down, 1f, Default)) {
            groundDetected = true;
        }
        else {
            groundDetected = false;
        }
    }


    public bool isGroundDetected() {
        return groundDetected;
    }
}
