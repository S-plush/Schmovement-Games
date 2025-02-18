using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneNodeCode : MonoBehaviour
{
    public string id;
    private bool isInGround;

    // Start is called before the first frame update
    void Start()
    {
        isInGround = false;    
    }


    private void OnCollisionEnter(Collision collision) {
        if(collision.collider.tag == "Ground" || collision.collider.tag == "Wall") {

            isInGround = true;
        }
            
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.collider.tag == "Ground" || collision.collider.tag == "Wall") {
            isInGround = false;
        }
    }


    private void OnTriggerEnter(Collider other) {
        
    }
    public bool checkInGround() {
        return isInGround;
    }
}
