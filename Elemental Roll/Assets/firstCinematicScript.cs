using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using Cinemachine;




public class firstCinematicScript : MonoBehaviour
{
    private bool goesToGate = false;
    public GameObject gatePosition;
    public Rigidbody player;
    public GameObject gatePortal;
    public GameObject gateSlime;
    public Material gateGemMaterial;
    public ParticleSystem particles;
    public ParticleSystem gateParticles;
    public GameObject gateRubyLit;

    public float speedmodifier;

    public float jumpintensity;
   
    private Vector3 initialSizePortal;
    private Vector3 initialSizeSlime;
    private Vector3 initialSizeRuby;



    public void Start()
    {
        initialSizePortal = gatePortal.transform.localScale;
        initialSizeSlime = gateSlime.transform.localScale;
        initialSizeRuby = gateRubyLit.transform.localScale;
    }


    public void PropellUp()
    {
        ParticleSystem.EmitParams emitOverride = new ParticleSystem.EmitParams();
        particles.Emit(emitOverride, 20);
        player.isKinematic = false;
        player.AddForce(new Vector3(0f, jumpintensity, 0f));
        Invoke("GoToGate", 0.3f);
        
    }

    public void GoToGate()
    {
        goesToGate = true;
    }

    public void GateLightsUp()
    {
        gatePortal.transform.localScale = Vector3.zero;
        gateSlime.transform.localScale = Vector3.zero;
        gateRubyLit.transform.localScale = Vector3.zero;
        gatePortal.SetActive(true);
        gateSlime.SetActive(true);
        gateRubyLit.SetActive(true);
        gatePortal.transform.LeanScale(initialSizePortal, 0.5f);
        gateSlime.transform.LeanScale(initialSizeSlime, 0.5f);
        gateRubyLit.transform.LeanScale(initialSizeRuby, 0.5f);
        Invoke("emit", 0.3f);
        Invoke("ResetGate", 0.5f);
    }

    public void emit()
    {
        ParticleSystem.EmitParams emitOverride = new ParticleSystem.EmitParams();
        gateParticles.Emit(emitOverride, 1000);

        
        /*if (gateCloth != null)
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
        }*/
    }

    public void ResetGate()
    {
        Cloth gateCloth = gatePortal.GetComponent<Cloth>();
        gateCloth.enabled = false;
        gateCloth.gameObject.SetActive(false);

        gateCloth.gameObject.SetActive(true);
        gateCloth.enabled = true;
    }

    public void OnConfirm(InputValue value)
    {
        PlayableDirector director = this.gameObject.GetComponent<PlayableDirector>();
        if (!director.playableGraph.IsValid())
        {
            director.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (goesToGate)
        {
            player.AddForce((player.position-gatePosition.transform.position)*speedmodifier);
        }
    }
}
