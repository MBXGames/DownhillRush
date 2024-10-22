using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMessages : MonoBehaviour
{
    private float tTrick;
    private PlayerController playerController;
    private void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    private void Update()
    {
        if (tTrick <= 0.1)
        {
            tTrick += Time.deltaTime;
        }   
    }

    public void FailTrick(int points)
    {
        if (tTrick > 0.1)
        {
            tTrick = 0;
            playerController.FailTrick(points, false);
        }
    }

    public void SuccesTrick(int points)
    {
        if (tTrick > 0.1)
        {
            tTrick = 0;
            playerController.FailTrick(points, true);
        }
    }
}
