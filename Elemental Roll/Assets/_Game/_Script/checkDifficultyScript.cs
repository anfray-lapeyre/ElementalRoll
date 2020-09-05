using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkDifficultyScript : MonoBehaviour
{
    public IntVariable difficultyLife;
    // Start is called before the first frame update
    void Start()
    {
        if (difficultyLife.value < 1)
        {
            Destroy(this.gameObject);
        }
    }

}
