using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subject : MonoBehaviour
{

    private  Observer[] observers;
    private int numObservers=0;

    public void addObserver(Observer _observer)
    {
        observers[numObservers] = _observer;
        _observer.ID = numObservers;
        numObservers++;
    }

    public void removeObserver(Observer _observer)
    {
        //We get the observers ID
        int newID = _observer.ID;
        numObservers--;
        //Then replace it with the last observer
        observers[newID] = observers[numObservers];
        //And change its own ID
        //And change its own ID
        observers[newID].ID = newID;
    }
}
