using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class MiscDataToFile : MonoBehaviour
{
    private string fileName = "MiscData.txt";
    private string filePath;

    private int numberOfEntries;

    public GameObject player;

    GameObject createdPlayer;
    Alpha AlphaScript; //reference to the Alpha Script on the Player
    LoadoutsToFile LoadoutsToFileScript; //reference to the LoadoutsToFile on the InventoryManager
    InvDataBetweenRuns InvDataBetweenRunsScript; //reference to the InvDataBetweenRuns on the InventoryManager

    public static bool newGame = false; //can be switched to give the player a brand new save

    void Awake()
    {
        // Set the file path inside persistentDataPath
        filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (SceneManager.GetActiveScene().name != "Main Menu") //CREATING A PLAYER IN HERE
        {
            if (newGame == false)
            {
                loadJustRPStuff();
            }
            else
            {
                RespawnPoint.currentCheckpointName = "START"; //beginning of game hard coded
                RespawnPoint.currentCheckpointSceneName = "DetentionCenter";
            }

            Debug.Log("(From Misc Script) SceneName: " + SceneManager.GetActiveScene().name + " CHECKYNAME: " + RespawnPoint.currentCheckpointName);

            try
            {
                GameObject rp = GameObject.FindWithTag("Respawn Point");
                rp.transform.position = GameObject.Find(RespawnPoint.currentCheckpointName).transform.position;
                createdPlayer = Instantiate(player, rp.gameObject.transform.position, rp.gameObject.transform.rotation);
                createdPlayer.SetActive(true);

                AlphaScript = createdPlayer.GetComponent<Alpha>();
            }
            catch (System.Exception ex)
            {
                Debug.Log("PLAYER SPAWN FAILED (wrong checkpoint): INSTANTIATING AT RESPAWN POINT");
                GameObject rp = GameObject.FindWithTag("Respawn Point");
                createdPlayer = Instantiate(player, rp.gameObject.transform.position, rp.gameObject.transform.rotation);
                createdPlayer.SetActive(true);

                AlphaScript = createdPlayer.GetComponent<Alpha>();
            }
        }
        else
        {
            loadJustRPStuff();
            //String[] dataOut = ReadFromFile().Split('\n');
            //int ArrayLength = dataOut.Length;

            //RespawnPoint.currentCheckpointSceneName = dataOut[5];
            //Debug.Log(dataOut[5]);
        }

        LoadoutsToFileScript = FindObjectOfType<LoadoutsToFile>(); //initilize LoadoutsToFileScript with the actual script
        InvDataBetweenRunsScript = FindObjectOfType<InvDataBetweenRuns>(); //initilize LoadoutsToFileScript with the actual script
    }

    public void Start()
    {
        if (!File.Exists(filePath))
        {
            Debug.Log("File not found! Creating new file.");
            saveAllMiscData(); // creates the file
        }

        if (newGame == false)
        {

        }
        else
        {
            //Debug.Log("loadNew");

            InvDataBetweenRunsScript.ClearAllInv();

            //assign default stats
            //stims 3, health 5, mana 5, current loadout 1

            AlphaScript.maxHealth = 5;
            //AlphaScript.currentHealth = 5;
            AlphaScript.maxMana = 5;
            //AlphaScript.currentMana = 5;
            AlphaScript.maxStims = 3;
            //AlphaScript.stimCount = 3;
            AlphaScript.currentlyEquippedLoadout = 1;

            //updating changing values
            AlphaScript.currentHealth = AlphaScript.maxHealth;
            AlphaScript.currentMana = AlphaScript.maxMana;
            AlphaScript.stimCount = AlphaScript.maxStims;

            //updating the UI
            AlphaScript.currentHealth = AlphaScript.maxHealth;
            AlphaScript.healthBar.SetMaxHealth(AlphaScript.maxHealth);

            AlphaScript.manaBar.SetMaxMana(AlphaScript.maxMana);
            AlphaScript.currentMana = AlphaScript.maxMana;

            AlphaScript.stimCountText.text = AlphaScript.maxStims + "\n\nStims";
            saveAllMiscData();

            newGame = false;
        }
        loadAllMiscData();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) //simulates loading the data (on death or scene transition I presume)
        {
            loadAllMiscData();
        }

        if (Input.GetKeyDown(KeyCode.N)) //simulates new game
        {
            //assign default stats
            //stims 3, health 5, mana 5, current loadout 1

            AlphaScript.maxHealth = 5;
            AlphaScript.maxMana = 5;
            AlphaScript.maxStims = 3;
            AlphaScript.currentlyEquippedLoadout = 1;
            RespawnPoint.currentCheckpointName = "point 1 in D1";
            RespawnPoint.currentCheckpointSceneName = "DetentionCenter";

            //updating changing values
            AlphaScript.currentHealth = AlphaScript.maxHealth;
            AlphaScript.currentMana = AlphaScript.maxMana;
            AlphaScript.stimCount = AlphaScript.maxStims;

            //updating the UI
            AlphaScript.healthBar.SetMaxHealth(AlphaScript.maxHealth);
            AlphaScript.manaBar.SetMaxMana(AlphaScript.maxMana);
            AlphaScript.stimCountText.text = AlphaScript.maxStims + "\n\nStims";

            newGame = false;

            saveAllMiscData();
        }
    }

    public void saveAllMiscData()
    {
        string dataIn = "";
        numberOfEntries = 0;

        dataIn += AlphaScript.maxHealth + "\n"; //0
        numberOfEntries++;

        dataIn += AlphaScript.maxMana + "\n"; //1
        numberOfEntries++;

        dataIn += AlphaScript.maxStims + "\n"; //2
        numberOfEntries++;

        dataIn += AlphaScript.currentlyEquippedLoadout + "\n"; //3
        numberOfEntries++;

        dataIn += RespawnPoint.currentCheckpointName + "\n"; //4
        numberOfEntries++;

        dataIn += RespawnPoint.currentCheckpointSceneName + "\n"; //5
        numberOfEntries++;

        //////////////////////////////////////////////////////////////////////ADD NEW VALUES TO THE SAVE FUNCTION HERE (only add at the bottom though, order matters)

        WriteToFile(dataIn);
    }

    public void loadAllMiscData()
    {
        String[] dataOut = ReadFromFile().Split('\n');
        int ArrayLength = dataOut.Length;

        //foreach (String s in dataOut)
        //{
            //Debug.Log($"[{s}]");
        //}

        //player.GetComponent<Alpha>().currentHealth = Int32.Parse(dataOut[1]);
        //player.GetComponent<Alpha>().stimCount = Int32.Parse(dataOut[4]);

        //load all data to AlphaScript
        AlphaScript.maxHealth = Int32.Parse(dataOut[0]);
        AlphaScript.maxMana = Int32.Parse(dataOut[1]);
        AlphaScript.maxStims = Int32.Parse(dataOut[2]);
        AlphaScript.currentlyEquippedLoadout = Int32.Parse(dataOut[3]);
        RespawnPoint.currentCheckpointName = dataOut[4];
        RespawnPoint.currentCheckpointSceneName = dataOut[5];

        //updating changing values
        AlphaScript.currentHealth = AlphaScript.maxHealth;
        AlphaScript.currentMana = AlphaScript.maxMana;
        AlphaScript.stimCount = AlphaScript.maxStims;

        //updating the UI
        AlphaScript.healthBar.SetMaxHealth(AlphaScript.maxHealth);

        AlphaScript.manaBar.SetMaxMana(AlphaScript.maxMana);

        AlphaScript.stimCountText.text = AlphaScript.maxStims + "\n\nStims";
    }

    public void loadJustRPStuff ()
    {
        String[] dataOut = ReadFromFile().Split('\n');
        int ArrayLength = dataOut.Length;

        RespawnPoint.currentCheckpointName = dataOut[4];
        RespawnPoint.currentCheckpointSceneName = dataOut[5];
    }

    void WriteToFile(string text)
    {
        File.WriteAllText(filePath, text.TrimEnd('\n'));
    }

    string ReadFromFile()
    {
        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                return reader.ReadToEnd();
            }
        }
        else
        {
            Debug.LogWarning("File not found!");
            return "";
        }
    }

    void OnApplicationQuit()
    {
        saveAllMiscData();
    }
}
