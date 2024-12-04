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
        Head,
        Body,
        Legs,
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
        foreach(GameObject g in decors)
        {
            g.SetActive(false);
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
        switch (parte)
        {
            case Part.Head:
                if (playerInfo.GetHeadDecor() <= 0) return;
                decors[playerInfo.GetHeadDecor()].SetActive(true);
                break;
            case Part.Body:
                if (playerInfo.GetBodyDecor() <= 0) return;
                decors[playerInfo.GetBodyDecor()].SetActive(true);
                break;
            case Part.Legs:
                if (playerInfo.GetLegsDecor() <= 0) return;
                decors[playerInfo.GetLegsDecor()].SetActive(true);
                break;
            case Part.Skate:
                if (playerInfo.GetSkateDecor() <= 0)
                {
                    decors[0].SetActive(true);
                    return;
                }
                decors[playerInfo.GetSkateDecor()].SetActive(true);
                break;
        }
    }
}
