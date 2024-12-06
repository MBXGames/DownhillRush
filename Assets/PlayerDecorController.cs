using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDecorController : MonoBehaviour
{
    private PlayerInfoController playerInfo;
    private GameObject[] decors;
    public Part parte;
    public enum Part
    {
        Skin,
        Skate
    }
    // Start is called before the first frame update
    void Start()
    {
        decors=new GameObject[transform.childCount];
        for (int i=0; i<decors.Length; i++)
        {
            decors[i]=transform.GetChild(i).gameObject;
        }
        if (GameObject.FindGameObjectWithTag("PlayerInfo") == null)
        {
            return;
        }
        if (GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfoController>() == null)
        {
            return;
        }
        playerInfo = GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfoController>();
        DecorCheck();
    }

    public void DecorCheck()
    {
        foreach (GameObject g in decors)
        {
            g.SetActive(false);
        }
        switch (parte)
        {
            case Part.Skin:
                if (playerInfo.GetSkinDecor() <= 0)
                {
                    decors[0].SetActive(true);
                    return;
                }
                else
                {
                    decors[playerInfo.GetSkinDecor()].SetActive(true);
                }
                break;
            case Part.Skate:
                if (playerInfo.GetSkateDecor() <= 0)
                {
                    decors[0].SetActive(true);
                    return;
                }
                else
                {
                    decors[playerInfo.GetSkateDecor()].SetActive(true);
                }
                break;
        }
    }

    public void decorCheckShop(int n)
    {
        foreach (GameObject g in decors)
        {
            g.SetActive(false);
        }
        switch (parte)
        {
            case Part.Skin:
                if (n <= 0)
                {
                    decors[0].SetActive(true);
                    return;
                }
                else
                {
                    decors[n].SetActive(true);
                }
                break;
            case Part.Skate:
                if (n <= 0)
                {
                    decors[0].SetActive(true);
                    return;
                }
                else
                {
                    decors[n].SetActive(true);
                }
                break;
        }
    }
}
