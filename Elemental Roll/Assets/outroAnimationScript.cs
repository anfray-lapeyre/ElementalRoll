using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class outroAnimationScript : MonoBehaviour
{

    public float time;
    public int slimesCollected;
    public int totalSlimes;

    public GameObject congratsText;
    public GameObject timeText;
    public GameObject slimesText;

    public TextMeshProUGUI slimeText;
    public TextMeshProUGUI timeText2;
    // Start is called before the first frame update
    private void Start()
    {
        Invoke("Enter", 1f);
    }


    public void Enter()
    {
        slimeText.text = slimesCollected + "/" + totalSlimes;
        timeText2.text = (int)time+"."+(int)((time*100f-((int)time)*100)) + "s";
        LeanTween.moveLocalX(congratsText, 0f, 0.7f).setEase(LeanTweenType.easeOutBack).setOnComplete(EnterTime);

    }
    public void EnterTime()
    {
        
        LeanTween.moveLocalX(timeText, 0f, 0.5f).setEase(LeanTweenType.easeOutBack).setOnComplete(EnterSlime);

    }

    public void EnterSlime()
    {

        LeanTween.moveLocalX(slimesText, -0.3f, 0.3f).setEase(LeanTweenType.easeOutBack).setOnComplete(WaitThenUp);

    }

    private void WaitThenUp()
    {
        Invoke("Up", 4f);
        Invoke("UpTime", 4.5f);
        Invoke("UpSlime", 4.7f);
    }

    private void Up()
    {
        LeanTween.moveLocalY(congratsText, 1500f, 0.5f).setEase(LeanTweenType.easeInBack);
    }

    private void UpTime()
    {
        LeanTween.moveLocalY(timeText, 1500f, 0.5f).setEase(LeanTweenType.easeInBack);
    }

    private void UpSlime()
    {
        LeanTween.moveLocalY(slimesText, 1500f, 0.5f).setEase(LeanTweenType.easeInBack);
    }
}
