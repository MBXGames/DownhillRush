using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigObstacleController : MonoBehaviour
{
    public float dodgeBoost;
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().DodgeBoost(dodgeBoost);
        }
    }
}
