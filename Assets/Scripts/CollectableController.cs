using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : MonoBehaviour
{
    public float rotationSpeed=45;
    public float destroyTime;
    public Collectable type;
    public enum Collectable
    {
        EnergyDrink,
        RadicalCap,
        Medal
    }

    private void Update()
    {
        transform.Rotate(transform.up, Time.deltaTime * rotationSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (GetComponent<AudioSource>() != null)
            {
                GetComponent<AudioSource>().Play();
            }
            switch (type)
            {
                case Collectable.EnergyDrink:
                    other.GetComponent<PlayerController>().DodgeBoost(1.15f, 0);
                    other.GetComponent<PlayerController>().PlayEnergyParticles();
                    break;
                case Collectable.RadicalCap:
                    other.GetComponent<PlayerController>().GetRadicalCap();
                    break;
                case Collectable.Medal:
                    other.GetComponent<PlayerController>().IncreasePoints(10);
                    break;
            }
            Hide();
            Destroy(gameObject, destroyTime);
        }
    }

    private void Hide()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
