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
            Debug.Log("current dialogue num is: " + currentDialogue);
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
        Debug.Log("current dialogue num before is: " + currentDialogue);

        if (currentDialogue == characterDialogue.Length - 1)
        {
            currentDialogue = characterDialogue.Length - 1;
            inDialogue = false;
        }
        else if(currentDialogue < characterDialogue.Length)
        {
            currentDialogue++;
            inDialogue = false;
        }

        Debug.Log("current dialogue num after is: " + currentDialogue);
        //Time.timeScale = 1.0f;
    }
}
