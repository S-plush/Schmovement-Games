using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class VentScript : MonoBehaviour
{
    private Animator ventAnimator;

    public static bool playerInside = false;

    public GameObject ventSelectUI;
    public GameObject interactionUI;

    public GameObject HUD;

    private VentsToFile VentsToFileScript;

    private MiscDataToFile MiscDataToFileScript;

    public GameObject CampButton;
    public GameObject Detention1Button;
    public GameObject Storage1Button;
    public GameObject Detention2Button;

    //ADD YOUR NEW BUTTON AS A VARIABLE ABOVE THIS LINE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


    public void Start()
    {
        ventAnimator = GetComponentInChildren<Animator>();
        
        playerInside = false;

        HUD = Alpha.FindInScene("Main HUD Group");

        VentsToFileScript = FindObjectOfType<VentsToFile>(); //initilize VentsToFileScript with the actual script

        MiscDataToFileScript = FindObjectOfType<MiscDataToFile>(); //initilize MiscDataToFileScript with the actual script
    }

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.R) || (playerInside && ventSelectUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))) //checks if player is interacting with the vent or pressing ESC to leave the vent menu
        {
            if (ventSelectUI.activeSelf)
            {
                HUD.SetActive(true);
                ventSelectUI.SetActive(false);
                Time.timeScale = 1.0f;
                Alpha.isGamePaused = false;
            }
            else
            {
                HUD.SetActive(false);
                ventSelectUI.SetActive(true);
                Time.timeScale = 0.0f;
                Alpha.isGamePaused = true;

                if (VentsToFileScript.HasDetention1Vent == false && this.gameObject.name == "D1 Vent Interactable")
                {
                    VentsToFileScript.HasDetention1Vent = true;
                }
                if (VentsToFileScript.HasElevatorStorageVent == false && this.gameObject.name == "Storage1 Vent Interactable")
                {
                    VentsToFileScript.HasElevatorStorageVent = true;
                }
                if (VentsToFileScript.HasDetention2Vent == false && this.gameObject.name == "D2 Vent Interactable")
                {
                    VentsToFileScript.HasDetention2Vent = true;
                }

                //ADD NEW VENTS AS AN IF STATEMENT ABOVE THIS LINE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


                VentsToFileScript.saveAllVentData(); //saves the goods from just before

                if (VentsToFileScript.HasCampVent == true)
                {
                    CampButton.SetActive(VentsToFileScript.HasCampVent);
                }
                if(VentsToFileScript.HasDetention1Vent == true)
                {
                    Detention1Button.SetActive(VentsToFileScript.HasDetention1Vent);
                }
                if (VentsToFileScript.HasElevatorStorageVent == true)
                {
                    Storage1Button.SetActive(VentsToFileScript.HasElevatorStorageVent);
                }
                if (VentsToFileScript.HasDetention2Vent == true)
                {
                    Detention2Button.SetActive(VentsToFileScript.HasDetention2Vent);
                }

                //ADD NEW VENTS AS AN IF STATEMENT ABOVE THIS LINE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            }
        }
            
        ventAnimator.SetBool("open", playerInside);
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("player is in the trigger point");

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

    ///////stuff for UI buttons below here///////////////////////////////////////////////////

    public void LoadSceneOfPushedButton()
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;

        if (clickedButton.name == "CampButton")
        {
            RespawnPoint.currentCheckpointName = "Camp Vent Transition";
            RespawnPoint.currentCheckpointSceneName = "CampArea";
        }
        else if (clickedButton.name == "Detention1Button")
        {
            RespawnPoint.currentCheckpointName = "D1 Vent Transition";
            RespawnPoint.currentCheckpointSceneName = "DetentionCenter";
        }
        else if (clickedButton.name == "Elevators/Storage")
        {
            RespawnPoint.currentCheckpointName = "Storage1 Vent Transition";
            RespawnPoint.currentCheckpointSceneName = "StorageSector1";
        }
        else if (clickedButton.name == "Detention2Button")
        {
            RespawnPoint.currentCheckpointName = "D2 Vent Transition";
            RespawnPoint.currentCheckpointSceneName = "DetentionCenter2";
        }

        //ADD NEW VENTS AS AN ELSE IF STATEMENT ABOVE THIS LINE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


        MiscDataToFileScript.saveAllMiscData();
        Debug.Log(RespawnPoint.currentCheckpointName + "FROM VENT SCRIPT BEFORE");
        SceneManager.LoadScene(RespawnPoint.currentCheckpointSceneName);
    }
}
