using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gateUIParticleScript : MonoBehaviour
{
    private UIParticleSystem glow;
    private Color color;
    public GameObject gem;
    private Image gemImage;
    private float fixedTime = 12f;

    // Start is called before the first frame update
    void Start()
    {
        glow = this.GetComponent<UIParticleSystem>();
        color = glow.ColorOverLifetime.colorKeys[0].color;
        gemImage = gem.GetComponent<Image>();
        glow.transform.localScale = Vector3.zero;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ActualSave.actualSave.stats[0].Time < fixedTime)
        {
            GradientAlphaKey[] main = glow.ColorOverLifetime.alphaKeys;
            for (int i = 0; i < main.Length; i++)
            {
                if(i!= 0 && i!= main.Length-1)
                 main[i].alpha = Mathf.Min(((fixedTime - Mathf.Min(ActualSave.actualSave.stats[0].Time, fixedTime)) / fixedTime + (fixedTime-2) / 255f), 1f);
            }
            glow.ColorOverLifetime.alphaKeys = main;
            glow.transform.localScale = Vector3.one* (fixedTime - Mathf.Min(ActualSave.actualSave.stats[0].Time, fixedTime)) / (fixedTime);
        }
        gemImage.color = new Color(1f, 1f, 1f, Mathf.Max(Mathf.Min(ActualSave.actualSave.stats[0].Time, 10f) / 10f - (Mathf.Sin(ActualSave.actualSave.stats[0].Time) + 1f)/6f,0f));
    }
}
