using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoController : MonoBehaviour
{
    private string playerName;
    private int headDecorIndex = -1;
    private int bodyDecorIndex = -1;
    private int legsDecorIndex = -1;
    private int skateDecorIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("PlayerInfo").Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void SetPlayerName(string name)
    {
        playerName = name;
    }

    public string GetPlayerName()
    {
        return playerName;
    }

    public void SetHeadDecor(int i)
    {
        headDecorIndex = i;
    }

    public int GetHeadDecor()
    {
        return headDecorIndex; 
    }

    public void SetBodyDecor(int i)
    {
        bodyDecorIndex = i;
    }

    public int GetBodyDecor()
    {
        return bodyDecorIndex;
    }

    public void SetLegsDecor(int i)
    {
        legsDecorIndex = i;
    }

    public int GetLegsDecor()
    {
        return legsDecorIndex;
    }

    public void SetSkateDecor(int i)
    {
        skateDecorIndex = i;
    }

    public int GetSkateDecor()
    {
        return skateDecorIndex;
    }
}
