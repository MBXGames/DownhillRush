using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddSkipController : MonoBehaviour
{
    public GameObject skipButton;
    public TextMeshProUGUI timeText;
    public int time;
    private float t;
    // Start is called before the first frame update
    void Start()
    {
        skipButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(t<time)
        {
            t += Time.deltaTime;
            timeText.text = (time-t).ToString("F0")+" s";
        }
        else
        {
            timeText.text = "Saltar";
            if (!skipButton.activeSelf)
            {
                skipButton.SetActive(true);
            }
        }
    }

    public void SkipToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SkipToGame()
    {
        SceneManager.LoadScene("Circuito1");
    }

    public void SkipToEndless()
    {
        SceneManager.LoadScene("EndlessMode");
    }
}
