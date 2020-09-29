using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleMovingPlatformScript : MonoBehaviour
{

    public float amplitude=1f;
    public float speed=1f;
    [Range(0f, 180f)]
    public float delay=90f;
    private Vector3 startPosition;
    public bool horizontal = true;
    public bool vertical = false;
    public bool depth = false;

    private Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.gameObject.AddComponent<Rigidbody>() as Rigidbody;
        rigidbody.isKinematic = true;
        startPosition = this.transform.position;
    }

    //delay goes from -90 to 90
    //-90 = all the way left
    //90 = all the way right
    void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position + ((horizontal) ? this.transform.right * amplitude * Mathf.Sin(-90f * Mathf.Deg2Rad ) : Vector3.zero) +  ((vertical) ?this.transform.up* amplitude * Mathf.Sin(-90f * Mathf.Deg2Rad) : Vector3.zero) + ( (depth) ? this.transform.forward * amplitude * Mathf.Sin(-90f * Mathf.Deg2Rad) : Vector3.zero), 0.5f);
        Gizmos.DrawWireSphere(this.transform.position + ((horizontal) ? this.transform.right * amplitude * Mathf.Sin(90f * Mathf.Deg2Rad ) : Vector3.zero) +  ((vertical) ?this.transform.up * amplitude * Mathf.Sin(90f * Mathf.Deg2Rad ) : Vector3.zero) + ( (depth) ? this.transform.forward * amplitude * Mathf.Sin(90f * Mathf.Deg2Rad) : Vector3.zero), 0.5f);
        Gizmos.DrawWireMesh(this.GetComponent<MeshFilter>().sharedMesh, this.transform.position + ((horizontal) ? this.transform.right * amplitude * Mathf.Sin(-90f * Mathf.Deg2Rad  + delay*Mathf.Deg2Rad) : Vector3.zero) + ((vertical) ? this.transform.up * amplitude * Mathf.Sin(-90f * Mathf.Deg2Rad + delay*Mathf.Deg2Rad) : Vector3.zero) + ((depth) ? this.transform.forward * amplitude * Mathf.Sin(-90f * Mathf.Deg2Rad + delay*Mathf.Deg2Rad) : Vector3.zero), this.transform.rotation, this.transform.localScale);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(this.transform.position + ((horizontal) ? this.transform.right * amplitude * Mathf.Sin(-90f * Mathf.Deg2Rad ) : Vector3.zero) + ((vertical) ? this.transform.up * amplitude * Mathf.Sin(-90f * Mathf.Deg2Rad ) : Vector3.zero) + ((depth) ? this.transform.forward * amplitude * Mathf.Sin(-90f * Mathf.Deg2Rad) : Vector3.zero), this.transform.position + ((horizontal) ? this.transform.right * amplitude * Mathf.Sin(90f * Mathf.Deg2Rad ) : Vector3.zero) + ((vertical) ? this.transform.up * amplitude * Mathf.Sin(90f * Mathf.Deg2Rad ) : Vector3.zero) + ((depth) ? this.transform.forward * amplitude * Mathf.Sin(90f * Mathf.Deg2Rad ) : Vector3.zero));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigidbody.MovePosition(startPosition + ((horizontal) ? this.transform.right*amplitude * Mathf.Sin(Time.fixedTime * speed + delay*Mathf.Deg2Rad) : Vector3.zero) +  ((vertical) ? this.transform.up* amplitude * Mathf.Sin(Time.fixedTime * speed + delay * Mathf.Deg2Rad) : Vector3.zero) +  ((depth) ? this.transform.forward *amplitude * Mathf.Sin(Time.fixedTime * speed + delay * Mathf.Deg2Rad) : Vector3.zero));
    }
}
