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

    public bool NoHeal;

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
            respawn.respawnPoint.transform.position = this.gameObject.transform.position;
            //AlphaScript.respawnPointObj = this.gameObject;
            //AlphaScript.respawnPoint = respawn;

            RespawnPoint.currentCheckpointName = this.gameObject.name;
            RespawnPoint.currentCheckpointSceneName = SceneManager.GetActiveScene().name;
            MiscDataToFileScript.saveAllMiscData();

            //Int32.Parse(string.Concat(this.name.Where(Char.IsDigit)));

            if (animator != null)
            {
                animator.SetBool("Hit", true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(!NoHeal) //if not a noHeal checkpoint (transitions)
        {
            MiscDataToFileScript.loadAllMiscData();
        }
    }

    private void OnTriggerStay(Collider other) //can reload scene by interacting with checkpoints
    {
        if (other.gameObject.CompareTag("Player") && !NoHeal) //and if not a noHeal checkpoint (transitions)
        {
            if (Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene(RespawnPoint.currentCheckpointSceneName);
            }
        }
    }
}
