using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessGenerationController : MonoBehaviour
{
    private int r2;
    private List<GameObject> spawned;
    private Vector3 direction;
    private float spawnSpace;
    private Transform player;
    private int count;
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
        r2= Random.Range(10, 16);
    }

    // Update is called once per frame
    void Update()
    {
        while ((player.position - lastSpawnedModule.position).magnitude < spawnPlayerDistance)
        {
            int r = Random.Range(0, modules.Length);
            if (r == 0 || count>6)
            {
                count = 0;
                r = 0;
                r2 = Random.Range(10, 16);
            }
            else if (count<r2)
            {
                count++;
            }
            GameObject obj = Instantiate(modules[r], lastSpawnedModule.position + direction * spawnSpace + Vector3.down * (0.000001f), lastSpawnedModule.rotation, transform);
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
