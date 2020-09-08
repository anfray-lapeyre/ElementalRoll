using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelSelectionScript : MonoBehaviour
{
    public IntVariable currentLevel;
    public Scrollbar bar;
    public ScrollRect scrollRect;
    public EventSystem events;

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


            //FIRST LOOP
            GameObject actualContainer = Instantiate(horizontalContainer, levelsContainer.transform);
            firstBubble = Instantiate(levelBubble, actualContainer.transform);
            firstBubble.GetComponent<levelBubbleHandlerScript>().setData(levelsInJson.levels[0], 1);
            int modifier = ((Mathf.Min(ActualSave.actualSave.NextLevel() + 1, levelsInJson.levels.Length) % 4 > 0) ? 1 : 0);

            //Debug.Log("Resultat du calcul : " + Mathf.Min(ActualSave.actualSave.NextLevel() + 1, levelsInJson.levels.Length) % 4);
            levelsContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(700f, ((Mathf.Min(ActualSave.actualSave.NextLevel()+1, levelsInJson.levels.Length) / 4f + modifier ) * 133f +20f));
            levelsContainer.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ((Mathf.Min(ActualSave.actualSave.NextLevel() + 1, levelsInJson.levels.Length) / 4f + modifier) * 133f +20f)); 
            //THEN ITERATION
            for (int i = 1; i < Mathf.Min(levelsInJson.levels.Length,ActualSave.actualSave.NextLevel()+1); i++)
            {
               
                if (i % 4 == 0)
                {
                    actualContainer = Instantiate(horizontalContainer, levelsContainer.transform);
                }

                if (i == levelsInJson.levels.Length - 1)
                {
                    lastBubble = Instantiate(levelBubble, actualContainer.transform);
                    lastBubble.GetComponent<levelBubbleHandlerScript>().setData(levelsInJson.levels[i], i+1);
                }
                else
                {
                    GameObject created = Instantiate(levelBubble, actualContainer.transform);
                    created.GetComponent<levelBubbleHandlerScript>().setData(levelsInJson.levels[i], i+1);
                }

            }
            events.firstSelectedGameObject = firstBubble.GetComponent<levelBubbleHandlerScript>().getButton();
            size = ActualSave.actualSave.NextLevel() / 4 -1; /// A REVOIR -----------------------------------------

            initialAnchor = levelsContainer.GetComponent<RectTransform>().anchoredPosition.y;
            if(ActualSave.actualSave.NextLevel() < 12)
            {
                bar.enabled = false;
                Destroy(bar.gameObject);
            }

        }
    }
    public void Start()
    {
        if (bar.enabled)
            resetScrollBar();
    }

    public IEnumerator resetScrollBar()
    {
        yield return null; // Waiting just one frame is probably good enough, yield return null does that
        bar = GetComponentInChildren<Scrollbar>();
        bar.value = 1;
    }

    private void Update()
    {
        if (events)
        {
            GameObject handler = events.currentSelectedGameObject;
            RectTransform rect = levelsContainer.GetComponent<RectTransform>();
            if (handler.GetComponent<Button>() && handler.GetComponentInParent<levelBubbleHandlerScript>())
            {
                int value = (handler.GetComponentInParent<levelBubbleHandlerScript>().getNb()-1) / 4;
                if(bar != null && bar.enabled && Mathf.Abs((size - value) / (float)size - bar.value) >= 1f / (size+1))
                {
                    bar.value = (size - value) / (float)size;
                }

            }

        }
        /*if(Mathf.Abs(scrollRect.verticalNormalizedPosition - bar.value) >= 0.1f)
        {
            bar.value = scrollRect.verticalNormalizedPosition;
        }*/
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
        if (events)
        {
            GameObject selected = events.currentSelectedGameObject.transform.parent.gameObject;
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
        activeAnimation = actualPreview.LeanScale(Vector3.one * actualLevelData.previewScale, 1f).setEaseOutElastic().id;
        actualPreview.LeanRotateAround(Vector3.up, 2160, 10f).setEaseOutExpo();
        actualPreview.transform.position += new Vector3(actualLevelData.previewPosition[0], actualLevelData.previewPosition[1],0);
    }
}
