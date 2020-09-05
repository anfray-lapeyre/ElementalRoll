using System.Collections;
using System.Collections.Generic;
using UnityEngine;
  

public class introAnimationScript : MonoBehaviour
{
    public GameObject target;
    // Start is called before the first frame update
    private void Start()
    {
        Invoke("Enter", 2f);
    }

    public void Enter()
    {
        LeanTween.moveLocalX(target, 0f, 0.7f).setEase(LeanTweenType.easeOutBack).setOnComplete(WaitThenUp);

    }

    private void WaitThenUp()
    {
        Invoke("Up", 3f);
    }

    private void Up()
    {
        LeanTween.moveLocalY(target, 1500f, 0.5f).setEase(LeanTweenType.easeInBack);
    }


}
