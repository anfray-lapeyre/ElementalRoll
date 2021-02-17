using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCountScript : MonoBehaviour
{

    private void Awake()
    {
        //This is an emergency thing. It should not ever trigger except in testing environment
        if (ActualSave.actualSave == null)
        {
            ActualSave.actualSave = new SaveFileInfo();
        }
    }

    public void OnPlayerJoined()
    {
        Debug.Log("Player Joined");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = players.Length; i > 0; i--)
        {
            players[i-1].GetComponent<PlayerController>().updatePlayerScreenHandling(i-1, players.Length);
        }
    }

    public void OnPlayerLeft()
    {
        Debug.Log("Player Quit");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = players.Length; i >0; i--)
        {
            players[i-1].GetComponent<PlayerController>().updatePlayerScreenHandling(i-1, players.Length);
        }
    }

}
