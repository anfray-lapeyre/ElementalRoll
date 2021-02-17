using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkDifficultyScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (ActualSave.actualSave.stats[0].DifficultyLife < 1)
        {
            Destroy(this.gameObject);
        }
    }

}
