using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoints : MonoBehaviour
{
    private RespawnPoint respawn;

    private MiscDataToFile MiscDataToFileScript;

    private Alpha AlphaScript;

    private void Awake()
    {
        respawn = GameObject.FindGameObjectWithTag("Respawn Point").GetComponent<RespawnPoint>();

        MiscDataToFileScript = FindObjectOfType<MiscDataToFile>(); //initilize MiscDataToFileScript with the actual script
        AlphaScript = FindObjectOfType<Alpha>(); //initilize AlphaScript with the actual script

        AlphaScript.currentCheckpointName = "default";
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            respawn.respawnPoint = this.gameObject;
            //AlphaScript.respawnPoint = respawn;

            MiscDataToFileScript.saveAllMiscData();
            MiscDataToFileScript.loadAllMiscData();

            AlphaScript.currentCheckpointName = this.gameObject.name;
            //Int32.Parse(string.Concat(this.name.Where(Char.IsDigit)));
        }
    }
}
