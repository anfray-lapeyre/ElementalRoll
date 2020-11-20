using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDropDown : UIButton
{
    public UIStateMachine stateMachine;
    public string[] options;
    public GameObject template;
    public int maxItemSizeBeforeSlider = 4;

    private UIDropDownItem[] listItems;

    public string currentValue;
    public TMPro.TextMeshProUGUI text;

    public int value=0;
    public RectTransform Content;
    public GameObject Scrollbar;
    // Start is called before the first frame update
    void Start()
    {
        if(value<options.Length && value >=0)
            text.text = options[value];
    }


    override public void ExecuteFunction()
    {
        
        if (isActive)
        {
            if (Content == null)
            {
                Content = this.GetComponentInChildren<UIScrollBarContainer>().GetComponent<RectTransform>();
            }

            this.GetComponentInChildren<Mask>().GetComponent<RectTransform>().sizeDelta = new Vector2(this.GetComponent<RectTransform>().sizeDelta.x, this.GetComponent<RectTransform>().sizeDelta.y * maxItemSizeBeforeSlider);
            this.GetComponentInChildren<Mask>().GetComponent<RectTransform>().anchoredPosition = new Vector2(this.GetComponentInChildren<Mask>().GetComponent<RectTransform>().anchoredPosition.x, - this.GetComponentInChildren<Mask>().GetComponent<RectTransform>().sizeDelta.y/2f - this.GetComponent<RectTransform>().sizeDelta.y);


            Content.sizeDelta = new Vector2(this.GetComponent<RectTransform>().sizeDelta.x, this.GetComponent<RectTransform>().sizeDelta.y * options.Length);
            Content.anchoredPosition = new Vector2(Content.anchoredPosition.x, - Content.sizeDelta.y / 2f );
            listItems = new UIDropDownItem[options.Length];


            GameObject tmp;
            //In this case, we generate the list
            //First we instantiate the options
            for(int i = 0; i < options.Length; i++)
            {
                tmp = Instantiate(template, Content.transform);
                tmp.GetComponent<RectTransform>().anchoredPosition = new Vector2(tmp.GetComponent<RectTransform>().anchoredPosition.x, tmp.GetComponent<RectTransform>().anchoredPosition.y-i*this.GetComponent<RectTransform>().sizeDelta.y);
                tmp.name = options[i];
                tmp.SetActive(true);
                listItems[i] = tmp.GetComponent<UIDropDownItem>();
            }

            //Then we add interactions
            for (int i = 0; i < options.Length; i++)
            {
                if (i > 0) {
                    listItems[i].upButton = listItems[i - 1];
                 }
                if (i < options.Length - 1)
                {
                    listItems[i].downButton = listItems[i + 1];
                }
                listItems[i].text.text = options[i];
                listItems[i].value = i;
            }
            stateMachine.firstSelected = listItems[value];
            listItems[value].changeState(UIButton.SELECTED);
            this.changeState(UIButton.UNSELECTED);

            if (options.Length > maxItemSizeBeforeSlider)
            {
                Scrollbar.GetComponent<UISlider>().leftButton = listItems[0];
                Scrollbar.GetComponent<UISlider>().nbSteps = options.Length + 1 - maxItemSizeBeforeSlider;
                Scrollbar.SetActive(true);
                Scrollbar.GetComponent<RectTransform>().sizeDelta = new Vector2(Scrollbar.GetComponent<RectTransform>().sizeDelta.x, this.GetComponent<RectTransform>().sizeDelta.y * maxItemSizeBeforeSlider);
                Scrollbar.GetComponent<RectTransform>().anchoredPosition = new Vector2(Scrollbar.GetComponent<RectTransform>().anchoredPosition.x, 0);
                Scrollbar.GetComponent<UISlider>().size = maxItemSizeBeforeSlider / (float)options.Length;
                Scrollbar.GetComponent<UISlider>().Refresh();
                
            }
            Scrollbar.GetComponent<UIAutoScrollDropDown>().resetScrollBar();
        }
    }

    public void getBackControl(int _value)
    {
        listItems[_value].changeState(UIButton.UNSELECTED);
        this.changeState(UIButton.SELECTED);
        stateMachine.firstSelected = this;
        value = _value;
        for(int i = 0; i < listItems.Length; i++)
        {
            Destroy(listItems[i].gameObject);
        }
        Scrollbar.SetActive(false);
    }
}
