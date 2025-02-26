using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;

public class LoadoutsToFile : MonoBehaviour
{

    public GameObject loadout1Left;
    public GameObject loadout1Right;

    public GameObject loadout2Left;
    public GameObject loadout2Right;

    public GameObject loadout3Left;
    public GameObject loadout3Right;

    public GameObject loadout4Left;
    public GameObject loadout4Right;

    private string fileName = "LoadoutData.txt";
    private string filePath;

    public UnityEngine.Sprite defaultBox;

    public Image HUDSlot1; //ref to the left spell slot on HUD
    public Image HUDSlot2; //ref to the right spell slot on HUD

    //add spells here !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    public Spell Explosion;
    public Spell Lightning;

    void Start()
    {
        // Set the file path inside persistentDataPath
        filePath = Path.Combine(Application.persistentDataPath, fileName);

       
    }

    public void saveLoadoutsToFile()
    {
            string dataIn = "";

            //first loadout being saved (written to file)
            if (loadout1Left.transform.childCount > 0)
            {
                 dataIn += loadout1Left.transform.GetChild(0).gameObject.GetComponent<InventorySpell>().spell.name + "\n";
            }
            else
            {
                dataIn += "empty\n";
            }

            if (loadout1Right.transform.childCount > 0)
            {
                dataIn += loadout1Right.transform.GetChild(0).gameObject.GetComponent<InventorySpell>().spell.name + "\n";
            }
            else
            {
                dataIn += "empty\n";
            }

            //second loadout being saved
            if (loadout2Left.transform.childCount > 0)
            {
                dataIn += loadout2Left.transform.GetChild(0).gameObject.GetComponent<InventorySpell>().spell.name + "\n";
            }
            else
            {
                dataIn += "empty\n";
            }

            if (loadout2Right.transform.childCount > 0)
            {
                dataIn += loadout2Right.transform.GetChild(0).gameObject.GetComponent<InventorySpell>().spell.name + "\n";
            }
            else
            {
                dataIn += "empty\n";
            }

            //third loadout being saved
            if (loadout3Left.transform.childCount > 0)
            {
                dataIn += loadout3Left.transform.GetChild(0).gameObject.GetComponent<InventorySpell>().spell.name + "\n";
            }
            else
            {
                dataIn += "empty\n";
            }

            if (loadout3Right.transform.childCount > 0)
            {
                dataIn += loadout3Right.transform.GetChild(0).gameObject.GetComponent<InventorySpell>().spell.name + "\n";
            }
            else
            {
                dataIn += "empty\n";
            }

            //fourth loadout being saved
            if (loadout4Left.transform.childCount > 0)
            {
                dataIn += loadout4Left.transform.GetChild(0).gameObject.GetComponent<InventorySpell>().spell.name + "\n";
            }
            else
            {
                dataIn += "empty\n";
            }

            if (loadout4Right.transform.childCount > 0)
            {
                dataIn += loadout4Right.transform.GetChild(0).gameObject.GetComponent<InventorySpell>().spell.name + "\n";
            }
            else
            {
                dataIn += "empty\n";
            }

            
            WriteToFile(dataIn);

            //Debug.Log(ReadFromFile());
    }


    public void switchLoadouts(int numPressed) //int numPressed is the user's pushed key, for loadout switching, int 1-4
    {

        Loadout[] LoadoutSlots = FindObjectsOfType<Loadout>();

        // Loop through each instance and modify the variable, so all Loadout scripts know what loadout is currently equipped
        foreach (Loadout obj in LoadoutSlots)
        {
            obj.currentLoadoutSelected = numPressed;
        }

        //splitting up the data from the file to be more usable (could save it as indexs instead of string names though...)
        string beforeSplitData = ReadFromFile();
        string[] dataOut = beforeSplitData.Split('\n');

        string[] keyArray = {"empty", "Explosion", "Lightning", "etc"}; //add names of new spell scriptable objects to the end of the list here!!!!!!!!!!!!!!!!!!!!!!!!!!!

        int index1 = -1;
        int index2 = -1;

        //these all convert the information saved in the file to indexs that can be.........
        if (numPressed == 1)
        {
            index1 = Array.IndexOf(keyArray, dataOut[0]);
            index2 = Array.IndexOf(keyArray, dataOut[1]);
        }

        if (numPressed == 2)
        {
            index1 = Array.IndexOf(keyArray, dataOut[2]);
            index2 = Array.IndexOf(keyArray, dataOut[3]);
        }

        if (numPressed == 3)
        {
            index1 = Array.IndexOf(keyArray, dataOut[4]);
            index2 = Array.IndexOf(keyArray, dataOut[5]);
        }

        if (numPressed == 4)
        {
            index1 = Array.IndexOf(keyArray, dataOut[6]);
            index2 = Array.IndexOf(keyArray, dataOut[7]);
        }

        //these lines........ then add the spells image here!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        if (index1 == 0)
        {
            HUDSlot1.sprite = defaultBox;
        }
        else if (index1 == 1)
        {
            HUDSlot1.sprite = Explosion.image;
        }
        else if (index1 == 2)
        {
            HUDSlot1.sprite = Lightning.image;
        }
        else if (index1 == 3)
        {

        }

        if (index2 == 0)
        {
            HUDSlot2.sprite = defaultBox;
        }
        else if (index2 == 1)
        {
            HUDSlot2.sprite = Explosion.image;
        }
        else if (index2 == 2)
        {
            HUDSlot2.sprite = Lightning.image;
        }
        else if (index2 == 3)
        {

        }

        //Debug.Log(dataOut.Length);
        //Debug.Log(dataOut[0]);
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
