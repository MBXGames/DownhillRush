using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineController : MonoBehaviour
{
    public ParticleSystem[] endParticles;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponentInParent<PlayerController>().PlayerEnd();
            foreach(ParticleSystem p in endParticles)
            {
                p.Play();
            }
        }
    }
}
