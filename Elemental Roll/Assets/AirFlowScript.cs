using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirFlowScript : MonoBehaviour
{

    private Dictionary<int,Rigidbody> player;
    public float strength = 10f;
    private void Start()
    {
        player = new Dictionary<int, Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !player.ContainsKey(other.GetComponent<PlayerController>().playerNb))
        {
            player.Add(other.GetComponent<PlayerController>().playerNb, other.GetComponent<Rigidbody>());
        }
    }

    //When the Primitive exits the collision, it will change Color
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {

            player.Remove(other.GetComponent<PlayerController>().playerNb);
        }

    }


    private void FixedUpdate()
    {
        if (player.Count > 0)
        {
            for(int i=0; i<player.Count; i++)
            {
                if (player[i])
                {
                    player[i].AddForce(transform.up * strength);
                }
                else
                {
                    player.Remove(i);
                }
            }
            foreach(Rigidbody rb in player.Values)
            {
                
            }
        }
    }
}
