using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;
using EZCameraShake;


public class findPlayer : MonoBehaviour
{

    public CinemachineVirtualCamera endingCam;
    public CinemachineVirtualCamera replayCam;
    public GameObject blackhole;
    public GameObject[] rigidbodies;
    public GameObject slimeportal;
    public Material rubyGateMaterial;

    private GameObject blackHoleanimation;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Vector3 startSize;
    private Quaternion startRotation;
    private Vector3 slimeStartSize;
    private Quaternion slimeStartRotation;
    private float startIntensity;
    private float startHue;
    private bool mustIncrement = false;
    private float currentIntensity;
    private float currentHue;
    public ParticleSystem victoryParticles;
    // Start is called before the first frame update
    public void SetGateInteraction()
    {
        Cloth gateCloth = this.GetComponent<Cloth>();
        if (gateCloth != null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length > 0)
            {
                if (players[0] != null)
                {
                    ClothSphereColliderPair[] collider = new ClothSphereColliderPair[1];

                    collider[0] = new ClothSphereColliderPair(players[0].GetComponent<SphereCollider>());
                    gateCloth.sphereColliders = collider;
                }
            }
            else
            {
                Debug.Log("Victory Gate setter : Player not found.");
            }
        }
    }

    public void Awake()
    {
        startSize = transform.localScale;
        startRotation = transform.localRotation;
        slimeStartSize = slimeportal.transform.localScale;
        slimeStartRotation = slimeportal.transform.localRotation;
        startIntensity = rubyGateMaterial.GetFloat("CrystalIntensity");
        startHue = rubyGateMaterial.GetFloat("CrystalHue");
    }

    public void OnTriggerEnter(Collider other)
    {
        //If something enters, the player won
        victoryParticles.Play();
        skinnedMeshRenderer = this.GetComponent<SkinnedMeshRenderer>();
        endingCam.LookAt = other.transform;
        replayCam.LookAt = other.transform;
        this.GetComponent<PlayableDirector>().Play();
        Invoke("startBlackHole", 0.5f);
        Invoke("activateRocks", 1.5f);
        Invoke("Animate", 1.8f);
        Invoke("hideGate", 2.5f);
        Invoke("resetSize", 3.5f);
        mustIncrement = true;
        currentIntensity = startIntensity;
        currentHue = startHue;


    }


    private void startBlackHole()
    {
        blackHoleanimation = Instantiate(blackhole, this.transform.parent);
        blackHoleanimation.transform.localScale = Vector3.zero;
        blackHoleanimation.LeanScale(Vector3.one * 30, 2.5f).setEaseInExpo();
        transform.LeanScale(Vector3.zero, 1.5f).setEaseInExpo();
        slimeportal.LeanScale(Vector3.zero, 1.5f).setEaseInExpo();
    }


    private void activateRocks()
    {
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].GetComponent<Rigidbody>().isKinematic = false;
            rigidbodies[i].GetComponent<floatScript>().enabled = false;
        }
    }

    private void Animate()
    {
        
        for (int i = 0; i < rigidbodies.Length; i++){
            rigidbodies[i].GetComponent<Rigidbody>().isKinematic = false;
            rigidbodies[i].GetComponent<floatScript>().enabled = false;
            rigidbodies[i].GetComponent<Rigidbody>().AddForce((rigidbodies[i].transform.position - this.transform.position)*((i == 7) ? 1000f : 500f));
        }
    }

    private void hideGate()
    {
        skinnedMeshRenderer.enabled = false;

    }

    private void resetSize()
    {
        LeanTween.reset();
        Destroy(blackHoleanimation);
        transform.localRotation = startRotation;
        transform.localScale = startSize;
        slimeportal.transform.localRotation = slimeStartRotation;
        slimeportal.transform.localScale = slimeStartSize;
        skinnedMeshRenderer.enabled = true;
        mustIncrement = false;
        rubyGateMaterial.SetFloat("CrystalIntensity", startIntensity);
        rubyGateMaterial.SetFloat("CrystalHue", startHue);
    }

    public void FixedUpdate()
    {
        if (mustIncrement)
        {
            currentIntensity += 0.03f;
            currentHue -= 1.5f;
            currentHue = Mathf.Max(currentHue, -140.5f);//La valeur du rouge
            rubyGateMaterial.SetFloat("CrystalIntensity", currentIntensity);
            rubyGateMaterial.SetFloat("CrystalHue", currentHue);

        }
    }

    public void enableTimeBody()
    {
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            TimeBody replay = rigidbodies[i].GetComponent<TimeBody>();
            if (replay)
            {
                replay.StartRewind();
            }
        }
    }

}
