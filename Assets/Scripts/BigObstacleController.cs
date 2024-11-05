using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigObstacleController : MonoBehaviour
{
    public int track = 1;
    public GameObject[] modelsTrack1;
    public GameObject[] modelsTrack2;
    public GameObject[] modelsTrack3;
    public MeshRenderer exampleModel;
    public float dodgeBoost;
    public int dodgePoints;

    private void Start()
    {
        if (exampleModel != null)
        {
            exampleModel.enabled = false;
        }
        switch (track)
        {
            case 2:
                modelsTrack2[Random.Range(0, modelsTrack2.Length)].SetActive(true);
                break;
            case 3:
                modelsTrack3[Random.Range(0, modelsTrack3.Length)].SetActive(true);
                break;
            case 1:
            default:
                modelsTrack1[Random.Range(0, modelsTrack1.Length)].SetActive(true);
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
