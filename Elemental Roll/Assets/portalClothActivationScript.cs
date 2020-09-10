using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalClothActivationScript : MonoBehaviour
{
    public Cloth cloth;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("ActivateCloth", 0.5f);
    }

    public void ActivateCloth()
    {
        cloth.enabled = true;
    }
}
