using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusController : MonoBehaviour
{
    public GameObject bonusAnimation;

    private void OnTriggerEnter()
    {
        Instantiate(bonusAnimation);
    }
}
