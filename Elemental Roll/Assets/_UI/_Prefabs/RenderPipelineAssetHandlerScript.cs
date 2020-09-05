using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class RenderPipelineAssetHandlerScript : MonoBehaviour
{
    public RenderPipelineAsset renderPipelineAsset;
    // Start is called before the first frame update
    void Awake()
    {
        if (renderPipelineAsset != null)
        {
            GraphicsSettings.renderPipelineAsset = renderPipelineAsset;
        }
    }

}
