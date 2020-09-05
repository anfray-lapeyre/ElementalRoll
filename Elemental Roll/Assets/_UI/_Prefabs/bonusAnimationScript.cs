using System.Collections;
using System.Collections.Generic;
using UnityEngine;
  

public class bonusAnimationScript : MonoBehaviour
{
    public GameObject target;
    // Start is called before the first frame update
    private void Start()
    {
        target.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
        target.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
        target.GetComponent<RectTransform>().anchoredPosition = -target.transform.localPosition;
        LeanTween.move(target.GetComponent<RectTransform>(), new Vector3(55, -50f, 0f), 0.3f).setEase(LeanTweenType.easeOutCubic).setOnComplete(WaitThenUp);
    }

    private void WaitThenUp()
    {
        Invoke("Up", 1f);
        LeanTween.scale(target, Vector3.one, 0.1f).setLoopPingPong(1);
    }

    private void Up()
    {
        //LeanTween.moveLocalY(target, 1500f, 0.5f).setEase(LeanTweenType.easeInBack);
        Destroy(this.gameObject, 0f);
    }


}
