using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalBreakableScript : MonoBehaviour
{
   
    public void Break()
    {
        Destroy(this.gameObject);
    }
}
