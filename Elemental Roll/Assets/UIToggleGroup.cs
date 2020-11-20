using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToggleGroup : MonoBehaviour
{
    private UIToggle[] toggles;
    private int index = 0;
    private UIToggle selected;

    public void PopulateToggles(UIToggle toggle)
    {
        if (toggle)
        {
            if (toggles == null)
            {
                toggles = new UIToggle[10];
            }
            toggles[index] = toggle;
            index++;
        }
    }
    public void select(UIToggle button)
    {
        if (button)
        {
            //Only one button is selected each time
            if(selected)
                selected.Uncheck();
            selected = button;
        }
    }

    public void SetAllTogglesOff()
    {
        if (toggles == null)
        {
            toggles = this.GetComponentsInChildren<UIToggle>();
        }
        for (int i = 0; i < index; i++)
            {
                toggles[i].Uncheck();
            }
        
    }



}
