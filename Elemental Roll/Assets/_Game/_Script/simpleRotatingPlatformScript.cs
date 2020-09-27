using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class simpleRotatingPlatformScript : MonoBehaviour
{
    [Range(0.0f,360.0f)]
    public float amplitude = 360f;
    public float speed = 1f;
    private Quaternion startRotation;
    public bool horizontal = true;
    public bool vertical = false;
    public bool depth = false;
    public bool backAndForth = false;
    public bool easeInOut = false;

    // Start is called before the first frame update
    void Start()
    {
        startRotation = this.transform.rotation;
    }

    void OnDrawGizmosSelected()
    {
        if (this.GetComponent<MeshFilter>())
        {
            Mesh myMesh = this.GetComponent<MeshFilter>().sharedMesh;
            Gizmos.color = Color.red;
            if (easeInOut)
            {
                if (backAndForth)
                {
                    Gizmos.DrawWireMesh(myMesh, this.transform.position, Quaternion.Euler(startRotation.eulerAngles.x + ((horizontal) ? Mathf.Sin((float)EditorApplication.timeSinceStartup * speed) * amplitude/2f : 0), startRotation.eulerAngles.y + ((vertical) ? Mathf.Sin((float)EditorApplication.timeSinceStartup * speed) * amplitude/2f : 0), startRotation.eulerAngles.z + ((depth) ? Mathf.Sin((float)EditorApplication.timeSinceStartup * speed) * amplitude/2f : 0)), this.transform.localScale);

                }
                else
                {
                    Gizmos.DrawWireMesh(myMesh, this.transform.position, Quaternion.Euler(startRotation.eulerAngles.x + ((horizontal) ? Mathf.Sin(((float)EditorApplication.timeSinceStartup * speed) % (Mathf.PI) - Mathf.PI / 2f) * amplitude/2f + amplitude/2f : 0), startRotation.eulerAngles.y + ((vertical) ? Mathf.Sin(((float)EditorApplication.timeSinceStartup * speed) % (Mathf.PI) - Mathf.PI / 2f) * amplitude/2f + amplitude/2f : 0), startRotation.eulerAngles.z + ((depth) ? Mathf.Sin(((float)EditorApplication.timeSinceStartup * speed) % (Mathf.PI) - Mathf.PI / 2f) * amplitude/2f + amplitude/2f : 0)), this.transform.localScale);
                    //.MovePosition(startPosition + ((horizontal) ? this.transform.right * amplitude * Mathf.Sin(Time.fixedTime * speed + delay * Mathf.Deg2Rad) : Vector3.zero) + ((vertical) ? this.transform.up * amplitude * Mathf.Sin(Time.fixedTime * speed + delay * Mathf.Deg2Rad) : Vector3.zero) + ((depth) ? this.transform.forward * amplitude * Mathf.Sin(Time.fixedTime * speed + delay * Mathf.Deg2Rad) : Vector3.zero));
                }
            }
            else
            {
                if (backAndForth)
                {

                    Gizmos.DrawWireMesh(myMesh, this.transform.position, Quaternion.Euler(startRotation.eulerAngles.x + ((horizontal) ? (amplitude / Mathf.PI) * Mathf.Asin(Mathf.Sin((Mathf.PI * 2f * speed / 10f) * (float)EditorApplication.timeSinceStartup)) + amplitude/2f : 0), startRotation.eulerAngles.y + ((vertical) ? (amplitude / Mathf.PI) * Mathf.Asin(Mathf.Sin((Mathf.PI * 2f * speed / 10f) * (float)EditorApplication.timeSinceStartup)) + amplitude/2f : 0), startRotation.eulerAngles.z + ((depth) ? (amplitude / Mathf.PI) * Mathf.Asin(Mathf.Sin((Mathf.PI * 2f * speed / 10f) * (float)EditorApplication.timeSinceStartup)) + amplitude/2f : 0)), this.transform.localScale);

                }
                else
                {
                    Gizmos.DrawWireMesh(myMesh, this.transform.position, Quaternion.Euler(startRotation.eulerAngles.x + ((horizontal) ? (float)EditorApplication.timeSinceStartup * speed * 50f : 0), startRotation.eulerAngles.y + ((vertical) ? (float)EditorApplication.timeSinceStartup * speed * 50f : 0), startRotation.eulerAngles.z + ((depth) ? (float)EditorApplication.timeSinceStartup * speed * 50f : 0)), this.transform.localScale);


                    //this.transform.localRotation = Quaternion.Euler();
                    //.MovePosition(startPosition + ((horizontal) ? this.transform.right * amplitude * Mathf.Sin(Time.fixedTime * speed + delay * Mathf.Deg2Rad) : Vector3.zero) + ((vertical) ? this.transform.up * amplitude * Mathf.Sin(Time.fixedTime * speed + delay * Mathf.Deg2Rad) : Vector3.zero) + ((depth) ? this.transform.forward * amplitude * Mathf.Sin(Time.fixedTime * speed + delay * Mathf.Deg2Rad) : Vector3.zero));
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (easeInOut)
        {
            if (backAndForth)
            {
                this.transform.localEulerAngles = new Vector3(startRotation.eulerAngles.x + ((horizontal) ? Mathf.Sin(Time.fixedTime * speed)*(amplitude/2f) : 0), startRotation.eulerAngles.y + ((vertical) ? Mathf.Sin(Time.fixedTime * speed) * (amplitude/2f) : 0), startRotation.eulerAngles.z + ((depth) ? Mathf.Sin(Time.fixedTime * speed) * (amplitude/2f) : 0));

            }
            else
            {
           
                this.transform.localEulerAngles = new Vector3(startRotation.eulerAngles.x + ((horizontal) ? Mathf.Sin((Time.fixedTime * speed) % (Mathf.PI) - Mathf.PI / 2f) * (amplitude/2f) + (amplitude/2f) : 0), startRotation.eulerAngles.y + ((vertical) ? Mathf.Sin((Time.fixedTime * speed) % (Mathf.PI) - Mathf.PI / 2f) * (amplitude/2f) + (amplitude/2f) : 0), startRotation.eulerAngles.z + ((depth) ? Mathf.Sin((Time.fixedTime * speed) % (Mathf.PI) - Mathf.PI / 2f) * (amplitude/2f) + (amplitude/2f) : 0));
                //.MovePosition(startPosition + ((horizontal) ? this.transform.right * amplitude * Mathf.Sin(Time.fixedTime * speed + delay * Mathf.Deg2Rad) : Vector3.zero) + ((vertical) ? this.transform.up * amplitude * Mathf.Sin(Time.fixedTime * speed + delay * Mathf.Deg2Rad) : Vector3.zero) + ((depth) ? this.transform.forward * amplitude * Mathf.Sin(Time.fixedTime * speed + delay * Mathf.Deg2Rad) : Vector3.zero));
            }
        }
        else
        {
            if (backAndForth)
            {
                this.transform.localEulerAngles = new Vector3(startRotation.eulerAngles.x + ((horizontal) ? (2f * (amplitude/2f) / Mathf.PI) * Mathf.Asin(Mathf.Sin((Mathf.PI * 2f * speed / 10f) * (float)EditorApplication.timeSinceStartup)) + (amplitude/2f) : 0), startRotation.eulerAngles.y + ((vertical) ? (2f * (amplitude/2f) / Mathf.PI) * Mathf.Asin(Mathf.Sin((Mathf.PI * 2f * speed / 10f) * (float)EditorApplication.timeSinceStartup)) + (amplitude/2f) : 0), startRotation.eulerAngles.z + ((depth) ? (2f * (amplitude/2f) / Mathf.PI) * Mathf.Asin(Mathf.Sin((Mathf.PI * 2f * speed / 10f) * (float)EditorApplication.timeSinceStartup)) + (amplitude/2f) : 0));
            }
            else
            {
                this.transform.localEulerAngles = new Vector3(startRotation.eulerAngles.x + ((horizontal) ? Time.fixedTime*speed*50f : 0), startRotation.eulerAngles.y + ((vertical) ? Time.fixedTime*speed*50f : 0), startRotation.eulerAngles.z + ((depth) ? Time.fixedTime*speed * 50f : 0));
                //this.transform.localRotation = Quaternion.Euler();
                //.MovePosition(startPosition + ((horizontal) ? this.transform.right * amplitude * Mathf.Sin(Time.fixedTime * speed + delay * Mathf.Deg2Rad) : Vector3.zero) + ((vertical) ? this.transform.up * amplitude * Mathf.Sin(Time.fixedTime * speed + delay * Mathf.Deg2Rad) : Vector3.zero) + ((depth) ? this.transform.forward * amplitude * Mathf.Sin(Time.fixedTime * speed + delay * Mathf.Deg2Rad) : Vector3.zero));
            }
        }
    }
}
