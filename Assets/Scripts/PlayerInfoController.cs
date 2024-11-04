using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoController : MonoBehaviour
{
    private string playerName;
    private int headDecorIndex;
    private int neckDecorIndex;
    private int bodyDecorIndex;
    private int legsDecorIndex;
    private int skateDecorIndex;

    // Start is called before the first frame update
    void Start()
    {
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

    public void SetNeckDecor(int i)
    {
        neckDecorIndex = i;
    }

    public int GetNeckDecor()
    {
        return neckDecorIndex;
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
