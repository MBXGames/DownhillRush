using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;

public class Leaderboard : MonoBehaviour
{
    public bool endless;
    [SerializeField]
    private List<TextMeshProUGUI> names;
    [SerializeField]
    private List<TextMeshProUGUI> scores;
    [SerializeField]
    private List<TextMeshProUGUI> times;
    public GameObject uploadButton;

    private string publicLeaderboardKey = "9cdac0c381c53d0a7654826c8ae89dc885b52ac25f9a2e851772bf88cc34ccd2";
    private string publicLeaderboardKeyEndless = "29867372a5cba62c5934aad8358ff6bbeefacaa2e5a5b789f6440ea316393f3f";

    public void LeaderboardStart()
    {
        GetLeaderboard();
    }

    public void GetLeaderboard()
    {
        string k = publicLeaderboardKey;
        if (endless)
        {
            k= publicLeaderboardKeyEndless;
        }
        LeaderboardCreator.GetLeaderboard(k, ((msg) =>
        {
            int looplenght = (msg.Length < names.Count) ? msg.Length : names.Count;
            for (int i = 0; i < looplenght; ++i)
            {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
                times[i].text = msg[i].Extra;
            }
            if (uploadButton!=null)
            {
                uploadButton.SetActive(true);
            }
        }));
    }

    public void SetLeaderboardEntry(string username, int score, string time)
    {
        string aux;
        string k = publicLeaderboardKey;
        if (endless)
        {
            k = publicLeaderboardKeyEndless;
        }
        if (username.Length > 6)
        {
            aux = username.Substring(0, 6);
        }
        else
        {
            aux = username;
        }
        LeaderboardCreator.UploadNewEntry(k, aux, score, time, ((_) => 
        {
            GetLeaderboard();
        }));
        LeaderboardCreator.ResetPlayer();
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

    public void SetEndless()
    {
        endless = true;
    }
}
