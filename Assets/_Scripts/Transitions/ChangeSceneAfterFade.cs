using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneAfterFade : MonoBehaviour
{

    public string SceneName;

    private void Start()
    {
        SceneName = "";

    }


    public void SceneChange()
    {

        SceneManager.LoadScene(SceneName);

    }



}
