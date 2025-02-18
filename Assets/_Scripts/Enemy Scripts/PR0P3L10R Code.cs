using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR0P3L10RCode : MonoBehaviour
{

    [SerializeField] private GameObject player;


    LayerMask terrainLayerMask;
    LayerMask playerLayerMask;


    private bool one, two, three, four, six, seven, eight, nine;






    // Start is called before the first frame update
    void Start()
    {
        terrainLayerMask = LayerMask.GetMask("Default");
        playerLayerMask = LayerMask.GetMask("Player");

    }

    // Update is called once per frame
    void FixedUpdate() {
        RaycastHit hit;


            
 
    }

    void checkDirections() {
        RaycastHit hit;

        one = false; 
        two = false;
        three = false;
        four = false;
        six = false;
        seven = false;
        eight = false;
        nine = false;

  

        if (Physics.Raycast(transform.position, transform.TransformDirection(new Vector3(0, -1, -1)), out hit, 10f, terrainLayerMask)) {
            one = true;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 10f, terrainLayerMask)) {
            two = true;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 10f, terrainLayerMask)) {
            three = true;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 10f, terrainLayerMask)) {
            four = true;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 10f, terrainLayerMask)) {
            six = true;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 10f, terrainLayerMask)) {
            seven = true;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 10f, terrainLayerMask)) {
            eight = true;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 10f, terrainLayerMask)) {
            nine = true;
        }



    }
    
    
    void atkPattern() {
        RaycastHit hit;

        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 10f, terrainLayerMask)) {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");


        } else {


            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 10f, Color.white);
            Debug.Log("Did not Hit");
        }
    }
}
