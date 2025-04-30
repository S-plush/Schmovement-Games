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
        if(playerInside && Input.GetKeyDown(KeyCode.R) && !isActivated)
        {
            switchActivated.Play("Activating Switch", -1, 0f);
            isActivated = true;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("player is in the trigger point");

        if(other.gameObject.tag == "Player")
        {
            playerInside = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInside = false;
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
