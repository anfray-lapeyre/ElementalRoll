using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gateFloatScript : MonoBehaviour
{
    public  FloatVariable time;
    private float startTime;
    public floatScript[] rocks;
    public Material slimeMaterial;
    public Material rubyGateMaterial;
    public Material portalMaterial;
    private float baseTime;
    private bool isBaseTimeSet = false;
    // Start is called before the first frame update
    void Start()
    {
        // de 0.0005 à  0.0018
        startTime = ((CrossLevelInfo.time<= 0 ) ? 60 : CrossLevelInfo.time);
        slimeMaterial.SetFloat("CrystalIntensity", 1f);
        rubyGateMaterial.SetFloat("CrystalIntensity", 0.1f);
        rubyGateMaterial.SetFloat("crystalHue", 0f);
        portalMaterial.SetFloat("Vector1_4D1B1B62", 1f);

    }

    private void Move()
    {
            

    }

    // Update is called once per frame
    void Update()
    {
        float modifier = 0.1f + time.value / (startTime);
        foreach (floatScript rock in rocks)
        {
            rock.modifier = modifier;
        }

        //we change wobbleIntensity
        slimeMaterial.SetFloat("Vector1_E944F3D4", Mathf.Min(0.0005f + ((startTime - Mathf.Min(100f,time.value))/ startTime)*0.0013f,0.0005f));
        //then the intensity
        if(time.value < startTime / 3 || time.value<=10f)
        {
            if (!isBaseTimeSet)
            {
                isBaseTimeSet = true;
                baseTime = time.value;
            }
            slimeMaterial.SetFloat("CrystalIntensity", Mathf.Min(1f+ ((baseTime - time.value) / (baseTime)) * 2f,1f));
            //And portal intensity Vector1_4D1B1B62

            portalMaterial.SetFloat("Vector1_4D1B1B62", Mathf.Min(1f + ((baseTime - time.value) / (baseTime)) * 2f,1f));
        }

        //0.1 intensity CrystalIntensity
        //0.2 baseColorIntensity baseCrystalColorIntensity
        //0.642 finalIntenisty finalIntensity

        //to 
        //1 intensity
        //10 baseColorIntensity
        //10000 finalIntensity

        if (time.value <= 10f)
        {
            rubyGateMaterial.SetFloat("CrystalIntensity", Mathf.Min(0.1f + (10f-time.value),0.1f));
            rubyGateMaterial.SetFloat("CrystalHue", Mathf.Min(0f + (10f - time.value)*10f,0.2f));
        }

    }
}
