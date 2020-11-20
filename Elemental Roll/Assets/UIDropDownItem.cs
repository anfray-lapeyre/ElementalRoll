using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDropDownItem : UIButton
{
    public TMPro.TextMeshProUGUI text;
    public int value = 0;
    private UIDropDown father;
    // Start is called before the first frame update
    override protected void Awake()
    {
        text = this.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        base.Awake();
        father = this.GetComponentInParent<UIDropDown>();
    }

    override public void ExecuteFunction()
    {
        father.text.text = this.text.text;
        father.getBackControl(value);
    }

 }
