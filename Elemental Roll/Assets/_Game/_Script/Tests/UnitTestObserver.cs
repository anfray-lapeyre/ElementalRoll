using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTestObserver : Observer
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("unitTestAdd", 0.1f);
        Invoke("unitTestAddAgain", 0.2f);

    }

    // Update is called once per frame
    void Update()
    {
        print("ObserverID : " + ID);
    }

    void unitTestAdd()
    {
        print("BLAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAM");
        subject.addObserver(this);
    }

    void unitTestAddAgain()
    {
        if(ID == 0)
        {
            subject.addObserver(this);
        }
    }


    override public void OnNotify(GameObject entity, object notifiedEvent) {
        if("System.Int32" == notifiedEvent.GetType().ToString())
            print("OH MY GOD !!!! WE RECEIVED SOMETHING : " + notifiedEvent + " and " + notifiedEvent.GetType());
    }

}
