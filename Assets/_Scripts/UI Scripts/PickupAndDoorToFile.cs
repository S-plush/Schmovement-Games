using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class PickupAndDoorToFile : MonoBehaviour
{
    private string fileName = "PickupData.txt";
    private string filePath;

    public List<string> scenes = new List<string>();

    public List<GameObject> pickups = new List<GameObject>();
    public string claimedPickups = "";

    public List<Doors> doors = new List<Doors>();
    public string claimedDoors = "";

    public List<MovingPlatform> movingPlatforms = new List<MovingPlatform>();
    public string claimedPlatforms = "";

    void Start()
    {
        // Set the file path inside persistentDataPath
        filePath = Path.Combine(Application.persistentDataPath, fileName);

        // Ensure the file exists first
        if (!File.Exists(filePath))
        {
            Debug.Log("File not found! Creating new file.");
            saveAllPickupData(); // creates the file
        }

        if (MiscDataToFile.newGame == false)
        {
            loadAllPickupData();
        }
        else //if it is a new game
        {
            saveAllPickupData();
        }
        saveAllPickupData();
    }

    void FixedUpdate()
    {
        for (int i = 0; i < pickups.Count; i++)
        {
            if (pickups[i] == null)
            {
                claimedPickups += pickups[i].name + ","; //PROBABLY DOESN"T WORK BECAUSE THIS OBJECT HAS BEEN DESTROYED!?!?!?!?!?!?!?!?!?!?!?!?!!?!?!?!!?
                saveAllPickupData();
            }
        }
        for (int i = 0; i < doors.Count; i++)
        {
            if (doors[i] == null)
            {

                saveAllPickupData();
            }
        }
        for (int i = 0; i < movingPlatforms.Count; i++)
        {
            if (movingPlatforms[i] == null)
            {

                saveAllPickupData();
            }
        }
    }

    public void saveAllPickupData()
    {
        List<string> lines = new List<string>();

        // Read all existing lines
        if (File.Exists(filePath) && MiscDataToFile.newGame!) //POSSIBLE PROBLEM!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!?!!?!?!?!?!?!?!?
        {
            lines.AddRange(File.ReadAllLines(filePath));
        }

        // Ensure file has enough lines
        while (lines.Count <= (scenes.Count*3)+1)
        {
            lines.Add(""); // add empty lines if needed
        }

        if (!scenes.Contains(Alpha.PlayerRef.scene.name)) //add new scene if this is the first time visiting it
        {
            scenes.Add(Alpha.PlayerRef.scene.name);
        }

        for (int i = 0; i < scenes.Count; i++) //line 1, index 0, scenes
        {
            lines[0] = scenes[i] + ",";
        }
        lines[0] += "!";



        int fileGroup = scenes.IndexOf(Alpha.PlayerRef.scene.name) + 1;

        for (int i = 0; i < pickups.Count; i++) //1st line repeating (lines 1-3 blank), index 4,7,10,etc., pickups
        {
            lines[(3 * fileGroup)+1] = pickups[i] + ",";
        }
        lines[(3 * fileGroup) + 1] += "!";

        for (int i = 0; i < doors.Count; i++) //2nd line repeating (lines 1-3 blank), index 5,8,11,etc., doors
        {
            lines[(3 * fileGroup) + 2] = doors[i] + ",";
        }
        lines[(3 * fileGroup) + 2] += "!";

        for (int i = 0; i < movingPlatforms.Count; i++) //3nd line repeating (lines 1-3 blank), index 6,9,12,etc., movingPlatforms
        {
            lines[(3 * fileGroup) + 3] = movingPlatforms[i] + ",";
        }
        lines[(3 * fileGroup) + 3] += "!";



        File.WriteAllLines(filePath, lines);
    }

    public void loadAllPickupData()
    {
        String[] dataOut = ReadFromFile().Split('!');
        int ArrayLength = dataOut.Length;

        scenes = dataOut[0].Split(',').ToList();

        if(scenes.Contains(Alpha.PlayerRef.scene.name))
        {
            claimedPickups = dataOut[scenes.IndexOf(Alpha.PlayerRef.scene.name) + 4];
            for (int i = 0; i < claimedPickups.Split(',').ToList().Count; i++) //loop through all items from file
            {
                Destroy(pickups[i]);
            }
        }
        //REPEAT FOR DOORS AND MOVABLE IF THIS EVEN WORKS IN THE SLIGHTEST BRU!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    }

    void WriteToFile(string text)
    {
        File.WriteAllText(filePath, text.TrimEnd('!'));
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
        saveAllPickupData();
    }
}
