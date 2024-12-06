using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopMenuController : MonoBehaviour
{
    private PlayerInfoController playerInfo;
    public int menuSelectedSkin;
    public int menuSelectedSkate;
    public int maxSkin;
    public int maxSkate;
    public GameObject skinLockedButton;
    public GameObject skateLockedButton;
    public GameObject skinSelecteButton;
    public GameObject skateSelecteButton;
    public PlayerDecorController skin;
    public PlayerDecorController skate;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("PlayerInfo") == null)
        {
            return;
        }
        if (GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfoController>() == null)
        {
            return;
        }
        playerInfo = GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerInfoController>();

        menuSelectedSkin = playerInfo.GetSkinDecor();

        menuSelectedSkate = playerInfo.GetSkateDecor();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInfo.unlockedSkins.Contains(menuSelectedSkin))
        {
            skinLockedButton.SetActive(false);
            if (playerInfo.GetSkinDecor() == menuSelectedSkin)
            {
                skinSelecteButton.SetActive(false);
            }
            else
            {
                skinSelecteButton.SetActive(true);
            }
        }
        else
        {
            skinLockedButton.SetActive(true);
        }
        if (playerInfo.unlockedSkates.Contains(menuSelectedSkate))
        {
            skateLockedButton.SetActive(false);
            if (playerInfo.GetSkateDecor() == menuSelectedSkate)
            {
                skateSelecteButton.SetActive(false);
            }
            else
            {
                skateSelecteButton.SetActive(true);
            }
        }
        else
        {
            skateLockedButton.SetActive(true);
        }
    }

    public void SkinMove(int n)
    {
        menuSelectedSkin += n;
        if (menuSelectedSkin < 0)
        {
            menuSelectedSkin = maxSkin;
        }
        else if (menuSelectedSkin > maxSkin)
        {
            menuSelectedSkin = 0;
        }
        skin.decorCheckShop(menuSelectedSkin);
    }
    public void SkateMove(int n)
    {
        menuSelectedSkate += n;
        if (menuSelectedSkate < 0)
        {
            menuSelectedSkate = maxSkate;
        }
        else if (menuSelectedSkate > maxSkate)
        {
            menuSelectedSkate = 0;
        }
        skate.decorCheckShop(menuSelectedSkate);
    }

    public void SkinSelect()
    {
        playerInfo.SetSkinDecor(menuSelectedSkin);
    }

    public void SkateSelect()
    {
        playerInfo.SetSkateDecor(menuSelectedSkate);
    }

    public void SkinUnlock()
    {
        playerInfo.unlockedSkins.Add(menuSelectedSkin);
    }

    public void SkateUnlock()
    {
        playerInfo.unlockedSkates.Add(menuSelectedSkate);
    }

    public void BackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
