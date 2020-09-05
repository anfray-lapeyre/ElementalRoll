using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class AutoScrollDropdown : MonoBehaviour
{
    public EventSystem eventSystem;
    private Scrollbar bar;
    private ScrollRect scrollRect;
    public TMP_Dropdown dropdown;
    private int waitforframes = 1;
    private int iteration = 0;
    private int oldSelected=0;
    private float stepSize=0f;
    public void Awake()
    {
        bar = GetComponentInChildren<Scrollbar>();
        scrollRect = GetComponent<ScrollRect>();
    }

    public void Start()
    {

        
    }

    public void resetScrollBar()
    {
       
            bar.value = (dropdown.options.Count - 1f - dropdown.value) / (dropdown.options.Count - 1f);
            oldSelected = dropdown.value;
            stepSize = 1f / (dropdown.options.Count - 1f);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (iteration < waitforframes)
        {
            iteration++;
        }else if(iteration == waitforframes)
        {
            //We wait just the right number of iterations  
            resetScrollBar();
            iteration++;
        }
        else
        {
            //We are here after the first few frames
            if (eventSystem.currentSelectedGameObject.name.Split(' ')[0] == "Item")
            {
                int currentSelected = int.Parse(eventSystem.currentSelectedGameObject.name.Split(' ')[1].Split(':')[0]);
                if (currentSelected != oldSelected)
                {

                    bar.value -= Mathf.Sign(currentSelected - oldSelected) * stepSize;
                    oldSelected = currentSelected;
                }
            }
           
        }
    }
}
