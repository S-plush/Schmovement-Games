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

public class VentsToFile : MonoBehaviour
{
    private string fileName = "VentData.txt";
    private string filePath;

    [HideInInspector] public bool HasCampVent = true;
    [HideInInspector] public bool HasDetention1Vent = false;
    [HideInInspector] public bool HasElevatorStorageVent = false;
    [HideInInspector] public bool HasDetention2Vent = false;

    //ADD NEW VENTS ABOVE THIS LINE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


    void Start()
    {
        // Set the file path inside persistentDataPath
        filePath = Path.Combine(Application.persistentDataPath, fileName);

        // Ensure the file exists first
        if (!File.Exists(filePath))
        {
            Debug.Log("File not found! Creating new file.");
            saveAllVentData(); // creates the file
        }

        if (MiscDataToFile.newGame == false)
        {
            loadAllVentData();
        }
        else //if it is a new game
        {
            saveAllVentData();
        }
    }

    void Update()
    {

    }

    public void saveAllVentData()
    {
        string dataIn = "";

        dataIn += HasCampVent.ToString() + "\n"; //0

        dataIn += HasDetention1Vent.ToString() + "\n"; //1

        dataIn += HasElevatorStorageVent.ToString() + "\n"; //2

        dataIn += HasDetention2Vent.ToString() + "\n"; //3
        //////////////////////////////////////////////////////////////////////ADD NEW VALUES TO THE SAVE FUNCTION ABOVE THIS LINE (only add at the bottom of the list though, order matters)


        WriteToFile(dataIn);
    }

    public void loadAllVentData()
    {
        String[] dataOut = ReadFromFile().Split('\n');
        int ArrayLength = dataOut.Length;


        HasCampVent = bool.Parse(dataOut[0]);

        HasDetention1Vent = bool.Parse(dataOut[1]);

        HasElevatorStorageVent = bool.Parse(dataOut[2]);

        HasDetention2Vent = bool.Parse(dataOut[3]);

        ////////////////////////////////////////////////////////////////////ADD NEW VALUES TO THE SAVE FUNCTION ABOVE THIS LINE (only add at the bottom of the list though, order matters)
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

    }
}
