using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BigObstacleController : MonoBehaviour
{
    public int track = 1;
    public Transform modelsTrack1;
    public Transform modelsTrack2;
    public Transform modelsTrack3;
    public Transform modelsTrack4;
    public MeshRenderer exampleModel;
    public float dodgeBoost;
    public int dodgePoints;

    private void Start()
    {
        if (exampleModel != null)
        {
            exampleModel.enabled = false;
        }
        switch (SceneManager.GetActiveScene().name)
        {
            case "Circuito2":
                if (modelsTrack2.childCount< 1)
                {
                    modelsTrack1.GetChild(Random.Range(0, modelsTrack1.childCount)).gameObject.SetActive(true);
                    break;
                }
                modelsTrack2.GetChild(Random.Range(0, modelsTrack2.childCount)).gameObject.SetActive(true);
                break;
            case "Circuito3":
                if (modelsTrack3.childCount < 1)
                {
                    modelsTrack1.GetChild(Random.Range(0, modelsTrack1.childCount)).gameObject.SetActive(true);
                    break;
                }
                modelsTrack3.GetChild(Random.Range(0, modelsTrack3.childCount)).gameObject.SetActive(true);
                break;
            case "Circuito1":
                modelsTrack1.GetChild(Random.Range(0, modelsTrack1.childCount)).gameObject.SetActive(true);
                break;
            default:
                if (modelsTrack4.childCount < 1)
                {
                    modelsTrack1.GetChild(Random.Range(0, modelsTrack1.childCount)).gameObject.SetActive(true);
                    break;
                }
                modelsTrack4.GetChild(Random.Range(0, modelsTrack2.childCount)).gameObject.SetActive(true);
                break;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponentInParent<PlayerController>().DodgeBoost(dodgeBoost, dodgePoints);
        }
    }
}
