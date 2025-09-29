using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VCamera : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        player = Alpha.PlayerRef;
        virtualCamera.Follow = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
