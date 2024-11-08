using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaterial : MonoBehaviour
{
    public Material[] materialList;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material=materialList[Random.Range(0,materialList.Length)];
    }
}
