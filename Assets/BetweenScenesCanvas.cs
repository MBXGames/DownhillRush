using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class BetweenScenesCanvas : MonoBehaviour
{
    public GameObject timeText;
    public GameObject pointsText;
    private int sceneCount;
    public string[] scenesNames;
    public string[] trackNames;
    public float[] trackTimes;
    public int[] trackPoints;
    public GameObject nextbutton;
    public GameObject endbutton;
    public GameObject resultsTableImage;
    public Transform[] resultsTableInfos;
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
        if (!timeText.activeSelf)
        {
            timeText.SetActive(true);
        }
        if (!pointsText.activeSelf)
        {
            pointsText.SetActive(true);
        }
        if (nextbutton.activeSelf)
        {
            nextbutton.SetActive(false);
        }
        if (endbutton.activeSelf)
        {
            endbutton.SetActive(false);
        }
        if (resultsTableImage.activeSelf)
        {
            resultsTableImage.SetActive(false);
        }
    }

    public void StoreData(float time,int points)
    {
        trackTimes[sceneCount] = time;
        trackPoints[sceneCount] = points;
        sceneCount++;
        if(sceneCount >= scenesNames.Length)
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
        ResetCanvas();
    }

    public void EndButton()
    {
        SceneManager.LoadScene("Results");
        ResetCanvas();
    }

    public void ShowResults()
    {
        if (timeText.activeSelf)
        {
            timeText.SetActive(false);
        }
        if (pointsText.activeSelf)
        {
            pointsText.SetActive(false);
        }
        if (!resultsTableImage.activeSelf)
        {
            resultsTableImage.SetActive(true);
        }
        for (int i=0; i < scenesNames.Length; i++)
        {
            resultsTableInfos[i].GetChild(0).GetComponent<TextMeshProUGUI>().text = trackNames[i];
            resultsTableInfos[i].GetChild(1).GetComponent<TextMeshProUGUI>().text = TimeFormat(trackTimes[i]);
            resultsTableInfos[i].GetChild(2).GetComponent<TextMeshProUGUI>().text = trackPoints[i].ToString();
        }
    }

    public string TimeFormat(float t)
    {
        float min = 0;
        string minst = "00";
        float sec = 0;
        string secst = "00";
        if (t >= 60)
        {
            min = Mathf.FloorToInt(t / 60);
            if (min < 10)
            {
                minst = "0" + min.ToString();
            }
            else
            {
                minst = min.ToString();
            }
        }
        sec = t % 60;
        if (sec < 10)
        {
            secst = "0" + sec.ToString("F2");
        }
        else
        {
            secst = sec.ToString("F2");
        }
        return minst + ":" + secst;
    }

    public void Exit()
    {
        Destroy(gameObject);
    }
}   
