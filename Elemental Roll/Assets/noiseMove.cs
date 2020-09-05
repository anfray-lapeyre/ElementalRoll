using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noiseMove : MonoBehaviour
{
    private Vector3 startOffset;
    public float modifier = 1f;
    // Start is called before the first frame update
    void Start()
    {
        startOffset = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = startOffset + new Vector3(Random.Range(-modifier,modifier) , Random.Range(-modifier, modifier), Random.Range(-modifier, modifier));
    }
}
