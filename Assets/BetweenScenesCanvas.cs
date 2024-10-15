using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BetweenScenesCanvas : MonoBehaviour
{
    private int sceneCount;
    public string[] scenesNames;
    public string[] trackNames;
    public float[] trackTimes;
    public int[] trackPoints;
    public GameObject nextbutton;
    public GameObject endbutton;
    // Start is called before the first frame update
    void Start()
    {
        trackTimes = new float[scenesNames.Length];
        trackPoints = new int[scenesNames.Length];
        DontDestroyOnLoad(gameObject);
    }

    public void ShowNextButton()
    {
        if (!nextbutton.activeSelf) 
        { 
            nextbutton.SetActive(true);
        }
    }

    public void ShowEndButton()
    {
        if (!endbutton.activeSelf)
        {
            endbutton.SetActive(true);
        }
    }

    public void ResetCanvas()
    {
        if (nextbutton.activeSelf)
        {
            nextbutton.SetActive(false);
        }
        if (endbutton.activeSelf)
        {
            endbutton.SetActive(false);
        }
    }

    public void StoreData(float time,int points)
    {
        trackTimes[sceneCount] = time;
        trackPoints[sceneCount] = points;
        sceneCount++;
        if(sceneCount> scenesNames.Length)
        {
            ShowEndButton();
        }
        else
        {
            ShowNextButton();
        }
    }

    public void NextButton()
    {
        SceneManager.LoadScene(scenesNames[sceneCount]);
    }

    public void EndButton()
    {
        SceneManager.LoadScene("Results");
    }
}   
