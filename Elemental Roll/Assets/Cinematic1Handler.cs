using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Cinematic1Handler : MonoBehaviour
{
    public EmotionHandlerScript emotions;
    public Cinemachine.CinemachineVirtualCamera frontCamera;
    public UIFader whitefader;

    // Start is called before the first frame update
    void Start()
    {
        //Make the player bored
        emotions.Tired();
        frontCamera.Priority = 10;
    }

    
    public void Cinematic83Bored()
    {
        Debug.Log("Test");
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
}
