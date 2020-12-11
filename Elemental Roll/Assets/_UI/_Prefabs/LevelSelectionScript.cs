using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelSelectionScript : MonoBehaviour
{
    public IntVariable currentLevel;
    public UISlider bar;
    public RectTransform scrollRect;
    public UIStateMachine stateMachine;

    public GameObject levelsContainer;
    public GameObject horizontalContainer;
    public GameObject levelBubble;

    private GameObject firstBubble, lastBubble;

    private int size;

    private float initialAnchor;


    public GameObject LevelLoader; // Base
    private LevelLoader _levelLoader; //Instantiated child script

    public GameObject levelPreviewContainer;
    private GameObject actualPreview;
    private LevelFormat actualLevelData;
    private int activeAnimation;

    public UIFader startfading;

    private bool hasBeenInvoked = false;

    private UIButton[][] uibuttons;

    public UIButton backButton;

    public void Awake()
    {

         LeanTween.reset();
         LeanTween.init(800);
        startfading.FadeOut();
        Destroy(startfading.gameObject, 0.6f);
        GameObject levelLoader = Instantiate(LevelLoader);
        _levelLoader = levelLoader.GetComponent<LevelLoader>();
        if (levelsContainer && horizontalContainer && levelBubble)
        {
            string loadedJsonFile = Resources.Load<TextAsset>("levels").text;
            LevelsContainer levelsInJson = JsonUtility.FromJson<LevelsContainer>(loadedJsonFile);

            //We instanciate the matrix that will contain all the buttons
            uibuttons = new UIButton[(Mathf.Min(levelsInJson.levels.Length, ActualSave.actualSave.NextLevel() + 1) / 4)+1][];
            for (int i = 0; i < uibuttons.Length; i++)
            {
                uibuttons[i] = new UIButton[4];
            }

            //FIRST LOOP
            GameObject actualContainer = Instantiate(horizontalContainer, levelsContainer.transform);
            firstBubble = Instantiate(levelBubble, actualContainer.transform);
            firstBubble.GetComponent<levelBubbleHandlerScript>().setData(levelsInJson.levels[0], 1);
            int modifier = ((Mathf.Min(ActualSave.actualSave.NextLevel() + 1, levelsInJson.levels.Length) % 4 > 0) ? 1 : 0);

            //Debug.Log("Resultat du calcul : " + Mathf.Min(ActualSave.actualSave.NextLevel() + 1, levelsInJson.levels.Length) % 4);
            levelsContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(700f, ((Mathf.Min(ActualSave.actualSave.NextLevel()+1, levelsInJson.levels.Length) / 4f + modifier ) * 133f +20f));
            levelsContainer.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ((Mathf.Min(ActualSave.actualSave.NextLevel() + 1, levelsInJson.levels.Length) / 4f + modifier) * 133f +20f));

            //We enter the first element in the matrix
            uibuttons[0][0] = firstBubble.GetComponent<levelBubbleHandlerScript>().getButton();




            //THEN ITERATION



            //This will be the row number
            int j = 0;
            for (int i = 1; i < Mathf.Min(levelsInJson.levels.Length,ActualSave.actualSave.NextLevel()+1); i++)
            {
               
                if (i % 4 == 0)
                {
                    j++;
                    actualContainer = Instantiate(horizontalContainer, levelsContainer.transform);
                }

                if (i == levelsInJson.levels.Length - 1)
                {
                    //The only thing that changes is that we store it in a global variable
                    lastBubble = Instantiate(levelBubble, actualContainer.transform);
                    lastBubble.GetComponent<levelBubbleHandlerScript>().setData(levelsInJson.levels[i], i+1);
                    uibuttons[j][i % 4] = lastBubble.GetComponent<levelBubbleHandlerScript>().getButton();

                }
                else
                {

                    GameObject created = Instantiate(levelBubble, actualContainer.transform);
                    created.GetComponent<levelBubbleHandlerScript>().setData(levelsInJson.levels[i], i+1);
                    uibuttons[j][i % 4] = created.GetComponent<levelBubbleHandlerScript>().getButton();
                }

            }

            //We will now define the navigation of all buttons

            for(j = 0; j < uibuttons.Length; j++)
            {
                for(int i = 0; i < 4; i++)
                {
                    if (uibuttons[j][i])
                    {
                        //Horizontal movement
                        if (i == 0) //Left
                        {
                            if(uibuttons[j][i + 1])
                                uibuttons[j][i].rightButton = uibuttons[j][i + 1];
                            
                        }
                        else if (i == 3) //Right
                        {
                            if (uibuttons[j][i - 1])
                                uibuttons[j][i].leftButton = uibuttons[j][i - 1];
                            if (ActualSave.actualSave.NextLevel() >= 12)
                                uibuttons[j][i].rightButton = bar;
                            else
                                uibuttons[j][i].rightButton = bar.rightButton;
                        }
                        else //Center
                        {
                            if (uibuttons[j][i -1])
                                uibuttons[j][i].leftButton = uibuttons[j][i - 1];
                            if (uibuttons[j][i + 1])
                                uibuttons[j][i].rightButton = uibuttons[j][i + 1];
                        }

                        //We handle the case of the last button
                        if (j * 4 + i == ActualSave.actualSave.NextLevel() || j * 4 + i == ActualSave.actualSave.NextLevel() -1)
                        {
                            if (ActualSave.actualSave.NextLevel() >= 12)
                                uibuttons[j][i].rightButton = bar;
                            else
                                uibuttons[j][i].rightButton = bar.rightButton;
                        }

                        //Vertical movement
                        if (j == 0) //Top
                        {
                            if (j+1 <= uibuttons.Length-1)
                                uibuttons[j][i].downButton = uibuttons[j + 1][i];
                        }
                        else if (j == uibuttons.Length - 1)//Bottom
                        {
                            if (j-1>=0)
                                uibuttons[j][i].upButton = uibuttons[j - 1][i];

                        }
                        else //Center
                        {
                            if (j + 1 <= uibuttons.Length - 1)
                                uibuttons[j][i].downButton = uibuttons[j + 1][i];
                            if (j-1 >= 0)
                                uibuttons[j][i].upButton = uibuttons[j - 1][i];
                        }
                    }
                }
            }

            stateMachine.firstSelected = firstBubble.GetComponent<levelBubbleHandlerScript>().getButton();
            stateMachine.firstSelected.changeState(UIButton.SELECTED);
            //events.firstSelectedGameObject = firstBubble.GetComponent<levelBubbleHandlerScript>().getButton();
            size = ActualSave.actualSave.NextLevel() / 4 -1; /// A REVOIR -----------------------------------------

            initialAnchor = levelsContainer.GetComponent<RectTransform>().anchoredPosition.y;
            if(ActualSave.actualSave.NextLevel() < 12)
            {
                bar.rightButton.leftButton = uibuttons[0][(uibuttons[0][3]) ? 3 : (uibuttons[0][2]) ? 2 : (uibuttons[0][1]) ? 1 : 0];
                bar.isActive = false;
                Destroy(bar.gameObject);
                scrollRect.anchoredPosition = new Vector2(scrollRect.anchoredPosition.x, -scrollRect.sizeDelta.y / 2);
                backButton.leftButton = (uibuttons[0][3]) ? uibuttons[0][2]: (uibuttons[0][2]) ? uibuttons[0][1] : uibuttons[0][0];
            }
            else
            {
                bar.nbSteps = size+1;
                bar.size = (1f / size)*4f;
                bar.value = 0;
                bar.Refresh();
            }

        }
    }
    public void Start()
    {
        
    }


    private void Update()
    {
        if (stateMachine.ID>=0)
        {
            GameObject handler = stateMachine.firstSelected.gameObject;
            RectTransform rect = levelsContainer.GetComponent<RectTransform>();
            if (handler.GetComponent<UIButton>() && handler.GetComponentInParent<levelBubbleHandlerScript>())
            {
                int value = (handler.GetComponentInParent<levelBubbleHandlerScript>().getNb()-1) / 4;
                if(bar != null && bar.isActive && value>=0 && value<=bar.nbSteps)
                {

                    bar.value = Mathf.Max(0,Mathf.Min(value-1,bar.nbSteps));
                    bar.Refresh();
                }

            }

        }
        /*if(Mathf.Abs(scrollRect.verticalNormalizedPosition - bar.value) >= 0.1f)
        {
            bar.value = scrollRect.verticalNormalizedPosition;
        }*/
    }

    public UIButton getButtonForSlider()
    {
        if(uibuttons[Mathf.Max(0, Mathf.Min(uibuttons.Length - 1, bar.value + 1))][3] != null && uibuttons[Mathf.Max(0, Mathf.Min(uibuttons.Length - 1, bar.value + 1))][3].isActive) {
            return uibuttons[Mathf.Max(0, Mathf.Min(uibuttons.Length - 1, bar.value + 1))][3];
        }else if(uibuttons[Mathf.Max(0, Mathf.Min(uibuttons.Length - 1, bar.value + 1))][2] != null && uibuttons[Mathf.Max(0, Mathf.Min(uibuttons.Length - 1, bar.value + 1))][2].isActive)
        {
            return uibuttons[Mathf.Max(0, Mathf.Min(uibuttons.Length - 1, bar.value + 1))][2];
        }
        else if (uibuttons[Mathf.Max(0, Mathf.Min(uibuttons.Length - 1, bar.value + 1))][1] != null && uibuttons[Mathf.Max(0, Mathf.Min(uibuttons.Length - 1, bar.value + 1))][1].isActive)
        {
            return uibuttons[Mathf.Max(0, Mathf.Min(uibuttons.Length - 1, bar.value + 1))][1];
        }
        else if (uibuttons[Mathf.Max(0, Mathf.Min(uibuttons.Length - 1, bar.value + 1))][0] != null && uibuttons[Mathf.Max(0, Mathf.Min(uibuttons.Length - 1, bar.value + 1))][0].isActive)
        {
            return uibuttons[Mathf.Max(0, Mathf.Min(uibuttons.Length - 1, bar.value + 1))][0];
        }
        return uibuttons[Mathf.Max(0, Mathf.Min(uibuttons.Length - 1, bar.value))][3];
    }


    public void getBack()
    {
        _levelLoader.ShowLoader();
        _levelLoader.LoadNextLevel(- 2, false);
    }


    public void LoadLevel(int levelNb)
    {
        _levelLoader.ShowLoader();
        currentLevel.value = -levelNb;
        if(actualPreview)
            Destroy(actualPreview);
        _levelLoader.LoadNextLevel(levelNb-1, false);

    }

    public void HandlePreview()
    {
        if (actualPreview)
        {
            LeanTween.pause(activeAnimation);
            actualPreview.LeanScale(Vector3.zero, 0.1f);
            Destroy(actualPreview,0.2f);
            
        }
        if (stateMachine.ID >= 0)
        {
            hasBeenInvoked = false;
            GameObject selected = stateMachine.firstSelected.gameObject.transform.parent.gameObject;
            actualLevelData = selected.GetComponent<levelBubbleHandlerScript>().getLevelData();
            Object prefab = Resources.Load("Levels/"+ actualLevelData.objName); // Assets/Resources/Prefabs/prefab1.FBX
            actualPreview = (GameObject)Instantiate(prefab, levelPreviewContainer.transform);
            actualPreview.transform.localPosition = Vector3.zero;
            actualPreview.transform.localScale = Vector3.zero;
            Quaternion rotation = new Quaternion();
            rotation.eulerAngles = new Vector3(actualLevelData.previewRotation[0], actualLevelData.previewRotation[1], actualLevelData.previewRotation[2]);
            actualPreview.transform.rotation =rotation;

            Invoke("animatePreview", 0.3f);
        }
    }


    public void animatePreview()
    {
        if (!hasBeenInvoked)
        {
            activeAnimation = actualPreview.LeanScale(Vector3.one * actualLevelData.previewScale, 1f).setEaseOutElastic().id;
            actualPreview.LeanRotateAround(Vector3.up, 2160, 10f).setEaseOutExpo();
            actualPreview.transform.position += new Vector3(actualLevelData.previewPosition[0], actualLevelData.previewPosition[1], 0);
            hasBeenInvoked = true;
        }
    }
}
