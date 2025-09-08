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
            AlphaScript.transitioned = true; //tells alpha script not to try to spawn the player at a checkpoint
            FindObjectOfType<Alpha>().transform.position = EnterPoint.position;
        }
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<Alpha>().currentCheckpointName = "default";
            MiscDataToFileScript.saveAllMiscData(); //saves values associated with the player like stims and health

            AreaTransition.CurrentTransition = SceneConnection;            
            SceneManager.LoadScene(NextScene);
        }

     
    }


}
