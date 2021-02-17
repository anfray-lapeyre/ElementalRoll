using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracyBubbleScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.LeanScale(Vector3.one * 1.2f, 0.4f).setEaseOutElastic();
        Invoke("BubblePop", 2.3f);
    }

    public void BubblePop()
    {
        this.transform.LeanScale(Vector3.one * 1.5f, 0.19f);
    }
}
