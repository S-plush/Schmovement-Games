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

    [HideInInspector] public int index1;
    [HideInInspector] public int index2;

    [HideInInspector] public String[] equippedSpells;

    public UnityEngine.Sprite defaultBox;

    public Image HUDSlot1; //ref to the left spell slot on HUD
    public Image HUDSlot2; //ref to the right spell slot on HUD

    //add spells here !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    public Spell Explosion;
    public Spell Lightning;
    public Spell IcicleSpear;
    public Spell SoundWave;
    public Spell Boulder;
    public Spell Earth;
    public Spell Wind;

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

    public void switchLoadouts(int numPressed) //int numPressed is the user's pushed key, for loadout switching, int numpressed 1-4. returns a string of the index1 value followed by the index2 value, these two values are seperated by a comma
    {

        Loadout[] LoadoutSlots = FindObjectsOfType<Loadout>();

        // Loop through each instance and modify the variable, so all Loadout scripts know what loadout is currently equipped
        foreach (Loadout obj in LoadoutSlots)
        {
                obj.currentLoadoutSelected = numPressed;
        }

        //splitting up the data from the file to be more usable (could save it as ints instead of string names though...)
        string[] dataOut = ReadFromFile().Split('\n');

        string[] keyArray = { "empty", "Explosion", "Lightning", "Icicle Spear", "Sound Wave", "Boulder", "Earth", "Wind", "etc" }; //add names of new spell scriptable objects to the end of the list here!!!!!!!!!!!!!!!!!!!!!!!!!!!

            //these all convert the information saved in the file to indexs for the UI, and change the equippedSpells variable that the Alpha script is reading
            if (numPressed == 1)
            {
                index1 = Array.IndexOf(keyArray, dataOut[0]);
                index2 = Array.IndexOf(keyArray, dataOut[1]);

                String[] temp = { dataOut[0], dataOut[1] };
                equippedSpells = temp;

            }

            if (numPressed == 2)
            {
                index1 = Array.IndexOf(keyArray, dataOut[2]);
                index2 = Array.IndexOf(keyArray, dataOut[3]);

                String[] temp = { dataOut[2], dataOut[3] };
                equippedSpells = temp;
            }

            if (numPressed == 3)
            {
                index1 = Array.IndexOf(keyArray, dataOut[4]);
                index2 = Array.IndexOf(keyArray, dataOut[5]);

                String[] temp = { dataOut[4], dataOut[5] };
                equippedSpells = temp;
        }

            if (numPressed == 4)
            {
                index1 = Array.IndexOf(keyArray, dataOut[6]);
                index2 = Array.IndexOf(keyArray, dataOut[7]);

                String[] temp = { dataOut[6], dataOut[7] };
                equippedSpells = temp;
            }

        //these lines........    |    then add the spells image here!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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
            HUDSlot1.sprite = IcicleSpear.image;
        }
        else if (index1 == 4)
        {
            HUDSlot1.sprite = SoundWave.image;
        }
        else if (index1 == 5)
        {
            HUDSlot1.sprite = Boulder.image;
        }
        else if (index1 == 6)
        {
            HUDSlot1.sprite = Earth.image;
        }
        else if (index1 == 7)
        {
            HUDSlot1.sprite = Wind.image;
        }
        else if (index1 == 8)
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
            HUDSlot2.sprite = IcicleSpear.image;
        }
        else if (index2 == 4)
        {
            HUDSlot2.sprite = SoundWave.image;
        }
        else if (index2 == 5)
        {
            HUDSlot2.sprite = Boulder.image;
        }
        else if (index2 == 6)
        {
            HUDSlot2.sprite = Earth.image;
        }
        else if (index2 == 7)
        {
            HUDSlot2.sprite = Wind.image;
        }
        else if (index2 == 8)
        {
            
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
