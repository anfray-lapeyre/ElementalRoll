using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBubbleScript : MonoBehaviour
{

    private List<TimeBody> bodies;

    private bool mustFreeze = true;
    // Start is called before the first frame update
    void Start()
    {
        bodies = new List<TimeBody>();
        transform.LeanScale(Vector3.one * 15f, 2f).setEaseOutExpo();
        Invoke("EndFreeze", 8f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("other " + collision.collider.name);

    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("other " + other.name);
        if (mustFreeze)
        {
            TimeBody t;
            if (other.TryGetComponent<TimeBody>(out t))
            {
                if (other.tag != "Player")
                {

                    bodies.Add(t);
                    t.Freeze();

                }

            }
            else
            {
                t = other.GetComponentInParent<TimeBody>();
                if (t != null && t.tag != "Player")
                {
                    bodies.Add(t);
                    t.Freeze();
                }
            }
        }
        
    }

    public void EndFreeze()
    {
        mustFreeze = false;
        foreach(TimeBody t in bodies)
        {
            t.StopFreeze();
        }
        transform.LeanScale(Vector3.zero, 1f);
        Destroy(this.gameObject, 1.01f);
    }
}
