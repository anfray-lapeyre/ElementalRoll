using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstanceHandler : MonoBehaviour
{
    public GameObject playerCanvas;
    public GameObject playerContainer;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUIPlacement(int playerNb)
    {
        playerCanvas.GetComponent<UIElementPlacer>().UpdateRatio(playerNb);
    }


    public RenderTexture GetCurrentRenderTexture()
    {
        return playerCanvas.GetComponent<UIElementPlacer>().GetCurrentRenderTexture();
    }

}
