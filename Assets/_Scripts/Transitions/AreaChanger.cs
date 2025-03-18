using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaChanger : MonoBehaviour
{
    public AreaTransition SceneConnection;

    public string NextScene;

    public Transform EnterPoint;


    public void Start()
    {
        if (SceneConnection == AreaTransition.CurrentTransition)
        {
            FindObjectOfType<Alpha>().transform.position = EnterPoint.position;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            AreaTransition.CurrentTransition = SceneConnection;
            SceneManager.LoadScene(NextScene);
        }

     
    }


}
