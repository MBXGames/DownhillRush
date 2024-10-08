using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallObstacleController : MonoBehaviour
{
    public float speedReduction;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().SmallHit(speedReduction);
        }
    }
}
