using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessLimitsController : MonoBehaviour
{
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z < player.transform.position.z)
        {
            transform.position += transform.forward * (player.transform.position.z - transform.position.z);
        }
    }
}
