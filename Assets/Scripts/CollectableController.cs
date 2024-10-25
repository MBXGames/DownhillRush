using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : MonoBehaviour
{
    public Collectable type;
    public enum Collectable
    {
        EnergyDrink,
        RadicalCap,
        Medal
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switch (type)
            {
                case Collectable.EnergyDrink:
                    other.GetComponent<PlayerController>().DodgeBoost(1.15f, 0);
                    break;
                case Collectable.RadicalCap:
                    other.GetComponent<PlayerController>().GetRadicalCap();
                    break;
                case Collectable.Medal:
                    other.GetComponent<PlayerController>().IncreasePoints(10);
                    break;
            }
            Destroy(gameObject);
        }
    }
}
