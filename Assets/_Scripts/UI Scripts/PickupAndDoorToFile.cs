using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class PickupAndDoorToFile : MonoBehaviour
{
    [Header("File")]
    public string fileName = "PickupData.txt";
    private string filePath;

    [Header("Scene & Objects (assign in inspector)")]
    public List<string> scenes = new List<string>();
    public List<GameObject> pickups = new List<GameObject>();                 // inspector references for pickups
    public List<Doors> doors = new List<Doors>();                            // inspector references for doors (component)
    public List<MovingPlatform> movingPlatforms = new List<MovingPlatform>(); // inspector references for platforms

    // Internal name dictionaries / saved state
    private List<string> pickupNames = new List<string>();            // same order as pickups
    [HideInInspector] public List<string> claimedPickups = new List<string>();      // per-current-scene collected pickups
    [HideInInspector] public List<string> claimedDoors = new List<string>();        // names of doors unlocked in current scene
    [HideInInspector] public List<string> claimedPlatforms = new List<string>();    // names of platforms activated in current scene

    void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, fileName);
        // Build stable name list (trimmed) that corresponds to pickups indices
        pickupNames.Clear();
        for (int i = 0; i < pickups.Count; i++)
        {
            if (pickups[i] != null)
                pickupNames.Add(pickups[i].name.Trim());
            else
                pickupNames.Add("UnknownPickupIndex" + i);
        }

        // Ensure the file exists (create minimal structure if needed)
        if (!File.Exists(filePath))
        {
            Debug.Log("Pickup save file not found -- creating new file at: " + filePath);
            // initialize scenes list with current scene if available
            string current = GetCurrentSceneName();
            if (!string.IsNullOrEmpty(current) && !scenes.Contains(current))
                scenes.Add(current);
            SaveAllPickupData(); // writes initial file
        }
    }

    void Start()
    {
        if (MiscDataToFile.newGame == false)
        {
            LoadAllPickupData();
        }

        // Always save once after start to make sure file matches in-memory lists/format
        SaveAllPickupData();
    }

    void Update()
    {
        // Detect destroyed pickups (simple polling approach; you can call MarkPickupCollected() from pickup on destroy to be more efficient)
        for (int i = 0; i < pickups.Count; i++)
        {
            if (pickups[i] == null) // object was destroyed
            {
                string pname = pickupNames[i];
                if (!string.IsNullOrEmpty(pname) && !claimedPickups.Contains(pname))
                {
                    claimedPickups.Add(pname);
                    SaveAllPickupData();
                }
            }
        }

        // Doors & platforms: gather currently unlocked/activated ones into lists (avoid duplicating entries)
        bool anyDoorChange = false;
        for (int i = 0; i < doors.Count; i++)
        {
            if (doors[i] == null) continue;
            if (doors[i].notLockedDoor)
            {
                string dname = doors[i].name.Trim();
                if (!claimedDoors.Contains(dname))
                {
                    claimedDoors.Add(dname);
                    anyDoorChange = true;
                }
            }
        }

        bool anyPlatformChange = false;
        for (int i = 0; i < movingPlatforms.Count; i++)
        {
            if (movingPlatforms[i] == null) continue;
            if (movingPlatforms[i].fileActivation)
            {
                string pname = movingPlatforms[i].name.Trim();
                if (!claimedPlatforms.Contains(pname))
                {
                    claimedPlatforms.Add(pname);
                    anyPlatformChange = true;
                }
            }
        }

        if (anyDoorChange || anyPlatformChange)
            SaveAllPickupData();
    }

    // Public helper to mark pickup collected by name (useful if pickups call this on pickup)
    public void MarkPickupCollected(string pickupName)
    {
        if (string.IsNullOrEmpty(pickupName)) return;
        string p = pickupName.Trim();
        if (!claimedPickups.Contains(p))
        {
            claimedPickups.Add(p);
            SaveAllPickupData();
        }
    }

    // Save all data: header line = scenes CSV, then for each scene 3 lines (pickups, doors, platforms)
    public void SaveAllPickupData()
    {
        // Read existing lines if present (to preserve other scenes)
        List<string> lines = new List<string>();
        if (File.Exists(filePath))
            lines.AddRange(File.ReadAllLines(filePath));
        else
            lines.Add(""); // ensure at least one line for header

        string currentScene = GetCurrentSceneName();
        if (string.IsNullOrEmpty(currentScene))
        {
            Debug.LogWarning("Current scene name is empty — cannot save scene-specific data.");
            return;
        }

        // Ensure scenes list contains current scene (store trimmed)
        currentScene = currentScene.Trim();
        // Merge existing header scenes if file had them and in-memory scenes empty
        if ((lines.Count > 0) && !string.IsNullOrEmpty(lines[0]) && scenes.Count == 0)
        {
            var fromFile = lines[0].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                   .Select(s => s.Trim()).ToList();
            scenes = fromFile;
        }

        if (!scenes.Contains(currentScene))
            scenes.Add(currentScene);

        // Build header line
        lines[0] = string.Join(",", scenes.Select(s => s.Trim()));

        // Ensure file has enough lines: header + 3 lines per scene
        int requiredLines = 1 + scenes.Count * 3;
        while (lines.Count < requiredLines)
            lines.Add("");

        int sceneIndex = scenes.IndexOf(currentScene);
        int baseIndex = 1 + sceneIndex * 3; // pickups line, doors line, platforms line

        lines[baseIndex] = string.Join(",", claimedPickups.Select(x => x.Trim()));
        lines[baseIndex + 1] = string.Join(",", claimedDoors.Select(x => x.Trim()));
        lines[baseIndex + 2] = string.Join(",", claimedPlatforms.Select(x => x.Trim()));

        File.WriteAllLines(filePath, lines);
    }

    public void LoadAllPickupData()
    {
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("Save file does not exist when attempting load.");
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length == 0)
            return;

        // Parse scenes from header
        scenes = lines[0].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                         .Select(s => s.Trim()).ToList();

        string currentScene = GetCurrentSceneName();
        if (string.IsNullOrEmpty(currentScene) || !scenes.Contains(currentScene.Trim()))
            return; // nothing to load for this scene

        currentScene = currentScene.Trim();
        int sceneIndex = scenes.IndexOf(currentScene);
        int baseIndex = 1 + sceneIndex * 3;

        // Safety checks for array bounds
        if (baseIndex >= lines.Length) return;

        // LOAD claimed pickups
        claimedPickups = lines[baseIndex]
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim()).ToList();

        // Destroy pickups listed in claimedPickups
        foreach (var pname in claimedPickups)
        {
            if (string.IsNullOrEmpty(pname)) continue;
            string trimmed = pname.Trim();

            // try find in the pickupNames list first
            int idx = pickupNames.IndexOf(trimmed);
            GameObject goToDestroy = null;
            if (idx >= 0 && idx < pickups.Count)
            {
                goToDestroy = pickups[idx];
            }
            // fallback to scene find by name
            if (goToDestroy == null)
            {
                goToDestroy = GameObject.Find(trimmed);
            }
            if (goToDestroy != null)
            {
                Destroy(goToDestroy);
            }
            else
            {
                Debug.LogWarning("Couldn't find pickup to destroy: " + trimmed);
            }
        }

        // LOAD claimed doors
        if (baseIndex + 1 < lines.Length)
        {
            claimedDoors = lines[baseIndex + 1]
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim()).ToList();

            foreach (var dname in claimedDoors)
            {
                if (string.IsNullOrEmpty(dname)) continue;
                var doorObj = doors.Find(d => d != null && d.name.Trim() == dname.Trim());
                if (doorObj != null)
                {
                    doorObj.notLockedDoor = true;
                }
                else
                {
                    Debug.LogWarning("Couldn't find door to unlock: " + dname);
                }
            }
        }

        // LOAD claimed platforms
        if (baseIndex + 2 < lines.Length)
        {
            claimedPlatforms = lines[baseIndex + 2]
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim()).ToList();

            foreach (var pname in claimedPlatforms)
            {
                if (string.IsNullOrEmpty(pname)) continue;
                var plat = movingPlatforms.Find(p => p != null && p.name.Trim() == pname.Trim());
                if (plat != null)
                {
                    plat.fileActivation = true;
                }
                else
                {
                    Debug.LogWarning("Couldn't find platform to activate: " + pname);
                }
            }
        }
    }

    private string GetCurrentSceneName()
    {
        // Use Alpha.PlayerRef.scene.name as your code did; fallback to SceneManager if null
        try
        {
            if (Alpha.PlayerRef != null && Alpha.PlayerRef.scene != null)
                return Alpha.PlayerRef.scene.name.Trim();
        }
        catch { }

        try
        {
            return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Trim();
        }
        catch { }

        return string.Empty;
    }

    void OnApplicationQuit()
    {
        SaveAllPickupData();
    }
}