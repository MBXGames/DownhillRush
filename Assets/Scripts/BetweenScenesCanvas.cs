using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class BetweenScenesCanvas : MonoBehaviour
{
    public bool simulateMobile;
    private PlayerController playerController;
    private Rigidbody playerRb;
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
    [Header ("Endless")]
    public bool endless;
    private float tInit;
    private float tUnder;
    public float minSpeed;
    public float minSpeedIncrease;
    public float underSpeedMaxTime;
    public TextMeshProUGUI minSpeedText;
    public TextMeshProUGUI playerSpeedText;
    [Header("Leaderboard")]
    public GameObject leaderboard;
    public TextMeshProUGUI totalPoints;
    public TextMeshProUGUI totalTime;
    private float tTime;
    private int tPoints;
    private bool ended;

    // Start is called before the first frame update
    void Start()
    {
        playerController =GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerRb =playerController.gameObject.GetComponent<Rigidbody>();
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
        if ((Application.isMobilePlatform || simulateMobile))
        {
            if((paused || ended) && mobilebuttons.activeSelf)
            {
                mobilebuttons.SetActive(false);
            }
            else if(!(paused || ended) && !mobilebuttons.activeSelf)
            {
                mobilebuttons.SetActive(true);
            }
        }
        else if(mobilebuttons.activeSelf)
        {
            mobilebuttons.SetActive(false);
        }
        if (!paused && Time.timeScale < 1)
        {
            Time.timeScale +=Time.deltaTime*(1/ Time.timeScale)/3;
            if (Time.timeScale > 0.99f)
            {
                Time.timeScale = 1;
            }
        }

        if(endless && Time.timeScale == 1 && !ended)
        {
            playerSpeedText.text = playerRb.velocity.z.ToString("F2");
            minSpeedText.text = minSpeed.ToString("F2");
            if (tInit < 1)
            {
                tInit += Time.deltaTime;
            }
            else
            {
                minSpeed += Time.deltaTime * minSpeedIncrease;
                if (playerRb.velocity.z < minSpeed)
                {
                    if(tUnder < underSpeedMaxTime)
                    {
                        playerSpeedText.color = Color.Lerp(Color.white, Color.red, tUnder / underSpeedMaxTime);
                        tUnder += Time.deltaTime;
                    }
                    else if(!ended)
                    {
                        ended = true;
                        playerController.PlayerEnd();
                    }
                }
                else
                {
                    playerSpeedText.color=Color.white;
                    tUnder = 0;
                }
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
        if(sceneCount >= scenesNames.Length || endless)
        {
            ShowEndButton();
            if (endless){
                EndButton();
            }
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
        if(endless)
        {
            SceneManager.LoadScene("ResultsSceneEndless");
        }
        else
        {
            SceneManager.LoadScene("ResultsScene");
        }
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
        if (minSpeedText.gameObject.activeSelf)
        {
            minSpeedText.gameObject.SetActive(false);
        }
        if (playerSpeedText.gameObject.activeSelf)
        {
            playerSpeedText.gameObject.SetActive(false);
        }
    }

    public void RestartButton()
    {
        if (endless)
        {
            SceneManager.LoadScene("EndlessMode");
        }
        else
        {
            SceneManager.LoadScene(scenesNames[0]);
        }
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
        if (endless)
        {
            resultsTableTotalInfo.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Puntuación";
            resultsTableTotalInfo.GetChild(1).GetComponent<TextMeshProUGUI>().text = TimeFormat(trackTimes[0]);
            resultsTableTotalInfo.GetChild(2).GetComponent<TextMeshProUGUI>().text = trackPoints[0].ToString();
            tTime = trackTimes[0];
            tPoints = trackPoints[0];
        }
        else
        {
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
        if(endless)
        {
            leaderboard.GetComponent<Leaderboard>().SetEndless();
        }
        leaderboard.GetComponent<Leaderboard>().LeaderboardStart();
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
