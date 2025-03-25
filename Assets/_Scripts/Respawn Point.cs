using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public GameObject player;
    public GameObject respawnPoint;

    public void RespawnPlayer()
    {
        player.transform.position = respawnPoint.transform.position;
    }
}