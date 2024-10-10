using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrindBarController : MonoBehaviour
{
    private PlayerController player;
    public Transform start;
    public Transform end;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject.GetComponent<PlayerController>();
            if (!player.grinding && !player.grounded)
            {
                player.grinding = true;
                player.transform.position = FindNearestPointOnLine(start.position, end.position - start.position, player.transform.position);
            }
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject.GetComponent<PlayerController>();
            if (player.grinding)
            {
                player.grinding = false;
            }
        }
    }

    public Vector3 FindNearestPointOnLine(Vector3 origin, Vector3 direction, Vector3 point)
    {
        direction.Normalize();
        Vector3 lhs = point - origin;

        float dotP = Vector3.Dot(lhs, direction);
        return origin + direction * dotP;
    }
}
