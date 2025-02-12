using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchInteraction : MonoBehaviour
{
    public bool isActivated = false;
    public Animator animator;

    private void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("player is in the trigger point");

        if(Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("starting animation");
            //isActivated = true;
            animator.SetBool("Activated", true);
            animator.Play("Activated");
        }
    }
}
