using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("Circuito1");
    }

    public void StartEndlessButton()
    {
        SceneManager.LoadScene("EndlessMode");
    }
}
