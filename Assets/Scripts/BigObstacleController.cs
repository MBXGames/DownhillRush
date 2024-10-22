using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigObstacleController : MonoBehaviour
{
    public float dodgeBoost;
    public int dodgePoints;
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponentInParent<PlayerController>().DodgeBoost(dodgeBoost, dodgePoints);
        }
    }
}
