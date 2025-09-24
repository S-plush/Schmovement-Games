using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHolder : MonoBehaviour
{
    private bool playerInside = false;
    private bool inDialogue = false;
    private int currentDialogue = 0;

    public GameObject[] characterDialogue;

    // Update is called once per frame
    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.R) && !inDialogue)
        {
            inDialogue = true;
            characterDialogue[currentDialogue].SetActive(true);
            //Time.timeScale = 0.0f;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
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

    public void GoToNextDialogue()
    {
        if(currentDialogue < characterDialogue.Length)
        {
            currentDialogue++;
            inDialogue = false;
        }

        //Time.timeScale = 1.0f;
    }
}
