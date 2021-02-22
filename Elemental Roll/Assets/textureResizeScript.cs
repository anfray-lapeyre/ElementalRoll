using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textureResizeScript : MonoBehaviour
{
    void Start()
    {
 
        Mesh myMesh = GetComponent<MeshFilter>().mesh;
        Vector2[] uvs = myMesh.uv;
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i].Scale(new Vector2(transform.localScale.x, transform.localScale.z));
        }

        myMesh.uv = uvs;

    }
}
