using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class MiscDataToFile : MonoBehaviour
{
    private string fileName = "MiscData.txt";
    private string filePath;

    private int numberOfEntries;

    public GameObject player;
    Alpha AlphaScript; //reference to the Alpha Script on the Player

    LoadoutsToFile LoadoutsToFileScript; //reference to the LoadoutsToFile on the InventoryManager

    public bool newGame = false; //hopefully can be switched later to give the player a brand new save

    void Start()
    {
        if (newGame == false)
        {
            // Set the file path inside persistentDataPath
            filePath = Path.Combine(Application.persistentDataPath, fileName);

            AlphaScript = FindObjectOfType<Alpha>(); //initilize AlphaScript with the actual script
            LoadoutsToFileScript = FindObjectOfType<LoadoutsToFile>(); //initilize LoadoutsToFileScript with the actual script

            loadAllMiscData();
        }
        else
        {
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
            AlphaScript.healthBar.SetMaxHealth(AlphaScript.maxHealth);
            AlphaScript.manaBar.SetMaxMana(AlphaScript.maxMana);
            AlphaScript.stimCountText.text = AlphaScript.maxStims + "\n\nStims";

            newGame = false;
        }
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

        //////////////////////////////////////////////////////////////////////ADD NEW VALUES TO THE SAVE FUNCTION HERE (only add at the bottom though, order matters)

        WriteToFile(dataIn);
    }

    public void loadAllMiscData()
    {
        String[] dataOut = ReadFromFile().Split('\n');
        int ArrayLength = dataOut.Length;

        //foreach (String s in dataOut)
        //{
        //    Debug.Log($"[{s}]");
        //}

        //player.GetComponent<Alpha>().currentHealth = Int32.Parse(dataOut[1]);
        //player.GetComponent<Alpha>().stimCount = Int32.Parse(dataOut[4]);

        AlphaScript.maxHealth = Int32.Parse(dataOut[0]);
        AlphaScript.maxMana = Int32.Parse(dataOut[1]);
        AlphaScript.maxStims = Int32.Parse(dataOut[2]);
        AlphaScript.currentlyEquippedLoadout = Int32.Parse(dataOut[3]);
        //updating changing values
        AlphaScript.currentHealth = AlphaScript.maxHealth;
        AlphaScript.currentMana = AlphaScript.maxMana;
        AlphaScript.stimCount = AlphaScript.maxStims;

        //updating the UI
        AlphaScript.healthBar.SetMaxHealth(AlphaScript.maxHealth);
        AlphaScript.manaBar.SetMaxMana(AlphaScript.maxMana);
        AlphaScript.stimCountText.text = AlphaScript.maxStims + "\n\nStims";
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
