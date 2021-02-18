using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class simpleRotatingPlatformScript : MonoBehaviour
{
    [Range(0.0f, 360.0f)]
    public float amplitude = 360f;
    public float speed = 1f;
    private Quaternion startRotation;
    public bool horizontal = true;
    public bool vertical = false;
    public bool depth = false;
    public bool backAndForth = false;
    public bool easeInOut = false;
    public float offset = 0f;
    private Rigidbody playerRigidbody;


    [HideInInspector]
    public bool paused = false;

    void Awake()
    {
        playerRigidbody = this.gameObject.GetComponent<Rigidbody>();
        if (playerRigidbody == null)
            playerRigidbody = this.gameObject.AddComponent<Rigidbody>() as Rigidbody;
        playerRigidbody.isKinematic = true;
        TimeBody tb = this.gameObject.AddComponent<TimeBody>();
        tb.isPlatform = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        startRotation = this.transform.rotation;
    }

#if UNITY_EDITOR
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
                    Gizmos.DrawWireMesh(myMesh, this.transform.position, Quaternion.Euler(transform.rotation.eulerAngles.x + ((horizontal) ? Mathf.Sin((float)EditorApplication.timeSinceStartup * speed) * amplitude / 2f : 0), transform.rotation.eulerAngles.y + ((vertical) ? Mathf.Sin((float)EditorApplication.timeSinceStartup * speed) * amplitude / 2f : 0), transform.rotation.eulerAngles.z + ((depth) ? Mathf.Sin((float)EditorApplication.timeSinceStartup * speed) * amplitude / 2f : 0)), this.transform.localScale);

                }
                else
                {
                    Gizmos.DrawWireMesh(myMesh, this.transform.position, Quaternion.Euler(transform.rotation.eulerAngles.x + ((horizontal) ? Mathf.Sin(((float)EditorApplication.timeSinceStartup * speed) % (Mathf.PI) - Mathf.PI / 2f) * amplitude / 2f + amplitude / 2f : 0), transform.rotation.eulerAngles.y + ((vertical) ? Mathf.Sin(((float)EditorApplication.timeSinceStartup * speed) % (Mathf.PI) - Mathf.PI / 2f) * amplitude / 2f + amplitude / 2f : 0), transform.rotation.eulerAngles.z + ((depth) ? Mathf.Sin(((float)EditorApplication.timeSinceStartup * speed) % (Mathf.PI) - Mathf.PI / 2f) * amplitude / 2f + amplitude / 2f : 0)), this.transform.localScale);
                    //.MovePosition(startPosition + ((horizontal) ? this.transform.right * amplitude * Mathf.Sin((Time.fixedTime-offset) * speed + delay * Mathf.Deg2Rad) : Vector3.zero) + ((vertical) ? this.transform.up * amplitude * Mathf.Sin((Time.fixedTime-offset) * speed + delay * Mathf.Deg2Rad) : Vector3.zero) + ((depth) ? this.transform.forward * amplitude * Mathf.Sin((Time.fixedTime-offset) * speed + delay * Mathf.Deg2Rad) : Vector3.zero));
                }
            }
            else
            {
                if (backAndForth)
                {

                    Gizmos.DrawWireMesh(myMesh, this.transform.position, Quaternion.Euler(transform.rotation.eulerAngles.x + ((horizontal) ? (amplitude / Mathf.PI) * Mathf.Asin(Mathf.Sin((Mathf.PI * 2f * speed / 10f) * (float)EditorApplication.timeSinceStartup)) + amplitude / 2f : 0), transform.rotation.eulerAngles.y + ((vertical) ? (amplitude / Mathf.PI) * Mathf.Asin(Mathf.Sin((Mathf.PI * 2f * speed / 10f) * (float)EditorApplication.timeSinceStartup)) + amplitude / 2f : 0), transform.rotation.eulerAngles.z + ((depth) ? (amplitude / Mathf.PI) * Mathf.Asin(Mathf.Sin((Mathf.PI * 2f * speed / 10f) * (float)EditorApplication.timeSinceStartup)) + amplitude / 2f : 0)), this.transform.localScale);

                }
                else
                {
                    Gizmos.DrawWireMesh(myMesh, this.transform.position, Quaternion.Euler(transform.rotation.eulerAngles.x + ((horizontal) ? (float)EditorApplication.timeSinceStartup * speed * 50f : 0), transform.rotation.eulerAngles.y + ((vertical) ? (float)EditorApplication.timeSinceStartup * speed * 50f : 0), transform.rotation.eulerAngles.z + ((depth) ? (float)EditorApplication.timeSinceStartup * speed * 50f : 0)), this.transform.localScale);


                    //this.transform.localRotation = Quaternion.Euler();
                    //.MovePosition(startPosition + ((horizontal) ? this.transform.right * amplitude * Mathf.Sin((Time.fixedTime-offset) * speed + delay * Mathf.Deg2Rad) : Vector3.zero) + ((vertical) ? this.transform.up * amplitude * Mathf.Sin((Time.fixedTime-offset) * speed + delay * Mathf.Deg2Rad) : Vector3.zero) + ((depth) ? this.transform.forward * amplitude * Mathf.Sin((Time.fixedTime-offset) * speed + delay * Mathf.Deg2Rad) : Vector3.zero));
                }
            }
        }
    }
