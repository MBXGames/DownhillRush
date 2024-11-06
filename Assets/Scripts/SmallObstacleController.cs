using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SmallObstacleController : MonoBehaviour
{
    private GameObject model;
    public int track = 1;
    public Transform modelsTrack1;
    public Transform modelsTrack2;
    public Transform modelsTrack3;
    public MeshRenderer exampleModel;
    public float speedReduction;

    private void Start()
    {
        
        if (exampleModel != null)
        {
            exampleModel.enabled = false;
        }
        switch (SceneManager.GetActiveScene().name)
        {
            case "Circuito2":
                if (modelsTrack2.childCount < 1)
                {
                    model=modelsTrack1.GetChild(Random.Range(0, modelsTrack1.childCount)).gameObject;
                    break;
                }
                model = modelsTrack2.GetChild(Random.Range(0, modelsTrack2.childCount)).gameObject;
                break;
            case "Circuito3":
                if (modelsTrack3.childCount < 1)
                {
                    model = modelsTrack1.GetChild(Random.Range(0, modelsTrack1.childCount)).gameObject;

                    break;
                }
                model = modelsTrack3.GetChild(Random.Range(0, modelsTrack3.childCount)).gameObject;
                break;
            case "Circuito1":
            default:
                model = modelsTrack1.GetChild(Random.Range(0, modelsTrack1.childCount)).gameObject;
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
