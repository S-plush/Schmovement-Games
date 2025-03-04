using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoints : MonoBehaviour
{
    private RespawnPoint respawn;

    private void Awake()
    {
        respawn = GameObject.FindGameObjectWithTag("Respawn Point").GetComponent<RespawnPoint>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            respawn.respawnPoint = this.gameObject;
        }
    }
}
