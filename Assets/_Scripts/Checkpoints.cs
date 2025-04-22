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

    private Animator animator;
    private void Start()
    {
        respawn = GameObject.FindGameObjectWithTag("Respawn Point").GetComponent<RespawnPoint>();

        MiscDataToFileScript = FindObjectOfType<MiscDataToFile>(); //initilize MiscDataToFileScript with the actual script
        AlphaScript = FindObjectOfType<Alpha>(); //initilize AlphaScript with the actual script

        //AlphaScript.currentCheckpointName = "default";

        animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            respawn.respawnPoint = this.gameObject;
            //AlphaScript.respawnPointObj = this.gameObject;
            //AlphaScript.respawnPoint = respawn;

            AlphaScript.currentCheckpointName = this.gameObject.name;
            MiscDataToFileScript.saveAllMiscData();
            MiscDataToFileScript.loadAllMiscData();

            //Int32.Parse(string.Concat(this.name.Where(Char.IsDigit)));

            if (animator != null)
            {
                animator.SetBool("Hit", true);
            }
        }
    }
}
