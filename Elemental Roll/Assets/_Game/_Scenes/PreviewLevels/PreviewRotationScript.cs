using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PreviewRotationScript : MonoBehaviour
{
    public float speed = 0.2f;
    public Vector3 axis = new Vector3(0, 1, 0);

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(axis* speed);

    }


    void OnDirection(InputValue value)
    {
        if (value.Get<Vector2>().x != 0)
        {
            Invoke("Load", 0.1f);
        }
    }

    public void Load()
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % 6);
    }
}
