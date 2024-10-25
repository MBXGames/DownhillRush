using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsController : MonoBehaviour
{
    private BetweenScenesCanvas betweenScenesCanvas;
    private float t;
    private bool done;
    public float resultsShowTime;
    // Start is called before the first frame update
    void Start()
    {
        betweenScenesCanvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<BetweenScenesCanvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if (t < resultsShowTime)
        {
            t += Time.deltaTime;
        }
        else if(!done)
        {
            betweenScenesCanvas.ShowResults();
            done = true;
        }
    }
}
