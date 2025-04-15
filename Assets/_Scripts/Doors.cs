using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField] private bool notLockedDoor;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject leverObject;
    [SerializeField] private SwitchInteraction lever;

    private void Start()
    {
        if (lever != null)
        {
            lever = leverObject.GetComponent<SwitchInteraction>();
            notLockedDoor = false;
        }
        else
        {
            notLockedDoor = true;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (notLockedDoor || lever.SwitchActivated())
        {
            if (other.gameObject.tag == "Player")
            {
                door.transform.Translate(new Vector3(0, 0, 5));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (notLockedDoor || lever.SwitchActivated())
        {
            if (other.gameObject.tag == "Player")
            {
                door.transform.Translate(new Vector3(0, 0, -5));
            }
        }
    }
}
