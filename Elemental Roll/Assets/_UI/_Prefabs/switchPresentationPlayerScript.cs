using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class switchPresentationPlayerScript : MonoBehaviour
{
    public GameObject[] players;
    private int count;

    public void OnConfirm(InputValue input)
    {

        players[count].SetActive( false);
        count = (count + 1) % players.Length;
        players[count].SetActive(true);
    }
}
