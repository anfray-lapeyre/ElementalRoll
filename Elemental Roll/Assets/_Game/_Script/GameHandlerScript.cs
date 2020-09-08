using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandlerScript : MonoBehaviour
{

    public FloatVariable time;
    public FloatVariable playerScore;
    public bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        //We reset and score
        playerScore.value = 0f;
        //The time is set by spawnPlayerScript
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //We increment time
        if(isRunning)
            time.value -= Time.fixedDeltaTime;
    }

    public void StopRunning()
    {
        isRunning = false;
    }

    public void startRunning()
    {
        //We wait one second to give the impression that the first second is passing
        Invoke("doStartRunning", 0.5f);
    }

    public void doStartRunning()
    {
        isRunning = true;
    }


}
