using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class passCinematicScript : MonoBehaviour
{
    private bool isEnabled = true;
    public void OnRestart(InputValue input)
    {
        passCinematic();
    }

    public void OnSpecialAction(InputValue input)
    {
        passCinematic();
    }

    private void passCinematic()
    {
        if (isEnabled)
        {
            PlayableDirector director = this.gameObject.GetComponent<PlayableDirector>();
            director.playableGraph.GetRootPlayable(0).SetSpeed(10000);
        }

    }

    public void closeTimeline()
    {
        Destroy(this.gameObject);
    }

    public void disable()
    {
        isEnabled = false;
    }

    public void doEnable()
    {
        isEnabled = true;
    }
}
