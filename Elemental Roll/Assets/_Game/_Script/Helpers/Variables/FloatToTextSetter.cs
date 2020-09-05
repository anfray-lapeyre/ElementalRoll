
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class FloatToTextSetter : MonoBehaviour
{
    public TextMeshProUGUI Text;

    public FloatVariable Variable;

    public bool AlwaysUpdate;
    public bool isTime;
    public bool isInt;

    private void OnEnable()
    {
        if (isTime || isInt)
        {

            Text.text = (int)Variable.value + "";
        }
        else
        {
            Text.text = Variable.value + "";
        }
    }

    private void Update()
    {
        if (AlwaysUpdate && Variable)
        {
            if (isTime)
            {
                string second= (((int)Variable.value) % 999 < 100) ? ((((int)Variable.value) % 999 < 10) ? ("00" + (int)Variable.value % 999) : ("0"+ (int)Variable.value % 999)) : (int)Variable.value % 999 +  "";
                
                Text.text = second ;
            }else if (isInt)
            {
                Text.text = (int)Variable.value + "";
            }
            else
            {
                Text.text = Variable.value + "";
            }
        }
    }
}

