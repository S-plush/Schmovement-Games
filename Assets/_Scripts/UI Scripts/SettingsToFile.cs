using System;
using System.IO;
using System.Globalization;
using UnityEngine;

public class SettingsToFile : MonoBehaviour
{
    private string fileName = "SettingsData.txt";
    private string filePath;

    // Public variables (no struct) to store settings
    public int currentResolutionIndex = 2; // default: 1920x1080
    public float currentBrightness = 0f;
    public float currentVolume = 0f;

    void Start()
    {
        // Set the file path inside persistentDataPath
        filePath = Path.Combine(Application.persistentDataPath, fileName);

        // Load saved values (if any) into the public variables
        LoadSettings();
    }

    // Saves the public variables to file (three lines)
    public void SaveSettings()
    {
        string dataOut =
            currentResolutionIndex.ToString(CultureInfo.InvariantCulture) + "\n" +
            currentBrightness.ToString(CultureInfo.InvariantCulture) + "\n" +
            currentVolume.ToString(CultureInfo.InvariantCulture);

        WriteToFile(dataOut);
    }

    // Loads values from file and writes them into the public variables
    // If parsing fails or file missing, public variables keep their default values
    public void LoadSettings()
    {
        string fileText = ReadFromFile();
        if (string.IsNullOrEmpty(fileText)) return;

        string[] parts = fileText.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length >= 1)
        {
            int parsedInt;
            if (int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedInt))
                currentResolutionIndex = parsedInt;
        }

        if (parts.Length >= 2)
        {
            float parsedFloat;
            if (float.TryParse(parts[1], NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out parsedFloat))
                currentBrightness = parsedFloat;
        }

        if (parts.Length >= 3)
        {
            float parsedFloat;
            if (float.TryParse(parts[2], NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out parsedFloat))
                currentVolume = parsedFloat;
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
            //Debug.LogWarning("File not found!");
            return "";
        }
    }

    private void OnApplicationQuit()
    {
        SaveSettings();
    }
}
