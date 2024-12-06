using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoController : MonoBehaviour
{
    [SerializeField]private int skinIndex = 0;
    [SerializeField]private int skateDecorIndex = 0;
    public List<int> unlockedSkins;
    public List<int> unlockedSkates;

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

    public void SetSkinDecor(int i)
    {
        skinIndex = i;
    }

    public int GetSkinDecor()
    {
        return skinIndex;
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
