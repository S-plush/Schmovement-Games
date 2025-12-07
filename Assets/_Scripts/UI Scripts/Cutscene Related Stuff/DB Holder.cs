using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBHolder : MonoBehaviour
{
    private bool playerInside = false;
    private bool inDialogue = false;
    private int currentDialogue = 0;
    private Alpha alpha;

    public GameObject[] characterDialogue;
    public GameObject interactionUI;

    private void Start()
    {
        alpha = FindObjectOfType<Alpha>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.R) && !inDialogue)
        {
            alpha.IsInDialogue();
            Debug.Log("current dialogue num is: " + currentDialogue);
            alpha.PauseGame();
            characterDialogue[currentDialogue].SetActive(true);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInside = true;
            interactionUI.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInside = false;
            interactionUI.SetActive(false);
        }
    }

    public void GoToNextDialogue()
    {
        Debug.Log("current dialogue num before is: " + currentDialogue);

        if (currentDialogue == characterDialogue.Length - 1)
        {
            currentDialogue = characterDialogue.Length - 1;
            alpha.IsInDialogue();
            alpha.PauseGame();
        }
        else if(currentDialogue < characterDialogue.Length)
        {
            currentDialogue++;
            alpha.IsInDialogue();
            alpha.PauseGame();
        }

        Debug.Log("current dialogue num after is: " + currentDialogue);
    }

    public bool InDialogueCheck()
    {
        if (inDialogue)
        {
            return true;
        }
        
        return false;
    }
}