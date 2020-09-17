using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalClothActivationScript : MonoBehaviour
{
    public Cloth cloth;
    public AudioClip secondSound;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("ActivateCloth", 0.5f);
    }

    public void ActivateCloth()
    {
        cloth.enabled = true;
    }

    public void makeDisappear()
    {
        AudioSource src = this.GetComponent<AudioSource>();
        src.clip = secondSound;
        src.Play();
        this.transform.LeanScale(Vector3.zero, 0.5f);
        cloth.enabled = false;
        Destroy(this.gameObject, 0.5f);
    }
}
