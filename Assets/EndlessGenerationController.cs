using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessGenerationController : MonoBehaviour
{
    private List<GameObject> spawned;
    private Vector3 direction;
    private float spawnSpace;
    private Transform player;
    public Transform directionTransform;
    public Transform firstModule;
    public Transform secondModule;
    public GameObject[] modules;
    public float spawnPlayerDistance;
    private Transform lastSpawnedModule;
    private int spawnedNumber;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        direction = directionTransform.forward.normalized;
        spawnSpace = (firstModule.position - secondModule.position).magnitude;
        lastSpawnedModule = secondModule;
        spawned = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        while ((player.position - lastSpawnedModule.position).magnitude < spawnPlayerDistance)
        {
            spawnedNumber++;
            GameObject obj = Instantiate(modules[Random.Range(0, modules.Length)], lastSpawnedModule.position + direction * spawnSpace + Vector3.down*(0.000001f), lastSpawnedModule.rotation, transform);
            if (Random.Range(0f, 1f) < 0.5f)
            {
                obj.transform.localScale =new Vector3(-obj.transform.localScale.x, obj.transform.localScale.y, obj.transform.localScale.z);
            }
            spawned.Add(obj);
            lastSpawnedModule = obj.transform;
        }
        if ((spawned[0].transform.position - player.position).magnitude > spawnPlayerDistance * 1.25f)
        {
            GameObject obj = spawned[0];
            spawned.RemoveAt(0);
            Destroy(obj);
        }
    }
}
