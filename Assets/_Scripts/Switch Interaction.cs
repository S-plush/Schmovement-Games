using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchInteraction : MonoBehaviour
{
    private bool isActivated = false;
    [SerializeField] private Animator switchActivated;
    private bool playerInside = false;

    private void Update()
    {
        if(playerInside && Input.GetKeyDown(KeyCode.R))
        {
            isActivated = true;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("player is in the trigger point");

        if(other.gameObject.tag == "Player")
        {
            Debug.Log("starting animation");
            playerInside = true;
            switchActivated.Play("Activating Switch", -1, 0f);
        }
    }

    public bool SwitchActivated()
    {
        if (isActivated)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
