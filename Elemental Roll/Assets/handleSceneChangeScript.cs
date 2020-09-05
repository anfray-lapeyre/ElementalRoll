using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class handleSceneChangeScript : MonoBehaviour
{
    public int maxScenes = 13;

    void OnDirection(InputValue value)
    {
        if (value.Get<Vector2>().x != 0)
        {
            
            Invoke("Load", 0.1f); 
        }
    }

    public void Load()
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % maxScenes);
    }
}

