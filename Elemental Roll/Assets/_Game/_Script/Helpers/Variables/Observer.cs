using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{

    /*
     * * Note that Observer class is available to listen to only ONE subject. 
     *  The ID is unique, and marks the place of the Observer in the Subject's array
     * It is therefore not duplicable
     * 
     */

   virtual public void OnNotify(GameObject entity, object notifiedEvent) { }
    [HideInInspector]
    public int ID=-1;
    public Subject subject;

    virtual protected void OnDestroy()
    {
        if(subject != null)
        {
            subject.removeObserver(this);
        }
    }

    protected void InvokeRealTime(string functionName, float delay)
    {
        StartCoroutine(InvokeRealTimeHelper(functionName, delay));
    }

    private IEnumerator InvokeRealTimeHelper(string functionName, float delay)
    {
        float timeElapsed = 0f;
        while (timeElapsed < delay)
        {
            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        SendMessage(functionName);
    }
}
