using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaChanger : MonoBehaviour
{
    public AreaTransition SceneConnection;

    public string NextScene;

    public Transform EnterPoint;

    public Animator Fade;

    public ChangeSceneAfterFade change;

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
            MiscDataToFileScript.saveAllMiscData(); //saves values associated with the player like stims and health

            Fade.SetTrigger("End");
            AreaTransition.CurrentTransition = SceneConnection;
            change.SceneName = NextScene;
        }
    }

    public void Transition()
    {
        //SceneManager.LoadScene(NextScene);
    }

}
