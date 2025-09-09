using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaChanger : MonoBehaviour
{
    public AreaTransition SceneConnection;

    public string NextScene;

    public Transform EnterPoint;

    MiscDataToFile MiscDataToFileScript;

    Alpha AlphaScript;

    public void Start()
    {
        MiscDataToFileScript = FindObjectOfType<MiscDataToFile>(); //initilize MiscDataToFileScript with the actual script
        AlphaScript = FindObjectOfType<Alpha>();

        if (SceneConnection == AreaTransition.CurrentTransition)
        {
            FindObjectOfType<Alpha>().transform.position = EnterPoint.position;
        }
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            RespawnPoint.currentCheckpointName = "point 1"; //////////////////////////////////////////////////////BECOME WAY MORE COMPLEX FOR DIFF TRANSITIONS BASE ON THIS OBJ NAME
            MiscDataToFileScript.saveAllMiscData(); //saves values associated with the player like stims and health

            AreaTransition.CurrentTransition = SceneConnection;            
            SceneManager.LoadScene(NextScene);
        }

     
    }


}
