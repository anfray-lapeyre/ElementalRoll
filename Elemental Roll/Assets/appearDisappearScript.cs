using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class appearDisappearScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        this.transform.LeanScale(Vector3.one, 0.5f);
        Invoke("Disappear", 5f);
    }

    public void Disappear()
    {
        this.transform.LeanScale(Vector3.zero, 0.5f);
    }

}
