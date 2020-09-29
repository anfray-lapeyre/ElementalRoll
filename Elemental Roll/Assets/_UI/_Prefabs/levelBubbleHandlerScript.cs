using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class levelBubbleHandlerScript : MonoBehaviour
{
    public GameObject button;
    public TMP_Text levelName;
    public TMP_Text slimeNumber;
    public TMP_Text collectedSlime;
    public TMP_Text levelNbText;
    private int levelNb;

    public Image backgroundImg;
    public Image smallParticles;
    public Image bigParticles;

    public Color firstColor;
    public Color secondColor;
    public Color thirdColor;
    public Color fourthColor;
    public Color fifthColor;
    public Color sixthColor;

    public Image slimeImageContainer;

    public Sprite missingSlime;
    public Sprite completeSlime;

    private LevelFormat levelData;

    public Sprite firstBackground;
    public Sprite secondBackground;
    public Sprite thirdBackground;
    public Sprite fourthBackground;
    public Sprite fifthBackground;
    public Sprite sixthBackground;


    public GameObject getButton()
    {
        return button;
    }

    public void setData(LevelFormat _levelData, int _levelNb)
    {
        levelData = _levelData;
        levelName.text = levelData.name;
        if (ActualSave.actualSave.levels[_levelNb - 1].beaten)
        {
            if (ActualSave.actualSave.levels[_levelNb - 1].collectedSlime < _levelData.bonusCount)
            {
                collectedSlime.text = ActualSave.actualSave.levels[_levelNb - 1].collectedSlime + "";
                slimeImageContainer.sprite = missingSlime;
            }
            else
            {
                collectedSlime.text = _levelData.bonusCount + "";
                slimeImageContainer.sprite = completeSlime;
            }
            slimeNumber.text = "/" + levelData.bonusCount;

        }
        else
        {
            collectedSlime.text = "?";
            slimeImageContainer.sprite = missingSlime;
            slimeNumber.text = "??";

        }
        levelNb = _levelNb;
        levelNbText.text = "" + _levelNb;

    }

    public int getNb()
    {
        return levelNb;
    }

    public LevelFormat getLevelData()
    {
        return levelData;
    }

    // Start is called before the first frame update
    void Start()
    {

        Color actualColor;
        float value;
        switch ((levelNb-1)/20)
        {
            case 0:
                actualColor = firstColor;
                value = Random.Range(0.6f,1f);
                actualColor.r *= value;
                actualColor.g *= value;
                actualColor.b *= value;
                backgroundImg.sprite = firstBackground;
                backgroundImg.color = actualColor;
                actualColor.a = 0.4f;
                smallParticles.color = actualColor;
                bigParticles.color = actualColor;
                break;
            case 1:
                actualColor = secondColor;
                value = Random.Range(0.5f, 1f);
                actualColor.r *= value;
                actualColor.g *= value;
                actualColor.b *= value;
                smallParticles.color = actualColor;
                bigParticles.color = actualColor;
                backgroundImg.sprite = secondBackground;
                backgroundImg.color = actualColor;
                break;
            case 2:
                actualColor = thirdColor;
                value = Random.Range(0.5f, 1f);
                actualColor.r *= value;
                actualColor.g *= value;
                actualColor.b *= value;
                smallParticles.color = actualColor;
                bigParticles.color = actualColor;
                backgroundImg.sprite = thirdBackground;

                backgroundImg.color = actualColor;
                break;
            case 3:
                actualColor = fourthColor;
                value = Random.Range(0.6f, 1f);
                actualColor.r *= value;
                actualColor.g *= value;
                actualColor.b *= value;
                smallParticles.color = actualColor;
                bigParticles.color = actualColor;
                backgroundImg.sprite = fourthBackground;

                backgroundImg.color = actualColor;
                break;
            case 4:
                actualColor = fifthColor;
                value = Random.Range(0.6f, 1f);
                actualColor.r *= value;
                actualColor.g *= value;
                actualColor.b *= value;
                smallParticles.color = actualColor;
                bigParticles.color = actualColor;
                backgroundImg.sprite = fifthBackground;

                backgroundImg.color = actualColor;
                break;
            case 5:
                actualColor = sixthColor;
                value = Random.Range(0.6f, 1f);
                actualColor.r *= value;
                actualColor.g *= value;
                actualColor.b *= value;
                smallParticles.color = actualColor;
                bigParticles.color = actualColor;
                backgroundImg.sprite = sixthBackground;

                backgroundImg.color = actualColor;
                break;
            default:
                actualColor = sixthColor;
                value = Random.Range(0.6f, 1f);
                actualColor.r *= value;
                actualColor.g *= value;
                actualColor.b *= value;
                smallParticles.color = actualColor;
                bigParticles.color = actualColor;
                backgroundImg.sprite = sixthBackground;

                backgroundImg.color = actualColor;
                break;
        }
        if (!ActualSave.actualSave.levels[levelNb-1].beaten)
        {
            backgroundImg.color = new Color(0.2f,0.2f,0.2f);
            button.GetComponent<Button>().enabled = false;
            button.GetComponent<Image>().enabled = false;
        }
        LeanTween.rotateAround(smallParticles.gameObject, Vector3.forward, Mathf.Sign(Random.Range(-1f,1f))*360, 40 + Random.Range(-3f, 3f)).setLoopClamp();
        LeanTween.rotateAround(bigParticles.gameObject, Vector3.forward, Mathf.Sign(Random.Range(-1f, 1f)) * 360, 50 + Random.Range(-3f, 3f)).setLoopClamp();

        LeanTween.scale(button, Vector3.one * 1.1f, 1f).setEaseInOutQuad().setLoopPingPong();

    }

   
    public void launchLevel()
    {
        this.transform.parent.parent.parent.gameObject.GetComponent<LevelSelectionScript>().LoadLevel(levelNb);
    }

    public void askForHandlePreview()
    {
        this.transform.parent.parent.parent.gameObject.GetComponent<LevelSelectionScript>().HandlePreview();
    }

}
