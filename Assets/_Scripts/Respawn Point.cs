using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnPoint : MonoBehaviour
{
    public GameObject player;
    public GameObject respawnPoint;

    public static string currentCheckpointName;
    public static string currentCheckpointSceneName;

    public void Start()
    {
        player = Alpha.PlayerRef;
        /*
        if(currentCheckpointName != default)
        {
            this.transform.position = GameObject.Find(currentCheckpointName).transform.position;
            GameObject createdPlayer = Instantiate(player, this.gameObject.transform.position, this.gameObject.transform.rotation);
            createdPlayer.SetActive(true);
            Debug.Log(createdPlayer);
        }
        Debug.Log("checky " + currentCheckpointName);
        Debug.Log("scene " + currentCheckpointSceneName);
        */
    }
    public void RespawnPlayer()
    {
        //player.transform.position = respawnPoint.transform.position;
        SceneManager.LoadScene(currentCheckpointSceneName);
    }
}