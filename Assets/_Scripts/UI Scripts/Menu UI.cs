using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;

    public void PlayButtonNextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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
