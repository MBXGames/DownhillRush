using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessModuleController : MonoBehaviour
{
    public int obstacleNumber;
    public int obstacleNumberVariation;
    public List<GameObject> obstacles;
    private List<GameObject> activatedObstacles;
    // Start is called before the first frame update
    void Start()
    {
        activatedObstacles = new List<GameObject>();
        int r = Random.Range(-obstacleNumberVariation, obstacleNumberVariation+1);
        for (int i = 0; i < obstacleNumber+r; i++)
        {
            int r2 =Random.Range(0, obstacles.Count);
            activatedObstacles.Add(obstacles[r2]);
            obstacles.RemoveAt(r2);
        }
        foreach (GameObject obstacle in obstacles)
        {
            Destroy(obstacle);
        }
        obstacles.Clear();
    }
}
