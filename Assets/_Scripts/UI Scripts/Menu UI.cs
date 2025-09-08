using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MenuUI : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;

    public void PlayButtonResume()
    {
        try
        {
            //Debug.Log(Alpha.currentSceneName);
            MiscDataToFile.newGame = false;

            SceneManager.LoadScene(Alpha.currentSceneName);
        }
        catch (Exception ex)
        {
            Debug.Log("bigissue");
            SceneManager.LoadScene("DetentionCenter");
            MiscDataToFile.newGame = false;
        }
    }

    public void PlayButtonNew()
    {
        MiscDataToFile.newGame = true;
        //SceneManager.LoadScene("DetentionCenter");
        SceneManager.LoadScene("Opening Cutscene");
    }

    public void SettingsButton()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void BackButton()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
