using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallObstacleController : MonoBehaviour
{
    private GameObject model;
    public int track = 1;
    public GameObject[] modelsTrack1;
    public GameObject[] modelsTrack2;
    public GameObject[] modelsTrack3;
    public MeshRenderer exampleModel;
    public float speedReduction;

    private void Start()
    {
        if (exampleModel != null)
        {
            exampleModel.enabled = false;
        }
        switch (track)
        {
            case 2:
                model = modelsTrack2[Random.Range(0, modelsTrack2.Length)];
                break;
            case 3:
                model = modelsTrack3[Random.Range(0, modelsTrack3.Length)];
                break;
            case 1:
            default:
                model = modelsTrack1[Random.Range(0, modelsTrack1.Length)];
                break;
        }
        model.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().SmallHit(speedReduction);
            if (model.GetComponent<Animator>()!=null)
            {
                model.GetComponent<Animator>().SetTrigger("Collide");
            }
        }
    }
}
