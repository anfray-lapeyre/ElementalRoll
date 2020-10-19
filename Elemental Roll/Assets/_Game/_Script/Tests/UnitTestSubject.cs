using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTestSubject : Subject
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Num Observers : "+numObservers);
        Invoke("Notify", 0.3f);
        Invoke("removeObserverOne", 0.8f);
        Invoke("Notify", 0.9f);
        Invoke("NotifyFalse", 1f);

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Num Observers : "+numObservers);
    }


    private void removeObserverOne()
    {
        removeObserver(observers[0]);
    }

    public void Notify()
    {
        print("BLOOOOOOOOOOOOOOOUUUUUUUUUUUUUUUUUUUUUUUUUUUUM");

        int value = 2;
        for(int i=0; i<numObservers;i++)
        {
            observers[i].OnNotify(gameObject, value);
        }
    }

    public void NotifyFalse()
    {
        print("BLOOOOOOOOOOOOOOOUUUUUUUUUUUUUUUUUUUUUUUUUUUUM");

        char value = 'b';
        for (int i = 0; i < numObservers; i++)
        {
            observers[i].OnNotify(gameObject, value);
        }
    }
}
