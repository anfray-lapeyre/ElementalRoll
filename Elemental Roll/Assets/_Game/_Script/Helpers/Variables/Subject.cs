using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subject : MonoBehaviour
{

    protected Observer[] observers;
    protected int numObservers=0;

    protected void Awake()
    {
        observers = new Observer[20];
    }

    public void addObserver(Observer _observer)
    {
        if (_observer.ID < 0)
        {
            observers[numObservers] = _observer;
            _observer.ID = numObservers;
            numObservers++;
        }
        else
        {
            Debug.Log("This Observer is already observing a subject ! ");
        }
    }

    public void removeObserver(Observer _observer)
    {
        //We get the observers ID
        int newID = _observer.ID;
        numObservers--;
        //Then replace it with the last observer
        observers[newID] = observers[numObservers];
        //And change its own ID
        observers[newID].ID = newID;
        //And we reset the one removed
  
        _observer.ID = -1;
    }

    public void Notify( Object notifiedEvent)
    {
        for (int i = 0; i < numObservers; i++)
        {
            observers[i].OnNotify(this.gameObject, notifiedEvent);
        }
    }
}
