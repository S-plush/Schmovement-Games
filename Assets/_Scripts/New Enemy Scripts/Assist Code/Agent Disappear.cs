using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentDisappear : MonoBehaviour
{
    public GameObject childObject;

    private void FixedUpdate() {
        if(childObject == null) {
            Destroy(this.gameObject);
        }
    }
}