#endif

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!paused)
        {
            if (easeInOut)
            {
                if (backAndForth)
                {
                    playerRigidbody.MoveRotation(Quaternion.Euler(new Vector3(startRotation.eulerAngles.x + ((horizontal) ? Mathf.Sin((Time.fixedTime - offset) * speed) * (amplitude / 2f) : 0), startRotation.eulerAngles.y + ((vertical) ? Mathf.Sin((Time.fixedTime - offset) * speed) * (amplitude / 2f) : 0), startRotation.eulerAngles.z + ((depth) ? Mathf.Sin((Time.fixedTime - offset) * speed) * (amplitude / 2f) : 0))));
                    //this.transform.localEulerAngles =;

                }
                else
                {

                    playerRigidbody.MoveRotation(Quaternion.Euler(new Vector3(startRotation.eulerAngles.x + ((horizontal) ? Mathf.Sin(((Time.fixedTime - offset) * speed) % (Mathf.PI) - Mathf.PI / 2f) * (amplitude / 2f) + (amplitude / 2f) : 0), startRotation.eulerAngles.y + ((vertical) ? Mathf.Sin(((Time.fixedTime - offset) * speed) % (Mathf.PI) - Mathf.PI / 2f) * (amplitude / 2f) + (amplitude / 2f) : 0), startRotation.eulerAngles.z + ((depth) ? Mathf.Sin(((Time.fixedTime - offset) * speed) % (Mathf.PI) - Mathf.PI / 2f) * (amplitude / 2f) + (amplitude / 2f) : 0))));
                    //.MovePosition(startPosition + ((horizontal) ? this.transform.right * amplitude * Mathf.Sin((Time.fixedTime-offset) * speed + delay * Mathf.Deg2Rad) : Vector3.zero) + ((vertical) ? this.transform.up * amplitude * Mathf.Sin((Time.fixedTime-offset) * speed + delay * Mathf.Deg2Rad) : Vector3.zero) + ((depth) ? this.transform.forward * amplitude * Mathf.Sin((Time.fixedTime-offset) * speed + delay * Mathf.Deg2Rad) : Vector3.zero));
                }
            }
            else
            {
                if (backAndForth)
                {
                    playerRigidbody.MoveRotation(Quaternion.Euler(new Vector3(startRotation.eulerAngles.x + ((horizontal) ? (2f * (amplitude / 2f) / Mathf.PI) * Mathf.Asin(Mathf.Sin((Mathf.PI * 2f * speed / 10f) * (Time.fixedTime - offset))) + (amplitude / 2f) : 0), startRotation.eulerAngles.y + ((vertical) ? (2f * (amplitude / 2f) / Mathf.PI) * Mathf.Asin(Mathf.Sin((Mathf.PI * 2f * speed / 10f) * (Time.fixedTime - offset))) + (amplitude / 2f) : 0), startRotation.eulerAngles.z + ((depth) ? (2f * (amplitude / 2f) / Mathf.PI) * Mathf.Asin(Mathf.Sin((Mathf.PI * 2f * speed / 10f) * (Time.fixedTime - offset))) + (amplitude / 2f) : 0))));
                }
                else
                {
                    playerRigidbody.MoveRotation(Quaternion.Euler(new Vector3(startRotation.eulerAngles.x + ((horizontal) ? (Time.fixedTime - offset) * speed * 50f : 0), startRotation.eulerAngles.y + ((vertical) ? (Time.fixedTime - offset) * speed * 50f : 0), startRotation.eulerAngles.z + ((depth) ? (Time.fixedTime - offset) * speed * 50f : 0))));
                    //this.transform.localRotation = Quaternion.Euler();
                    //.MovePosition(startPosition + ((horizontal) ? this.transform.right * amplitude * Mathf.Sin((Time.fixedTime-offset) * speed + delay * Mathf.Deg2Rad) : Vector3.zero) + ((vertical) ? this.transform.up * amplitude * Mathf.Sin((Time.fixedTime-offset) * speed + delay * Mathf.Deg2Rad) : Vector3.zero) + ((depth) ? this.transform.forward * amplitude * Mathf.Sin((Time.fixedTime-offset) * speed + delay * Mathf.Deg2Rad) : Vector3.zero));
                }
            }
        }
    }

    public void AddOffset(float _offset){
        offset += _offset;
        
       }
}
