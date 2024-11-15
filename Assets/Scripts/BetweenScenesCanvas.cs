using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class BetweenScenesCanvas : MonoBehaviour
{
    public bool simulateMobile;
    private PlayerController playerController;
    public GameObject timeText;
    public GameObject pointsText;
    private int sceneCount;
    public string[] scenesNames;
    public string[] trackNames;
    public float[] trackTimes;
    public int[] trackPoints;
    public GameObject nextbutton;
    public GameObject endbutton;
    public GameObject leaderboardbutton;
    public GameObject pausebutton;
    public GameObject pausemenu;
    public GameObject mobilebuttons;
    public GameObject resultsTableImage;
    public Transform[] resultsTableInfos;
    public Transform resultsTableTotalInfo;
    private bool paused;
    [Header("Leaderboard")]
    public GameObject leaderboard;
    public TextMeshProUGUI totalPoints;
    public TextMeshProUGUI totalTime;
    private float tTime;
    private int tPoints;

    // Start is called before the first frame update
    void Start()
    {
        playerController =GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        trackTimes = new float[scenesNames.Length];
        trackPoints = new int[scenesNames.Length];
        DontDestroyOnLoad(gameObject);
        if (Application.isMobilePlatform || simulateMobile)
        {
            if (!mobilebuttons.activeSelf)
            {
                mobilebuttons.SetActive(true);
            }
        }
    }

    private void Update()
    {
        if (playerController == null)
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
        if(!paused && Time.timeScale < 1)
        {
            Time.timeScale +=Time.deltaTime*(1/ Time.timeScale)/3;
            if (Time.timeScale > 0.99f)
            {
                Time.timeScale = 1;
            }
        }
    }

    public void HideMovementsButton()
    {
        if (mobilebuttons.activeSelf)
        {
            mobilebuttons.SetActive(false);
        }
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
        if (!mobilebuttons.activeSelf && (Application.isMobilePlatform || simulateMobile))
        {
            mobilebuttons.SetActive(true);
        }
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
        if (playerController == null)
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
        ResetCanvas();
    }

    public void EndButton()
    {
        SceneManager.LoadScene("ResultsScene");
        ResetCanvas();
        if (timeText.activeSelf)
        {
            timeText.SetActive(false);
        }
        if (pointsText.activeSelf)
        {
            pointsText.SetActive(false);
        }
        if (mobilebuttons.activeSelf)
        {
            mobilebuttons.SetActive(false);
        }
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(scenesNames[0]);
        Exit();
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("MainMenu");
        Exit();
    }

    public void ShowResults()
    {
        if (mobilebuttons.activeSelf)
        {
            mobilebuttons.SetActive(false);
        }
        if (timeText.activeSelf)
        {
            timeText.SetActive(false);
        }
        if (pointsText.activeSelf)
        {
            pointsText.SetActive(false);
        }
        if (!leaderboardbutton.activeSelf)
        {
            leaderboardbutton.SetActive(true);
        }
        if (!resultsTableImage.activeSelf)
        {
            resultsTableImage.SetActive(true);
        }
        tTime = 0;
        tPoints = 0;
        for (int i = 0; i < scenesNames.Length; i++)
        {
            resultsTableInfos[i].GetChild(0).GetComponent<TextMeshProUGUI>().text = trackNames[i];
            resultsTableInfos[i].GetChild(1).GetComponent<TextMeshProUGUI>().text = TimeFormat(trackTimes[i]);
            tTime += trackTimes[i];
            resultsTableInfos[i].GetChild(2).GetComponent<TextMeshProUGUI>().text = trackPoints[i].ToString();
            tPoints += trackPoints[i];
        }
        resultsTableTotalInfo.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Total";
        resultsTableTotalInfo.GetChild(1).GetComponent<TextMeshProUGUI>().text = TimeFormat(tTime);
        resultsTableTotalInfo.GetChild(2).GetComponent<TextMeshProUGUI>().text = tPoints.ToString();
    }

    public void ShowLeaderboard()
    {
        if (mobilebuttons.activeSelf)
        {
            mobilebuttons.SetActive(false);
        }
        if (timeText.activeSelf)
        {
            timeText.SetActive(false);
        }
        if (pointsText.activeSelf)
        {
            pointsText.SetActive(false);
        }
        if (leaderboardbutton.activeSelf)
        {
            leaderboardbutton.SetActive(false);
        }
        if (resultsTableImage.activeSelf)
        {
            resultsTableImage.SetActive(false);
        }
        totalPoints.text = "Puntuación: " + tPoints;
        totalTime.text = "Tiempo: " + TimeFormat(tTime);
        leaderboard.SetActive(true);
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

    public void Pause()
    {
        paused = true;
        Time.timeScale = 0;
        pausebutton.SetActive(false);
        pausemenu.SetActive(true);
    }

    public void UnPause()
    {
        paused = false;
        Time.timeScale = 0.1f;
        pausebutton.SetActive(true);
        pausemenu.SetActive(false);
    }

    public void PauseExit()
    {
        Time.timeScale = 1;
        ExitButton();
    }

    public void Exit()
    {
        Destroy(gameObject);
    }

    public void MovementRightOn()
    {
        if (playerController == null) { return; }
        playerController.RightOn();
    }
    public void MovementRightOff()
    {
        if (playerController == null) { return; }
        playerController.RightOff();
    }

    public void MovementLeftOn()
    {
        if (playerController == null) { return; }
        playerController.LeftOn();
    }
    public void MovementLeftOff()
    {
        if (playerController == null) { return; }
        playerController.LeftOff();
    }

    public void MovementJump()
    {
        if (playerController == null) { return; }
        playerController.Jump();
    }

    public void MovementCrouch()
    {
        if (playerController == null) { return; }
        playerController.Crouch();
    }

    public void MovementUncrouch()
    {
        if (playerController == null) { return; }
        playerController.Uncrouch();
    }
}   
