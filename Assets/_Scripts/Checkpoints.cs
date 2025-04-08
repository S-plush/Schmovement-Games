using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoints : MonoBehaviour
{
    private RespawnPoint respawn;

    private MiscDataToFile MiscDataToFileScript;

    private void Awake()
    {
        respawn = GameObject.FindGameObjectWithTag("Respawn Point").GetComponent<RespawnPoint>();

        MiscDataToFileScript = FindObjectOfType<MiscDataToFile>(); //initilize MiscDataToFileScript with the actual script
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            respawn.respawnPoint = this.gameObject;

            MiscDataToFileScript.saveAllMiscData();
            MiscDataToFileScript.loadAllMiscData();
        }
    }
}
