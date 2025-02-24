using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;
using UnityEngine.tvOS;

[System.Serializable]
public class InventoryData
{
    public List<string> items = new List<string>();
}

public class InvDataBetweenRuns : MonoBehaviour
{
    public GameObject LightningPrefab;
    public GameObject ExplosionPrefab; //add new scriptable objects for spells here !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    public Spell LightningScriptable;
    public Spell ExplosionScriptable; //add new scritpable objects for spells here !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    public List<GameObject> allInvSlots;

    InventoryData inventory; //stores state of each inventory slot to be sent to file

public void Update() // TEMPORARY, NEEDS TO BE REMOVED
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            SaveInventory();
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            LoadInventory();
        }
    }

    public void SaveInventory() //saves the state of the whole inventory to a file
    {
        inventory = new InventoryData { items = new List<string> { } }; //stores state of each inventory slot to be sent to file

        foreach (GameObject inv in allInvSlots)
        {
            if(inv.transform.childCount > 0)
            {
                inventory.items.Add(inv.transform.GetChild(0).gameObject.GetComponent<InventorySpell>().spell.name);
                //Destroy(inv.transform.GetChild(0).gameObject); // testing TEMPORARY NEEDS TO BE REMOVED /testing TEMPORARY NEEDS TO BE REMOVED /testing TEMPORARY NEEDS TO BE REMOVED /testing TEMPORARY NEEDS TO BE REMOVED
            }
            else
            {
                inventory.items.Add("empty");
            }
        }

        string json = JsonUtility.ToJson(inventory);
        File.WriteAllText(Application.persistentDataPath + "/inventory.json", json);
    }

    public void LoadInventory() //loads the state of the whole inventory from a file
    {
        string path = Application.persistentDataPath + "/inventory.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            inventory = JsonUtility.FromJson<InventoryData>(json);


            

            int i = 0; //keeps track of which inventory slot in allInvSlots is currently being considered
            foreach (string item in inventory.items)
            {

                if (allInvSlots[i].transform.childCount > 0)
                {
                    Destroy(allInvSlots[i].transform.GetChild(0).gameObject);
                }

                    //Debug.Log(allInvSlots.Count);

                //Debug.Log("Loaded Item: " + item); //REMOVE

                if(item != "empty")
                {
                    if (item == "Lightning") //add new spells as new if statements here!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    {
                        //Instantiate(LightningPrefab).transform.SetParent(allInvSlots[i].transform, false);
                        //LightningPrefab.GetComponent<InventorySpell>().InitialiseSpell(LightningScriptable);
                        this.GetComponent<InventoryManager>().SpawnNewSpell(LightningScriptable, allInvSlots[i].ConvertTo<InventorySlot>());
                    }
                    else if (item == "Explosion")
                    {
                        //Instantiate(ExplosionPrefab).transform.SetParent(allInvSlots[i].transform, false);
                        //ExplosionPrefab.GetComponent<InventorySpell>().InitialiseSpell(ExplosionScriptable);
                        this.GetComponent<InventoryManager>().SpawnNewSpell(ExplosionScriptable, allInvSlots[i].ConvertTo<InventorySlot>());
                    }
                }
                i++; // increments i after every item from the file has been processed
            }
        }
    }
}