using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void InstructionsButton()
    {
        SceneManager.LoadScene(2);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
