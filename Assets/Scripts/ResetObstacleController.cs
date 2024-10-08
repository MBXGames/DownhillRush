using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObstacleController : MonoBehaviour
{
    private PlayerController player;
    public float resetTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject.GetComponent<PlayerController>();
            player.LockPlayer();
            Invoke("Respawn", resetTime);
        }
    }

    public void Respawn()
    {
        if (player != null)
        {
            player.UnlockPlayer();
            player.ResetToCheckpoint();
        }
    }
}
