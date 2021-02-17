using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIElementPlacer : MonoBehaviour
{
    private Camera UIcamera;
    private Canvas canvas;
    private CanvasScaler scaler;
    private RectTransform[] children;
    private Vector2 initialResolution;
    // Start is called before the first frame update

    //Here we get all the elements nedded to spread the player Nb to others
    public int playerNb=0;
    public SpeedometerScript speedometer;
    public powerProgressBarScript powerBar;
    public RawImage raw;
    public Image bubble;
    //For the moment I don't update the bonus section, still needs some conception work to be sure

    //Array of RawImages that each contain the render of the faceCamera of a player
    public Sprite[] bubbles; //0 = Byle 1 = Tracy 2 = Rocky 3 = Tim 
    public RenderTexture[] renderTextures;


    void Awake()
    {
        if (canvas == null)
        {
            canvas = GetComponent<Canvas>();
            scaler = GetComponent<CanvasScaler>();
            UIcamera = canvas.worldCamera;
            initialResolution = scaler.referenceResolution;
        }
    }

    public void UpdateRatio(int _playerNb)
    {
        playerNb = _playerNb;
        if (canvas == null)
        {
            Awake();
        }

        UIcamera.rect = UIcamera.transform.parent.GetComponent<Camera>().rect;
        scaler.referenceResolution = new Vector2(initialResolution.x / UIcamera.rect.size.x, initialResolution.y / UIcamera.rect.size.y);

        speedometer.playerNb = playerNb;
        raw.texture = renderTextures[playerNb];
        bubble.sprite = bubbles[ActualSave.actualSave.stats[playerNb].activePlayer];

    }


    public RenderTexture GetCurrentRenderTexture()
    {
        return renderTextures[playerNb];
    }
}
