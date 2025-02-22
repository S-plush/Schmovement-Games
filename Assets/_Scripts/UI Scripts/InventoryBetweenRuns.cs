using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class InventoryData
{
    public List<string> items = new List<string>();
}

public class InventoryBetweenRuns : MonoBehaviour
{
    InventoryData inventory = new InventoryData { items = new List<string> { "Explosion", "Lightning",} }; //but read these in order they appear from the game...

    public void SaveInventory()
    {
        string json = JsonUtility.ToJson(inventory);
        File.WriteAllText(Application.persistentDataPath + "/inventory.json", json);
    }

    public void LoadInventory()
    {
        string path = Application.persistentDataPath + "/inventory.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            inventory = JsonUtility.FromJson<InventoryData>(json);

            foreach (string item in inventory.items)
            {
                Debug.Log("Loaded Item: " + item);
            }
        }
    }
}
