using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;

public class Leaderboard : MonoBehaviour
{
    public bool endless;
    private LeaderboardReference myLeaderboard;
    [SerializeField]
    private List<TextMeshProUGUI> names;
    [SerializeField]
    private List<TextMeshProUGUI> scores;
    [SerializeField]
    private List<TextMeshProUGUI> times;

    private string publicLeaderboardKey = "934ce0b9ffba1fded7ba3edaee3ed380343a9ce920ca27c5731a2a1ea57d768e";
    private string publicLeaderboardKeyEndless = "28674e04246c5999e8066806c8dbdc95d54b944319f7c4de0493701b3e831e42";

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
        }));
    }

    public void SetLeaderboardEntry(string username, int score, string time)
    {
        string k = publicLeaderboardKey;
        if (endless)
        {
            k = publicLeaderboardKeyEndless;
        }
        LeaderboardCreator.UploadNewEntry(k, username, score, time, ((_) => 
        {
            if (username.Length > 16)
            {
                username.Substring(0, 16);
            }
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
