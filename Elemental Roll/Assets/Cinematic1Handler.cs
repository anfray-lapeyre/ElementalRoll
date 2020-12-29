using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Playables;


public class Cinematic1Handler : MonoBehaviour
{
    

    [Header("Global")]
    public UIFader whitefader;
    public int cinematicNumber=0;


    [Header("1st Scene Specific")]
    public EmotionHandlerScript FirstCinematicemotions;
    public Cinemachine.CinemachineVirtualCamera frontCamera;

    [Header("2nd Scene Specific")]

    //Space between variables 
    [Space(10)]
    public GameObject Hell1;
    public GameObject Hell2;
    public GameObject Hell3;
    public Cinemachine.CinemachineVirtualCamera HELLVCAM1;
    public Cinemachine.CinemachineVirtualCamera HELLVCAM2;
    public Cinemachine.CinemachineVirtualCamera HELLVCAM3;
    public Cinemachine.CinemachineVirtualCamera HellFlyingVCAM;
    public Cinemachine.CinemachineVirtualCamera GreenFlyingVCAM;
    public Cinemachine.CinemachineVirtualCamera BlueFlyingVCAM;
    public Cinemachine.CinemachineVirtualCamera RiverVCAM;

    public AudioSource audioSource;
    public AudioClip boom;
    public AudioClip openPortal;
    public AudioClip closePortal;

    public Canvas canvas;



    private bool goesToGate = false;

    [Header("3rd Scene Specific")]

    //Space between variables 
    [Space(10)]

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

    public Cinemachine.CinemachineVirtualCamera VCAM1;
    public Cinemachine.CinemachineVirtualCamera VCAM2;
    public Cinemachine.CinemachineVirtualCamera VCAM3;
    public Cinemachine.CinemachineVirtualCamera VCAM4;
    public Cinemachine.CinemachineVirtualCamera VCAM5;
    public Cinemachine.CinemachineVirtualCamera VCAM6;


    // Start is called before the first frame update
    void Start()
    {
        //Make the player bored
        if (cinematicNumber == 0)
        {
            FirstCinematicemotions.Tired();
            frontCamera.Priority = 10;
        }else if(cinematicNumber == 2)
        {
            initialSizePortal = gatePortal.transform.localScale;
            initialSizeSlime = gateSlime.transform.localScale;
            initialSizeRuby = gateRubyLit.transform.localScale;
        }
    }

    
    public void Cinematic83Bored()
    {
        //Debug.Log("Test");
    }

    public void ResetPriority()
    {
        HELLVCAM1.Priority = 1;
        HELLVCAM2.Priority = 1;
        HELLVCAM3.Priority = 1;
        HellFlyingVCAM.Priority = 1;
        GreenFlyingVCAM.Priority = 1;
        BlueFlyingVCAM.Priority = 1;
        RiverVCAM.Priority = 1;
    }

    public void Cinematic1()
    {
        Hell1.SetActive(true);
        Hell2.SetActive(false);
        ResetPriority();
        HELLVCAM1.Priority = 10;
    }

    public void Cinematic2()
    {
        Hell2.SetActive(true);
        Hell1.SetActive(false);
        ResetPriority();
        HELLVCAM2.Priority = 10;
    }

    public void Cinematic3()
    {
        audioSource.PlayOneShot(openPortal);

        Hell3.SetActive(true);
        Hell2.SetActive(false);
        ResetPriority();
        HELLVCAM3.Priority = 10;
    }

    public void Cinematic4()
    {
        audioSource.PlayOneShot(closePortal);
        ResetPriority();
        HellFlyingVCAM.Priority = 10;
    }

    public void Cinematic5()
    {
        ResetPriority();
        GreenFlyingVCAM.Priority = 10;
    }

    public void Cinematic6()
    {
        ResetPriority();
        BlueFlyingVCAM.Priority = 10;
    }

    public void Cinematic7()
    {
        audioSource.PlayOneShot(boom);
        ResetPriority();
        RiverVCAM.Priority = 10;
    }

    public void LoadCutscene12()
    {
        //WhiteFadeIn
        whitefader.FadeIn(0.5f);
        //LoadCustscene
        Invoke("LoadCutscene12afterwait", 0.5f);
    }

    public void LoadCutscene12afterwait()
    {
        //CutsceneWithoutCabin
        SceneManager.LoadScene("CutsceneWithoutCabin");
    }


    public void LoadCutsceneCabin2()
    {
        canvas.gameObject.SetActive(true);

        //WhiteFadeIn
        whitefader.FadeIn(0.5f);
        //LoadCustscene
        Invoke("LoadCutsceneCabin2afterwait", 0.5f);
    }

    public void LoadCutsceneCabin2afterwait()
    {
        //CutsceneCabin2
        SceneManager.LoadScene("CutsceneCabin2");
        whitefader.FadeIn(0.1f);
    }





    /* THIRD CUTSCENE ELEMENT*/

    public void Cinematic82Wait()
    {
        ResetPriority3rdScene();
        VCAM6.Priority = 10;
    }

    public void Cinematic84Wait()
    {
        ResetPriority3rdScene();
        VCAM3.Priority = 10;
    }

    public void Cinematic84ActiveWait()
    {
        ResetPriority3rdScene();
        VCAM3.Priority = 10;
        GateLightsUp();
        audioSource.PlayOneShot(openPortal);
    }

    public void Cinematic82Go()
    {
        ResetPriority3rdScene();
        VCAM6.Priority = 10;
        Invoke("PropellUp",3f);
    }

    public void ResetPriority3rdScene()
    {
        VCAM1.Priority = 1;
        VCAM2.Priority = 1;
        VCAM3.Priority = 1;
        VCAM4.Priority = 1;
        VCAM5.Priority = 1;
        VCAM6.Priority = 1;

    }

    public void PropellUp()
    {
        audioSource.PlayOneShot(boom);
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
        gatePortal.SetActive(true);
        gateSlime.SetActive(true);
        gateRubyLit.SetActive(true);
        gatePortal.transform.localScale=initialSizePortal;
        gateSlime.transform.localScale=initialSizeSlime;
        gateRubyLit.transform.localScale = initialSizeRuby;
        //Invoke("emit", 0.3f);
        Invoke("ResetGate", 0.1f);
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


    // Update is called once per frame
    void Update()
    {
        if (goesToGate)
        {
            player.AddForce((player.position - gatePosition.transform.position) * speedmodifier);
        }
    }

    public void loadFirstLevel()
    {
        Debug.Log("BIM");
        canvas.gameObject.SetActive(true);
        SceneManager.LoadScene("WelcomeHome");

    }
}
