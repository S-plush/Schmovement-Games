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

    void Start()
    {
        // Set the file path inside persistentDataPath
        filePath = Path.Combine(Application.persistentDataPath, fileName);

        AlphaScript = FindObjectOfType<Alpha>(); //initilize AlphaScript with the actual script

        LoadoutsToFileScript = FindObjectOfType<LoadoutsToFile>(); //initilize LoadoutsToFileScript with the actual script

        loadAllMiscData();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            loadAllMiscData();
        }
    }

    public void saveAllMiscData()
    {
        string dataIn = "";
        numberOfEntries = 0;

        dataIn += AlphaScript.maxHealth + "\n"; //0                                        /////ADD NEW VALUES TO THE SAVE FUNCTION HERE (only add at the bottom though, order matters)
        numberOfEntries++;
        dataIn += AlphaScript.currentHealth + "\n"; //1
        numberOfEntries++;
        dataIn += AlphaScript.maxMana + "\n"; //2
        numberOfEntries++;
        dataIn += AlphaScript.currentMana + "\n"; //3
        numberOfEntries++;
        dataIn += AlphaScript.stimCount + "\n"; //4
        numberOfEntries++;
        dataIn += AlphaScript.currentlyEquippedLoadout + "\n"; //5
        numberOfEntries++;
        //dataIn += AlphaScript.; //6
        //numberOfEntries++;


        WriteToFile(dataIn);
    }

    public void loadAllMiscData()
    {
        String[] dataOut = ReadFromFile().Split('\n');
        int ArrayLength = dataOut.Length;

            player.GetComponent<Alpha>().currentHealth = Int32.Parse(dataOut[1]);
        //player.GetComponent<Alpha>().stimCount = Int32.Parse(dataOut[4]);

        AlphaScript.maxHealth = Int32.Parse(dataOut[0]);
            //AlphaScript.currentHealth = Int32.Parse(dataOut[1]);
            AlphaScript.maxMana = Int32.Parse(dataOut[2]);
            AlphaScript.currentMana = Int32.Parse(dataOut[3]);
            AlphaScript.stimCount = Int32.Parse(dataOut[4]);
            AlphaScript.currentlyEquippedLoadout = Int32.Parse(dataOut[5]); //does nothing yet

            foreach (String s in dataOut)
            {
                Debug.Log(s);
            }
    }

    void WriteToFile(string text)
    {
        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            writer.WriteLine(text);
        }
        //Debug.Log($"File written at: {filePath}");
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
}