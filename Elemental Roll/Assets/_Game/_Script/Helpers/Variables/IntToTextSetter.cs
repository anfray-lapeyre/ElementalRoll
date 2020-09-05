
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class IntToTextSetter : MonoBehaviour
{
    public TextMeshProUGUI Text;

    public IntVariable Variable;

    public bool AlwaysUpdate;

    private void OnEnable()
    {
        Text.text = (int)Variable.value + "";
    }

    private void Update()
    {
        if (AlwaysUpdate && Variable)
        {
            Text.text = (int)Variable.value + "";
        }
    }
}

